using System;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.SHCRelayDriver.Exceptions;
using WebSocketLibrary;
using WebSocketLibrary.Common;
using WebSocketLibrary.Common.Cookies;
using WebSocketLibrary.Managers.Frames;

namespace RWE.SmartHome.SHC.SHCRelayDriver;

public class WebSocketManager
{
	private readonly string relayServiceAddress;

	private readonly ICertificateManager certificateManager;

	private readonly Action connectedSuccsessfulCallback;

	private readonly object sync = new object();

	private readonly CookiesCollection cookies = new CookiesCollection();

	private WebSocket wsClient;

	public WebSocketManager(string relayServiceAddress, ICertificateManager certificateManager, Action connectedSuccsessfulCallback)
	{
		this.relayServiceAddress = relayServiceAddress;
		this.certificateManager = certificateManager;
		this.connectedSuccsessfulCallback = connectedSuccsessfulCallback;
	}

	public void Send(string message)
	{
		try
		{
			EnsureSocketExists();
			wsClient.SendText(message);
		}
		catch (Exception ex)
		{
			Log.Error(Module.RelayDriver, $"Exception WebSocketManager.Send: {ex.Message}");
			CloseSocket();
			throw new WebSocketException(ex.Message);
		}
	}

	public ReceivedResult ReceiveData(ArraySegment<byte> data)
	{
		try
		{
			EnsureSocketExists();
			return wsClient.ReceiveData(data);
		}
		catch (Exception ex)
		{
			Log.Error(Module.RelayDriver, $"Exception WebSocketManager.Read: {ex.Message}");
			CloseSocket();
			throw new WebSocketException(ex.Message);
		}
	}

	public void CloseSocket()
	{
		lock (sync)
		{
			if (wsClient != null)
			{
				try
				{
					wsClient.CloseConnection();
				}
				catch
				{
					wsClient.Disconnect();
				}
			}
			wsClient = null;
		}
	}

	private void EnsureSocketExists()
	{
		lock (sync)
		{
			if (!IsSocketCreated())
			{
				RecreateSocket();
			}
			if (!IsSocketConnected())
			{
				ConnectSocket();
			}
		}
	}

	private bool IsSocketConnected()
	{
		if (wsClient == null || wsClient.Status != ConnectionStatus.Connecting)
		{
			return wsClient.Status == ConnectionStatus.Connected;
		}
		return true;
	}

	private void ConnectSocket()
	{
		wsClient.Connect();
		ChangedConnectionSuccessful();
	}

	private bool IsSocketCreated()
	{
		return wsClient != null;
	}

	private void RecreateSocket()
	{
		if (wsClient != null)
		{
			try
			{
				wsClient.CloseConnection();
				wsClient.Disconnect();
			}
			catch
			{
			}
		}
		wsClient = new WebSocket(relayServiceAddress, new CertificateResolver(certificateManager), cookies, DebugLog, ErrorLog);
	}

	private void ChangedConnectionSuccessful()
	{
		connectedSuccsessfulCallback?.Invoke();
	}

	private void DebugLog(string message)
	{
		Log.Debug(Module.RelayDriver, message);
	}

	private void ErrorLog(string message)
	{
		Log.Error(Module.RelayDriver, message);
	}
}
