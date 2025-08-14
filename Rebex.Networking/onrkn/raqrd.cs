using System.Net.Sockets;
using Rebex.Net;

namespace onrkn;

internal class raqrd : phvuu
{
	private readonly ISocket khkzp;

	public int Timeout
	{
		get
		{
			return khkzp.Timeout;
		}
		set
		{
			khkzp.Timeout = value;
		}
	}

	public raqrd(ISocket socket)
	{
		khkzp = socket;
	}

	public void Close()
	{
		khkzp.Close();
	}

	public bool hznqz(int p0)
	{
		return khkzp.Poll(p0, SocketSelectMode.SelectRead);
	}

	public int Receive(byte[] buffer, int offset, int count)
	{
		return khkzp.Receive(buffer, offset, count, SocketFlags.None);
	}

	public int Send(byte[] buffer, int offset, int count)
	{
		return khkzp.Send(buffer, offset, count, SocketFlags.None);
	}

	public ISocket muiaz()
	{
		return khkzp;
	}
}
