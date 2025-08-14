using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Rebex;
using Rebex.Net;
using SmartHome.Common.Generic.LogManager;
using WebServerHost.Web.Http;
using WebSocketLibrary;
using WebSocketLibrary.Common;
using WebSocketLibrary.Common.Cookies;

namespace WebServerHost.Web;

public class WebClientConnection : IEqualityComparer<WebClientConnection>, IDisposable
{
	private class ReceiveState
	{
		public const int BufferSize = 40960;

		public byte[] buffer = new byte[40960];

		public readonly Action<WebClientConnection, object> externatlCallback;

		public MemoryStream receivedData = new MemoryStream();

		public ShcWebRequest webRequest;

		public ReceiveState(Action<WebClientConnection, object> externatlCallback)
		{
			this.externatlCallback = externatlCallback;
		}

		public void ReadReceivedBytes(int count)
		{
			if (!receivedData.CanSeek)
			{
				receivedData = new MemoryStream();
			}
			receivedData.Write(buffer, 0, count);
			using StreamReader streamReader = new StreamReader(receivedData);
			if (webRequest == null)
			{
				receivedData.Seek(0L, SeekOrigin.Begin);
				webRequest = new ShcWebRequest(streamReader);
				receivedData.Close();
			}
			else if (receivedData.Length == webRequest.ContentLength)
			{
				receivedData.Seek(0L, SeekOrigin.Begin);
				webRequest.ReadContent(streamReader);
			}
		}
	}

	private class TlsSocketLogger : ILogWriter
	{
		private SmartHome.Common.Generic.LogManager.ILogger logger;

		public Rebex.LogLevel Level { get; set; }

		public TlsSocketLogger(Rebex.LogLevel level, SmartHome.Common.Generic.LogManager.ILogger logger)
		{
			Level = level;
			this.logger = logger;
		}

		public void Write(Rebex.LogLevel level, Type objectType, int objectId, string area, string message, byte[] buffer, int offset, int length)
		{
			string fullMessage = $"TlsSocket [Type={objectType} id={objectId}, area={area}, offset = {offset}, legth={length}]: {message}";
			Log(level, fullMessage);
		}

		private void Log(Rebex.LogLevel level, string fullMessage)
		{
		}

		public void Write(Rebex.LogLevel level, Type objectType, int objectId, string area, string message)
		{
			string fullMessage = $"TlsSocket [Type={objectType} id={objectId}, area={area}]: {message}";
			Log(level, fullMessage);
		}
	}

	private readonly SmartHome.Common.Generic.LogManager.ILogger logger = LogManager.Instance.GetLogger<WebClientConnection>();

	private readonly Socket client;

	private readonly TcpClient tcpClient;

	private readonly TlsSocket tlsSocket;

	private readonly string remoteEndpoint;

	private IWebSocket webSocket;

	private volatile bool isReceiving;

	private volatile bool isBusy;

	private volatile bool faulted;

	private volatile int requestsCount;

	private DateTime lastActivity = DateTime.Now;

	public string RemoteEndPoint => remoteEndpoint;

	public bool isSecured { get; private set; }

	public bool isWebSocket { get; private set; }

	public bool Connected => IsConnected();

	public DateTime LastActivity
	{
		get
		{
			if (isWebSocket || isBusy)
			{
				return DateTime.Now;
			}
			return lastActivity;
		}
	}

	public int RequestsCount => requestsCount;

	public WebClientConnection(TcpClient tcpClient)
	{
		client = tcpClient.Client;
		this.tcpClient = tcpClient;
		remoteEndpoint = client.RemoteEndPoint.ToString();
		isWebSocket = false;
		isSecured = false;
		lastActivity = DateTime.Now;
	}

	public WebClientConnection(TcpClient tcpClient, TlsParameters tlsParams)
	{
		client = tcpClient.Client;
		this.tcpClient = tcpClient;
		remoteEndpoint = client.RemoteEndPoint.ToString();
		tlsSocket = new TlsSocket(client);
		tlsSocket.Parameters = tlsParams;
		tlsSocket.Negotiate();
		isWebSocket = false;
		isSecured = true;
		lastActivity = DateTime.Now;
	}

	public void Dispose()
	{
		try
		{
			if (webSocket != null)
			{
				webSocket.Disconnect();
			}
			if (client != null)
			{
				client.Close();
			}
			if (tlsSocket != null)
			{
				tlsSocket.Close();
			}
			if (tcpClient != null)
			{
				tcpClient.Close();
			}
		}
		catch (Exception exception)
		{
			logger.Error("Client connection disposing error ", exception);
		}
	}

