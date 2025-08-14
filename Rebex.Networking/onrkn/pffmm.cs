using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Rebex;

namespace onrkn;

internal class pffmm : yddoj, lmroh<Socket>, IDisposable
{
	private enum tetum
	{
		jhxlk,
		cmemh,
		oiwrl
	}

	private readonly npgzc lzskz;

	private bbwjd<Socket> lgwgw;

	private readonly wvrwq<tetum> wojvd = new wvrwq<tetum>(tetum.jhxlk);

	public pffmm(EndPoint localEndPoint)
	{
		lzskz = new npgzc(localEndPoint, null, this);
	}

	public void Dispose()
	{
		rwwwq().txebj();
	}

	public exkzi rwwwq()
	{
		return wojvd.wvvao(poomy);
	}

	public njvzu<EndPoint> hqejt()
	{
		return wojvd.jopnb<EndPoint>(erfva);
	}

	public exkzi jikiw()
	{
		return wojvd.wvvao(illhy);
	}

	public IDisposable sypnm(bbwjd<Socket> p0)
	{
		bbwjd<Socket> bbwjd2 = Interlocked.CompareExchange(ref lgwgw, p0, null);
		if (bbwjd2 != null && 0 == 0)
		{
			throw new InvalidOperationException("Only one observer is supported.");
		}
		return new jygpb(hzudk);
	}

	private ObjectDisposedException etwol()
	{
		return new ObjectDisposedException("ChannelObservable");
	}

	private void ywlja(LogLevel p0, string p1, Exception p2)
	{
		bbwjd<Socket> bbwjd2 = Interlocked.CompareExchange(ref lgwgw, null, null);
		if (bbwjd2 != null && 0 == 0)
		{
			bbwjd2.zvcvv(p2);
		}
	}

	void yddoj.ewpna(LogLevel p0, string p1, Exception p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ywlja
		this.ywlja(p0, p1, p2);
	}

	private void zobpj(Socket p0)
	{
		bbwjd<Socket> bbwjd2 = Interlocked.CompareExchange(ref lgwgw, null, null);
		if (bbwjd2 == null || 1 == 0)
		{
			try
			{
				p0.Shutdown(SocketShutdown.Both);
				p0.Close();
				return;
			}
			catch
			{
				return;
			}
		}
		bbwjd2.cvhgi(p0);
	}

	void yddoj.rslch(Socket p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in zobpj
		this.zobpj(p0);
	}

	private exkzi poomy(tetum p0, wvrwq<tetum>.mphde p1)
	{
		if (p0 == tetum.cmemh && lzskz.rzuye && 0 == 0)
		{
			lzskz.cznyx();
		}
		if (p0 != tetum.oiwrl)
		{
			lzskz.Dispose();
		}
		p1(tetum.oiwrl);
		bbwjd<Socket> bbwjd2 = Interlocked.Exchange(ref lgwgw, null);
		if (bbwjd2 != null && 0 == 0)
		{
			bbwjd2.suvgv();
		}
		return rxpjc.iccat;
	}

	private njvzu<EndPoint> erfva(tetum p0, wvrwq<tetum>.mphde p1)
	{
		switch (p0)
		{
		case tetum.oiwrl:
			throw etwol();
		case tetum.cmemh:
			return rxpjc.caxut(lzskz.sccha);
		case tetum.jhxlk:
			lzskz.itzax();
			p1(tetum.cmemh);
			return rxpjc.caxut(lzskz.sccha);
		default:
			throw new NotSupportedException();
		}
	}

	private exkzi illhy(tetum p0, wvrwq<tetum>.mphde p1)
	{
		switch (p0)
		{
		case tetum.cmemh:
			lzskz.cznyx();
			p1(tetum.jhxlk);
			return rxpjc.iccat;
		case tetum.jhxlk:
		case tetum.oiwrl:
			return rxpjc.iccat;
		default:
			throw new NotSupportedException();
		}
	}

	private void hzudk()
	{
		lgwgw = null;
	}
}
