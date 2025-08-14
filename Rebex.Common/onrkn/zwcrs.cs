using System;
using System.Diagnostics;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;

namespace onrkn;

internal abstract class zwcrs : ktjcg, IDisposable
{
	protected enum cmzdt
	{
		uzvuc,
		bcrsg
	}

	public const int zudba = 16;

	public const int wvcxa = 32;

	public const int jurvv = 12;

	private const string vqlnw = "Encryption has already been finalized";

	private readonly cmzdt zvdkq;

	private readonly byte[] gifvg;

	private bvxhw fhrwg;

	private vifpo yotfm;

	private byte[] beodf;

	private long qhukn;

	private byte[] dbihr;

	private bool liqby;

	private nxtme<byte> dobuv;

	private readonly byte[] urnzw;

	private rnqdw qeovs;

	private bool ldpze;

	public bool drhcp => true;

	public bool ecsvv => true;

	public int InputBlockSize => 64;

	public int wqhtn => 64;

	protected zwcrs(byte[] key, cmzdt authMode)
	{
		if (key == null || 1 == 0)
		{
			throw new ArgumentNullException("key");
		}
		if (key.Length != 32)
		{
			throw new ArgumentException("Key must have 32 bytes.");
		}
		if (CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			throw new CryptographicException("Algorithm not supported in FIPS-compliant mode.");
		}
		zvdkq = authMode;
		gifvg = key;
		liqby = true;
		urnzw = sxztb<byte>.ahblv.vfhlp(32);
		dobuv = urnzw.pynmq(0, 32);
		qeovs = dobuv;
		ldpze = false;
	}

	public void Dispose()
	{
		rjybx(p0: true);
	}

	public byte[] ktakl()
	{
		return fhrwg.GetHash();
	}

	public void uqckg(nxtme<byte> p0)
	{
		fhrwg.zzsom(p0);
	}

	public void xinrl()
	{
		if (!liqby)
		{
			qhukn = 0L;
			liqby = true;
		}
	}

	public void seoke(byte[] p0)
	{
		beodf = p0;
		xinrl();
	}

	public void yirig(byte[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("nonce");
		}
		if (p0.Length != 12)
		{
			throw new ArgumentException("Nonce must have 12 bytes.", "nonce");
		}
		dbihr = p0;
		xinrl();
	}

	public byte[] zueba(byte[] p0, int p1, int p2)
	{
		byte[] array = new byte[p2];
		yajzn(p0, p1, p2, array, 0);
		return array;
	}

	public int yajzn(byte[] p0, int p1, int p2, byte[] p3, int p4)
	{
		int result = awhqv(p0, p1, p2, p3, p4);
		ulong p5 = (ulong)((beodf == null) ? 0 : beodf.Length);
		fhrwg.saubf(p5, (ulong)qhukn);
		gocdi();
		return result;
	}

	protected virtual void gocdi()
	{
	}

	protected virtual void rjybx(bool p0)
	{
		if (!ldpze && p0 && 0 == 0)
		{
			ldpze = true;
			if (gifvg != null && 0 == 0)
			{
				Array.Clear(gifvg, 0, gifvg.Length);
			}
			if (urnzw != null && 0 == 0)
			{
				qeovs.fbdzt();
				Array.Clear(urnzw, 0, urnzw.Length);
				sxztb<byte>.ahblv.uqydw(urnzw);
			}
			if (yotfm != null && 0 == 0)
			{
				yotfm.Dispose();
			}
			if (fhrwg != null && 0 == 0)
			{
				((IDisposable)fhrwg).Dispose();
			}
		}
	}

	protected void zlghn()
	{
		if (liqby ? true : false)
		{
			if (yotfm == null || 1 == 0)
			{
				yotfm = vifpo.nukst(gifvg, dbihr, 0);
			}
			else
			{
				yotfm.vpvaw(dbihr, 0);
			}
			nxtme<byte> p = dobuv;
			yotfm.iohsh(p, qeovs.peara());
			if (fhrwg == null || 1 == 0)
			{
				fhrwg = bvxhw.aztdt(p);
			}
			else
			{
				fhrwg.gaxag(p);
			}
			if (beodf != null && 0 == 0)
			{
				fhrwg.fimcw(beodf, 0, beodf.Length, p3: true);
			}
			liqby = false;
		}
	}

	[Conditional("DEBUG")]
	protected void gyfli()
	{
		if (ldpze && 0 == 0)
		{
			throw new ObjectDisposedException("AeadChacha20Poly1305TransformBase");
		}
	}

	private int awhqv(byte[] p0, int p1, int p2, byte[] p3, int p4)
	{
		zlghn();
		int result;
		switch (zvdkq)
		{
		case cmzdt.uzvuc:
			fhrwg.fimcw(p0, p1, p2, p3: true);
			result = yotfm.ivxhj(p0, p1, p2, p3, p4);
			break;
		case cmzdt.bcrsg:
			result = yotfm.ivxhj(p0, p1, p2, p3, p4);
			fhrwg.fimcw(p3, p4, p2, p3: true);
			break;
		default:
			throw new NotSupportedException();
		}
		checked
		{
			qhukn += p2;
			return result;
		}
	}
}
