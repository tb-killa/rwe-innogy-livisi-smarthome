using System.Collections.Generic;
using System.Security.Cryptography;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class ovpxz
{
	private enum xryay
	{
		oagdc,
		yfkrd,
		ftytz,
		utvoh
	}

	internal const int iuadq = 16;

	internal const int vxmoo = 16;

	private SymmetricKeyAlgorithmId phphv;

	private xryay osebu;

	private CipherMode hluza;

	private string wdhwl;

	private SshEncryptionAlgorithm dxckh;

	private int uposa;

	private bool pyxao;

	private byte[] mwpzg;

	private byte[] lndbx;

	private int maqha;

	private int iqgop;

	private int ftswq;

	public string hzzec => wdhwl;

	public SshEncryptionAlgorithm ohfaq => dxckh;

	public int zgfun => uposa;

	public bool fepmd => osebu == xryay.utvoh;

	public int fkjgo
	{
		get
		{
			return iqgop;
		}
		private set
		{
			iqgop = value;
		}
	}

	public int ttokf
	{
		get
		{
			return ftswq;
		}
		private set
		{
			ftswq = value;
		}
	}

	public void mlcki(byte[] p0)
	{
		mwpzg = p0;
	}

	public void luhux(byte[] p0)
	{
		lndbx = p0;
	}

	internal static bool lsjck()
	{
		return wfcez.riopy(16, 16);
	}

	internal SshEncryptionMode ujdcx()
	{
		switch (osebu)
		{
		case xryay.utvoh:
			return SshEncryptionMode.GCM;
		case xryay.ftytz:
			return SshEncryptionMode.CTR;
		default:
			if (phphv == SymmetricKeyAlgorithmId.ArcFour)
			{
				return SshEncryptionMode.None;
			}
			return SshEncryptionMode.CBC;
		}
	}

	public ICryptoTransform xoait()
	{
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = new SymmetricKeyAlgorithm(phphv);
		symmetricKeyAlgorithm.Padding = PaddingMode.None;
		symmetricKeyAlgorithm.Mode = hluza;
		symmetricKeyAlgorithm.BlockSize = maqha * 8;
		symmetricKeyAlgorithm.SetKey(mwpzg);
		if (osebu == xryay.ftytz)
		{
			if (phphv != SymmetricKeyAlgorithmId.AES && 0 == 0)
			{
				return new wkxwh(symmetricKeyAlgorithm.CreateEncryptor(), lndbx);
			}
			return new zlqaj(symmetricKeyAlgorithm.CreateEncryptor(), lndbx);
		}
		if (symmetricKeyAlgorithm.Algorithm != SymmetricKeyAlgorithmId.ArcFour)
		{
			symmetricKeyAlgorithm.SetIV(lndbx);
		}
		ICryptoTransform cryptoTransform = ((!pyxao) ? symmetricKeyAlgorithm.CreateDecryptor() : symmetricKeyAlgorithm.CreateEncryptor());
		if (osebu == xryay.yfkrd)
		{
			byte[] array = new byte[1536];
			cryptoTransform.TransformBlock(array, 0, array.Length, array, 0);
		}
		return cryptoTransform;
	}

	public ovpxz(string alg, bool encryptor)
	{
		wdhwl = alg;
		pyxao = encryptor;
		string key;
		if ((key = alg) == null)
		{
			return;
		}
		if (awprl.ftjng == null || 1 == 0)
		{
			awprl.ftjng = new Dictionary<string, int>(23)
			{
				{ "3des-ctr", 0 },
				{ "3des-cbc", 1 },
				{ "aes256-ctr", 2 },
				{ "aes256-cbc", 3 },
				{ "aes192-ctr", 4 },
				{ "aes192-cbc", 5 },
				{ "aes128-ctr", 6 },
				{ "aes128-cbc", 7 },
				{ "arcfour", 8 },
				{ "arcfour128", 9 },
				{ "arcfour256", 10 },
				{ "blowfish-ctr", 11 },
				{ "blowfish-cbc", 12 },
				{ "twofish256-ctr", 13 },
				{ "twofish-cbc", 14 },
				{ "twofish256-cbc", 15 },
				{ "twofish192-ctr", 16 },
				{ "twofish192-cbc", 17 },
				{ "twofish128-ctr", 18 },
				{ "twofish128-cbc", 19 },
				{ "aes128-gcm@openssh.com", 20 },
				{ "aes256-gcm@openssh.com", 21 },
				{ "chacha20-poly1305@openssh.com", 22 }
			};
		}
		if (awprl.ftjng.TryGetValue(key, out var value) && 0 == 0)
		{
			switch (value)
			{
			default:
				return;
			case 0:
			case 1:
				uposa = 168;
				dxckh = SshEncryptionAlgorithm.TripleDES;
				phphv = SymmetricKeyAlgorithmId.TripleDES;
				fkjgo = 24;
				ttokf = (maqha = 8);
				break;
			case 2:
			case 3:
				uposa = 256;
				dxckh = SshEncryptionAlgorithm.AES;
				phphv = SymmetricKeyAlgorithmId.AES;
				fkjgo = 32;
				ttokf = (maqha = 16);
				break;
			case 4:
			case 5:
				uposa = 192;
				dxckh = SshEncryptionAlgorithm.AES;
				phphv = SymmetricKeyAlgorithmId.AES;
				fkjgo = 24;
				ttokf = (maqha = 16);
				break;
			case 6:
			case 7:
				uposa = 128;
				dxckh = SshEncryptionAlgorithm.AES;
				phphv = SymmetricKeyAlgorithmId.AES;
				fkjgo = 16;
				ttokf = (maqha = 16);
				break;
			case 8:
				uposa = 128;
				dxckh = SshEncryptionAlgorithm.RC4;
				phphv = SymmetricKeyAlgorithmId.ArcFour;
				fkjgo = 16;
				maqha = 1;
				break;
			case 9:
				uposa = 128;
				dxckh = SshEncryptionAlgorithm.RC4;
				phphv = SymmetricKeyAlgorithmId.ArcFour;
				osebu = xryay.yfkrd;
				fkjgo = 16;
				maqha = 1;
				break;
			case 10:
				uposa = 256;
				dxckh = SshEncryptionAlgorithm.RC4;
				phphv = SymmetricKeyAlgorithmId.ArcFour;
				osebu = xryay.yfkrd;
				fkjgo = 32;
				maqha = 1;
				break;
			case 11:
			case 12:
				uposa = 128;
				dxckh = SshEncryptionAlgorithm.Blowfish;
				phphv = SymmetricKeyAlgorithmId.Blowfish;
				fkjgo = 16;
				ttokf = (maqha = 8);
				break;
			case 13:
			case 14:
			case 15:
				uposa = 256;
				dxckh = SshEncryptionAlgorithm.Twofish;
				phphv = SymmetricKeyAlgorithmId.Twofish;
				fkjgo = 32;
				ttokf = (maqha = 16);
				break;
			case 16:
			case 17:
				uposa = 192;
				dxckh = SshEncryptionAlgorithm.Twofish;
				phphv = SymmetricKeyAlgorithmId.Twofish;
				fkjgo = 24;
				ttokf = (maqha = 16);
				break;
			case 18:
			case 19:
				uposa = 128;
				dxckh = SshEncryptionAlgorithm.Twofish;
				phphv = SymmetricKeyAlgorithmId.Twofish;
				fkjgo = 16;
				ttokf = (maqha = 16);
				break;
			case 20:
				uposa = 128;
				dxckh = SshEncryptionAlgorithm.AES;
				phphv = SymmetricKeyAlgorithmId.AES;
				fkjgo = 16;
				maqha = 16;
				ttokf = 12;
				osebu = xryay.utvoh;
				return;
			case 21:
				uposa = 256;
				dxckh = SshEncryptionAlgorithm.AES;
				phphv = SymmetricKeyAlgorithmId.AES;
				fkjgo = 32;
				maqha = 16;
				ttokf = 12;
				osebu = xryay.utvoh;
				return;
			case 22:
				uposa = 256;
				dxckh = SshEncryptionAlgorithm.Chacha20Poly1305;
				phphv = (SymmetricKeyAlgorithmId)(-1);
				fkjgo = 64;
				maqha = 1;
				ttokf = 0;
				osebu = xryay.utvoh;
				return;
			}
			if (alg.EndsWith("-ctr") && 0 == 0)
			{
				osebu = xryay.ftytz;
				hluza = CipherMode.ECB;
			}
			else if (phphv == SymmetricKeyAlgorithmId.ArcFour)
			{
				hluza = CipherMode.ECB;
			}
			else
			{
				hluza = CipherMode.CBC;
			}
		}
	}

	internal agxpx gnmax(agxpx p0, eswpb p1, bool p2)
	{
		if (osebu == xryay.utvoh)
		{
			if (phphv != SymmetricKeyAlgorithmId.AES && 0 == 0)
			{
				return new xwmdx(p0, mwpzg, p2);
			}
			gajry transform = wfcez.usflo(mwpzg, maqha, 16);
			return new xtazq(p0, transform, lndbx, p2);
		}
		if (p1.satyn && 0 == 0)
		{
			return new ufjug(p0, p1.ktkgi(), xoait(), p2);
		}
		return new fcyyj(p0, p1.ktkgi(), xoait(), hluza, p2);
	}

	internal agxpx dfcac(agxpx p0, eswpb p1, int? p2)
	{
		if (osebu == xryay.utvoh)
		{
			if (phphv != SymmetricKeyAlgorithmId.AES && 0 == 0)
			{
				return new rywyx(p0, mwpzg, p2);
			}
			fhryo transform = wfcez.stfbw(mwpzg, maqha, 16);
			return new vsueu(p0, transform, lndbx, p2);
		}
		if (p1.satyn && 0 == 0)
		{
			return new zssnc(p0, p1.ktkgi(), xoait(), p2);
		}
		return new tdsxy(p0, p1.ktkgi(), xoait(), hluza, p2);
	}
}
