using System.Collections.Generic;
using System.Security.Cryptography;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal static class ikgnw
{
	public static byte[] xqfdb(byte[] p0, int p1)
	{
		wmbjj wmbjj2 = new wmbjj(p0);
		byte[] array = wmbjj2.sklfv();
		byte[] array2 = wmbjj2.sklfv();
		if (array.Length > p1 || array2.Length > p1)
		{
			throw new CryptographicException("Invalid ECDSA signature.");
		}
		p0 = new byte[p1 * 2];
		array.CopyTo(p0, p1 - array.Length);
		array2.CopyTo(p0, p1 * 2 - array2.Length);
		return p0;
	}

	public static byte[] ctjzt(byte[] p0, int p1)
	{
		if (p0.Length != p1 * 2)
		{
			throw new CryptographicException("Invalid ECDSA signature.");
		}
		wmbjj wmbjj2 = new wmbjj();
		wmbjj2.qjpch(p0, 0, p1);
		wmbjj2.qjpch(p0, p1, p1);
		return wmbjj2.ihelo();
	}

	public static AsymmetricKeyAlgorithm hyvbq(AsymmetricKeyAlgorithmId p0, string p1, zyppx p2)
	{
		string p3 = p2.mdsgo();
		byte[] key = p2.rypuc(p0: false);
		if (p1 != bpkgq.mjwcm(p3) && 0 == 0)
		{
			throw new CryptographicException("EC curve mismatch.");
		}
		AsymmetricKeyAlgorithm asymmetricKeyAlgorithm = new AsymmetricKeyAlgorithm();
		asymmetricKeyAlgorithm.ImportKey(p0, p1, key, AsymmetricKeyFormat.RawPublicKey);
		return asymmetricKeyAlgorithm;
	}

	public static string ovsfz(SshHostKeyAlgorithm p0)
	{
		return p0 switch
		{
			SshHostKeyAlgorithm.RSA => "ssh-rsa", 
			SshHostKeyAlgorithm.DSS => "ssh-dss", 
			SshHostKeyAlgorithm.ED25519 => "ssh-ed25519", 
			SshHostKeyAlgorithm.ECDsaNistP256 => "ecdsa-sha2-nistp256", 
			SshHostKeyAlgorithm.ECDsaNistP384 => "ecdsa-sha2-nistp384", 
			SshHostKeyAlgorithm.ECDsaNistP521 => "ecdsa-sha2-nistp521", 
			_ => null, 
		};
	}

	internal static void qzuiv(ckzrf p0, SshKeyExchangeAlgorithm p1, SshHostKeyAlgorithm p2, ovpxz p3, ovpxz p4, eswpb p5, eswpb p6)
	{
		string text;
		string text2;
		if (p1 == SshKeyExchangeAlgorithm.None || 1 == 0)
		{
			string[] supportedKeyExchangeAlgorithms = SshParameters.GetSupportedKeyExchangeAlgorithms();
			text = "key exchange";
			text2 = szdct(p0.fjrpn, supportedKeyExchangeAlgorithms);
		}
		else if (p2 == SshHostKeyAlgorithm.None || 1 == 0)
		{
			string[] supportedHostKeyAlgorithms = SshParameters.GetSupportedHostKeyAlgorithms();
			text = "host key";
			text2 = szdct(p0.kwdyx, supportedHostKeyAlgorithms);
		}
		else if (p3 == null || false || p4 == null)
		{
			string[] supportedEncryptionAlgorithms = SshParameters.GetSupportedEncryptionAlgorithms();
			text = "encryption";
			text2 = szdct(p0.wdccr, supportedEncryptionAlgorithms);
			if (string.IsNullOrEmpty(text2) && 0 == 0)
			{
				text2 = szdct(p0.wdccr, supportedEncryptionAlgorithms);
			}
		}
		else
		{
			if ((p5 != null || p3.fepmd) && (p6 != null || (p4.fepmd ? true : false)))
			{
				return;
			}
			string[] supportedMacAlgorithms = SshParameters.GetSupportedMacAlgorithms();
			text = "MAC";
			text2 = null;
			if ((!p3.fepmd || 1 == 0) && (p5 == null || 1 == 0))
			{
				text2 = szdct(p0.hfzts, supportedMacAlgorithms);
			}
			if ((!p4.fepmd || 1 == 0) && (p6 == null || 1 == 0) && string.IsNullOrEmpty(text2) && 0 == 0)
			{
				text2 = szdct(p0.ipvna, supportedMacAlgorithms);
			}
		}
		SshException ex = new SshException(tcpjq.ziezw, brgjd.edcru("The client and the server have no common {0} algorithm.", text));
		if (text2 != null && 0 == 0)
		{
			ex.izlra = "{0} " + text2;
		}
		throw ex;
	}

	private static string szdct(IList<string> p0, IList<string> p1)
	{
		string text = "";
		string text2 = null;
		string text3 = null;
		string text4 = null;
		IEnumerator<string> enumerator = p0.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				string current = enumerator.Current;
				if ((text2 == null || 1 == 0) && SshParameters.kwmql(current))
				{
					text2 = current;
				}
				else if ((text3 == null || 1 == 0) && p1.Contains(current) && 0 == 0)
				{
					text3 = current;
				}
				if ((text4 == null || 1 == 0) && SshParameters.wlxtp(current) && 0 == 0)
				{
					text4 = current;
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		if (text2 != null && 0 == 0)
		{
			text = "Server supports '" + text2 + "' which is weak and not enabled at the client.";
		}
		else if (text3 != null && 0 == 0)
		{
			text = "Server supports '" + text3 + "' which is not enabled at the client.";
		}
		if ((text3 == null || 1 == 0) && text4 != null && 0 == 0)
		{
			text += ((text == null) ? "Server" : " Server also");
			text = text + " supports '" + text4 + "' which needs a plugin on this platform. See https://www.rebex.net/kb/elliptic-curve-plugins/ for more information.";
		}
		return text;
	}
}
