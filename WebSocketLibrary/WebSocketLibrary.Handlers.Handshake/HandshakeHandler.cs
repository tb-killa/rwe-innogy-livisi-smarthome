using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using WebSocketLibrary.Common;
using WebSocketLibrary.Common.Cookies;
using WebSocketLibrary.Socket;

namespace WebSocketLibrary.Handlers.Handshake;

public class HandshakeHandler : IHandshakeHandler
{
	private const int WAITING_SLEEP_MS = 100;

	private const int WAITING_HANSHAKE_TIMEOUT_SECONDS = 30;

	private const int WAITING_TIMES_COUNT = 300;

	private const string END_HANDSHAKE_STRING = "\r\n\r\n";

	private readonly byte[] buffer = new byte[100];

	private readonly byte[] endHandhsakeBytes = Encoding.UTF8.GetBytes("\r\n\r\n");

	private readonly ILogger logger;

	private readonly ISenderSocket sender;

	private readonly IReceiverSocket receiver;

	private readonly IAvailabilitySocket availableSocket;

	private readonly RandomNumberGenerator randomNumberGenerator = new RNGCryptoServiceProvider();

	public HandshakeHandler(IReceiverSocket receiver, ISenderSocket sender, IAvailabilitySocket availableSocket, ILogger logger)
	{
		this.sender = sender;
		this.logger = logger;
		this.receiver = receiver;
		this.availableSocket = availableSocket;
	}

	public void SendHandshake(string url, CookiesCollection cookies)
	{
		Uri uri = new Uri(url);
		string host = uri.Host;
		string absolutePath = uri.AbsolutePath;
		string clientHandshake = GetClientHandshake(host, absolutePath, cookies);
		logger.Info("Send handshake with cookies: {0}", cookies.GetHeaderValue());
		byte[] bytes = Encoding.UTF8.GetBytes(clientHandshake);
		sender.Send(bytes);
	}

	public ReceivedHandshake ReceiveHandshake()
	{
		WaitUntilAvailableBytes();
		string text = ReadHandshakeFromSocket();
		ReceivedHandshake receivedHandshake = ((!string.IsNullOrEmpty(text)) ? HeadersParser.ParseResultHandshake(text) : null);
		if (receivedHandshake != null)
		{
			logger.Info("Received handshake with status:{0}", receivedHandshake.StatusCode);
		}
		return receivedHandshake;
	}

	private void WaitUntilAvailableBytes()
	{
		int num = 0;
		while (!availableSocket.AreAvailableBytes && num < 300)
		{
			Thread.Sleep(100);
			num++;
		}
	}

	private string ReadHandshakeFromSocket()
	{
		string text = null;
		using MemoryStream memoryStream = new MemoryStream();
		bool flag = false;
		while (availableSocket.AreAvailableBytes && !flag)
		{
			ArraySegment<byte> segmentData = new ArraySegment<byte>(buffer, 0, buffer.Length);
			int num = receiver.ReceiveBytes(segmentData, 0u);
			int indexOfEndHandshake = GetIndexOfEndHandshake(buffer, 0, num);
			int count = buffer.Length;
			if (indexOfEndHandshake >= 0)
			{
				int num2 = indexOfEndHandshake + endHandhsakeBytes.Length;
				if (num2 < num)
				{
					receiver.InsertBufferForReceive(buffer, num2, num - num2);
				}
				count = num2;
				flag = true;
			}
			memoryStream.Write(buffer, 0, count);
		}
		text = GetStringUTF8FromStream(memoryStream);
		memoryStream.Close();
		return text;
	}

	private string GetStringUTF8FromStream(MemoryStream mStream)
	{
		string result = null;
		if (mStream.Length > 0)
		{
			mStream.Seek(0L, SeekOrigin.Begin);
			byte[] array = mStream.ToArray();
			result = Encoding.UTF8.GetString(array, 0, array.Length);
		}
		return result;
	}

	private int GetIndexOfEndHandshake(byte[] buffer, int offset, int count)
	{
		int num = -1;
		int num2 = 0;
		for (int i = 0; i < count; i++)
		{
			if (num >= 0)
			{
				break;
			}
			if (endHandhsakeBytes[num2] == buffer[offset + i])
			{
				num2++;
				if (num2 >= endHandhsakeBytes.Length)
				{
					num = offset + i - num2 + 1;
				}
			}
			else
			{
				num2 = 0;
			}
		}
		return num;
	}

	private string GetClientHandshake(string host, string endpoint, CookiesCollection cookies)
	{
		string text = CreateBase64Key();
		StringBuilder stringBuilder = new StringBuilder();
		string value = $"GET {endpoint} HTTP/1.1\r\nUser-Agent: websocket-sharp/1.0\r\nHost: {host}\r\nUpgrade: websocket\r\nConnection: Upgrade\r\nSec-WebSocket-Key: {text}\r\nSec-WebSocket-Version: {13}\r\n";
		stringBuilder.Append(value);
		if (cookies != null && cookies.Count > 0)
		{
			stringBuilder.AppendFormat("Cookie: {0}\r\n", new object[1] { cookies.GetHeaderValue() });
		}
		stringBuilder.Append("\r\n");
		return stringBuilder.ToString();
	}

	private string CreateBase64Key()
	{
		byte[] array = new byte[16];
		randomNumberGenerator.GetBytes(array);
		return Convert.ToBase64String(array);
	}
}
