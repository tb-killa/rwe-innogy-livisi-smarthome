using Rebex.Net;
using Rebex.Security.Certificates;
using Rebex.Security.Cryptography;

namespace onrkn;

internal abstract class ofuit : qoqui
{
	private nsvut bhzqi;

	public nsvut zijgx => bhzqi;

	public override void gjile(byte[] p0, int p1)
	{
		int num = nimwj - 4;
		p0[p1] = (byte)bhzqi;
		p0[p1 + 1] = (byte)((num >> 16) & 0xFF);
		p0[p1 + 2] = (byte)((num >> 8) & 0xFF);
		p0[p1 + 3] = (byte)(num & 0xFF);
	}

	public ofuit(nsvut type)
		: base(vcedo.ztfcr)
	{
		bhzqi = type;
	}

	public static ofuit paptc(byte[] p0, int p1, int p2, TlsProtocol p3, TlsCipher p4)
	{
		return (int)p0[p1] switch
		{
			0 => new tjpdc(p0, p1, p2), 
			1 => new aoind(p0, p1, p2), 
			2 => new kzdrw(p0, p1, p2), 
			11 => new eupdk(p0, p1, p2), 
			12 => new ccgaj(p0, p1, p2, p3, p4), 
			13 => new cujbv(p0, p1, p2, p3), 
			14 => new watdq(p0, p1, p2), 
			15 => new vssis(p0, p1, p2, p3), 
			16 => new ypafr(p0, p1, p2), 
			20 => new euwhd(p0, p1, p2), 
			_ => null, 
		};
	}

	protected static void rhqlp(byte[] p0, ref int p1, out SignatureHashAlgorithm? p2, out KeyAlgorithm? p3, out SignatureParameters p4)
	{
		p4 = null;
		switch ((kxbfg)p0[p1])
		{
		case kxbfg.xyyso:
			throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
		case kxbfg.ghcbk:
			p2 = SignatureHashAlgorithm.SHA1;
			break;
		case kxbfg.kudrn:
			p2 = SignatureHashAlgorithm.SHA256;
			break;
		case kxbfg.jnuky:
			p2 = SignatureHashAlgorithm.SHA384;
			break;
		case kxbfg.niato:
			p2 = SignatureHashAlgorithm.SHA512;
			break;
		default:
			throw new TlsException(mjddr.qssln, "Unsupported algorithm.");
		}
		switch ((qhkrk)p0[p1 + 1])
		{
		case qhkrk.fsthl:
			p3 = KeyAlgorithm.RSA;
			break;
		case qhkrk.ankum:
			p3 = KeyAlgorithm.DSA;
			break;
		case qhkrk.owbxa:
			p3 = KeyAlgorithm.ECDsa;
			break;
		default:
			throw new TlsException(mjddr.qssln, "Unsupported algorithm.");
		}
		p1 += 2;
	}

	protected static void chycn(SignatureHashAlgorithm p0, KeyAlgorithm p1, byte[] p2, ref int p3)
	{
		kxbfg kxbfg2;
		switch (p0)
		{
		case SignatureHashAlgorithm.MD5:
		case SignatureHashAlgorithm.MD4:
			throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
		case SignatureHashAlgorithm.SHA1:
			kxbfg2 = kxbfg.ghcbk;
			if (kxbfg2 != kxbfg.mmngh)
			{
				break;
			}
			goto case SignatureHashAlgorithm.SHA256;
		case SignatureHashAlgorithm.SHA256:
			kxbfg2 = kxbfg.kudrn;
			if (kxbfg2 != kxbfg.mmngh)
			{
				break;
			}
			goto case SignatureHashAlgorithm.SHA384;
		case SignatureHashAlgorithm.SHA384:
			kxbfg2 = kxbfg.jnuky;
			if (kxbfg2 != kxbfg.mmngh)
			{
				break;
			}
			goto case SignatureHashAlgorithm.SHA512;
		case SignatureHashAlgorithm.SHA512:
			kxbfg2 = kxbfg.niato;
			if (kxbfg2 != kxbfg.mmngh)
			{
				break;
			}
			goto default;
		default:
			throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
		}
		p2[p3] = (byte)kxbfg2;
		p3++;
		qhkrk qhkrk2;
		switch (p1)
		{
		case KeyAlgorithm.RSA:
			qhkrk2 = qhkrk.fsthl;
			if (qhkrk2 != qhkrk.ltthm)
			{
				break;
			}
			goto case KeyAlgorithm.DSA;
		case KeyAlgorithm.DSA:
			qhkrk2 = qhkrk.ankum;
			if (qhkrk2 != qhkrk.ltthm)
			{
				break;
			}
			goto case KeyAlgorithm.ECDsa;
		case KeyAlgorithm.ECDsa:
			qhkrk2 = qhkrk.owbxa;
			if (qhkrk2 != qhkrk.ltthm)
			{
				break;
			}
			goto default;
		default:
			throw new TlsException(mjddr.qssln, "Unexpected algorithm.");
		}
		p2[p3] = (byte)qhkrk2;
		p3++;
	}

