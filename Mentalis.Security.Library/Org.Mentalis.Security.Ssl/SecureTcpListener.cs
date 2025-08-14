using System;
using System.Net;
using System.Net.Sockets;

namespace Org.Mentalis.Security.Ssl;

public class SecureTcpListener
{
	private EndPoint m_LocalEndpoint;

	private SecurityOptions m_SecurityOptions;

	private SecureSocket m_Server;

	protected bool Active => m_Server != null;

	public EndPoint LocalEndpoint
	{
		get
		{
			if (Server == null)
			{
				return m_LocalEndpoint;
			}
			return Server.LocalEndPoint;
		}
	}

	protected SecureSocket Server => m_Server;

	protected SecurityOptions SecurityOptions => m_SecurityOptions;

	public SecureTcpListener(int port)
		: this(IPAddress.Any, port)
	{
	}

	public SecureTcpListener(int port, SecurityOptions options)
		: this(IPAddress.Any, port, options)
	{
	}

	public SecureTcpListener(IPAddress localaddr, int port)
		: this(new IPEndPoint(localaddr, port))
	{
	}

	public SecureTcpListener(IPAddress localaddr, int port, SecurityOptions options)
		: this(new IPEndPoint(localaddr, port), options)
	{
	}

	public SecureTcpListener(IPEndPoint localEP)
		: this(localEP, new SecurityOptions(SecureProtocol.None, null, ConnectionEnd.Server))
	{
	}

	public SecureTcpListener(IPEndPoint localEP, SecurityOptions options)
	{
		if (localEP == null)
		{
			throw new ArgumentNullException();
		}
		m_LocalEndpoint = localEP;
		m_SecurityOptions = options;
	}

	protected SecureTcpListener(SecureSocket listener, SecurityOptions options)
	{
		if (listener == null)
		{
			throw new ArgumentNullException();
		}
		m_Server = listener;
		m_LocalEndpoint = listener.LocalEndPoint;
		m_SecurityOptions = options;
	}

	public virtual SecureSocket AcceptSocket()
	{
		if (Server == null)
		{
			throw new InvalidOperationException();
		}
		return (SecureSocket)Server.Accept();
	}

	public virtual SecureTcpClient AcceptTcpClient()
	{
		if (Server == null)
		{
			throw new InvalidOperationException();
		}
		return new SecureTcpClient(AcceptSocket());
	}

	public virtual bool Pending()
	{
		if (Server == null)
		{
			throw new InvalidOperationException();
		}
		return Server.Poll(0, SelectMode.SelectRead);
	}

	public virtual void Start()
	{
		if (Server == null)
		{
			EndPoint localEndpoint = LocalEndpoint;
			m_Server = new SecureSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, SecurityOptions);
			Server.Bind(localEndpoint);
			Server.Listen(int.MaxValue);
		}
	}

	public virtual void Stop()
	{
		if (Server != null)
		{
			Server.Close();
			m_Server = null;
		}
	}
}
