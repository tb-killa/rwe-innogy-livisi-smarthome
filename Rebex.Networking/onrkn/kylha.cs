using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal class kylha : IDisposable
{
	private class jrpqk
	{
		public ieaus bkand;

		public EndPoint idbjf;
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CProcess_003Ed__0 : fgyyk
	{
		public int vdpjs;

		public ljmxa jseod;

		public kylha anbgm;

		public ydtkc<byte> xqbpx;

		public pynjh dkbws;

		public eqttv durso;

		public bool faxjw;

		public jrpqk qilse;

		private xuwyj<eqttv> kagpw;

		private object lqyvy;

		private xuwyj<bool> yfmpc;

		private xuwyj<jrpqk> kwbvj;

		private kpthf bbcfx;

		private void nsocv()
		{
			try
			{
				bool flag = true;
				eqttv num;
				xuwyj<eqttv> p6;
				eqttv eqttv2;
				kpthf p3;
				bool num2;
				xuwyj<bool> p5;
				bool flag2;
				kpthf p2;
				jrpqk jrpqk;
				xuwyj<jrpqk> p4;
				jrpqk jrpqk2;
				kpthf p;
				switch (vdpjs)
				{
				default:
					xqbpx = new ydtkc<byte>(1024);
					dkbws = new pynjh(xqbpx, anbgm.xaugv);
					p6 = anbgm.myonb(dkbws, anbgm.xaugv).giftg(p1: false).vuozn();
					if (!p6.hqxbj || 1 == 0)
					{
						vdpjs = 0;
						kagpw = p6;
						jseod.wqiyk(ref p6, ref this);
						flag = false;
						return;
					}
					goto IL_00d7;
				case 0:
					p6 = kagpw;
					kagpw = default(xuwyj<eqttv>);
					vdpjs = -1;
					goto IL_00d7;
				case 1:
					p5 = yfmpc;
					yfmpc = default(xuwyj<bool>);
					vdpjs = -1;
					goto IL_01a1;
				case 2:
					p4 = kwbvj;
					kwbvj = default(xuwyj<jrpqk>);
					vdpjs = -1;
					goto IL_024a;
				case 3:
					p3 = bbcfx;
					bbcfx = default(kpthf);
					vdpjs = -1;
					goto IL_0301;
				case 4:
					p2 = bbcfx;
					bbcfx = default(kpthf);
					vdpjs = -1;
					goto IL_03ad;
				case 5:
					{
						p = bbcfx;
						bbcfx = default(kpthf);
						vdpjs = -1;
						break;
					}
					IL_0315:
					if (anbgm.qgnau.ulwec && 0 == 0)
					{
						p2 = anbgm.klajg(qilse.idbjf).avdby(p1: false).vrtmi();
						if (!p2.zpafv || 1 == 0)
						{
							vdpjs = 4;
							bbcfx = p2;
							jseod.wqiyk(ref p2, ref this);
							flag = false;
							return;
						}
						goto IL_03ad;
					}
					goto IL_03c1;
					IL_00d7:
					num = p6.gbccf();
					p6 = default(xuwyj<eqttv>);
					eqttv2 = num;
					durso = eqttv2;
					switch (durso)
					{
					case eqttv.fawah:
						goto IL_0110;
					case eqttv.pgyos:
						break;
					case eqttv.mgppu:
						goto IL_01d4;
					}
					goto end_IL_0000;
					IL_0110:
					p5 = anbgm.idadz(dkbws, anbgm.xaugv, anbgm.qgnau.lcqnx).giftg(p1: false).vuozn();
					if (!p5.hqxbj || 1 == 0)
					{
						vdpjs = 1;
						yfmpc = p5;
						jseod.wqiyk(ref p5, ref this);
						flag = false;
						return;
					}
					goto IL_01a1;
					IL_0301:
					p3.ekzxl();
					p3 = default(kpthf);
					goto end_IL_0000;
					IL_01a1:
					num2 = p5.gbccf();
					p5 = default(xuwyj<bool>);
					flag2 = num2;
					faxjw = flag2;
					if (faxjw ? true : false)
					{
						goto IL_01d4;
					}
					goto end_IL_0000;
					IL_03ad:
					p2.ekzxl();
					p2 = default(kpthf);
					goto end_IL_0000;
					IL_01d4:
					p4 = anbgm.ziznj(dkbws).giftg(p1: false).vuozn();
					if (!p4.hqxbj || 1 == 0)
					{
						vdpjs = 2;
						kwbvj = p4;
						jseod.wqiyk(ref p4, ref this);
						flag = false;
						return;
					}
					goto IL_024a;
					IL_03c1:
					p = anbgm.exbnq(zibdr.kndtf, null).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						vdpjs = 5;
						bbcfx = p;
						jseod.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
					IL_024a:
					jrpqk = p4.gbccf();
					p4 = default(xuwyj<jrpqk>);
					jrpqk2 = jrpqk;
					qilse = jrpqk2;
					switch (qilse.bkand)
					{
					case ieaus.bkfgc:
						break;
					case ieaus.wbmhj:
						goto IL_0315;
					default:
						goto IL_03c1;
					}
					p3 = anbgm.ysuoe(qilse.idbjf).avdby(p1: false).vrtmi();
					if (!p3.zpafv || 1 == 0)
					{
						vdpjs = 3;
						bbcfx = p3;
						jseod.wqiyk(ref p3, ref this);
						flag = false;
						return;
					}
					goto IL_0301;
				}
				p.ekzxl();
				p = default(kpthf);
				end_IL_0000:;
			}
			catch (Exception p7)
			{
				vdpjs = -2;
				jseod.iurqb(p7);
				return;
			}
			vdpjs = -2;
			jseod.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in nsocv
			this.nsocv();
		}

		private void hxgzy(fgyyk p0)
		{
			jseod.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in hxgzy
			this.hxgzy(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CBridgeChannels_003Ed__b : fgyyk
	{
		public int eveyh;

		public ljmxa hmvxz;

		public mggni fgxuv;

		public mggni jwyvx;

		public int vmvqy;

		public ihlqx<ArraySegment<byte>> eydbv;

		public ArraySegment<byte> kmjmj;

		public int dzrlk;

		public kylha nbqvf;

		private xuwyj<int> seuod;

		private object nawwp;

		private kpthf sltao;

		private void cwdre()
		{
			try
			{
				bool flag = true;
				kpthf p;
				switch (eveyh)
				{
				default:
					eydbv = sxztb<byte>.ahblv.dwplz(vmvqy);
					goto case 0;
				case 0:
				case 1:
					try
					{
						kpthf p2;
						int num;
						xuwyj<int> p3;
						int num2;
						switch (eveyh)
						{
						default:
							kmjmj = eydbv.wnjdk;
							goto IL_005b;
						case 0:
							p3 = seuod;
							seuod = default(xuwyj<int>);
							eveyh = -1;
							goto IL_00d1;
						case 1:
							{
								p2 = sltao;
								sltao = default(kpthf);
								eveyh = -1;
								goto IL_0184;
							}
							IL_005b:
							p3 = fgxuv.rhjom(kmjmj).giftg(p1: false).vuozn();
							if (!p3.hqxbj || 1 == 0)
							{
								eveyh = 0;
								seuod = p3;
								hmvxz.wqiyk(ref p3, ref this);
								flag = false;
								return;
							}
							goto IL_00d1;
							IL_0184:
							p2.ekzxl();
							p2 = default(kpthf);
							goto IL_005b;
							IL_00d1:
							num = p3.gbccf();
							p3 = default(xuwyj<int>);
							num2 = num;
							dzrlk = num2;
							if (dzrlk != 0 && 0 == 0)
							{
								p2 = jwyvx.zykkj(new nxtme<byte>(kmjmj.Array, 0, dzrlk)).avdby(p1: false).vrtmi();
								if (!p2.zpafv || 1 == 0)
								{
									break;
								}
								goto IL_0184;
							}
							goto end_IL_0035;
						}
						eveyh = 1;
						sltao = p2;
						hmvxz.wqiyk(ref p2, ref this);
						flag = false;
						return;
						end_IL_0035:;
					}
					finally
					{
						if (flag && 0 == 0 && eydbv != null && 0 == 0)
						{
							((IDisposable)eydbv).Dispose();
						}
					}
					p = jwyvx.qxxgh().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						eveyh = 2;
						sltao = p;
						hmvxz.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
				case 2:
					p = sltao;
					sltao = default(kpthf);
					eveyh = -1;
					break;
				}
				p.ekzxl();
				p = default(kpthf);
			}
			catch (Exception p4)
			{
				eveyh = -2;
				hmvxz.iurqb(p4);
				return;
			}
			eveyh = -2;
			hmvxz.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in cwdre
			this.cwdre();
		}

		private void uvokq(fgyyk p0)
		{
			hmvxz.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in uvokq
			this.uvokq(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CExecuteBindCommand_003Ed__12 : fgyyk
	{
		public int xicsz;

		public ljmxa hlzac;

		public kylha nnnga;

		public EndPoint utwel;

		public stynd llbyh;

		public ecllo kkfql;

		public EndPoint bwqmz;

		public rlrlj bzzby;

		private kpthf pztku;

		private object hozqi;

		private xuwyj<EndPoint> sbysg;

		private xuwyj<rlrlj> skwpc;

		private void elati()
		{
			try
			{
				bool flag = true;
				EndPoint endPoint;
				xuwyj<EndPoint> p5;
				EndPoint endPoint2;
				kpthf p4;
				rlrlj obj;
				xuwyj<rlrlj> p3;
				rlrlj rlrlj2;
				kpthf p6;
				kpthf p2;
				kpthf p;
				switch (xicsz)
				{
				default:
					llbyh = null;
					kkfql = nnnga.qgnau.sgheu;
					if (kkfql != null && 0 == 0)
					{
						llbyh = kkfql(utwel, nnnga.iverh.qncaj);
					}
					if (llbyh == null || 1 == 0)
					{
						p6 = nnnga.exbnq(zibdr.ijuom, null).avdby(p1: false).vrtmi();
						if (!p6.zpafv || 1 == 0)
						{
							xicsz = 0;
							pztku = p6;
							hlzac.wqiyk(ref p6, ref this);
							flag = false;
							return;
						}
						goto IL_0103;
					}
					p5 = llbyh.nkxna.giftg(p1: false).vuozn();
					if (!p5.hqxbj || 1 == 0)
					{
						xicsz = 1;
						sbysg = p5;
						hlzac.wqiyk(ref p5, ref this);
						flag = false;
						return;
					}
					goto IL_0187;
				case 0:
					p6 = pztku;
					pztku = default(kpthf);
					xicsz = -1;
					goto IL_0103;
				case 1:
					p5 = sbysg;
					sbysg = default(xuwyj<EndPoint>);
					xicsz = -1;
					goto IL_0187;
				case 2:
					p4 = pztku;
					pztku = default(kpthf);
					xicsz = -1;
					goto IL_0217;
				case 3:
					p3 = skwpc;
					skwpc = default(xuwyj<rlrlj>);
					xicsz = -1;
					goto IL_033a;
				case 4:
					p2 = pztku;
					pztku = default(kpthf);
					xicsz = -1;
					goto IL_03e5;
				case 5:
					{
						p = pztku;
						pztku = default(kpthf);
						xicsz = -1;
						break;
					}
					IL_0187:
					endPoint = p5.gbccf();
					p5 = default(xuwyj<EndPoint>);
					endPoint2 = endPoint;
					bwqmz = endPoint2;
					p4 = nnnga.exbnq(zibdr.eizzn, bwqmz).avdby(p1: false).vrtmi();
					if (!p4.zpafv || 1 == 0)
					{
						xicsz = 2;
						pztku = p4;
						hlzac.wqiyk(ref p4, ref this);
						flag = false;
						return;
					}
					goto IL_0217;
					IL_0217:
					p4.ekzxl();
					p4 = default(kpthf);
					nnnga.alvus.byfnx(LogLevel.Debug, "SocksServer", "SOCKS5 Bind on session #{0}: (client) {1} <==> {2} (proxy) {3} <==> (waiting for connection).", nnnga.mkntd, (nnnga.frnta == null) ? ((object)"-") : ((object)nnnga.frnta.RemoteEndPoint), (nnnga.frnta == null) ? ((object)"-") : ((object)nnnga.frnta.LocalEndPoint), bwqmz);
					p3 = llbyh.scfbb.giftg(p1: false).vuozn();
					if (!p3.hqxbj || 1 == 0)
					{
						xicsz = 3;
						skwpc = p3;
						hlzac.wqiyk(ref p3, ref this);
						flag = false;
						return;
					}
					goto IL_033a;
					IL_033a:
					obj = p3.gbccf();
					p3 = default(xuwyj<rlrlj>);
					rlrlj2 = obj;
					bzzby = rlrlj2;
					nnnga.uawpb = bzzby.cvyrt;
					p2 = nnnga.exbnq(zibdr.eizzn, bzzby.grnci).avdby(p1: false).vrtmi();
					if (!p2.zpafv || 1 == 0)
					{
						xicsz = 4;
						pztku = p2;
						hlzac.wqiyk(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_03e5;
					IL_0103:
					p6.ekzxl();
					p6 = default(kpthf);
					goto end_IL_0000;
					IL_03e5:
					p2.ekzxl();
					p2 = default(kpthf);
					p = nnnga.khumi(bzzby.cvyrt).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						xicsz = 5;
						pztku = p;
						hlzac.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
				}
				p.ekzxl();
				p = default(kpthf);
				nnnga.hqfdd();
				end_IL_0000:;
			}
			catch (Exception p7)
			{
				xicsz = -2;
				hlzac.iurqb(p7);
				return;
			}
			xicsz = -2;
			hlzac.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in elati
			this.elati();
		}

		private void xradt(fgyyk p0)
		{
			hlzac.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in xradt
			this.xradt(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CExecuteConnectCommand_003Ed__1b : fgyyk
	{
		public int spwou;

		public ljmxa hnpwl;

		public kylha wkmwy;

		public EndPoint ywcrn;

		public smalv fxrey;

		public rlrlj xsvci;

		private xuwyj<rlrlj> zsuwa;

		private object hqhdx;

		private kpthf vdfyi;

		private void maftj()
		{
			try
			{
				bool flag = true;
				kpthf p2;
				kpthf p3;
				rlrlj obj;
				xuwyj<rlrlj> p4;
				rlrlj rlrlj2;
				kpthf p;
				switch (spwou)
				{
				default:
					fxrey = wkmwy.qgnau.hpkux;
					xsvci = null;
					if (fxrey != null && 0 == 0)
					{
						p4 = fxrey(ywcrn).giftg(p1: false).vuozn();
						if (!p4.hqxbj || 1 == 0)
						{
							spwou = 0;
							zsuwa = p4;
							hnpwl.wqiyk(ref p4, ref this);
							flag = false;
							return;
						}
						goto IL_00c8;
					}
					goto IL_00e1;
				case 0:
					p4 = zsuwa;
					zsuwa = default(xuwyj<rlrlj>);
					spwou = -1;
					goto IL_00c8;
				case 1:
					p3 = vdfyi;
					vdfyi = default(kpthf);
					spwou = -1;
					goto IL_0166;
				case 2:
					p2 = vdfyi;
					vdfyi = default(kpthf);
					spwou = -1;
					goto IL_0233;
				case 3:
					{
						p = vdfyi;
						vdfyi = default(kpthf);
						spwou = -1;
						break;
					}
					IL_0233:
					p2.ekzxl();
					p2 = default(kpthf);
					wkmwy.alvus.byfnx(LogLevel.Debug, "SocksServer", "SOCKS5 Connect on session #{0}: (client) {1} <==> {2} (proxy) {3} <==> {4} (target).", wkmwy.mkntd, (wkmwy.frnta == null) ? ((object)"-") : ((object)wkmwy.frnta.RemoteEndPoint), (wkmwy.frnta == null) ? ((object)"-") : ((object)wkmwy.frnta.LocalEndPoint), (xsvci.jgwdr == null) ? ((object)"-") : ((object)xsvci.jgwdr), ywcrn);
					p = wkmwy.khumi(xsvci.cvyrt).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						spwou = 3;
						vdfyi = p;
						hnpwl.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
					IL_00e1:
					if (xsvci == null || 1 == 0)
					{
						p3 = wkmwy.exbnq(zibdr.ijuom, null).avdby(p1: false).vrtmi();
						if (!p3.zpafv || 1 == 0)
						{
							spwou = 1;
							vdfyi = p3;
							hnpwl.wqiyk(ref p3, ref this);
							flag = false;
							return;
						}
						goto IL_0166;
					}
					wkmwy.dsvpf = ywcrn;
					wkmwy.tvmss = xsvci.jgwdr;
					wkmwy.uawpb = xsvci.cvyrt;
					p2 = wkmwy.exbnq(zibdr.eizzn, xsvci.jgwdr).avdby(p1: false).vrtmi();
					if (!p2.zpafv || 1 == 0)
					{
						spwou = 2;
						vdfyi = p2;
						hnpwl.wqiyk(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_0233;
					IL_0166:
					p3.ekzxl();
					p3 = default(kpthf);
					goto end_IL_0000;
					IL_00c8:
					obj = p4.gbccf();
					p4 = default(xuwyj<rlrlj>);
					rlrlj2 = obj;
					xsvci = rlrlj2;
					goto IL_00e1;
				}
				p.ekzxl();
				p = default(kpthf);
				wkmwy.hqfdd();
				end_IL_0000:;
			}
			catch (Exception p5)
			{
				spwou = -2;
				hnpwl.iurqb(p5);
				return;
			}
			spwou = -2;
			hnpwl.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in maftj
			this.maftj();
		}

		private void wowwq(fgyyk p0)
		{
			hnpwl.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in wowwq
			this.wowwq(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CNegotiateAuthMethod_003Ed__23 : fgyyk
	{
		public int xkfjj;

		public vxvbw<eqttv> zyvdq;

		public kylha qfaxr;

		public pynjh pqzzk;

		public jghfk kpigp;

		public byte eweet;

		public eqttv mwchd;

		private kpthf tmiya;

		private object jzhwg;

		private void zjfql()
		{
			eqttv p4;
			try
			{
				bool flag = true;
				kpthf p2;
				nxtme<byte>.rrjio rrjio;
				kpthf p3;
				pynjh obj;
				kpthf p;
				switch (xkfjj)
				{
				default:
					p3 = pqzzk.wdaeu(2).avdby(p1: false).vrtmi();
					if (!p3.zpafv || 1 == 0)
					{
						xkfjj = 0;
						tmiya = p3;
						zyvdq.xiwgo(ref p3, ref this);
						flag = false;
						return;
					}
					goto IL_0092;
				case 0:
					p3 = tmiya;
					tmiya = default(kpthf);
					xkfjj = -1;
					goto IL_0092;
				case 1:
					p2 = tmiya;
					tmiya = default(kpthf);
					xkfjj = -1;
					goto IL_0164;
				case 2:
					{
						p = tmiya;
						tmiya = default(kpthf);
						xkfjj = -1;
						break;
					}
					IL_0164:
					p2.ekzxl();
					p2 = default(kpthf);
					mwchd = eqttv.qjjax;
					rrjio = pqzzk.hycpz.jayzd(eweet).twvtt();
					try
					{
						while (rrjio.MoveNext() ? true : false)
						{
							byte current = rrjio.Current;
							eqttv eqttv2 = (eqttv)current;
							if (((qfaxr.qgnau.lcqnx == null || 1 == 0) && eqttv2 == eqttv.mgppu) || (qfaxr.qgnau.lcqnx != null && 0 == 0 && eqttv2 == eqttv.fawah))
							{
								mwchd = eqttv2;
								break;
							}
						}
					}
					finally
					{
						if (flag && 0 == 0)
						{
							((IDisposable)rrjio/*cast due to .constrained prefix*/).Dispose();
						}
					}
					pqzzk.hycpz.pzwbh(eweet);
					p = ysesn(kpigp, 5, (byte)mwchd).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						xkfjj = 2;
						tmiya = p;
						zyvdq.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
					break;
					IL_0092:
					p3.ekzxl();
					p3 = default(kpthf);
					pqzzk.fosmh(5, "Expected SOCKS protocol version.");
					obj = pqzzk;
					if (reudl == null || 1 == 0)
					{
						reudl = rergw;
					}
					eweet = obj.tbben(reudl, "At least one SOCKS5 method is expected.");
					p2 = pqzzk.wdaeu(eweet).avdby(p1: false).vrtmi();
					if (!p2.zpafv || 1 == 0)
					{
						xkfjj = 1;
						tmiya = p2;
						zyvdq.xiwgo(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_0164;
				}
				p.ekzxl();
				p = default(kpthf);
				p4 = mwchd;
			}
			catch (Exception p5)
			{
				xkfjj = -2;
				zyvdq.tudwl(p5);
				return;
			}
			xkfjj = -2;
			zyvdq.vzyck(p4);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in zjfql
			this.zjfql();
		}

		private void jangu(fgyyk p0)
		{
			zyvdq.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in jangu
			this.jangu(p0);
		}
	}

	private sealed class vymlv
	{
		public byte ywjgg;

		public string spxke(nxtme<byte> p0)
		{
			return Encoding.ASCII.GetString(p0.lthjd, p0.frlfs, ywjgg);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CNegotiateCommand_003Ed__2c : fgyyk
	{
		public int yeqcd;

		public vxvbw<jrpqk> unoth;

		public kylha fzpjj;

		public pynjh bywps;

		public ieaus drrtq;

		public qwdoo ryrkx;

		public EndPoint bjzgq;

		public byte[] coyum;

		public ushort jrylx;

		public byte[] dkbar;

		public ushort jxfwl;

		public string ylpoa;

		public ushort mbkng;

		public vymlv vjbha;

		public jrpqk zdrfr;

		private kpthf dbvfc;

		private object apacs;

		private xuwyj<byte> wkwjo;

		private void asgoh()
		{
			jrpqk p7;
			try
			{
				bool flag = true;
				vymlv vymlv2;
				kpthf p2;
				kpthf p6;
				kpthf p4;
				kpthf p;
				kpthf p5;
				byte ywjgg;
				xuwyj<byte> p3;
				switch (yeqcd)
				{
				default:
					p6 = bywps.wdaeu(4).avdby(p1: false).vrtmi();
					if (!p6.zpafv || 1 == 0)
					{
						yeqcd = 0;
						dbvfc = p6;
						unoth.xiwgo(ref p6, ref this);
						flag = false;
						return;
					}
					goto IL_009c;
				case 0:
					p6 = dbvfc;
					dbvfc = default(kpthf);
					yeqcd = -1;
					goto IL_009c;
				case 1:
					p5 = dbvfc;
					dbvfc = default(kpthf);
					yeqcd = -1;
					goto IL_0184;
				case 2:
					p4 = dbvfc;
					dbvfc = default(kpthf);
					yeqcd = -1;
					goto IL_0249;
				case 3:
				{
					vymlv vymlv = (vymlv)apacs;
					vymlv2 = vymlv;
					apacs = null;
					p3 = wkwjo;
					wkwjo = default(xuwyj<byte>);
					yeqcd = -1;
					goto IL_033e;
				}
				case 4:
					p2 = dbvfc;
					dbvfc = default(kpthf);
					yeqcd = -1;
					goto IL_03cf;
				case 5:
					{
						p = dbvfc;
						dbvfc = default(kpthf);
						yeqcd = -1;
						goto IL_04aa;
					}
					IL_03cf:
					p2.ekzxl();
					p2 = default(kpthf);
					ylpoa = bywps.idxfw(vjbha.ywjgg, vjbha.spxke);
					mbkng = bywps.wnmjb();
					bjzgq = new Rebex.Net.DnsEndPoint(ylpoa, mbkng);
					break;
					IL_009c:
					p6.ekzxl();
					p6 = default(kpthf);
					bywps.fosmh(5, "Expected SOCKS protocol version.");
					drrtq = bywps.xzoul<ieaus>();
					bywps.fosmh(0);
					ryrkx = bywps.xzoul<qwdoo>();
					switch (ryrkx)
					{
					case qwdoo.ahajn:
						break;
					case qwdoo.cczgb:
						goto IL_01d7;
					case qwdoo.oteeg:
						goto IL_029d;
					default:
						goto IL_0438;
					}
					p5 = bywps.wdaeu(6).avdby(p1: false).vrtmi();
					if (!p5.zpafv || 1 == 0)
					{
						yeqcd = 1;
						dbvfc = p5;
						unoth.xiwgo(ref p5, ref this);
						flag = false;
						return;
					}
					goto IL_0184;
					IL_0438:
					p = fzpjj.exbnq(zibdr.pskyu, null).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						yeqcd = 5;
						dbvfc = p;
						unoth.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_04aa;
					IL_0249:
					p4.ekzxl();
					p4 = default(kpthf);
					dkbar = bywps.ixnqx(16);
					jxfwl = bywps.wnmjb();
					bjzgq = new IPEndPoint(new IPAddress(dkbar), jxfwl);
					break;
					IL_04aa:
					p.ekzxl();
					p = default(kpthf);
					p7 = null;
					goto end_IL_0000;
					IL_0184:
					p5.ekzxl();
					p5 = default(kpthf);
					coyum = bywps.ixnqx(4);
					jrylx = bywps.wnmjb();
					bjzgq = new IPEndPoint(new IPAddress(coyum), jrylx);
					break;
					IL_029d:
					vjbha = new vymlv();
					vymlv2 = vjbha;
					p3 = bywps.khotx().giftg(p1: false).vuozn();
					if (!p3.hqxbj || 1 == 0)
					{
						vymlv vymlv3 = vymlv2;
						apacs = vymlv3;
						yeqcd = 3;
						wkwjo = p3;
						unoth.xiwgo(ref p3, ref this);
						flag = false;
						return;
					}
					goto IL_033e;
					IL_01d7:
					p4 = bywps.wdaeu(18).avdby(p1: false).vrtmi();
					if (!p4.zpafv || 1 == 0)
					{
						yeqcd = 2;
						dbvfc = p4;
						unoth.xiwgo(ref p4, ref this);
						flag = false;
						return;
					}
					goto IL_0249;
					IL_033e:
					ywjgg = p3.gbccf();
					p3 = default(xuwyj<byte>);
					vymlv2.ywjgg = ywjgg;
					p2 = bywps.wdaeu(vjbha.ywjgg + 2).avdby(p1: false).vrtmi();
					if (!p2.zpafv || 1 == 0)
					{
						yeqcd = 4;
						dbvfc = p2;
						unoth.xiwgo(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_03cf;
				}
				zdrfr = new jrpqk();
				zdrfr.bkand = drrtq;
				zdrfr.idbjf = bjzgq;
				p7 = zdrfr;
				end_IL_0000:;
			}
			catch (Exception p8)
			{
				yeqcd = -2;
				unoth.tudwl(p8);
				return;
			}
			yeqcd = -2;
			unoth.vzyck(p7);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in asgoh
			this.asgoh();
		}

		private void qdzsf(fgyyk p0)
		{
			unoth.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in qdzsf
			this.qdzsf(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CSendCommandResponse_003Ed__39 : fgyyk
	{
		public int yyjyz;

		public ljmxa jerke;

		public kylha kzuun;

		public zibdr aoukf;

		public EndPoint bttfa;

		public ushort trcff;

		public qwdoo oihnp;

		public byte[] iltva;

		public int rrcpb;

		public byte[] puidb;

		private kpthf uwsxh;

		private object azmxs;

		private void gztnt()
		{
			try
			{
				bool flag = true;
				if (yyjyz != 0)
				{
					iltva = fezfv(bttfa, out oihnp, out trcff);
					rrcpb = 6 + iltva.Length;
					puidb = sxztb<byte>.ahblv.vfhlp(rrcpb);
				}
				try
				{
					kpthf p;
					if (yyjyz != 0)
					{
						puidb[0] = 5;
						puidb[1] = (byte)aoukf;
						puidb[2] = 0;
						puidb[3] = (byte)oihnp;
						Array.Copy(iltva, 0, puidb, 4, iltva.Length);
						jlfbq.bktwx(puidb, 4 + iltva.Length, trcff);
						p = kzuun.xaugv.zykkj(new nxtme<byte>(puidb, 0, rrcpb)).avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							yyjyz = 0;
							uwsxh = p;
							jerke.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
					}
					else
					{
						p = uwsxh;
						uwsxh = default(kpthf);
						yyjyz = -1;
					}
					p.ekzxl();
					p = default(kpthf);
				}
				finally
				{
					if (flag && 0 == 0)
					{
						sxztb<byte>.ahblv.uqydw(puidb);
					}
				}
			}
			catch (Exception p2)
			{
				yyjyz = -2;
				jerke.iurqb(p2);
				return;
			}
			yyjyz = -2;
			jerke.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in gztnt
			this.gztnt();
		}

		private void typgy(fgyyk p0)
		{
			jerke.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in typgy
			this.typgy(p0);
		}
	}

	private sealed class nzaqb
	{
		public byte vpwsl;

		public byte maaaq;

		public string rpuua(nxtme<byte> p0)
		{
			return Encoding.ASCII.GetString(p0.lthjd, p0.frlfs, vpwsl);
		}

		public string zkrca(nxtme<byte> p0)
		{
			return Encoding.ASCII.GetString(p0.lthjd, p0.frlfs, maaaq);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CUsernamePasswordAuth_003Ed__45 : fgyyk
	{
		public int uszez;

		public vxvbw<bool> kjsck;

		public pynjh pjzbt;

		public jghfk hykjv;

		public nebnu ncedt;

		public string kmnpd;

		public string jetuj;

		public bool nrsei;

		public zibdr meolc;

		public nzaqb oxfib;

		public kylha tbxdf;

		private kpthf kkanf;

		private object xbquk;

		private xuwyj<string> wkaoo;

		private xuwyj<bool> qqbdt;

		private void mtaos()
		{
			bool p6;
			try
			{
				bool flag = true;
				string text;
				xuwyj<string> p3;
				string text2;
				kpthf p5;
				bool num;
				xuwyj<bool> p2;
				bool flag2;
				kpthf p4;
				kpthf p;
				switch (uszez)
				{
				default:
					oxfib = new nzaqb();
					p5 = pjzbt.wdaeu(2).avdby(p1: false).vrtmi();
					if (!p5.zpafv || 1 == 0)
					{
						uszez = 0;
						kkanf = p5;
						kjsck.xiwgo(ref p5, ref this);
						flag = false;
						return;
					}
					goto IL_00a3;
				case 0:
					p5 = kkanf;
					kkanf = default(kpthf);
					uszez = -1;
					goto IL_00a3;
				case 1:
					p4 = kkanf;
					kkanf = default(kpthf);
					uszez = -1;
					goto IL_0153;
				case 2:
					p3 = wkaoo;
					wkaoo = default(xuwyj<string>);
					uszez = -1;
					goto IL_0231;
				case 3:
					p2 = qqbdt;
					qqbdt = default(xuwyj<bool>);
					uszez = -1;
					goto IL_02c6;
				case 4:
					{
						p = kkanf;
						kkanf = default(kpthf);
						uszez = -1;
						break;
					}
					IL_0231:
					text = p3.gbccf();
					p3 = default(xuwyj<string>);
					text2 = text;
					jetuj = text2;
					p2 = ncedt(kmnpd, jetuj).giftg(p1: false).vuozn();
					if (!p2.hqxbj || 1 == 0)
					{
						uszez = 3;
						qqbdt = p2;
						kjsck.xiwgo(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_02c6;
					IL_00a3:
					p5.ekzxl();
					p5 = default(kpthf);
					pjzbt.fosmh(1);
					oxfib.vpwsl = pjzbt.uopgg();
					p4 = pjzbt.wdaeu(oxfib.vpwsl + 1).avdby(p1: false).vrtmi();
					if (!p4.zpafv || 1 == 0)
					{
						uszez = 1;
						kkanf = p4;
						kjsck.xiwgo(ref p4, ref this);
						flag = false;
						return;
					}
					goto IL_0153;
					IL_02c6:
					num = p2.gbccf();
					p2 = default(xuwyj<bool>);
					flag2 = num;
					nrsei = flag2;
					meolc = ((!nrsei || 1 == 0) ? zibdr.ijuom : zibdr.eizzn);
					p = ysesn(hykjv, 1, (byte)meolc).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						uszez = 4;
						kkanf = p;
						kjsck.xiwgo(ref p, ref this);
						flag = false;
						return;
					}
					break;
					IL_0153:
					p4.ekzxl();
					p4 = default(kpthf);
					kmnpd = pjzbt.idxfw(oxfib.vpwsl, oxfib.rpuua);
					oxfib.maaaq = pjzbt.uopgg();
					p3 = pjzbt.pkqyh(oxfib.maaaq, oxfib.zkrca).giftg(p1: false).vuozn();
					if (!p3.hqxbj || 1 == 0)
					{
						uszez = 2;
						wkaoo = p3;
						kjsck.xiwgo(ref p3, ref this);
						flag = false;
						return;
					}
					goto IL_0231;
				}
				p.ekzxl();
				p = default(kpthf);
				p6 = nrsei;
			}
			catch (Exception p7)
			{
				uszez = -2;
				kjsck.tudwl(p7);
				return;
			}
			uszez = -2;
			kjsck.vzyck(p6);
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in mtaos
			this.mtaos();
		}

		private void mifcf(fgyyk p0)
		{
			kjsck.viwxd(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in mifcf
			this.mifcf(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CSendAllAsync_003Ed__4e : fgyyk
	{
		public int uqxbx;

		public ljmxa aajue;

		public jghfk udzrv;

		public byte dxjzs;

		public byte fjkgt;

		public byte[] vqukz;

		private kpthf epfcp;

		private object cccqo;

		private void ixyyx()
		{
			try
			{
				bool flag = true;
				if (uqxbx != 0)
				{
					vqukz = sxztb<byte>.ahblv.vfhlp(2);
					vqukz[0] = dxjzs;
					vqukz[1] = fjkgt;
				}
				try
				{
					kpthf p;
					if (uqxbx != 0)
					{
						p = udzrv.zykkj(new nxtme<byte>(vqukz, 0, 2)).avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							uqxbx = 0;
							epfcp = p;
							aajue.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
					}
					else
					{
						p = epfcp;
						epfcp = default(kpthf);
						uqxbx = -1;
					}
					p.ekzxl();
					p = default(kpthf);
				}
				finally
				{
					if (flag && 0 == 0)
					{
						sxztb<byte>.ahblv.uqydw(vqukz);
					}
				}
			}
			catch (Exception p2)
			{
				uqxbx = -2;
				aajue.iurqb(p2);
				return;
			}
			uqxbx = -2;
			aajue.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ixyyx
			this.ixyyx();
		}

		private void vofoo(fgyyk p0)
		{
			aajue.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in vofoo
			this.vofoo(p0);
		}
	}

	private const int nvdno = 65536;

	private const byte kilvl = 5;

	private const byte hmmvu = 1;

	private const string ohsxa = "Expected SOCKS protocol version.";

	private static int avyrr;

	private readonly znuay iverh;

	private mggni xaugv;

	private Socket frnta;

	private mggni uawpb;

	private readonly nlsqa qgnau;

	private readonly trheg mhpzn;

	private readonly sjhqe alvus;

	private int zngwx;

	private EndPoint xngvz;

	private EndPoint eajio;

	private static Predicate<byte> reudl;

	internal int mkntd
	{
		get
		{
			return zngwx;
		}
		private set
		{
			zngwx = value;
		}
	}

	public EndPoint dsvpf
	{
		get
		{
			return xngvz;
		}
		private set
		{
			xngvz = value;
		}
	}

	public EndPoint tvmss
	{
		get
		{
			return eajio;
		}
		private set
		{
			eajio = value;
		}
	}

	public kylha(Socket socket, trheg server)
		: this(new xbyeu(socket), server)
	{
		if (socket == null || 1 == 0)
		{
			throw new ArgumentNullException();
		}
		frnta = socket;
	}

	public kylha(mggni clientChannel, trheg server)
	{
		if (clientChannel == null || 1 == 0)
		{
			throw new ArgumentNullException("clientChannel");
		}
		if (server == null || 1 == 0)
		{
			throw new ArgumentNullException("server");
		}
		xaugv = clientChannel;
		iverh = new znuay();
		qgnau = server.kobou;
		mhpzn = server;
		mkntd = Interlocked.Increment(ref avyrr);
		alvus = server.kobou.newbe.ymjug(typeof(kylha), mkntd);
		alvus.byfnx(LogLevel.Debug, "SocksServer", "Creating SOCKS5 session #{0} (server #{1}).", mkntd, mhpzn.aoral);
	}

	public void hqfdd()
	{
		iverh.pvutk();
		ntqtl(ref xaugv, "client channel");
		ntqtl(ref frnta, "client socket");
		ntqtl(ref uawpb, "host channel");
	}

	public void Dispose()
	{
		hqfdd();
		alvus.byfnx(LogLevel.Debug, "SocksServer", "SOCKS5 session #{0} ended.", mkntd);
	}

	[vtsnh(typeof(_003CProcess_003Ed__0))]
	public exkzi gnenj()
	{
		_003CProcess_003Ed__0 p = default(_003CProcess_003Ed__0);
		p.anbgm = this;
		p.jseod = ljmxa.nmskg();
		p.vdpjs = -1;
		ljmxa jseod = p.jseod;
		jseod.nncuo(ref p);
		return p.jseod.donjp;
	}

	private exkzi khumi(mggni p0)
	{
		return rxpjc.ybfqj(vuuyj(xaugv, p0, 65536, p3: true), vuuyj(p0, xaugv, 65536, p3: false));
	}

	[vtsnh(typeof(_003CBridgeChannels_003Ed__b))]
	private exkzi vuuyj(mggni p0, mggni p1, int p2, bool p3)
	{
		_003CBridgeChannels_003Ed__b p4 = default(_003CBridgeChannels_003Ed__b);
		p4.nbqvf = this;
		p4.fgxuv = p0;
		p4.jwyvx = p1;
		p4.vmvqy = p2;
		p4.hmvxz = ljmxa.nmskg();
		p4.eveyh = -1;
		ljmxa hmvxz = p4.hmvxz;
		hmvxz.nncuo(ref p4);
		return p4.hmvxz.donjp;
	}

	private static byte[] fezfv(EndPoint p0, out qwdoo p1, out ushort p2)
	{
		if (p0 == null || 1 == 0)
		{
			p1 = qwdoo.ahajn;
			p2 = 0;
			return IPAddress.Any.GetAddressBytes();
		}
		if (p0 is IPEndPoint iPEndPoint && 0 == 0)
		{
			byte[] addressBytes = iPEndPoint.Address.GetAddressBytes();
			p1 = ((addressBytes.Length == 4) ? qwdoo.ahajn : qwdoo.cczgb);
			p2 = (ushort)iPEndPoint.Port;
			return addressBytes;
		}
		if (p0 is Rebex.Net.DnsEndPoint dnsEndPoint && 0 == 0)
		{
			byte[] array = new byte[1 + dnsEndPoint.Host.Length];
			array[0] = (byte)dnsEndPoint.Host.Length;
			Encoding.ASCII.GetBytes(dnsEndPoint.Host, 0, dnsEndPoint.Host.Length, array, 1);
			p1 = qwdoo.oteeg;
			p2 = (ushort)dnsEndPoint.Port;
			return array;
		}
		throw new NotSupportedException();
	}

	[vtsnh(typeof(_003CExecuteBindCommand_003Ed__12))]
	private exkzi klajg(EndPoint p0)
	{
		_003CExecuteBindCommand_003Ed__12 p1 = default(_003CExecuteBindCommand_003Ed__12);
		p1.nnnga = this;
		p1.utwel = p0;
		p1.hlzac = ljmxa.nmskg();
		p1.xicsz = -1;
		ljmxa hlzac = p1.hlzac;
		hlzac.nncuo(ref p1);
		return p1.hlzac.donjp;
	}

	[vtsnh(typeof(_003CExecuteConnectCommand_003Ed__1b))]
	private exkzi ysuoe(EndPoint p0)
	{
		_003CExecuteConnectCommand_003Ed__1b p1 = default(_003CExecuteConnectCommand_003Ed__1b);
		p1.wkmwy = this;
		p1.ywcrn = p0;
		p1.hnpwl = ljmxa.nmskg();
		p1.spwou = -1;
		ljmxa hnpwl = p1.hnpwl;
		hnpwl.nncuo(ref p1);
		return p1.hnpwl.donjp;
	}

	[vtsnh(typeof(_003CNegotiateAuthMethod_003Ed__23))]
	private njvzu<eqttv> myonb(pynjh p0, jghfk p1)
	{
		_003CNegotiateAuthMethod_003Ed__23 p2 = default(_003CNegotiateAuthMethod_003Ed__23);
		p2.qfaxr = this;
		p2.pqzzk = p0;
		p2.kpigp = p1;
		p2.zyvdq = vxvbw<eqttv>.rdzxj();
		p2.xkfjj = -1;
		vxvbw<eqttv> zyvdq = p2.zyvdq;
		zyvdq.vklen(ref p2);
		return p2.zyvdq.xieya;
	}

	[vtsnh(typeof(_003CNegotiateCommand_003Ed__2c))]
	private njvzu<jrpqk> ziznj(pynjh p0)
	{
		_003CNegotiateCommand_003Ed__2c p1 = default(_003CNegotiateCommand_003Ed__2c);
		p1.fzpjj = this;
		p1.bywps = p0;
		p1.unoth = vxvbw<jrpqk>.rdzxj();
		p1.yeqcd = -1;
		vxvbw<jrpqk> unoth = p1.unoth;
		unoth.vklen(ref p1);
		return p1.unoth.xieya;
	}

	private void ntqtl<T>(ref T p0, string p1) where T : class
	{
		if (!(Interlocked.Exchange(ref p0, null) is IDisposable disposable))
		{
			return;
		}
		try
		{
			disposable.Dispose();
		}
		catch (Exception ex)
		{
			alvus.byfnx(LogLevel.Info, "SocksServer", "Unexpected error while disposing {0}: {1}", p1, ex);
		}
	}

	[vtsnh(typeof(_003CSendCommandResponse_003Ed__39))]
	private exkzi exbnq(zibdr p0, EndPoint p1)
	{
		_003CSendCommandResponse_003Ed__39 p2 = default(_003CSendCommandResponse_003Ed__39);
		p2.kzuun = this;
		p2.aoukf = p0;
		p2.bttfa = p1;
		p2.jerke = ljmxa.nmskg();
		p2.yyjyz = -1;
		ljmxa jerke = p2.jerke;
		jerke.nncuo(ref p2);
		return p2.jerke.donjp;
	}

	[vtsnh(typeof(_003CUsernamePasswordAuth_003Ed__45))]
	private njvzu<bool> idadz(pynjh p0, jghfk p1, nebnu p2)
	{
		_003CUsernamePasswordAuth_003Ed__45 p3 = default(_003CUsernamePasswordAuth_003Ed__45);
		p3.tbxdf = this;
		p3.pjzbt = p0;
		p3.hykjv = p1;
		p3.ncedt = p2;
		p3.kjsck = vxvbw<bool>.rdzxj();
		p3.uszez = -1;
		vxvbw<bool> kjsck = p3.kjsck;
		kjsck.vklen(ref p3);
		return p3.kjsck.xieya;
	}

	[vtsnh(typeof(_003CSendAllAsync_003Ed__4e))]
	private static exkzi ysesn(jghfk p0, byte p1, byte p2)
	{
		_003CSendAllAsync_003Ed__4e p3 = default(_003CSendAllAsync_003Ed__4e);
		p3.udzrv = p0;
		p3.dxjzs = p1;
		p3.fjkgt = p2;
		p3.aajue = ljmxa.nmskg();
		p3.uqxbx = -1;
		ljmxa aajue = p3.aajue;
		aajue.nncuo(ref p3);
		return p3.aajue.donjp;
	}

	private static bool rergw(byte p0)
	{
		return p0 != 0;
	}
}
