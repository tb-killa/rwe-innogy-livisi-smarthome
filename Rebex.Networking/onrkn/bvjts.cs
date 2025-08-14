using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Rebex.Net;

namespace onrkn;

internal class bvjts : ISocket, phvuu, IDisposable
{
	public const int yuggv = -1;

	private const int ommpx = 8192;

	private const string dgrjl = "ChannelToRebexSocketAdapter - could not reconnect socket.";

	private readonly mggni lzllp;

	private readonly EndPoint lzmam;

	private readonly EndPoint hbjdp;

	private bool fnbvd;

	private njvzu<int> mzuag;

	private nxtme<byte> kylgb;

	private readonly ridny bovlp;

	private int dkoyf;

	public int Timeout
	{
		get
		{
			jjxlv();
			return dkoyf;
		}
		set
		{
			jjxlv();
			dkoyf = ((value <= 0) ? (-1) : value);
		}
	}

	public bool Connected
	{
		get
		{
			jjxlv();
			return true;
		}
	}

	public EndPoint LocalEndPoint
	{
		get
		{
			jjxlv();
			return lzmam;
		}
	}

	public EndPoint RemoteEndPoint
	{
		get
		{
			jjxlv();
			return hbjdp;
		}
	}

	public bvjts(mggni channel, EndPoint localEndPoint, EndPoint remoteEndPoint)
	{
		if (channel == null || 1 == 0)
		{
			throw new ArgumentNullException("channel");
		}
		if (localEndPoint == null || 1 == 0)
		{
			throw new ArgumentNullException("localEndPoint");
		}
		if (remoteEndPoint == null || 1 == 0)
		{
			throw new ArgumentNullException("remoteEndPoint");
		}
		lzllp = channel;
		lzmam = localEndPoint;
		hbjdp = remoteEndPoint;
		bovlp = new ridny();
		dkoyf = -1;
	}

	public void Dispose()
	{
		if (!fnbvd)
		{
			xisau(p0: true);
			fnbvd = true;
		}
	}

	public bool hznqz(int p0)
	{
		return Poll(p0, SocketSelectMode.SelectRead);
	}

	public int Receive(byte[] buffer, int offset, int count)
	{
		jjxlv();
		return Receive(buffer, offset, count, SocketFlags.None);
	}

	public int Send(byte[] buffer, int offset, int count)
	{
		jjxlv();
		return Send(buffer, offset, count, SocketFlags.None);
	}

	public bool Poll(int microSeconds, SocketSelectMode mode)
	{
		jjxlv();
		if ((!kylgb.hvbtp || 1 == 0) && kylgb.frlfs != 0 && 0 == 0)
		{
			return true;
		}
		if (mzuag != null && 0 == 0)
		{
			if (!svdkz(0) || 1 == 0)
			{
				Thread.Sleep(0);
			}
			return svdkz(microSeconds);
		}
		if (bovlp.scnlq && 0 == 0)
		{
			return true;
		}
		if (kylgb.hvbtp && 0 == 0)
		{
			kylgb = new byte[8192].liutv();
		}
		mzuag = lzllp.rhjom(kylgb);
		return svdkz(microSeconds);
	}

	public void Connect(EndPoint remoteEP)
	{
		jjxlv();
		yeify();
	}

	public void Connect(string serverName, int serverPort)
	{
		jjxlv();
		yeify();
	}

	public int Send(byte[] buffer, int offset, int count, SocketFlags socketFlags)
	{
		jjxlv();
		ArraySegment<byte> p = new ArraySegment<byte>(buffer, offset, count);
		njvzu<int> njvzu2 = azojo(lzllp.razzy(p));
		njvzu2.xgngc();
		return njvzu2.islme;
	}

	public int Receive(byte[] buffer, int offset, int count, SocketFlags socketFlags)
	{
		jjxlv();
		nxtme<byte> nxtme2 = buffer.plhfl(offset, count);
		if (nxtme2.hvbtp && 0 == 0)
		{
			return 0;
		}
		if (bovlp.scnlq && 0 == 0)
		{
			return 0;
		}
		if (kylgb.frlfs != 0 && 0 == 0)
		{
			return omodr(nxtme2);
		}
		if (mzuag == null || 1 == 0)
		{
			njvzu<int> njvzu2 = azojo(lzllp.rhjom(nxtme2));
			njvzu2.xgngc();
			return njvzu2.islme;
		}
		njvzu<int> njvzu3 = azojo(mzuag);
		njvzu3.xgngc();
		mzuag = null;
		int islme = njvzu3.islme;
		if (islme == 0 || 1 == 0)
		{
			bovlp.qxocb();
		}
		kylgb = kylgb.jlxhy(0, islme);
		return omodr(nxtme2);
	}

	public void Shutdown(SocketShutdown how)
	{
		jjxlv();
		if ((how != SocketShutdown.Receive) ? true : false)
		{
			lzllp.qxxgh().txebj();
		}
	}

	public void Close()
	{
		Dispose();
	}

	protected void xisau(bool p0)
	{
		if (p0 && 0 == 0)
		{
			lzllp.Dispose();
		}
	}

	private njvzu<int> azojo(njvzu<int> p0)
	{
		if (dkoyf == -1)
		{
			return p0;
		}
		return p0.obrzd(dkoyf);
	}

	private void jjxlv()
	{
		if (fnbvd && 0 == 0)
		{
			throw new ObjectDisposedException(typeof(bvjts).FullName);
		}
	}

	private void yeify()
	{
		throw new InvalidOperationException("ChannelToRebexSocketAdapter - could not reconnect socket.");
	}

	private bool svdkz(int p0)
	{
		if (p0 <= 0)
		{
			return mzuag.IsCompleted;
		}
		double value = (double)p0 * 0.001;
		int p1 = Convert.ToInt32(value);
		mzuag.mtfep(p1);
		return mzuag.IsCompleted;
	}

	private int omodr(nxtme<byte> p0)
	{
		int num = Math.Min(p0.tvoem, kylgb.tvoem);
		kylgb.jlxhy(0, num).rjwrl(p0);
		kylgb = ((num < kylgb.tvoem) ? kylgb.xjycg(num) : kylgb.lthjd.liutv().holjr());
		return num;
	}
}
