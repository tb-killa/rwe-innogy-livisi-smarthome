using System;
using System.Security.Cryptography;

namespace onrkn;

internal abstract class riucd : IDisposable
{
	private const string klwmw = "GcmCore";

	public const int kvxia = 16;

	public const int drznx = 15;

	protected const byte hkmbs = 225;

	private static readonly byte[] wpmma = new byte[16];

	protected bool rcqub;

	protected bool yhlms;

	protected ICryptoTransform gfonr;

	protected byte[] fjqua;

	protected byte[] kavhj;

	protected byte[] aqrum;

	protected byte[] jfvpw;

	protected byte[] viltv;

	protected byte[] pgmif;

	protected byte[] jtste;

	protected byte[] jfjuo;

	protected byte[] uubgu;

	private static readonly byte[] jdylk = new byte[512]
	{
		0, 0, 1, 194, 3, 132, 2, 70, 7, 8,
		6, 202, 4, 140, 5, 78, 14, 16, 15, 210,
		13, 148, 12, 86, 9, 24, 8, 218, 10, 156,
		11, 94, 28, 32, 29, 226, 31, 164, 30, 102,
		27, 40, 26, 234, 24, 172, 25, 110, 18, 48,
		19, 242, 17, 180, 16, 118, 21, 56, 20, 250,
		22, 188, 23, 126, 56, 64, 57, 130, 59, 196,
		58, 6, 63, 72, 62, 138, 60, 204, 61, 14,
		54, 80, 55, 146, 53, 212, 52, 22, 49, 88,
		48, 154, 50, 220, 51, 30, 36, 96, 37, 162,
		39, 228, 38, 38, 35, 104, 34, 170, 32, 236,
		33, 46, 42, 112, 43, 178, 41, 244, 40, 54,
		45, 120, 44, 186, 46, 252, 47, 62, 112, 128,
		113, 66, 115, 4, 114, 198, 119, 136, 118, 74,
		116, 12, 117, 206, 126, 144, 127, 82, 125, 20,
		124, 214, 121, 152, 120, 90, 122, 28, 123, 222,
		108, 160, 109, 98, 111, 36, 110, 230, 107, 168,
		106, 106, 104, 44, 105, 238, 98, 176, 99, 114,
		97, 52, 96, 246, 101, 184, 100, 122, 102, 60,
		103, 254, 72, 192, 73, 2, 75, 68, 74, 134,
		79, 200, 78, 10, 76, 76, 77, 142, 70, 208,
		71, 18, 69, 84, 68, 150, 65, 216, 64, 26,
		66, 92, 67, 158, 84, 224, 85, 34, 87, 100,
		86, 166, 83, 232, 82, 42, 80, 108, 81, 174,
		90, 240, 91, 50, 89, 116, 88, 182, 93, 248,
		92, 58, 94, 124, 95, 190, 225, 0, 224, 194,
		226, 132, 227, 70, 230, 8, 231, 202, 229, 140,
		228, 78, 239, 16, 238, 210, 236, 148, 237, 86,
		232, 24, 233, 218, 235, 156, 234, 94, 253, 32,
		252, 226, 254, 164, 255, 102, 250, 40, 251, 234,
		249, 172, 248, 110, 243, 48, 242, 242, 240, 180,
		241, 118, 244, 56, 245, 250, 247, 188, 246, 126,
		217, 64, 216, 130, 218, 196, 219, 6, 222, 72,
		223, 138, 221, 204, 220, 14, 215, 80, 214, 146,
		212, 212, 213, 22, 208, 88, 209, 154, 211, 220,
		210, 30, 197, 96, 196, 162, 198, 228, 199, 38,
		194, 104, 195, 170, 193, 236, 192, 46, 203, 112,
		202, 178, 200, 244, 201, 54, 204, 120, 205, 186,
		207, 252, 206, 62, 145, 128, 144, 66, 146, 4,
		147, 198, 150, 136, 151, 74, 149, 12, 148, 206,
		159, 144, 158, 82, 156, 20, 157, 214, 152, 152,
		153, 90, 155, 28, 154, 222, 141, 160, 140, 98,
		142, 36, 143, 230, 138, 168, 139, 106, 137, 44,
		136, 238, 131, 176, 130, 114, 128, 52, 129, 246,
		132, 184, 133, 122, 135, 60, 134, 254, 169, 192,
		168, 2, 170, 68, 171, 134, 174, 200, 175, 10,
		173, 76, 172, 142, 167, 208, 166, 18, 164, 84,
		165, 150, 160, 216, 161, 26, 163, 92, 162, 158,
		181, 224, 180, 34, 182, 100, 183, 166, 178, 232,
		179, 42, 177, 108, 176, 174, 187, 240, 186, 50,
		184, 116, 185, 182, 188, 248, 189, 58, 191, 124,
		190, 190
	};

