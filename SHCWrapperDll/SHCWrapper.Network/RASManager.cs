using System;
using System.Threading;

namespace SHCWrapper.Network;

public class RASManager
{
	public enum RAS_EVENT
	{
		RAS_CONNECT,
		RAS_CONNECT_ERROR,
		RAS_DISCONNECT
	}

	public enum RAS_STATE
	{
		RAS_CONNECTED,
		RAS_DISCONNECTED
	}

	public delegate void RasEvent(RAS_EVENT ras_event);

	private RAS_STATE _CurrentState = RAS_STATE.RAS_DISCONNECTED;

	private string _ConnectionName;

	private string _Login;

	private string _Password;

	private AutoResetEvent _DisconnectEvent;

	public RAS_STATE CurrentState => _CurrentState;

	public event RasEvent OnRasStatusChanged;

	public RASManager(string connection_name)
	{
		_ConnectionName = connection_name;
		_DisconnectEvent = new AutoResetEvent(initialState: false);
	}

	private void ThreadRasConnnection()
	{
		bool flag = false;
		try
		{
			if (PrivateWrapper.RasConnect(_ConnectionName, _Login, _Password))
			{
				_CurrentState = RAS_STATE.RAS_CONNECTED;
				if (this.OnRasStatusChanged != null)
				{
					this.OnRasStatusChanged(RAS_EVENT.RAS_CONNECT);
				}
				while (!_DisconnectEvent.WaitOne(1000, exitContext: false))
				{
					if (!PrivateWrapper.RasIsConnected(_ConnectionName))
					{
						_CurrentState = RAS_STATE.RAS_DISCONNECTED;
						if (this.OnRasStatusChanged != null)
						{
							this.OnRasStatusChanged(RAS_EVENT.RAS_DISCONNECT);
						}
						break;
					}
				}
			}
			else
			{
				flag = true;
			}
			PrivateWrapper.RasDisconnect(_ConnectionName);
			_CurrentState = RAS_STATE.RAS_DISCONNECTED;
			if (this.OnRasStatusChanged != null)
			{
				if (flag)
				{
					this.OnRasStatusChanged(RAS_EVENT.RAS_CONNECT_ERROR);
				}
				else
				{
					this.OnRasStatusChanged(RAS_EVENT.RAS_DISCONNECT);
				}
			}
		}
		catch
		{
		}
	}

	public void Connect(string login, string password)
	{
		if (login == null || password == null)
		{
			throw new Exception("Null login or password are not supported");
		}
		_Login = login;
		_Password = password;
		Thread thread = new Thread(ThreadRasConnnection);
		thread.Start();
	}

	public void Disconnect()
	{
		PrivateWrapper.RasDisconnect(_ConnectionName);
		_DisconnectEvent.Set();
	}
}
