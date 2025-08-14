using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Rebex;
using Rebex.Net;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class jyhtt : ortbm, mwxgh
{
	[StructLayout(LayoutKind.Auto)]
	private struct _003COnHandshakeReceived_003Ed__0 : fgyyk
	{
		public int uikqt;

		public ljmxa rumtj;

		public jyhtt klfsq;

		public byte[] fxttr;

		public int mtvpc;

		public int tqnwn;

		public ofuit fryvl;

		private kpthf sbjve;

		private object vuphq;

		private void kmatn()
		{
			try
			{
				bool flag = true;
				kpthf p2;
				kpthf p;
				switch (uikqt)
				{
				default:
					fryvl = ofuit.paptc(fxttr, mtvpc, tqnwn, klfsq.xkrqa, klfsq.gjtmk);
					if (fryvl == null || 1 == 0)
					{
						throw new TlsException(mjddr.ypibb, "Received unknown handshake message.");
					}
					if (klfsq.exdje.Entity == TlsConnectionEnd.Client)
					{
						p2 = klfsq.ftdrl(fxttr, mtvpc, tqnwn, fryvl).avdby(p1: false).vrtmi();
						if (!p2.zpafv || 1 == 0)
						{
							uikqt = 0;
							sbjve = p2;
							rumtj.wqiyk(ref p2, ref this);
							flag = false;
							return;
						}
						goto IL_0108;
					}
					p = klfsq.jlkqe(fxttr, mtvpc, tqnwn, fryvl).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						uikqt = 1;
						sbjve = p;
						rumtj.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
				case 0:
					p2 = sbjve;
					sbjve = default(kpthf);
					uikqt = -1;
					goto IL_0108;
				case 1:
					{
						p = sbjve;
						sbjve = default(kpthf);
						uikqt = -1;
						break;
					}
					IL_0108:
					p2.ekzxl();
					p2 = default(kpthf);
					goto end_IL_0000;
				}
				p.ekzxl();
				p = default(kpthf);
				end_IL_0000:;
			}
			catch (Exception p3)
			{
				uikqt = -2;
				rumtj.iurqb(p3);
				return;
			}
			uikqt = -2;
			rumtj.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in kmatn
			this.kmatn();
		}

		private void qiwxs(fgyyk p0)
		{
			rumtj.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in qiwxs
			this.qiwxs(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003COnHandshakeReceivedServer_003Ed__6 : fgyyk
	{
		public int jkzyg;

		public ljmxa hwshc;

		public jyhtt nqaae;

		public byte[] irjtl;

		public int qzahe;

		public int nlnbu;

		public ofuit flygj;

		public MemoryStream kgitc;

		public byte[] azosq;

		public euwhd rwdsz;

		public webdt hwuuq;

		public byte[] mzmyu;

		public wmbjj vownu;

		public CertificateChain beesh;

		public string jfhtg;

		public nxtme<string> skogv;

		public smtii iayro;

		public nfbik iupih;

		public TlsKeyExchangeAlgorithm lbqeh;

		public eupdk qvxou;

		public cujbv frybl;

		public eupdk bvhsx;

		public vssis mmtsn;

		public nfbik wayfk;

		private kpthf bivmj;

		private object zsymm;

		private void tiktg()
		{
			try
			{
				bool flag = true;
				kpthf p6;
				CertificateChain certificateChain;
				kpthf p11;
				kpthf p10;
				kpthf p4;
				kpthf p9;
				kpthf p8;
				kpthf p3;
				kpthf p7;
				kpthf p2;
				kpthf p5;
				kpthf p;
				switch (jkzyg)
				{
				default:
				{
					kgitc = null;
					switch (flygj.zijgx)
					{
					case nsvut.enndd:
						kgitc = new MemoryStream(nqaae.jakzf.GetBuffer(), 0, (int)nqaae.jakzf.Length, writable: false, publiclyVisible: true);
						goto default;
					case nsvut.lnjcv:
						nqaae.jakzf = new MemoryStream();
						goto default;
					default:
						nqaae.qsytu(irjtl, qzahe, nlnbu);
						break;
					case nsvut.rvoru:
					case nsvut.oaufz:
						break;
					}
					switch (flygj.zijgx)
					{
					case nsvut.rvoru:
						nqaae.gffnx.kxlvu(TlsDebugEventType.HelloRequest, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, irjtl, qzahe, nlnbu);
						goto end_IL_0000;
					case nsvut.lnjcv:
						break;
					case nsvut.jthry:
						nqaae.gffnx.kxlvu(TlsDebugEventType.ServerHello, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, irjtl, qzahe, nlnbu);
						throw new TlsException(mjddr.ypibb, brgjd.edcru("Received {0} from a client.", "ServerHello"));
					case nsvut.upgjx:
						goto IL_12e3;
					case nsvut.zmyoj:
						try
						{
							nqaae.qgfwn = (ypafr)flygj;
							nqaae.gffnx.kxlvu(TlsDebugEventType.ClientKeyExchange, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, irjtl, qzahe, nlnbu);
							if (nqaae.exdje.CertificatePolicy == TlsCertificatePolicy.RequireClientCertificate && (nqaae.jqjbc == null || 1 == 0))
							{
								throw new TlsException(mjddr.vyvjd);
							}
							sgjhx sgjhx2 = null;
							byte[] array = null;
							TlsKeyExchangeAlgorithm keyExchangeAlgorithm = nqaae.gjtmk.KeyExchangeAlgorithm;
							if (keyExchangeAlgorithm != TlsKeyExchangeAlgorithm.RSA)
							{
								try
								{
									if (keyExchangeAlgorithm == TlsKeyExchangeAlgorithm.ECDHE_RSA || keyExchangeAlgorithm == TlsKeyExchangeAlgorithm.ECDHE_ECDSA)
									{
										byte[] array2 = nqaae.qgfwn.ubcyg(p0: true);
										array = nqaae.nutgc.qpwqy(array2);
										if (array == null || 1 == 0)
										{
											KeyMaterialDeriver keyMaterialDeriver = nqaae.nutgc.GetKeyMaterialDeriver(array2);
											sgjhx2 = new tmtag(keyMaterialDeriver);
										}
									}
									else
									{
										byte[] p12 = nqaae.qgfwn.ubcyg(p0: false);
										array = nqaae.fqlki(nqaae.nutgc, null, p12);
									}
								}
								catch
								{
									array = null;
								}
							}
							else
							{
								try
								{
									if (nqaae.ugnwx == null || 1 == 0)
									{
										nqaae.qgfwn.uecax(nqaae.rnzei.hdraz, nqaae.wprub.LeafCertificate);
									}
									else
									{
										nqaae.qgfwn.isbyb(nqaae.rnzei.hdraz, nqaae.nutgc);
										nqaae.nutgc.Dispose();
									}
									array = nqaae.qgfwn.mujic;
								}
								catch
								{
									array = null;
								}
								if (array != null && 0 == 0)
								{
									if (array != null && 0 == 0 && array.Length != 48)
									{
										Array.Clear(array, 0, array.Length);
										array = null;
									}
									else if ((nqaae.exdje.Options & TlsOptions.SkipRollbackDetection) != TlsOptions.None && 0 == 0)
									{
										int num = (array[0] << 8) + array[1];
										if (num != nqaae.rnzei.hdraz)
										{
											Array.Clear(array, 0, array.Length);
											array = null;
										}
									}
								}
							}
							if (sgjhx2 == null || 1 == 0)
							{
								if (array == null || 1 == 0)
								{
									array = new byte[48];
									jtxhe.ubsib(array, 0, 48);
								}
								sgjhx2 = new onash(array);
							}
							nqaae.ydiid = new fldyu(nqaae.gjtmk, TlsConnectionEnd.Server, nqaae.sjzby.ioriz, nqaae.rnzei.okraw);
							nqaae.ydiid.opzff(sgjhx2, (nqaae.fbjcb ? true : false) ? nqaae.jakzf : null);
							nqaae.ydiid.ybvgj();
							nqaae.ihqij = nqaae.ydiid.juoso;
							nqaae.gvvsl = nqaae.ydiid.pbohw;
						}
						finally
						{
							if (flag && 0 == 0)
							{
								if (nqaae.nutgc != null && 0 == 0)
								{
									nqaae.nutgc.Dispose();
								}
								if (nqaae.wprub != null && 0 == 0)
								{
									nqaae.wprub = nqaae.wprub.qdige();
								}
							}
						}
						goto end_IL_0000;
					case nsvut.enndd:
						goto IL_16fc;
					case nsvut.oaufz:
						goto IL_182e;
					default:
						nqaae.gffnx.kxlvu(TlsDebugEventType.UnknownHandshakeMessage, TlsDebugEventSource.Received, TlsDebugLevel.Important, irjtl, qzahe, nlnbu);
						goto end_IL_0000;
					}
					nqaae.sjzby = (aoind)flygj;
					if (!nqaae.exdje.aljgi || 1 == 0)
					{
						nqaae.gffnx.kxlvu(TlsDebugEventType.ClientHello, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, irjtl, qzahe, nlnbu);
					}
					hwuuq = nqaae.exdje.auypu();
					mzmyu = null;
					nqaae.jbiwm = null;
					nqaae.yhqed = false;
					if (hwuuq != null && 0 == 0 && nqaae.sjzby.kchbb.Length > 0)
					{
						TlsSession tlsSession = hwuuq.bfjil(nqaae.sjzby.kchbb);
						if (tlsSession != null && 0 == 0)
						{
							nqaae.jbiwm = tlsSession;
							nqaae.yhqed = true;
							nqaae.drmar = (nqaae.fbjcb = tlsSession.zqpiy);
							mzmyu = nqaae.jbiwm.wqpxu;
							nqaae.gffnx.ivtmn(TlsDebugEventType.ResumingCachedSession, TlsDebugEventSource.Unspecified, TlsDebugLevel.Important);
						}
					}
					if (mzmyu == null || 1 == 0)
					{
						mzmyu = new byte[32];
						jtxhe.ubsib(mzmyu, 0, 32);
						if (hwuuq != null && 0 == 0)
						{
							hwuuq.lacjz(mzmyu, null);
						}
					}
					vownu = new wmbjj();
					if (nqaae.qfutg && 0 == 0)
					{
						nqaae.chsrv(TlsConnectionEnd.Server);
						if (nqaae.fmqju && 0 == 0)
						{
							ceuvv(vownu, nqaae.qaqnn, nqaae.nahbm);
						}
					}
					if (nqaae.drmar && 0 == 0 && nqaae.sjzby.szwbv >= 769)
					{
						nqaae.zalaq(TlsConnectionEnd.Server, nqaae.yhqed);
						if (nqaae.fbjcb && 0 == 0)
						{
							jwkpf(vownu);
						}
					}
					beesh = null;
					jfhtg = nqaae.sjzby.vfiwv();
					skogv = nqaae.sjzby.nihvb();
					iayro = new smtii(jfhtg, skogv, nqaae.exdje.qopqb, isCertificateRequired: true);
					nqaae.exdje.vplfc(iayro);
					nqaae.exdje.ylflp(iayro.vubgy);
					nxtme<CertificateChain> vubgy = iayro.vubgy;
					if (yhptp == null || 1 == 0)
					{
						yhptp = wiwtv;
					}
					beesh = vubgy.xoiju(yhptp);
					nqaae.iagyo = iayro.vwhek;
					nqaae.uoanx(vownu, nqaae.iagyo);
					nqaae.rnzei = new kzdrw(nqaae.exdje, beesh.LeafCertificate, nqaae.sjzby, mzmyu, vownu.ihelo(), out nqaae.gjtmk);
					nqaae.zdprx = nqaae.sjzby.szwbv;
					if (nqaae.zdprx >= 771)
					{
						nqaae.urayu = nqaae.sjzby.dvlkv();
					}
					else
					{
						nqaae.urayu = nxtme<byte>.gihlo;
					}
					p11 = nqaae.hqyqj(nqaae.rnzei).avdby(p1: false).vrtmi();
					if (!p11.zpafv || 1 == 0)
					{
						jkzyg = 0;
						bivmj = p11;
						hwshc.wqiyk(ref p11, ref this);
						flag = false;
						return;
					}
					goto IL_05eb;
				}
				case 0:
					p11 = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_05eb;
				case 1:
					p10 = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_068d;
				case 2:
					p9 = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_080e;
				case 3:
					p8 = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_08ab;
				case 4:
					p7 = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_0992;
				case 5:
					p6 = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_0a12;
				case 6:
					p5 = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_0aec;
				case 7:
					p4 = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_0b89;
				case 8:
					p3 = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_0f37;
				case 9:
					p2 = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_0fda;
				case 10:
					p = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_10a4;
				case 11:
					p = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_1142;
				case 12:
					p = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_11de;
				case 13:
					p = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_1281;
				case 14:
					p = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_1997;
				case 15:
					p = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_1a35;
				case 16:
					p = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_1b3a;
				case 17:
					p = bivmj;
					bivmj = default(kpthf);
					jkzyg = -1;
					goto IL_1bbb;
				case 18:
					{
						p = bivmj;
						bivmj = default(kpthf);
						jkzyg = -1;
						break;
					}
					IL_0a12:
					p6.ekzxl();
					p6 = default(kpthf);
					goto end_IL_0000;
					IL_182e:
					rwdsz = (euwhd)flygj;
					nqaae.gffnx.kxlvu(TlsDebugEventType.Finished, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, irjtl, qzahe, nlnbu);
					if (nqaae.ihqij != null || nqaae.mmbtf == bpnki.yiqfh)
					{
						throw new TlsException(mjddr.ypibb, "Received Finished, but the preceding ChangeCipherSpec was missing.");
					}
					azosq = nqaae.ydiid.wgqsm(nqaae.jakzf, TlsConnectionEnd.Client);
					nqaae.qaqnn = azosq;
					if (!nqaae.yhqed || 1 == 0)
					{
						if (!jtxhe.hbsgb(azosq, rwdsz.lkwbh) || 1 == 0)
						{
							throw new TlsException(mjddr.wmgut, "Invalid Finished verify data.");
						}
						wayfk = new nfbik();
						p = nqaae.hqyqj(wayfk).avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							jkzyg = 14;
							bivmj = p;
							hwshc.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
						goto IL_1997;
					}
					goto IL_1bca;
					IL_16fc:
					nqaae.gffnx.kxlvu(TlsDebugEventType.CertificateVerify, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, irjtl, qzahe, nlnbu);
					mmtsn = (vssis)flygj;
					if (nqaae.jqjbc == null || 1 == 0)
					{
						throw new TlsException(mjddr.fvtwt);
					}
					certificateChain = nqaae.jqjbc.qdige();
					try
					{
						Certificate leafCertificate = certificateChain.LeafCertificate;
						mmtsn.hrbim(leafCertificate.KeyAlgorithm, out var p13, out var p14);
						if (!ofuit.mhozp(nqaae.urayu, leafCertificate.KeyAlgorithm, p14) || 1 == 0)
						{
							throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
						}
						byte[] hash = nqaae.ydiid.wfrbn(kgitc, p13);
						if (!leafCertificate.VerifyHash(hash, p14, mmtsn.gksmh) || 1 == 0)
						{
							throw new TlsException(mjddr.fvtwt);
						}
						nqaae.syoyz(null, "client", certificateChain);
					}
					finally
					{
						if (flag && 0 == 0 && certificateChain != null && 0 == 0)
						{
							((IDisposable)certificateChain).Dispose();
						}
					}
					goto end_IL_0000;
					IL_05eb:
					p11.ekzxl();
					p11 = default(kpthf);
					nqaae.gffnx.jkyaz(TlsDebugEventType.ServerHello, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, nqaae.rnzei);
					p10 = nqaae.zupbs().avdby(p1: false).vrtmi();
					if (!p10.zpafv || 1 == 0)
					{
						jkzyg = 1;
						bivmj = p10;
						hwshc.wqiyk(ref p10, ref this);
						flag = false;
						return;
					}
					goto IL_068d;
					IL_0fe9:
					if (nqaae.exdje.CertificatePolicy != TlsCertificatePolicy.NoClientCertificate && 0 == 0)
					{
						frybl = new cujbv(nqaae.exdje.AcceptableAuthorities, nqaae.xkrqa);
						p = nqaae.hqyqj(frybl).avdby(p1: false).vrtmi();
						if (!p.zpafv || 1 == 0)
						{
							jkzyg = 10;
							bivmj = p;
							hwshc.wqiyk(ref p, ref this);
							flag = false;
							return;
						}
						goto IL_10a4;
					}
					goto IL_1151;
					IL_1281:
					p.ekzxl();
					p = default(kpthf);
					goto end_IL_0000;
					IL_068d:
					p10.ekzxl();
					p10 = default(kpthf);
					if (nqaae.yhqed && 0 == 0)
					{
						nqaae.wprub = nqaae.jbiwm.twwdt;
						nqaae.jqjbc = nqaae.jbiwm.xalxj;
						nqaae.ydiid = new fldyu(nqaae.gjtmk, TlsConnectionEnd.Server, nqaae.sjzby.ioriz, nqaae.rnzei.okraw);
						nqaae.ydiid.qqffv = nqaae.jbiwm.tydvs;
						nqaae.ydiid.ybvgj();
						nqaae.ihqij = nqaae.ydiid.juoso;
						nqaae.gvvsl = nqaae.ydiid.pbohw;
						iupih = new nfbik();
						p9 = nqaae.hqyqj(iupih).avdby(p1: false).vrtmi();
						if (!p9.zpafv || 1 == 0)
						{
							jkzyg = 2;
							bivmj = p9;
							hwshc.wqiyk(ref p9, ref this);
							flag = false;
							return;
						}
						goto IL_080e;
					}
					lbqeh = nqaae.gjtmk.KeyExchangeAlgorithm;
					if (lbqeh != TlsKeyExchangeAlgorithm.DH_anon)
					{
						nqaae.wprub = beesh;
						qvxou = new eupdk(beesh, nqaae.exdje);
						p5 = nqaae.hqyqj(qvxou).avdby(p1: false).vrtmi();
						if (!p5.zpafv || 1 == 0)
						{
							jkzyg = 6;
							bivmj = p5;
							hwshc.wqiyk(ref p5, ref this);
							flag = false;
							return;
						}
						goto IL_0aec;
					}
					goto IL_0b98;
					IL_12e3:
					bvhsx = (eupdk)flygj;
					nqaae.gffnx.kxlvu(TlsDebugEventType.Certificate, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, irjtl, qzahe, nlnbu);
					if (nqaae.exdje.CertificatePolicy == TlsCertificatePolicy.NoClientCertificate || 1 == 0)
					{
						throw new TlsException(mjddr.ypibb);
					}
					nqaae.jqjbc = bvhsx.tycix();
					goto end_IL_0000;
					IL_0b89:
					p4.ekzxl();
					p4 = default(kpthf);
					goto IL_0b98;
					IL_10a4:
					p.ekzxl();
					p = default(kpthf);
					nqaae.gffnx.jkyaz(TlsDebugEventType.CertificateRequest, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, frybl);
					p = nqaae.zupbs().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						jkzyg = 11;
						bivmj = p;
						hwshc.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_1142;
					IL_080e:
					p9.ekzxl();
					p9 = default(kpthf);
					nqaae.gffnx.jkyaz(TlsDebugEventType.ChangeCipherSpec, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, iupih);
					p8 = nqaae.zupbs().avdby(p1: false).vrtmi();
					if (!p8.zpafv || 1 == 0)
					{
						jkzyg = 3;
						bivmj = p8;
						hwshc.wqiyk(ref p8, ref this);
						flag = false;
						return;
					}
					goto IL_08ab;
					IL_1997:
					p.ekzxl();
					p = default(kpthf);
					nqaae.gffnx.jkyaz(TlsDebugEventType.ChangeCipherSpec, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, wayfk);
					p = nqaae.zupbs().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						jkzyg = 15;
						bivmj = p;
						hwshc.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_1a35;
					IL_08ab:
					p8.ekzxl();
					p8 = default(kpthf);
					azosq = nqaae.ydiid.wgqsm(nqaae.jakzf, TlsConnectionEnd.Server);
					nqaae.nahbm = azosq;
					rwdsz = new euwhd(azosq);
					nqaae.gffnx.jkyaz(TlsDebugEventType.Finished, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, rwdsz);
					p7 = nqaae.hqyqj(rwdsz).avdby(p1: false).vrtmi();
					if (!p7.zpafv || 1 == 0)
					{
						jkzyg = 4;
						bivmj = p7;
						hwshc.wqiyk(ref p7, ref this);
						flag = false;
						return;
					}
					goto IL_0992;
					IL_1a35:
					p.ekzxl();
					p = default(kpthf);
					nqaae.qsytu(irjtl, qzahe, nlnbu);
					azosq = nqaae.ydiid.wgqsm(nqaae.jakzf, TlsConnectionEnd.Server);
					nqaae.nahbm = azosq;
					rwdsz = new euwhd(azosq);
					nqaae.gffnx.jkyaz(TlsDebugEventType.Finished, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, rwdsz);
					p = nqaae.hqyqj(rwdsz).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						jkzyg = 16;
						bivmj = p;
						hwshc.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_1b3a;
					IL_0b98:
					nqaae.nutgc = null;
					if (lbqeh == TlsKeyExchangeAlgorithm.RSA)
					{
						if (nqaae.gjtmk.Exportable && 0 == 0 && beesh[0].GetKeySize() > nqaae.gjtmk.zorll)
						{
							nqaae.nutgc = new AsymmetricKeyAlgorithm();
							nqaae.nutgc.ImportKey(nqaae.exdje.TemporaryRSAParameters);
							RSAParameters rSAParameters = nqaae.nutgc.GetPublicKey().GetRSAParameters();
							nqaae.ugnwx = new ccgaj(beesh[0], rSAParameters, nqaae.sjzby.ioriz, nqaae.rnzei.okraw, nqaae.xkrqa);
						}
					}
					else if (lbqeh == TlsKeyExchangeAlgorithm.ECDHE_RSA || lbqeh == TlsKeyExchangeAlgorithm.ECDHE_ECDSA)
					{
						urofm urofm2 = nqaae.rnzei.izerh ?? ((urofm)0);
						if (urofm2 == (urofm)0 || 1 == 0)
						{
							throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
						}
						nqaae.exdje.tokwu(urofm2, out var _, out var p16, out var p17, out var _);
						nqaae.bvdcq(LogLevel.Debug, "Using ephemeral ECDH public key exchange with {0}.", p17);
						nqaae.nutgc = new AsymmetricKeyAlgorithm();
						nqaae.nutgc.GenerateKey(AsymmetricKeyAlgorithmId.ECDH, p16);
						nqaae.ugnwx = new ccgaj(beesh[0], nqaae.urayu, urofm2, nqaae.nutgc.zimkk(), nqaae.sjzby.ioriz, nqaae.rnzei.okraw, nqaae.xkrqa);
					}
					else
					{
						nqaae.nutgc = new AsymmetricKeyAlgorithm();
						DiffieHellmanParameters diffieHellmanParameters = new DiffieHellmanParameters
						{
							P = (byte[])nqaae.exdje.EphemeralDiffieHellmanParameters.P.Clone(),
							G = (byte[])nqaae.exdje.EphemeralDiffieHellmanParameters.G.Clone()
						};
						nqaae.nutgc.ImportKey(diffieHellmanParameters);
						diffieHellmanParameters.Y = nqaae.fyuvk(nqaae.nutgc);
						if (nqaae.gjtmk.KeyExchangeAlgorithm != TlsKeyExchangeAlgorithm.DH_anon)
						{
							nqaae.ugnwx = new ccgaj(beesh[0], nqaae.urayu, diffieHellmanParameters, nqaae.sjzby.ioriz, nqaae.rnzei.okraw, nqaae.xkrqa);
						}
						else
						{
							nqaae.ugnwx = new ccgaj(diffieHellmanParameters);
						}
					}
					if (nqaae.ugnwx != null && 0 == 0)
					{
						p3 = nqaae.hqyqj(nqaae.ugnwx).avdby(p1: false).vrtmi();
						if (!p3.zpafv || 1 == 0)
						{
							jkzyg = 8;
							bivmj = p3;
							hwshc.wqiyk(ref p3, ref this);
							flag = false;
							return;
						}
						goto IL_0f37;
					}
					goto IL_0fe9;
					IL_1b3a:
					p.ekzxl();
					p = default(kpthf);
					p = nqaae.zupbs().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						jkzyg = 17;
						bivmj = p;
						hwshc.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_1bbb;
					IL_0f37:
					p3.ekzxl();
					p3 = default(kpthf);
					nqaae.gffnx.jkyaz(TlsDebugEventType.ServerKeyExchange, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, nqaae.ugnwx);
					p2 = nqaae.zupbs().avdby(p1: false).vrtmi();
					if (!p2.zpafv || 1 == 0)
					{
						jkzyg = 9;
						bivmj = p2;
						hwshc.wqiyk(ref p2, ref this);
						flag = false;
						return;
					}
					goto IL_0fda;
					IL_1bbb:
					p.ekzxl();
					p = default(kpthf);
					goto IL_1bca;
					IL_1bca:
					nqaae.jakzf = null;
					nqaae.rirto = ((nqaae.exdje.Options & TlsOptions.DoNotInsertEmptyFragment) == 0 || 1 == 0) && nqaae.gjtmk.Cbc && nqaae.rnzei.hdraz <= 769;
					if (!nqaae.yhqed || 1 == 0)
					{
						webdt webdt2 = nqaae.exdje.auypu();
						if (webdt2 != null && 0 == 0)
						{
							TlsSession p19 = new TlsSession(nqaae.rnzei.finbq, nqaae.ydiid.qqffv, nqaae.wprub, nqaae.jqjbc, nqaae.fbjcb);
							webdt2.lacjz(nqaae.rnzei.finbq, p19);
						}
					}
					nqaae.ywqqr = true;
					p = nqaae.soeay().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						jkzyg = 18;
						bivmj = p;
						hwshc.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
					IL_1151:
					nqaae.laixd = new watdq();
					p = nqaae.hqyqj(nqaae.laixd).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						jkzyg = 12;
						bivmj = p;
						hwshc.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_11de;
					IL_0992:
					p7.ekzxl();
					p7 = default(kpthf);
					p6 = nqaae.zupbs().avdby(p1: false).vrtmi();
					if (!p6.zpafv || 1 == 0)
					{
						jkzyg = 5;
						bivmj = p6;
						hwshc.wqiyk(ref p6, ref this);
						flag = false;
						return;
					}
					goto IL_0a12;
					IL_0fda:
					p2.ekzxl();
					p2 = default(kpthf);
					goto IL_0fe9;
					IL_11de:
					p.ekzxl();
					p = default(kpthf);
					nqaae.gffnx.jkyaz(TlsDebugEventType.ServerHelloDone, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, nqaae.laixd);
					p = nqaae.zupbs().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						jkzyg = 13;
						bivmj = p;
						hwshc.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_1281;
					IL_0aec:
					p5.ekzxl();
					p5 = default(kpthf);
					nqaae.gffnx.jkyaz(TlsDebugEventType.Certificate, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, qvxou);
					p4 = nqaae.zupbs().avdby(p1: false).vrtmi();
					if (!p4.zpafv || 1 == 0)
					{
						jkzyg = 7;
						bivmj = p4;
						hwshc.wqiyk(ref p4, ref this);
						flag = false;
						return;
					}
					goto IL_0b89;
					IL_1142:
					p.ekzxl();
					p = default(kpthf);
					goto IL_1151;
				}
				p.ekzxl();
				p = default(kpthf);
				end_IL_0000:;
			}
			catch (Exception p20)
			{
				jkzyg = -2;
				hwshc.iurqb(p20);
				return;
			}
			jkzyg = -2;
			hwshc.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in tiktg
			this.tiktg();
		}

		private void fyugs(fgyyk p0)
		{
			hwshc.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in fyugs
			this.fyugs(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003COnHandshakeReceivedClient_003Ed__1a : fgyyk
	{
		public int lfyrj;

		public ljmxa lwfmt;

		public jyhtt lpqes;

		public byte[] vjgil;

		public int murhd;

		public int cvzwp;

		public ofuit xwmpy;

		public byte[] nftdp;

		public euwhd dxing;

		public bool ionuy;

		public eupdk kwfav;

		public Certificate gvkem;

		public KeyAlgorithm jaide;

		public TlsKeyExchangeAlgorithm gkcdn;

		public CertificateChain vwjen;

		public Certificate vyzcb;

		public bool gwikt;

		public AsymmetricKeyAlgorithm ycgit;

		public AsymmetricKeyAlgorithm tbmsf;

		public byte[] usezv;

		public byte[] lbfjc;

		public bool bwapa;

		public sgjhx klovs;

		public eupdk iqtmy;

		public zppmb cfbon;

		public Certificate robzd;

		public HashingAlgorithmId liycq;

		public SignatureHashAlgorithm xxwdd;

		public byte[] nibxf;

		public byte[] hbcvq;

		public vssis shpkf;

		public nfbik oueks;

		public nfbik uzhwn;

		private kpthf oatnq;

		private object vtabd;

		private void usnlc()
		{
			try
			{
				bool flag = true;
				kpthf p5;
				kpthf p10;
				kpthf p6;
				kpthf p4;
				kpthf p7;
				kpthf p3;
				kpthf p9;
				kpthf p2;
				kpthf p8;
				kpthf p11;
				kpthf p;
				switch (lfyrj)
				{
				default:
				{
					nsvut zijgx = xwmpy.zijgx;
					if (zijgx != nsvut.rvoru && zijgx != nsvut.oaufz)
					{
						lpqes.qsytu(vjgil, murhd, cvzwp);
					}
					switch (xwmpy.zijgx)
					{
					case nsvut.rvoru:
						break;
					case nsvut.lnjcv:
						lpqes.gffnx.kxlvu(TlsDebugEventType.ClientHello, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, vjgil, murhd, cvzwp);
						throw new TlsException(mjddr.ypibb, brgjd.edcru("Received {0} from a server.", "ClientHello"));
					case nsvut.jthry:
						goto IL_01e4;
					case nsvut.upgjx:
						goto IL_051d;
					case nsvut.iysge:
						lpqes.ugnwx = (ccgaj)xwmpy;
						lpqes.gffnx.kxlvu(TlsDebugEventType.ServerKeyExchange, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, vjgil, murhd, cvzwp);
						goto end_IL_0000;
					case nsvut.jctye:
						lpqes.guvib = (cujbv)xwmpy;
						lpqes.gffnx.kxlvu(TlsDebugEventType.CertificateRequest, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, vjgil, murhd, cvzwp);
						goto end_IL_0000;
					case nsvut.wsvfx:
						goto IL_06d0;
					case nsvut.enndd:
						lpqes.gffnx.kxlvu(TlsDebugEventType.CertificateVerify, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, vjgil, murhd, cvzwp);
						throw new TlsException(mjddr.ypibb, brgjd.edcru("Received {0} from a server.", "CertificateVerify"));
					case nsvut.zmyoj:
						lpqes.gffnx.kxlvu(TlsDebugEventType.ClientKeyExchange, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, vjgil, murhd, cvzwp);
						throw new TlsException(mjddr.ypibb, brgjd.edcru("Received {0} from a server.", "ClientKeyExchange"));
					case nsvut.oaufz:
						goto IL_1613;
					default:
						lpqes.gffnx.kxlvu(TlsDebugEventType.UnknownHandshakeMessage, TlsDebugEventSource.Received, TlsDebugLevel.Important, vjgil, murhd, cvzwp);
						goto end_IL_0000;
					}
					lpqes.gffnx.kxlvu(TlsDebugEventType.HelloRequest, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, vjgil, murhd, cvzwp);
					p11 = lpqes.jfwrj().avdby(p1: false).vrtmi();
					if (!p11.zpafv || 1 == 0)
					{
						lfyrj = 0;
						oatnq = p11;
						lwfmt.wqiyk(ref p11, ref this);
						flag = false;
						return;
					}
					goto IL_0182;
				}
				case 0:
					p11 = oatnq;
					oatnq = default(kpthf);
					lfyrj = -1;
					goto IL_0182;
				case 1:
					p10 = oatnq;
					oatnq = default(kpthf);
					lfyrj = -1;
					goto IL_0e21;
				case 2:
					p9 = oatnq;
					oatnq = default(kpthf);
					lfyrj = -1;
					goto IL_0ed7;
				case 3:
					p8 = oatnq;
					oatnq = default(kpthf);
					lfyrj = -1;
					goto IL_0f9a;
				case 4:
					p7 = oatnq;
					oatnq = default(kpthf);
					lfyrj = -1;
					goto IL_10bf;
				case 5:
					p6 = oatnq;
					oatnq = default(kpthf);
					lfyrj = -1;
					goto IL_12fb;
				case 6:
					p5 = oatnq;
					oatnq = default(kpthf);
					lfyrj = -1;
					goto IL_13df;
				case 7:
					p4 = oatnq;
					oatnq = default(kpthf);
					lfyrj = -1;
					goto IL_14e3;
				case 8:
					p3 = oatnq;
					oatnq = default(kpthf);
					lfyrj = -1;
					goto IL_1563;
				case 9:
					p2 = oatnq;
					oatnq = default(kpthf);
					lfyrj = -1;
					goto IL_1799;
				case 10:
					p = oatnq;
					oatnq = default(kpthf);
					lfyrj = -1;
					goto IL_1837;
				case 11:
					p = oatnq;
					oatnq = default(kpthf);
					lfyrj = -1;
					goto IL_191f;
				case 12:
					p = oatnq;
					oatnq = default(kpthf);
					lfyrj = -1;
					goto IL_19a0;
				case 13:
					{
						p = oatnq;
						oatnq = default(kpthf);
						lfyrj = -1;
						break;
					}
					IL_13df:
					p5.ekzxl();
					p5 = default(kpthf);
					lpqes.gffnx.jkyaz(TlsDebugEventType.ChangeCipherSpec, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, oueks);
					nftdp = lpqes.ydiid.wgqsm(lpqes.jakzf, TlsConnectionEnd.Client);
					lpqes.qaqnn = nftdp;
					dxing = new euwhd(nftdp);
					lpqes.gffnx.jkyaz(TlsDebugEventType.Finished, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, dxing);
					p4 = lpqes.hqyqj(dxing).avdby(p1: false).vrtmi();
					if (!p4.zpafv || 1 == 0)
					{
						lfyrj = 7;
						oatnq = p4;
						lwfmt.wqiyk(ref p4, ref this);
						flag = false;
						return;
					}
					goto IL_14e3;
					IL_0e21:
					p10.ekzxl();
					p10 = default(kpthf);
					lpqes.gffnx.jkyaz(TlsDebugEventType.Certificate, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, iqtmy);
					goto IL_0f29;
					IL_19af:
					lpqes.jakzf = null;
					lpqes.rirto = ((lpqes.exdje.Options & TlsOptions.DoNotInsertEmptyFragment) == 0 || 1 == 0) && lpqes.gjtmk.Cbc && lpqes.rnzei.hdraz <= 769;
					lpqes.ywqqr = true;
					p = lpqes.soeay().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						lfyrj = 13;
						oatnq = p;
						lwfmt.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
					IL_1613:
					dxing = (euwhd)xwmpy;
					lpqes.gffnx.kxlvu(TlsDebugEventType.Finished, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, vjgil, murhd, cvzwp);
					if (lpqes.ihqij != null || lpqes.mmbtf == bpnki.yiqfh)
					{
						throw new TlsException(mjddr.ypibb, "Received Finished, but the preceding ChangeCipherSpec was missing.");
					}
					nftdp = lpqes.ydiid.wgqsm(lpqes.jakzf, TlsConnectionEnd.Server);
					lpqes.nahbm = nftdp;
					if (!jtxhe.hbsgb(nftdp, dxing.lkwbh) || 1 == 0)
					{
						throw new TlsException(mjddr.wmgut, "Invalid Finished verify data.");
					}
					if (lpqes.yhqed && 0 == 0)
					{
						lpqes.qsytu(vjgil, murhd, cvzwp);
						uzhwn = new nfbik();
						p2 = lpqes.hqyqj(uzhwn).avdby(p1: false).vrtmi();
						if (!p2.zpafv || 1 == 0)
						{
							lfyrj = 9;
							oatnq = p2;
							lwfmt.wqiyk(ref p2, ref this);
							flag = false;
							return;
						}
						goto IL_1799;
					}
					goto IL_19af;
					IL_1327:
					lpqes.ihqij = lpqes.ydiid.juoso;
					lpqes.gvvsl = lpqes.ydiid.pbohw;
					oueks = new nfbik();
					p5 = lpqes.hqyqj(oueks).avdby(p1: false).vrtmi();
					if (!p5.zpafv || 1 == 0)
					{
						lfyrj = 6;
						oatnq = p5;
						lwfmt.wqiyk(ref p5, ref this);
						flag = false;
						return;
					}
					goto IL_13df;
					IL_12fb:
					p6.ekzxl();
					p6 = default(kpthf);
					lpqes.gffnx.jkyaz(TlsDebugEventType.CertificateVerify, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, shpkf);
					goto IL_1327;
					IL_14e3:
					p4.ekzxl();
					p4 = default(kpthf);
					p3 = lpqes.zupbs().avdby(p1: false).vrtmi();
					if (!p3.zpafv || 1 == 0)
					{
						lfyrj = 8;
						oatnq = p3;
						lwfmt.wqiyk(ref p3, ref this);
						flag = false;
						return;
					}
					goto IL_1563;
					IL_10bf:
					p7.ekzxl();
					p7 = default(kpthf);
					lpqes.gffnx.jkyaz(TlsDebugEventType.ClientKeyExchange, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, lpqes.qgfwn);
					lpqes.ydiid = new fldyu(lpqes.gjtmk, TlsConnectionEnd.Client, lpqes.sjzby.ioriz, lpqes.rnzei.okraw);
					lpqes.ydiid.opzff(klovs, (lpqes.fbjcb ? true : false) ? lpqes.jakzf : null);
					lpqes.ydiid.ybvgj();
					if (tbmsf != null && 0 == 0)
					{
						tbmsf.Dispose();
					}
					if (lpqes.jqjbc != null && 0 == 0)
					{
						lpqes.mnuoq(LogLevel.Info, "Performing client certificate authentication.");
						robzd = lpqes.jqjbc.LeafCertificate;
						lpqes.ydiid.llosq(robzd.KeyAlgorithm, lpqes.exdje.PreferredHashAlgorithm, lpqes.urayu, out xxwdd, out liycq);
						nibxf = lpqes.ydiid.wfrbn(lpqes.jakzf, liycq);
						hbcvq = robzd.SignHash(nibxf, xxwdd, silent: false);
						shpkf = new vssis(xxwdd, robzd.KeyAlgorithm, hbcvq, lpqes.xkrqa);
						p6 = lpqes.hqyqj(shpkf).avdby(p1: false).vrtmi();
						if (!p6.zpafv || 1 == 0)
						{
							lfyrj = 5;
							oatnq = p6;
							lwfmt.wqiyk(ref p6, ref this);
							flag = false;
							return;
						}
						goto IL_12fb;
					}
					goto IL_1327;
					IL_01e4:
					lpqes.rnzei = (kzdrw)xwmpy;
					if (!lpqes.exdje.aljgi || 1 == 0)
					{
						lpqes.gffnx.kxlvu(TlsDebugEventType.ServerHello, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, vjgil, murhd, cvzwp);
					}
					if (lpqes.rnzei.hdraz != lpqes.sjzby.szwbv)
					{
						string text = TlsCipher.tvgwn((TlsProtocol)lpqes.sjzby.szwbv);
						lpqes.bvdcq(LogLevel.Info, "Client requested {0}, server is asking for {1}.", text, TlsCipher.tvgwn((TlsProtocol)lpqes.rnzei.hdraz));
					}
					lpqes.gjtmk = lpqes.sjzby.mtlwh(lpqes.rnzei, lpqes.exdje.vhnmk());
					lpqes.bvdcq(LogLevel.Info, "Negotiating {0}.", lpqes.gjtmk);
					lpqes.zdprx = lpqes.rnzei.hdraz;
					lpqes.iagyo = lpqes.rnzei.dgjdv();
					if (lpqes.qfutg && 0 == 0)
					{
						lpqes.chsrv(TlsConnectionEnd.Client);
					}
					if (lpqes.drmar && 0 == 0 && lpqes.rnzei.tzsfd() && 0 == 0)
					{
						lpqes.fbjcb = true;
					}
					ionuy = lpqes.sjzby.kchbb.Length > 0 && jtxhe.hbsgb(lpqes.sjzby.kchbb, lpqes.rnzei.finbq);
					if (lpqes.drmar && 0 == 0)
					{
						lpqes.zalaq(TlsConnectionEnd.Client, ionuy);
					}
					if (ionuy && 0 == 0)
					{
						lpqes.gffnx.ivtmn(TlsDebugEventType.ResumingCachedSession, TlsDebugEventSource.Unspecified, TlsDebugLevel.Important);
						lpqes.wprub = lpqes.jbiwm.twwdt;
						lpqes.ydiid = new fldyu(lpqes.gjtmk, TlsConnectionEnd.Client, lpqes.sjzby.ioriz, lpqes.rnzei.okraw);
						lpqes.ydiid.qqffv = lpqes.jbiwm.tydvs;
						lpqes.ydiid.ybvgj();
						lpqes.ihqij = lpqes.ydiid.juoso;
						lpqes.gvvsl = lpqes.ydiid.pbohw;
						lpqes.yhqed = true;
					}
					else
					{
						lpqes.yhqed = false;
						lpqes.jbiwm = null;
					}
					goto end_IL_0000;
					IL_1563:
					p3.ekzxl();
					p3 = default(kpthf);
					goto end_IL_0000;
					IL_06d0:
					lpqes.laixd = (watdq)xwmpy;
					lpqes.gffnx.kxlvu(TlsDebugEventType.ServerHelloDone, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, vjgil, murhd, cvzwp);
					gkcdn = lpqes.gjtmk.KeyExchangeAlgorithm;
					if (lpqes.ihqij != null || lpqes.gvvsl != null)
					{
						throw new TlsException(mjddr.ypibb, "Received ServerHelloDone, but another handshake is already pending.");
					}
					if (lpqes.wprub == null || 1 == 0)
					{
						if (gkcdn != TlsKeyExchangeAlgorithm.DH_anon)
						{
							throw new TlsException(mjddr.ypibb, "Received ServerHelloDone, but the certificate was not received from the server.");
						}
						vwjen = null;
						vyzcb = null;
						gwikt = true;
					}
					else
					{
						vwjen = lpqes.wprub.qdige();
						vyzcb = vwjen.LeafCertificate;
						lpqes.syoyz(lpqes.mhfmu, "server", vwjen);
						int keySize = vyzcb.GetKeySize();
						gwikt = lpqes.gjtmk.KeyExchangeAlgorithm != TlsKeyExchangeAlgorithm.RSA || (lpqes.gjtmk.Exportable && 0 == 0 && keySize > lpqes.gjtmk.zorll);
					}
					ycgit = null;
					tbmsf = null;
					usezv = null;
					lbfjc = null;
					bwapa = false;
					klovs = null;
					if (lpqes.ugnwx == null || 1 == 0)
					{
						if (gwikt && 0 == 0)
						{
							throw new TlsException(mjddr.jhrgr, "ServerKeyExchange message was not sent by the server.");
						}
						if (vyzcb.KeyAlgorithm != KeyAlgorithm.RSA && 0 == 0)
						{
							throw new TlsException(mjddr.fvtwt);
						}
						ycgit = new AsymmetricKeyAlgorithm();
						RSAParameters rSAParameters = vyzcb.GetRSAParameters();
						ycgit.ImportKey(rSAParameters);
					}
					else
					{
						if (!gwikt || 1 == 0)
						{
							throw new TlsException(mjddr.jhrgr, "Received unexpected {0}.");
						}
						if (vyzcb != null && 0 == 0)
						{
							lpqes.mnuoq(LogLevel.Debug, "Verifying server key exchange signature.");
							if (!lpqes.ugnwx.ynbct(vyzcb, lpqes.urayu, lpqes.sjzby.ioriz, lpqes.rnzei.okraw) || 1 == 0)
							{
								throw new TlsException(mjddr.wmgut, "Unable to verify ServerKeyExchange signature.");
							}
						}
						if (gkcdn == TlsKeyExchangeAlgorithm.RSA)
						{
							ycgit = new AsymmetricKeyAlgorithm();
							RSAParameters key = lpqes.ugnwx.dtcci();
							if (lpqes.gjtmk.Exportable && 0 == 0 && jtxhe.kexzb(key.Modulus) * 8 > lpqes.gjtmk.zorll)
							{
								throw new TlsException(mjddr.nkvah, "Invalid key length for exportable cipher.");
							}
							ycgit.ImportKey(key);
						}
						else if (gkcdn == TlsKeyExchangeAlgorithm.ECDHE_RSA || gkcdn == TlsKeyExchangeAlgorithm.ECDHE_ECDSA)
						{
							urofm p12 = lpqes.ugnwx.anrsz();
							if (!lpqes.exdje.tokwu(p12, out var _, out var p14, out var p15, out var _) || 1 == 0)
							{
								throw new TlsException(mjddr.jhrgr, "Unsupported curve.");
							}
							if (!lpqes.sjzby.yiwjt(p12) || 1 == 0)
							{
								throw new TlsException(mjddr.jhrgr, "Server is trying to use a curve that is not supported by the client.");
							}
							lpqes.bvdcq(LogLevel.Debug, "Using ephemeral ECDH public key exchange with {0}.", p15);
							byte[] array = lpqes.ugnwx.afugf();
							bwapa = true;
							tbmsf = new AsymmetricKeyAlgorithm();
							tbmsf.kvrol(AsymmetricKeyAlgorithmId.ECDH, p14, 0);
							lbfjc = tbmsf.zimkk();
							usezv = tbmsf.qpwqy(array);
							if (usezv == null || 1 == 0)
							{
								KeyMaterialDeriver keyMaterialDeriver = tbmsf.GetKeyMaterialDeriver(array);
								klovs = new tmtag(keyMaterialDeriver);
							}
							else
							{
								klovs = new onash(usezv);
							}
						}
						else
						{
							tbmsf = new AsymmetricKeyAlgorithm();
							lpqes.mnuoq(LogLevel.Debug, "Received ephemeral Diffie-Hellman prime.");
							DiffieHellmanParameters diffieHellmanParameters = lpqes.ugnwx.acery();
							byte[] y = diffieHellmanParameters.Y;
							tbmsf.ImportKey(diffieHellmanParameters);
							int keySize2 = tbmsf.KeySize;
							int minimumDiffieHellmanKeySize = lpqes.exdje.MinimumDiffieHellmanKeySize;
							lpqes.bvdcq(LogLevel.Debug, "Ephemeral Diffie-Hellman prime size is {0} bits (minimum allowed size is {1} bits).", keySize2, minimumDiffieHellmanKeySize);
							if (keySize2 < minimumDiffieHellmanKeySize)
							{
								throw new TlsException(mjddr.jhrgr, "Ephemeral Diffie-Hellman prime received from the server is considered weak.");
							}
							lbfjc = lpqes.fyuvk(tbmsf);
							usezv = lpqes.fqlki(tbmsf, diffieHellmanParameters, y);
							klovs = new onash(usezv);
						}
					}
					if (vwjen != null && 0 == 0)
					{
						vwjen.Dispose();
					}
					if (lpqes.guvib != null && 0 == 0)
					{
						if (lpqes.jqjbc == null || 1 == 0)
						{
							CertificateChain certificateChain;
							try
							{
								lpqes.mnuoq(LogLevel.Debug, "Client certificate authentication was requested.");
								certificateChain = lpqes.exdje.CertificateRequestHandler.Request(lpqes.gffnx.xlbxi, lpqes.guvib.ilqau);
							}
							catch (Exception inner)
							{
								throw new TlsException("An exception occurred in certificate request handler.", inner);
							}
							if (certificateChain != null && 0 == 0 && certificateChain.LeafCertificate != null && 0 == 0)
							{
								lpqes.jqjbc = certificateChain;
							}
						}
						if (lpqes.jqjbc != null || lpqes.rnzei.hdraz >= 769)
						{
							if (lpqes.jqjbc == null || 1 == 0)
							{
								lpqes.mnuoq(LogLevel.Debug, "No suitable client certificate is available.");
							}
							else
							{
								lpqes.bvdcq(LogLevel.Debug, "Suitable client certificate is available ('{0}').", lpqes.jqjbc.LeafCertificate.GetSubjectName());
							}
							iqtmy = new eupdk(lpqes.jqjbc, lpqes.exdje);
							p10 = lpqes.hqyqj(iqtmy).avdby(p1: false).vrtmi();
							if (!p10.zpafv || 1 == 0)
							{
								lfyrj = 1;
								oatnq = p10;
								lwfmt.wqiyk(ref p10, ref this);
								flag = false;
								return;
							}
							goto IL_0e21;
						}
						cfbon = new zppmb(rtzwv.iddlf, mjddr.frppg);
						p9 = lpqes.hqyqj(cfbon).avdby(p1: false).vrtmi();
						if (!p9.zpafv || 1 == 0)
						{
							lfyrj = 2;
							oatnq = p9;
							lwfmt.wqiyk(ref p9, ref this);
							flag = false;
							return;
						}
						goto IL_0ed7;
					}
					goto IL_0fa9;
					IL_0ed7:
					p9.ekzxl();
					p9 = default(kpthf);
					lpqes.bvdcq(LogLevel.Debug, "{0} was sent.", cfbon);
					lpqes.gffnx.jkyaz(TlsDebugEventType.Alert, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, cfbon);
					goto IL_0f29;
					IL_0f29:
					p8 = lpqes.zupbs().avdby(p1: false).vrtmi();
					if (!p8.zpafv || 1 == 0)
					{
						lfyrj = 3;
						oatnq = p8;
						lwfmt.wqiyk(ref p8, ref this);
						flag = false;
						return;
					}
					goto IL_0f9a;
					IL_1799:
					p2.ekzxl();
					p2 = default(kpthf);
					lpqes.gffnx.jkyaz(TlsDebugEventType.ChangeCipherSpec, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, uzhwn);
					p = lpqes.zupbs().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						lfyrj = 10;
						oatnq = p;
						lwfmt.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_1837;
					IL_0f9a:
					p8.ekzxl();
					p8 = default(kpthf);
					goto IL_0fa9;
					IL_1837:
					p.ekzxl();
					p = default(kpthf);
					nftdp = lpqes.ydiid.wgqsm(lpqes.jakzf, TlsConnectionEnd.Client);
					lpqes.qaqnn = nftdp;
					dxing = new euwhd(nftdp);
					lpqes.gffnx.jkyaz(TlsDebugEventType.Finished, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, dxing);
					p = lpqes.hqyqj(dxing).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						lfyrj = 11;
						oatnq = p;
						lwfmt.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_191f;
					IL_0fa9:
					if (ycgit != null && 0 == 0)
					{
						lpqes.qgfwn = new ypafr(lpqes.sjzby.szwbv);
						lpqes.qgfwn.ueuef(lpqes.rnzei.hdraz, ycgit);
						klovs = new onash(lpqes.qgfwn.mujic);
						ycgit.Dispose();
					}
					else
					{
						lpqes.qgfwn = new ypafr(lbfjc, bwapa);
					}
					p7 = lpqes.hqyqj(lpqes.qgfwn).avdby(p1: false).vrtmi();
					if (!p7.zpafv || 1 == 0)
					{
						lfyrj = 4;
						oatnq = p7;
						lwfmt.wqiyk(ref p7, ref this);
						flag = false;
						return;
					}
					goto IL_10bf;
					IL_191f:
					p.ekzxl();
					p = default(kpthf);
					p = lpqes.zupbs().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						lfyrj = 12;
						oatnq = p;
						lwfmt.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_19a0;
					IL_051d:
					kwfav = (eupdk)xwmpy;
					lpqes.gffnx.kxlvu(TlsDebugEventType.Certificate, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, vjgil, murhd, cvzwp);
					lpqes.wprub = kwfav.tycix();
					gvkem = lpqes.wprub.LeafCertificate;
					jaide = ((lpqes.wprub != null) ? gvkem.KeyAlgorithm : KeyAlgorithm.Unsupported);
					if (jaide == KeyAlgorithm.Unsupported || jaide != lpqes.gjtmk.hflum)
					{
						throw new TlsException(mjddr.ypibb, "Received unsuitable server certificate.");
					}
					if (lpqes.gjtmk.KeyExchangeAlgorithm == TlsKeyExchangeAlgorithm.DH_anon)
					{
						throw new TlsException(mjddr.ypibb, "Received unexpected server certificate while negotiating anonymous cipher.");
					}
					if (jaide == KeyAlgorithm.ECDsa)
					{
						urofm urofm2 = TlsParameters.nbrrb(gvkem);
						if (urofm2 == (urofm)0 || false || !lpqes.sjzby.yiwjt(urofm2) || 1 == 0)
						{
							throw new TlsException(mjddr.ypibb, "Received unexpected server certificate while negotiating anonymous cipher.");
						}
					}
					goto end_IL_0000;
					IL_19a0:
					p.ekzxl();
					p = default(kpthf);
					goto IL_19af;
					IL_0182:
					p11.ekzxl();
					p11 = default(kpthf);
					goto end_IL_0000;
				}
				p.ekzxl();
				p = default(kpthf);
				end_IL_0000:;
			}
			catch (Exception p17)
			{
				lfyrj = -2;
				lwfmt.iurqb(p17);
				return;
			}
			lfyrj = -2;
			lwfmt.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in usnlc
			this.usnlc();
		}

		private void gwjwf(fgyyk p0)
		{
			lwfmt.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in gwjwf
			this.gwjwf(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003COnAlertReceived_003Ed__37 : fgyyk
	{
		public int qdetl;

		public ljmxa snpii;

		public jyhtt amspz;

		public byte[] pcieg;

		public int bnzja;

		public int iopfg;

		public zppmb rxcwn;

		private kpthf avcjc;

		private object jlnle;

		private void lnnsh()
		{
			try
			{
				bool flag = true;
				kpthf p;
				if (qdetl == 0)
				{
					p = avcjc;
					avcjc = default(kpthf);
					qdetl = -1;
					goto IL_017e;
				}
				rxcwn = new zppmb(pcieg[bnzja], pcieg[bnzja + 1]);
				amspz.bvdcq((rxcwn.bgpwy ? true : false) ? LogLevel.Info : LogLevel.Debug, "{0} was received.", rxcwn);
				amspz.gffnx.kxlvu(TlsDebugEventType.Alert, TlsDebugEventSource.Received, TlsDebugLevel.Important, pcieg, bnzja, iopfg);
				if (rxcwn.jmwmm == 100 && amspz.qvaka == hlkgm.iucmn)
				{
					throw new TlsException(isRemote: true, "Renegotiation rejected by the remote connection end.");
				}
				if (rxcwn.wzwvm == 2)
				{
					throw new TlsException(isRemote: true, TlsException.feqgy((mjddr)rxcwn.jmwmm, p1: true));
				}
				if (rxcwn.jmwmm == 0)
				{
					p = amspz.ebnci(1, rxcwn.jmwmm, p2: false).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						qdetl = 0;
						avcjc = p;
						snpii.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					goto IL_017e;
				}
				goto end_IL_0000;
				IL_017e:
				p.ekzxl();
				p = default(kpthf);
				end_IL_0000:;
			}
			catch (Exception p2)
			{
				qdetl = -2;
				snpii.iurqb(p2);
				return;
			}
			qdetl = -2;
			snpii.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in lnnsh
			this.lnnsh();
		}

		private void fkymc(fgyyk p0)
		{
			snpii.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in fkymc
			this.fkymc(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CEnqueue_003Ed__3b : fgyyk
	{
		public int obsos;

		public ljmxa huknq;

		public jyhtt iaick;

		public qoqui kmxev;

		public byte[] ksqlm;

		private kpthf gopfn;

		private object unbdx;

		private void sofas()
		{
			try
			{
				bool flag = true;
				kpthf p;
				if (obsos != 0)
				{
					ksqlm = kmxev.szrqi();
					if (kmxev.qeloj == vcedo.ztfcr && (!(kmxev is tjpdc) || 1 == 0))
					{
						iaick.qsytu(ksqlm, 0, ksqlm.Length);
					}
					p = iaick.kjdzy(kmxev.qeloj, ksqlm, 0, ksqlm.Length).avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						obsos = 0;
						gopfn = p;
						huknq.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
				}
				else
				{
					p = gopfn;
					gopfn = default(kpthf);
					obsos = -1;
				}
				p.ekzxl();
				p = default(kpthf);
			}
			catch (Exception p2)
			{
				obsos = -2;
				huknq.iurqb(p2);
				return;
			}
			obsos = -2;
			huknq.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in sofas
			this.sofas();
		}

		private void jmidi(fgyyk p0)
		{
			huknq.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in jmidi
			this.jmidi(p0);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	private struct _003CBeginHandshake_003Ed__3f : fgyyk
	{
		public int bolmo;

		public ljmxa dqvyi;

		public jyhtt afjqz;

		public bool gbfdi;

		public auaka prwkp;

		public byte[] vokgn;

		private xuwyj<bool> cyjmd;

		private object vaedj;

		private kpthf oxytm;

		private void pqmef()
		{
			try
			{
				bool flag = true;
				bool num;
				xuwyj<bool> p4;
				kpthf p2;
				kpthf p3;
				kpthf p;
				switch (bolmo)
				{
				default:
					gbfdi = false;
					p4 = afjqz.ezgki().giftg(p1: false).vuozn();
					if (!p4.hqxbj || 1 == 0)
					{
						bolmo = 0;
						cyjmd = p4;
						dqvyi.wqiyk(ref p4, ref this);
						flag = false;
						return;
					}
					goto IL_0099;
				case 0:
					p4 = cyjmd;
					cyjmd = default(xuwyj<bool>);
					bolmo = -1;
					goto IL_0099;
				case 1:
					p3 = oxytm;
					oxytm = default(kpthf);
					bolmo = -1;
					goto IL_0406;
				case 2:
					p2 = oxytm;
					oxytm = default(kpthf);
					bolmo = -1;
					goto IL_04be;
				case 3:
					{
						p = oxytm;
						oxytm = default(kpthf);
						bolmo = -1;
						break;
					}
					IL_04cd:
					afjqz.gffnx.ivtmn(TlsDebugEventType.Negotiating, TlsDebugEventSource.Unspecified, TlsDebugLevel.Important);
					gbfdi = gbfdi && 0 == 0 && !afjqz.exdje.aljgi;
					if (gbfdi && 0 == 0)
					{
						afjqz.gffnx.jkyaz(TlsDebugEventType.ClientHello, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, afjqz.sjzby);
					}
					p = afjqz.zupbs().avdby(p1: false).vrtmi();
					if (!p.zpafv || 1 == 0)
					{
						bolmo = 3;
						oxytm = p;
						dqvyi.wqiyk(ref p, ref this);
						flag = false;
						return;
					}
					break;
					IL_0099:
					num = p4.gbccf();
					p4 = default(xuwyj<bool>);
					if (num ? true : false)
					{
						prwkp = eczxc(afjqz.exdje, afjqz.ywqqr, afjqz.zdprx, afjqz.qfutg, afjqz.drmar, afjqz.sjzby != null);
						afjqz.qfutg = prwkp.ikkfq;
						afjqz.drmar = prwkp.xaaih;
						if (afjqz.exdje.Entity == TlsConnectionEnd.Client)
						{
							afjqz.jakzf = new MemoryStream();
							vokgn = null;
							afjqz.jbiwm = afjqz.exdje.Session;
							if (afjqz.jbiwm != null && 0 == 0)
							{
								if (!afjqz.ywqqr || 1 == 0)
								{
									vokgn = afjqz.jbiwm.wqpxu;
								}
								afjqz.jqjbc = afjqz.jbiwm.xalxj;
								afjqz.drmar = (afjqz.fbjcb = afjqz.jbiwm.zqpiy);
							}
							afjqz.bvdcq(LogLevel.Debug, "Enabled cipher suites: 0x{0:X}.", afjqz.exdje.tznry());
							afjqz.bvdcq(LogLevel.Debug, "Applicable cipher suites: 0x{0:X}.", prwkp.zjvps);
							if ((prwkp.zjvps & (TlsCipherSuite)8926398345262397440L) != 0)
							{
								afjqz.mnuoq(LogLevel.Debug, "Some ephemeral Diffie-Hellman ciphers are enabled. These might be slow on legacy platforms.");
							}
							if ((prwkp.zjvps & (TlsCipherSuite.RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.DHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.DHE_DSS_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_GCM_SHA384)) != 0)
							{
								afjqz.mnuoq(LogLevel.Debug, "Some AES/GCM ciphers are enabled. These might be slow on legacy platforms.");
							}
							afjqz.sjzby = dtpci(afjqz.exdje, p1: false, prwkp, afjqz.mhfmu, afjqz.fmqju, afjqz.drmar, afjqz.qaqnn);
							afjqz.zdprx = afjqz.sjzby.szwbv;
							afjqz.urayu = afjqz.sjzby.dvlkv();
							if (vokgn != null && 0 == 0)
							{
								afjqz.mnuoq(LogLevel.Info, "Trying to resume session.");
								afjqz.hzxof(LogLevel.Verbose, "Session to resume: ", vokgn, 0, vokgn.Length);
							}
							p3 = afjqz.hqyqj(afjqz.sjzby).avdby(p1: false).vrtmi();
							if (!p3.zpafv || 1 == 0)
							{
								bolmo = 1;
								oxytm = p3;
								dqvyi.wqiyk(ref p3, ref this);
								flag = false;
								return;
							}
							goto IL_0406;
						}
						afjqz.exdje.wnzfz();
						if (afjqz.ywqqr && 0 == 0)
						{
							p2 = afjqz.hqyqj(new tjpdc()).avdby(p1: false).vrtmi();
							if (!p2.zpafv || 1 == 0)
							{
								bolmo = 2;
								oxytm = p2;
								dqvyi.wqiyk(ref p2, ref this);
								flag = false;
								return;
							}
							goto IL_04be;
						}
						goto IL_04cd;
					}
					goto end_IL_0000;
					IL_04be:
					p2.ekzxl();
					p2 = default(kpthf);
					goto IL_04cd;
					IL_0406:
					p3.ekzxl();
					p3 = default(kpthf);
					gbfdi = true;
					goto IL_04cd;
				}
				p.ekzxl();
				p = default(kpthf);
				end_IL_0000:;
			}
			catch (Exception p5)
			{
				bolmo = -2;
				dqvyi.iurqb(p5);
				return;
			}
			bolmo = -2;
			dqvyi.vjftv();
		}

		void fgyyk.tkrrn()
		{
			//ILSpy generated this explicit interface implementation from .override directive in pqmef
			this.pqmef();
		}

		private void iumqn(fgyyk p0)
		{
			dqvyi.hdlij(p0);
		}

		void fgyyk.nrgxk(fgyyk p0)
		{
			//ILSpy generated this explicit interface implementation from .override directive in iumqn
			this.iumqn(p0);
		}
	}

	private const string nmzsn = "TlsHandshake";

	private TlsSocket gffnx;

	private MemoryStream jakzf;

	private fldyu ydiid;

	private bool yhqed;

	private TlsSession jbiwm;

	private bool ywqqr;

	private bool drmar;

	private bool fbjcb;

	private bool qfutg;

	private bool fmqju;

	private byte[] qaqnn;

	private byte[] nahbm;

	private aoind sjzby;

	private kzdrw rnzei;

	private CertificateChain wprub;

	private ccgaj ugnwx;

	private cujbv guvib;

	private watdq laixd;

	private CertificateChain jqjbc;

	private ypafr qgfwn;

	private nxtme<byte> urayu;

	private AsymmetricKeyAlgorithm nutgc;

	private TlsCipher gjtmk;

	private string iagyo;

	private string mhfmu;

	private static Func<CertificateChain, bool> yhptp;

	public byte[] wkgnu => rnzei.finbq;

	public string nflrx
	{
		get
		{
			if (gjtmk == null || 1 == 0)
			{
				return null;
			}
			if (rnzei == null || 1 == 0)
			{
				return null;
			}
			if (rnzei.finbq.Length == 0 || 1 == 0)
			{
				return null;
			}
			return TlsSession.pwkvr(rnzei.finbq);
		}
	}

	public TlsCipher whuly
	{
		get
		{
			if (!ywqqr || 1 == 0)
			{
				return null;
			}
			return gjtmk;
		}
	}

	public string fvvyo => iagyo;

	public CertificateChain lutxt => wprub;

	public CertificateChain bxids => jqjbc;

	public string ukxui => "TlsHandshake";

	public TlsSession jnoks()
	{
		if (gjtmk == null || 1 == 0)
		{
			return null;
		}
		if (rnzei == null || 1 == 0)
		{
			return null;
		}
		if (rnzei.finbq.Length == 0 || 1 == 0)
		{
			return null;
		}
		if (jbiwm == null || 1 == 0)
		{
			jbiwm = new TlsSession(rnzei.finbq, ydiid.qqffv, wprub, jqjbc, fbjcb);
		}
		return jbiwm;
	}

	public jyhtt(TlsSocket owner, mggni channel, TlsParameters parameters, string serverName)
		: base(owner, channel, parameters)
	{
		gffnx = owner;
		string text = parameters.CommonName;
		if (text == null || 1 == 0)
		{
			text = serverName;
		}
		mhfmu = text;
	}

	private void syoyz(string p0, string p1, CertificateChain p2)
	{
		TlsCertificateAcceptance tlsCertificateAcceptance;
		try
		{
			if (p2.Count > 0)
			{
				bvdcq(LogLevel.Debug, "Verifying {0} certificate ('{1}').", p1, p2[0].GetSubjectName());
			}
			tlsCertificateAcceptance = base.exdje.CertificateVerifier.Verify(gffnx.xlbxi, p0, p2);
		}
		catch (Exception ex)
		{
			bvdcq(LogLevel.Debug, "Certificate verification failed: {0}", ex);
			throw new TlsException("An exception occurred in certificate verifier.", ex);
		}
		bvdcq(LogLevel.Debug, "Certificate verification result: {0}", tlsCertificateAcceptance);
		if (tlsCertificateAcceptance != TlsCertificateAcceptance.Accept && 0 == 0)
		{
			string message = CertificateVerifier.nmzmo(tlsCertificateAcceptance, p1, p2, p0);
			mjddr description = CertificateVerifier.rjdmd(tlsCertificateAcceptance, (TlsProtocol)rnzei.hdraz);
			throw new TlsException(description, message);
		}
	}

	[vtsnh(typeof(_003COnHandshakeReceived_003Ed__0))]
	protected override exkzi tmymn(byte[] p0, int p1, int p2)
	{
		_003COnHandshakeReceived_003Ed__0 p3 = default(_003COnHandshakeReceived_003Ed__0);
		p3.klfsq = this;
		p3.fxttr = p0;
		p3.mtvpc = p1;
		p3.tqnwn = p2;
		p3.rumtj = ljmxa.nmskg();
		p3.uikqt = -1;
		ljmxa rumtj = p3.rumtj;
		rumtj.nncuo(ref p3);
		return p3.rumtj.donjp;
	}

	[vtsnh(typeof(_003COnHandshakeReceivedServer_003Ed__6))]
	private exkzi jlkqe(byte[] p0, int p1, int p2, ofuit p3)
	{
		_003COnHandshakeReceivedServer_003Ed__6 p4 = default(_003COnHandshakeReceivedServer_003Ed__6);
		p4.nqaae = this;
		p4.irjtl = p0;
		p4.qzahe = p1;
		p4.nlnbu = p2;
		p4.flygj = p3;
		p4.hwshc = ljmxa.nmskg();
		p4.jkzyg = -1;
		ljmxa hwshc = p4.hwshc;
		hwshc.nncuo(ref p4);
		return p4.hwshc.donjp;
	}

	[vtsnh(typeof(_003COnHandshakeReceivedClient_003Ed__1a))]
	private exkzi ftdrl(byte[] p0, int p1, int p2, ofuit p3)
	{
		_003COnHandshakeReceivedClient_003Ed__1a p4 = default(_003COnHandshakeReceivedClient_003Ed__1a);
		p4.lpqes = this;
		p4.vjgil = p0;
		p4.murhd = p1;
		p4.cvzwp = p2;
		p4.xwmpy = p3;
		p4.lwfmt = ljmxa.nmskg();
		p4.lfyrj = -1;
		ljmxa lwfmt = p4.lwfmt;
		lwfmt.nncuo(ref p4);
		return p4.lwfmt.donjp;
	}

	private byte[] fqlki(AsymmetricKeyAlgorithm p0, DiffieHellmanParameters? p1, byte[] p2)
	{
		byte[] p3 = p0.fevai(p2);
		int keySize = p0.KeySize;
		if (p1.HasValue && 0 == 0 && p3.Length * 8 < keySize && dqpnj(p1) && 0 == 0)
		{
			opipb(ref p3, p0.KeySize);
			mnuoq(LogLevel.Debug, "Applied alternative Diffie-Hellman premaster secret padding.");
		}
		return p3;
	}

	private byte[] fyuvk(AsymmetricKeyAlgorithm p0)
	{
		byte[] p1 = p0.zimkk();
		if (opipb(ref p1, p0.KeySize) && 0 == 0)
		{
			mnuoq(LogLevel.Debug, "Applied Diffie-Hellman key padding workaround.");
		}
		return p1;
	}

	private static bool opipb(ref byte[] p0, int p1)
	{
		if (p0.Length * 8 >= p1)
		{
			return false;
		}
		byte[] array = new byte[p1 + 7 >> 3];
		p0.CopyTo(array, array.Length - p0.Length);
		p0 = array;
		return true;
	}

	private static bool dqpnj(DiffieHellmanParameters? p0)
	{
		bool flag = false;
		byte[] p1 = p0.Value.P;
		byte[] g = p0.Value.G;
		int num;
		if (p1.Length == g.Length && p1.Length > 8)
		{
			flag = true;
			num = 0;
			if (num != 0)
			{
				goto IL_0032;
			}
			goto IL_0052;
		}
		goto IL_005d;
		IL_005d:
		return flag;
		IL_0052:
		if (num >= 8)
		{
			goto IL_005d;
		}
		goto IL_0032;
		IL_0032:
		if (((g[num] != 0) ? true : false) || p1[num] != byte.MaxValue)
		{
			flag = false;
			if (!flag)
			{
				goto IL_005d;
			}
		}
		num++;
		goto IL_0052;
	}

	private void chsrv(TlsConnectionEnd p0)
	{
		bool flag = p0 == TlsConnectionEnd.Client;
		string text = ((flag ? true : false) ? "server" : "client");
		byte[] array = ((flag ? true : false) ? rnzei.ckhgw() : sjzby.nsqrx());
		if (array == null || 1 == 0)
		{
			if (fmqju && 0 == 0)
			{
				bvdcq(LogLevel.Error, "The {0} did not provide renegotiation info.", text);
				throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "Secure renegotiation failure.", null);
			}
			bvdcq(LogLevel.Debug, "The {0} does not support secure renegotiation.", text);
			return;
		}
		int num = array.Length - 1;
		if (num < 0 || array[0] != num)
		{
			bvdcq(LogLevel.Error, "The {0} sent broken renegotiation info.", text);
			throw new TlsException(rtzwv.iogyt, mjddr.gkkle, "Secure renegotiation failure.", null);
		}
		if (!fmqju || 1 == 0)
		{
			if (ywqqr && 0 == 0)
			{
				bvdcq(LogLevel.Error, "The {0} claims to support secure renegotiation despite previously claiming otherwise.", text);
				throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "Secure renegotiation failure.", null);
			}
			fmqju = true;
			if (num != 0 && 0 == 0)
			{
				bvdcq(LogLevel.Error, "The {0} sent bad renegotiation info.", text);
				throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "Secure renegotiation failure.", null);
			}
			bvdcq(LogLevel.Debug, "The {0} supports secure renegotiation.", text);
			return;
		}
		bool flag2 = false;
		int num2;
		int num3;
		if (num == qaqnn.Length + ((flag ? true : false) ? nahbm.Length : 0))
		{
			flag2 = true;
			num2 = 1;
			num3 = 0;
			if (num3 != 0)
			{
				goto IL_01c0;
			}
			goto IL_01e0;
		}
		goto IL_022a;
		IL_021e:
		int num4;
		if (num4 < nahbm.Length)
		{
			goto IL_01fe;
		}
		goto IL_022a;
		IL_01fe:
		flag2 &= array[num2] == nahbm[num4];
		num2++;
		num4++;
		goto IL_021e;
		IL_01c0:
		flag2 &= array[num2] == qaqnn[num3];
		num2++;
		num3++;
		goto IL_01e0;
		IL_01e0:
		if (num3 < qaqnn.Length)
		{
			goto IL_01c0;
		}
		if (flag && 0 == 0)
		{
			num4 = 0;
			if (num4 != 0)
			{
				goto IL_01fe;
			}
			goto IL_021e;
		}
		goto IL_022a;
		IL_022a:
		if (!flag2 || 1 == 0)
		{
			bvdcq(LogLevel.Error, "The {0} sent wrong renegotiation info.", text);
			throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "Secure renegotiation failure.", null);
		}
		mnuoq(LogLevel.Debug, "Performing secure renegotiation.");
	}

	private void zalaq(TlsConnectionEnd p0, bool p1)
	{
		bool flag = p0 == TlsConnectionEnd.Client;
		string text = ((flag ? true : false) ? "server" : "client");
		if (((flag ? true : false) ? rnzei.tzsfd() : sjzby.jhgcv()) && 0 == 0)
		{
			if (!fbjcb || 1 == 0)
			{
				if ((ywqqr ? true : false) || p1)
				{
					bvdcq(LogLevel.Error, "The {0} claims to support extended master secret despite previously claiming otherwise.", text);
					throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "Negotiation failure.", null);
				}
				fbjcb = true;
			}
			mnuoq(LogLevel.Debug, "Extended master secret is enabled.");
		}
		else
		{
			if (fbjcb && 0 == 0)
			{
				bvdcq(LogLevel.Error, "The {0} claims not to support extended master secret despite previously claiming otherwise.", text);
				throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "Negotiation failure.", null);
			}
			bvdcq(LogLevel.Debug, "The {0} does not support extended master secret.", text);
		}
	}

	[vtsnh(typeof(_003COnAlertReceived_003Ed__37))]
	protected override exkzi ekqhl(byte[] p0, int p1, int p2)
	{
		_003COnAlertReceived_003Ed__37 p3 = default(_003COnAlertReceived_003Ed__37);
		p3.amspz = this;
		p3.pcieg = p0;
		p3.bnzja = p1;
		p3.iopfg = p2;
		p3.snpii = ljmxa.nmskg();
		p3.qdetl = -1;
		ljmxa snpii = p3.snpii;
		snpii.nncuo(ref p3);
		return p3.snpii.donjp;
	}

	private void qsytu(byte[] p0, int p1, int p2)
	{
		jakzf.Write(p0, p1, p2);
	}

	[vtsnh(typeof(_003CEnqueue_003Ed__3b))]
	private exkzi hqyqj(qoqui p0)
	{
		_003CEnqueue_003Ed__3b p1 = default(_003CEnqueue_003Ed__3b);
		p1.iaick = this;
		p1.kmxev = p0;
		p1.huknq = ljmxa.nmskg();
		p1.obsos = -1;
		ljmxa huknq = p1.huknq;
		huknq.nncuo(ref p1);
		return p1.huknq.donjp;
	}

	[vtsnh(typeof(_003CBeginHandshake_003Ed__3f))]
	protected override exkzi jfwrj()
	{
		_003CBeginHandshake_003Ed__3f p = default(_003CBeginHandshake_003Ed__3f);
		p.afjqz = this;
		p.dqvyi = ljmxa.nmskg();
		p.bolmo = -1;
		ljmxa dqvyi = p.dqvyi;
		dqvyi.nncuo(ref p);
		return p.dqvyi.donjp;
	}

	private static auaka eczxc(TlsParameters p0, bool p1, int? p2, bool p3, bool p4, bool p5)
	{
		TlsCipherSuite tlsCipherSuite = p0.tznry();
		TlsVersion tlsVersion = p0.vhnmk();
		TlsProtocol tlsProtocol;
		if ((tlsVersion & TlsVersion.TLS12) != TlsVersion.None && 0 == 0)
		{
			tlsProtocol = TlsProtocol.TLS12;
			if (tlsProtocol != TlsProtocol.None)
			{
				goto IL_009b;
			}
		}
		if ((tlsVersion & TlsVersion.TLS11) != TlsVersion.None && 0 == 0)
		{
			tlsProtocol = TlsProtocol.TLS11;
			tlsCipherSuite &= ~(TlsCipherSuite.Secure | TlsCipherSuite.DHE_DSS_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_GCM_SHA384);
		}
		else if ((tlsVersion & TlsVersion.TLS10) != TlsVersion.None && 0 == 0)
		{
			tlsProtocol = TlsProtocol.TLS10;
			tlsCipherSuite &= ~(TlsCipherSuite.Secure | TlsCipherSuite.DHE_DSS_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_GCM_SHA384);
		}
		else
		{
			if ((tlsVersion & TlsVersion.SSL30) == 0)
			{
				throw new TlsException(mjddr.puqjh, "TLS/SSL protocol version not specified or not allowed.");
			}
			tlsProtocol = TlsProtocol.SSL30;
			tlsCipherSuite &= TlsCipherSuite.RSA_EXPORT_WITH_RC4_40_MD5 | TlsCipherSuite.RSA_WITH_RC4_128_MD5 | TlsCipherSuite.RSA_WITH_RC4_128_SHA | TlsCipherSuite.RSA_EXPORT_WITH_RC2_CBC_40_MD5 | TlsCipherSuite.RSA_EXPORT_WITH_DES40_CBC_SHA | TlsCipherSuite.RSA_WITH_DES_CBC_SHA | TlsCipherSuite.RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.RSA_EXPORT1024_WITH_DES_CBC_SHA | TlsCipherSuite.RSA_EXPORT1024_WITH_RC4_56_SHA | TlsCipherSuite.DH_anon_WITH_RC4_128_MD5 | TlsCipherSuite.DH_anon_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.DH_anon_WITH_DES_CBC_SHA;
		}
		goto IL_009b;
		IL_009b:
		if (tlsCipherSuite == TlsCipherSuite.None)
		{
			throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "All usable cipher suites have been disabled.", null);
		}
		bool secureRenegotiationEnabled = p3;
		bool extendedMasterSecretEnabled = p4;
		if (p1 && 0 == 0)
		{
			tlsProtocol = (TlsProtocol)p2.Value;
		}
		else
		{
			secureRenegotiationEnabled = (p0.Options & TlsOptions.DisableRenegotiationExtension) == 0;
			extendedMasterSecretEnabled = p0.uocnj();
		}
		return new auaka(tlsCipherSuite, tlsProtocol, tlsVersion, secureRenegotiationEnabled, extendedMasterSecretEnabled, p5);
	}

	internal static aoind dtpci(TlsParameters p0, bool p1, auaka p2 = null, string p3 = null, bool p4 = false, bool p5 = false, byte[] p6 = null)
	{
		auaka obj = p2;
		if (obj == null || 1 == 0)
		{
			obj = eczxc(p0, p1: false, null, p3: false, p4: false, p5: false);
		}
		p2 = obj;
		wmbjj wmbjj2 = new wmbjj();
		TlsCipherSuite tlsCipherSuite = p2.zjvps;
		TlsProtocol lmbkk = p2.lmbkk;
		if ((tlsCipherSuite & (TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_RC4_128_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA384 | TlsCipherSuite.ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_RC4_128_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256)) != 0 && (!bhdat(wmbjj2, p2.lmbkk, p0) || 1 == 0))
		{
			tlsCipherSuite &= ~(TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_RC4_128_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA384 | TlsCipherSuite.ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_RC4_128_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256);
		}
		if (lmbkk == TlsProtocol.TLS12)
		{
			lqktm(wmbjj2, p0.ggdwh);
		}
		if (p2.xaaih && 0 == 0 && lmbkk >= TlsProtocol.TLS10)
		{
			jwkpf(wmbjj2);
		}
		bool announceRenegotiationIndication = false;
		if (p2.ikkfq && 0 == 0)
		{
			if (p4 && 0 == 0)
			{
				ceuvv(wmbjj2, p6, null);
			}
			else
			{
				announceRenegotiationIndication = true;
			}
		}
		string serverName = ((p3 != null && 0 == 0 && (p0.Options & TlsOptions.DisableServerNameIndication) == 0) ? p3 : null);
		byte[] sessionID = cexil(p0);
		return new aoind(lmbkk, tlsCipherSuite, announceRenegotiationIndication, p0, sessionID, serverName, wmbjj2.ihelo());
	}

	private void uoanx(wmbjj p0, string p1)
	{
	}

	private static byte[] cexil(TlsParameters p0)
	{
		byte[] result = null;
		TlsSession session = p0.Session;
		if (session != null && 0 == 0)
		{
			result = session.wqpxu;
		}
		return result;
	}

	private static void jwkpf(wmbjj p0)
	{
		p0.mmgwn(23);
		p0.mmgwn(0);
	}

	private static void ceuvv(wmbjj p0, byte[] p1, byte[] p2)
	{
		int num = ((p1 != null) ? p1.Length : 0) + ((p2 != null) ? p2.Length : 0);
		p0.mmgwn(65281);
		p0.mmgwn((ushort)(num + 1));
		p0.ywmoe((byte)num);
		if (p1 != null && 0 == 0)
		{
			p0.udtyl(p1);
		}
		if (p2 != null && 0 == 0)
		{
			p0.udtyl(p2);
		}
	}

	private static void lqktm(wmbjj p0, ttceu p1)
	{
		p0.mmgwn(13);
		int hpxkw = p0.hpxkw;
		p0.bmhvq(0u);
		int num = 0;
		if (HashingAlgorithm.IsSupported(HashingAlgorithmId.SHA256) && 0 == 0)
		{
			if ((p1 & ttceu.daxam) != 0)
			{
				p0.mmgwn(1025);
				num++;
			}
			if ((p1 & ttceu.suhyu) != 0)
			{
				p0.mmgwn(1027);
				num++;
			}
		}
		if (HashingAlgorithm.IsSupported(HashingAlgorithmId.SHA384) && 0 == 0)
		{
			if ((p1 & ttceu.mpoxm) != 0)
			{
				p0.mmgwn(1281);
				num++;
			}
			if ((p1 & ttceu.wgahn) != 0)
			{
				p0.mmgwn(1283);
				num++;
			}
		}
		if (HashingAlgorithm.IsSupported(HashingAlgorithmId.SHA512) && 0 == 0)
		{
			if ((p1 & ttceu.bwgpd) != 0)
			{
				p0.mmgwn(1537);
				num++;
			}
			if ((p1 & ttceu.mazpq) != 0)
			{
				p0.mmgwn(1539);
				num++;
			}
		}
		if ((p1 & ttceu.glmiu) != 0)
		{
			p0.mmgwn(513);
			num++;
		}
		if ((p1 & ttceu.mcypf) != 0)
		{
			p0.mmgwn(515);
			num++;
		}
		if ((p1 & ttceu.ddgzs) != 0)
		{
			p0.mmgwn(514);
			num++;
		}
		int hpxkw2 = p0.hpxkw;
		p0.hpxkw = hpxkw;
		p0.mmgwn((ushort)(num * 2 + 2));
		p0.mmgwn((ushort)(num * 2));
		p0.hpxkw = hpxkw2;
	}

	private static bool bhdat(wmbjj p0, TlsProtocol p1, TlsParameters p2)
	{
		int hpxkw = p0.hpxkw;
		p0.mmgwn(10);
		int hpxkw2 = p0.hpxkw;
		p0.bmhvq(0u);
		int num = 0;
		urofm[] array = new urofm[7]
		{
			urofm.lzkxw,
			urofm.eaxxm,
			urofm.vmdec,
			urofm.mfnhs,
			urofm.hfvjd,
			urofm.bnugk,
			urofm.qeprw
		};
		urofm[] array2 = array;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0062;
		}
		goto IL_008f;
		IL_0062:
		urofm urofm2 = array2[num2];
		if (p2.cfmws(urofm2, p1) && 0 == 0)
		{
			p0.mmgwn((ushort)urofm2);
			num++;
		}
		num2++;
		goto IL_008f;
		IL_008f:
		if (num2 >= array2.Length)
		{
			int hpxkw3 = p0.hpxkw;
			p0.hpxkw = hpxkw2;
			p0.mmgwn((ushort)(num * 2 + 2));
			p0.mmgwn((ushort)(num * 2));
			p0.hpxkw = hpxkw3;
			if (num == 0 || 1 == 0)
			{
				p0.tdjyr = hpxkw;
				return false;
			}
			p0.mmgwn(11);
			p0.mmgwn(2);
			p0.ywmoe(1);
			p0.ywmoe(0);
			return true;
		}
		goto IL_0062;
	}

	public njvzu<int> knwel()
	{
		return base.znydr.xtyvd();
	}

	private static bool wiwtv(CertificateChain p0)
	{
		if (p0 != null && 0 == 0)
		{
			return p0.LeafCertificate != null;
		}
		return false;
	}
}
