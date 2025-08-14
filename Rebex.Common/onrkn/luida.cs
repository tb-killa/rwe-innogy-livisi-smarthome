using System;
using System.Runtime.InteropServices;

namespace onrkn;

internal static class luida
{
	internal struct vczsd
	{
		public bqhon mvfyf;

		public int ecguj;

		public ulong voglf;

		public ulong sjbog;

		public nxtme<byte> xwjae;

		public int gkkde;

		private byte[] qmmax;

		public nxtme<byte> ilxka => xwjae;

		public vczsd(bqhon hash, int outputHashSize, ulong t0, ulong t1)
		{
			mvfyf = hash;
			ecguj = outputHashSize;
			voglf = t0;
			sjbog = t1;
			qmmax = sxztb<byte>.ahblv.vfhlp(128);
			Array.Clear(qmmax, 0, qmmax.Length);
			xwjae = qmmax.pagxw().jlxhy(0, 128);
			gkkde = 0;
		}

		public void ksapj()
		{
			if (qmmax != null && 0 == 0)
			{
				Array.Clear(qmmax, 0, qmmax.Length);
				sxztb<byte>.ahblv.uqydw(qmmax);
				qmmax = null;
			}
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 64)]
	internal struct bqhon
	{
		[FieldOffset(0)]
		public byte qxupp;

		[FieldOffset(0)]
		internal tnypw sqsfv;

		[FieldOffset(0)]
		internal ncrnc qisht;

		[FieldOffset(0)]
		public ulong dxstg;

		[FieldOffset(8)]
		public ulong dkzve;

		[FieldOffset(16)]
		public ulong lyuqn;

		[FieldOffset(24)]
		public ulong cbumn;

		[FieldOffset(32)]
		public ulong ivtym;

		[FieldOffset(40)]
		public ulong zjgkv;

		[FieldOffset(48)]
		public ulong uiszp;

		[FieldOffset(56)]
		public ulong lvrob;

		public bqhon(ulong h0, ulong h1, ulong h2, ulong h3, ulong h4, ulong h5, ulong h6, ulong h7)
		{
			this = default(bqhon);
			qxupp = 0;
			dxstg = h0;
			dkzve = h1;
			lyuqn = h2;
			cbumn = h3;
			ivtym = h4;
			zjgkv = h5;
			uiszp = h6;
			lvrob = h7;
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 32)]
	internal struct tnypw
	{
		[FieldOffset(0)]
		public ulong tukbq;

		[FieldOffset(8)]
		public ulong tcenv;

		[FieldOffset(16)]
		public ulong iounw;

		[FieldOffset(24)]
		public ulong hjkno;
	}

	[StructLayout(LayoutKind.Explicit, Size = 48)]
	internal struct ncrnc
	{
		[FieldOffset(0)]
		public ulong ekuje;

		[FieldOffset(8)]
		public ulong tstpx;

		[FieldOffset(16)]
		public ulong axemj;

		[FieldOffset(24)]
		public ulong psusy;

		[FieldOffset(32)]
		public ulong xzegq;

		[FieldOffset(40)]
		public ulong ejksu;
	}

	public const int hepns = 64;

	public const int slnpp = 12;

	public const int tepgh = 128;

	public const int orqea = 127;

	public const int mhzur = 16;

	public const int txhai = 1;

	public const int ythuu = 64;

	public const int ydsvy = 0;

	public const int aygnt = 64;

	public const ulong gnrlc = 0uL;

	public const ulong yffgs = ulong.MaxValue;

	public const int ppolu = 32;

	public const int ddhvm = 24;

	public const int gswjz = 16;

	public const int pyevw = 63;

	public const ulong vkjdg = 7640891576956012808uL;

	public const ulong jhdur = 13503953896175478587uL;

	public const ulong tywec = 4354685564936845355uL;

	public const ulong lddaa = 11912009170470909681uL;

	public const ulong hgqxl = 5840696475078001361uL;

	public const ulong mzhlq = 11170449401992604703uL;

	public const ulong tazao = 2270897969802886507uL;

	public const ulong rojed = 6620516959819538809uL;

	public const int zpdjr = 0;

	public const int pxdil = 1;

	public const int gmnpx = 2;

	public const int azcyx = 3;

	public const int hhgii = 4;

	public const int kqlid = 5;

	public const int qbkxk = 6;

	public const int burtk = 7;

	public const int pkrut = 8;

	public const int dxwbl = 9;

	public const int vgueh = 10;

	public const int qobkn = 11;

	public const int fdvak = 12;

	public const int pypon = 13;

	public const int oxfme = 14;

	public const int vpstd = 15;

	public const int slxdz = 14;

	public const int ftvnm = 10;

	public const int rjxzi = 4;

	public const int xepki = 8;

	public const int kuuue = 9;

	public const int czudx = 15;

	public const int dtzqc = 13;

	public const int gqvan = 6;

	public const int cylfj = 1;

	public const int xobom = 12;

	public const int qeibq = 0;

	public const int mrjpx = 2;

	public const int jviuu = 11;

	public const int qyxpw = 7;

	public const int tjyox = 5;

	public const int whcmz = 3;

	public const int mxgkd = 11;

	public const int eselo = 8;

	public const int xmisr = 12;

	public const int hsvnb = 0;

	public const int gyrxv = 5;

	public const int rkqyw = 2;

	public const int kxkkn = 15;

	public const int hnicr = 13;

	public const int uovvc = 10;

	public const int yttwb = 14;

	public const int slzyc = 3;

	public const int yhgdp = 6;

	public const int olyvj = 7;

	public const int dykje = 1;

	public const int ynrdf = 9;

