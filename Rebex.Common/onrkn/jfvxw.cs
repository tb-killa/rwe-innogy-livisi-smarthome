using System;
using System.Security.Cryptography;
using Rebex.Security.Cryptography;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class jfvxw : eatps, lncnv, ijjlm
{
	private class ghmog
	{
		private bdjih tnjrz;

		private byte[] keroy;

		private shscm uuysl;

		private byte[] prrsu;

		public bdjih wwewy
		{
			get
			{
				return tnjrz;
			}
			private set
			{
				tnjrz = value;
			}
		}

		public byte[] slhyl
		{
			get
			{
				return keroy;
			}
			private set
			{
				keroy = value;
			}
		}

		public shscm eaevi
		{
			get
			{
				return uuysl;
			}
			private set
			{
				uuysl = value;
			}
		}

		public byte[] yoxtt
		{
			get
			{
				return prrsu;
			}
			private set
			{
				prrsu = value;
			}
		}

		public ghmog(bdjih s, byte[] h, shscm A, byte[] pubKey)
		{
			wwewy = s;
			slhyl = h;
			eaevi = A;
			yoxtt = pubKey;
		}
	}

	private class puqzg
	{
		private shscm wcqyg;

		public shscm nleoa
		{
			get
			{
				return wcqyg;
			}
			private set
			{
				wcqyg = value;
			}
		}

		public puqzg(shscm A)
		{
			nleoa = A;
		}
	}

	private class iwzjc
	{
		private byte[] odzbe;

		private byte[] iguma;

		public byte[] hwwen
		{
			get
			{
				return odzbe;
			}
			private set
			{
				odzbe = value;
			}
		}

		public byte[] zknll
		{
			get
			{
				return iguma;
			}
			private set
			{
				iguma = value;
			}
		}

		public iwzjc(byte[] secret)
		{
			hwwen = secret;
			zknll = zcvcz(secret, 0, 32);
		}
	}

	private struct shscm
	{
		public bdjih oyprt;

		public bdjih eazar;

		public bdjih jryyw;

		public bdjih msjlx;

		public static readonly shscm yzrwo = new shscm(0, 1, 1, 0);

		public shscm(bdjih x, bdjih y, bdjih z, bdjih t)
		{
			oyprt = x;
			eazar = y;
			jryyw = z;
			msjlx = t;
		}

		public override bool Equals(object obj)
		{
			shscm shscm = this;
			shscm shscm2 = (shscm)obj;
			if (jptcv(shscm.oyprt * shscm2.jryyw - shscm2.oyprt * shscm.jryyw) != 0 && 0 == 0)
			{
				return false;
			}
			if (jptcv(shscm.eazar * shscm2.jryyw - shscm2.eazar * shscm.jryyw) != 0 && 0 == 0)
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return sgrfh("X");
		}

		public string sgrfh(string p0)
		{
			return $"[\n  {oyprt.mympd(p0)},\n  {eazar.mympd(p0)},\n  {jryyw.mympd(p0)},\n  {msjlx.mympd(p0)}\n]";
		}
	}

	private class bqqll : imfrk
	{
		public eatps xaunu(PrivateKeyInfo p0)
		{
			return new jfvxw(p0.kfvak(), p0.acfcx());
		}

		public eatps neqkn(PublicKeyInfo p0)
		{
			return new jfvxw(null, p0.ToBytes());
		}

		public eatps poerm()
		{
			byte[] privateKey = CryptoHelper.aqljw(32);
			return new jfvxw(privateKey, null);
		}
	}

	private sealed class ggywk
	{
		public bool cecqu;

		public void bgteg(byte[] p0)
		{
			cecqu = (p0[31] & 0x80) == 128;
			p0[31] &= 127;
		}
	}

	private const int nsxqu = 256;

	private byte[] wkhth;

	private byte[] sozwo;

	private ghmog ozmzv;

	private puqzg ytzdj;

	private static readonly bdjih qczlv = bdjih.xbxmo.hkocc(255) - 19;

	private static readonly bdjih ixsqj = jptcv(-121665 * kuhpx(121666));

	private static readonly bdjih sdzim = bdjih.xbxmo.hkocc(252) + dmwgl.tfydq("27742317777372353535851937790883648493");

	private static readonly bdjih xsdvs = bdjih.xbxmo.hbhui((qczlv - 1) / 4, qczlv);

	private static readonly bdjih hhjkb = jptcv(4 * kuhpx(5));

	private static readonly bdjih qqrfs = eehmh(hhjkb, p1: false);

	private static readonly shscm aczcl = new shscm(qqrfs, hhjkb, 1, jptcv(qqrfs * hhjkb));

	private static Action<byte[]> llmrd;

	internal byte[] moqvd
	{
		get
		{
			if (sozwo == null || 1 == 0)
			{
				sozwo = akfqr();
				ytzdj = new puqzg(ozmzv.eaevi);
			}
			return sozwo;
		}
	}

	private string vbmym => "ed25519-sha512";

	private AsymmetricKeyAlgorithmId ifktr => AsymmetricKeyAlgorithmId.EdDsa;

	private int cpupu => 256;

	public jfvxw(byte[] privateKey, byte[] publicKey)
	{
		if ((privateKey == null || 1 == 0) && (publicKey == null || 1 == 0))
		{
			throw new ArgumentException("At least one key has to be specified.");
		}
		if (privateKey != null && 0 == 0 && privateKey.Length != 32)
		{
			throw new ArgumentException("Private key has invalid length.");
		}
		if (publicKey != null && 0 == 0 && publicKey.Length != 32)
		{
			throw new ArgumentException("Public key has invalid length.");
		}
		wkhth = privateKey;
		sozwo = publicKey;
	}

	private CspParameters dktgr()
	{
		return null;
	}

	CspParameters lncnv.iqqfj()
	{
		//ILSpy generated this explicit interface implementation from .override directive in dktgr
		return this.dktgr();
	}

	private PublicKeyInfo ywuzw()
	{
		return new PublicKeyInfo(AlgorithmIdentifier.xhnfa("1.3.101.112", "1.3.101.112"), moqvd);
	}

	PublicKeyInfo lncnv.kptoi()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ywuzw
		return this.ywuzw();
	}

	private PrivateKeyInfo vghyy(bool p0)
	{
		if (wkhth == null || 1 == 0)
		{
			throw new CryptographicException("Private key is not available.");
		}
		return new PrivateKeyInfo(AlgorithmIdentifier.xhnfa("1.3.101.112", "1.3.101.112"), jlfbq.usqov(wkhth, moqvd), moqvd, AsymmetricKeyAlgorithmId.EdDsa);
	}

	PrivateKeyInfo lncnv.jbbgs(bool p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in vghyy
		return this.vghyy(p0);
	}

	private byte[] wwsxd(byte[] p0, HashingAlgorithmId p1)
	{
		if (p1 != HashingAlgorithmId.SHA512)
		{
			throw new CryptographicException("Only SHA-512 is supported.");
		}
		gffhm();
		byte[] slhyl = ozmzv.slhyl;
		bdjih wwewy = ozmzv.wwewy;
		byte[] yoxtt = ozmzv.yoxtt;
		byte[] array = new byte[32 + p0.Length];
		Buffer.BlockCopy(slhyl, 32, array, 0, 32);
		Buffer.BlockCopy(p0, 0, array, 32, p0.Length);
		bdjih bdjih2 = dlltw(array);
		shscm p2 = tvmgk(bdjih2, aczcl);
		byte[] array2 = nungl(p2);
		array = new byte[array2.Length + yoxtt.Length + p0.Length];
		array2.CopyTo(array, 0);
		yoxtt.CopyTo(array, 32);
		Buffer.BlockCopy(p0, 0, array, 64, p0.Length);
		bdjih bdjih3 = dlltw(array);
		bdjih p3 = (bdjih2 + bdjih3 * wwewy).fvbmy(sdzim);
		byte[] array3 = new byte[64];
		array2.CopyTo(array3, 0);
		p3.csvpb(array3, 32);
		return array3;
	}

	byte[] ijjlm.vxuyd(byte[] p0, HashingAlgorithmId p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in wwsxd
		return this.wwsxd(p0, p1);
	}

	private bool guoyu(byte[] p0, HashingAlgorithmId p1, byte[] p2)
	{
		if (p1 != HashingAlgorithmId.SHA512)
		{
			throw new CryptographicException("Only SHA-512 is supported.");
		}
		if (p2.Length != 64)
		{
			return false;
		}
		byte[] src = moqvd;
		shscm p3 = vrlih();
		shscm p4;
		bool flag = jdwea(p2, 0, out p4);
		bdjih bdjih2 = xzkcz(p2, 32, 32);
		if (!flag || false || ((bdjih2 < 0) ? true : false) || bdjih2 >= sdzim)
		{
			return false;
		}
		byte[] array = new byte[64 + p0.Length];
		Buffer.BlockCopy(p2, 0, array, 0, 32);
		Buffer.BlockCopy(src, 0, array, 32, 32);
		Buffer.BlockCopy(p0, 0, array, 64, p0.Length);
		bdjih p5 = dlltw(array);
		shscm shscm = tvmgk(bdjih2, aczcl);
		shscm p6 = tvmgk(p5, p3);
		shscm shscm2 = fdgyd(p4, p6);
		return shscm.Equals(shscm2);
	}

	bool ijjlm.swsbt(byte[] p0, HashingAlgorithmId p1, byte[] p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in guoyu
		return this.guoyu(p0, p1, p2);
	}

	private shscm vrlih()
	{
		lgign();
		return ytzdj.nleoa;
	}

	private void lgign()
	{
		byte[] p = moqvd;
		if (ytzdj == null)
		{
			if (!jdwea(p, 0, out var p2))
			{
				throw new CryptographicException("Public key is invalid.");
			}
			ytzdj = new puqzg(p2);
			iksvm();
		}
	}

	private byte[] akfqr()
	{
		gffhm();
		return ozmzv.yoxtt;
	}

	private void gffhm()
	{
		if (ozmzv == null)
		{
			if (wkhth == null || 1 == 0)
			{
				throw new CryptographicException("Private key is required.");
			}
			iwzjc iwzjc = new iwzjc(wkhth);
			bdjih bdjih2 = arvti(iwzjc);
			shscm shscm = tvmgk(bdjih2, aczcl);
			byte[] pubKey = nungl(shscm);
			ozmzv = new ghmog(bdjih2, iwzjc.zknll, shscm, pubKey);
			iksvm();
		}
	}

	private void iksvm()
	{
		if (ozmzv == null || false || ytzdj == null || 1 == 0 || ytzdj.nleoa.Equals(ozmzv.eaevi))
		{
			return;
		}
		throw new CryptographicException("Public key does not correspond to the private key.");
	}

	private static bdjih jptcv(bdjih p0)
	{
		return p0.fvbmy(qczlv);
	}

	private static bdjih kuhpx(bdjih p0)
	{
		return p0.hbhui(qczlv - 2, qczlv);
	}

	private static byte[] bhhuw(byte[] p0)
	{
		return zcvcz(p0, 0, p0.Length);
	}

	private static byte[] zcvcz(byte[] p0, int p1, int p2)
	{
		return HashingAlgorithm.ComputeHash(HashingAlgorithmId.SHA512, p0, p1, p2);
	}

	private static bdjih hezqc(byte[] p0)
	{
		return uqvle(p0, 0, p0.Length, null);
	}

	private static bdjih xzkcz(byte[] p0, int p1, int p2)
	{
		return uqvle(p0, p1, p2, null);
	}

	private static bdjih uqvle(byte[] p0, int p1, int p2, Action<byte[]> p3)
	{
		byte[] array = p0;
		if (p3 != null && 0 == 0)
		{
			array = new byte[p2];
			Buffer.BlockCopy(p0, p1, array, 0, p2);
			p1 = 0;
			p3(array);
		}
		return bdjih.zlbhm(array, p1, p2);
	}

	private static bdjih dlltw(byte[] p0)
	{
		return hezqc(bhhuw(p0)).fvbmy(sdzim);
	}

	private static shscm fdgyd(shscm p0, shscm p1)
	{
		bdjih oyprt = p0.oyprt;
		bdjih eazar = p0.eazar;
		bdjih jryyw = p0.jryyw;
		bdjih msjlx = p0.msjlx;
		bdjih oyprt2 = p1.oyprt;
		bdjih eazar2 = p1.eazar;
		bdjih jryyw2 = p1.jryyw;
		bdjih msjlx2 = p1.msjlx;
		bdjih bdjih2 = jptcv((eazar - oyprt) * (eazar2 - oyprt2));
		bdjih bdjih3 = jptcv((eazar + oyprt) * (eazar2 + oyprt2));
		bdjih bdjih4 = jptcv(msjlx * 2 * ixsqj * msjlx2);
		bdjih bdjih5 = jptcv(jryyw * 2 * jryyw2);
		bdjih bdjih6 = bdjih3 - bdjih2;
		bdjih bdjih7 = bdjih5 - bdjih4;
		bdjih bdjih8 = bdjih5 + bdjih4;
		bdjih bdjih9 = bdjih3 + bdjih2;
		return new shscm(bdjih6 * bdjih7, bdjih8 * bdjih9, bdjih7 * bdjih8, bdjih6 * bdjih9);
	}

	private static shscm bysvr(shscm p0)
	{
		bdjih bdjih2 = jptcv(p0.oyprt * p0.oyprt);
		bdjih bdjih3 = jptcv(p0.eazar * p0.eazar);
		bdjih bdjih4 = jptcv(2 * p0.jryyw * p0.jryyw);
		bdjih bdjih5 = jptcv(p0.oyprt + p0.eazar);
		bdjih bdjih6 = bdjih2 + bdjih3;
		bdjih bdjih7 = bdjih6 - jptcv(bdjih5 * bdjih5);
		bdjih bdjih8 = bdjih2 - bdjih3;
		bdjih bdjih9 = bdjih4 + bdjih8;
		return new shscm(jptcv(bdjih7 * bdjih9), jptcv(bdjih8 * bdjih6), jptcv(bdjih9 * bdjih8), jptcv(bdjih7 * bdjih6));
	}

	private static shscm tvmgk(bdjih p0, shscm p1)
	{
		shscm shscm = shscm.yzrwo;
		while ((p0 > 0) ? true : false)
		{
			if (p0.jixdr() && 0 == 0)
			{
				shscm = fdgyd(shscm, p1);
			}
			p1 = bysvr(p1);
			p0 >>= 1;
		}
		return shscm;
	}

	private static bdjih eehmh(bdjih p0, bool p1)
	{
		if (!tpleg(p0, p1, out var p2) || 1 == 0)
		{
			throw new CryptographicException("Cannot decode X-coordinate.");
		}
		return p2;
	}

	private static bool tpleg(bdjih p0, bool p1, out bdjih p2)
	{
		p2 = 0;
		if (p0 >= qczlv && 0 == 0)
		{
			return false;
		}
		bdjih bdjih2 = (p0 * p0 - 1) * kuhpx(ixsqj * p0 * p0 + 1);
		if (bdjih2 == 0 && 0 == 0)
		{
			if (p1 && 0 == 0)
			{
				return false;
			}
			return true;
		}
		bdjih bdjih3 = bdjih2.hbhui((qczlv + 3) / 8, qczlv);
		if (jptcv(bdjih3 * bdjih3 - bdjih2) != 0 && 0 == 0)
		{
			bdjih3 = jptcv(bdjih3 * xsdvs);
		}
		if (jptcv(bdjih3 * bdjih3 - bdjih2) != 0 && 0 == 0)
		{
			return false;
		}
		if (bdjih3.jixdr() != p1)
		{
			bdjih3 = qczlv - bdjih3;
		}
		p2 = bdjih3;
		return true;
	}

	private static byte[] nungl(shscm p0)
	{
		bdjih bdjih2 = kuhpx(p0.jryyw);
		bdjih p1 = jptcv(p0.oyprt * bdjih2);
		bdjih p2 = jptcv(p0.eazar * bdjih2);
		byte[] array = new byte[32];
		p2.bdrka(array);
		if (p1.jixdr() && 0 == 0)
		{
			array[31] |= 128;
		}
		return array;
	}

	private static bool jdwea(byte[] p0, int p1, out shscm p2)
	{
		ggywk ggywk = new ggywk();
		if (p1 + 32 > p0.Length)
		{
			throw new CryptographicException("Invalid data.");
		}
		ggywk.cecqu = false;
		bdjih bdjih2 = uqvle(p0, p1, 32, ggywk.bgteg);
		if (tpleg(bdjih2, ggywk.cecqu, out var p3) && 0 == 0 && whzjc(p3, bdjih2) && 0 == 0)
		{
			p2 = new shscm(p3, bdjih2, 1, jptcv(p3 * bdjih2));
			return true;
		}
		p2 = shscm.yzrwo;
		return false;
	}

	private static bool whzjc(bdjih p0, bdjih p1)
	{
		bdjih bdjih2 = p0 * p0;
		bdjih bdjih3 = p1 * p1;
		return jptcv(bdjih3 - bdjih2 - 1 - ixsqj * bdjih2 * bdjih3) == 0;
	}

	private static bdjih arvti(iwzjc p0)
	{
		byte[] zknll = p0.zknll;
		if (llmrd == null || 1 == 0)
		{
			llmrd = lykzm;
		}
		return uqvle(zknll, 0, 32, llmrd);
	}

	internal static imfrk pcftj(string p0)
	{
		if (CryptoHelper.UseFipsAlgorithmsOnly && 0 == 0)
		{
			return null;
		}
		if (!bpkgq.guhie(p0, out var p1, out var p2) || 1 == 0)
		{
			return null;
		}
		if (p1 != AsymmetricKeyAlgorithmId.EdDsa || p2 != 256)
		{
			return null;
		}
		return new bqqll();
	}

	private static void lykzm(byte[] p0)
	{
		p0[31] &= 127;
		p0[31] |= 64;
		p0[0] &= 248;
	}
}
