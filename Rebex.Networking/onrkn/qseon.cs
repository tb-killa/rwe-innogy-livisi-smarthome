using System;
using System.Net;
using System.Net.Sockets;
using Rebex.Net;

namespace onrkn;

internal class qseon : ISocket, IDisposable
{
	private readonly SshSession lmidt;

	private SshChannel oxkvs;

	public ISocketFactory xfqnk => lmidt.ToSocketFactory();

	public int Timeout
	{
		get
		{
			return lmidt.Timeout;
		}
		set
		{
		}
	}

	public int ioxja
	{
		get
		{
			int available = oxkvs.Available;
			if (available > 0)
			{
				return available;
			}
			return Math.Max(0, oxkvs.pbbvq(0));
		}
	}

	public bool Connected
	{
		get
		{
			if (oxkvs == null || 1 == 0)
			{
				return false;
			}
			if (ioxja > 0)
			{
				return true;
			}
			return oxkvs.State == SshChannelState.Connected;
		}
	}

	public EndPoint LocalEndPoint => null;

	public EndPoint RemoteEndPoint => null;

	internal qseon(SshSession session)
	{
		lmidt = session;
	}

	internal qseon(SshChannel channel)
	{
		lmidt = channel.xwmfj;
		oxkvs = channel;
	}

	public bool Poll(int microSeconds, SocketSelectMode mode)
	{
		return oxkvs.Poll(microSeconds, mode);
	}

	public void Connect(EndPoint remoteEP)
	{
		if (!(remoteEP is IPEndPoint iPEndPoint) || 1 == 0)
		{
			throw new ArgumentException("Only IPEndPoint is supported at the moment.");
		}
		oxkvs = lmidt.OpenTcpIpTunnel(iPEndPoint.Address.ToString(), iPEndPoint.Port);
	}

	public void Connect(string serverName, int serverPort)
	{
		oxkvs = lmidt.OpenTcpIpTunnel(serverName, serverPort);
	}

	public int Send(byte[] buffer, int offset, int count, SocketFlags socketFlags)
	{
		if (oxkvs == null || 1 == 0)
		{
			throw new InvalidOperationException("Socket is not connected.");
		}
		return oxkvs.Send(buffer, offset, count);
	}

	public int Receive(byte[] buffer, int offset, int count, SocketFlags socketFlags)
	{
		if (oxkvs == null || 1 == 0)
		{
			throw new InvalidOperationException("Socket is not connected.");
		}
		return oxkvs.Receive(buffer, offset, count);
	}

	public void Shutdown(SocketShutdown how)
	{
		if (oxkvs != null && 0 == 0)
		{
			oxkvs.Shutdown();
		}
	}

	public void Close()
	{
		if (oxkvs != null && 0 == 0)
		{
			oxkvs.Close();
		}
	}

	public void Dispose()
	{
		Close();
	}
}