	public const int ujnqo = 4;

	public const int gzhum = 7;

	public const int jzopu = 9;

	public const int ampej = 3;

	public const int hfusk = 1;

	public const int bazcz = 13;

	public const int wdrqj = 12;

	public const int daeld = 11;

	public const int zzxka = 14;

	public const int xvkle = 2;

	public const int vyplc = 6;

	public const int dvwqc = 5;

	public const int xbifs = 10;

	public const int hanhn = 4;

	public const int jynus = 0;

	public const int gxoff = 15;

	public const int gsqoc = 8;

	public const int rribo = 9;

	public const int vwfmf = 0;

	public const int iswpi = 5;

	public const int oxblq = 7;

	public const int gngpm = 2;

	public const int smyrn = 4;

	public const int dhykn = 10;

	public const int fpyyj = 15;

	public const int sswtw = 14;

	public const int xhbdf = 1;

	public const int wxskv = 11;

	public const int fuwbt = 12;

	public const int ycqmm = 6;

	public const int znfoz = 8;

	public const int askxw = 3;

	public const int ajzie = 13;

	public const int ebizj = 2;

	public const int mojny = 12;

	public const int gusob = 6;

	public const int xnqgl = 10;

	public const int vitwq = 0;

	public const int bcsdn = 11;

	public const int hbleg = 8;

	public const int jelmv = 3;

	public const int uyjhp = 4;

	public const int lgzko = 13;

	public const int fvczm = 7;

	public const int swdvp = 5;

	public const int jjgob = 15;

	public const int chqxd = 14;

	public const int oungv = 1;

	public const int iqyzp = 9;

	public const int yzccm = 12;

	public const int yustk = 5;

	public const int tagpw = 1;

	public const int imnoh = 15;

	public const int jwdzi = 14;

	public const int gskls = 13;

	public const int dwmav = 4;

	public const int qbhfn = 10;

	public const int azhgh = 0;

	public const int peyca = 7;

	public const int xvcab = 6;

	public const int tpobb = 3;

	public const int mfqnr = 9;

	public const int ujjwa = 2;

	public const int gpazp = 8;

	public const int dvhtz = 11;

	public const int esjkv = 13;

	public const int pfdpz = 11;

	public const int dnckn = 7;

	public const int qjldp = 14;

	public const int qbqoa = 12;

	public const int brosd = 1;

	public const int bndca = 3;

	public const int sivwo = 9;

	public const int zwfpm = 5;

	public const int tnlri = 0;

	public const int qkrjc = 15;

	public const int jhwnf = 4;

	public const int xtqol = 8;

	public const int lbxfy = 6;

	public const int cyrtu = 2;

	public const int leeqw = 10;

	public const int jxndy = 6;

	public const int ewxqu = 15;

	public const int kvvvy = 14;

	public const int wpymd = 9;

	public const int xioer = 11;

	public const int btegj = 3;

	public const int xbtql = 0;

	public const int xuqlw = 8;

	public const int jojpg = 12;

	public const int srsyy = 2;

	public const int piesw = 13;

	public const int jsbir = 7;

	public const int iflfs = 1;

	public const int mrlwm = 4;

	public const int lqjbq = 10;

	public const int ermbn = 5;

	public const int atede = 10;

	public const int cvura = 2;

	public const int pgckm = 8;

	public const int fvhjz = 4;

	public const int poxnn = 7;

	public const int grlup = 6;

	public const int ndalt = 1;

	public const int ekkmy = 5;

	public const int gsprt = 15;

	public const int tjdhe = 11;

	public const int uejyd = 9;

	public const int iistx = 14;

	public const int pwqod = 3;

	public const int mctar = 12;

	public const int qvmzk = 13;

	public const int kwnbj = 0;

	public const int dmnag = 0;

	public const int rsaae = 1;

	public const int pdtyh = 2;

	public const int qutmq = 3;

	public const int odykt = 4;

	public const int oxxda = 5;

	public const int wlpqb = 6;

	public const int hkqwl = 7;

	public const int fphkj = 8;

	public const int gzsss = 9;

	public const int xonkf = 10;

	public const int aeljn = 11;

	public const int sxwqv = 12;

	public const int bamec = 13;

	public const int plogc = 14;

	public const int rqoqe = 15;

	public const int htvdl = 14;

	public const int ttnra = 10;

	public const int rzifv = 4;

	public const int yokaq = 8;

	public const int jrktt = 9;

	public const int jxrzc = 15;

	public const int znavi = 13;

	public const int raekw = 6;

	public const int rhdgw = 1;

	public const int bjffn = 12;

	public const int oyrwz = 0;

	public const int njgjv = 2;

	public const int qrxos = 11;

	public const int wuknv = 7;

	public const int bzzvf = 5;

	public const int tqjyg = 3;

	public const string vipmp = "Blake2b is not supported on big endian platforms.";

	private const int ubkro = 7;

	public static readonly byte[] nysqo;

	static luida()
	{
		nysqo = new byte[0];
		if (!BitConverter.IsLittleEndian || 1 == 0)
		{
			throw new NotSupportedException("Blake2b is not supported on big endian platforms.");
		}
	}

	public static void feopy(nxtme<byte> p0, nxtme<byte> p1, nxtme<byte> p2)
	{
		vczsd p3 = gylwc(p1, p2.hvvsm);
		try
		{
			toshr(ref p3, p0);
			mdjru(ref p3, p2);
		}
		finally
		{
			p3.ksapj();
		}
	}

