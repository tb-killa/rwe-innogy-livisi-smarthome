using System;
using Rebex.Net;
using Rebex.Security.Certificates;
using WebSocketLibrary.Common;
using WebSocketLibrary.Exceptions;

namespace WebSocketLibrary.Socket;

internal class BaseSocket : IBaseSocket, IAvailabilitySocket
{
	private const int DefaultPort = 80;

	private const int SecureDefaultPort = 443;

	private readonly ILogger logger;

	private readonly TlsSocket socket;

	private string url;

	public bool AreAvailableBytes
	{
		get
		{
			if (socket.Available <= 0)
			{
				return socket.Socket.Available > 0;
			}
			return true;
		}
	}

	public int AvaialbleBytes => socket.Available;

	public BaseSocket(TlsSocket tlsSocket, ILogger logger)
	{
		socket = tlsSocket;
		this.logger = logger;
	}

	public BaseSocket(ICertificateRequestHandler certificateRequestHandler, ILogger logger)
	{
		this.logger = logger;
		socket = new TlsSocket();
		socket.Parameters.CertificateRequestHandler = certificateRequestHandler;
		socket.Parameters.AllowedSuites = TlsCipherSuite.Fast;
		socket.Parameters.PreferredHashAlgorithm = SignatureHashAlgorithm.SHA1;
	}

	public bool IsConnected()
	{
		return socket.Connected;
	}

	public void Connect(string url)
	{
		logger.Info("Trying to connect socket");
		this.url = url;
		Uri uri = new Uri(url);
		int num = ((uri.Port != -1) ? uri.Port : GetPortByScheme(uri.Scheme));
		string host = uri.Host;
		_ = uri.AbsolutePath;
		socket.Connect(host, num);
		if (IsSecureProtocol(uri.Scheme))
		{
			socket.Negotiate();
			socket.Timeout = 0;
		}
		logger.Info("Socket connected: {0} {1}", host, num);
	}

	public void Disconnect()
	{
		socket.Close();
		socket.Dispose();
		logger.Info("Socket disconnected");
	}

	public int SendBytes(byte[] data, int offset, int size)
	{
		try
		{
			if (!socket.Connected)
			{
				throw new ConnectionFailedException("Socket was closed.");
			}
			return socket.Send(data, offset, size);
		}
		catch (Exception ex)
		{
			string message = $"BaseSocket -> SendBytes : {ex.Message}";
			throw new ConnectionFailedException(message);
		}
	}

	public int ReceiveBytes(byte[] data, int offset, int size)
	{
		try
		{
			if (!socket.Connected)
			{
				throw new ConnectionFailedException("Socket was closed.");
			}
			return socket.Receive(data, offset, size);
		}
		catch (Exception ex)
		{
			string message = $"BaseSocket -> ReceiveBytes : {ex.Message}";
			throw new ConnectionFailedException(message);
		}
	}

	private int GetPortByScheme(string scheme)
	{
		return scheme switch
		{
			"ws" => 80, 
			"wss" => 443, 
			_ => throw new ArgumentException("Scheme unknown"), 
		};
	}

	private bool IsSecureProtocol(string protocol)
	{
		return protocol == "wss";
	}
}
