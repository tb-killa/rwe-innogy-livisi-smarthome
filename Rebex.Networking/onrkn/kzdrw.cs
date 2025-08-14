using System;
using System.Collections.Generic;
using Rebex.Net;
using Rebex.Security.Certificates;

namespace onrkn;

internal class kzdrw : wvwdg
{
	private int ktguj;

	private byte[] omhhj;

	private byte[] hfjjl;

	private int vvowd;

	private byte qoaim;

	private urofm? hcwtn;

	public override int nimwj => 42 + hfjjl.Length + ((uqsbq != null && 0 == 0) ? (uqsbq.Length + 2) : 0);

	public int hdraz => ktguj;

	public byte[] okraw => omhhj;

	public byte[] finbq => hfjjl;

	public int csymh => vvowd;

	public urofm? izerh => hcwtn;

	public string dgjdv()
	{
		return null;
	}

	public byte[] ckhgw()
	{
		nxtme<byte> nxtme2 = kqmgh(eppge.pnxlm);
		if (!nxtme2.hvbtp || 1 == 0)
		{
			return nxtme2.trkhv();
		}
		return null;
	}

	public bool tzsfd()
	{
		nxtme<byte> nxtme2 = kqmgh(eppge.gynad, p1: true);
		if (nxtme2.tvoem == 2 && (nxtme2[0] == 0 || 1 == 0))
		{
			return nxtme2[1] == 0;
		}
		return false;
	}

	public override void gjile(byte[] p0, int p1)
	{
		base.gjile(p0, p1);
		p0[p1 + 4] = (byte)((ktguj >> 8) & 0xFF);
		p0[p1 + 5] = (byte)(ktguj & 0xFF);
		p1 += 6;
		omhhj.CopyTo(p0, p1);
		p1 += omhhj.Length;
		p0[p1] = (byte)hfjjl.Length;
		hfjjl.CopyTo(p0, p1 + 1);
		p1 += 1 + hfjjl.Length;
		p0[p1] = (byte)(vvowd >> 8);
		p0[p1 + 1] = (byte)(vvowd & 0xFF);
		p0[p1 + 2] = qoaim;
		p1 += 3;
		if (uqsbq != null && 0 == 0)
		{
			int num = uqsbq.Length;
			p0[p1] = (byte)(num >> 8);
			p0[p1 + 1] = (byte)(num & 0xFF);
			p1 += 2;
			uqsbq.CopyTo(p0, p1);
		}
	}