	public static vczsd gylwc(nxtme<byte> p0, int p1)
	{
		if (p1 < 1 || p1 > 64)
		{
			throw new ArgumentOutOfRangeException("outputHashSizeInBytes");
		}
		if (p0.hvvsm < 0 || p0.hvvsm > 64)
		{
			throw new ArgumentOutOfRangeException("key");
		}
		bqhon hash = new bqhon(7640891576956012808uL, 13503953896175478587uL, 4354685564936845355uL, 11912009170470909681uL, 5840696475078001361uL, 11170449401992604703uL, 2270897969802886507uL, 6620516959819538809uL);
		hash.dxstg ^= (ulong)(0x1010000 ^ (p0.hvvsm << 8) ^ p1);
		vczsd result = new vczsd(hash, p1, 0uL, 0uL);
		if (!p0.hvbtp || 1 == 0)
		{
			p0.rjwrl(result.ilxka);
			result.gkkde = 128;
		}
		return result;
	}

	public static void toshr(ref vczsd p0, nxtme<byte> p1)
	{
		if (p1.hvvsm == 0 || 1 == 0)
		{
			return;
		}
		nxtme<byte> nxtme2 = p1;
		nxtme<byte> ilxka = p0.ilxka;
		if (p0.gkkde == 128)
		{
			p0.voglf += 128uL;
			if (p0.voglf < 128)
			{
				p0.sjbog++;
			}
			pekag(ilxka, ref p0, ref p0.mvfyf, 0uL);
			p0.gkkde = 0;
		}
		if (p0.gkkde != 0 && 0 == 0)
		{
			int num = 128 - p0.gkkde;
			nxtme<byte> p2 = ilxka.xjycg(p0.gkkde);
			if (nxtme2.hvvsm <= num)
			{
				nxtme2.rjwrl(p2);
				p0.gkkde += nxtme2.hvvsm;
				return;
			}
			nxtme2.jlxhy(0, num).rjwrl(p2);
			p0.voglf += 128uL;
			if (p0.voglf < 128)
			{
				p0.sjbog++;
			}
			pekag(ilxka, ref p0, ref p0.mvfyf, 0uL);
			nxtme2 = nxtme2.xjycg(num);
			p0.gkkde = 0;
		}
		int num2 = nxtme2.hvvsm >> 7;
		int num3 = nxtme2.hvvsm & 0x7F;
		if ((num3 == 0 || 1 == 0) && num2 > 0)
		{
			num3 = 128;
			num2--;
		}
		nxtme<byte> nxtme3 = nxtme2;
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_0167;
		}
		goto IL_01ca;
		IL_01ca:
		if (num4 >= num2)
		{
			if (num3 > 0)
			{
				nxtme2.xjycg(nxtme2.hvvsm - num3).rjwrl(ilxka);
				p0.gkkde += num3;
			}
			return;
		}
		goto IL_0167;
		IL_0167:
		nxtme<byte> p3 = nxtme3.jlxhy(0, 128);
		nxtme3 = nxtme3.xjycg(128);
		p0.voglf += 128uL;
		if (p0.voglf < 128)
		{
			p0.sjbog++;
		}
		pekag(p3, ref p0, ref p0.mvfyf, 0uL);
		num4++;
		goto IL_01ca;
	}

	public static void mdjru(ref vczsd p0, nxtme<byte> p1)
	{
		nxtme<byte> ilxka = p0.ilxka;
		ulong num = (ulong)p0.gkkde;
		p0.voglf += num;
		if (p0.voglf < num)
		{
			p0.sjbog++;
		}
		ilxka.xjycg(p0.gkkde).qkihq();
		pekag(ilxka, ref p0, ref p0.mvfyf, ulong.MaxValue);
		rnqdw rnqdw2 = default(rnqdw);
		byte[] array = null;
		try
		{
			switch (p1.hvvsm)
			{
			case 64:
				rnqdw2 = p1;
				Marshal.StructureToPtr((object)p0.mvfyf, rnqdw2.peara(), fDeleteOld: false);
				return;
			case 48:
				rnqdw2 = p1;
				Marshal.StructureToPtr((object)p0.mvfyf.qisht, rnqdw2.peara(), fDeleteOld: false);
				return;
			case 32:
				rnqdw2 = p1;
				Marshal.StructureToPtr((object)p0.mvfyf.sqsfv, rnqdw2.peara(), fDeleteOld: false);
				return;
			}
			array = sxztb<byte>.ahblv.vfhlp(64);
			nxtme<byte> nxtme2 = array.plhfl(0, 64);
			rnqdw2 = nxtme2;
			Marshal.StructureToPtr((object)p0.mvfyf, rnqdw2.peara(), fDeleteOld: false);
			nxtme2.jlxhy(0, p1.hvvsm).rjwrl(p1);
		}
		finally
		{
			rnqdw2.fbdzt();
			if (array != null && 0 == 0)
			{
				Array.Clear(array, 0, array.Length);
				sxztb<byte>.ahblv.uqydw(array);
			}
		}
	}

	private static void pekag(nxtme<byte> p0, ref vczsd p1, ref bqhon p2, ulong p3)
	{
		ayjol ayjol2 = peekn.nmjtk(p0.lthjd, p0.frlfs, p0.hvvsm, p3: true);
		try
		{
			imhbe(ayjol2.rjqep, ref p1, ref p2, p3);
		}
		finally
		{
			ayjol2.uqjka();
		}
	}

	private static void imhbe(nxtme<ulong> p0, ref vczsd p1, ref bqhon p2, ulong p3)
	{
		ulong voglf = p1.voglf;
		ulong sjbog = p1.sjbog;
		ulong dxstg = p2.dxstg;
		ulong dkzve = p2.dkzve;
		ulong lyuqn = p2.lyuqn;
		ulong cbumn = p2.cbumn;
		ulong ivtym = p2.ivtym;
		ulong zjgkv = p2.zjgkv;
		ulong uiszp = p2.uiszp;
		ulong lvrob = p2.lvrob;
		ulong num = 7640891576956012808uL;
		ulong num2 = 13503953896175478587uL;
		ulong num3 = 4354685564936845355uL;
		ulong num4 = 11912009170470909681uL;
		ulong num5 = 0x510E527FADE682D1L ^ voglf;
		ulong num6 = 0x9B05688C2B3E6C1FuL ^ sjbog;
		ulong num7 = 0x1F83D9ABFB41BD6BL ^ p3;
		ulong num8 = 6620516959819538809uL;
		dxstg += ivtym + p0[0];
		num5 ^= dxstg;
		num5 = (num5 >> 32) | (num5 << 32);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		dxstg += ivtym + p0[1];
		num5 ^= dxstg;
		num5 = (num5 >> 16) | (num5 << 48);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dkzve += zjgkv + p0[2];
		num6 ^= dkzve;
		num6 = (num6 >> 32) | (num6 << 32);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dkzve += zjgkv + p0[3];
		num6 ^= dkzve;
		num6 = (num6 >> 16) | (num6 << 48);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		lyuqn += uiszp + p0[4];
		num7 ^= lyuqn;
		num7 = (num7 >> 32) | (num7 << 32);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		lyuqn += uiszp + p0[5];
		num7 ^= lyuqn;
		num7 = (num7 >> 16) | (num7 << 48);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		cbumn += lvrob + p0[6];
		num8 ^= cbumn;
		num8 = (num8 >> 32) | (num8 << 32);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		cbumn += lvrob + p0[7];
		num8 ^= cbumn;
		num8 = (num8 >> 16) | (num8 << 48);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		dxstg += zjgkv + p0[8];
		num8 ^= dxstg;
		num8 = (num8 >> 32) | (num8 << 32);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dxstg += zjgkv + p0[9];
		num8 ^= dxstg;
		num8 = (num8 >> 16) | (num8 << 48);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		dkzve += uiszp + p0[10];
		num5 ^= dkzve;
		num5 = (num5 >> 32) | (num5 << 32);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		dkzve += uiszp + p0[11];
		num5 ^= dkzve;
		num5 = (num5 >> 16) | (num5 << 48);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		lyuqn += lvrob + p0[12];
		num6 ^= lyuqn;
		num6 = (num6 >> 32) | (num6 << 32);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		lyuqn += lvrob + p0[13];
		num6 ^= lyuqn;
		num6 = (num6 >> 16) | (num6 << 48);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		cbumn += ivtym + p0[14];
		num7 ^= cbumn;
		num7 = (num7 >> 32) | (num7 << 32);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		cbumn += ivtym + p0[15];
		num7 ^= cbumn;
		num7 = (num7 >> 16) | (num7 << 48);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dxstg += ivtym + p0[14];
		num5 ^= dxstg;
		num5 = (num5 >> 32) | (num5 << 32);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		dxstg += ivtym + p0[10];
		num5 ^= dxstg;
		num5 = (num5 >> 16) | (num5 << 48);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dkzve += zjgkv + p0[4];
		num6 ^= dkzve;
		num6 = (num6 >> 32) | (num6 << 32);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dkzve += zjgkv + p0[8];
		num6 ^= dkzve;
		num6 = (num6 >> 16) | (num6 << 48);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		lyuqn += uiszp + p0[9];
		num7 ^= lyuqn;
		num7 = (num7 >> 32) | (num7 << 32);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		lyuqn += uiszp + p0[15];
		num7 ^= lyuqn;
		num7 = (num7 >> 16) | (num7 << 48);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		cbumn += lvrob + p0[13];
		num8 ^= cbumn;
		num8 = (num8 >> 32) | (num8 << 32);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		cbumn += lvrob + p0[6];
		num8 ^= cbumn;
		num8 = (num8 >> 16) | (num8 << 48);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		dxstg += zjgkv + p0[1];
		num8 ^= dxstg;
		num8 = (num8 >> 32) | (num8 << 32);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dxstg += zjgkv + p0[12];
		num8 ^= dxstg;
		num8 = (num8 >> 16) | (num8 << 48);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		dkzve += uiszp + p0[0];
		num5 ^= dkzve;
		num5 = (num5 >> 32) | (num5 << 32);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		dkzve += uiszp + p0[2];
		num5 ^= dkzve;
		num5 = (num5 >> 16) | (num5 << 48);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		lyuqn += lvrob + p0[11];
		num6 ^= lyuqn;
		num6 = (num6 >> 32) | (num6 << 32);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		lyuqn += lvrob + p0[7];
		num6 ^= lyuqn;
		num6 = (num6 >> 16) | (num6 << 48);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		cbumn += ivtym + p0[5];
		num7 ^= cbumn;
		num7 = (num7 >> 32) | (num7 << 32);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		cbumn += ivtym + p0[3];
		num7 ^= cbumn;
		num7 = (num7 >> 16) | (num7 << 48);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dxstg += ivtym + p0[11];
		num5 ^= dxstg;
		num5 = (num5 >> 32) | (num5 << 32);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		dxstg += ivtym + p0[8];
		num5 ^= dxstg;
		num5 = (num5 >> 16) | (num5 << 48);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dkzve += zjgkv + p0[12];
		num6 ^= dkzve;
		num6 = (num6 >> 32) | (num6 << 32);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dkzve += zjgkv + p0[0];
		num6 ^= dkzve;
		num6 = (num6 >> 16) | (num6 << 48);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		lyuqn += uiszp + p0[5];
		num7 ^= lyuqn;
		num7 = (num7 >> 32) | (num7 << 32);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		lyuqn += uiszp + p0[2];
		num7 ^= lyuqn;
		num7 = (num7 >> 16) | (num7 << 48);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		cbumn += lvrob + p0[15];
		num8 ^= cbumn;
		num8 = (num8 >> 32) | (num8 << 32);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		cbumn += lvrob + p0[13];
		num8 ^= cbumn;
		num8 = (num8 >> 16) | (num8 << 48);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		dxstg += zjgkv + p0[10];
		num8 ^= dxstg;
		num8 = (num8 >> 32) | (num8 << 32);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dxstg += zjgkv + p0[14];
		num8 ^= dxstg;
		num8 = (num8 >> 16) | (num8 << 48);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		dkzve += uiszp + p0[3];
		num5 ^= dkzve;
		num5 = (num5 >> 32) | (num5 << 32);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		dkzve += uiszp + p0[6];
		num5 ^= dkzve;
		num5 = (num5 >> 16) | (num5 << 48);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		lyuqn += lvrob + p0[7];
		num6 ^= lyuqn;
		num6 = (num6 >> 32) | (num6 << 32);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		lyuqn += lvrob + p0[1];
		num6 ^= lyuqn;
		num6 = (num6 >> 16) | (num6 << 48);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		cbumn += ivtym + p0[9];
		num7 ^= cbumn;
		num7 = (num7 >> 32) | (num7 << 32);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		cbumn += ivtym + p0[4];
		num7 ^= cbumn;
		num7 = (num7 >> 16) | (num7 << 48);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dxstg += ivtym + p0[7];
		num5 ^= dxstg;
		num5 = (num5 >> 32) | (num5 << 32);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		dxstg += ivtym + p0[9];
		num5 ^= dxstg;
		num5 = (num5 >> 16) | (num5 << 48);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dkzve += zjgkv + p0[3];
		num6 ^= dkzve;
		num6 = (num6 >> 32) | (num6 << 32);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dkzve += zjgkv + p0[1];
		num6 ^= dkzve;
		num6 = (num6 >> 16) | (num6 << 48);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		lyuqn += uiszp + p0[13];
		num7 ^= lyuqn;
		num7 = (num7 >> 32) | (num7 << 32);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		lyuqn += uiszp + p0[12];
		num7 ^= lyuqn;
		num7 = (num7 >> 16) | (num7 << 48);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		cbumn += lvrob + p0[11];
		num8 ^= cbumn;
		num8 = (num8 >> 32) | (num8 << 32);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		cbumn += lvrob + p0[14];
		num8 ^= cbumn;
		num8 = (num8 >> 16) | (num8 << 48);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		dxstg += zjgkv + p0[2];
		num8 ^= dxstg;
		num8 = (num8 >> 32) | (num8 << 32);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dxstg += zjgkv + p0[6];
		num8 ^= dxstg;
		num8 = (num8 >> 16) | (num8 << 48);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		dkzve += uiszp + p0[5];
		num5 ^= dkzve;
		num5 = (num5 >> 32) | (num5 << 32);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		dkzve += uiszp + p0[10];
		num5 ^= dkzve;
		num5 = (num5 >> 16) | (num5 << 48);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		lyuqn += lvrob + p0[4];
		num6 ^= lyuqn;
		num6 = (num6 >> 32) | (num6 << 32);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		lyuqn += lvrob + p0[0];
		num6 ^= lyuqn;
		num6 = (num6 >> 16) | (num6 << 48);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		cbumn += ivtym + p0[15];
		num7 ^= cbumn;
		num7 = (num7 >> 32) | (num7 << 32);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		cbumn += ivtym + p0[8];
		num7 ^= cbumn;
		num7 = (num7 >> 16) | (num7 << 48);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dxstg += ivtym + p0[9];
		num5 ^= dxstg;
		num5 = (num5 >> 32) | (num5 << 32);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		dxstg += ivtym + p0[0];
		num5 ^= dxstg;
		num5 = (num5 >> 16) | (num5 << 48);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dkzve += zjgkv + p0[5];
		num6 ^= dkzve;
		num6 = (num6 >> 32) | (num6 << 32);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dkzve += zjgkv + p0[7];
		num6 ^= dkzve;
		num6 = (num6 >> 16) | (num6 << 48);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		lyuqn += uiszp + p0[2];
		num7 ^= lyuqn;
		num7 = (num7 >> 32) | (num7 << 32);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		lyuqn += uiszp + p0[4];
		num7 ^= lyuqn;
		num7 = (num7 >> 16) | (num7 << 48);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		cbumn += lvrob + p0[10];
		num8 ^= cbumn;
		num8 = (num8 >> 32) | (num8 << 32);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		cbumn += lvrob + p0[15];
		num8 ^= cbumn;
		num8 = (num8 >> 16) | (num8 << 48);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		dxstg += zjgkv + p0[14];
		num8 ^= dxstg;
		num8 = (num8 >> 32) | (num8 << 32);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dxstg += zjgkv + p0[1];
		num8 ^= dxstg;
		num8 = (num8 >> 16) | (num8 << 48);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		dkzve += uiszp + p0[11];
		num5 ^= dkzve;
		num5 = (num5 >> 32) | (num5 << 32);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		dkzve += uiszp + p0[12];
		num5 ^= dkzve;
		num5 = (num5 >> 16) | (num5 << 48);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		lyuqn += lvrob + p0[6];
		num6 ^= lyuqn;
		num6 = (num6 >> 32) | (num6 << 32);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		lyuqn += lvrob + p0[8];
		num6 ^= lyuqn;
		num6 = (num6 >> 16) | (num6 << 48);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		cbumn += ivtym + p0[3];
		num7 ^= cbumn;
		num7 = (num7 >> 32) | (num7 << 32);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		cbumn += ivtym + p0[13];
		num7 ^= cbumn;
		num7 = (num7 >> 16) | (num7 << 48);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dxstg += ivtym + p0[2];
		num5 ^= dxstg;
		num5 = (num5 >> 32) | (num5 << 32);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		dxstg += ivtym + p0[12];
		num5 ^= dxstg;
		num5 = (num5 >> 16) | (num5 << 48);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dkzve += zjgkv + p0[6];
		num6 ^= dkzve;
		num6 = (num6 >> 32) | (num6 << 32);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dkzve += zjgkv + p0[10];
		num6 ^= dkzve;
		num6 = (num6 >> 16) | (num6 << 48);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		lyuqn += uiszp + p0[0];
		num7 ^= lyuqn;
		num7 = (num7 >> 32) | (num7 << 32);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		lyuqn += uiszp + p0[11];
		num7 ^= lyuqn;
		num7 = (num7 >> 16) | (num7 << 48);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		cbumn += lvrob + p0[8];
		num8 ^= cbumn;
		num8 = (num8 >> 32) | (num8 << 32);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		cbumn += lvrob + p0[3];
		num8 ^= cbumn;
		num8 = (num8 >> 16) | (num8 << 48);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		dxstg += zjgkv + p0[4];
		num8 ^= dxstg;
		num8 = (num8 >> 32) | (num8 << 32);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dxstg += zjgkv + p0[13];
		num8 ^= dxstg;
		num8 = (num8 >> 16) | (num8 << 48);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		dkzve += uiszp + p0[7];
		num5 ^= dkzve;
		num5 = (num5 >> 32) | (num5 << 32);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		dkzve += uiszp + p0[5];
		num5 ^= dkzve;
		num5 = (num5 >> 16) | (num5 << 48);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		lyuqn += lvrob + p0[15];
		num6 ^= lyuqn;
		num6 = (num6 >> 32) | (num6 << 32);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		lyuqn += lvrob + p0[14];
		num6 ^= lyuqn;
		num6 = (num6 >> 16) | (num6 << 48);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		cbumn += ivtym + p0[1];
		num7 ^= cbumn;
		num7 = (num7 >> 32) | (num7 << 32);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		cbumn += ivtym + p0[9];
		num7 ^= cbumn;
		num7 = (num7 >> 16) | (num7 << 48);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dxstg += ivtym + p0[12];
		num5 ^= dxstg;
		num5 = (num5 >> 32) | (num5 << 32);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		dxstg += ivtym + p0[5];
		num5 ^= dxstg;
		num5 = (num5 >> 16) | (num5 << 48);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dkzve += zjgkv + p0[1];
		num6 ^= dkzve;
		num6 = (num6 >> 32) | (num6 << 32);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dkzve += zjgkv + p0[15];
		num6 ^= dkzve;
		num6 = (num6 >> 16) | (num6 << 48);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		lyuqn += uiszp + p0[14];
		num7 ^= lyuqn;
		num7 = (num7 >> 32) | (num7 << 32);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		lyuqn += uiszp + p0[13];
		num7 ^= lyuqn;
		num7 = (num7 >> 16) | (num7 << 48);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		cbumn += lvrob + p0[4];
		num8 ^= cbumn;
		num8 = (num8 >> 32) | (num8 << 32);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		cbumn += lvrob + p0[10];
		num8 ^= cbumn;
		num8 = (num8 >> 16) | (num8 << 48);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		dxstg += zjgkv + p0[0];
		num8 ^= dxstg;
		num8 = (num8 >> 32) | (num8 << 32);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dxstg += zjgkv + p0[7];
		num8 ^= dxstg;
		num8 = (num8 >> 16) | (num8 << 48);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		dkzve += uiszp + p0[6];
		num5 ^= dkzve;
		num5 = (num5 >> 32) | (num5 << 32);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		dkzve += uiszp + p0[3];
		num5 ^= dkzve;
		num5 = (num5 >> 16) | (num5 << 48);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		lyuqn += lvrob + p0[9];
		num6 ^= lyuqn;
		num6 = (num6 >> 32) | (num6 << 32);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		lyuqn += lvrob + p0[2];
		num6 ^= lyuqn;
		num6 = (num6 >> 16) | (num6 << 48);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		cbumn += ivtym + p0[8];
		num7 ^= cbumn;
		num7 = (num7 >> 32) | (num7 << 32);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		cbumn += ivtym + p0[11];
		num7 ^= cbumn;
		num7 = (num7 >> 16) | (num7 << 48);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dxstg += ivtym + p0[13];
		num5 ^= dxstg;
		num5 = (num5 >> 32) | (num5 << 32);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		dxstg += ivtym + p0[11];
		num5 ^= dxstg;
		num5 = (num5 >> 16) | (num5 << 48);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dkzve += zjgkv + p0[7];
		num6 ^= dkzve;
		num6 = (num6 >> 32) | (num6 << 32);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dkzve += zjgkv + p0[14];
		num6 ^= dkzve;
		num6 = (num6 >> 16) | (num6 << 48);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		lyuqn += uiszp + p0[12];
		num7 ^= lyuqn;
		num7 = (num7 >> 32) | (num7 << 32);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		lyuqn += uiszp + p0[1];
		num7 ^= lyuqn;
		num7 = (num7 >> 16) | (num7 << 48);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		cbumn += lvrob + p0[3];
		num8 ^= cbumn;
		num8 = (num8 >> 32) | (num8 << 32);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		cbumn += lvrob + p0[9];
		num8 ^= cbumn;
		num8 = (num8 >> 16) | (num8 << 48);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		dxstg += zjgkv + p0[5];
		num8 ^= dxstg;
		num8 = (num8 >> 32) | (num8 << 32);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dxstg += zjgkv + p0[0];
		num8 ^= dxstg;
		num8 = (num8 >> 16) | (num8 << 48);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		dkzve += uiszp + p0[15];
		num5 ^= dkzve;
		num5 = (num5 >> 32) | (num5 << 32);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		dkzve += uiszp + p0[4];
		num5 ^= dkzve;
		num5 = (num5 >> 16) | (num5 << 48);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		lyuqn += lvrob + p0[8];
		num6 ^= lyuqn;
		num6 = (num6 >> 32) | (num6 << 32);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		lyuqn += lvrob + p0[6];
		num6 ^= lyuqn;
		num6 = (num6 >> 16) | (num6 << 48);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		cbumn += ivtym + p0[2];
		num7 ^= cbumn;
		num7 = (num7 >> 32) | (num7 << 32);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		cbumn += ivtym + p0[10];
		num7 ^= cbumn;
		num7 = (num7 >> 16) | (num7 << 48);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dxstg += ivtym + p0[6];
		num5 ^= dxstg;
		num5 = (num5 >> 32) | (num5 << 32);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		dxstg += ivtym + p0[15];
		num5 ^= dxstg;
		num5 = (num5 >> 16) | (num5 << 48);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dkzve += zjgkv + p0[14];
		num6 ^= dkzve;
		num6 = (num6 >> 32) | (num6 << 32);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dkzve += zjgkv + p0[9];
		num6 ^= dkzve;
		num6 = (num6 >> 16) | (num6 << 48);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		lyuqn += uiszp + p0[11];
		num7 ^= lyuqn;
		num7 = (num7 >> 32) | (num7 << 32);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		lyuqn += uiszp + p0[3];
		num7 ^= lyuqn;
		num7 = (num7 >> 16) | (num7 << 48);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		cbumn += lvrob + p0[0];
		num8 ^= cbumn;
		num8 = (num8 >> 32) | (num8 << 32);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		cbumn += lvrob + p0[8];
		num8 ^= cbumn;
		num8 = (num8 >> 16) | (num8 << 48);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		dxstg += zjgkv + p0[12];
		num8 ^= dxstg;
		num8 = (num8 >> 32) | (num8 << 32);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dxstg += zjgkv + p0[2];
		num8 ^= dxstg;
		num8 = (num8 >> 16) | (num8 << 48);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		dkzve += uiszp + p0[13];
		num5 ^= dkzve;
		num5 = (num5 >> 32) | (num5 << 32);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		dkzve += uiszp + p0[7];
		num5 ^= dkzve;
		num5 = (num5 >> 16) | (num5 << 48);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		lyuqn += lvrob + p0[1];
		num6 ^= lyuqn;
		num6 = (num6 >> 32) | (num6 << 32);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		lyuqn += lvrob + p0[4];
		num6 ^= lyuqn;
		num6 = (num6 >> 16) | (num6 << 48);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		cbumn += ivtym + p0[10];
		num7 ^= cbumn;
		num7 = (num7 >> 32) | (num7 << 32);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		cbumn += ivtym + p0[5];
		num7 ^= cbumn;
		num7 = (num7 >> 16) | (num7 << 48);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dxstg += ivtym + p0[10];
		num5 ^= dxstg;
		num5 = (num5 >> 32) | (num5 << 32);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		dxstg += ivtym + p0[2];
		num5 ^= dxstg;
		num5 = (num5 >> 16) | (num5 << 48);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dkzve += zjgkv + p0[8];
		num6 ^= dkzve;
		num6 = (num6 >> 32) | (num6 << 32);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dkzve += zjgkv + p0[4];
		num6 ^= dkzve;
		num6 = (num6 >> 16) | (num6 << 48);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		lyuqn += uiszp + p0[7];
		num7 ^= lyuqn;
		num7 = (num7 >> 32) | (num7 << 32);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		lyuqn += uiszp + p0[6];
		num7 ^= lyuqn;
		num7 = (num7 >> 16) | (num7 << 48);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		cbumn += lvrob + p0[1];
		num8 ^= cbumn;
		num8 = (num8 >> 32) | (num8 << 32);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		cbumn += lvrob + p0[5];
		num8 ^= cbumn;
		num8 = (num8 >> 16) | (num8 << 48);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		dxstg += zjgkv + p0[15];
		num8 ^= dxstg;
		num8 = (num8 >> 32) | (num8 << 32);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dxstg += zjgkv + p0[11];
		num8 ^= dxstg;
		num8 = (num8 >> 16) | (num8 << 48);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		dkzve += uiszp + p0[9];
		num5 ^= dkzve;
		num5 = (num5 >> 32) | (num5 << 32);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		dkzve += uiszp + p0[14];
		num5 ^= dkzve;
		num5 = (num5 >> 16) | (num5 << 48);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		lyuqn += lvrob + p0[3];
		num6 ^= lyuqn;
		num6 = (num6 >> 32) | (num6 << 32);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		lyuqn += lvrob + p0[12];
		num6 ^= lyuqn;
		num6 = (num6 >> 16) | (num6 << 48);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		cbumn += ivtym + p0[13];
		num7 ^= cbumn;
		num7 = (num7 >> 32) | (num7 << 32);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		cbumn += ivtym + p0[0];
		num7 ^= cbumn;
		num7 = (num7 >> 16) | (num7 << 48);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dxstg += ivtym + p0[0];
		num5 ^= dxstg;
		num5 = (num5 >> 32) | (num5 << 32);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		dxstg += ivtym + p0[1];
		num5 ^= dxstg;
		num5 = (num5 >> 16) | (num5 << 48);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dkzve += zjgkv + p0[2];
		num6 ^= dkzve;
		num6 = (num6 >> 32) | (num6 << 32);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dkzve += zjgkv + p0[3];
		num6 ^= dkzve;
		num6 = (num6 >> 16) | (num6 << 48);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		lyuqn += uiszp + p0[4];
		num7 ^= lyuqn;
		num7 = (num7 >> 32) | (num7 << 32);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		lyuqn += uiszp + p0[5];
		num7 ^= lyuqn;
		num7 = (num7 >> 16) | (num7 << 48);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		cbumn += lvrob + p0[6];
		num8 ^= cbumn;
		num8 = (num8 >> 32) | (num8 << 32);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		cbumn += lvrob + p0[7];
		num8 ^= cbumn;
		num8 = (num8 >> 16) | (num8 << 48);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		dxstg += zjgkv + p0[8];
		num8 ^= dxstg;
		num8 = (num8 >> 32) | (num8 << 32);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dxstg += zjgkv + p0[9];
		num8 ^= dxstg;
		num8 = (num8 >> 16) | (num8 << 48);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		dkzve += uiszp + p0[10];
		num5 ^= dkzve;
		num5 = (num5 >> 32) | (num5 << 32);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		dkzve += uiszp + p0[11];
		num5 ^= dkzve;
		num5 = (num5 >> 16) | (num5 << 48);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		lyuqn += lvrob + p0[12];
		num6 ^= lyuqn;
		num6 = (num6 >> 32) | (num6 << 32);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		lyuqn += lvrob + p0[13];
		num6 ^= lyuqn;
		num6 = (num6 >> 16) | (num6 << 48);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		cbumn += ivtym + p0[14];
		num7 ^= cbumn;
		num7 = (num7 >> 32) | (num7 << 32);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		cbumn += ivtym + p0[15];
		num7 ^= cbumn;
		num7 = (num7 >> 16) | (num7 << 48);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dxstg += ivtym + p0[14];
		num5 ^= dxstg;
		num5 = (num5 >> 32) | (num5 << 32);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		dxstg += ivtym + p0[10];
		num5 ^= dxstg;
		num5 = (num5 >> 16) | (num5 << 48);
		num += num5;
		ivtym ^= num;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		dkzve += zjgkv + p0[4];
		num6 ^= dkzve;
		num6 = (num6 >> 32) | (num6 << 32);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dkzve += zjgkv + p0[8];
		num6 ^= dkzve;
		num6 = (num6 >> 16) | (num6 << 48);
		num2 += num6;
		zjgkv ^= num2;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		lyuqn += uiszp + p0[9];
		num7 ^= lyuqn;
		num7 = (num7 >> 32) | (num7 << 32);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		lyuqn += uiszp + p0[15];
		num7 ^= lyuqn;
		num7 = (num7 >> 16) | (num7 << 48);
		num3 += num7;
		uiszp ^= num3;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		cbumn += lvrob + p0[13];
		num8 ^= cbumn;
		num8 = (num8 >> 32) | (num8 << 32);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		cbumn += lvrob + p0[6];
		num8 ^= cbumn;
		num8 = (num8 >> 16) | (num8 << 48);
		num4 += num8;
		lvrob ^= num4;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		dxstg += zjgkv + p0[1];
		num8 ^= dxstg;
		num8 = (num8 >> 32) | (num8 << 32);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 24) | (zjgkv << 40);
		dxstg += zjgkv + p0[12];
		num8 ^= dxstg;
		num8 = (num8 >> 16) | (num8 << 48);
		num3 += num8;
		zjgkv ^= num3;
		zjgkv = (zjgkv >> 63) | (zjgkv << 1);
		dkzve += uiszp + p0[0];
		num5 ^= dkzve;
		num5 = (num5 >> 32) | (num5 << 32);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 24) | (uiszp << 40);
		dkzve += uiszp + p0[2];
		num5 ^= dkzve;
		num5 = (num5 >> 16) | (num5 << 48);
		num4 += num5;
		uiszp ^= num4;
		uiszp = (uiszp >> 63) | (uiszp << 1);
		lyuqn += lvrob + p0[11];
		num6 ^= lyuqn;
		num6 = (num6 >> 32) | (num6 << 32);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 24) | (lvrob << 40);
		lyuqn += lvrob + p0[7];
		num6 ^= lyuqn;
		num6 = (num6 >> 16) | (num6 << 48);
		num += num6;
		lvrob ^= num;
		lvrob = (lvrob >> 63) | (lvrob << 1);
		cbumn += ivtym + p0[5];
		num7 ^= cbumn;
		num7 = (num7 >> 32) | (num7 << 32);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 24) | (ivtym << 40);
		cbumn += ivtym + p0[3];
		num7 ^= cbumn;
		num7 = (num7 >> 16) | (num7 << 48);
		num2 += num7;
		ivtym ^= num2;
		ivtym = (ivtym >> 63) | (ivtym << 1);
		p2.dxstg ^= dxstg ^ num;
		p2.dkzve ^= dkzve ^ num2;
		p2.lyuqn ^= lyuqn ^ num3;
		p2.cbumn ^= cbumn ^ num4;
		p2.ivtym ^= ivtym ^ num5;
		p2.zjgkv ^= zjgkv ^ num6;
		p2.uiszp ^= uiszp ^ num7;
		p2.lvrob ^= lvrob ^ num8;
	}
}
