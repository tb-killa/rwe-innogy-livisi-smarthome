using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Org.Mentalis.Security.Ssl;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.WebSocketsService.Common;
using RWE.SmartHome.SHC.WebSocketsService.Extensions;
using RWE.SmartHome.SHC.WebSocketsService.Util;

namespace RWE.SmartHome.SHC.WebSocketsService.Client;

public class WebSocketSecureClient : IDisposable
{
	protected enum SubState
	{
		Initialized,
		OpenTcpConnection,
		SendHandshake,
		WaitForHandshake,
		ProcessHandshake,
		Redirected,
		Connected,
		ReceiveStart,
		Receiving,
		SendCloseFrame,
		SendCloseResponse,
		WaitForCloseResponse,
		CloseTcpConnection,
		Disconnected,
		Failed
	}

	protected enum ActivityTimerState
	{
		MessageSent,
		WaitingForMessage,
		WaitingForPingResponse
	}

	protected object eventLock;

	protected object sendLock;

	protected bool isDisposed;

	protected bool runThreadLoop;

	protected AutoResetEvent runStateMachine;

	protected string loggerID;

	protected string origin;

	protected string subProtocol;

	protected string extensions;

	protected WebSocketState state;

	protected SubState subState;

	protected WSOptions options;

	protected string securityKey;

	protected bool activityTimerEnabled;

	protected bool sendFramesMasked;

	protected TimerEx activityTimer;

	protected int waitHandshakeTimeout;

	protected int waitCloseMsgTimeout;

	protected int waitReceiveTimeout;

	protected int waitActivityTimeout;

	protected int waitPingRespTimeout;

	protected string optCookies;

	protected string optAcceptEncoding;

	protected string optAcceptLanguage;

	protected string optCacheControl;

	protected string optUserAgent;

	protected string redirectUrl;

	protected UriEx serverUri;

	protected EndPoint serverEndpoint;

	protected SecureSocket socket;

	protected SecureNetworkStream socketStream;

	protected SecureTcpClient secureTcpClient;

	private WSFrame lastRcvdFrame;

	protected ushort closeStatus;

	protected string closeReason;

	protected byte[] receiveBuffer;

	protected int bytesInBuffer;

	protected int posHeaderEOF;

	private WSFrameQueue sendQueue;

	public string LoggerID => loggerID;

	public string Origin => origin;

	public string SubProtocol
	{
		get
		{
			if (State == WebSocketState.Connected)
			{
				return subProtocol;
			}
			return "";
		}
	}

	public string Extensions
	{
		get
		{
			if (State == WebSocketState.Connected)
			{
				return extensions;
			}
			return "";
		}
	}

	public string Version => "13";

	public WebSocketState State => state;

	public event WSDelegates.ConnectionChangedEventHandler ConnectionChanged;

	public event WSDelegates.TextMessageReceivedEventHandler TextMessageReceived;

	public event WSDelegates.DataMessageReceivedEventHandler DataMessageReceived;

	public event WSDelegates.ErrorEventHandler Error;

	public WebSocketSecureClient(string logSourceID, WSOptions options)
	{
		loggerID = logSourceID;
		this.options = options;
		if (this.options == null)
		{
			this.options = new WSOptions();
		}
		origin = this.options.Origin;
		subProtocol = this.options.SubProtocol;
		extensions = this.options.Extensions;
		activityTimerEnabled = this.options.ActivityTimerEnabled;
		sendFramesMasked = this.options.MaskingEnabled;
		waitHandshakeTimeout = this.options.HandshakeTimeout;
		waitCloseMsgTimeout = this.options.CloseMsgTimeout;
		waitReceiveTimeout = this.options.ReceiveTimeout;
		waitActivityTimeout = this.options.ActivityTimeout;
		waitPingRespTimeout = this.options.PingRespTimeout;
		optAcceptEncoding = this.options.AcceptEncoding;
		optAcceptLanguage = this.options.AcceptLanguage;
		optCacheControl = this.options.CacheControl;
		optUserAgent = this.options.UserAgent;
		optCookies = this.options.Cookies;
		activityTimer = new TimerEx();
		state = WebSocketState.Initialized;
		eventLock = new object();
		sendLock = new object();
		isDisposed = false;
		receiveBuffer = new byte[this.options.MaxReceiveFrameLength];
		lastRcvdFrame = null;
		sendQueue = new WSFrameQueue(this.options.MaxSendQueueSize);
		try
		{
			runThreadLoop = true;
			runStateMachine = new AutoResetEvent(initialState: false);
			ThreadPool.QueueUserWorkItem(delegate
			{
				WSStateMachine();
			});
		}
		catch (Exception ex)
		{
			Log.Error(Module.Core, ex.Message + ex.StackTrace);
		}
	}

