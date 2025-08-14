using System;
using WebSocketLibrary.Common;
using WebSocketLibrary.Exceptions;

namespace WebSocketLibrary.Controllers;

public class ConnectionState
{
	public ConnectionStatus Status { get; private set; }

	public Action<ConnectionStatus, ConnectionSide> OnStatusChangedCallback { get; set; }

	public ConnectionState()
	{
		Status = ConnectionStatus.Undefined;
	}

	public void ConnectionStarted()
	{
		if (Status == ConnectionStatus.Undefined)
		{
			ChangeStatus(ConnectionStatus.Connecting, ConnectionSide.Client);
			return;
		}
		throw new ConnectionFailedException("Cannot set connection for a socket that is already connected or was closed");
	}

	public void ConnectionOpened()
	{
		if (Status == ConnectionStatus.Connecting)
		{
			ChangeStatus(ConnectionStatus.Connected, ConnectionSide.Client);
			return;
		}
		throw new ConnectionFailedException($"Cannot set a connection state directly in connected state, previous state: {Status}");
	}

	public void ServerClosedConnection()
	{
		ChangeStatus(ConnectionStatus.Closing, ConnectionSide.Server);
	}

	public void ServerClosedSocket()
	{
		ChangeStatus(ConnectionStatus.Closed, ConnectionSide.Server);
	}

	public void ClosingConnection()
	{
		if (IsConnectionOpened())
		{
			ChangeStatus(ConnectionStatus.Closing, ConnectionSide.Client);
		}
		else
		{
			CloseConnection();
		}
	}

	public void CloseConnection()
	{
		ChangeStatus(ConnectionStatus.Closed, ConnectionSide.Client);
	}

	public bool IsConnectionOpened()
	{
		return Status == ConnectionStatus.Connected;
	}

	private void ChangeStatus(ConnectionStatus newStatus, ConnectionSide side)
	{
		if (Status != newStatus)
		{
			Status = newStatus;
			OnStatusChangedCallback?.Invoke(Status, side);
		}
	}
}
