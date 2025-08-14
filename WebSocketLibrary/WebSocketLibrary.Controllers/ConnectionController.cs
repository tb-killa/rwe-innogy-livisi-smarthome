using System;
using System.Threading;
using WebSocketLibrary.Common;
using WebSocketLibrary.Common.Cookies;
using WebSocketLibrary.Handlers.Handshake;
using WebSocketLibrary.Managers;
using WebSocketLibrary.Managers.Frames;
using WebSocketLibrary.Socket;

namespace WebSocketLibrary.Controllers;

public class ConnectionController : IDisposable
{
	private static readonly TimeSpan ClosingSocketTimeWait = TimeSpan.FromSeconds(1.0);

	private readonly string url;

	private readonly ILogger logger;

	private readonly IBaseSocket socket;

	private readonly CookiesCollection cookies;

	private readonly IFramesManager framesManager;

	private readonly IHandshakeManager handshakeManager;

	private readonly ConnectionState connectionState;

	private readonly ManualResetEvent waitingCloseEvent = new ManualResetEvent(initialState: false);

	private readonly object sync = new object();

	public event Action OnConnectionClosed;

	public ConnectionController(CookiesCollection cookies, ConnectionState connectionState, IBaseSocket socket, IHandshakeManager handshakeManager, IFramesManager framesManager, ILogger logger)
	{
		this.logger = logger;
		this.socket = socket;
		this.cookies = cookies;
		this.framesManager = framesManager;
		this.connectionState = connectionState;
		this.handshakeManager = handshakeManager;
		this.connectionState.OnStatusChangedCallback = OnStatusChanged;
	}

	public ConnectionController(string url, CookiesCollection cookies, ConnectionState connectionState, IBaseSocket socket, IHandshakeManager handshakeManager, IFramesManager framesManager, ILogger logger)
	{
		this.url = url;
		this.logger = logger;
		this.socket = socket;
		this.cookies = cookies;
		this.framesManager = framesManager;
		this.connectionState = connectionState;
		this.handshakeManager = handshakeManager;
		this.connectionState.OnStatusChangedCallback = OnStatusChanged;
	}

	public void Dispose()
	{
		FinishConnection();
	}

	private void OnStatusChanged(ConnectionStatus status, ConnectionSide side)
	{
		switch (status)
		{
		case ConnectionStatus.Connecting:
			StartConnection();
			break;
		case ConnectionStatus.Connected:
			OpenConnection();
			break;
		case ConnectionStatus.Closing:
			StartCloseConnection(side);
			break;
		case ConnectionStatus.Closed:
			FinishConnection();
			break;
		}
	}

	private void StartConnection()
	{
		try
		{
			if (!socket.IsConnected())
			{
				socket.Connect(url);
				ReceivedHandshake receivedHandshake = handshakeManager.NegotiateHandshake(url, cookies);
				receivedHandshake.Cookies?.ForEach(cookies.AddCookieFromHeader);
			}
			connectionState.ConnectionOpened();
		}
		catch (Exception ex)
		{
			logger.Error("{0}", ex.Message);
			connectionState.CloseConnection();
			throw;
		}
	}

	private void OpenConnection()
	{
		framesManager.Start();
	}

	private void StartCloseConnection(ConnectionSide side)
	{
		framesManager.SendCloseConnection(CloseFrameStatusCode.NormalClosure, string.Empty);
		if (side == ConnectionSide.Client)
		{
			waitingCloseEvent.WaitOne((int)ClosingSocketTimeWait.TotalMilliseconds, exitContext: false);
		}
		connectionState.CloseConnection();
	}

	private void FinishConnection()
	{
		if (waitingCloseEvent.WaitOne(0, exitContext: false))
		{
			return;
		}
		lock (sync)
		{
			if (!waitingCloseEvent.WaitOne(0, exitContext: false))
			{
				try
				{
					waitingCloseEvent.Set();
					InvokeConnectionClosedEvent();
					return;
				}
				finally
				{
					framesManager.Stop();
					socket.Disconnect();
				}
			}
		}
	}

	private void InvokeConnectionClosedEvent()
	{
		this.OnConnectionClosed?.Invoke();
	}
}