	~WebSocketSecureClient()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (isDisposed)
		{
			return;
		}
		if (disposing)
		{
			runThreadLoop = false;
			runStateMachine.Set();
			if (sendQueue != null)
			{
				sendQueue.Clear();
			}
			if (activityTimer != null)
			{
				activityTimer.Dispose();
			}
			if (socketStream != null)
			{
				socketStream.Close();
			}
			if (socket != null)
			{
				socket.Close();
			}
		}
		eventLock = null;
		sendLock = null;
		activityTimer = null;
		sendQueue = null;
		socket = null;
		socketStream = null;
		receiveBuffer = null;
		lastRcvdFrame = null;
		serverUri = null;
		loggerID = null;
		origin = null;
		subProtocol = null;
		extensions = null;
		securityKey = null;
		options = null;
		isDisposed = true;
	}

	protected void OnConnectionStateChanged()
	{
		WSDelegates.ConnectionChangedEventHandler connectionChangedEventHandler = null;
		lock (eventLock)
		{
			connectionChangedEventHandler = this.ConnectionChanged;
		}
		connectionChangedEventHandler?.Invoke(state);
	}

	protected void OnTextReceived(string payload)
	{
		WSDelegates.TextMessageReceivedEventHandler textMessageReceivedEventHandler = null;
		lock (eventLock)
		{
			textMessageReceivedEventHandler = this.TextMessageReceived;
		}
		textMessageReceivedEventHandler?.Invoke(payload);
	}

	protected void OnDataReceived(byte[] payload)
	{
		WSDelegates.DataMessageReceivedEventHandler dataMessageReceivedEventHandler = null;
		lock (eventLock)
		{
			dataMessageReceivedEventHandler = this.DataMessageReceived;
		}
		dataMessageReceivedEventHandler?.Invoke(payload);
	}

	protected void OnError(WSErrorCode errorCode, string message, string stackTrace)
	{
		Log.Error(Module.Core, loggerID, "Error: (" + errorCode.ToString() + ") " + message + stackTrace);
		WSDelegates.ErrorEventHandler errorEventHandler = null;
		lock (eventLock)
		{
			errorEventHandler = this.Error;
		}
		errorEventHandler?.Invoke(message, stackTrace);
	}

	public void Connect(string serverUrl)
	{
		Connect(serverUrl, null);
	}

	public void Connect(string serverUrl, int? port)
	{
		if (State != WebSocketState.Initialized && State != WebSocketState.Disconnected)
		{
			return;
		}
		try
		{
			serverUri = new UriEx(serverUrl);
			serverEndpoint = evaluateEndpoint(serverUrl, port);
			if (serverEndpoint == null)
			{
				OnError(WSErrorCode.NativeError, "Host not found: " + serverUri.Host, null);
				return;
			}
			Log.Debug(Module.Core, loggerID, "Connecting to " + serverUrl + " ...");
			state = WebSocketState.Connecting;
			subState = SubState.OpenTcpConnection;
			runStateMachine.Set();
		}
		catch (Exception ex)
		{
			OnError(WSErrorCode.NativeError, ex.Message, ex.StackTrace);
		}
	}

	public void Disconnect()
	{
		if (State == WebSocketState.Connected || State == WebSocketState.Connecting)
		{
			Disconnect(1000, "Bye!");
		}
	}

	public void Disconnect(ushort statusCode, string reason)
	{
		if (state == WebSocketState.Initialized)
		{
			state = WebSocketState.Disconnected;
			subState = SubState.Disconnected;
		}
		else if (state == WebSocketState.Connecting || state == WebSocketState.Connected)
		{
			Log.Debug(Module.Core, loggerID, "Disconnecting...");
			closeStatus = statusCode;
			closeReason = reason;
			state = WebSocketState.Disconnecting;
			subState = SubState.SendCloseFrame;
		}
	}

	public void SendText(string text)
	{
		if (state == WebSocketState.Connected)
		{
			EnqueueMessage(WSFrameType.Text, options.MaskingEnabled, text, highPriority: false);
		}
	}

	public void SendData(byte[] data)
	{
		if (state == WebSocketState.Connected)
		{
			EnqueueMessage(WSFrameType.Binary, options.MaskingEnabled, data, highPriority: false);
		}
	}

	protected void WSStateMachine()
	{
		while (runThreadLoop)
		{
			try
			{
				switch (state)
				{
				case WebSocketState.Initialized:
					runStateMachine.WaitOne();
					break;
				case WebSocketState.Connecting:
					OnConnectionStateChanged();
					smConnect();
					break;
				case WebSocketState.Connected:
					OnConnectionStateChanged();
					smReceive();
					break;
				case WebSocketState.Disconnecting:
					OnConnectionStateChanged();
					smDisconnect();
					break;
				case WebSocketState.Disconnected:
					OnConnectionStateChanged();
					runStateMachine.WaitOne();
					break;
				default:
					throw new NotSupportedException("ReadyState " + State.ToString() + " is invalid.");
				}
			}
			catch (Exception ex)
			{
				if (socketStream != null)
				{
					socketStream.Close();
					socketStream = null;
				}
				if (socket != null)
				{
					socket.Close();
					socket = null;
				}
				OnError(WSErrorCode.NativeError, ex.Message, ex.StackTrace);
				state = WebSocketState.Disconnected;
				subState = SubState.Disconnected;
			}
		}
	}

	protected void smConnect()
	{
		while (state == WebSocketState.Connecting)
		{
			switch (subState)
			{
			case SubState.Redirected:
				DoRedirect();
				break;
			case SubState.OpenTcpConnection:
				DoOpenTcpConnection();
				break;
			case SubState.SendHandshake:
				DoSendHandshake();
				break;
			case SubState.WaitForHandshake:
				DoWaitForHandshake();
				break;
			case SubState.ProcessHandshake:
				DoProcessHandshake();
				break;
			case SubState.Connected:
				DoConnected();
				return;
			case SubState.Failed:
				DoConnectFailed();
				return;
			}
		}
	}

	protected void DoRedirect()
	{
		if (!string.IsNullOrEmpty(redirectUrl))
		{
			string text = redirectUrl.Replace("https://", "wss://");
			redirectUrl = "";
			serverUri = new UriEx(text);
			serverEndpoint = evaluateEndpoint(text, null);
			if (serverEndpoint == null)
			{
				OnError(WSErrorCode.NativeError, "Host not found: " + serverUri.Host, null);
				return;
			}
			secureTcpClient.Close();
			subState = SubState.OpenTcpConnection;
		}
		else
		{
			subState = SubState.Failed;
		}
	}

	protected void DoOpenTcpConnection()
	{
		if (serverUri.Scheme == "wss")
		{
			secureTcpClient = new SecureTcpClient(new SecurityOptions(SecureProtocol.Tls1));
			secureTcpClient.NoDelay = true;
			secureTcpClient.Connect(serverEndpoint as IPEndPoint);
			socketStream = secureTcpClient.GetStream();
			socket = secureTcpClient.GetSocket();
			subState = SubState.SendHandshake;
			return;
		}
		throw new ArgumentException("wrong client or protocol");
	}

	protected void DoSendHandshake()
	{
		securityKey = GetSecurityKey();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("GET " + serverUri.GetPathAndQuery() + " HTTP/1.1" + "\r\n");
		stringBuilder.Append("Host: " + serverUri.Host + "\r\n");
		stringBuilder.Append("Upgrade: websocket" + "\r\n");
		stringBuilder.Append("Connection: Upgrade" + "\r\n");
		if (origin.Length > 0)
		{
			stringBuilder.Append("Origin: " + origin + "\r\n");
		}
		stringBuilder.Append("Sec-WebSocket-Version: " + "13" + "\r\n");
		if (extensions.Length > 0)
		{
			stringBuilder.Append("Sec-WebSocket-Extensions: " + extensions + "\r\n");
		}
		if (subProtocol.Length > 0)
		{
			stringBuilder.Append("Sec-WebSocket-Protocol: " + subProtocol + "\r\n");
		}
		stringBuilder.Append("Sec-WebSocket-Key: " + securityKey + "\r\n");
		if (!string.IsNullOrEmpty(optAcceptEncoding))
		{
			stringBuilder.Append("Accept-Encoding: " + optAcceptEncoding + "\r\n");
		}
		if (!string.IsNullOrEmpty(optAcceptLanguage))
		{
			stringBuilder.Append("Accept-Language: " + optAcceptLanguage + "\r\n");
		}
		if (!string.IsNullOrEmpty(optCacheControl))
		{
			stringBuilder.Append("Cache-Control: " + optCacheControl + "\r\n");
		}
		if (!string.IsNullOrEmpty(optUserAgent))
		{
			stringBuilder.Append("User-Agent: " + optUserAgent + "\r\n");
		}
		if (!string.IsNullOrEmpty(optCookies))
		{
			stringBuilder.Append("Cookie: " + optCookies + "\r\n");
		}
		stringBuilder.Append("\r\n");
		string text = stringBuilder.ToString();
		byte[] array = text.ToByteArray();
		socketStream.Write(array, 0, array.Length);
		Log.Debug(Module.Core, loggerID, "Handshake sent: \n" + text);
		subState = SubState.WaitForHandshake;
	}

	protected void DoWaitForHandshake()
	{
		int num = 0;
		bytesInBuffer = 0;
		posHeaderEOF = 0;
		bytesInBuffer = 0;
		if (socket.Poll(waitHandshakeTimeout, SelectMode.SelectRead) && socket.Available > 0)
		{
			do
			{
				num = socketStream.Read(receiveBuffer, bytesInBuffer, receiveBuffer.Length - bytesInBuffer);
				if (num > 0)
				{
					bytesInBuffer += num;
					posHeaderEOF = receiveBuffer.IndexOf("\r\n\r\n", 0);
					if (posHeaderEOF >= 0)
					{
						LogBufferContent("Handshake received: ", receiveBuffer, 0, bytesInBuffer);
						subState = SubState.ProcessHandshake;
						return;
					}
				}
			}
			while (socket.Available > 0 && bytesInBuffer < receiveBuffer.Length);
		}
		Log.Error(Module.Core, loggerID, "Handshake not received.");
		subState = SubState.Failed;
	}

	protected void DoProcessHandshake()
	{
		ArrayList arrayList = receiveBuffer.Split("\r\n", 0, posHeaderEOF + "\r\n\r\n".Length, toLower: false);
		if (posHeaderEOF >= 0)
		{
			posHeaderEOF += "\r\n\r\n".Length;
			Array.Copy(receiveBuffer, posHeaderEOF, receiveBuffer, 0, bytesInBuffer - posHeaderEOF);
			bytesInBuffer -= posHeaderEOF;
		}
		for (int i = 0; i < arrayList.Count; i++)
		{
			Log.Debug(Module.Core, loggerID, "Handshake header: " + arrayList[i]);
		}
		Regex regex = null;
		string text = "";
		bool flag = false;
		bool flag2 = false;
		int num = 0;
		int num2 = -1;
		bool flag3 = false;
		while (!flag)
		{
			while (true)
			{
				num2++;
				switch (num2)
				{
				case 4:
				case 7:
					break;
				case 0:
					for (num = 0; num < arrayList.Count; num++)
					{
						text = (string)arrayList[num];
						if (!flag3)
						{
							regex = new Regex("http/\\d+\\.\\d+\\s+307", RegexOptions.IgnoreCase);
							if (!regex.IsMatch(text))
							{
								goto end_IL_00d2;
							}
							flag3 = true;
							continue;
						}
						regex = new Regex("Location:\\s(?<redirectUrl>https.*)", RegexOptions.IgnoreCase);
						Match match3 = regex.Match(text);
						if (match3.Success)
						{
							redirectUrl = match3.Groups["redirectUrl"].Value;
							Log.Debug(Module.Core, loggerID, "--->>" + redirectUrl);
							subState = SubState.Redirected;
							return;
						}
					}
					goto end_IL_00c6;
				case 1:
					num = 0;
					while (num < arrayList.Count)
					{
						text = (string)arrayList[num];
						regex = new Regex("http/\\d+\\.\\d+\\s+101", RegexOptions.IgnoreCase);
						if (!regex.IsMatch(text))
						{
							num++;
							continue;
						}
						goto IL_01d2;
					}
					goto end_IL_00c6;
				case 2:
					num = 0;
					while (num < arrayList.Count)
					{
						text = (string)arrayList[num];
						regex = new Regex("upgrade:\\s+websocket", RegexOptions.IgnoreCase);
						if (!regex.IsMatch(text))
						{
							num++;
							continue;
						}
						goto IL_021c;
					}
					goto end_IL_00c6;
				case 3:
					num = 0;
					while (num < arrayList.Count)
					{
						text = (string)arrayList[num];
						regex = new Regex("connection:\\s+upgrade", RegexOptions.IgnoreCase);
						if (!regex.IsMatch(text))
						{
							num++;
							continue;
						}
						goto IL_0266;
					}
					goto end_IL_00c6;
				case 5:
					if (subProtocol.Length == 0)
					{
						break;
					}
					num = 0;
					while (num < arrayList.Count)
					{
						text = (string)arrayList[num];
						regex = new Regex("sec-websocket-protocol:\\s+([A-Za-z0-9!#%'-_@~\\$\\*\\+\\.\\^\\|]+$)", RegexOptions.IgnoreCase);
						Match match2 = regex.Match(text);
						if (!match2.Success || !IsValidProtocolResponse(match2.Groups[1].Value))
						{
							num++;
							continue;
						}
						goto IL_02e3;
					}
					goto end_IL_00c6;
				case 6:
					num = 0;
					while (num < arrayList.Count)
					{
						text = (string)arrayList[num];
						regex = new Regex("sec-websocket-accept:\\s+([A-Za-z0-9+/=]+)", RegexOptions.IgnoreCase);
						Match match = regex.Match(text);
						if (!match.Success || !IsValidSecurityResponse(match.Groups[1].Value))
						{
							num++;
							continue;
						}
						goto IL_034d;
					}
					goto end_IL_00c6;
				case 8:
					flag2 = true;
					goto end_IL_00c6;
				default:
					goto end_IL_00c6;
					IL_01d2:
					arrayList.RemoveAt(num);
					break;
					IL_034d:
					arrayList.RemoveAt(num);
					break;
					IL_021c:
					arrayList.RemoveAt(num);
					break;
					IL_02e3:
					arrayList.RemoveAt(num);
					break;
					IL_0266:
					arrayList.RemoveAt(num);
					break;
					end_IL_00d2:
					break;
				}
				continue;
				end_IL_00c6:
				break;
			}
			flag = true;
		}
		arrayList.Clear();
		arrayList = null;
		if (!flag2)
		{
			Log.Error(Module.Core, loggerID, "Handshake response is invalid");
			subState = SubState.Failed;
		}
		else
		{
			subState = SubState.Connected;
		}
	}

	protected void DoConnected()
	{
		Log.Debug(Module.Core, loggerID, "Connected");
		state = WebSocketState.Connected;
		subState = SubState.ReceiveStart;
	}

	protected void DoConnectFailed()
	{
		if (socketStream != null)
		{
			socketStream.Close();
			socketStream = null;
		}
		if (socket != null)
		{
			socket.Close();
			socket = null;
		}
		state = WebSocketState.Disconnected;
		subState = SubState.Disconnected;
	}

	protected EndPoint evaluateEndpoint(string url, int? port)
	{
		serverUri = new UriEx(url);
		IPHostEntry hostEntry = Dns.GetHostEntry(serverUri.Host);
		IPAddress iPAddress = IPAddress.Any;
		IPAddress[] addressList = hostEntry.AddressList;
		foreach (IPAddress iPAddress2 in addressList)
		{
			if (iPAddress2.GetAddressBytes().Length == 4)
			{
				iPAddress = iPAddress2;
				break;
			}
		}
		if (iPAddress == IPAddress.Any)
		{
			OnError(WSErrorCode.NativeError, "Host not found: " + serverUri.Host, null);
			return null;
		}
		return new IPEndPoint(iPAddress, port ?? serverUri.Port);
	}

	protected void smReceive()
	{
		while (state == WebSocketState.Connected)
		{
			switch (subState)
			{
			case SubState.ReceiveStart:
				DoReceiveStart();
				break;
			case SubState.Receiving:
				DoReceiving();
				break;
			case SubState.Failed:
				DoReceivingFailed();
				break;
			}
		}
	}

	protected void DoReceiveStart()
	{
		lastRcvdFrame = null;
		if (activityTimerEnabled)
		{
			activityTimer.Start(waitActivityTimeout, ActivityTimerState.WaitingForMessage);
		}
		subState = SubState.Receiving;
	}

	protected void DoReceiving()
	{
		if (bytesInBuffer > 0 && WSFrame.TryParse(receiveBuffer, 0, bytesInBuffer, options.MaxReceiveFrameLength, out lastRcvdFrame))
		{
			if (activityTimerEnabled)
			{
				activityTimer.Restart(ActivityTimerState.WaitingForMessage);
			}
			Array.Copy(receiveBuffer, lastRcvdFrame.FrameData.Length, receiveBuffer, 0, bytesInBuffer - lastRcvdFrame.FrameData.Length);
			bytesInBuffer -= lastRcvdFrame.FrameData.Length;
			switch (lastRcvdFrame.OpCode)
			{
			case WSFrameType.Text:
				OnTextReceived(lastRcvdFrame.PayloadText);
				break;
			case WSFrameType.Binary:
				OnDataReceived(lastRcvdFrame.PayloadBytes);
				break;
			case WSFrameType.Close:
				activityTimer.Stop();
				state = WebSocketState.Disconnecting;
				subState = SubState.SendCloseResponse;
				return;
			case WSFrameType.Ping:
				DoSendPongMessage();
				break;
			}
		}
		if (DequeueAndSendMessages() && activityTimerEnabled)
		{
			activityTimer.Restart(ActivityTimerState.MessageSent);
		}
		if (socket.Poll(waitReceiveTimeout, SelectMode.SelectRead) && socket.Available > 0)
		{
			int num = socketStream.Read(receiveBuffer, bytesInBuffer, receiveBuffer.Length - bytesInBuffer);
			bytesInBuffer += num;
			if (num > 0)
			{
				activityTimer.Restart(ActivityTimerState.WaitingForMessage);
			}
		}
		if (activityTimerEnabled && activityTimer.HasTimedOut)
		{
			switch ((ActivityTimerState)activityTimer.State)
			{
			case ActivityTimerState.MessageSent:
			case ActivityTimerState.WaitingForMessage:
				DoSendPingMessage();
				activityTimer.Restart(ActivityTimerState.WaitingForPingResponse);
				break;
			case ActivityTimerState.WaitingForPingResponse:
				Log.Error(Module.Core, loggerID, string.Concat("Ping response timed out."));
				activityTimer.Stop();
				subState = SubState.Failed;
				break;
			}
		}
	}

	protected void DoSendPingMessage()
	{
		WSFrame wSFrame = WSFrame.CreateFrame(WSFrameType.Ping, options.MaskingEnabled, "Hello");
		LogBufferContent("Sending ping frame: ", wSFrame.FrameData, 0, wSFrame.FrameData.Length);
		socketStream.Write(wSFrame.FrameData, 0, wSFrame.FrameData.Length);
	}

	protected void DoSendPongMessage()
	{
		WSFrame wSFrame = WSFrame.CreateFrame(WSFrameType.Pong, options.MaskingEnabled, "Hello");
		LogBufferContent("Sending pong frame: ", wSFrame.FrameData, 0, wSFrame.FrameData.Length);
		socketStream.Write(wSFrame.FrameData, 0, wSFrame.FrameData.Length);
	}

	protected void DoReceivingFailed()
	{
		state = WebSocketState.Disconnecting;
		subState = SubState.CloseTcpConnection;
	}

	protected void smDisconnect()
	{
		while (state == WebSocketState.Disconnecting)
		{
			switch (subState)
			{
			case SubState.SendCloseFrame:
				DoSendCloseFrame();
				break;
			case SubState.SendCloseResponse:
				DoSendCloseResponse();
				break;
			case SubState.WaitForCloseResponse:
				DoWaitCloseResponse();
				break;
			case SubState.CloseTcpConnection:
				DoCloseTcpConnection();
				break;
			case SubState.Disconnected:
				DoDisconnected();
				return;
			case SubState.Failed:
				DoDisconnectFailed();
				return;
			default:
				throw new NotSupportedException("ConnectionState " + subState.ToString() + " is invalid.");
			}
		}
	}

	protected void DoSendCloseFrame()
	{
		WSFrame wSFrame = WSFrame.CreateFrame(WSFrameType.Close, options.MaskingEnabled, ArrayUtil.Concat(closeStatus, closeReason));
		LogBufferContent("Sending close frame: ", wSFrame.FrameData, 0, wSFrame.FrameData.Length);
		socketStream.Write(wSFrame.FrameData, 0, wSFrame.FrameData.Length);
		subState = SubState.WaitForCloseResponse;
	}

	protected void DoSendCloseResponse()
	{
		byte[] payLoad = null;
		if (lastRcvdFrame.OpCode == WSFrameType.Close && lastRcvdFrame.PayloadLength > 0)
		{
			payLoad = lastRcvdFrame.PayloadBytes;
		}
		WSFrame wSFrame = WSFrame.CreateFrame(WSFrameType.Close, options.MaskingEnabled, payLoad);
		LogBufferContent("Sending close frame: ", wSFrame.FrameData, 0, wSFrame.FrameData.Length);
		socketStream.Write(wSFrame.FrameData, 0, wSFrame.FrameData.Length);
		socketStream.Flush();
		subState = SubState.CloseTcpConnection;
	}

	protected void DoWaitCloseResponse()
	{
		int num = 0;
		posHeaderEOF = 0;
		bytesInBuffer = 0;
		if (socket.Poll(waitCloseMsgTimeout, SelectMode.SelectRead) && socket.Available > 0)
		{
			do
			{
				num = socketStream.Read(receiveBuffer, bytesInBuffer, receiveBuffer.Length - bytesInBuffer);
				if (num <= 0)
				{
					continue;
				}
				bytesInBuffer += num;
				if (WSFrame.TryParse(receiveBuffer, 0, bytesInBuffer, options.MaxReceiveFrameLength, out lastRcvdFrame))
				{
					Array.Copy(receiveBuffer, lastRcvdFrame.FrameData.Length, receiveBuffer, 0, bytesInBuffer - lastRcvdFrame.FrameData.Length);
					bytesInBuffer -= lastRcvdFrame.FrameData.Length;
					if (lastRcvdFrame.OpCode == WSFrameType.Close)
					{
						LogBufferContent("Close frame received: ", lastRcvdFrame.FrameData, 0, lastRcvdFrame.FrameData.Length);
						subState = SubState.CloseTcpConnection;
						return;
					}
					LogBufferContent("Data frame received: ", lastRcvdFrame.FrameData, 0, lastRcvdFrame.FrameData.Length);
				}
			}
			while (socket.Available > 0 && bytesInBuffer < receiveBuffer.Length);
		}
		Log.Error(Module.Core, loggerID, "Close frame not received.");
		subState = SubState.Failed;
	}

	protected void DoCloseTcpConnection()
	{
		if (socketStream != null)
		{
			socketStream.Close();
			socketStream = null;
		}
		if (socket != null)
		{
			socket.Close();
			socket = null;
		}
		subState = SubState.Disconnected;
	}

	protected void DoDisconnected()
	{
		state = WebSocketState.Disconnected;
		subState = SubState.Disconnected;
	}

	protected void DoDisconnectFailed()
	{
		if (socketStream != null)
		{
			socketStream.Close();
			socketStream = null;
		}
		if (socket != null)
		{
			socket.Close();
			socket = null;
		}
		state = WebSocketState.Disconnected;
		subState = SubState.Disconnected;
	}

	protected string GetSecurityKey()
	{
		byte[] randomBytes = CryptoUtils.GetRandomBytes(16);
		return ConvertEx.ToBase64String(randomBytes);
	}

	protected bool IsValidExtensionsResponse(string responseExtensionList)
	{
		return false;
	}

	protected bool IsValidProtocolResponse(string responseProtocol)
	{
		responseProtocol = responseProtocol.Trim().ToLower();
		string[] array = subProtocol.Split(',');
		subProtocol = "";
		for (int i = 0; i < array.Length; i++)
		{
			if (responseProtocol == array[i].Trim().ToLower())
			{
				subProtocol = array[i];
				break;
			}
		}
		return subProtocol.Length > 0;
	}

	protected bool IsValidSecurityResponse(string responseSecKey)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(securityKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11");
		string text = ConvertEx.ToBase64String(CryptoUtils.ComputeSha1Hash(bytes));
		Log.Debug(Module.Core, loggerID, "Expected sec key: " + text);
		Log.Debug(Module.Core, loggerID, "Response sec key: " + responseSecKey);
		if (text == responseSecKey)
		{
			return true;
		}
		return false;
	}

	protected void EnqueueMessage(WSFrameType opCode, bool isMasked, string payLoad, bool highPriority)
	{
		WSFrame wsFrame = WSFrame.CreateFrame(opCode, isMasked, payLoad);
		if (highPriority)
		{
			sendQueue.Poke(wsFrame);
		}
		else
		{
			sendQueue.Enqueue(wsFrame);
		}
	}

	protected void EnqueueMessage(WSFrameType opCode, bool isMasked, byte[] payLoad, bool highPriority)
	{
		WSFrame wsFrame = WSFrame.CreateFrame(opCode, isMasked, payLoad);
		if (highPriority)
		{
			sendQueue.Poke(wsFrame);
		}
		else
		{
			sendQueue.Enqueue(wsFrame);
		}
	}

	protected bool DequeueAndSendMessages()
	{
		bool result = sendQueue.Count > 0;
		lock (sendLock)
		{
			while (sendQueue.Count > 0)
			{
				WSFrame wSFrame = sendQueue.Dequeue();
				socketStream.Write(wSFrame.FrameData, 0, wSFrame.FrameData.Length);
			}
			return result;
		}
	}

	protected void LogBufferContent(string prefix, byte[] buffer, int startIndex, int length)
	{
		Log.Debug(Module.Core, loggerID, prefix + buffer.ToHex(startIndex, length));
	}
}
