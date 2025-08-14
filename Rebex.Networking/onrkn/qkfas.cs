using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using Rebex.Net;

namespace onrkn;

internal sealed class qkfas : mggni, jghfk, cyhjf, gwbla, IDisposable, maowd, vrloh
{
	private sealed class luujt
	{
		public qkfas tuxki;

		public ArraySegment<byte> kyfxd;

		public int qkqat()
		{
			try
			{
				return tuxki.rwvwc.Receive(kyfxd.Array, kyfxd.Offset, kyfxd.Count, SocketFlags.None);
			}
			catch (SocketException p)
			{
				int num = p.skehp();
				if (num == 10004 || num == 995 || num == 10035)
				{
					return 0;
				}
				throw;
			}
			catch (ObjectDisposedException)
			{
				return 0;
			}
			finally
			{
				tuxki.kiedf.ivvbj();
			}
		}
	}

	private sealed class xapdm
	{
		public qkfas mjuin;

		public ArraySegment<byte> kegeg;

		public int ekgfo()
		{
			try
			{
				return mjuin.rwvwc.Send(kegeg.Array, kegeg.Offset, kegeg.Count, SocketFlags.None);
			}
			finally
			{
				mjuin.kiedf.ehxaz();
			}
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CDisposeAsyncInner_003Ed__8 : fgyyk
	{
		public int igshz;

		public ljmxa gcnsn;

		public qkfas ianrv;

		public Func<exkzi> dcvng;

		private kpthf kaysj;

		private object wqknk;

		private void xkwmn()
		{
			try
			{
				bool flag = true;
				switch (igshz)
				{
				default:
					dcvng = null;
					break;
				case 0:
				case 1:
					break;
				}
				try
				{
					kpthf p2;
					hjkmq<qkfas> kiedf;
					kpthf p;
					switch (igshz)
					{
					default:
						p2 = ianrv.pvofn().avdby(p1: false).vrtmi();
						if (!p2.zpafv || 1 == 0)
						{
							igshz = 0;
							kaysj = p2;
							gcnsn.wqiyk(ref p2, ref this);
							flag = false;
							return;
						}
						goto IL_00a9;
					case 0:
						p2 = kaysj;
						kaysj = default(kpthf);
						igshz = -1;
						goto IL_00a9;
					case 1:
						{
							p = kaysj;
							kaysj = default(kpthf);
							igshz = -1;
							break;
						}
						IL_00a9:
						p2.ekzxl();
						p2 = default(kpthf);
						kiedf = ianrv.kiedf;
						if (dcvng == null || 1 == 0)
						{
							dcvng = ianrv.qgacb;
						}
						p = kiedf.bkutr(dcvng).avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							igshz = 1;
							kaysj = p;
							gcnsn.wqiyk(ref p, ref this);
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
				igshz = -2;
				gcnsn.iurqb(p3);
				return;
			}
			igshz = -2;
			gcnsn.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in xkwmn
			this.xkwmn();
		}

		private void fthnc(fgyyk p0)
		{
			gcnsn.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in fthnc
			this.fthnc(p0);
		}
	}

	private const int lqpet = 10004;

	private const int tgwdi = 995;

	private const int ampuq = 10035;

	private readonly hjkmq<qkfas> kiedf;

	private ISocket rwvwc;

	public apajk<ISocket> axhuh => vvchs.zyyfq(rwvwc);

	public apajk<Socket> wbidm => apajk<Socket>.uceou;

	public qkfas(ISocket socket)
	{
		if (socket == null || 1 == 0)
		{
			throw new ArgumentNullException("socket");
		}
		rwvwc = socket;
		rwvwc.Timeout = -1;
		kiedf = new hjkmq<qkfas>();
	}

	public void Dispose()
	{
		jhbpr().pearr().txebj();
	}

	public exkzi jhbpr()
	{
		return davek();
	}

	public njvzu<int> rhjom(ArraySegment<byte> p0)
	{
		luujt luujt = new luujt();
		luujt.kyfxd = p0;
		luujt.tuxki = this;
		kiedf.iqced();
		kiedf.dvyvd();
		return rxpjc.jgkuq(luujt.qkqat);
	}

	public njvzu<int> razzy(ArraySegment<byte> p0)
	{
		xapdm xapdm = new xapdm();
		xapdm.kegeg = p0;
		xapdm.mjuin = this;
		kiedf.iqced();
		kiedf.pshha();
		kiedf.grhvh();
		return rxpjc.jgkuq(xapdm.ekgfo);
	}

	public exkzi qxxgh()
	{
		kiedf.iqced();
		return pvofn();
	}

	public override string ToString()
	{
		kiedf.iqced();
		return brgjd.edcru("Channel Info: {0}", kiedf);
	}

	[vtsnh(typeof(_003CDisposeAsyncInner_003Ed__8))]
	private exkzi davek()
	{
		_003CDisposeAsyncInner_003Ed__8 p = default(_003CDisposeAsyncInner_003Ed__8);
		p.ianrv = this;
		p.gcnsn = ljmxa.nmskg();
		p.igshz = -1;
		ljmxa gcnsn = p.gcnsn;
		gcnsn.nncuo(ref p);
		return p.gcnsn.donjp;
	}

	private exkzi pvofn()
	{
		return kiedf.czebc(fhmta);
	}

	private exkzi qgacb()
	{
		ISocket socket = Interlocked.Exchange(ref rwvwc, null);
		if (socket == null || 1 == 0)
		{
			return rxpjc.iccat;
		}
		if (socket is IDisposable disposable && 0 == 0)
		{
			disposable.Dispose();
		}
		else
		{
			socket.Close();
		}
		return rxpjc.iccat;
	}

	private exkzi fhmta()
	{
		rwvwc.Shutdown(SocketShutdown.Send);
		return rxpjc.iccat;
	}
}