	public kzdrw(byte[] buffer, int offset, int length)
		: base(nsvut.jthry)
	{
		if (length < 37)
		{
			throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "ServerHello"));
		}
		if (buffer[38 + offset] < 0 || buffer[38 + offset] > 32 || length - buffer[38 + offset] - 42 < 0)
		{
			throw new TlsException(mjddr.gkkle, "Message with invalid SessionID.");
		}
		int num = offset + length;
		ktguj = (buffer[offset + 4] << 8) + buffer[offset + 5];
		omhhj = new byte[32];
		Array.Copy(buffer, offset + 6, omhhj, 0, 32);
		int num2 = buffer[offset + 38];
		hfjjl = new byte[num2];
		if (num2 > 0)
		{
			Array.Copy(buffer, offset + 39, hfjjl, 0, num2);
		}
		vvowd = (buffer[offset + num2 + 39] << 8) + buffer[offset + num2 + 40];
		qoaim = buffer[offset + num2 + 41];
		offset += num2 + 42;
		if (num >= offset + 2)
		{
			int num3 = (buffer[offset] << 8) + buffer[offset + 1];
			offset += 2;
			if (num < offset + num3)
			{
				throw new TlsException(mjddr.gkkle, brgjd.edcru("Invalid {0} message.", "ServerHello"));
			}
			uqsbq = new byte[num3];
			Array.Copy(buffer, offset, uqsbq, 0, num3);
			offset += num3;
		}
	}

	public kzdrw(TlsParameters parameters, Certificate certificate, aoind clientHello, byte[] sessionId, byte[] extensions, out TlsCipher cipher)
		: base(nsvut.jthry)
	{
		if (clientHello.szwbv < 768)
		{
			throw new TlsException(mjddr.jhrgr, "Client only supports the deprecated SSL 2.0 protocol");
		}
		TlsVersion tlsVersion = parameters.vhnmk();
		if (tlsVersion == TlsVersion.None || 1 == 0)
		{
			throw new TlsException(mjddr.puqjh, "Client is trying to use a version of TLS/SSL protocol that is not enabled at the server.");
		}
		TlsProtocol tlsProtocol;
		if (clientHello.szwbv > 770 && (tlsVersion & TlsVersion.TLS12) != TlsVersion.None && 0 == 0)
		{
			tlsProtocol = TlsProtocol.TLS12;
			if (tlsProtocol != TlsProtocol.None)
			{
				goto IL_00dc;
			}
		}
		if (clientHello.szwbv > 769 && (tlsVersion & TlsVersion.TLS11) != TlsVersion.None && 0 == 0)
		{
			tlsProtocol = TlsProtocol.TLS11;
			if (tlsProtocol != TlsProtocol.None)
			{
				goto IL_00dc;
			}
		}
		if (clientHello.szwbv > 768 && (tlsVersion & TlsVersion.TLS10) != TlsVersion.None && 0 == 0)
		{
			tlsProtocol = TlsProtocol.TLS10;
			if (tlsProtocol != TlsProtocol.None)
			{
				goto IL_00dc;
			}
		}
		if (clientHello.szwbv == 768 && (tlsVersion & TlsVersion.SSL30) != TlsVersion.None && 0 == 0)
		{
			tlsProtocol = TlsProtocol.SSL30;
			if (tlsProtocol != TlsProtocol.None)
			{
				goto IL_00dc;
			}
		}
		throw new TlsException(mjddr.puqjh, "Client is trying to use a version of TLS/SSL protocol that is not enabled at the server.");
		IL_00dc:
		ktguj = (int)tlsProtocol;
		int num = 0;
		urofm p = (urofm)0;
		if (certificate != null && 0 == 0)
		{
			num = certificate.GetKeySize();
			p = TlsParameters.nbrrb(certificate);
		}
		TlsCipherSuite tlsCipherSuite = parameters.tznry();
		if (tlsProtocol >= TlsProtocol.TLS11)
		{
			tlsCipherSuite &= ~(TlsCipherSuite.RSA_EXPORT_WITH_RC4_40_MD5 | TlsCipherSuite.RSA_WITH_RC4_128_MD5 | TlsCipherSuite.RSA_EXPORT_WITH_RC2_CBC_40_MD5 | TlsCipherSuite.RSA_EXPORT_WITH_DES40_CBC_SHA | TlsCipherSuite.RSA_WITH_DES_CBC_SHA | TlsCipherSuite.RSA_EXPORT1024_WITH_DES_CBC_SHA | TlsCipherSuite.RSA_EXPORT1024_WITH_RC4_56_SHA | TlsCipherSuite.DHE_DSS_EXPORT_WITH_DES40_CBC_SHA | TlsCipherSuite.DHE_DSS_WITH_DES_CBC_SHA | TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_DES_CBC_SHA | TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_RC4_56_SHA | TlsCipherSuite.DHE_RSA_WITH_DES_CBC_SHA | TlsCipherSuite.DH_anon_WITH_RC4_128_MD5 | TlsCipherSuite.DH_anon_WITH_DES_CBC_SHA);
		}
		if (tlsProtocol < TlsProtocol.TLS12)
		{
			tlsCipherSuite &= ~(TlsCipherSuite.Secure | TlsCipherSuite.DHE_DSS_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_GCM_SHA384);
		}
		IEnumerator<TlsCipherSuite> enumerator = TlsCipher.xnjzt(tlsCipherSuite, parameters.GetPreferredSuites()).GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				TlsCipherSuite current = enumerator.Current;
				TlsCipherSuiteId p2 = TlsCipher.plzju(current);
				urofm? urofm2 = null;
				if (!clientHello.mahuq(p2) || false || ((current & (TlsCipherSuite.RSA_EXPORT_WITH_RC4_40_MD5 | TlsCipherSuite.RSA_EXPORT_WITH_RC2_CBC_40_MD5 | TlsCipherSuite.RSA_EXPORT_WITH_DES40_CBC_SHA | TlsCipherSuite.DHE_DSS_EXPORT_WITH_DES40_CBC_SHA)) != 0 && certificate != null && 0 == 0 && num > 512 && (parameters.TemporaryRSAParameters.Modulus == null || false || parameters.TemporaryRSAParameters.Exponent == null || false || parameters.TemporaryRSAParameters.D == null || false || parameters.TemporaryRSAParameters.Modulus.Length > 64)) || ((current & (TlsCipherSuite.RSA_EXPORT1024_WITH_DES_CBC_SHA | TlsCipherSuite.RSA_EXPORT1024_WITH_RC4_56_SHA | TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_DES_CBC_SHA | TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_RC4_56_SHA)) != 0 && certificate != null && 0 == 0 && num > 1024 && (parameters.TemporaryRSAParameters.Modulus == null || false || parameters.TemporaryRSAParameters.Exponent == null || false || parameters.TemporaryRSAParameters.D == null || false || parameters.TemporaryRSAParameters.Modulus.Length > 128)) || ((current & (TlsCipherSuite)8926398345262397440L) != 0 && (parameters.EphemeralDiffieHellmanParameters.G == null || false || parameters.EphemeralDiffieHellmanParameters.P == null)))
				{
					continue;
				}
				if ((current & (TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_RC4_128_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA384 | TlsCipherSuite.ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_RC4_128_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256)) != 0)
				{
					urofm2 = clientHello.ndqws(parameters, tlsProtocol);
					urofm? urofm3 = urofm2;
					if ((urofm3.GetValueOrDefault() == (urofm)0 || 1 == 0) && (urofm3.HasValue ? true : false))
					{
						continue;
					}
				}
				if ((current & (TlsCipherSuite)8349673709144899584L) == 0)
				{
					if (certificate == null)
					{
						continue;
					}
					switch (certificate.KeyAlgorithm)
					{
					case KeyAlgorithm.RSA:
						if ((current & (TlsCipherSuite.RSA_EXPORT_WITH_RC4_40_MD5 | TlsCipherSuite.RSA_WITH_RC4_128_MD5 | TlsCipherSuite.RSA_WITH_RC4_128_SHA | TlsCipherSuite.RSA_EXPORT_WITH_RC2_CBC_40_MD5 | TlsCipherSuite.RSA_EXPORT_WITH_DES40_CBC_SHA | TlsCipherSuite.RSA_WITH_DES_CBC_SHA | TlsCipherSuite.RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.RSA_EXPORT1024_WITH_DES_CBC_SHA | TlsCipherSuite.RSA_EXPORT1024_WITH_RC4_56_SHA | TlsCipherSuite.RSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.RSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.DHE_RSA_EXPORT_WITH_DES40_CBC_SHA | TlsCipherSuite.DHE_RSA_WITH_DES_CBC_SHA | TlsCipherSuite.DHE_RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.DHE_RSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.DHE_RSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.RSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.RSA_WITH_AES_256_CBC_SHA256 | TlsCipherSuite.DHE_RSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.DHE_RSA_WITH_AES_256_CBC_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_CBC_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_RSA_WITH_RC4_128_SHA | TlsCipherSuite.RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.DHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_RSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256 | TlsCipherSuite.DHE_RSA_WITH_CHACHA20_POLY1305_SHA256)) == 0)
						{
							continue;
						}
						break;
					case KeyAlgorithm.ECDsa:
						if ((current & (TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_CBC_SHA384 | TlsCipherSuite.ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_RC4_128_SHA | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 | TlsCipherSuite.ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256)) == 0 || !clientHello.yiwjt(p) || 1 == 0)
						{
							continue;
						}
						break;
					case KeyAlgorithm.DSA:
						if ((current & (TlsCipherSuite.DHE_DSS_EXPORT_WITH_DES40_CBC_SHA | TlsCipherSuite.DHE_DSS_WITH_DES_CBC_SHA | TlsCipherSuite.DHE_DSS_WITH_3DES_EDE_CBC_SHA | TlsCipherSuite.DHE_DSS_WITH_AES_128_CBC_SHA | TlsCipherSuite.DHE_DSS_WITH_AES_256_CBC_SHA | TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_DES_CBC_SHA | TlsCipherSuite.DHE_DSS_EXPORT1024_WITH_RC4_56_SHA | TlsCipherSuite.DHE_DSS_WITH_RC4_128_SHA | TlsCipherSuite.DHE_DSS_WITH_AES_128_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_CBC_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_128_GCM_SHA256 | TlsCipherSuite.DHE_DSS_WITH_AES_256_GCM_SHA384)) == 0)
						{
							continue;
						}
						break;
					default:
						continue;
					}
				}
				vvowd = (int)p2;
				hcwtn = urofm2;
				break;
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		if (parameters.bimgq.HasValue && 0 == 0)
		{
			vvowd = (int)TlsCipher.plzju(parameters.bimgq.Value);
			hcwtn = null;
		}
		if (vvowd <= 0)
		{
			throw new TlsException(mjddr.jhrgr, "Client provided no supported cipher.");
		}
		long num2 = (long)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
		omhhj = new byte[32];
		omhhj[0] = (byte)((num2 >> 24) & 0xFF);
		omhhj[1] = (byte)((num2 >> 16) & 0xFF);
		omhhj[2] = (byte)((num2 >> 8) & 0xFF);
		omhhj[3] = (byte)(num2 & 0xFF);
		jtxhe.ubsib(omhhj, 4, 28);
		if (sessionId == null || 1 == 0)
		{
			hfjjl = new byte[0];
		}
		else
		{
			hfjjl = sessionId;
		}
		qoaim = 0;
		cipher = TlsCipher.lfexb((TlsCipherSuiteId)vvowd, ktguj);
		if (cipher == null || 1 == 0)
		{
			throw new TlsException(mjddr.jhrgr, "Unsupported cipher suite.");
		}
		if (tlsProtocol >= TlsProtocol.TLS10 && extensions != null && 0 == 0 && extensions.Length > 0)
		{
			uqsbq = extensions;
		}
	}
}