	internal static SignatureHashAlgorithm uojyj(nxtme<byte> p0, KeyAlgorithm p1, SignatureHashAlgorithm p2)
	{
		if (p2 != SignatureHashAlgorithm.Unsupported && mhozp(p0, p1, p2) && 0 == 0)
		{
			return p2;
		}
		if (mhozp(p0, p1, SignatureHashAlgorithm.SHA256) && 0 == 0)
		{
			return SignatureHashAlgorithm.SHA256;
		}
		if (mhozp(p0, p1, SignatureHashAlgorithm.SHA384) && 0 == 0)
		{
			return SignatureHashAlgorithm.SHA384;
		}
		if (mhozp(p0, p1, SignatureHashAlgorithm.SHA512) && 0 == 0)
		{
			return SignatureHashAlgorithm.SHA512;
		}
		if (mhozp(p0, p1, SignatureHashAlgorithm.SHA1) && 0 == 0)
		{
			return SignatureHashAlgorithm.SHA1;
		}
		return SignatureHashAlgorithm.Unsupported;
	}

	internal static bool mhozp(nxtme<byte> p0, KeyAlgorithm p1, SignatureHashAlgorithm p2)
	{
		return kplxz(p0, p1, p2, SignaturePaddingScheme.Default);
	}

	internal static bool kplxz(nxtme<byte> p0, KeyAlgorithm p1, SignatureHashAlgorithm p2, SignaturePaddingScheme p3)
	{
		if (p0.hvbtp && 0 == 0)
		{
			return true;
		}
		if (p3 == SignaturePaddingScheme.Pss)
		{
			return false;
		}
		int num = 2;
		if (num == 0)
		{
			goto IL_0024;
		}
		goto IL_00a2;
		IL_0024:
		switch ((kxbfg)p0[num])
		{
		case kxbfg.ghcbk:
			if (p2 != SignatureHashAlgorithm.SHA1)
			{
				break;
			}
			goto IL_0064;
		case kxbfg.kudrn:
			if (p2 != SignatureHashAlgorithm.SHA256)
			{
				break;
			}
			goto IL_0064;
		case kxbfg.jnuky:
			if (p2 != SignatureHashAlgorithm.SHA384)
			{
				break;
			}
			goto IL_0064;
		case kxbfg.niato:
			{
				if (p2 != SignatureHashAlgorithm.SHA512)
				{
					break;
				}
				goto IL_0064;
			}
			IL_0064:
			switch ((qhkrk)p0[num + 1])
			{
			case qhkrk.fsthl:
				if (p1 != KeyAlgorithm.RSA && 0 == 0)
				{
					break;
				}
				goto IL_009c;
			case qhkrk.ankum:
				if (p1 != KeyAlgorithm.DSA)
				{
					break;
				}
				goto IL_009c;
			case qhkrk.owbxa:
				{
					if (p1 != KeyAlgorithm.ECDsa)
					{
						break;
					}
					goto IL_009c;
				}
				IL_009c:
				return true;
			}
			break;
		}
		num += 2;
		goto IL_00a2;
		IL_00a2:
		if (num < p0.tvoem)
		{
			goto IL_0024;
		}
		return false;
	}
}
