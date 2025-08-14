using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Rebex.Net;

namespace onrkn;

internal class xbyeu : mggni, jghfk, cyhjf, gwbla, IDisposable, maowd, vrloh
{
	private sealed class rpyej
	{
		public xbyeu cqtqr;

		public ArraySegment<byte> lmjpb;

		public void ujdsa(AsyncCallback p0)
		{
			cqtqr.cdnyv.BeginReceive(lmjpb.Array, lmjpb.Offset, lmjpb.Count, SocketFlags.None, p0, null);
		}
	}

	private sealed class atikv
	{
		public xbyeu ztfuj;

		public ArraySegment<byte> gmcjq;

		public void vfcfo(AsyncCallback p0)
		{
			ztfuj.cdnyv.BeginSend(gmcjq.Array, gmcjq.Offset, gmcjq.Count, SocketFlags.None, p0, null);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CdisposeAsyncInner_003Ed__a : fgyyk
	{
		public int udpvb;

		public ljmxa tuoir;

		public xbyeu bzeij;

		public Func<exkzi> ykykk;

		private kpthf cvdiy;

		private object ugtxg;

		private void zzhxb()
		{
			try
			{
				bool flag = true;
				switch (udpvb)
				{
				default:
					ykykk = null;
					break;
				case 0:
				case 1:
					break;
				}
				try
				{
					kpthf p2;
					hjkmq<xbyeu> otvhw;
					kpthf p;
					switch (udpvb)
					{
					default:
						p2 = bzeij.wuqja().avdby(p1: false).vrtmi();
						if (!p2.zpafv || 1 == 0)
						{
							udpvb = 0;
							cvdiy = p2;
							tuoir.wqiyk(ref p2, ref this);
							flag = false;
							return;
						}
						goto IL_00a9;
					case 0:
						p2 = cvdiy;
						cvdiy = default(kpthf);
						udpvb = -1;
						goto IL_00a9;
					case 1:
						{
							p = cvdiy;
							cvdiy = default(kpthf);
							udpvb = -1;
							break;
						}
						IL_00a9:
						p2.ekzxl();
						p2 = default(kpthf);
						otvhw = bzeij.otvhw;
						if (ykykk == null || 1 == 0)
						{
							ykykk = bzeij.oerjn;
						}
						p = otvhw.bkutr(ykykk).avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							udpvb = 1;
							cvdiy = p;
							tuoir.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
						break;
					}
					p.ekzxl();
					p = default(kpthf);
				}
				catch (Exception)
				{
				}
			}
			catch (Exception p3)
			{
				udpvb = -2;
				tuoir.iurqb(p3);
				return;
			}
			udpvb = -2;
			tuoir.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in zzhxb
			this.zzhxb();
		}

		private void zwdpd(fgyyk p0)
		{
			tuoir.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in zwdpd
			this.zwdpd(p0);
		}
	}

	private readonly hjkmq<xbyeu> otvhw;

	private bool dedof;

	private Socket cdnyv;

	private apajk<Socket> fhgar;

	private apajk<ISocket> nfeov => apajk<ISocket>.uceou;

	private apajk<Socket> fytyu => fhgar;

	public xbyeu(Socket socket)
	{
		if (socket == null || 1 == 0)
		{
			throw new ArgumentNullException("socket");
		}
		cdnyv = socket;
		fhgar = socket;
		otvhw = new hjkmq<xbyeu>();
		dedof = false;
	}

	public njvzu<int> rhjom(ArraySegment<byte> p0)
	{
		rpyej rpyej = new rpyej();
		rpyej.lmjpb = p0;
		rpyej.cqtqr = this;
		otvhw.iqced();
		int num = otvhw.dvyvd();
		if (num != 1)
		{
			otvhw.ivvbj();
			throw new InvalidOperationException("Another read operation is in progress.");
		}
		return rxpjc.pimhv(rpyej.ujdsa, cdnyv.EndReceive).osdty(tafth);
	}

	public njvzu<int> razzy(ArraySegment<byte> p0)
	{
		atikv atikv = new atikv();
		atikv.gmcjq = p0;
		atikv.ztfuj = this;
		otvhw.iqced();
		otvhw.pshha();
		int num = otvhw.grhvh();
		if (num > 1)
		{
			otvhw.ehxaz();
			throw new InvalidOperationException("Another write operation is in progress.");
		}
		return rxpjc.pimhv(atikv.vfcfo, cdnyv.EndSend).osdty(bqoqi);
	}

	public exkzi qxxgh()
	{
		otvhw.iqced();
		return wuqja();
	}

	public void Dispose()
	{
		jhbpr().txebj();
	}

	public exkzi jhbpr()
	{
		return yateu();
	}

	public override string ToString()
	{
		otvhw.iqced();
		return brgjd.edcru("Channel Info: {0}", otvhw);
	}

	protected virtual exkzi lsdvy()
	{
		try
		{
			cdnyv.Shutdown(SocketShutdown.Send);
		}
		catch (SocketException p)
		{
			int num = p.skehp();
			if (num != 10053 && num != 10054 && num != 10038)
			{
				throw;
			}
		}
		return rxpjc.iccat;
	}

	protected virtual void bldpx(bool p0)
	{
		if (p0 && 0 == 0)
		{
			((IDisposable)cdnyv).Dispose();
			cdnyv = null;
		}
	}

	private exkzi wuqja()
	{
		return otvhw.czebc(lsdvy);
	}

	[vtsnh(typeof(_003CdisposeAsyncInner_003Ed__a))]
	private exkzi yateu()
	{
		_003CdisposeAsyncInner_003Ed__a p = default(_003CdisposeAsyncInner_003Ed__a);
		p.bzeij = this;
		p.tuoir = ljmxa.nmskg();
		p.udpvb = -1;
		ljmxa tuoir = p.tuoir;
		tuoir.nncuo(ref p);
		return p.tuoir.donjp;
	}

	private void rleaf()
	{
		if (!dedof)
		{
			bldpx(p0: true);
			dedof = true;
		}
	}

	private int tafth(njvzu<int> p0)
	{
		otvhw.ivvbj();
		p0.xgngc();
		return p0.islme;
	}

	private int bqoqi(njvzu<int> p0)
	{
		otvhw.ehxaz();
		p0.xgngc();
		return p0.islme;
	}

	private exkzi oerjn()
	{
		rleaf();
		return rxpjc.iccat;
	}
}
