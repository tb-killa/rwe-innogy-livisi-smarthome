using System;
using System.Net;
using System.Net.Sockets;

namespace Org.Mentalis.Security.Ssl;

public class SecureTcpClient
{
	private bool m_Active;

	private bool m_CleanedUp;

	private SecureSocket m_Client;

	private SecureNetworkStream m_DataStream;

	public LingerOption LingerState
	{
		get
		{
			return (LingerOption)Client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger);
		}
		set
		{
			Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, value);
		}
	}

	public bool NoDelay
	{
		get
		{
			return (int)Client.GetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug) != 0;
		}
		set
		{
			Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, value ? 1 : 0);
		}
	}

	public int ReceiveBufferSize
	{
		get
		{
			return (int)Client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer);
		}
		set
		{
			Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, value);
		}
	}

	public int ReceiveTimeout
	{
		get
		{
			return (int)Client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
		}
		set
		{
			Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, value);
		}
	}

	public int SendBufferSize
	{
		get
		{
			return (int)Client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer);
		}
		set
		{
			Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, value);
		}
	}

	public int SendTimeout
	{
		get
		{
			return (int)Client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout);
		}
		set
		{
			Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, value);
		}
	}

	protected SecureSocket Client
	{
		get
		{
			return m_Client;
		}
		set
		{
			m_Client = value;
		}
	}

	protected bool Active
	{
		get
		{
			return m_Active;
		}
		set
		{
			m_Active = value;
		}
	}

	protected bool CleanedUp
	{
		get
		{
			return m_CleanedUp;
		}
		set
		{
			m_CleanedUp = value;
		}
	}

	protected SecureNetworkStream DataStream
	{
		get
		{
			return m_DataStream;
		}
		set
		{
			m_DataStream = value;
		}
	}

	public SecureTcpClient()
		: this(new SecurityOptions(SecureProtocol.None))
	{
	}

	public SecureTcpClient(IPEndPoint localEP)
		: this(localEP, new SecurityOptions(SecureProtocol.None))
	{
	}

	public SecureTcpClient(string hostname, int port)
		: this(hostname, port, new SecurityOptions(SecureProtocol.None))
	{
	}

	public SecureTcpClient(SecurityOptions options)
	{
		m_Client = new SecureSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, options);
	}

	public SecureTcpClient(IPEndPoint localEP, SecurityOptions options)
		: this(options)
	{
		m_Client.Bind(localEP);
	}

	public SecureTcpClient(string hostname, int port, SecurityOptions options)
		: this(options)
	{
		if (hostname == null)
		{
			throw new ArgumentNullException();
		}
		Connect(hostname, port);
	}

	internal SecureTcpClient(SecureSocket socket)
	{
		m_Client = socket;
		m_Active = true;
	}

	public SecureTcpClient(SecureTcpClient client)
	{
		m_Client = client.Client;
		m_Active = client.Active;
		m_CleanedUp = client.CleanedUp;
		m_DataStream = client.DataStream;
	}

	public virtual void Connect(IPEndPoint remoteEP)
	{
		Client.Connect(remoteEP);
		Active = true;
	}

	public virtual void Connect(IPAddress address, int port)
	{
		if (address == null)
		{
			throw new ArgumentNullException();
		}
		Connect(new IPEndPoint(address, port));
	}

	public virtual void Connect(string hostname, int port)
	{
		if (hostname == null)
		{
			throw new ArgumentNullException();
		}
		Connect(Dns.GetHostEntry(hostname).AddressList[0], port);
	}

	public virtual SecureNetworkStream GetStream()
	{
		if (CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (!Client.Connected)
		{
			throw new InvalidOperationException();
		}
		if (DataStream == null)
		{
			DataStream = new SecureNetworkStream(Client, ownsSocket: false);
		}
		return DataStream;
	}

	public virtual SecureSocket GetSocket()
	{
		return m_Client;
	}

	public void Close()
	{
		Dispose();
	}

	protected virtual void Dispose()
	{
		if (CleanedUp)
		{
			return;
		}
		CleanedUp = true;
		Active = false;
		if (DataStream != null)
		{
			DataStream.Close();
			DataStream = null;
		}
		if (Client.Connected)
		{
			try
			{
				Client.Shutdown(SocketShutdown.Both);
			}
			catch
			{
			}
		}
		Client.Close();
	}
}