	private byte[] bdzcx;

	public bool gveuj => rcqub;

	public byte[] hnquu => pgmif;

	protected abstract void ctcwj(byte[] p0, byte[] p1);

	protected riucd(ICryptoTransform encryptor)
	{
		if (encryptor == null || 1 == 0)
		{
			throw new ArgumentNullException("encryptor");
		}
		gfonr = encryptor;
		jfvpw = null;
		fjqua = new byte[0];
		bdzcx = new byte[16];
		kavhj = new byte[16];
		aqrum = new byte[16];
		viltv = new byte[16];
		pgmif = new byte[16];
		jtste = new byte[16];
		jfjuo = new byte[16];
		uubgu = new byte[16];
		ssigs(wpmma, aqrum);
	}

	public void bjzbe(byte[] p0)
	{
		byita();
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("rgbIV");
		}
		if (p0.Length < 1)
		{
			throw new ArgumentException("IV must be at least one byte long.", "rgbIV");
		}
		if (rcqub && 0 == 0)
		{
			throw new InvalidOperationException("Cannot set IV in this state.");
		}
		jfvpw = p0;
	}

	public void pqwad(byte[] p0)
	{
		byita();
		byte[] array = p0;
		if (array == null || 1 == 0)
		{
			array = new byte[0];
		}
		fjqua = array;
	}

	public void llivr()
	{
		if (rcqub)
		{
			return;
		}
		if (jfvpw == null || 1 == 0)
		{
			throw new InvalidOperationException("IV is not specified.");
		}
		if (jfvpw.Length == 12)
		{
			Buffer.BlockCopy(jfvpw, 0, viltv, 0, jfvpw.Length);
			viltv[15] = 1;
		}
		else
		{
			int num = jfvpw.Length % 16;
			if (num != 0 && 0 == 0)
			{
				num = 16 - num;
			}
			byte[] array = new byte[jfvpw.Length + num + 16];
			Buffer.BlockCopy(jfvpw, 0, array, 0, jfvpw.Length);
			zzucl(jfvpw.Length * 8, array, array.Length - 8);
			qywod(array, viltv);
		}
		if (fjqua.Length > 0)
		{
			jjgvr(wpmma, fjqua, 0, fjqua.Length, pgmif);
		}
		zjnvs(viltv, kavhj);
		rcqub = true;
	}

	public void liazg()
	{
		Array.Clear(bdzcx, 0, 16);
		Array.Clear(kavhj, 0, 16);
		Array.Clear(viltv, 0, 16);
		Array.Clear(pgmif, 0, 16);
		Array.Clear(jtste, 0, 16);
		Array.Clear(jfjuo, 0, 16);
		Array.Clear(uubgu, 0, 16);
		rcqub = false;
	}

	protected void zzucl(int p0, byte[] p1, int p2)
	{
		p1[p2++] = 0;
		p1[p2++] = 0;
		p1[p2++] = 0;
		p1[p2++] = 0;
		p1[p2++] = (byte)(p0 >> 24);
		p1[p2++] = (byte)(p0 >> 16);
		p1[p2++] = (byte)(p0 >> 8);
		p1[p2++] = (byte)p0;
	}

	protected void hxjxg(long p0, byte[] p1, int p2)
	{
		p1[p2++] = (byte)(p0 >> 56);
		p1[p2++] = (byte)(p0 >> 48);
		p1[p2++] = (byte)(p0 >> 40);
		p1[p2++] = (byte)(p0 >> 32);
		p1[p2++] = (byte)(p0 >> 24);
		p1[p2++] = (byte)(p0 >> 16);
		p1[p2++] = (byte)(p0 >> 8);
		p1[p2++] = (byte)p0;
	}

	protected void ssigs(byte[] p0, byte[] p1)
	{
		gfonr.TransformBlock(p0, 0, 16, p1, 0);
	}

	protected bool cxagw(byte[] p0, byte[] p1)
	{
		bool flag = false;
		int num = 0;
		if (num != 0)
		{
			goto IL_0008;
		}
		goto IL_003b;
		IL_0008:
		byte b = p0[num];
		if (flag && 0 == 0)
		{
			p1[num] = (byte)((b >> 1) | 0x80);
		}
		else
		{
			p1[num] = (byte)(b >> 1);
		}
		flag = (b & 1) != 0;
		num++;
		goto IL_003b;
		IL_003b:
		if (num < p0.Length)
		{
			goto IL_0008;
		}
		return flag;
	}

	protected static bool ujnwo(byte[] p0, int p1, byte[] p2, int p3, int p4)
	{
		bool flag = false;
		if (flag)
		{
			goto IL_0006;
		}
		goto IL_0043;
		IL_0006:
		byte b = p0[p1];
		if (flag && 0 == 0)
		{
			p2[p3] = (byte)((b >> 1) | 0x80);
		}
		else
		{
			p2[p3] = (byte)(b >> 1);
		}
		flag = (b & 1) != 0;
		p1++;
		p3++;
		goto IL_0043;
		IL_0043:
		if (p4-- > 0)
		{
			goto IL_0006;
		}
		return flag;
	}

	protected static void whfsu(byte[] p0, int p1, byte[] p2, int p3, int p4)
	{
		byte b = 0;
		if (b != 0)
		{
			goto IL_0006;
		}
		goto IL_002a;
		IL_0006:
		byte b2 = p0[p1++];
		p2[p3++] = (byte)((b << 6) | (b2 >> 2));
		b = b2;
		goto IL_002a;
		IL_002a:
		if (p4-- <= 0)
		{
			return;
		}
		goto IL_0006;
	}

	protected static void vdcvd(byte[] p0, int p1, byte[] p2, int p3, int p4)
	{
		byte b = 0;
		if (b != 0)
		{
			goto IL_0006;
		}
		goto IL_002a;
		IL_0006:
		byte b2 = p0[p1++];
		p2[p3++] = (byte)((b << 4) | (b2 >> 4));
		b = b2;
		goto IL_002a;
		IL_002a:
		if (p4-- <= 0)
		{
			return;
		}
		goto IL_0006;
	}

	protected void droym(byte[] p0, byte[] p1, byte[] p2)
	{
		hqlwz(p0, 0, p1, 0, p2, 0);
	}

	protected void hqlwz(byte[] p0, int p1, byte[] p2, int p3, byte[] p4, int p5)
	{
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
		p4[p5++] = (byte)(p0[p1++] ^ p2[p3++]);
	}

	public void xrmgr(byte[] p0, byte[] p1, int p2, int p3, byte[] p4, long p5)
	{
		jjgvr(p0, p1, p2, p3, p4);
		zzucl(fjqua.Length * 8, bdzcx, 0);
		hxjxg(p5 * 8, bdzcx, 8);
		tlsva(pgmif, bdzcx, 0, 16, pgmif);
		xzqeg(viltv, pgmif, 0, pgmif, 0, 16, p6: false);
	}

	protected void jjgvr(byte[] p0, byte[] p1, int p2, int p3, byte[] p4)
	{
		int num = p3 % 16;
		int num2 = p3 - num;
		if (num2 > 0)
		{
			tlsva(p0, p1, p2, num2, p4);
			p2 += num2;
			p0 = p4;
		}
		if (num > 0)
		{
			Buffer.BlockCopy(p1, p2, bdzcx, 0, num);
			Array.Clear(bdzcx, num, 16 - num);
			tlsva(p0, bdzcx, 0, 16, p4);
		}
	}

	protected void qywod(byte[] p0, byte[] p1)
	{
		tlsva(wpmma, p0, 0, p0.Length, p1);
	}

	public void tlsva(byte[] p0, byte[] p1, int p2, int p3, byte[] p4)
	{
		if (p3 == 0 || 1 == 0)
		{
			Buffer.BlockCopy(p0, 0, p4, 0, 16);
			return;
		}
		int num = p3 >> 4;
		hqlwz(p0, 0, p1, p2, jfjuo, 0);
		int num2 = 1;
		if (num2 == 0)
		{
			goto IL_0039;
		}
		goto IL_006d;
		IL_006d:
		if (num2 < num)
		{
			goto IL_0039;
		}
		ctcwj(jfjuo, p4);
		return;
		IL_0039:
		p2 += 16;
		ctcwj(jfjuo, jfjuo);
		hqlwz(jfjuo, 0, p1, p2, jfjuo, 0);
		num2++;
		goto IL_006d;
	}

	protected static void nagjg(byte[] p0, int p1, byte[] p2, int p3)
	{
		if (ujnwo(p0, p1, p2, p3, 16) && 0 == 0)
		{
			p2[p3] ^= 225;
		}
	}

	protected static void etfhr(byte[] p0, int p1, byte[] p2, int p3)
	{
		int num = p0[p1 + 15] & 3;
		whfsu(p0, p1, p2, p3, 16);
		int num2 = num << 7;
		p2[p3] ^= jdylk[num2];
		p2[p3 + 1] ^= jdylk[num2 + 1];
	}

	protected static void xflkz(byte[] p0, int p1, byte[] p2, int p3)
	{
		int num = p0[p1 + 15] & 0xF;
		vdcvd(p0, p1, p2, p3, 16);
		int num2 = num << 5;
		p2[p3] ^= jdylk[num2];
		p2[p3 + 1] ^= jdylk[num2 + 1];
	}

	protected static void zgtcb(byte[] p0, int p1, byte[] p2, int p3)
	{
		int num = p0[p1 + 15];
		Buffer.BlockCopy(p0, p1, p2, p3 + 1, 15);
		int num2 = num << 1;
		p2[p3] = jdylk[num2];
		p2[p3 + 1] ^= jdylk[num2 + 1];
	}

	public void ktplf(byte[] p0, int p1, byte[] p2, int p3, int p4)
	{
		xzqeg(kavhj, p0, p1, p2, p3, p4, p6: true);
	}

	protected void xzqeg(byte[] p0, byte[] p1, int p2, byte[] p3, int p4, int p5, bool p6)
	{
		if (p5 > 16 && jlfbq.kdbec(p1, p2, p3, p4, p5) && 0 == 0)
		{
			eyvxf(p0, p1, p2, p3, p4, p5);
			return;
		}
		while (p5 > 0)
		{
			ssigs(p0, bdzcx);
			if (p5 >= 16)
			{
				if (p6 && 0 == 0)
				{
					boarv(p0);
				}
				jlfbq.cfvhy(p1, p2, bdzcx, 0, p3, p4, 16);
				if (p5 <= 16)
				{
					break;
				}
				p5 -= 16;
				p2 += 16;
				p4 += 16;
				continue;
			}
			jlfbq.cfvhy(p1, p2, bdzcx, 0, p3, p4, p5);
			break;
		}
	}

	private void eyvxf(byte[] p0, byte[] p1, int p2, byte[] p3, int p4, int p5)
	{
		int num = p5 / 16;
		int num2 = p5 % 16;
		if (num2 > 0)
		{
			num++;
		}
		else
		{
			num2 = 16;
		}
		hgpeb(p0, num - 1);
		p2 += 16 * (num - 1);
		p4 += 16 * (num - 1);
		int num3 = num;
		while (num3 > 0)
		{
			ssigs(p0, bdzcx);
			jlfbq.cfvhy(p1, p2, bdzcx, 0, p3, p4, num2);
			if (--num3 == 0)
			{
				break;
			}
			cylwp(p0);
			p2 -= 16;
			p4 -= 16;
			num2 = 16;
		}
		hgpeb(p0, num);
	}

	protected void hgpeb(byte[] p0, int p1)
	{
		if ((p1 != 0) ? true : false)
		{
			int num = p0[15] | (p0[14] << 8) | (p0[13] << 16) | (p0[12] << 24);
			int num2 = num + p1;
			p0[15] = (byte)num2;
			p0[14] = (byte)(num2 >> 8);
			p0[13] = (byte)(num2 >> 16);
			p0[12] = (byte)(num2 >> 24);
		}
	}

	protected void cylwp(byte[] p0)
	{
		int num = 15;
		if (num == 0)
		{
			goto IL_0007;
		}
		goto IL_0030;
		IL_0007:
		if (--p0[num] != byte.MaxValue)
		{
			return;
		}
		num--;
		goto IL_0030;
		IL_0030:
		if (num <= 11)
		{
			return;
		}
		goto IL_0007;
	}

	protected void boarv(byte[] p0)
	{
		int num = 15;
		if (num == 0)
		{
			goto IL_0007;
		}
		goto IL_0033;
		IL_0007:
		if (++p0[num] != 0 && 0 == 0)
		{
			return;
		}
		num--;
		goto IL_0033;
		IL_0033:
		if (num <= 11)
		{
			return;
		}
		goto IL_0007;
	}

	protected void zjnvs(byte[] p0, byte[] p1)
	{
		Buffer.BlockCopy(p0, 0, p1, 0, 16);
		int num = 15;
		if (num == 0)
		{
			goto IL_000e;
		}
		goto IL_0032;
		IL_000e:
		if ((p1[num] = (byte)(p0[num] + 1)) != 0 && 0 == 0)
		{
			return;
		}
		num--;
		goto IL_0032;
		IL_0032:
		if (num <= 11)
		{
			return;
		}
		goto IL_000e;
	}

	private void byita()
	{
		if (yhlms && 0 == 0)
		{
			throw new ObjectDisposedException("GcmCore");
		}
	}

	protected virtual void bevpu(bool p0)
	{
		if (p0 && 0 == 0 && !yhlms)
		{
			yhlms = true;
			if (rcqub && 0 == 0)
			{
				liazg();
			}
			Array.Clear(aqrum, 0, 16);
			gfonr.Dispose();
			gfonr = null;
			bdzcx = null;
			fjqua = null;
			kavhj = null;
			aqrum = null;
			jfvpw = null;
			viltv = null;
			pgmif = null;
			jtste = null;
			jfjuo = null;
			uubgu = null;
		}
	}

	public void Dispose()
	{
		bevpu(p0: true);
		GC.SuppressFinalize(this);
	}
}