	private bool IsConnected()
	{
		if (isWebSocket)
		{
			if (webSocket.Status != ConnectionStatus.Closed)
			{
				return !faulted;
			}
			return false;
		}
		if (client.Connected)
		{
			return !faulted;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is Socket objB)
		{
			return object.ReferenceEquals(client, objB);
		}
		if (obj is WebClientConnection webClientConnection)
		{
			return object.ReferenceEquals(client, webClientConnection.client);
		}
		if (obj is TcpClient tcpClient)
		{
			return object.ReferenceEquals(client, tcpClient.Client);
		}
		if (obj is TlsSocket { Socket: not null } tlsSocket)
		{
			return object.ReferenceEquals(client, tlsSocket.Socket);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return client.GetHashCode();
	}

	public bool Equals(WebClientConnection x, WebClientConnection y)
	{
		return x.Equals(y);
	}

	public int GetHashCode(WebClientConnection obj)
	{
		return obj.client.GetHashCode();
	}

	public void ReceiveAsync(Action<WebClientConnection, object> callback)
	{
		if (!isReceiving)
		{
			isReceiving = true;
			ReceiveState state = new ReceiveState(callback);
			if (isWebSocket)
			{
				ReceiveWsAsync(callback);
			}
			else
			{
				BeginReceive(state);
			}
		}
	}

	private void BeginReceive(ReceiveState state)
	{
		if (isSecured)
		{
			tlsSocket.BeginReceive(state.buffer, 0, 40960, SocketFlags.None, OnReceive, state);
		}
		else
		{
			client.BeginReceive(state.buffer, 0, 40960, SocketFlags.None, OnReceive, state);
		}
	}

	private void ReceiveWsAsync(Action<WebClientConnection, object> callback)
	{
		new Thread((ThreadStart)delegate
		{
			try
			{
				string arg;
				using (StreamReader streamReader = new StreamReader(new SocketReadStream(webSocket)))
				{
					arg = streamReader.ReadToEnd();
				}
				isBusy = true;
				callback(this, arg);
				isBusy = false;
			}
			catch (Exception exception)
			{
				if (IsConnected())
				{
					logger.Warn($"WS error (remote {client.RemoteEndPoint.ToString()})", exception);
				}
			}
			isReceiving = false;
		}).Start();
	}

	private void OnReceive(IAsyncResult result)
	{
		try
		{
			ReceiveState receiveState = (ReceiveState)result.AsyncState;
			int num = (isSecured ? tlsSocket.EndReceive(result) : client.EndReceive(result));
			if (num > 0)
			{
				lastActivity = DateTime.Now;
				receiveState.ReadReceivedBytes(num);
				int byteCount = Encoding.UTF8.GetByteCount(receiveState.webRequest.RequestContent);
				if (receiveState.webRequest.ContentLength != byteCount)
				{
					BeginReceive(receiveState);
					return;
				}
				receiveState.receivedData.Close();
				isBusy = true;
				receiveState.externatlCallback(this, receiveState.webRequest);
				requestsCount++;
				lastActivity = DateTime.Now;
				isBusy = false;
			}
		}
		catch (Exception)
		{
			faulted = true;
		}
		isReceiving = false;
	}

	public int Send(byte[] buffer, int offest, int size, SocketFlags socketFlags)
	{
		try
		{
			lastActivity = DateTime.Now;
			if (isSecured)
			{
				return tlsSocket.Send(buffer, offest, size, socketFlags);
			}
			return client.Send(buffer, offest, size, socketFlags);
		}
		catch (Exception)
		{
			faulted = true;
			return 0;
		}
	}

	public void SendTextOnWebSocket(string text)
	{
		try
		{
			if (isWebSocket)
			{
				logger.Debug($"Sending text msg on WS to {remoteEndpoint}:{text}");
				webSocket.SendText(text);
			}
		}
		catch (Exception exception)
		{
			logger.Debug($"Failed Sending text msg on WS to {remoteEndpoint}:{text}", exception);
			faulted = true;
		}
	}

	public void UpgradeToWebSocket()
	{
		TlsSocket socket = (isSecured ? tlsSocket : new TlsSocket(client));
		webSocket = new ServerWebSocket(new TlsSocket(socket), new CookiesCollection(), delegate(string debug)
		{
			logger.Debug(debug);
		}, delegate(string error)
		{
			logger.Error(error);
		});
		webSocket.Connect();
		isWebSocket = true;
	}
}
