using System;
using System.Net;
using System.Net.Sockets;

namespace onrkn;

internal class dywyv
{
	private readonly Socket kieqd;

	private bool xkwyq;

	private Action<SocketException, int> xayxy;

	public EndPoint gvgsi => kieqd.LocalEndPoint;

	public EndPoint ralwf => kieqd.RemoteEndPoint;

	public dywyv(Socket socket)
	{
		kieqd = socket;
	}

	public void xpmtx(byte[] p0, int p1, int p2)
	{
		kieqd.Send(p0, p1, p2, SocketFlags.None);
	}

	private void hreha(SocketException p0, int p1)
	{
		Action<SocketException, int> action = xayxy;
		xayxy = null;
		xkwyq = false;
		action(p0, p1);
	}

	private void acinm(IAsyncResult p0)
	{
		int p1;
		SocketException p2;
		try
		{
			p1 = kieqd.EndReceive(p0);
			p2 = null;
		}
		catch (ObjectDisposedException)
		{
			p1 = 0;
			p2 = new SocketException(10058);
		}
		catch (SocketException ex2)
		{
			p1 = 0;
			p2 = ex2;
		}
		catch (Exception)
		{
			p1 = 0;
			p2 = new SocketException(-1);
		}
		hreha(p2, p1);
	}

	public void piyap(byte[] p0, int p1, int p2, Action<SocketException, int> p3)
	{
		SocketException ex;
		lock (kieqd)
		{
			if (xkwyq && 0 == 0)
			{
				throw new InvalidOperationException("Receive operation is already pending.");
			}
			xkwyq = true;
			xayxy = p3;
			try
			{
				kieqd.BeginReceive(p0, p1, p2, SocketFlags.None, acinm, null);
				ex = null;
			}
			catch (ObjectDisposedException)
			{
				ex = new SocketException(10058);
			}
			catch (SocketException ex3)
			{
				ex = ex3;
			}
			catch (Exception)
			{
				ex = new SocketException(-1);
			}
		}
		if (ex != null && 0 == 0)
		{
			hreha(ex, 0);
		}
	}

	public void zwvkn(SocketShutdown p0)
	{
		kieqd.Shutdown(p0);
	}

	public void wleqy()
	{
		kieqd.Close();
	}
}
