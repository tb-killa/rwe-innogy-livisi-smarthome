using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal abstract class ortbm
{
	[StructLayout(LayoutKind.Auto)]
	private struct _003CStartNegotiating_003Ed__0 : fgyyk
	{
		public int sgnjs;

		public vxvbw<bool> bdcik;

		public ortbm iqjrc;

		public IDisposable vpzrx;

		private xuwyj<IDisposable> lwvmc;

		private object romtn;

		private void qmpsb()
		{
			bool p2;
			try
			{
				bool flag = true;
				xuwyj<IDisposable> p;
				if (sgnjs != 0)
				{
					p = iqjrc.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						sgnjs = 0;
						lwvmc = p;
						bdcik.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = lwvmc;
					lwvmc = default(xuwyj<IDisposable>);
					sgnjs = -1;
				}
				IDisposable disposable = p.gbccf();
				p = default(xuwyj<IDisposable>);
				IDisposable disposable2 = disposable;
				vpzrx = disposable2;
				try
				{
					if (iqjrc.lptja != hlkgm.iucmn)
					{
						iqjrc.lptja = hlkgm.iucmn;
						goto end_IL_00a7;
					}
					p2 = false;
					goto end_IL_0000;
					end_IL_00a7:;
				}
				finally
				{
					if (flag && 0 == 0 && vpzrx != null && 0 == 0)
					{
						vpzrx.Dispose();
					}
				}
				p2 = true;
				end_IL_0000:;
			}
			catch (Exception p3)
			{
				sgnjs = -2;
				bdcik.tudwl(p3);
				return;
			}
			sgnjs = -2;
			bdcik.vzyck(p2);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in qmpsb
			this.qmpsb();
		}

		private void xiqrn(fgyyk p0)
		{
			bdcik.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in xiqrn
			this.xiqrn(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CisConnected_003Ed__4 : fgyyk
	{
		public int hymhk;

		public vxvbw<bool> hzkqp;

		public ortbm bufnd;

		public IDisposable yiaus;

		private xuwyj<IDisposable> knaxj;

		private object bjlmi;

		private void ukjkn()
		{
			bool p2;
			try
			{
				bool flag = true;
				xuwyj<IDisposable> p;
				if (hymhk != 0)
				{
					p = bufnd.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						hymhk = 0;
						knaxj = p;
						hzkqp.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = knaxj;
					knaxj = default(xuwyj<IDisposable>);
					hymhk = -1;
				}
				IDisposable disposable = p.gbccf();
				p = default(xuwyj<IDisposable>);
				IDisposable disposable2 = disposable;
				yiaus = disposable2;
				try
				{
					p2 = bufnd.lptja != hlkgm.tqqib || bufnd.quhdu.Length > 0;
				}
				finally
				{
					if (flag && 0 == 0 && yiaus != null && 0 == 0)
					{
						yiaus.Dispose();
					}
				}
			}
			catch (Exception p3)
			{
				hymhk = -2;
				hzkqp.tudwl(p3);
				return;
			}
			hymhk = -2;
			hzkqp.vzyck(p2);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ukjkn
			this.ukjkn();
		}

		private void pkjfu(fgyyk p0)
		{
			hzkqp.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in pkjfu
			this.pkjfu(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CProcessSsl2Handshake_003Ed__8 : fgyyk
	{
		public int jqwmz;

		public ljmxa mackw;

		public ortbm glkat;

		public byte[] xlxeu;

		public int wzzyr;

		public int nbcco;

		private kpthf pauga;

		private object rggft;

		private void szofg()
		{
			try
			{
				bool flag = true;
				kpthf p;
				if (jqwmz != 0)
				{
					p = glkat.tmymn(xlxeu, wzzyr, nbcco).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						jqwmz = 0;
						pauga = p;
						mackw.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = pauga;
					pauga = default(kpthf);
					jqwmz = -1;
				}
				p.ekzxl();
				p = default(kpthf);
			}
			catch (Exception p2)
			{
				jqwmz = -2;
				mackw.iurqb(p2);
				return;
			}
			jqwmz = -2;
			mackw.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in szofg
			this.szofg();
		}

		private void docia(fgyyk p0)
		{
			mackw.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in docia
			this.docia(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CProcessHandshakeAsync_003Ed__b : fgyyk
	{
		public int jtgpl;

		public ljmxa vcrih;

		public ortbm wxngw;

		public byte[] kmbla;

		public int myhgg;

		public int xrhmv;

		private kpthf gemco;

		private object pyqnp;

		private void tfbva()
		{
			try
			{
				bool flag = true;
				if (jtgpl != 0)
				{
					wxngw.kfgxs.Write(kmbla, myhgg, xrhmv);
					goto IL_0189;
				}
				kpthf p = gemco;
				gemco = default(kpthf);
				jtgpl = -1;
				goto IL_0147;
				IL_0147:
				p.ekzxl();
				p = default(kpthf);
				wxngw.kfgxs.ejbiu(wxngw.mukqe);
				wxngw.mukqe = int.MaxValue;
				goto IL_0189;
				IL_0189:
				if (wxngw.kfgxs.Length >= 4)
				{
					if (wxngw.mukqe == int.MaxValue)
					{
						wxngw.mukqe = 4 + wxngw.kfgxs[1] * 65536 + wxngw.kfgxs[2] * 256 + wxngw.kfgxs[3];
					}
					if (wxngw.kfgxs.Length >= wxngw.mukqe)
					{
						p = wxngw.tmymn(wxngw.kfgxs.GetBuffer(), 0, wxngw.mukqe).avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							jtgpl = 0;
							gemco = p;
							vcrih.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
						goto IL_0147;
					}
				}
			}
			catch (Exception p2)
			{
				jtgpl = -2;
				vcrih.iurqb(p2);
				return;
			}
			jtgpl = -2;
			vcrih.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in tfbva
			this.tfbva();
		}

		private void gfsna(fgyyk p0)
		{
			vcrih.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in gfsna
			this.gfsna(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CProcessAlert_003Ed__e : fgyyk
	{
		public int pifmc;

		public ljmxa wnzrl;

		public ortbm hozth;

		public byte[] ltllv;

		public int rcsgj;

		public int swbbk;

		public byte[] bwwmv;

		private kpthf mjpsa;

		private object rqckg;

		private void mxazi()
		{
			try
			{
				bool flag = true;
				kpthf p2;
				kpthf p;
				switch (pifmc)
				{
				default:
					if (swbbk == 0 || 1 == 0)
					{
						throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "Alert"));
					}
					if (hozth.gyria != -1)
					{
						bwwmv = new byte[2];
						bwwmv[0] = (byte)hozth.gyria;
						bwwmv[1] = ltllv[rcsgj];
						p2 = hozth.ekqhl(bwwmv, 0, 2).avdby(p1: false).vrtmi();
						if (!p2.zpafv || 1 == 0)
						{
							pifmc = 0;
							mjpsa = p2;
							wnzrl.wqiyk(ref p2, ref this);
							flag = false;
							return;
						}
						goto IL_010b;
					}
					goto IL_01ef;
				case 0:
					p2 = mjpsa;
					mjpsa = default(kpthf);
					pifmc = -1;
					goto IL_010b;
				case 1:
					{
						p = mjpsa;
						mjpsa = default(kpthf);
						pifmc = -1;
						goto IL_01c4;
					}
					IL_010b:
					p2.ekzxl();
					p2 = default(kpthf);
					hozth.gyria = -1;
					rcsgj++;
					swbbk--;
					goto IL_01ef;
					IL_01c4:
					p.ekzxl();
					p = default(kpthf);
					rcsgj += 2;
					swbbk -= 2;
					goto IL_01ef;
					IL_01ef:
					if (swbbk > 1)
					{
						p = hozth.ekqhl(ltllv, rcsgj, 2).avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							pifmc = 1;
							mjpsa = p;
							wnzrl.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
						goto IL_01c4;
					}
					if (swbbk == 1)
					{
						hozth.gyria = ltllv[rcsgj];
					}
					break;
				}
			}
			catch (Exception p3)
			{
				pifmc = -2;
				wnzrl.iurqb(p3);
				return;
			}
			pifmc = -2;
			wnzrl.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in mxazi
			this.mxazi();
		}

		private void lwwub(fgyyk p0)
		{
			wnzrl.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in lwwub
			this.lwwub(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CProcessData_003Ed__12 : fgyyk
	{
		public int hkaxy;

		public ljmxa bqyno;

		public ortbm ityac;

		public byte[] hzjwq;

		public int zocep;

		public int baphv;

		public IDisposable acdun;

		private xuwyj<IDisposable> npyct;

		private object pcgfq;

		private void wfxpf()
		{
			try
			{
				bool flag = true;
				xuwyj<IDisposable> p;
				if (hkaxy != 0)
				{
					p = ityac.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						hkaxy = 0;
						npyct = p;
						bqyno.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = npyct;
					npyct = default(xuwyj<IDisposable>);
					hkaxy = -1;
				}
				IDisposable disposable = p.gbccf();
				p = default(xuwyj<IDisposable>);
				IDisposable disposable2 = disposable;
				acdun = disposable2;
				try
				{
					if (ityac.qjbzn == bpnki.yiqfh)
					{
						throw new TlsException(mjddr.ypibb, "Received unencrypted application data.");
					}
					ityac.quhdu.Write(hzjwq, zocep, baphv);
				}
				finally
				{
					if (flag && 0 == 0 && acdun != null && 0 == 0)
					{
						acdun.Dispose();
					}
				}
			}
			catch (Exception p2)
			{
				hkaxy = -2;
				bqyno.iurqb(p2);
				return;
			}
			hkaxy = -2;
			bqyno.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in wfxpf
			this.wfxpf();
		}

		private void zvzqc(fgyyk p0)
		{
			bqyno.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in zvzqc
			this.zvzqc(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CReceiveData_003Ed__16 : fgyyk
	{
		public int zvjlw;

		public vxvbw<bool> viqvn;

		public ortbm wwnap;

		public int vemdp;

		public int xwkno;

		public nxtme<byte> rgziq;

		private xuwyj<int> yxjkf;

		private object qapkk;

		private kpthf dxthp;

		private void qtmik()
		{
			bool p2;
			try
			{
				bool flag = true;
				kpthf p;
				switch (zvjlw)
				{
				default:
					if (wwnap.twcca >= vemdp)
					{
						p2 = true;
					}
					else
					{
						if (wwnap.lptja != hlkgm.tqqib)
						{
							goto case 0;
						}
						wwnap.hzxof(LogLevel.Debug, "Incomplete TLS packet received before closed connection:", wwnap.yevcd, 0, wwnap.twcca);
						wwnap.twcca = 0;
						p2 = false;
					}
					goto end_IL_0000;
				case 0:
					while (true)
					{
						try
						{
							xuwyj<int> p3;
							if (zvjlw != 0)
							{
								rgziq = wwnap.yevcd.plhfl(wwnap.twcca, 18437 - wwnap.twcca);
								p3 = wwnap.bguay.rhjom(rgziq).giftg(p1: false).vuozn();
								if (!p3.hqxbj || 1 == 0)
								{
									zvjlw = 0;
									yxjkf = p3;
									viqvn.xiwgo(ref p3, ref this);
									flag = false;
									return;
								}
							}
							else
							{
								p3 = yxjkf;
								yxjkf = default(xuwyj<int>);
								zvjlw = -1;
							}
							int num = p3.gbccf();
							p3 = default(xuwyj<int>);
							int num2 = num;
							xwkno = num2;
						}
						catch (SocketException)
						{
							wwnap.bvdcq(LogLevel.Debug, "TLS socket error, {0} bytes of data were received.", wwnap.twcca);
							throw;
						}
						if (xwkno == 0 || 1 == 0)
						{
							wwnap.bvdcq(LogLevel.Debug, "TLS socket was closed, {0} bytes of data were received.", wwnap.twcca);
							p = wwnap.szxmn().avdby(p1: false).vrtmi();
							if (p.zpafv ? true : false)
							{
								goto end_IL_000e;
							}
							zvjlw = 1;
							dxthp = p;
							viqvn.xiwgo(ref p, ref this);
							flag = false;
							return;
						}
						wwnap.twcca += xwkno;
						if (wwnap.twcca < vemdp)
						{
							continue;
						}
						p2 = true;
						break;
					}
					goto end_IL_0000;
				case 1:
					{
						p = dxthp;
						dxthp = default(kpthf);
						zvjlw = -1;
						break;
					}
					end_IL_000e:
					break;
				}
				p.ekzxl();
				p = default(kpthf);
				wwnap.exyau();
				p2 = false;
				end_IL_0000:;
			}
			catch (Exception p4)
			{
				zvjlw = -2;
				viqvn.tudwl(p4);
				return;
			}
			zvjlw = -2;
			viqvn.vzyck(p2);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in qtmik
			this.qtmik();
		}

		private void fiyki(fgyyk p0)
		{
			viqvn.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in fiyki
			this.fiyki(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CReceivePacketInner_003Ed__1c : fgyyk
	{
		public int nfqzn;

		public vxvbw<int> unqok;

		public ortbm pnubm;

		public int lzned;

		public int oexdr;

		public byte[] lziut;

		public int bkrcg;

		private xuwyj<bool> itstk;

		private object wtgoc;

		public IDisposable bqedz;

		private xuwyj<IDisposable> jxqnp;

		private xuwyj<int> oqebl;

		private void vpott()
		{
			int p3;
			try
			{
				bool flag = true;
				IDisposable disposable;
				xuwyj<IDisposable> p;
				IDisposable disposable2;
				switch (nfqzn)
				{
				default:
					try
					{
						xuwyj<bool> p2;
						if (nfqzn != 0)
						{
							p2 = pnubm.jmhop(5).obrzd(pnubm.gawrz.aeqtf).giftg(p1: false)
								.vuozn();
							if (!p2.hqxbj || 1 == 0)
							{
								nfqzn = 0;
								itstk = p2;
								unqok.xiwgo(ref p2, ref this);
								flag = false;
								return;
							}
						}
						else
						{
							p2 = itstk;
							itstk = default(xuwyj<bool>);
							nfqzn = -1;
						}
						bool num = p2.gbccf();
						p2 = default(xuwyj<bool>);
						if (num ? true : false)
						{
							goto end_IL_0025;
						}
						p3 = 0;
						goto end_IL_0000;
						end_IL_0025:;
					}
					catch (TimeoutException inner)
					{
						throw new TlsException("The operation was not completed within the specified time limit.", NetworkSessionExceptionStatus.Timeout, inner);
					}
					lzned = 0;
					oexdr = (pnubm.yevcd[1] << 8) + pnubm.yevcd[2];
					if (oexdr < 256 || oexdr >= 1024)
					{
						if (pnubm.lptja == hlkgm.iucmn)
						{
							lzned = ((pnubm.yevcd[0] & 0x3F) << 8) + pnubm.yevcd[1];
							if (lzned > 0)
							{
								byte b = ((((pnubm.yevcd[0] & 0x80) != 0) ? true : false) ? pnubm.yevcd[2] : pnubm.yevcd[3]);
								if (b == 1)
								{
									if ((pnubm.yevcd[0] & 0x80) == 0 || 1 == 0)
									{
										pnubm.yevcd[0] = b;
										pnubm.yevcd[1] = pnubm.yevcd[4];
										Array.Copy(pnubm.yevcd, 5, pnubm.yevcd, 2, pnubm.twcca - 5);
										pnubm.twcca -= 3;
									}
									else
									{
										pnubm.yevcd[0] = b;
										pnubm.yevcd[1] = pnubm.yevcd[3];
										pnubm.yevcd[2] = pnubm.yevcd[4];
										Array.Copy(pnubm.yevcd, 5, pnubm.yevcd, 3, pnubm.twcca - 5);
										pnubm.twcca -= 2;
									}
								}
								else
								{
									lzned = 0;
								}
							}
						}
						if (lzned == 0 || 1 == 0)
						{
							pnubm.hzxof(LogLevel.Debug, "Invalid TLS packet received:", pnubm.yevcd, 0, pnubm.twcca);
							string value = EncodingTools.ASCII.GetString(pnubm.yevcd, 0, 4);
							TlsException ex = new TlsException(mjddr.puqjh);
							ex.Data["TlsData"] = value;
							throw ex;
						}
					}
					else
					{
						lzned = (pnubm.yevcd[3] << 8) + pnubm.yevcd[4] + 5;
					}
					if (lzned > pnubm.twcca)
					{
						goto case 1;
					}
					goto IL_0443;
				case 1:
					try
					{
						int num2 = nfqzn;
						xuwyj<bool> p4;
						if (num2 != 1)
						{
							p4 = pnubm.jmhop(lzned).obrzd(pnubm.gawrz.aeqtf).giftg(p1: false)
								.vuozn();
							if (!p4.hqxbj || 1 == 0)
							{
								nfqzn = 1;
								itstk = p4;
								unqok.xiwgo(ref p4, ref this);
								flag = false;
								return;
							}
						}
						else
						{
							p4 = itstk;
							itstk = default(xuwyj<bool>);
							nfqzn = -1;
						}
						bool num3 = p4.gbccf();
						p4 = default(xuwyj<bool>);
						if (!num3 || 1 == 0)
						{
							throw new TlsException("Connection was closed by the remote connection end.", NetworkSessionExceptionStatus.ConnectionClosed);
						}
					}
					catch (TimeoutException inner2)
					{
						throw new TlsException("The operation was not completed within the specified time limit.", NetworkSessionExceptionStatus.Timeout, inner2);
					}
					goto IL_0443;
				case 2:
					p = jxqnp;
					jxqnp = default(xuwyj<IDisposable>);
					nfqzn = -1;
					goto IL_04b9;
				case 3:
					break;
					IL_0443:
					p = pnubm.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						nfqzn = 2;
						jxqnp = p;
						unqok.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_04b9;
					IL_04b9:
					disposable = p.gbccf();
					p = default(xuwyj<IDisposable>);
					disposable2 = disposable;
					bqedz = disposable2;
					break;
				}
				try
				{
					int num4 = nfqzn;
					xuwyj<int> p5;
					if (num4 == 3)
					{
						p5 = oqebl;
						oqebl = default(xuwyj<int>);
						nfqzn = -1;
						goto IL_05d2;
					}
					if (pnubm.zdprx != oexdr)
					{
						if (pnubm.lptja != hlkgm.iucmn)
						{
							throw new TlsException(mjddr.puqjh);
						}
						if (oexdr < 768)
						{
							lziut = new byte[5] { 128, 3, 0, 0, 1 };
							p5 = pnubm.bguay.razzy(lziut.liutv()).obrzd(pnubm.gawrz.abtaf).giftg(p1: false)
								.vuozn();
							if (!p5.hqxbj || 1 == 0)
							{
								nfqzn = 3;
								oqebl = p5;
								unqok.xiwgo(ref p5, ref this);
								flag = false;
								return;
							}
							goto IL_05d2;
						}
						goto IL_05e2;
					}
					goto end_IL_04d2;
					IL_05d2:
					p5.gbccf();
					p5 = default(xuwyj<int>);
					goto IL_05e2;
					IL_05e2:
					pnubm.zdprx = oexdr;
					end_IL_04d2:;
				}
				finally
				{
					if (flag && 0 == 0 && bqedz != null && 0 == 0)
					{
						bqedz.Dispose();
					}
				}
				Array.Copy(pnubm.yevcd, 0, pnubm.zddyj, 0, lzned);
				bkrcg = pnubm.qjbzn.bvfhg(pnubm.zddyj, lzned);
				pnubm.twcca -= lzned;
				Array.Copy(pnubm.yevcd, lzned, pnubm.yevcd, 0, pnubm.twcca);
				p3 = bkrcg;
				end_IL_0000:;
			}
			catch (Exception p6)
			{
				nfqzn = -2;
				unqok.tudwl(p6);
				return;
			}
			nfqzn = -2;
			unqok.vzyck(p3);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in vpott
			this.vpott();
		}

		private void qprix(fgyyk p0)
		{
			unqok.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in qprix
			this.qprix(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003C_003CProcess_003Eb__26_003Ed__27 : fgyyk
	{
		public int ksvvg;

		public ljmxa rbtxn;

		public ortbm bndxc;

		public IDisposable miryc;

		private xuwyj<IDisposable> fchzq;

		private object bgrwh;

		private void bgliu()
		{
			try
			{
				bool flag = true;
				xuwyj<IDisposable> p;
				if (ksvvg != 0)
				{
					p = bndxc.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						ksvvg = 0;
						fchzq = p;
						rbtxn.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = fchzq;
					fchzq = default(xuwyj<IDisposable>);
					ksvvg = -1;
				}
				IDisposable disposable = p.gbccf();
				p = default(xuwyj<IDisposable>);
				IDisposable disposable2 = disposable;
				miryc = disposable2;
				try
				{
					bndxc.wqwju = false;
				}
				finally
				{
					if (flag && 0 == 0 && miryc != null && 0 == 0)
					{
						miryc.Dispose();
					}
				}
			}
			catch (Exception p2)
			{
				ksvvg = -2;
				rbtxn.iurqb(p2);
				return;
			}
			ksvvg = -2;
			rbtxn.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in bgliu
			this.bgliu();
		}

		private void kbyqu(fgyyk p0)
		{
			rbtxn.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in kbyqu
			this.kbyqu(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CProcess_003Ed__2b : fgyyk
	{
		public int yqqqt;

		public ljmxa klxcb;

		public ortbm qmfch;

		public IDisposable rxgba;

		private xuwyj<IDisposable> xgcys;

		private object gehcz;

		private kpthf lpvlh;

		private void crogq()
		{
			try
			{
				bool flag = true;
				IDisposable disposable;
				xuwyj<IDisposable> p2;
				IDisposable disposable2;
				kpthf p;
				switch (yqqqt)
				{
				default:
					p2 = qmfch.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p2.hqxbj || 1 == 0)
					{
						yqqqt = 0;
						xgcys = p2;
						klxcb.wqiyk(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_008f;
				case 0:
					p2 = xgcys;
					xgcys = default(xuwyj<IDisposable>);
					yqqqt = -1;
					goto IL_008f;
				case 1:
					{
						p = lpvlh;
						lpvlh = default(kpthf);
						yqqqt = -1;
						break;
					}
					IL_008f:
					disposable = p2.gbccf();
					p2 = default(xuwyj<IDisposable>);
					disposable2 = disposable;
					rxgba = disposable2;
					try
					{
						if (!qmfch.wqwju)
						{
							qmfch.wqwju = true;
							goto end_IL_00a8;
						}
						Thread.Sleep(0);
						goto end_IL_0000;
						end_IL_00a8:;
					}
					finally
					{
						if (flag && 0 == 0 && rxgba != null && 0 == 0)
						{
							rxgba.Dispose();
						}
					}
					p = qmfch.zrpeb().orims(qmfch.vsijy).avdby(p1: false)
						.vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						yqqqt = 1;
						lpvlh = p;
						klxcb.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
				}
				p.ekzxl();
				p = default(kpthf);
				end_IL_0000:;
			}
			catch (Exception p3)
			{
				yqqqt = -2;
				klxcb.iurqb(p3);
				return;
			}
			yqqqt = -2;
			klxcb.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in crogq
			this.crogq();
		}

		private void feioz(fgyyk p0)
		{
			klxcb.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in feioz
			this.feioz(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CprocessInnerAsync_003Ed__30 : fgyyk
	{
		public int hdmzi;

		public ljmxa opnky;

		public ortbm iuxwb;

		public Exception vguyf;

		public int rrwpy;

		public string bhegs;

		public Exception mpsak;

		private xuwyj<int> leals;

		private object fdhww;

		private kpthf bqond;

		private xuwyj<Exception> iqgka;

		private void thlyo()
		{
			try
			{
				bool flag = true;
				xuwyj<Exception> p;
				switch (hdmzi)
				{
				default:
					vguyf = null;
					goto case 0;
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
					try
					{
						int num;
						xuwyj<int> p6;
						int num2;
						kpthf p5;
						kpthf p3;
						kpthf p4;
						kpthf p2;
						switch (hdmzi)
						{
						default:
							if (iuxwb.qjbzn != null)
							{
								p6 = iuxwb.ynzni().giftg(p1: false).vuozn();
								if (!p6.hqxbj || 1 == 0)
								{
									hdmzi = 0;
									leals = p6;
									opnky.wqiyk(ref p6, ref this);
									flag = false;
									return;
								}
								goto IL_00df;
							}
							goto end_IL_0032;
						case 0:
							p6 = leals;
							leals = default(xuwyj<int>);
							hdmzi = -1;
							goto IL_00df;
						case 1:
							p5 = bqond;
							bqond = default(kpthf);
							hdmzi = -1;
							goto IL_01f2;
						case 2:
							p4 = bqond;
							bqond = default(kpthf);
							hdmzi = -1;
							goto IL_028a;
						case 3:
							p3 = bqond;
							bqond = default(kpthf);
							hdmzi = -1;
							goto IL_0346;
						case 4:
							{
								p2 = bqond;
								bqond = default(kpthf);
								hdmzi = -1;
								break;
							}
							IL_035a:
							p2 = iuxwb.mlrkx(iuxwb.zddyj, 5, rrwpy - 5).avdby(p1: false).vrtmi();
							if (!p2.zpafv || 1 == 0)
							{
								hdmzi = 4;
								bqond = p2;
								opnky.wqiyk(ref p2, ref this);
								flag = false;
								return;
							}
							break;
							IL_0206:
							p4 = iuxwb.jgsir(iuxwb.zddyj, 5, rrwpy - 5).avdby(p1: false).vrtmi();
							if (!p4.zpafv || 1 == 0)
							{
								hdmzi = 2;
								bqond = p4;
								opnky.wqiyk(ref p4, ref this);
								flag = false;
								return;
							}
							goto IL_028a;
							IL_02c2:
							p3 = iuxwb.jmkba(iuxwb.zddyj, 5, rrwpy - 5).avdby(p1: false).vrtmi();
							if (!p3.zpafv || 1 == 0)
							{
								hdmzi = 3;
								bqond = p3;
								opnky.wqiyk(ref p3, ref this);
								flag = false;
								return;
							}
							goto IL_0346;
							IL_00df:
							num = p6.gbccf();
							p6 = default(xuwyj<int>);
							num2 = num;
							rrwpy = num2;
							if ((rrwpy != 0) ? true : false)
							{
								iuxwb.hzxof(LogLevel.Verbose, "Received TLS packet: ", iuxwb.zddyj, 0, rrwpy);
								if (rrwpy > 5)
								{
									switch (iuxwb.zddyj[0])
									{
									case 1:
										break;
									case 22:
										goto IL_0206;
									case 20:
										iuxwb.pfzex(iuxwb.zddyj, 5, rrwpy - 5);
										goto IL_0429;
									case 21:
										goto IL_02c2;
									case 23:
										goto IL_035a;
									default:
										iuxwb.oclcs.kxlvu(TlsDebugEventType.UnknownMessageType, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, iuxwb.zddyj, 5, rrwpy - 5);
										goto IL_0429;
									}
									p5 = iuxwb.ligyt(iuxwb.zddyj, 0, rrwpy).avdby(p1: false).vrtmi();
									if (!p5.zpafv || 1 == 0)
									{
										hdmzi = 1;
										bqond = p5;
										opnky.wqiyk(ref p5, ref this);
										flag = false;
										return;
									}
									goto IL_01f2;
								}
								goto IL_0429;
							}
							goto end_IL_0032;
							IL_01f2:
							p5.ekzxl();
							p5 = default(kpthf);
							goto IL_0429;
							IL_0346:
							p3.ekzxl();
							p3 = default(kpthf);
							goto IL_0429;
							IL_028a:
							p4.ekzxl();
							p4 = default(kpthf);
							goto IL_0429;
						}
						p2.ekzxl();
						p2 = default(kpthf);
						goto IL_0429;
						end_IL_0032:;
					}
					catch (Exception ex)
					{
						vguyf = (vguyf = ex);
						goto IL_0429;
					}
					goto end_IL_0000;
				case 5:
					{
						p = iqgka;
						iqgka = default(xuwyj<Exception>);
						hdmzi = -1;
						break;
					}
					IL_0429:
					if (vguyf != null && 0 == 0)
					{
						bhegs = ((vguyf is TlsException) ? null : "Error while processing TLS packet");
						p = iuxwb.fptsn(vguyf, bhegs).giftg(p1: false).vuozn();
						if (!p.hqxbj || 1 == 0)
						{
							hdmzi = 5;
							iqgka = p;
							opnky.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
						break;
					}
					goto end_IL_0000;
				}
				Exception ex2 = p.gbccf();
				p = default(xuwyj<Exception>);
				Exception ex3 = ex2;
				mpsak = ex3;
				throw mpsak;
				end_IL_0000:;
			}
			catch (Exception p7)
			{
				hdmzi = -2;
				opnky.iurqb(p7);
				return;
			}
			hdmzi = -2;
			opnky.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in thlyo
			this.thlyo();
		}

		private void odbtd(fgyyk p0)
		{
			opnky.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in odbtd
			this.odbtd(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CEnqueue_003Ed__39 : fgyyk
	{
		public int stvac;

		public ljmxa yvdyv;

		public ortbm gdcec;

		public vcedo yzdtz;

		public byte[] fmhsc;

		public int zhpos;

		public int vpuau;

		private kpthf tryvq;

		private object ubrzp;

		public IDisposable clrtd;

		private xuwyj<IDisposable> fcnng;

		public IDisposable gwemn;

		private void jfwod()
		{
			try
			{
				bool flag = true;
				kpthf p2;
				IDisposable disposable;
				xuwyj<IDisposable> p;
				IDisposable disposable2;
				switch (stvac)
				{
				case 0:
					p2 = tryvq;
					tryvq = default(kpthf);
					stvac = -1;
					goto IL_00ad;
				default:
					if (vpuau > 16384)
					{
						p2 = gdcec.kjdzy(yzdtz, fmhsc, zhpos, 16384).avdby(p1: false).vrtmi();
						if (!p2.zpafv || 1 == 0)
						{
							stvac = 0;
							tryvq = p2;
							yvdyv.wqiyk(ref p2, ref this);
							flag = false;
							return;
						}
						goto IL_00ad;
					}
					p = gdcec.mpawi.uivze().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						stvac = 1;
						fcnng = p;
						yvdyv.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_0165;
				case 1:
					p = fcnng;
					fcnng = default(xuwyj<IDisposable>);
					stvac = -1;
					goto IL_0165;
				case 2:
					break;
					IL_00ad:
					p2.ekzxl();
					p2 = default(kpthf);
					zhpos += 16384;
					vpuau -= 16384;
					goto default;
					IL_0165:
					disposable = p.gbccf();
					p = default(xuwyj<IDisposable>);
					disposable2 = disposable;
					clrtd = disposable2;
					break;
				}
				try
				{
					int num = stvac;
					xuwyj<IDisposable> p3;
					if (num != 2)
					{
						Array.Copy(fmhsc, zhpos, gdcec.tdtgo, 5, vpuau);
						gdcec.tdtgo[0] = (byte)yzdtz;
						gdcec.tdtgo[1] = (byte)(gdcec.zdprx >> 8);
						gdcec.tdtgo[2] = (byte)(gdcec.zdprx & 0xFF);
						gdcec.tdtgo[3] = (byte)(vpuau >> 8);
						gdcec.tdtgo[4] = (byte)(vpuau & 0xFF);
						if (vpuau > 0 && gdcec.rirto && 0 == 0 && yzdtz == vcedo.idfcl)
						{
							gdcec.hsfzx[0] = 23;
							gdcec.hsfzx[1] = gdcec.tdtgo[1];
							gdcec.hsfzx[2] = gdcec.tdtgo[2];
							gdcec.hsfzx[3] = 0;
							gdcec.hsfzx[4] = 0;
							gdcec.hzxof(LogLevel.Verbose, "Sent TLS packet: ", gdcec.hsfzx, 0, 5);
							int count = gdcec.tjjru.bvfhg(gdcec.hsfzx, 5);
							gdcec.vyimy.Write(gdcec.hsfzx, 0, count);
						}
						gdcec.hzxof(LogLevel.Verbose, "Sent TLS packet: ", gdcec.tdtgo, 0, vpuau + 5);
						vpuau = gdcec.tjjru.bvfhg(gdcec.tdtgo, vpuau + 5);
						if (yzdtz == vcedo.dwbji)
						{
							bpnki tjjru = gdcec.tjjru;
							gdcec.tjjru = gdcec.ebitx;
							gdcec.ebitx = null;
							if (tjjru != null && 0 == 0)
							{
								tjjru.egphd();
							}
						}
						gdcec.vyimy.Write(gdcec.tdtgo, 0, vpuau);
						p3 = gdcec.apyzr.uivze().giftg(p1: false).vuozn();
						if (!p3.hqxbj || 1 == 0)
						{
							stvac = 2;
							fcnng = p3;
							yvdyv.wqiyk(ref p3, ref this);
							flag = false;
							return;
						}
					}
					else
					{
						p3 = fcnng;
						fcnng = default(xuwyj<IDisposable>);
						stvac = -1;
					}
					IDisposable disposable3 = p3.gbccf();
					p3 = default(xuwyj<IDisposable>);
					IDisposable disposable4 = disposable3;
					gwemn = disposable4;
					try
					{
						gdcec.bxowl = true;
					}
					finally
					{
						if (flag && 0 == 0 && gwemn != null && 0 == 0)
						{
							gwemn.Dispose();
						}
					}
				}
				finally
				{
					if (flag && 0 == 0 && clrtd != null && 0 == 0)
					{
						clrtd.Dispose();
					}
				}
			}
			catch (Exception p4)
			{
				stvac = -2;
				yvdyv.iurqb(p4);
				return;
			}
			stvac = -2;
			yvdyv.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in jfwod
			this.jfwod();
		}

		private void ocmhx(fgyyk p0)
		{
			yvdyv.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ocmhx
			this.ocmhx(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003C_003CSendQueue_003Eb__3f_003Ed__41 : fgyyk
	{
		public int wnpfl;

		public ljmxa ybnhw;

		public ortbm oletx;

		public IDisposable etadn;

		private xuwyj<IDisposable> gwofx;

		private object lmqal;

		private void cqqrr()
		{
			try
			{
				bool flag = true;
				xuwyj<IDisposable> p;
				if (wnpfl != 0)
				{
					oletx.vyimy.SetLength(0L);
					p = oletx.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						wnpfl = 0;
						gwofx = p;
						ybnhw.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = gwofx;
					gwofx = default(xuwyj<IDisposable>);
					wnpfl = -1;
				}
				IDisposable disposable = p.gbccf();
				p = default(xuwyj<IDisposable>);
				IDisposable disposable2 = disposable;
				etadn = disposable2;
				try
				{
					oletx.bxowl = false;
				}
				finally
				{
					if (flag && 0 == 0 && etadn != null && 0 == 0)
					{
						etadn.Dispose();
					}
				}
			}
			catch (Exception p2)
			{
				wnpfl = -2;
				ybnhw.iurqb(p2);
				return;
			}
			wnpfl = -2;
			ybnhw.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in cqqrr
			this.cqqrr();
		}

		private void wjizf(fgyyk p0)
		{
			ybnhw.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in wjizf
			this.wjizf(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CSendQueue_003Ed__45 : fgyyk
	{
		public int bborp;

		public ljmxa tevnb;

		public ortbm woonb;

		public Func<exkzi> qphnu;

		public IDisposable obykg;

		private xuwyj<IDisposable> samyw;

		private object wwqap;

		private kpthf eovcb;

		private void qurjf()
		{
			try
			{
				bool flag = true;
				IDisposable disposable;
				xuwyj<IDisposable> p;
				IDisposable disposable2;
				switch (bborp)
				{
				default:
					qphnu = null;
					p = woonb.mpawi.uivze().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						bborp = 0;
						samyw = p;
						tevnb.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_0096;
				case 0:
					p = samyw;
					samyw = default(xuwyj<IDisposable>);
					bborp = -1;
					goto IL_0096;
				case 1:
					break;
					IL_0096:
					disposable = p.gbccf();
					p = default(xuwyj<IDisposable>);
					disposable2 = disposable;
					obykg = disposable2;
					break;
				}
				try
				{
					int num = bborp;
					kpthf p2;
					if (num == 1)
					{
						p2 = eovcb;
						eovcb = default(kpthf);
						bborp = -1;
						goto IL_0182;
					}
					if (woonb.vyimy.Length != 0)
					{
						exkzi p3 = woonb.ulpqp();
						if (qphnu == null || 1 == 0)
						{
							qphnu = woonb.ilueg;
						}
						p2 = p3.orims(qphnu).avdby(p1: false).vrtmi();
						if (!p2.zpafv || 1 == 0)
						{
							bborp = 1;
							eovcb = p2;
							tevnb.wqiyk(ref p2, ref this);
							flag = false;
							return;
						}
						goto IL_0182;
					}
					goto end_IL_00b7;
					IL_0182:
					p2.ekzxl();
					p2 = default(kpthf);
					end_IL_00b7:;
				}
				finally
				{
					if (flag && 0 == 0 && obykg != null && 0 == 0)
					{
						obykg.Dispose();
					}
				}
			}
			catch (Exception p4)
			{
				bborp = -2;
				tevnb.iurqb(p4);
				return;
			}
			bborp = -2;
			tevnb.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in qurjf
			this.qurjf();
		}

		private void jctww(fgyyk p0)
		{
			tevnb.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in jctww
			this.jctww(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CsendQueueInner_003Ed__4a : fgyyk
	{
		public int vrmud;

		public ljmxa imxpy;

		public ortbm xzakh;

		public nxtme<byte> kmgav;

		private kpthf nbjmk;

		private object dqyhy;

		private void gfszo()
		{
			try
			{
				bool flag = true;
				int num = vrmud;
				_ = 0;
				try
				{
					kpthf p;
					if (vrmud != 0)
					{
						ref nxtme<byte> reference = ref kmgav;
						reference = new nxtme<byte>(xzakh.vyimy.GetBuffer(), 0, (int)xzakh.vyimy.Length);
						p = xzakh.bguay.zykkj(kmgav).rvbbe(xzakh.gawrz.abtaf).avdby(p1: false)
							.vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							vrmud = 0;
							nbjmk = p;
							imxpy.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
					}
					else
					{
						p = nbjmk;
						nbjmk = default(kpthf);
						vrmud = -1;
					}
					p.ekzxl();
					p = default(kpthf);
				}
				catch (Exception p2)
				{
					xzakh.luosf.ngryw(p2);
					throw;
				}
			}
			catch (Exception p3)
			{
				vrmud = -2;
				imxpy.iurqb(p3);
				return;
			}
			vrmud = -2;
			imxpy.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in gfszo
			this.gfszo();
		}

		private void gvpoj(fgyyk p0)
		{
			imxpy.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in gvpoj
			this.gvpoj(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CSetSecureState_003Ed__4e : fgyyk
	{
		public int mucko;

		public ljmxa cijkt;

		public ortbm rdugx;

		public IDisposable ockov;

		private xuwyj<IDisposable> akysj;

		private object sdkhb;

		private void rsbhf()
		{
			try
			{
				bool flag = true;
				xuwyj<IDisposable> p;
				if (mucko != 0)
				{
					p = rdugx.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						mucko = 0;
						akysj = p;
						cijkt.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = akysj;
					akysj = default(xuwyj<IDisposable>);
					mucko = -1;
				}
				IDisposable disposable = p.gbccf();
				p = default(xuwyj<IDisposable>);
				IDisposable disposable2 = disposable;
				ockov = disposable2;
				try
				{
					if (rdugx.lptja == hlkgm.iucmn)
					{
						rdugx.lptja = hlkgm.rhxxi;
					}
					rdugx.qteas = false;
				}
				finally
				{
					if (flag && 0 == 0 && ockov != null && 0 == 0)
					{
						ockov.Dispose();
					}
				}
				rdugx.oclcs.ivtmn(TlsDebugEventType.Secured, TlsDebugEventSource.Unspecified, TlsDebugLevel.Important);
			}
			catch (Exception p2)
			{
				mucko = -2;
				cijkt.iurqb(p2);
				return;
			}
			mucko = -2;
			cijkt.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in rsbhf
			this.rsbhf();
		}

		private void zumbk(fgyyk p0)
		{
			cijkt.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in zumbk
			this.zumbk(p0);
		}
	}

	private sealed class uxmyo
	{
		[StructLayout(LayoutKind.Auto)]
		private struct _003C_003CAlert_003Eb__52_003Ed__56 : fgyyk
		{
			public int hgmpz;

			public ljmxa bnxue;

			public uxmyo wideu;

			private kpthf vqzgz;

			private object agodf;

			public IDisposable nqfaj;

			private xuwyj<IDisposable> gugvl;

			private void pqbro()
			{
				try
				{
					bool flag = true;
					kpthf p4;
					IDisposable disposable;
					xuwyj<IDisposable> p2;
					IDisposable disposable2;
					kpthf p3;
					kpthf p;
					switch (hgmpz)
					{
					default:
						p4 = wideu.tzpfq.kjdzy(wideu.arbla.qeloj, wideu.tyyci, 0, wideu.tyyci.Length).avdby(p1: false).vrtmi();
						if (!p4.zpafv || 1 == 0)
						{
							hgmpz = 0;
							vqzgz = p4;
							bnxue.wqiyk(ref p4, ref this);
							flag = false;
							return;
						}
						goto IL_00c4;
					case 0:
						p4 = vqzgz;
						vqzgz = default(kpthf);
						hgmpz = -1;
						goto IL_00c4;
					case 1:
						p3 = vqzgz;
						vqzgz = default(kpthf);
						hgmpz = -1;
						goto IL_0148;
					case 2:
						p2 = gugvl;
						gugvl = default(xuwyj<IDisposable>);
						hgmpz = -1;
						goto IL_01e9;
					case 3:
						try
						{
							int num = hgmpz;
							kpthf p5;
							if (num != 3)
							{
								wideu.tzpfq.lptja = hlkgm.tqqib;
								p5 = wideu.tzpfq.bguay.qxxgh().avdby(p1: false).vrtmi();
								if (!p5.zpafv || 1 == 0)
								{
									hgmpz = 3;
									vqzgz = p5;
									bnxue.wqiyk(ref p5, ref this);
									flag = false;
									return;
								}
							}
							else
							{
								p5 = vqzgz;
								vqzgz = default(kpthf);
								hgmpz = -1;
							}
							p5.ekzxl();
							p5 = default(kpthf);
						}
						finally
						{
							if (flag && 0 == 0 && nqfaj != null && 0 == 0)
							{
								nqfaj.Dispose();
							}
						}
						if (!wideu.ejuby || 1 == 0)
						{
							p = wideu.tzpfq.szxmn().avdby(p1: false).vrtmi();
							if (!p.zpafv || 1 == 0)
							{
								hgmpz = 4;
								vqzgz = p;
								bnxue.wqiyk(ref p, ref this);
								flag = false;
								return;
							}
							break;
						}
						goto end_IL_0000;
					case 4:
						{
							p = vqzgz;
							vqzgz = default(kpthf);
							hgmpz = -1;
							break;
						}
						IL_00c4:
						p4.ekzxl();
						p4 = default(kpthf);
						p3 = wideu.tzpfq.zupbs().avdby(p1: false).vrtmi();
						if (!p3.zpafv || 1 == 0)
						{
							hgmpz = 1;
							vqzgz = p3;
							bnxue.wqiyk(ref p3, ref this);
							flag = false;
							return;
						}
						goto IL_0148;
						IL_01e9:
						disposable = p2.gbccf();
						p2 = default(xuwyj<IDisposable>);
						disposable2 = disposable;
						nqfaj = disposable2;
						goto case 3;
						IL_0148:
						p3.ekzxl();
						p3 = default(kpthf);
						if (wideu.lbtgz && 0 == 0)
						{
							p2 = wideu.tzpfq.apyzr.uivze().giftg(p1: false).vuozn();
							if (!p2.hqxbj || 1 == 0)
							{
								hgmpz = 2;
								gugvl = p2;
								bnxue.wqiyk(ref p2, ref this);
								flag = false;
								return;
							}
							goto IL_01e9;
						}
						goto end_IL_0000;
					}
					p.ekzxl();
					p = default(kpthf);
					end_IL_0000:;
				}
				catch (Exception p6)
				{
					hgmpz = -2;
					bnxue.iurqb(p6);
					return;
				}
				hgmpz = -2;
				bnxue.vjftv();
			}

			void fgyyk.tkrrn()
			{
				//ILSpy generated this explicit interface implementation from .override directive in pqbro
				this.pqbro();
			}

			private void bfjnv(fgyyk p0)
			{
				bnxue.hdlij(p0);
			}

			void fgyyk.nrgxk(fgyyk p0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in bfjnv
				this.bfjnv(p0);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		private struct _003C_003CAlert_003Eb__53_003Ed__5b : fgyyk
		{
			public int alehe;

			public ljmxa eblfz;

			public uxmyo lyhwf;

			public Exception fettd;

			private kpthf fexxe;

			private object drqzi;

			private xuwyj<Exception> dwbrp;

			private void dhbzq()
			{
				try
				{
					bool flag = true;
					kpthf p2;
					xuwyj<Exception> p;
					switch (alehe)
					{
					default:
						p2 = lyhwf.tzpfq.szxmn().avdby(p1: false).vrtmi();
						if (!p2.zpafv || 1 == 0)
						{
							alehe = 0;
							fexxe = p2;
							eblfz.wqiyk(ref p2, ref this);
							flag = false;
							return;
						}
						goto IL_008f;
					case 0:
						p2 = fexxe;
						fexxe = default(kpthf);
						alehe = -1;
						goto IL_008f;
					case 1:
						{
							p = dwbrp;
							dwbrp = default(xuwyj<Exception>);
							alehe = -1;
							break;
						}
						IL_008f:
						p2.ekzxl();
						p2 = default(kpthf);
						if (((!(fettd is SocketException) || 1 == 0) && !(fettd is ObjectDisposedException)) || !lyhwf.lbtgz)
						{
							p = lyhwf.tzpfq.fptsn(fettd, "Error while sending alert packet").giftg(p1: false).vuozn();
							if (!p.hqxbj || 1 == 0)
							{
								alehe = 1;
								dwbrp = p;
								eblfz.wqiyk(ref p, ref this);
								flag = false;
								return;
							}
							break;
						}
						goto end_IL_0000;
					}
					Exception ex = p.gbccf();
					p = default(xuwyj<Exception>);
					throw ex;
					end_IL_0000:;
				}
				catch (Exception p3)
				{
					alehe = -2;
					eblfz.iurqb(p3);
					return;
				}
				alehe = -2;
				eblfz.vjftv();
			}

			void fgyyk.tkrrn()
			{
				//ILSpy generated this explicit interface implementation from .override directive in dhbzq
				this.dhbzq();
			}

			private void lyyrl(fgyyk p0)
			{
				eblfz.hdlij(p0);
			}

			void fgyyk.nrgxk(fgyyk p0)
			{
				//ILSpy generated this explicit interface implementation from .override directive in lyyrl
				this.lyyrl(p0);
			}
		}

		public zppmb arbla;

		public byte[] tyyci;

		public bool lbtgz;

		public ortbm tzpfq;

		public bool ejuby;

		[vtsnh(typeof(_003C_003CAlert_003Eb__52_003Ed__56))]
		public exkzi izyuz()
		{
			_003C_003CAlert_003Eb__52_003Ed__56 p = default(_003C_003CAlert_003Eb__52_003Ed__56);
			p.wideu = this;
			p.bnxue = ljmxa.nmskg();
			p.hgmpz = -1;
			ljmxa bnxue = p.bnxue;
			bnxue.nncuo(ref p);
			return p.bnxue.donjp;
		}

		[vtsnh(typeof(_003C_003CAlert_003Eb__53_003Ed__5b))]
		public exkzi uervl(Exception p0)
		{
			_003C_003CAlert_003Eb__53_003Ed__5b p1 = default(_003C_003CAlert_003Eb__53_003Ed__5b);
			p1.lyhwf = this;
			p1.fettd = p0;
			p1.eblfz = ljmxa.nmskg();
			p1.alehe = -1;
			ljmxa eblfz = p1.eblfz;
			eblfz.nncuo(ref p1);
			return p1.eblfz.donjp;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CAlert_003Ed__5f : fgyyk
	{
		public int lmpta;

		public ljmxa xijri;

		public ortbm bvpva;

		public byte yqfol;

		public byte cegah;

		public bool egtlq;

		public bool hcpls;

		public uxmyo vwbye;

		public IDisposable aauyb;

		private xuwyj<IDisposable> agjtd;

		private object reagb;

		private kpthf waflq;

		private void vucwr()
		{
			try
			{
				bool flag = true;
				IDisposable disposable;
				xuwyj<IDisposable> p2;
				IDisposable disposable2;
				kpthf p;
				switch (lmpta)
				{
				default:
					vwbye = new uxmyo();
					vwbye.ejuby = egtlq;
					vwbye.tzpfq = bvpva;
					vwbye.arbla = new zppmb(yqfol, cegah);
					vwbye.tyyci = vwbye.arbla.szrqi();
					hcpls = yqfol == 2;
					vwbye.lbtgz = (hcpls ? true : false) || cegah == 0;
					if (bvpva.rognc && 0 == 0)
					{
						if (!vwbye.ejuby || 1 == 0)
						{
							p2 = bvpva.apyzr.uivze().giftg(p1: false).vuozn();
							if (!p2.hqxbj || 1 == 0)
							{
								lmpta = 0;
								agjtd = p2;
								xijri.wqiyk(ref p2, ref this);
								flag = false;
								return;
							}
							goto IL_0159;
						}
						vwbye.lbtgz = false;
					}
					bvpva.bvdcq((vwbye.arbla.bgpwy ? true : false) ? LogLevel.Info : LogLevel.Debug, "{0} was sent.", vwbye.arbla);
					bvpva.oclcs.kxlvu(TlsDebugEventType.Alert, TlsDebugEventSource.Sent, TlsDebugLevel.Important, vwbye.tyyci, 0, vwbye.tyyci.Length);
					if (vwbye.lbtgz && 0 == 0)
					{
						bvpva.exyau();
					}
					p = zvcde.qlzna(vwbye.izyuz, vwbye.uervl).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						lmpta = 1;
						waflq = p;
						xijri.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
				case 0:
					p2 = agjtd;
					agjtd = default(xuwyj<IDisposable>);
					lmpta = -1;
					goto IL_0159;
				case 1:
					{
						p = waflq;
						waflq = default(kpthf);
						lmpta = -1;
						break;
					}
					IL_0159:
					disposable = p2.gbccf();
					p2 = default(xuwyj<IDisposable>);
					disposable2 = disposable;
					aauyb = disposable2;
					try
					{
						bvpva.lptja = hlkgm.nzcmy;
					}
					finally
					{
						if (flag && 0 == 0 && aauyb != null && 0 == 0)
						{
							aauyb.Dispose();
						}
					}
					goto end_IL_0000;
				}
				p.ekzxl();
				p = default(kpthf);
				end_IL_0000:;
			}
			catch (Exception p3)
			{
				lmpta = -2;
				xijri.iurqb(p3);
				return;
			}
			lmpta = -2;
			xijri.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in vucwr
			this.vucwr();
		}

		private void rocfq(fgyyk p0)
		{
			xijri.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in rocfq
			this.rocfq(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003C_003CSend_003Eb__65_003Ed__69 : fgyyk
	{
		public int tbjrm;

		public ljmxa iukii;

		public ortbm ebjwy;

		private kpthf kziux;

		private object bdqfi;

		private void qzilh()
		{
			try
			{
				bool flag = true;
				kpthf p;
				if (tbjrm != 0)
				{
					p = ebjwy.zupbs().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						tbjrm = 0;
						kziux = p;
						iukii.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = kziux;
					kziux = default(kpthf);
					tbjrm = -1;
				}
				p.ekzxl();
				p = default(kpthf);
			}
			catch (Exception p2)
			{
				tbjrm = -2;
				iukii.iurqb(p2);
				return;
			}
			tbjrm = -2;
			iukii.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in qzilh
			this.qzilh();
		}

		private void fmexu(fgyyk p0)
		{
			iukii.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in fmexu
			this.fmexu(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003C_003CSend_003Eb__66_003Ed__6c : fgyyk
	{
		public int nbvzz;

		public ljmxa nauzz;

		public ortbm ysvdf;

		public Exception zfdjd;

		private xuwyj<Exception> xfils;

		private object urbpn;

		private void ctffc()
		{
			try
			{
				bool flag = true;
				xuwyj<Exception> p;
				if (nbvzz != 0)
				{
					p = ysvdf.fptsn(zfdjd, "Error while sending data over TLS").giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						nbvzz = 0;
						xfils = p;
						nauzz.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = xfils;
					xfils = default(xuwyj<Exception>);
					nbvzz = -1;
				}
				Exception ex = p.gbccf();
				p = default(xuwyj<Exception>);
				throw ex;
			}
			catch (Exception p2)
			{
				nbvzz = -2;
				nauzz.iurqb(p2);
			}
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ctffc
			this.ctffc();
		}

		private void ccgcx(fgyyk p0)
		{
			nauzz.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ccgcx
			this.ccgcx(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CSend_003Ed__6f : fgyyk
	{
		public int trdup;

		public ljmxa tvrvt;

		public ortbm yjavt;

		public byte[] uhecb;

		public int rwtvq;

		public int ujqor;

		public bool ulrwr;

		public Func<exkzi> wvbxq;

		public Func<Exception, exkzi> dnalq;

		public IDisposable tdkiv;

		private xuwyj<IDisposable> yrucv;

		private object kdinl;

		public IDisposable tpunr;

		private kpthf mizhx;

		public IDisposable xgaaw;

		private void vwvxn()
		{
			try
			{
				bool flag = true;
				IDisposable disposable;
				xuwyj<IDisposable> p;
				IDisposable disposable2;
				switch (trdup)
				{
				default:
					wvbxq = null;
					dnalq = null;
					yjavt.xpoxa.qtzjv(vhsmv);
					p = yjavt.tebsv.uivze().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						trdup = 0;
						yrucv = p;
						tvrvt.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_00c2;
				case 0:
					p = yrucv;
					yrucv = default(xuwyj<IDisposable>);
					trdup = -1;
					goto IL_00c2;
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
					break;
					IL_00c2:
					disposable = p.gbccf();
					p = default(xuwyj<IDisposable>);
					disposable2 = disposable;
					tdkiv = disposable2;
					break;
				}
				try
				{
					kpthf p5;
					IDisposable disposable3;
					xuwyj<IDisposable> p6;
					IDisposable disposable4;
					IDisposable disposable5;
					xuwyj<IDisposable> p4;
					IDisposable disposable6;
					kpthf p3;
					Func<exkzi> p7;
					kpthf p2;
					switch (trdup)
					{
					default:
						p6 = yjavt.apyzr.uivze().giftg(p1: false).vuozn();
						if (!p6.hqxbj || 1 == 0)
						{
							trdup = 1;
							yrucv = p6;
							tvrvt.wqiyk(ref p6, ref this);
							flag = false;
							return;
						}
						goto IL_017d;
					case 1:
						p6 = yrucv;
						yrucv = default(xuwyj<IDisposable>);
						trdup = -1;
						goto IL_017d;
					case 2:
						p5 = mizhx;
						mizhx = default(kpthf);
						trdup = -1;
						goto IL_027b;
					case 3:
						p4 = yrucv;
						yrucv = default(xuwyj<IDisposable>);
						trdup = -1;
						goto IL_02ff;
					case 4:
						p3 = mizhx;
						mizhx = default(kpthf);
						trdup = -1;
						goto IL_03e9;
					case 5:
						{
							p2 = mizhx;
							mizhx = default(kpthf);
							trdup = -1;
							break;
						}
						IL_027b:
						p5.ekzxl();
						p5 = default(kpthf);
						p4 = yjavt.apyzr.uivze().giftg(p1: false).vuozn();
						if (!p4.hqxbj || 1 == 0)
						{
							trdup = 3;
							yrucv = p4;
							tvrvt.wqiyk(ref p4, ref this);
							flag = false;
							return;
						}
						goto IL_02ff;
						IL_017d:
						disposable3 = p6.gbccf();
						p6 = default(xuwyj<IDisposable>);
						disposable4 = disposable3;
						tpunr = disposable4;
						try
						{
							if (yjavt.lptja == hlkgm.nzcmy || 1 == 0)
							{
								throw new TlsException("Socket has not been secured yet.");
							}
							if (yjavt.lptja == hlkgm.tqqib)
							{
								throw new InvalidOperationException("Socket was closed.");
							}
							ulrwr = yjavt.qteas;
						}
						finally
						{
							if (flag && 0 == 0 && tpunr != null && 0 == 0)
							{
								tpunr.Dispose();
							}
						}
						goto IL_0352;
						IL_0352:
						if (ulrwr ? true : false)
						{
							p5 = rxpjc.vaukn(1).avdby(p1: false).vrtmi();
							if (!p5.zpafv || 1 == 0)
							{
								trdup = 2;
								mizhx = p5;
								tvrvt.wqiyk(ref p5, ref this);
								flag = false;
								return;
							}
							goto IL_027b;
						}
						p3 = yjavt.kjdzy(vcedo.idfcl, uhecb, rwtvq, ujqor).avdby(p1: false).vrtmi();
						if (!p3.zpafv || 1 == 0)
						{
							trdup = 4;
							mizhx = p3;
							tvrvt.wqiyk(ref p3, ref this);
							flag = false;
							return;
						}
						goto IL_03e9;
						IL_02ff:
						disposable5 = p4.gbccf();
						p4 = default(xuwyj<IDisposable>);
						disposable6 = disposable5;
						xgaaw = disposable6;
						try
						{
							ulrwr = yjavt.qteas;
						}
						finally
						{
							if (flag && 0 == 0 && xgaaw != null && 0 == 0)
							{
								xgaaw.Dispose();
							}
						}
						goto IL_0352;
						IL_03e9:
						p3.ekzxl();
						p3 = default(kpthf);
						if (wvbxq == null || 1 == 0)
						{
							wvbxq = yjavt.yrfdi;
						}
						p7 = wvbxq;
						if (dnalq == null || 1 == 0)
						{
							dnalq = yjavt.elfgo;
						}
						p2 = p7.qlzna(dnalq).avdby(p1: false).vrtmi();
						if (!p2.zpafv || 1 == 0)
						{
							trdup = 5;
							mizhx = p2;
							tvrvt.wqiyk(ref p2, ref this);
							flag = false;
							return;
						}
						break;
					}
					p2.ekzxl();
					p2 = default(kpthf);
				}
				finally
				{
					if (flag && 0 == 0 && tdkiv != null && 0 == 0)
					{
						tdkiv.Dispose();
					}
				}
			}
			catch (Exception p8)
			{
				trdup = -2;
				tvrvt.iurqb(p8);
				return;
			}
			trdup = -2;
			tvrvt.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in vwvxn
			this.vwvxn();
		}

		private void kebgs(fgyyk p0)
		{
			tvrvt.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in kebgs
			this.kebgs(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003C_003CReceive_003Eb__77_003Ed__79 : fgyyk
	{
		public int khizg;

		public ljmxa uiwtx;

		public ortbm jzbrt;

		public IDisposable qqcdi;

		private xuwyj<IDisposable> sunqv;

		private object cdpoe;

		private void toxzg()
		{
			try
			{
				bool flag = true;
				xuwyj<IDisposable> p;
				if (khizg != 0)
				{
					p = jzbrt.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						khizg = 0;
						sunqv = p;
						uiwtx.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = sunqv;
					sunqv = default(xuwyj<IDisposable>);
					khizg = -1;
				}
				IDisposable disposable = p.gbccf();
				p = default(xuwyj<IDisposable>);
				IDisposable disposable2 = disposable;
				qqcdi = disposable2;
				try
				{
					jzbrt.wqwju = false;
				}
				finally
				{
					if (flag && 0 == 0 && qqcdi != null && 0 == 0)
					{
						qqcdi.Dispose();
					}
				}
			}
			catch (Exception p2)
			{
				khizg = -2;
				uiwtx.iurqb(p2);
				return;
			}
			khizg = -2;
			uiwtx.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in toxzg
			this.toxzg();
		}

		private void rqiah(fgyyk p0)
		{
			uiwtx.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in rqiah
			this.rqiah(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CReceive_003Ed__7d : fgyyk
	{
		public int cdkcr;

		public vxvbw<int> cubwf;

		public ortbm jcjvx;

		public byte[] ysuqg;

		public int cudjb;

		public int myeml;

		public bool lginc;

		public Func<exkzi> hetll;

		public IDisposable xmyie;

		private xuwyj<IDisposable> ofkko;

		private object pwiuc;

		public IDisposable jliuz;

		private xuwyj<int> vvdax;

		private void zconj()
		{
			int p4;
			try
			{
				bool flag = true;
				IDisposable disposable;
				xuwyj<IDisposable> p;
				IDisposable disposable2;
				switch (cdkcr)
				{
				default:
					hetll = null;
					jcjvx.xpoxa.uggrs(vhsmv);
					p = jcjvx.kdjaj.uivze().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						cdkcr = 0;
						ofkko = p;
						cubwf.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_00b0;
				case 0:
					p = ofkko;
					ofkko = default(xuwyj<IDisposable>);
					cdkcr = -1;
					goto IL_00b0;
				case 1:
				case 2:
					break;
					IL_00b0:
					disposable = p.gbccf();
					p = default(xuwyj<IDisposable>);
					disposable2 = disposable;
					xmyie = disposable2;
					break;
				}
				try
				{
					IDisposable disposable3;
					xuwyj<IDisposable> p3;
					IDisposable disposable4;
					njvzu<int> p5;
					xuwyj<int> p2;
					switch (cdkcr)
					{
					default:
						p3 = jcjvx.apyzr.uivze().giftg(p1: false).vuozn();
						if (!p3.hqxbj || 1 == 0)
						{
							cdkcr = 1;
							ofkko = p3;
							cubwf.xiwgo(ref p3, ref this);
							flag = false;
							return;
						}
						goto IL_015f;
					case 1:
						p3 = ofkko;
						ofkko = default(xuwyj<IDisposable>);
						cdkcr = -1;
						goto IL_015f;
					case 2:
						{
							p2 = vvdax;
							vvdax = default(xuwyj<int>);
							cdkcr = -1;
							break;
						}
						IL_015f:
						disposable3 = p3.gbccf();
						p3 = default(xuwyj<IDisposable>);
						disposable4 = disposable3;
						jliuz = disposable4;
						try
						{
							if (jcjvx.lptja == hlkgm.nzcmy || 1 == 0)
							{
								throw new TlsException("Socket has not been secured yet.");
							}
							if (jcjvx.quhdu.Length != 0 || jcjvx.lptja != hlkgm.tqqib)
							{
								goto end_IL_0178;
							}
							p4 = 0;
							goto end_IL_00d1;
							end_IL_0178:;
						}
						finally
						{
							if (flag && 0 == 0 && jliuz != null && 0 == 0)
							{
								jliuz.Dispose();
							}
						}
						p5 = jcjvx.ywwoe(ysuqg, cudjb, myeml, lginc);
						if (hetll == null || 1 == 0)
						{
							hetll = jcjvx.ucupl;
						}
						p2 = p5.tctma(hetll).giftg(p1: false).vuozn();
						if (!p2.hqxbj || 1 == 0)
						{
							cdkcr = 2;
							vvdax = p2;
							cubwf.xiwgo(ref p2, ref this);
							flag = false;
							return;
						}
						break;
					}
					int num = p2.gbccf();
					p2 = default(xuwyj<int>);
					p4 = num;
					end_IL_00d1:;
				}
				finally
				{
					if (flag && 0 == 0 && xmyie != null && 0 == 0)
					{
						xmyie.Dispose();
					}
				}
			}
			catch (Exception p6)
			{
				cdkcr = -2;
				cubwf.tudwl(p6);
				return;
			}
			cdkcr = -2;
			cubwf.vzyck(p4);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in zconj
			this.zconj();
		}

		private void qsklu(fgyyk p0)
		{
			cubwf.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in qsklu
			this.qsklu(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CreceiveInnerAsync_003Ed__83 : fgyyk
	{
		public int lkthh;

		public vxvbw<int> uwhfz;

		public ortbm xlcxb;

		public byte[] qpvke;

		public int czcub;

		public int vpsze;

		public bool znrqt;

		public int zbirs;

		public IDisposable rnxfr;

		private xuwyj<IDisposable> qyrwh;

		private object ftotw;

		private kpthf sutla;

		private void qetqs()
		{
			int p3;
			try
			{
				bool flag = true;
				kpthf p;
				IDisposable disposable;
				xuwyj<IDisposable> p2;
				IDisposable disposable2;
				switch (lkthh)
				{
				default:
					p2 = xlcxb.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p2.hqxbj || 1 == 0)
					{
						lkthh = 0;
						qyrwh = p2;
						uwhfz.xiwgo(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_0090;
				case 0:
					p2 = qyrwh;
					qyrwh = default(xuwyj<IDisposable>);
					lkthh = -1;
					goto IL_0090;
				case 1:
					{
						p = sutla;
						sutla = default(kpthf);
						lkthh = -1;
						goto IL_0172;
					}
					IL_0172:
					p.ekzxl();
					p = default(kpthf);
					if (znrqt && 0 == 0)
					{
						break;
					}
					goto default;
					IL_0090:
					disposable = p2.gbccf();
					p2 = default(xuwyj<IDisposable>);
					disposable2 = disposable;
					rnxfr = disposable2;
					try
					{
						if (xlcxb.quhdu.Length > 0)
						{
							goto end_IL_00a9;
						}
						if (xlcxb.lptja != hlkgm.tqqib)
						{
							goto IL_0102;
						}
						p3 = 0;
						goto end_IL_0000;
						end_IL_00a9:;
					}
					finally
					{
						if (flag && 0 == 0 && rnxfr != null && 0 == 0)
						{
							rnxfr.Dispose();
						}
					}
					break;
					IL_0102:
					p = xlcxb.dnapo().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						lkthh = 1;
						sutla = p;
						uwhfz.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_0172;
				}
				zbirs = Math.Min(vpsze, (int)xlcxb.quhdu.Length);
				Array.Copy(xlcxb.quhdu.GetBuffer(), 0, qpvke, czcub, zbirs);
				xlcxb.quhdu.ejbiu(zbirs);
				p3 = zbirs;
				end_IL_0000:;
			}
			catch (Exception p4)
			{
				lkthh = -2;
				uwhfz.tudwl(p4);
				return;
			}
			lkthh = -2;
			uwhfz.vzyck(p3);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in qetqs
			this.qetqs();
		}

		private void hvjaf(fgyyk p0)
		{
			uwhfz.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in hvjaf
			this.hvjaf(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CNegotiate_003Ed__89 : fgyyk
	{
		public int xgbmv;

		public ljmxa dinfl;

		public ortbm qtrkn;

		public IDisposable oyoii;

		private xuwyj<IDisposable> scedo;

		private object blhmj;

		private kpthf ajyac;

		private void tclhg()
		{
			try
			{
				bool flag = true;
				kpthf p2;
				IDisposable disposable;
				xuwyj<IDisposable> p3;
				IDisposable disposable2;
				kpthf p;
				switch (xgbmv)
				{
				default:
					p3 = qtrkn.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p3.hqxbj || 1 == 0)
					{
						xgbmv = 0;
						scedo = p3;
						dinfl.wqiyk(ref p3, ref this);
						flag = false;
						return;
					}
					goto IL_0094;
				case 0:
					p3 = scedo;
					scedo = default(xuwyj<IDisposable>);
					xgbmv = -1;
					goto IL_0094;
				case 1:
					p2 = ajyac;
					ajyac = default(kpthf);
					xgbmv = -1;
					goto IL_019a;
				case 2:
					{
						p = ajyac;
						ajyac = default(kpthf);
						xgbmv = -1;
						goto IL_021e;
					}
					IL_019a:
					p2.ekzxl();
					p2 = default(kpthf);
					goto IL_02aa;
					IL_0094:
					disposable = p3.gbccf();
					p3 = default(xuwyj<IDisposable>);
					disposable2 = disposable;
					oyoii = disposable2;
					try
					{
						if (!qtrkn.qteas)
						{
							qtrkn.qteas = true;
							goto IL_00fc;
						}
					}
					finally
					{
						if (flag && 0 == 0 && oyoii != null && 0 == 0)
						{
							oyoii.Dispose();
						}
					}
					goto end_IL_0000;
					IL_00fc:
					if ((qtrkn.exdje.Version & TlsVersion.SSL30) != TlsVersion.None && 0 == 0)
					{
						qtrkn.mnuoq(LogLevel.Info, "Warning: SSL 3.0 has been deprecated. According to RFC 7568, it must no longer be used.");
					}
					p2 = qtrkn.jfwrj().avdby(p1: false).vrtmi();
					if (!p2.zpafv || 1 == 0)
					{
						xgbmv = 1;
						ajyac = p2;
						dinfl.wqiyk(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_019a;
					IL_02aa:
					if (!qtrkn.qteas || 1 == 0)
					{
						break;
					}
					p = qtrkn.dnapo().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						xgbmv = 2;
						ajyac = p;
						dinfl.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_021e;
					IL_021e:
					p.ekzxl();
					p = default(kpthf);
					if (qtrkn.lptja == hlkgm.tqqib)
					{
						if ((qtrkn.exdje.Options & TlsOptions.AllowCloseWhileNegotiating) != TlsOptions.None)
						{
							qtrkn.qteas = false;
							break;
						}
						qtrkn.mnuoq(LogLevel.Debug, "TLS socket closed while negotiation was in progress.");
						string text = "Connection was closed by the remote connection end.";
						if (qtrkn.exdje.Version == TlsVersion.SSL30)
						{
							text += " Try enabling TLS 1.2, 1.1 or 1.0 instead of legacy SSL 3.0.";
						}
						throw new TlsException(text, NetworkSessionExceptionStatus.ConnectionClosed);
					}
					goto IL_02aa;
				}
				yelnx xpoxa = qtrkn.xpoxa;
				bool svsgt = (qtrkn.xpoxa.rtcbh = true);
				xpoxa.svsgt = svsgt;
				end_IL_0000:;
			}
			catch (Exception p4)
			{
				xgbmv = -2;
				dinfl.iurqb(p4);
				return;
			}
			xgbmv = -2;
			dinfl.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in tclhg
			this.tclhg();
		}

		private void rgaxa(fgyyk p0)
		{
			dinfl.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in rgaxa
			this.rgaxa(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CUnprotect_003Ed__8e : fgyyk
	{
		public int swvhd;

		public ljmxa xudqy;

		public ortbm lpteh;

		public IDisposable bgwgn;

		private xuwyj<IDisposable> ntnyg;

		private object eongt;

		public IDisposable zufbz;

		private kpthf dmriq;

		private void btxvn()
		{
			try
			{
				bool flag = true;
				kpthf p2;
				IDisposable disposable;
				xuwyj<IDisposable> p4;
				IDisposable disposable2;
				kpthf p;
				IDisposable disposable3;
				xuwyj<IDisposable> p3;
				IDisposable disposable4;
				switch (swvhd)
				{
				default:
					p4 = lpteh.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p4.hqxbj || 1 == 0)
					{
						swvhd = 0;
						ntnyg = p4;
						xudqy.wqiyk(ref p4, ref this);
						flag = false;
						return;
					}
					goto IL_0097;
				case 0:
					p4 = ntnyg;
					ntnyg = default(xuwyj<IDisposable>);
					swvhd = -1;
					goto IL_0097;
				case 1:
					p3 = ntnyg;
					ntnyg = default(xuwyj<IDisposable>);
					swvhd = -1;
					goto IL_01fc;
				case 2:
					p2 = dmriq;
					dmriq = default(kpthf);
					swvhd = -1;
					goto IL_02c2;
				case 3:
					{
						p = dmriq;
						dmriq = default(kpthf);
						swvhd = -1;
						goto IL_0346;
					}
					IL_02c2:
					p2.ekzxl();
					p2 = default(kpthf);
					goto IL_0355;
					IL_0097:
					disposable = p4.gbccf();
					p4 = default(xuwyj<IDisposable>);
					disposable2 = disposable;
					bgwgn = disposable2;
					try
					{
						if ((lpteh.qteas ? true : false) || lpteh.rognc)
						{
							throw new TlsException("Silent unprotect is not possible at the moment, because sending or receiving is in progress.");
						}
						if (lpteh.lptja != hlkgm.rhxxi)
						{
							break;
						}
						lpteh.rognc = true;
						goto IL_012f;
					}
					finally
					{
						if (flag && 0 == 0 && bgwgn != null && 0 == 0)
						{
							bgwgn.Dispose();
						}
					}
					IL_012f:
					if ((lpteh.exdje.Options & TlsOptions.SilentUnprotect) != TlsOptions.None && 0 == 0)
					{
						if ((lpteh.wqwju ? true : false) || lpteh.bxowl)
						{
							throw new TlsException("Silent unprotect is not possible at the moment, because sending or receiving is in progress.");
						}
						p3 = lpteh.apyzr.uivze().giftg(p1: false).vuozn();
						if (!p3.hqxbj || 1 == 0)
						{
							swvhd = 1;
							ntnyg = p3;
							xudqy.wqiyk(ref p3, ref this);
							flag = false;
							return;
						}
						goto IL_01fc;
					}
					p2 = lpteh.ebnci(1, 0, p2: true).avdby(p1: false).vrtmi();
					if (!p2.zpafv || 1 == 0)
					{
						swvhd = 2;
						dmriq = p2;
						xudqy.wqiyk(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_02c2;
					IL_0346:
					p.ekzxl();
					p = default(kpthf);
					goto IL_0355;
					IL_0355:
					if (lpteh.lptja == hlkgm.rhxxi)
					{
						p = lpteh.dnapo().avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							swvhd = 3;
							dmriq = p;
							xudqy.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
						goto IL_0346;
					}
					if (lpteh.lptja != hlkgm.tqqib)
					{
						break;
					}
					throw new TlsException("Connection was closed by the remote connection end.", NetworkSessionExceptionStatus.ConnectionClosed);
					IL_01fc:
					disposable3 = p3.gbccf();
					p3 = default(xuwyj<IDisposable>);
					disposable4 = disposable3;
					zufbz = disposable4;
					try
					{
						lpteh.lptja = hlkgm.tqqib;
					}
					finally
					{
						if (flag && 0 == 0 && zufbz != null && 0 == 0)
						{
							zufbz.Dispose();
						}
					}
					break;
				}
			}
			catch (Exception p5)
			{
				swvhd = -2;
				xudqy.iurqb(p5);
				return;
			}
			swvhd = -2;
			xudqy.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in btxvn
			this.btxvn();
		}

		private void subbx(fgyyk p0)
		{
			xudqy.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in subbx
			this.subbx(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CClose_003Ed__94 : fgyyk
	{
		public int wzfvl;

		public vxvbw<Exception> uhczw;

		public ortbm wbngj;

		public Exception tvdez;

		public string sqzil;

		public bool kjrlw;

		public TlsException lyokl;

		public SocketException fczwn;

		public int ytedk;

		public byte iayxi;

		public IDisposable yfbgb;

		private xuwyj<IDisposable> lpabv;

		private object hxahn;

		private kpthf ltucm;

		private void xhwao()
		{
			Exception p4;
			try
			{
				bool flag = true;
				kpthf p;
				IDisposable disposable;
				xuwyj<IDisposable> p2;
				IDisposable disposable2;
				switch (wzfvl)
				{
				default:
					if (sqzil != null && 0 == 0)
					{
						wbngj.bvdcq(LogLevel.Debug, sqzil + ": {0}", tvdez);
					}
					else
					{
						wbngj.mnuoq(LogLevel.Debug, tvdez.ToString());
					}
					kjrlw = true;
					lyokl = tvdez as TlsException;
					fczwn = tvdez as SocketException;
					ytedk = ((fczwn != null && 0 == 0) ? fczwn.skehp() : 0);
					if (ytedk == 10038 || ytedk == 10053 || ytedk == 10054)
					{
						lyokl = new TlsException("Connection was closed by the remote connection end.", NetworkSessionExceptionStatus.ConnectionClosed, tvdez);
						kjrlw = false;
						tvdez = lyokl;
					}
					if (wbngj.lptja == hlkgm.tqqib)
					{
						break;
					}
					p2 = wbngj.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p2.hqxbj || 1 == 0)
					{
						wzfvl = 0;
						lpabv = p2;
						uhczw.xiwgo(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_01a0;
				case 0:
					p2 = lpabv;
					lpabv = default(xuwyj<IDisposable>);
					wzfvl = -1;
					goto IL_01a0;
				case 1:
					p = ltucm;
					ltucm = default(kpthf);
					wzfvl = -1;
					goto IL_02b7;
				case 2:
					{
						try
						{
							int num = wzfvl;
							kpthf p3;
							if (num != 2)
							{
								p3 = wbngj.ebnci(2, iayxi, p2: true).avdby(p1: false).vrtmi();
								if (!p3.zpafv || 1 == 0)
								{
									wzfvl = 2;
									ltucm = p3;
									uhczw.xiwgo(ref p3, ref this);
									flag = false;
									return;
								}
							}
							else
							{
								p3 = ltucm;
								ltucm = default(kpthf);
								wzfvl = -1;
							}
							p3.ekzxl();
							p3 = default(kpthf);
						}
						catch
						{
						}
						break;
					}
					IL_02b7:
					p.ekzxl();
					p = default(kpthf);
					break;
					IL_01a0:
					disposable = p2.gbccf();
					p2 = default(xuwyj<IDisposable>);
					disposable2 = disposable;
					yfbgb = disposable2;
					try
					{
						wbngj.lptja = hlkgm.tqqib;
					}
					finally
					{
						if (flag && 0 == 0 && yfbgb != null && 0 == 0)
						{
							yfbgb.Dispose();
						}
					}
					if ((lyokl != null && 0 == 0 && (lyokl.dipvo ? true : false)) || (todgf.hudgy(tvdez) ? true : false) || wbngj.luosf.rnqba)
					{
						p = wbngj.szxmn().avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							wzfvl = 1;
							ltucm = p;
							uhczw.xiwgo(ref p, ref this);
							flag = false;
							return;
						}
						goto IL_02b7;
					}
					if (lyokl != null && 0 == 0 && lyokl.rkbch != null)
					{
						iayxi = lyokl.rkbch.jmwmm;
					}
					else
					{
						iayxi = 80;
					}
					goto case 2;
				}
				wbngj.exyau();
				p4 = ((lyokl == null) ? new TlsException(rtzwv.iogyt, mjddr.qssln, tvdez.Message, tvdez) : ((!kjrlw) ? lyokl : TlsException.yxoes(lyokl)));
			}
			catch (Exception p5)
			{
				wzfvl = -2;
				uhczw.tudwl(p5);
				return;
			}
			wzfvl = -2;
			uhczw.vzyck(p4);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in xhwao
			this.xhwao();
		}

		private void bxzse(fgyyk p0)
		{
			uhczw.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in bxzse
			this.bxzse(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CCloseSocket_003Ed__9e : fgyyk
	{
		public int snowf;

		public ljmxa cldgz;

		public ortbm pqdiz;

		public IDisposable gnujp;

		private xuwyj<IDisposable> oewgs;

		private object hnysq;

		private kpthf byjfo;

		private void axpkx()
		{
			try
			{
				bool flag = true;
				IDisposable disposable;
				xuwyj<IDisposable> p;
				IDisposable disposable2;
				switch (snowf)
				{
				default:
					p = pqdiz.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p.hqxbj || 1 == 0)
					{
						snowf = 0;
						oewgs = p;
						cldgz.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_008f;
				case 0:
					p = oewgs;
					oewgs = default(xuwyj<IDisposable>);
					snowf = -1;
					goto IL_008f;
				case 1:
					break;
					IL_008f:
					disposable = p.gbccf();
					p = default(xuwyj<IDisposable>);
					disposable2 = disposable;
					gnujp = disposable2;
					break;
				}
				try
				{
					int num = snowf;
					if (num != 1)
					{
						pqdiz.lptja = hlkgm.tqqib;
						pqdiz.xpoxa.ngryw();
					}
					try
					{
						int num2 = snowf;
						kpthf p2;
						if (num2 != 1)
						{
							p2 = pqdiz.bguay.jhbpr().avdby(p1: false).vrtmi();
							if (!p2.zpafv || 1 == 0)
							{
								snowf = 1;
								byjfo = p2;
								cldgz.wqiyk(ref p2, ref this);
								flag = false;
								return;
							}
						}
						else
						{
							p2 = byjfo;
							byjfo = default(kpthf);
							snowf = -1;
						}
						p2.ekzxl();
						p2 = default(kpthf);
						if (pqdiz.qjbzn != null && 0 == 0)
						{
							pqdiz.qjbzn.egphd();
							pqdiz.qjbzn = null;
						}
						if (pqdiz.tjjru != null && 0 == 0)
						{
							pqdiz.tjjru.egphd();
							pqdiz.tjjru = null;
						}
						if (pqdiz.uljiq != null && 0 == 0)
						{
							pqdiz.uljiq.egphd();
							pqdiz.uljiq = null;
						}
						if (pqdiz.ebitx != null && 0 == 0)
						{
							pqdiz.ebitx.egphd();
							pqdiz.ebitx = null;
						}
					}
					catch
					{
					}
				}
				finally
				{
					if (flag && 0 == 0 && gnujp != null && 0 == 0)
					{
						gnujp.Dispose();
					}
				}
			}
			catch (Exception p3)
			{
				snowf = -2;
				cldgz.iurqb(p3);
				return;
			}
			snowf = -2;
			cldgz.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in axpkx
			this.axpkx();
		}

		private void bqhdw(fgyyk p0)
		{
			cldgz.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in bqhdw
			this.bqhdw(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003C_003CClose_003Eb__a3_003Ed__a4 : fgyyk
	{
		public int usmhk;

		public ljmxa xutev;

		public ortbm gtqmq;

		private kpthf opdux;

		private object orbnt;

		private void ivitj()
		{
			try
			{
				bool flag = true;
				kpthf p;
				if (usmhk != 0)
				{
					p = zvcde.jrrmx(gtqmq.szxmn).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						usmhk = 0;
						opdux = p;
						xutev.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = opdux;
					opdux = default(kpthf);
					usmhk = -1;
				}
				p.ekzxl();
				p = default(kpthf);
				gtqmq.xpoxa.ltanj();
				gtqmq.xpoxa.azkaz();
			}
			catch (Exception p2)
			{
				usmhk = -2;
				xutev.iurqb(p2);
				return;
			}
			usmhk = -2;
			xutev.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ivitj
			this.ivitj();
		}

		private void taqzz(fgyyk p0)
		{
			xutev.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in taqzz
			this.taqzz(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CClose_003Ed__a7 : fgyyk
	{
		public int dqajn;

		public ljmxa zxzxg;

		public ortbm nuaoe;

		public hlkgm imppr;

		public IDisposable rkhyd;

		private xuwyj<IDisposable> fxwdw;

		private object wmgpu;

		private kpthf aeloo;

		public IDisposable mxeby;

		private void lyqik()
		{
			try
			{
				bool flag = true;
				IDisposable disposable;
				xuwyj<IDisposable> p2;
				IDisposable disposable2;
				IDisposable disposable3;
				xuwyj<IDisposable> p4;
				IDisposable disposable4;
				kpthf p3;
				kpthf p;
				switch (dqajn)
				{
				default:
					nuaoe.mnuoq(LogLevel.Debug, "Closing TLS socket.");
					p4 = nuaoe.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p4.hqxbj || 1 == 0)
					{
						dqajn = 0;
						fxwdw = p4;
						zxzxg.wqiyk(ref p4, ref this);
						flag = false;
						return;
					}
					goto IL_00a9;
				case 0:
					p4 = fxwdw;
					fxwdw = default(xuwyj<IDisposable>);
					dqajn = -1;
					goto IL_00a9;
				case 1:
					p3 = aeloo;
					aeloo = default(kpthf);
					dqajn = -1;
					goto IL_017a;
				case 2:
					p2 = fxwdw;
					fxwdw = default(xuwyj<IDisposable>);
					dqajn = -1;
					goto IL_0203;
				case 3:
					{
						p = aeloo;
						aeloo = default(kpthf);
						dqajn = -1;
						break;
					}
					IL_0203:
					disposable = p2.gbccf();
					p2 = default(xuwyj<IDisposable>);
					disposable2 = disposable;
					mxeby = disposable2;
					try
					{
						nuaoe.lptja = hlkgm.tqqib;
					}
					finally
					{
						if (flag && 0 == 0 && mxeby != null && 0 == 0)
						{
							mxeby.Dispose();
						}
					}
					p = nuaoe.jdlav().orims(nuaoe.lecpj).avdby(p1: false)
						.vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						dqajn = 3;
						aeloo = p;
						zxzxg.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
					IL_00a9:
					disposable3 = p4.gbccf();
					p4 = default(xuwyj<IDisposable>);
					disposable4 = disposable3;
					rkhyd = disposable4;
					try
					{
						imppr = nuaoe.lptja;
					}
					finally
					{
						if (flag && 0 == 0 && rkhyd != null && 0 == 0)
						{
							rkhyd.Dispose();
						}
					}
					if (imppr == hlkgm.tqqib)
					{
						p3 = nuaoe.szxmn().avdby(p1: false).vrtmi();
						if (!p3.zpafv || 1 == 0)
						{
							dqajn = 1;
							aeloo = p3;
							zxzxg.wqiyk(ref p3, ref this);
							flag = false;
							return;
						}
						goto IL_017a;
					}
					p2 = nuaoe.apyzr.uivze().giftg(p1: false).vuozn();
					if (!p2.hqxbj || 1 == 0)
					{
						dqajn = 2;
						fxwdw = p2;
						zxzxg.wqiyk(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_0203;
					IL_017a:
					p3.ekzxl();
					p3 = default(kpthf);
					goto end_IL_0000;
				}
				p.ekzxl();
				p = default(kpthf);
				end_IL_0000:;
			}
			catch (Exception p5)
			{
				dqajn = -2;
				zxzxg.iurqb(p5);
				return;
			}
			dqajn = -2;
			zxzxg.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in lyqik
			this.lyqik();
		}

		private void ytkhn(fgyyk p0)
		{
			zxzxg.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ytkhn
			this.ytkhn(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CShutdownAsync_003Ed__ae : fgyyk
	{
		public int geitg;

		public ljmxa ynseq;

		public ortbm tviks;

		private kpthf reria;

		private object nnclr;

		private void eqwgx()
		{
			try
			{
				bool flag = true;
				kpthf p;
				if (geitg != 0)
				{
					p = tviks.gpihd(SocketShutdown.Send).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						geitg = 0;
						reria = p;
						ynseq.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = reria;
					reria = default(kpthf);
					geitg = -1;
				}
				p.ekzxl();
				p = default(kpthf);
			}
			catch (Exception p2)
			{
				geitg = -2;
				ynseq.iurqb(p2);
				return;
			}
			geitg = -2;
			ynseq.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in eqwgx
			this.eqwgx();
		}

		private void ocvou(fgyyk p0)
		{
			ynseq.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ocvou
			this.ocvou(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CShutdownAsync_003Ed__b1 : fgyyk
	{
		public int xkikl;

		public ljmxa xwfej;

		public ortbm newqr;

		public SocketShutdown wylwu;

		private kpthf zrpff;

		private object bxxfm;

		private void tsisp()
		{
			try
			{
				bool flag = true;
				switch (xkikl)
				{
				default:
					if ((wylwu != SocketShutdown.Receive) ? true : false)
					{
						break;
					}
					newqr.xpoxa.rtcbh = false;
					goto end_IL_0000;
				case 0:
				case 1:
					break;
				}
				try
				{
					kpthf p2;
					kpthf p;
					switch (xkikl)
					{
					default:
						if (!newqr.xpoxa.geywj() || 1 == 0)
						{
							p2 = newqr.xpoxa.onwxy.avdby(p1: false).vrtmi();
							if (!p2.zpafv || 1 == 0)
							{
								xkikl = 0;
								zrpff = p2;
								xwfej.wqiyk(ref p2, ref this);
								flag = false;
								return;
							}
							goto IL_00ec;
						}
						p = newqr.kwvqt().avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							xkikl = 1;
							zrpff = p;
							xwfej.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
						break;
					case 0:
						p2 = zrpff;
						zrpff = default(kpthf);
						xkikl = -1;
						goto IL_00ec;
					case 1:
						{
							p = zrpff;
							zrpff = default(kpthf);
							xkikl = -1;
							break;
						}
						IL_00ec:
						p2.ekzxl();
						p2 = default(kpthf);
						goto end_IL_0045;
					}
					p.ekzxl();
					p = default(kpthf);
					end_IL_0045:;
				}
				finally
				{
					if (flag && 0 == 0)
					{
						if (wylwu == SocketShutdown.Both)
						{
							newqr.xpoxa.rtcbh = false;
						}
						newqr.xpoxa.vhyzo();
					}
				}
				end_IL_0000:;
			}
			catch (Exception p3)
			{
				xkikl = -2;
				xwfej.iurqb(p3);
				return;
			}
			xkikl = -2;
			xwfej.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in tsisp
			this.tsisp();
		}

		private void ejvfj(fgyyk p0)
		{
			xwfej.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ejvfj
			this.ejvfj(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CcloseInner_003Ed__b4 : fgyyk
	{
		public int bdjqq;

		public ljmxa buhmu;

		public ortbm rhfnt;

		private kpthf tbplx;

		private object glbjf;

		private void tacia()
		{
			try
			{
				bool flag = true;
				int num = bdjqq;
				_ = 0;
				try
				{
					kpthf p;
					if (bdjqq == 0)
					{
						p = tbplx;
						tbplx = default(kpthf);
						bdjqq = -1;
						goto IL_00ed;
					}
					if (rhfnt.luosf.svsgt && true && rhfnt.lptja != hlkgm.nzcmy && 0 == 0 && ((rhfnt.exdje.Options & TlsOptions.SilentClose) == 0 || 1 == 0))
					{
						p = rhfnt.ebnci(1, 0, p2: true).avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							bdjqq = 0;
							tbplx = p;
							buhmu.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
						goto IL_00ed;
					}
					goto end_IL_0013;
					IL_00ed:
					p.ekzxl();
					p = default(kpthf);
					end_IL_0013:;
				}
				catch
				{
				}
			}
			catch (Exception p2)
			{
				bdjqq = -2;
				buhmu.iurqb(p2);
				return;
			}
			bdjqq = -2;
			buhmu.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in tacia
			this.tacia();
		}

		private void jdqhe(fgyyk p0)
		{
			buhmu.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in jdqhe
			this.jdqhe(p0);
		}
	}

	public const int ykfvp = 16384;

	public const int tdeox = 17408;

	public const int xvgmj = 18432;

	public const int ffmcf = 18437;

	private TlsSocket oclcs;

	private qjasm gawrz;

	private volatile hlkgm lptja;

	private TlsParameters xsgvr;

	private bpnki qjbzn = bpnki.yiqfh;

	private bpnki tjjru = bpnki.yiqfh;

	private bpnki uljiq;

	private bpnki ebitx;

	private static readonly Func<Exception, Exception> vhsmv = TlsException.yxoes;

	private readonly nmhgd kfgxs = new nmhgd();

	private int mukqe = int.MaxValue;

	private int gyria = -1;

	private readonly byte[] zddyj = new byte[18437];

	private readonly byte[] yevcd = new byte[18437];

	private int twcca;

	private readonly nmhgd quhdu = new nmhgd();

	private bool qteas;

	private bool bxowl;

	private bool wqwju;

	private bool rognc;

	private readonly byte[] tdtgo = new byte[18437];

	private byte[] hsfzx = new byte[18437];

	private MemoryStream vyimy = new MemoryStream();

	protected bool rirto;

	private readonly fdnhz apyzr = new fdnhz();

	protected int zdprx;

	private bool ojcyj;

	private ILogWriter tlqxx;

	private readonly yelnx luosf = new yelnx();

	private readonly fdnhz mpawi = new fdnhz();

	private readonly fdnhz tebsv = new fdnhz();

	private readonly fdnhz kdjaj = new fdnhz();

	private mggni rzdbs;

	public ILogWriter vapxi
	{
		get
		{
			return tlqxx;
		}
		set
		{
			tlqxx = value;
		}
	}

	public mggni bguay
	{
		get
		{
			return rzdbs;
		}
		private set
		{
			rzdbs = value;
		}
	}

	public qjasm lddmf
	{
		get
		{
			return gawrz;
		}
		set
		{
			gawrz = value;
		}
	}

	public TlsParameters exdje => xsgvr;

	public hlkgm qvaka => lptja;

	public TlsProtocol xkrqa => (TlsProtocol)zdprx;

	public njvzu<bool> vrrcu => qajsf();

	internal bpnki ihqij
	{
		get
		{
			return uljiq;
		}
		set
		{
			uljiq = value;
		}
	}

	internal bpnki gvvsl
	{
		get
		{
			return ebitx;
		}
		set
		{
			ebitx = value;
		}
	}

	internal bpnki mmbtf => qjbzn;

	internal bpnki vkohr => tjjru;

	internal bool eymst => twcca > 0;

	public int znydr => (int)quhdu.Length;

	protected yelnx xpoxa => luosf;

	protected void hzxof(LogLevel p0, string p1, byte[] p2, int p3, int p4)
	{
		ILogWriter logWriter = tlqxx;
		if (logWriter != null && 0 == 0 && logWriter.Level <= p0)
		{
			logWriter.Write(p0, typeof(TlsSocket), bguay.GetHashCode(), "TLS", p1, p2, p3, p4);
		}
	}

	protected void bvdcq(LogLevel p0, string p1, params object[] p2)
	{
		ILogWriter logWriter = tlqxx;
		if (logWriter != null && 0 == 0 && logWriter.Level <= p0)
		{
			logWriter.Write(p0, typeof(TlsSocket), bguay.GetHashCode(), "TLS", brgjd.edcru(p1, p2));
		}
	}

	protected void mnuoq(LogLevel p0, string p1)
	{
		ILogWriter logWriter = tlqxx;
		if (logWriter != null && 0 == 0 && logWriter.Level <= p0)
		{
			logWriter.Write(p0, typeof(TlsSocket), bguay.GetHashCode(), "TLS", p1);
		}
	}

	protected void pfedo(LogLevel p0, Func<string> p1)
	{
		ILogWriter logWriter = tlqxx;
		if (logWriter != null && 0 == 0 && logWriter.Level <= p0)
		{
			logWriter.Write(p0, typeof(TlsSocket), bguay.GetHashCode(), "TLS", p1());
		}
	}

	[vtsnh(typeof(_003CStartNegotiating_003Ed__0))]
	protected njvzu<bool> ezgki()
	{
		_003CStartNegotiating_003Ed__0 p = default(_003CStartNegotiating_003Ed__0);
		p.iqjrc = this;
		p.bdcik = vxvbw<bool>.rdzxj();
		p.sgnjs = -1;
		vxvbw<bool> bdcik = p.bdcik;
		bdcik.vklen(ref p);
		return p.bdcik.xieya;
	}

	[vtsnh(typeof(_003CisConnected_003Ed__4))]
	private njvzu<bool> qajsf()
	{
		_003CisConnected_003Ed__4 p = default(_003CisConnected_003Ed__4);
		p.bufnd = this;
		p.hzkqp = vxvbw<bool>.rdzxj();
		p.hymhk = -1;
		vxvbw<bool> hzkqp = p.hzkqp;
		hzkqp.vklen(ref p);
		return p.hzkqp.xieya;
	}

	protected ortbm(TlsSocket owner, mggni channel, TlsParameters parameters)
	{
		bguay = channel;
		oclcs = owner;
		xsgvr = parameters;
		zdprx = 768;
	}

	protected abstract exkzi tmymn(byte[] p0, int p1, int p2);

	protected abstract exkzi ekqhl(byte[] p0, int p1, int p2);

	protected abstract exkzi jfwrj();

	private void pfzex(byte[] p0, int p1, int p2)
	{
		if (uljiq == null || 1 == 0)
		{
			throw new TlsException(mjddr.ypibb, brgjd.edcru("Received unexpected {0}.", "ChangeCipherSpec"));
		}
		if (p2 != 1)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "ChangeCipherSpec"));
		}
		if (p0[p1] != 1)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "ChangeCipherSpec"));
		}
		bpnki bpnki2 = qjbzn;
		qjbzn = uljiq;
		uljiq = null;
		if (bpnki2 != null && 0 == 0)
		{
			bpnki2.egphd();
		}
		oclcs.kxlvu(TlsDebugEventType.ChangeCipherSpec, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
	}

	[vtsnh(typeof(_003CProcessSsl2Handshake_003Ed__8))]
	private exkzi ligyt(byte[] p0, int p1, int p2)
	{
		_003CProcessSsl2Handshake_003Ed__8 p3 = default(_003CProcessSsl2Handshake_003Ed__8);
		p3.glkat = this;
		p3.xlxeu = p0;
		p3.wzzyr = p1;
		p3.nbcco = p2;
		p3.mackw = ljmxa.nmskg();
		p3.jqwmz = -1;
		ljmxa mackw = p3.mackw;
		mackw.nncuo(ref p3);
		return p3.mackw.donjp;
	}

	[vtsnh(typeof(_003CProcessHandshakeAsync_003Ed__b))]
	private exkzi jgsir(byte[] p0, int p1, int p2)
	{
		_003CProcessHandshakeAsync_003Ed__b p3 = default(_003CProcessHandshakeAsync_003Ed__b);
		p3.wxngw = this;
		p3.kmbla = p0;
		p3.myhgg = p1;
		p3.xrhmv = p2;
		p3.vcrih = ljmxa.nmskg();
		p3.jtgpl = -1;
		ljmxa vcrih = p3.vcrih;
		vcrih.nncuo(ref p3);
		return p3.vcrih.donjp;
	}

	[vtsnh(typeof(_003CProcessAlert_003Ed__e))]
	private exkzi jmkba(byte[] p0, int p1, int p2)
	{
		_003CProcessAlert_003Ed__e p3 = default(_003CProcessAlert_003Ed__e);
		p3.hozth = this;
		p3.ltllv = p0;
		p3.rcsgj = p1;
		p3.swbbk = p2;
		p3.wnzrl = ljmxa.nmskg();
		p3.pifmc = -1;
		ljmxa wnzrl = p3.wnzrl;
		wnzrl.nncuo(ref p3);
		return p3.wnzrl.donjp;
	}

	[vtsnh(typeof(_003CProcessData_003Ed__12))]
	private exkzi mlrkx(byte[] p0, int p1, int p2)
	{
		_003CProcessData_003Ed__12 p3 = default(_003CProcessData_003Ed__12);
		p3.ityac = this;
		p3.hzjwq = p0;
		p3.zocep = p1;
		p3.baphv = p2;
		p3.bqyno = ljmxa.nmskg();
		p3.hkaxy = -1;
		ljmxa bqyno = p3.bqyno;
		bqyno.nncuo(ref p3);
		return p3.bqyno.donjp;
	}

	[vtsnh(typeof(_003CReceiveData_003Ed__16))]
	private njvzu<bool> jmhop(int p0)
	{
		_003CReceiveData_003Ed__16 p1 = default(_003CReceiveData_003Ed__16);
		p1.wwnap = this;
		p1.vemdp = p0;
		p1.viqvn = vxvbw<bool>.rdzxj();
		p1.zvjlw = -1;
		vxvbw<bool> viqvn = p1.viqvn;
		viqvn.vklen(ref p1);
		return p1.viqvn.xieya;
	}

	[vtsnh(typeof(_003CReceivePacketInner_003Ed__1c))]
	private njvzu<int> ynzni()
	{
		_003CReceivePacketInner_003Ed__1c p = default(_003CReceivePacketInner_003Ed__1c);
		p.pnubm = this;
		p.unqok = vxvbw<int>.rdzxj();
		p.nfqzn = -1;
		vxvbw<int> unqok = p.unqok;
		unqok.vklen(ref p);
		return p.unqok.xieya;
	}

	[vtsnh(typeof(_003CProcess_003Ed__2b))]
	internal exkzi dnapo()
	{
		_003CProcess_003Ed__2b p = default(_003CProcess_003Ed__2b);
		p.qmfch = this;
		p.klxcb = ljmxa.nmskg();
		p.yqqqt = -1;
		ljmxa klxcb = p.klxcb;
		klxcb.nncuo(ref p);
		return p.klxcb.donjp;
	}

	[vtsnh(typeof(_003CprocessInnerAsync_003Ed__30))]
	private exkzi zrpeb()
	{
		_003CprocessInnerAsync_003Ed__30 p = default(_003CprocessInnerAsync_003Ed__30);
		p.iuxwb = this;
		p.opnky = ljmxa.nmskg();
		p.hdmzi = -1;
		ljmxa opnky = p.opnky;
		opnky.nncuo(ref p);
		return p.opnky.donjp;
	}

	[vtsnh(typeof(_003CEnqueue_003Ed__39))]
	protected exkzi kjdzy(vcedo p0, byte[] p1, int p2, int p3)
	{
		_003CEnqueue_003Ed__39 p4 = default(_003CEnqueue_003Ed__39);
		p4.gdcec = this;
		p4.yzdtz = p0;
		p4.fmhsc = p1;
		p4.zhpos = p2;
		p4.vpuau = p3;
		p4.yvdyv = ljmxa.nmskg();
		p4.stvac = -1;
		ljmxa yvdyv = p4.yvdyv;
		yvdyv.nncuo(ref p4);
		return p4.yvdyv.donjp;
	}

	[vtsnh(typeof(_003CSendQueue_003Ed__45))]
	internal exkzi zupbs()
	{
		_003CSendQueue_003Ed__45 p = default(_003CSendQueue_003Ed__45);
		p.woonb = this;
		p.tevnb = ljmxa.nmskg();
		p.bborp = -1;
		ljmxa tevnb = p.tevnb;
		tevnb.nncuo(ref p);
		return p.tevnb.donjp;
	}

	[vtsnh(typeof(_003CsendQueueInner_003Ed__4a))]
	private exkzi ulpqp()
	{
		_003CsendQueueInner_003Ed__4a p = default(_003CsendQueueInner_003Ed__4a);
		p.xzakh = this;
		p.imxpy = ljmxa.nmskg();
		p.vrmud = -1;
		ljmxa imxpy = p.imxpy;
		imxpy.nncuo(ref p);
		return p.imxpy.donjp;
	}

	[vtsnh(typeof(_003CSetSecureState_003Ed__4e))]
	internal exkzi soeay()
	{
		_003CSetSecureState_003Ed__4e p = default(_003CSetSecureState_003Ed__4e);
		p.rdugx = this;
		p.cijkt = ljmxa.nmskg();
		p.mucko = -1;
		ljmxa cijkt = p.cijkt;
		cijkt.nncuo(ref p);
		return p.cijkt.donjp;
	}

	[vtsnh(typeof(_003CAlert_003Ed__5f))]
	protected exkzi ebnci(byte p0, byte p1, bool p2)
	{
		_003CAlert_003Ed__5f p3 = default(_003CAlert_003Ed__5f);
		p3.bvpva = this;
		p3.yqfol = p0;
		p3.cegah = p1;
		p3.egtlq = p2;
		p3.xijri = ljmxa.nmskg();
		p3.lmpta = -1;
		ljmxa xijri = p3.xijri;
		xijri.nncuo(ref p3);
		return p3.xijri.donjp;
	}

	[vtsnh(typeof(_003CSend_003Ed__6f))]
	public exkzi xehuo(byte[] p0, int p1, int p2)
	{
		_003CSend_003Ed__6f p3 = default(_003CSend_003Ed__6f);
		p3.yjavt = this;
		p3.uhecb = p0;
		p3.rwtvq = p1;
		p3.ujqor = p2;
		p3.tvrvt = ljmxa.nmskg();
		p3.trdup = -1;
		ljmxa tvrvt = p3.tvrvt;
		tvrvt.nncuo(ref p3);
		return p3.tvrvt.donjp;
	}

	[vtsnh(typeof(_003CReceive_003Ed__7d))]
	public njvzu<int> mohjs(byte[] p0, int p1, int p2, bool p3)
	{
		_003CReceive_003Ed__7d p4 = default(_003CReceive_003Ed__7d);
		p4.jcjvx = this;
		p4.ysuqg = p0;
		p4.cudjb = p1;
		p4.myeml = p2;
		p4.lginc = p3;
		p4.cubwf = vxvbw<int>.rdzxj();
		p4.cdkcr = -1;
		vxvbw<int> cubwf = p4.cubwf;
		cubwf.vklen(ref p4);
		return p4.cubwf.xieya;
	}

	[vtsnh(typeof(_003CreceiveInnerAsync_003Ed__83))]
	private njvzu<int> ywwoe(byte[] p0, int p1, int p2, bool p3)
	{
		_003CreceiveInnerAsync_003Ed__83 p4 = default(_003CreceiveInnerAsync_003Ed__83);
		p4.xlcxb = this;
		p4.qpvke = p0;
		p4.czcub = p1;
		p4.vpsze = p2;
		p4.znrqt = p3;
		p4.uwhfz = vxvbw<int>.rdzxj();
		p4.lkthh = -1;
		vxvbw<int> uwhfz = p4.uwhfz;
		uwhfz.vklen(ref p4);
		return p4.uwhfz.xieya;
	}

	[vtsnh(typeof(_003CNegotiate_003Ed__89))]
	public exkzi vsztj()
	{
		_003CNegotiate_003Ed__89 p = default(_003CNegotiate_003Ed__89);
		p.qtrkn = this;
		p.dinfl = ljmxa.nmskg();
		p.xgbmv = -1;
		ljmxa dinfl = p.dinfl;
		dinfl.nncuo(ref p);
		return p.dinfl.donjp;
	}

	[vtsnh(typeof(_003CUnprotect_003Ed__8e))]
	public exkzi dtabq()
	{
		_003CUnprotect_003Ed__8e p = default(_003CUnprotect_003Ed__8e);
		p.lpteh = this;
		p.xudqy = ljmxa.nmskg();
		p.swvhd = -1;
		ljmxa xudqy = p.xudqy;
		xudqy.nncuo(ref p);
		return p.xudqy.donjp;
	}

	private void exyau()
	{
		if (!ojcyj || 1 == 0)
		{
			ojcyj = true;
			oclcs.ivtmn(TlsDebugEventType.Closed, TlsDebugEventSource.Unspecified, TlsDebugLevel.Important);
		}
	}

	[vtsnh(typeof(_003CClose_003Ed__94))]
	private njvzu<Exception> fptsn(Exception p0, string p1)
	{
		_003CClose_003Ed__94 p2 = default(_003CClose_003Ed__94);
		p2.wbngj = this;
		p2.tvdez = p0;
		p2.sqzil = p1;
		p2.uhczw = vxvbw<Exception>.rdzxj();
		p2.wzfvl = -1;
		vxvbw<Exception> uhczw = p2.uhczw;
		uhczw.vklen(ref p2);
		return p2.uhczw.xieya;
	}

	[vtsnh(typeof(_003CCloseSocket_003Ed__9e))]
	private exkzi szxmn()
	{
		_003CCloseSocket_003Ed__9e p = default(_003CCloseSocket_003Ed__9e);
		p.pqdiz = this;
		p.cldgz = ljmxa.nmskg();
		p.snowf = -1;
		ljmxa cldgz = p.cldgz;
		cldgz.nncuo(ref p);
		return p.cldgz.donjp;
	}

	[vtsnh(typeof(_003CClose_003Ed__a7))]
	public exkzi czclf()
	{
		_003CClose_003Ed__a7 p = default(_003CClose_003Ed__a7);
		p.nuaoe = this;
		p.zxzxg = ljmxa.nmskg();
		p.dqajn = -1;
		ljmxa zxzxg = p.zxzxg;
		zxzxg.nncuo(ref p);
		return p.zxzxg.donjp;
	}

	[vtsnh(typeof(_003CShutdownAsync_003Ed__ae))]
	public exkzi jdlav()
	{
		_003CShutdownAsync_003Ed__ae p = default(_003CShutdownAsync_003Ed__ae);
		p.tviks = this;
		p.ynseq = ljmxa.nmskg();
		p.geitg = -1;
		ljmxa ynseq = p.ynseq;
		ynseq.nncuo(ref p);
		return p.ynseq.donjp;
	}

	[vtsnh(typeof(_003CShutdownAsync_003Ed__b1))]
	public exkzi gpihd(SocketShutdown p0)
	{
		_003CShutdownAsync_003Ed__b1 p1 = default(_003CShutdownAsync_003Ed__b1);
		p1.newqr = this;
		p1.wylwu = p0;
		p1.xwfej = ljmxa.nmskg();
		p1.xkikl = -1;
		ljmxa xwfej = p1.xwfej;
		xwfej.nncuo(ref p1);
		return p1.xwfej.donjp;
	}

	[vtsnh(typeof(_003CcloseInner_003Ed__b4))]
	private exkzi kwvqt()
	{
		_003CcloseInner_003Ed__b4 p = default(_003CcloseInner_003Ed__b4);
		p.rhfnt = this;
		p.buhmu = ljmxa.nmskg();
		p.bdjqq = -1;
		ljmxa buhmu = p.buhmu;
		buhmu.nncuo(ref p);
		return p.buhmu.donjp;
	}

	[vtsnh(typeof(_003C_003CProcess_003Eb__26_003Ed__27))]
	private exkzi vsijy()
	{
		_003C_003CProcess_003Eb__26_003Ed__27 p = default(_003C_003CProcess_003Eb__26_003Ed__27);
		p.bndxc = this;
		p.rbtxn = ljmxa.nmskg();
		p.ksvvg = -1;
		ljmxa rbtxn = p.rbtxn;
		rbtxn.nncuo(ref p);
		return p.rbtxn.donjp;
	}

	[vtsnh(typeof(_003C_003CSendQueue_003Eb__3f_003Ed__41))]
	private exkzi ilueg()
	{
		_003C_003CSendQueue_003Eb__3f_003Ed__41 p = default(_003C_003CSendQueue_003Eb__3f_003Ed__41);
		p.oletx = this;
		p.ybnhw = ljmxa.nmskg();
		p.wnpfl = -1;
		ljmxa ybnhw = p.ybnhw;
		ybnhw.nncuo(ref p);
		return p.ybnhw.donjp;
	}

	[vtsnh(typeof(_003C_003CSend_003Eb__65_003Ed__69))]
	private exkzi yrfdi()
	{
		_003C_003CSend_003Eb__65_003Ed__69 p = default(_003C_003CSend_003Eb__65_003Ed__69);
		p.ebjwy = this;
		p.iukii = ljmxa.nmskg();
		p.tbjrm = -1;
		ljmxa iukii = p.iukii;
		iukii.nncuo(ref p);
		return p.iukii.donjp;
	}

	[vtsnh(typeof(_003C_003CSend_003Eb__66_003Ed__6c))]
	private exkzi elfgo(Exception p0)
	{
		_003C_003CSend_003Eb__66_003Ed__6c p1 = default(_003C_003CSend_003Eb__66_003Ed__6c);
		p1.ysvdf = this;
		p1.zfdjd = p0;
		p1.nauzz = ljmxa.nmskg();
		p1.nbvzz = -1;
		ljmxa nauzz = p1.nauzz;
		nauzz.nncuo(ref p1);
		return p1.nauzz.donjp;
	}

	[vtsnh(typeof(_003C_003CReceive_003Eb__77_003Ed__79))]
	private exkzi ucupl()
	{
		_003C_003CReceive_003Eb__77_003Ed__79 p = default(_003C_003CReceive_003Eb__77_003Ed__79);
		p.jzbrt = this;
		p.uiwtx = ljmxa.nmskg();
		p.khizg = -1;
		ljmxa uiwtx = p.uiwtx;
		uiwtx.nncuo(ref p);
		return p.uiwtx.donjp;
	}

	[vtsnh(typeof(_003C_003CClose_003Eb__a3_003Ed__a4))]
	private exkzi lecpj()
	{
		_003C_003CClose_003Eb__a3_003Ed__a4 p = default(_003C_003CClose_003Eb__a3_003Ed__a4);
		p.gtqmq = this;
		p.xutev = ljmxa.nmskg();
		p.usmhk = -1;
		ljmxa xutev = p.xutev;
		xutev.nncuo(ref p);
		return p.xutev.donjp;
	}
}
