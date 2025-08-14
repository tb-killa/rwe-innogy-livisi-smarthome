using System;
using System.Net;
using System.Net.Sockets;
using Rebex;

namespace onrkn;

internal class wuzaj : phvuu
{
	private Socket ijzwo;

	private ILogWriter ozxnn;

	private int ipxpg;

	private int zsswi;

	private int xqvdi;

	public ILogWriter veynb
	{
		get
		{
			return ozxnn;
		}
		internal set
		{
			ozxnn = value;
		}
	}

	public int Timeout
	{
		get
		{
			return ipxpg;
		}
		set
		{
			ipxpg = value;
		}
	}

	public int acagx
	{
		get
		{
			return zsswi;
		}
		set
		{
			zsswi = value;
		}
	}

	public int ljnfy
	{
		get
		{
			return xqvdi;
		}
		set
		{
			xqvdi = value;
		}
	}

	public void Close()
	{
		ijzwo.Close();
	}

	public void ktyok(string p0, int p1)
	{
		IPEndPoint iPEndPoint = auilw.bolwk(p0, p1);
		if (iPEndPoint == null && p0 != null)
		{
			IPHostEntry p2;
			try
			{
				p2 = auilw.qennv(p0);
			}
			catch (Exception ex)
			{
				throw new ksggh("Unable to resolve host name: " + ex.Message, mlaam.pldcn, ex);
			}
			iPEndPoint = auilw.tulbp(p2, p1);
			Socket socket = yxasp(iPEndPoint.AddressFamily);
			try
			{
				socket.Connect(iPEndPoint);
			}
			catch (SocketException e)
			{
				throw new ksggh(e, mlaam.vbeck);
			}
			catch (Exception ex2)
			{
				throw new ksggh(ex2.Message, mlaam.vbeck, ex2);
			}
			ijzwo = socket;
			return;
		}
		throw new ksggh("Unable to resolve host name.", mlaam.pldcn, null);
	}

	public bool hznqz(int p0)
	{
		return ijzwo.Poll(p0, SelectMode.SelectRead);
	}

	public int Receive(byte[] buffer, int offset, int count)
	{
		return ijzwo.Receive(buffer, offset, count, SocketFlags.None);
	}

	public int Send(byte[] buffer, int offset, int count)
	{
		return ijzwo.Send(buffer, offset, count, SocketFlags.None);
	}

	internal Socket yxasp(AddressFamily p0)
	{
		return new Socket(p0, SocketType.Stream, ProtocolType.Tcp);
	}
}
