using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Rebex;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class qfmgt
{
	public const string mwjut = "bcrypt";

	public const string jdokf = "none";

	public const string txjiq = "none";

	private readonly Dictionary<string, wmgeg> ezumm = new Dictionary<string, wmgeg>();

	public qfmgt()
	{
		ezumm.Add("ssh-dss", new niqud());
		ezumm.Add("ssh-rsa", new kiekr());
		ezumm.Add("ssh-ed25519", new ieqlp());
		ycsmb value = new ycsmb();
		ezumm.Add("ecdsa-sha2-nistp256", value);
		ezumm.Add("ecdsa-sha2-nistp384", value);
		ezumm.Add("ecdsa-sha2-nistp521", value);
	}

	public PrivateKeyInfo ttubb(byte[] p0, string p1)
	{
		wmbjj wmbjj2 = new wmbjj(p0);
		string text = wmbjj2.mmajl();
		if (text != "openssh-key-v1" && 0 == 0)
		{
			throw new CryptographicException("Unsupported OpenSSH key.");
		}
		string text2 = wmbjj2.dmqqk();
		bool flag = text2 == "aes256-ctr";
		SymmetricKeyAlgorithm symmetricKeyAlgorithm;
		if (!flag || 1 == 0)
		{
			symmetricKeyAlgorithm = SymmetricKeyAlgorithm.ciqvc(text2);
		}
		else
		{
			SymmetricKeyAlgorithm symmetricKeyAlgorithm2 = new SymmetricKeyAlgorithm(SymmetricKeyAlgorithmId.AES);
			symmetricKeyAlgorithm2.KeySize = 256;
			symmetricKeyAlgorithm2.Mode = CipherMode.ECB;
			symmetricKeyAlgorithm2.Padding = PaddingMode.None;
			symmetricKeyAlgorithm = symmetricKeyAlgorithm2;
		}
		SymmetricKeyAlgorithm symmetricKeyAlgorithm3 = symmetricKeyAlgorithm;
		try
		{
			string p2 = wmbjj2.dmqqk();
			byte[] p3 = wmbjj2.jcckr();
			Rebex.Security.Cryptography.DeriveBytes deriveBytes = rwngu(p1, p2, p3, symmetricKeyAlgorithm3);
			int num = wmbjj2.nnram();
			if (num != 1)
			{
				throw new CryptographicException("Only one key is supported.");
			}
			wmbjj2.mtame();
			byte[] array = wmbjj2.jcckr();
			byte[] array2 = deriveBytes?.GetBytes(symmetricKeyAlgorithm3.KeySize / 8);
			byte[] array3 = deriveBytes?.GetBytes(symmetricKeyAlgorithm3.BlockSize / 8);
			byte[] array4;
			if (flag && 0 == 0)
			{
				symmetricKeyAlgorithm3.SetKey(array2);
				zlqaj zlqaj2 = new zlqaj(symmetricKeyAlgorithm3.CreateEncryptor(), array3);
				int num2;
				try
				{
					array4 = new byte[array.Length];
					num2 = zlqaj2.TransformBlock(array, 0, array.Length, array4, 0);
				}
				finally
				{
					if (zlqaj2 != null && 0 == 0)
					{
						((IDisposable)zlqaj2).Dispose();
					}
				}
				if (num2 != array.Length)
				{
					throw new InvalidOperationException();
				}
			}
			else
			{
				array4 = PrivateKeyInfo.viqym(array, array2, array3, symmetricKeyAlgorithm3);
			}
			wmbjj p4 = new wmbjj(array4);
			return upsdf(p4);
		}
		finally
		{
			if (symmetricKeyAlgorithm3 != null && 0 == 0)
			{
				symmetricKeyAlgorithm3.Dispose();
			}
		}
	}

	public byte[] rsops(PrivateKeyInfo p0, string p1)
	{
		return sbete(p0, p1, "aes256-cbc");
	}

	public byte[] sbete(PrivateKeyInfo p0, string p1, string p2)
	{
		if (p2 == null || 1 == 0)
		{
			p2 = "none";
		}
		wmbjj wmbjj2 = new wmbjj();
		wmbjj2.qusct("openssh-key-v1");
		bool flag = p1 != null && 0 == 0 && p1 != "";
		if (flag && 0 == 0 && p2 == "none" && 0 == 0)
		{
			throw new ArgumentException("Symmetric algorithm must be specified when password is set.");
		}
		if (!ezumm.TryGetValue(p0.jvnzi, out var value) || 1 == 0)
		{
			throw new NotSupportedException("Parser " + p0.jvnzi + " is not supported.");
		}
		qbkfb qbkfb2 = value.ojgoe(p0);
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = ((flag ? true : false) ? SymmetricKeyAlgorithm.ciqvc(p2) : null);
		try
		{
			wmbjj2.vokoa((flag ? true : false) ? symmetricKeyAlgorithm.nwvkz : "none");
			wmbjj2.vokoa((flag ? true : false) ? "bcrypt" : "none");
			int p3 = symmetricKeyAlgorithm?.BlockSize ?? 8;
			byte[] p4 = zvilb(qbkfb2.myebl, p0.jvnzi, p3);
			trkbc(p4, symmetricKeyAlgorithm, p1, out var p5, out var p6);
			wmbjj2.qtrnf(p6);
			wmbjj2.hhmvg(1);
			wmbjj2.qtrnf(qbkfb2.cqyff);
			wmbjj2.qtrnf(p5);
		}
		finally
		{
			if (symmetricKeyAlgorithm != null && 0 == 0)
			{
				symmetricKeyAlgorithm.Dispose();
			}
		}
		byte[] inArray = wmbjj2.ihelo();
		string value2 = Convert.ToBase64String(inArray);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("-----BEGIN OPENSSH PRIVATE KEY-----\n");
		stringBuilder.Append(value2);
		stringBuilder.Append("\n-----END OPENSSH PRIVATE KEY-----\n");
		return EncodingTools.ASCII.GetBytes(stringBuilder.ToString());
	}

	private static feymk tytxb(string p0, byte[] p1, int p2, SymmetricKeyAlgorithm p3)
	{
		int num = p3.KeySize / 8;
		int num2 = p3.BlockSize / 8;
		string text = p0;
		if (text == null || 1 == 0)
		{
			text = string.Empty;
		}
		return new feymk(text, p1, p2, num + num2);
	}

	private static Rebex.Security.Cryptography.DeriveBytes rwngu(string p0, string p1, byte[] p2, SymmetricKeyAlgorithm p3)
	{
		string text;
		if ((text = p1) != null && 0 == 0)
		{
			if (text == "none")
			{
				return null;
			}
			if (text == "bcrypt")
			{
				wmbjj wmbjj2 = new wmbjj(p2);
				byte[] p4 = wmbjj2.jcckr();
				int p5 = wmbjj2.nnram();
				return tytxb(p0, p4, p5, p3);
			}
		}
		throw new NotSupportedException(brgjd.edcru("Key derivation function {0} is not supported.", p1));
	}

	private void trkbc(byte[] p0, SymmetricKeyAlgorithm p1, string p2, out byte[] p3, out byte[] p4)
	{
		if (p1 == null || 1 == 0)
		{
			p4 = new byte[0];
			p3 = p0;
			return;
		}
		byte[] randomBytes = CryptoHelper.GetRandomBytes(128);
		feymk feymk2 = tytxb(p2, randomBytes, 10, p1);
		wmbjj wmbjj2 = new wmbjj();
		wmbjj2.qtrnf(feymk2.nnkdm);
		wmbjj2.hhmvg(feymk2.saopy);
		p4 = wmbjj2.ihelo();
		byte[] bytes = feymk2.GetBytes(p1.KeySize / 8);
		byte[] bytes2 = feymk2.GetBytes(p1.BlockSize / 8);
		p3 = PrivateKeyInfo.cntzn(p0, bytes, bytes2, p1);
	}

	private PrivateKeyInfo upsdf(wmbjj p0)
	{
		int num = p0.nnram();
		int num2 = p0.nnram();
		if (num != num2)
		{
			throw new CryptographicException("Invalid password or bad data.");
		}
		string text = p0.dmqqk();
		if (!ezumm.TryGetValue(text, out var value) || 1 == 0)
		{
			throw new NotSupportedException("Parser " + text + " is not registered.");
		}
		return value.tmkiq(p0, text);
	}

	private byte[] zvilb(byte[] p0, string p1, int p2)
	{
		wmbjj wmbjj2 = new wmbjj();
		uint p3 = jlfbq.vtwgv(CryptoHelper.GetRandomBytes(4), 0);
		wmbjj2.bmhvq(p3);
		wmbjj2.bmhvq(p3);
		wmbjj2.vokoa(p1);
		wmbjj2.udtyl(p0);
		wmbjj2.qtrnf(new byte[0]);
		byte b = 0;
		if (b != 0)
		{
			goto IL_0041;
		}
		goto IL_004d;
		IL_0041:
		wmbjj2.ywmoe(++b);
		goto IL_004d;
		IL_004d:
		if (wmbjj2.tdjyr % p2 > 0)
		{
			goto IL_0041;
		}
		return wmbjj2.ihelo();
	}
}
