using System;
using System.IO;
using System.Security.Cryptography;
using Rebex;
using Rebex.Net;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class pxaob : csnwg, mwxgh
{
	private sealed class fqfeg
	{
		public TlsCipherSuite dtfwc;

		public string cuzlu()
		{
			return brgjd.edcru("Enabled cipher suites: 0x{0:X}.", dtfwc);
		}

		public string ltrgy()
		{
			return brgjd.edcru("Applicable cipher suites: 0x{0:X}.", dtfwc);
		}

		public string clvry()
		{
			return brgjd.edcru("Some ephemeral Diffie-Hellman ciphers are enabled. These might be slow on legacy platforms.", dtfwc);
		}

		public string llelg()
		{
			return brgjd.edcru("Some AES/GCM ciphers are enabled. These might be slow on legacy platforms.", dtfwc);
		}
	}

	private const string okxae = "TlsHandshakeSync";

	private TlsSocket vachj;

	private MemoryStream yqdqs;

	private fldyu tubqp;

	private bool objjc;

	private TlsSession eqbmp;

	private bool olskl;

	private bool ejgqw;

	private bool beohg;

	private bool udugd;

	private bool bbnuu;

	private byte[] ttgfn;

	private byte[] kujxz;

	private aoind yrmwj;

	private kzdrw cdblf;

	private CertificateChain yvivf;

	private ccgaj ubcvm;

	private cujbv ogbin;

	private watdq ierva;

	private CertificateChain jcsyq;

	private ypafr kvnxu;

	private nxtme<byte> jevdi;

	private AsymmetricKeyAlgorithm xcyjy;

	private TlsCipher pbpdx;

	private string bdlzj;

	private string qnwpy;

	private static Func<CertificateChain, bool> cmokz;

	public byte[] tjkch => cdblf.finbq;

	public string obkur
	{
		get
		{
			if (pbpdx == null || 1 == 0)
			{
				return null;
			}
			if (cdblf == null || 1 == 0)
			{
				return null;
			}
			if (cdblf.finbq.Length == 0 || 1 == 0)
			{
				return null;
			}
			return TlsSession.pwkvr(cdblf.finbq);
		}
	}

	public TlsCipher qmszr
	{
		get
		{
			if (!olskl || 1 == 0)
			{
				return null;
			}
			return pbpdx;
		}
	}

	public string gjdwe => bdlzj;

	public CertificateChain vqoau => yvivf;

	public CertificateChain eqpjs => jcsyq;

	public string ukxui => "TlsHandshakeSync";

	public TlsSession swkdt()
	{
		if (pbpdx == null || 1 == 0)
		{
			return null;
		}
		if (cdblf == null || 1 == 0)
		{
			return null;
		}
		if (cdblf.finbq.Length == 0 || 1 == 0)
		{
			return null;
		}
		if (eqbmp == null || 1 == 0)
		{
			eqbmp = new TlsSession(cdblf.finbq, tubqp.qqffv, yvivf, jcsyq, beohg);
		}
		return eqbmp;
	}

	public pxaob(TlsSocket owner, ISocket socket, TlsParameters parameters, string serverName)
		: base(owner, socket, parameters)
	{
		vachj = owner;
		string text = parameters.CommonName;
		if (text == null || 1 == 0)
		{
			text = serverName;
		}
		qnwpy = text;
	}

	private void ipjff(string p0, string p1, CertificateChain p2)
	{
		TlsCertificateAcceptance tlsCertificateAcceptance;
		try
		{
			if (p2.Count > 0)
			{
				kxjds(LogLevel.Debug, "Verifying {0} certificate ('{1}').", p1, p2[0].GetSubjectName());
			}
			tlsCertificateAcceptance = base.knmxu.CertificateVerifier.Verify(vachj.xlbxi, p0, p2);
		}
		catch (Exception ex)
		{
			kxjds(LogLevel.Debug, "Certificate verification failed: {0}", ex);
			throw new TlsException("An exception occurred in certificate verifier.", ex);
		}
		kxjds(LogLevel.Debug, "Certificate verification result: {0}", tlsCertificateAcceptance);
		if (tlsCertificateAcceptance != TlsCertificateAcceptance.Accept && 0 == 0)
		{
			string message = CertificateVerifier.nmzmo(tlsCertificateAcceptance, p1, p2, p0);
			mjddr description = CertificateVerifier.rjdmd(tlsCertificateAcceptance, (TlsProtocol)cdblf.hdraz);
			throw new TlsException(description, message);
		}
	}

	protected override void doutw(byte[] p0, int p1, int p2)
	{
		ofuit ofuit2 = ofuit.paptc(p0, p1, p2, base.gzlwv, pbpdx);
		if (ofuit2 == null || 1 == 0)
		{
			throw new TlsException(mjddr.ypibb, "Received unknown handshake message.");
		}
		if (base.knmxu.Entity == TlsConnectionEnd.Client)
		{
			pxsjp(p0, p1, p2, ofuit2);
		}
		else
		{
			gfmbj(p0, p1, p2, ofuit2);
		}
	}

	private void gfmbj(byte[] p0, int p1, int p2, ofuit p3)
	{
		MemoryStream p4 = null;
		switch (p3.zijgx)
		{
		case nsvut.enndd:
			p4 = new MemoryStream(yqdqs.GetBuffer(), 0, (int)yqdqs.Length, writable: false, publiclyVisible: true);
			goto default;
		case nsvut.lnjcv:
			yqdqs = new MemoryStream();
			goto default;
		default:
			unnsm(p0, p1, p2);
			break;
		case nsvut.rvoru:
		case nsvut.oaufz:
			break;
		}
		switch (p3.zijgx)
		{
		case nsvut.rvoru:
			vachj.kxlvu(TlsDebugEventType.HelloRequest, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			break;
		case nsvut.lnjcv:
		{
			yrmwj = (aoind)p3;
			if (!base.knmxu.aljgi || 1 == 0)
			{
				vachj.kxlvu(TlsDebugEventType.ClientHello, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			}
			webdt webdt3 = base.knmxu.auypu();
			byte[] array = null;
			eqbmp = null;
			objjc = false;
			if (webdt3 != null && 0 == 0 && yrmwj.kchbb.Length > 0)
			{
				TlsSession tlsSession = webdt3.bfjil(yrmwj.kchbb);
				if (tlsSession != null && 0 == 0)
				{
					eqbmp = tlsSession;
					objjc = true;
					ejgqw = (beohg = tlsSession.zqpiy);
					array = eqbmp.wqpxu;
					vachj.ivtmn(TlsDebugEventType.ResumingCachedSession, TlsDebugEventSource.Unspecified, TlsDebugLevel.Important);
				}
			}
			if (array == null || 1 == 0)
			{
				array = new byte[32];
				jtxhe.ubsib(array, 0, 32);
				if (webdt3 != null && 0 == 0)
				{
					webdt3.lacjz(array, null);
				}
			}
			wmbjj wmbjj2 = new wmbjj();
			if (udugd && 0 == 0)
			{
				lxhmj(TlsConnectionEnd.Server);
				if (bbnuu && 0 == 0)
				{
					asnnl(wmbjj2, ttgfn, kujxz);
				}
			}
			if (ejgqw && 0 == 0 && yrmwj.szwbv >= 769)
			{
				ohrnk(TlsConnectionEnd.Server, objjc);
				if (beohg && 0 == 0)
				{
					phadm(wmbjj2);
				}
			}
			CertificateChain certificateChain = null;
			string serverName = yrmwj.vfiwv();
			nxtme<string> applicationProtocols = yrmwj.nihvb();
			smtii smtii2 = new smtii(serverName, applicationProtocols, base.knmxu.qopqb, isCertificateRequired: true);
			base.knmxu.vplfc(smtii2);
			base.knmxu.ylflp(smtii2.vubgy);
			nxtme<CertificateChain> vubgy = smtii2.vubgy;
			if (cmokz == null || 1 == 0)
			{
				cmokz = qvssb;
			}
			certificateChain = vubgy.xoiju(cmokz);
			bdlzj = smtii2.vwhek;
			viujn(wmbjj2, bdlzj);
			cdblf = new kzdrw(base.knmxu, certificateChain.LeafCertificate, yrmwj, array, wmbjj2.ihelo(), out pbpdx);
			lwwzy = yrmwj.szwbv;
			if (lwwzy >= 771)
			{
				jevdi = yrmwj.dvlkv();
			}
			else
			{
				jevdi = nxtme<byte>.gihlo;
			}
			uxxiy(cdblf);
			vachj.jkyaz(TlsDebugEventType.ServerHello, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, cdblf);
			vituh();
			if (objjc && 0 == 0)
			{
				yvivf = eqbmp.twwdt;
				jcsyq = eqbmp.xalxj;
				tubqp = new fldyu(pbpdx, TlsConnectionEnd.Server, yrmwj.ioriz, cdblf.okraw);
				tubqp.qqffv = eqbmp.tydvs;
				tubqp.ybvgj();
				base.pmfcd = tubqp.juoso;
				base.ihtfw = tubqp.pbohw;
				nfbik nfbik3 = new nfbik();
				uxxiy(nfbik3);
				vachj.jkyaz(TlsDebugEventType.ChangeCipherSpec, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, nfbik3);
				vituh();
				euwhd euwhd2 = new euwhd(kujxz = tubqp.wgqsm(yqdqs, TlsConnectionEnd.Server));
				vachj.jkyaz(TlsDebugEventType.Finished, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, euwhd2);
				uxxiy(euwhd2);
				vituh();
				break;
			}
			TlsKeyExchangeAlgorithm keyExchangeAlgorithm = pbpdx.KeyExchangeAlgorithm;
			if (keyExchangeAlgorithm != TlsKeyExchangeAlgorithm.DH_anon)
			{
				yvivf = certificateChain;
				eupdk eupdk3 = new eupdk(certificateChain, base.knmxu);
				uxxiy(eupdk3);
				vachj.jkyaz(TlsDebugEventType.Certificate, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, eupdk3);
				vituh();
			}
			xcyjy = null;
			switch (keyExchangeAlgorithm)
			{
			case TlsKeyExchangeAlgorithm.RSA:
				if (pbpdx.Exportable && 0 == 0 && certificateChain[0].GetKeySize() > pbpdx.zorll)
				{
					xcyjy = new AsymmetricKeyAlgorithm();
					xcyjy.ImportKey(base.knmxu.TemporaryRSAParameters);
					RSAParameters rSAParameters = xcyjy.GetPublicKey().GetRSAParameters();
					ubcvm = new ccgaj(certificateChain[0], rSAParameters, yrmwj.ioriz, cdblf.okraw, base.gzlwv);
				}
				break;
			case TlsKeyExchangeAlgorithm.ECDHE_RSA:
			case TlsKeyExchangeAlgorithm.ECDHE_ECDSA:
			{
				urofm urofm2 = cdblf.izerh ?? ((urofm)0);
				if (urofm2 == (urofm)0 || 1 == 0)
				{
					throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
				}
				base.knmxu.tokwu(urofm2, out var _, out var p8, out var p9, out var _);
				kxjds(LogLevel.Debug, "Using ephemeral ECDH public key exchange with {0}.", p9);
				xcyjy = new AsymmetricKeyAlgorithm();
				xcyjy.GenerateKey(AsymmetricKeyAlgorithmId.ECDH, p8);
				ubcvm = new ccgaj(certificateChain[0], jevdi, urofm2, xcyjy.zimkk(), yrmwj.ioriz, cdblf.okraw, base.gzlwv);
				break;
			}
			default:
			{
				xcyjy = new AsymmetricKeyAlgorithm();
				DiffieHellmanParameters diffieHellmanParameters = new DiffieHellmanParameters
				{
					P = (byte[])base.knmxu.EphemeralDiffieHellmanParameters.P.Clone(),
					G = (byte[])base.knmxu.EphemeralDiffieHellmanParameters.G.Clone()
				};
				xcyjy.ImportKey(diffieHellmanParameters);
				diffieHellmanParameters.Y = xrdts(xcyjy);
				if (pbpdx.KeyExchangeAlgorithm != TlsKeyExchangeAlgorithm.DH_anon)
				{
					ubcvm = new ccgaj(certificateChain[0], jevdi, diffieHellmanParameters, yrmwj.ioriz, cdblf.okraw, base.gzlwv);
				}
				else
				{
					ubcvm = new ccgaj(diffieHellmanParameters);
				}
				break;
			}
			}
			if (ubcvm != null && 0 == 0)
			{
				uxxiy(ubcvm);
				vachj.jkyaz(TlsDebugEventType.ServerKeyExchange, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, ubcvm);
				vituh();
			}
			if (base.knmxu.CertificatePolicy != TlsCertificatePolicy.NoClientCertificate && 0 == 0)
			{
				cujbv cujbv2 = new cujbv(base.knmxu.AcceptableAuthorities, base.gzlwv);
				uxxiy(cujbv2);
				vachj.jkyaz(TlsDebugEventType.CertificateRequest, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, cujbv2);
				vituh();
			}
			ierva = new watdq();
			uxxiy(ierva);
			vachj.jkyaz(TlsDebugEventType.ServerHelloDone, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, ierva);
			vituh();
			break;
		}
		case nsvut.jthry:
			vachj.kxlvu(TlsDebugEventType.ServerHello, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			throw new TlsException(mjddr.ypibb, brgjd.edcru("Received {0} from a client.", "ServerHello"));
		case nsvut.upgjx:
		{
			eupdk eupdk2 = (eupdk)p3;
			vachj.kxlvu(TlsDebugEventType.Certificate, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			if (base.knmxu.CertificatePolicy == TlsCertificatePolicy.NoClientCertificate || 1 == 0)
			{
				throw new TlsException(mjddr.ypibb);
			}
			jcsyq = eupdk2.tycix();
			break;
		}
		case nsvut.zmyoj:
			try
			{
				kvnxu = (ypafr)p3;
				vachj.kxlvu(TlsDebugEventType.ClientKeyExchange, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
				if (base.knmxu.CertificatePolicy == TlsCertificatePolicy.RequireClientCertificate && jcsyq == null)
				{
					throw new TlsException(mjddr.vyvjd);
				}
				sgjhx sgjhx2 = null;
				byte[] array2 = null;
				TlsKeyExchangeAlgorithm keyExchangeAlgorithm2 = pbpdx.KeyExchangeAlgorithm;
				if (keyExchangeAlgorithm2 != TlsKeyExchangeAlgorithm.RSA)
				{
					try
					{
						if (keyExchangeAlgorithm2 == TlsKeyExchangeAlgorithm.ECDHE_RSA || keyExchangeAlgorithm2 == TlsKeyExchangeAlgorithm.ECDHE_ECDSA)
						{
							byte[] array3 = kvnxu.ubcyg(p0: true);
							array2 = xcyjy.qpwqy(array3);
							if (array2 == null || 1 == 0)
							{
								KeyMaterialDeriver keyMaterialDeriver = xcyjy.GetKeyMaterialDeriver(array3);
								sgjhx2 = new tmtag(keyMaterialDeriver);
							}
						}
						else
						{
							byte[] p13 = kvnxu.ubcyg(p0: false);
							array2 = gzxty(xcyjy, null, p13);
						}
					}
					catch
					{
						array2 = null;
					}
				}
				else
				{
					try
					{
						if (ubcvm == null || 1 == 0)
						{
							kvnxu.uecax(cdblf.hdraz, yvivf.LeafCertificate);
						}
						else
						{
							kvnxu.isbyb(cdblf.hdraz, xcyjy);
							xcyjy.Dispose();
						}
						array2 = kvnxu.mujic;
					}
					catch
					{
						array2 = null;
					}
					if (array2 != null && 0 == 0)
					{
						if (array2 != null && 0 == 0 && array2.Length != 48)
						{
							Array.Clear(array2, 0, array2.Length);
							array2 = null;
						}
						else if ((base.knmxu.Options & TlsOptions.SkipRollbackDetection) != TlsOptions.None && 0 == 0)
						{
							int num = (array2[0] << 8) + array2[1];
							if (num != cdblf.hdraz)
							{
								Array.Clear(array2, 0, array2.Length);
								array2 = null;
							}
						}
					}
				}
				if (sgjhx2 == null || 1 == 0)
				{
					if (array2 == null || 1 == 0)
					{
						array2 = new byte[48];
						jtxhe.ubsib(array2, 0, 48);
					}
					sgjhx2 = new onash(array2);
				}
				tubqp = new fldyu(pbpdx, TlsConnectionEnd.Server, yrmwj.ioriz, cdblf.okraw);
				tubqp.opzff(sgjhx2, (beohg ? true : false) ? yqdqs : null);
				tubqp.ybvgj();
				base.pmfcd = tubqp.juoso;
				base.ihtfw = tubqp.pbohw;
				break;
			}
			finally
			{
				if (xcyjy != null && 0 == 0)
				{
					xcyjy.Dispose();
				}
				if (yvivf != null && 0 == 0)
				{
					yvivf = yvivf.qdige();
				}
			}
		case nsvut.enndd:
		{
			vachj.kxlvu(TlsDebugEventType.CertificateVerify, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			vssis vssis2 = (vssis)p3;
			if (jcsyq == null || 1 == 0)
			{
				throw new TlsException(mjddr.fvtwt);
			}
			CertificateChain certificateChain2 = jcsyq.qdige();
			try
			{
				Certificate leafCertificate = certificateChain2.LeafCertificate;
				vssis2.hrbim(leafCertificate.KeyAlgorithm, out var p11, out var p12);
				if (!ofuit.mhozp(jevdi, leafCertificate.KeyAlgorithm, p12) || 1 == 0)
				{
					throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
				}
				byte[] hash = tubqp.wfrbn(p4, p11);
				if (!leafCertificate.VerifyHash(hash, p12, vssis2.gksmh) || 1 == 0)
				{
					throw new TlsException(mjddr.fvtwt);
				}
				ipjff(null, "client", certificateChain2);
				break;
			}
			finally
			{
				if (certificateChain2 != null && 0 == 0)
				{
					((IDisposable)certificateChain2).Dispose();
				}
			}
		}
		case nsvut.oaufz:
		{
			euwhd euwhd2 = (euwhd)p3;
			vachj.kxlvu(TlsDebugEventType.Finished, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			if (base.pmfcd != null || base.xsiiu == bpnki.yiqfh)
			{
				throw new TlsException(mjddr.ypibb, "Received Finished, but the preceding ChangeCipherSpec was missing.");
			}
			byte[] p5 = (ttgfn = tubqp.wgqsm(yqdqs, TlsConnectionEnd.Client));
			if (!objjc || 1 == 0)
			{
				if (!jtxhe.hbsgb(p5, euwhd2.lkwbh) || 1 == 0)
				{
					throw new TlsException(mjddr.wmgut, "Invalid Finished verify data.");
				}
				nfbik nfbik2 = new nfbik();
				uxxiy(nfbik2);
				vachj.jkyaz(TlsDebugEventType.ChangeCipherSpec, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, nfbik2);
				vituh();
				unnsm(p0, p1, p2);
				euwhd2 = new euwhd(kujxz = tubqp.wgqsm(yqdqs, TlsConnectionEnd.Server));
				vachj.jkyaz(TlsDebugEventType.Finished, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, euwhd2);
				uxxiy(euwhd2);
				vituh();
			}
			yqdqs = null;
			liroy = ((base.knmxu.Options & TlsOptions.DoNotInsertEmptyFragment) == 0 || 1 == 0) && pbpdx.Cbc && cdblf.hdraz <= 769;
			if (!objjc || 1 == 0)
			{
				webdt webdt2 = base.knmxu.auypu();
				if (webdt2 != null && 0 == 0)
				{
					TlsSession p6 = new TlsSession(cdblf.finbq, tubqp.qqffv, yvivf, jcsyq, beohg);
					webdt2.lacjz(cdblf.finbq, p6);
				}
			}
			olskl = true;
			otnhu();
			break;
		}
		default:
			vachj.kxlvu(TlsDebugEventType.UnknownHandshakeMessage, TlsDebugEventSource.Received, TlsDebugLevel.Important, p0, p1, p2);
			break;
		}
	}

	private void pxsjp(byte[] p0, int p1, int p2, ofuit p3)
	{
		nsvut zijgx = p3.zijgx;
		if (zijgx != nsvut.rvoru && zijgx != nsvut.oaufz)
		{
			unnsm(p0, p1, p2);
		}
		TlsKeyExchangeAlgorithm keyExchangeAlgorithm;
		CertificateChain certificateChain;
		Certificate certificate;
		bool flag;
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm;
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm2;
		byte[] yc;
		bool ecc;
		sgjhx p4;
		byte[] array;
		nfbik nfbik3;
		euwhd euwhd2;
		switch (p3.zijgx)
		{
		case nsvut.rvoru:
			vachj.kxlvu(TlsDebugEventType.HelloRequest, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			kaqny();
			break;
		case nsvut.lnjcv:
			vachj.kxlvu(TlsDebugEventType.ClientHello, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			throw new TlsException(mjddr.ypibb, brgjd.edcru("Received {0} from a server.", "ClientHello"));
		case nsvut.jthry:
		{
			cdblf = (kzdrw)p3;
			if (!base.knmxu.aljgi || 1 == 0)
			{
				vachj.kxlvu(TlsDebugEventType.ServerHello, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			}
			if (cdblf.hdraz != yrmwj.szwbv)
			{
				string text = TlsCipher.tvgwn((TlsProtocol)yrmwj.szwbv);
				kxjds(LogLevel.Info, "Client requested {0}, server is asking for {1}.", text, TlsCipher.tvgwn((TlsProtocol)cdblf.hdraz));
			}
			pbpdx = yrmwj.mtlwh(cdblf, base.knmxu.vhnmk());
			kxjds(LogLevel.Info, "Negotiating {0}.", pbpdx);
			lwwzy = cdblf.hdraz;
			bdlzj = cdblf.dgjdv();
			if (udugd && 0 == 0)
			{
				lxhmj(TlsConnectionEnd.Client);
			}
			bool flag2 = yrmwj.kchbb.Length > 0 && jtxhe.hbsgb(yrmwj.kchbb, cdblf.finbq);
			if (ejgqw && 0 == 0)
			{
				ohrnk(TlsConnectionEnd.Client, flag2);
			}
			if (flag2 && 0 == 0)
			{
				vachj.ivtmn(TlsDebugEventType.ResumingCachedSession, TlsDebugEventSource.Unspecified, TlsDebugLevel.Important);
				yvivf = eqbmp.twwdt;
				tubqp = new fldyu(pbpdx, TlsConnectionEnd.Client, yrmwj.ioriz, cdblf.okraw);
				tubqp.qqffv = eqbmp.tydvs;
				tubqp.ybvgj();
				base.pmfcd = tubqp.juoso;
				base.ihtfw = tubqp.pbohw;
				objjc = true;
			}
			else
			{
				objjc = false;
				eqbmp = null;
			}
			break;
		}
		case nsvut.upgjx:
		{
			eupdk eupdk2 = (eupdk)p3;
			vachj.kxlvu(TlsDebugEventType.Certificate, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			yvivf = eupdk2.tycix();
			Certificate leafCertificate = yvivf.LeafCertificate;
			KeyAlgorithm keyAlgorithm = ((yvivf != null) ? leafCertificate.KeyAlgorithm : KeyAlgorithm.Unsupported);
			if (keyAlgorithm == KeyAlgorithm.Unsupported || keyAlgorithm != pbpdx.hflum)
			{
				throw new TlsException(mjddr.ypibb, "Received unsuitable server certificate.");
			}
			if (pbpdx.KeyExchangeAlgorithm == TlsKeyExchangeAlgorithm.DH_anon)
			{
				throw new TlsException(mjddr.ypibb, "Received unexpected server certificate while negotiating anonymous cipher.");
			}
			if (keyAlgorithm == KeyAlgorithm.ECDsa)
			{
				urofm urofm2 = TlsParameters.nbrrb(leafCertificate);
				if (urofm2 == (urofm)0 || false || !yrmwj.yiwjt(urofm2) || 1 == 0)
				{
					throw new TlsException(mjddr.ypibb, "Received unexpected server certificate while negotiating anonymous cipher.");
				}
			}
			break;
		}
		case nsvut.iysge:
			ubcvm = (ccgaj)p3;
			vachj.kxlvu(TlsDebugEventType.ServerKeyExchange, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			break;
		case nsvut.jctye:
			ogbin = (cujbv)p3;
			vachj.kxlvu(TlsDebugEventType.CertificateRequest, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			break;
		case nsvut.wsvfx:
		{
			ierva = (watdq)p3;
			vachj.kxlvu(TlsDebugEventType.ServerHelloDone, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			keyExchangeAlgorithm = pbpdx.KeyExchangeAlgorithm;
			if (base.pmfcd != null || base.ihtfw != null)
			{
				throw new TlsException(mjddr.ypibb, "Received ServerHelloDone, but another handshake is already pending.");
			}
			if (yvivf == null || 1 == 0)
			{
				if (keyExchangeAlgorithm != TlsKeyExchangeAlgorithm.DH_anon)
				{
					throw new TlsException(mjddr.ypibb, "Received ServerHelloDone, but the certificate was not received from the server.");
				}
				certificateChain = null;
				certificate = null;
				flag = true;
				if (flag)
				{
					goto IL_04f3;
				}
			}
			certificateChain = yvivf.qdige();
			certificate = certificateChain.LeafCertificate;
			ipjff(qnwpy, "server", certificateChain);
			int keySize2 = certificateChain.LeafCertificate.GetKeySize();
			flag = pbpdx.KeyExchangeAlgorithm != TlsKeyExchangeAlgorithm.RSA || (pbpdx.Exportable && 0 == 0 && keySize2 > pbpdx.zorll);
			goto IL_04f3;
		}
		case nsvut.enndd:
			vachj.kxlvu(TlsDebugEventType.CertificateVerify, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			throw new TlsException(mjddr.ypibb, brgjd.edcru("Received {0} from a server.", "CertificateVerify"));
		case nsvut.zmyoj:
			vachj.kxlvu(TlsDebugEventType.ClientKeyExchange, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			throw new TlsException(mjddr.ypibb, brgjd.edcru("Received {0} from a server.", "ClientKeyExchange"));
		case nsvut.oaufz:
			euwhd2 = (euwhd)p3;
			vachj.kxlvu(TlsDebugEventType.Finished, TlsDebugEventSource.Received, TlsDebugLevel.Detailed, p0, p1, p2);
			if (base.pmfcd != null || base.xsiiu == bpnki.yiqfh)
			{
				throw new TlsException(mjddr.ypibb, "Received Finished, but the preceding ChangeCipherSpec was missing.");
			}
			if (!jtxhe.hbsgb(kujxz = tubqp.wgqsm(yqdqs, TlsConnectionEnd.Server), euwhd2.lkwbh) || 1 == 0)
			{
				throw new TlsException(mjddr.wmgut, "Invalid Finished verify data.");
			}
			if (objjc && 0 == 0)
			{
				unnsm(p0, p1, p2);
				nfbik nfbik2 = new nfbik();
				uxxiy(nfbik2);
				vachj.jkyaz(TlsDebugEventType.ChangeCipherSpec, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, nfbik2);
				vituh();
				euwhd2 = new euwhd(ttgfn = tubqp.wgqsm(yqdqs, TlsConnectionEnd.Client));
				vachj.jkyaz(TlsDebugEventType.Finished, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, euwhd2);
				uxxiy(euwhd2);
				vituh();
			}
			yqdqs = null;
			liroy = ((base.knmxu.Options & TlsOptions.DoNotInsertEmptyFragment) == 0 || 1 == 0) && pbpdx.Cbc && cdblf.hdraz <= 769;
			olskl = true;
			otnhu();
			break;
		default:
			{
				vachj.kxlvu(TlsDebugEventType.UnknownHandshakeMessage, TlsDebugEventSource.Received, TlsDebugLevel.Important, p0, p1, p2);
				break;
			}
			IL_04f3:
			asymmetricKeyAlgorithm = null;
			asymmetricKeyAlgorithm2 = null;
			array = null;
			yc = null;
			ecc = false;
			p4 = null;
			if (ubcvm == null || 1 == 0)
			{
				if (flag && 0 == 0)
				{
					throw new TlsException(mjddr.jhrgr, "ServerKeyExchange message was not sent by the server.");
				}
				if (certificate.KeyAlgorithm != KeyAlgorithm.RSA && 0 == 0)
				{
					throw new TlsException(mjddr.fvtwt);
				}
				asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
				RSAParameters rSAParameters = certificate.GetRSAParameters();
				asymmetricKeyAlgorithm.ImportKey(rSAParameters);
			}
			else
			{
				if (!flag || 1 == 0)
				{
					throw new TlsException(mjddr.jhrgr, "Received unexpected {0}.");
				}
				if (certificate != null && 0 == 0)
				{
					lvwig(LogLevel.Debug, "Verifying server key exchange signature.");
					if (!ubcvm.ynbct(certificate, jevdi, yrmwj.ioriz, cdblf.okraw) || 1 == 0)
					{
						throw new TlsException(mjddr.wmgut, "Unable to verify ServerKeyExchange signature.");
					}
				}
				switch (keyExchangeAlgorithm)
				{
				case TlsKeyExchangeAlgorithm.RSA:
				{
					asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
					RSAParameters key = ubcvm.dtcci();
					if (pbpdx.Exportable && 0 == 0 && jtxhe.kexzb(key.Modulus) * 8 > pbpdx.zorll)
					{
						throw new TlsException(mjddr.nkvah, "Invalid key length for exportable cipher.");
					}
					asymmetricKeyAlgorithm.ImportKey(key);
					break;
				}
				case TlsKeyExchangeAlgorithm.ECDHE_RSA:
				case TlsKeyExchangeAlgorithm.ECDHE_ECDSA:
				{
					urofm p5 = ubcvm.anrsz();
					if (!base.knmxu.tokwu(p5, out var _, out var p7, out var p8, out var _) || 1 == 0)
					{
						throw new TlsException(mjddr.jhrgr, "Unsupported curve.");
					}
					if (!yrmwj.yiwjt(p5) || 1 == 0)
					{
						throw new TlsException(mjddr.jhrgr, "Server is trying to use a curve that is not supported by the client.");
					}
					kxjds(LogLevel.Debug, "Using ephemeral ECDH public key exchange with {0}.", p8);
					byte[] array2 = ubcvm.afugf();
					ecc = true;
					asymmetricKeyAlgorithm2 = new AsymmetricKeyAlgorithm();
					asymmetricKeyAlgorithm2.kvrol(AsymmetricKeyAlgorithmId.ECDH, p7, 0);
					yc = asymmetricKeyAlgorithm2.zimkk();
					array = asymmetricKeyAlgorithm2.qpwqy(array2);
					if (array == null || 1 == 0)
					{
						KeyMaterialDeriver keyMaterialDeriver = asymmetricKeyAlgorithm2.GetKeyMaterialDeriver(array2);
						p4 = new tmtag(keyMaterialDeriver);
					}
					else
					{
						p4 = new onash(array);
					}
					break;
				}
				default:
				{
					asymmetricKeyAlgorithm2 = new AsymmetricKeyAlgorithm();
					lvwig(LogLevel.Debug, "Received ephemeral Diffie-Hellman prime.");
					DiffieHellmanParameters diffieHellmanParameters = ubcvm.acery();
					byte[] y = diffieHellmanParameters.Y;
					asymmetricKeyAlgorithm2.ImportKey(diffieHellmanParameters);
					int keySize = asymmetricKeyAlgorithm2.KeySize;
					int minimumDiffieHellmanKeySize = base.knmxu.MinimumDiffieHellmanKeySize;
					kxjds(LogLevel.Debug, "Ephemeral Diffie-Hellman prime size is {0} bits (minimum allowed size is {1} bits).", keySize, minimumDiffieHellmanKeySize);
					if (keySize < minimumDiffieHellmanKeySize)
					{
						throw new TlsException(mjddr.jhrgr, "Ephemeral Diffie-Hellman prime received from the server is considered weak.");
					}
					yc = xrdts(asymmetricKeyAlgorithm2);
					array = gzxty(asymmetricKeyAlgorithm2, diffieHellmanParameters, y);
					p4 = new onash(array);
					break;
				}
				}
			}
			if (certificateChain != null && 0 == 0)
			{
				certificateChain.Dispose();
			}
			if (ogbin != null && 0 == 0)
			{
				if (jcsyq == null || 1 == 0)
				{
					CertificateChain certificateChain2;
					try
					{
						lvwig(LogLevel.Debug, "Client certificate authentication was requested.");
						certificateChain2 = base.knmxu.CertificateRequestHandler.Request(vachj.xlbxi, ogbin.ilqau);
					}
					catch (Exception inner)
					{
						throw new TlsException("An exception occurred in certificate request handler.", inner);
					}
					if (certificateChain2 != null && 0 == 0 && certificateChain2.LeafCertificate != null && 0 == 0)
					{
						jcsyq = certificateChain2;
					}
				}
				if (jcsyq != null || cdblf.hdraz >= 769)
				{
					if (jcsyq == null || 1 == 0)
					{
						lvwig(LogLevel.Debug, "No suitable client certificate is available.");
					}
					else
					{
						kxjds(LogLevel.Debug, "Suitable client certificate is available ('{0}').", jcsyq.LeafCertificate.GetSubjectName());
					}
					eupdk eupdk3 = new eupdk(jcsyq, base.knmxu);
					uxxiy(eupdk3);
					vachj.jkyaz(TlsDebugEventType.Certificate, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, eupdk3);
				}
				else
				{
					zppmb zppmb2 = new zppmb(rtzwv.iddlf, mjddr.frppg);
					uxxiy(zppmb2);
					kxjds(LogLevel.Debug, "{0} was sent.", zppmb2);
					vachj.jkyaz(TlsDebugEventType.Alert, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, zppmb2);
				}
				vituh();
			}
			if (asymmetricKeyAlgorithm != null && 0 == 0)
			{
				kvnxu = new ypafr(yrmwj.szwbv);
				kvnxu.ueuef(cdblf.hdraz, asymmetricKeyAlgorithm);
				p4 = new onash(kvnxu.mujic);
				asymmetricKeyAlgorithm.Dispose();
			}
			else
			{
				kvnxu = new ypafr(yc, ecc);
			}
			uxxiy(kvnxu);
			vachj.jkyaz(TlsDebugEventType.ClientKeyExchange, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, kvnxu);
			tubqp = new fldyu(pbpdx, TlsConnectionEnd.Client, yrmwj.ioriz, cdblf.okraw);
			tubqp.opzff(p4, (beohg ? true : false) ? yqdqs : null);
			tubqp.ybvgj();
			if (asymmetricKeyAlgorithm2 != null && 0 == 0)
			{
				asymmetricKeyAlgorithm2.Dispose();
			}
			if (jcsyq != null && 0 == 0)
			{
				lvwig(LogLevel.Info, "Performing client certificate authentication.");
				Certificate leafCertificate2 = jcsyq.LeafCertificate;
				tubqp.llosq(leafCertificate2.KeyAlgorithm, base.knmxu.PreferredHashAlgorithm, jevdi, out var p10, out var p11);
				byte[] hash = tubqp.wfrbn(yqdqs, p11);
				byte[] signature = leafCertificate2.SignHash(hash, p10, silent: false);
				vssis vssis2 = new vssis(p10, leafCertificate2.KeyAlgorithm, signature, base.gzlwv);
				uxxiy(vssis2);
				vachj.jkyaz(TlsDebugEventType.CertificateVerify, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, vssis2);
			}
			base.pmfcd = tubqp.juoso;
			base.ihtfw = tubqp.pbohw;
			nfbik3 = new nfbik();
			uxxiy(nfbik3);
			vachj.jkyaz(TlsDebugEventType.ChangeCipherSpec, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, nfbik3);
			euwhd2 = new euwhd(ttgfn = tubqp.wgqsm(yqdqs, TlsConnectionEnd.Client));
			vachj.jkyaz(TlsDebugEventType.Finished, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, euwhd2);
			uxxiy(euwhd2);
			vituh();
			break;
		}
	}

	private byte[] gzxty(AsymmetricKeyAlgorithm p0, DiffieHellmanParameters? p1, byte[] p2)
	{
		byte[] p3 = p0.fevai(p2);
		int keySize = p0.KeySize;
		if (p1.HasValue && 0 == 0 && p3.Length * 8 < keySize && btqpb(p1) && 0 == 0)
		{
			nmksm(ref p3, p0.KeySize);
			lvwig(LogLevel.Debug, "Applied alternative Diffie-Hellman premaster secret padding.");
		}
		return p3;
	}

	private byte[] xrdts(AsymmetricKeyAlgorithm p0)
	{
		byte[] p1 = p0.zimkk();
		if (nmksm(ref p1, p0.KeySize) && 0 == 0)
		{
			lvwig(LogLevel.Debug, "Applied Diffie-Hellman key padding workaround.");
		}
		return p1;
	}

	private static bool nmksm(ref byte[] p0, int p1)
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

	private static bool btqpb(DiffieHellmanParameters? p0)
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

	private void lxhmj(TlsConnectionEnd p0)
	{
		bool flag = p0 == TlsConnectionEnd.Client;
		string text = ((flag ? true : false) ? "server" : "client");
		byte[] array = ((flag ? true : false) ? cdblf.ckhgw() : yrmwj.nsqrx());
		if (array == null || 1 == 0)
		{
			if (bbnuu && 0 == 0)
			{
				kxjds(LogLevel.Error, "The {0} did not provide renegotiation info.", text);
				throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "Secure renegotiation failure.", null);
			}
			kxjds(LogLevel.Debug, "The {0} does not support secure renegotiation.", text);
			return;
		}
		int num = array.Length - 1;
		if (num < 0 || array[0] != num)
		{
			kxjds(LogLevel.Error, "The {0} sent broken renegotiation info.", text);
			throw new TlsException(rtzwv.iogyt, mjddr.gkkle, "Secure renegotiation failure.", null);
		}
		if (!bbnuu || 1 == 0)
		{
			if (olskl && 0 == 0)
			{
				kxjds(LogLevel.Error, "The {0} claims to support secure renegotiation despite previously claiming otherwise.", text);
				throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "Secure renegotiation failure.", null);
			}
			bbnuu = true;
			if (num != 0 && 0 == 0)
			{
				kxjds(LogLevel.Error, "The {0} sent bad renegotiation info.", text);
				throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "Secure renegotiation failure.", null);
			}
			kxjds(LogLevel.Debug, "The {0} supports secure renegotiation.", text);
			return;
		}
		bool flag2 = false;
		int num2;
		int num3;
		if (num == ttgfn.Length + ((flag ? true : false) ? kujxz.Length : 0))
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
		if (num4 < kujxz.Length)
		{
			goto IL_01fe;
		}
		goto IL_022a;
		IL_01fe:
		flag2 &= array[num2] == kujxz[num4];
		num2++;
		num4++;
		goto IL_021e;
		IL_01c0:
		flag2 &= array[num2] == ttgfn[num3];
		num2++;
		num3++;
		goto IL_01e0;
		IL_01e0:
		if (num3 < ttgfn.Length)
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
			kxjds(LogLevel.Error, "The {0} sent wrong renegotiation info.", text);
			throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "Secure renegotiation failure.", null);
		}
		lvwig(LogLevel.Debug, "Performing secure renegotiation.");
	}

	private void ohrnk(TlsConnectionEnd p0, bool p1)
	{
		bool flag = p0 == TlsConnectionEnd.Client;
		string text = ((flag ? true : false) ? "server" : "client");
		if (((flag ? true : false) ? cdblf.tzsfd() : yrmwj.jhgcv()) && 0 == 0)
		{
			if (!beohg || 1 == 0)
			{
				if ((olskl ? true : false) || p1)
				{
					kxjds(LogLevel.Error, "The {0} claims to support extended master secret despite previously claiming otherwise.", text);
					throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "Negotiation failure.", null);
				}
				beohg = true;
			}
			lvwig(LogLevel.Debug, "Extended master secret is enabled.");
		}
		else
		{
			if (beohg && 0 == 0)
			{
				kxjds(LogLevel.Error, "The {0} claims not to support extended master secret despite previously claiming otherwise.", text);
				throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "Negotiation failure.", null);
			}
			kxjds(LogLevel.Debug, "The {0} does not support extended master secret.", text);
		}
	}

	protected override void tcbzt(byte[] p0, int p1, int p2)
	{
		zppmb zppmb2 = new zppmb(p0[p1], p0[p1 + 1]);
		kxjds((zppmb2.bgpwy ? true : false) ? LogLevel.Info : LogLevel.Debug, "{0} was received.", zppmb2);
		vachj.kxlvu(TlsDebugEventType.Alert, TlsDebugEventSource.Received, TlsDebugLevel.Important, p0, p1, p2);
		if (zppmb2.jmwmm == 100 && base.mgfog == hlkgm.iucmn)
		{
			throw new TlsException(isRemote: true, "Renegotiation rejected by the remote connection end.");
		}
		if (zppmb2.wzwvm == 2)
		{
			throw new TlsException(isRemote: true, TlsException.feqgy((mjddr)zppmb2.jmwmm, p1: true));
		}
		if (zppmb2.jmwmm == 0)
		{
			cdaod(1, zppmb2.jmwmm, p2: false);
		}
	}

	private void unnsm(byte[] p0, int p1, int p2)
	{
		yqdqs.Write(p0, p1, p2);
	}

	private void uxxiy(qoqui p0)
	{
		byte[] array = p0.szrqi();
		if (p0.qeloj == vcedo.ztfcr && (!(p0 is tjpdc) || 1 == 0))
		{
			unnsm(array, 0, array.Length);
		}
		jwkmn(p0.qeloj, array, 0, array.Length);
	}

	protected override void kaqny()
	{
		Func<string> func = null;
		Func<string> func2 = null;
		Func<string> func3 = null;
		fqfeg fqfeg = new fqfeg();
		bool flag = false;
		if (!gvlry() || 1 == 0)
		{
			return;
		}
		fqfeg.dtfwc = base.knmxu.tznry();
		wgllv(LogLevel.Debug, fqfeg.cuzlu);
		TlsVersion tlsVersion = base.knmxu.vhnmk();
		TlsProtocol tlsProtocol;
		if ((tlsVersion & TlsVersion.TLS12) != TlsVersion.None && 0 == 0)
		{
			tlsProtocol = TlsProtocol.TLS12;
			if (tlsProtocol != TlsProtocol.None)
			{
				goto IL_0106;
			}
		}
		if ((tlsVersion & TlsVersion.TLS11) != TlsVersion.None && 0 == 0)
		{
			tlsProtocol = TlsProtocol.TLS11;
			fqfeg.dtfwc &= ~(TlsCipherSuite.Secure | TlsCipherSuite.DHE_DSS_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_GCM_SHA384);
		}
		else if ((tlsVersion & TlsVersion.TLS10) != TlsVersion.None && 0 == 0)
		{
			tlsProtocol = TlsProtocol.TLS10;
			fqfeg.dtfwc &= ~(TlsCipherSuite.Secure | TlsCipherSuite.DHE_DSS_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_GCM_SHA384);
		}
		else
		{
			if ((tlsVersion & TlsVersion.SSL30) == 0)
			{
				throw new TlsException(mjddr.puqjh, "TLS/SSL protocol version not specified or not allowed.");
			}
			tlsProtocol = TlsProtocol.SSL30;
			fqfeg.dtfwc &= TlsCipherSuite.RSA_EXPORT_WITH_RC4_40_MD5 | TlsCipherSuite.RSA_WITH_RC4_128_MD5 | TlsCipherSuite.RSA_WITH_RC4_128_SHA | TlsCipherSuite.RSA_EXPORT_WITH_RC2_CBC_40_MD5 | TlsCipherSuite.RSA_EXPORT_WITH_DES40_CBC_SHA | TlsCipherSuite.RSA_WITH_DES_CBC_SHA | TlsCipherSuite.RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.RSA_EXPORT1024_WITH_DES_CBC_SHA | TlsCipherSuite.RSA_EXPORT1024_WITH_RC4_56_SHA | TlsCipherSuite.DH_anon_WITH_RC4_128_MD5 | TlsCipherSuite.DH_anon_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.DH_anon_WITH_DES_CBC_SHA;
		}
		goto IL_0106;
		IL_0106:
		if (fqfeg.dtfwc == TlsCipherSuite.None)
		{
			throw new TlsException(rtzwv.iogyt, mjddr.jhrgr, "All usable cipher suites have been disabled.", null);
		}
		if (olskl && 0 == 0)
		{
			tlsProtocol = (TlsProtocol)lwwzy;
		}
		else
		{
			udugd = (base.knmxu.Options & TlsOptions.DisableRenegotiationExtension) == 0;
			ejgqw = base.knmxu.uocnj();
		}
		if (base.knmxu.Entity == TlsConnectionEnd.Client)
		{
			yqdqs = new MemoryStream();
			byte[] array = null;
			eqbmp = base.knmxu.Session;
			if (eqbmp != null && 0 == 0)
			{
				if (!olskl || 1 == 0)
				{
					array = eqbmp.wqpxu;
				}
				jcsyq = eqbmp.xalxj;
				ejgqw = (beohg = eqbmp.zqpiy);
			}
			wmbjj wmbjj2 = new wmbjj();
			if ((fqfeg.dtfwc & (TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_RC4_128_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA384 | TlsCipherSuite.ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_RC4_128_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256)) != 0 && (!yzucu(wmbjj2, tlsProtocol, base.knmxu) || 1 == 0))
			{
				fqfeg.dtfwc &= ~(TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_RC4_128_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA384 | TlsCipherSuite.ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_RC4_128_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256);
			}
			if (tlsProtocol == TlsProtocol.TLS12)
			{
				viwzf(wmbjj2, base.knmxu.ggdwh);
			}
			bool announceRenegotiationIndication = false;
			if (udugd && 0 == 0)
			{
				if (bbnuu && 0 == 0)
				{
					asnnl(wmbjj2, ttgfn, null);
				}
				else
				{
					announceRenegotiationIndication = true;
				}
			}
			if (ejgqw && 0 == 0 && tlsProtocol >= TlsProtocol.TLS10)
			{
				phadm(wmbjj2);
			}
			okkma(wmbjj2);
			if (func == null || 1 == 0)
			{
				func = fqfeg.ltrgy;
			}
			wgllv(LogLevel.Debug, func);
			if ((fqfeg.dtfwc & (TlsCipherSuite)8926398345262397440L) != 0)
			{
				if (func2 == null || 1 == 0)
				{
					func2 = fqfeg.clvry;
				}
				wgllv(LogLevel.Debug, func2);
			}
			if ((fqfeg.dtfwc & (TlsCipherSuite.RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.DHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.DHE_DSS_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_GCM_SHA384)) != 0)
			{
				if (func3 == null || 1 == 0)
				{
					func3 = fqfeg.llelg;
				}
				wgllv(LogLevel.Debug, func3);
			}
			string serverName = (((base.knmxu.Options & TlsOptions.DisableServerNameIndication) == 0) ? qnwpy : null);
			yrmwj = new aoind(tlsProtocol, fqfeg.dtfwc, announceRenegotiationIndication, base.knmxu, array, serverName, wmbjj2.ihelo());
			lwwzy = yrmwj.szwbv;
			jevdi = yrmwj.dvlkv();
			if (array != null && 0 == 0)
			{
				lvwig(LogLevel.Info, "Trying to resume session.");
				otbhg(LogLevel.Verbose, "Session to resume: ", array, 0, array.Length);
			}
			uxxiy(yrmwj);
			flag = true;
			if (flag)
			{
				goto IL_0415;
			}
		}
		base.knmxu.wnzfz();
		if (olskl && 0 == 0)
		{
			uxxiy(new tjpdc());
		}
		goto IL_0415;
		IL_0415:
		vachj.ivtmn(TlsDebugEventType.Negotiating, TlsDebugEventSource.Unspecified, TlsDebugLevel.Important);
		if (flag && 0 == 0 && !base.knmxu.aljgi && 0 == 0)
		{
			vachj.jkyaz(TlsDebugEventType.ClientHello, TlsDebugEventSource.Sent, TlsDebugLevel.Detailed, yrmwj);
		}
		vituh();
	}

	private void okkma(wmbjj p0)
	{
	}

	private void viujn(wmbjj p0, string p1)
	{
	}

	private static void phadm(wmbjj p0)
	{
		p0.mmgwn(23);
		p0.mmgwn(0);
	}

	private static void asnnl(wmbjj p0, byte[] p1, byte[] p2)
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

	private static void viwzf(wmbjj p0, ttceu p1)
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

	private static bool yzucu(wmbjj p0, TlsProtocol p1, TlsParameters p2)
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
		return base.nvoco.xtyvd();
	}

	private static bool qvssb(CertificateChain p0)
	{
		if (p0 != null && 0 == 0)
		{
			return p0.LeafCertificate != null;
		}
		return false;
	}
}
