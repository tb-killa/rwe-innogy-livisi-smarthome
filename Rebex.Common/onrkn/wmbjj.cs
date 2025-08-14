using System;
using System.Text;
using Rebex;

namespace onrkn;

internal class wmbjj : qqldc, IDisposable
{
	public const int ahlrf = 1048576;

	private const string fzarg = "Buffer is not available. Source view (ArrayView<byte>) is not contiguous memory. Consider using GetBufferView() method.";

	private const string zumjf = "Buffer is not available. Source view (ArrayView<byte>) does not start on offset 0. Consider using GetBufferView() method.";

	private Encoding bhcuv;

	private nxtme<byte> tduxh;

	private int qlijb;

	private int tdcwg;

	private readonly byte[] pnoyi = new byte[8];

	public Encoding hcojw
	{
		get
		{
			return bhcuv;
		}
		set
		{
			if (value == null || 1 == 0)
			{
				throw new ArgumentNullException("value");
			}
			bhcuv = value;
		}
	}

	public int tdjyr
	{
		get
		{
			return tdcwg;
		}
		set
		{
			if (value > tduxh.tvoem)
			{
				dosfs(value - qlijb);
			}
			tdcwg = value;
			if (qlijb > tdcwg)
			{
				qlijb = tdcwg;
			}
		}
	}

	public int hpxkw
	{
		get
		{
			return qlijb;
		}
		set
		{
			qlijb = value;
			if (qlijb > tdcwg)
			{
				tdcwg = qlijb;
			}
		}
	}

	public int ednpq => tduxh.tvoem;

	public byte this[int index] => tduxh[qlijb + index];

	public wmbjj()
		: this(new byte[4096].liutv())
	{
		tdcwg = 0;
	}

	public wmbjj(byte[] buffer)
		: this(buffer.liutv())
	{
	}

	public wmbjj(byte[] buffer, int length)
		: this(buffer.plhfl(0, length))
	{
	}

	public wmbjj(nxtme<byte> arrayView)
	{
		bhcuv = Encoding.UTF8;
		qlijb = 0;
		tdcwg = arrayView.tvoem;
		tduxh = arrayView;
	}

	public virtual void Dispose()
	{
	}

	public byte[] ihelo()
	{
		byte[] array = new byte[tdcwg];
		tduxh.jlxhy(0, tdcwg).rjwrl(array);
		return array;
	}

	public void chipy(int p0)
	{
		int num = tdcwg - p0;
		tduxh.xjycg(p0).rjwrl(tduxh);
		tdcwg = num;
		qlijb -= p0;
		if (qlijb < 0)
		{
			qlijb = 0;
		}
	}

	public void yubxo()
	{
		tdcwg = 0;
		qlijb = 0;
	}

	public byte[] jgiei()
	{
		if (!tduxh.bopab || 1 == 0)
		{
			throw new teymq("Buffer is not available. Source view (ArrayView<byte>) is not contiguous memory. Consider using GetBufferView() method.");
		}
		if (tduxh.frlfs != 0 && 0 == 0)
		{
			throw new teymq("Buffer is not available. Source view (ArrayView<byte>) does not start on offset 0. Consider using GetBufferView() method.");
		}
		return tduxh.lthjd;
	}

	public void xqsga(int p0)
	{
		int num = qlijb + p0;
		if (num > tdcwg)
		{
			throw new teymq("Not enough data available.");
		}
		qlijb = num;
	}

	public void mtame()
	{
		int num = nnram();
		if (num < 0 || num > 1048576)
		{
			throw new teymq("Invalid length.");
		}
		xqsga(num);
	}

	public byte fhuaz()
	{
		if (qlijb >= tdcwg)
		{
			throw new teymq("Not enough data available.");
		}
		byte result = tduxh[qlijb];
		qlijb++;
		return result;
	}

	public void xyozc(byte[] p0, int p1, int p2)
	{
		if ((p2 != 0) ? true : false)
		{
			int num = qlijb + p2;
			if (num > tdcwg)
			{
				throw new teymq("Not enough data available.");
			}
			tduxh.jlxhy(qlijb, p2).rjwrl(p0.plhfl(p1, p2));
			qlijb = num;
		}
	}

	public byte[] dliku(int p0)
	{
		byte[] array = new byte[p0];
		xyozc(array, 0, p0);
		return array;
	}

	public nxtme<byte> rrihx(int p0)
	{
		if (p0 == 0 || 1 == 0)
		{
			return nxtme<byte>.gihlo;
		}
		int num = qlijb + p0;
		if (num > tdcwg)
		{
			throw new teymq("Not enough data available.");
		}
		nxtme<byte> result = tduxh.jlxhy(qlijb, p0);
		qlijb = num;
		return result;
	}

	public byte[] dosyi()
	{
		return dliku(tdcwg - qlijb);
	}

	public void ppeor(int p0, Func<Exception> p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_003e;
		IL_0006:
		if (fhuaz() != 0 && 0 == 0)
		{
			object ex = p1();
			if (ex == null || 1 == 0)
			{
				ex = new teymq("Zero data expected.");
			}
			throw ex;
		}
		num++;
		goto IL_003e;
		IL_003e:
		if (num >= p0)
		{
			return;
		}
		goto IL_0006;
	}

	public void wjill(int p0)
	{
		if (tduxh.tvoem < p0)
		{
			byte[] array = new byte[(p0 + 4095) & -4096];
			tduxh.rjwrl(array);
			tduxh = array;
		}
	}

	private void dosfs(int p0)
	{
		wjill(qlijb + p0);
	}

	private void mspbz()
	{
		if (qlijb > tdcwg)
		{
			tdcwg = qlijb;
		}
	}

	public void ywmoe(byte p0)
	{
		dosfs(1);
		tduxh[qlijb++] = p0;
		mspbz();
	}

	public void jzheh(bool p0)
	{
		dosfs(1);
		tduxh[qlijb++] = (byte)((p0 ? true : false) ? 1 : 0);
		mspbz();
	}

	public void mmgwn(ushort p0)
	{
		dosfs(2);
		tduxh[qlijb++] = (byte)((p0 >> 8) & 0xFF);
		tduxh[qlijb++] = (byte)(p0 & 0xFF);
		mspbz();
	}

	public void bmhvq(uint p0)
	{
		dosfs(4);
		tduxh[qlijb++] = (byte)((p0 >> 24) & 0xFF);
		tduxh[qlijb++] = (byte)((p0 >> 16) & 0xFF);
		tduxh[qlijb++] = (byte)((p0 >> 8) & 0xFF);
		tduxh[qlijb++] = (byte)(p0 & 0xFF);
		mspbz();
	}

	public void qqtlt(uint p0)
	{
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			dosfs(4);
			tduxh[qlijb++] = (byte)(p0 & 0xFF);
			tduxh[qlijb++] = (byte)((p0 >> 8) & 0xFF);
			tduxh[qlijb++] = (byte)((p0 >> 16) & 0xFF);
			tduxh[qlijb++] = (byte)((p0 >> 24) & 0xFF);
			mspbz();
		}
		else
		{
			bmhvq(p0);
		}
	}

	public void vokej(ulong p0)
	{
		dosfs(8);
		tduxh[qlijb++] = (byte)((p0 >> 56) & 0xFF);
		tduxh[qlijb++] = (byte)((p0 >> 48) & 0xFF);
		tduxh[qlijb++] = (byte)((p0 >> 40) & 0xFF);
		tduxh[qlijb++] = (byte)((p0 >> 32) & 0xFF);
		tduxh[qlijb++] = (byte)((p0 >> 24) & 0xFF);
		tduxh[qlijb++] = (byte)((p0 >> 16) & 0xFF);
		tduxh[qlijb++] = (byte)((p0 >> 8) & 0xFF);
		tduxh[qlijb++] = (byte)(p0 & 0xFF);
		mspbz();
	}

	public void orhzf(byte[] p0, int p1, int p2)
	{
		dahxy.dionp(p0, p1, p2);
		dosfs(p2);
		p0.plhfl(p1, p2).rjwrl(tduxh.jlxhy(qlijb, p2));
		qlijb += p2;
		mspbz();
	}

	public void qusct(string p0)
	{
		udtyl(bhcuv.GetBytes(p0));
		ywmoe(0);
	}

	public void wddds(int p0)
	{
		dosfs(p0);
		int num = 0;
		if (num != 0)
		{
			goto IL_0009;
		}
		goto IL_002a;
		IL_0009:
		tduxh[qlijb++] = 0;
		num++;
		goto IL_002a;
		IL_002a:
		if (num < p0)
		{
			goto IL_0009;
		}
		mspbz();
	}

	public void khsfm(byte p0, int p1)
	{
		if (p1 <= 0)
		{
			throw new ArgumentOutOfRangeException("byteCount");
		}
		dosfs(p1);
		int num = 0;
		if (num != 0)
		{
			goto IL_001b;
		}
		goto IL_003c;
		IL_003c:
		if (num < p1)
		{
			goto IL_001b;
		}
		mspbz();
		return;
		IL_001b:
		tduxh[qlijb++] = p0;
		num++;
		goto IL_003c;
	}

	public void seieo(short p0)
	{
		mmgwn((ushort)p0);
	}

	public void hhmvg(int p0)
	{
		bmhvq((uint)p0);
	}

	public void kplbc(int p0)
	{
		qqtlt((uint)p0);
	}

	public void vtooz(long p0)
	{
		vokej((ulong)p0);
	}

	public void udtyl(byte[] p0)
	{
		orhzf(p0, 0, p0.Length);
	}

	public void donbi(byte[] p0, int p1, int p2)
	{
		hhmvg(p2);
		orhzf(p0, p1, p2);
	}

	public void qtrnf(byte[] p0)
	{
		hhmvg(p0.Length);
		orhzf(p0, 0, p0.Length);
	}

	public void vokoa(string p0)
	{
		byte[] bytes = bhcuv.GetBytes(p0);
		donbi(bytes, 0, bytes.Length);
	}

	public void ffsfu(string[] p0)
	{
		if (p0 == null || 1 == 0)
		{
			hhmvg(0);
		}
		else
		{
			vokoa(string.Join(",", p0));
		}
	}

	public void cwcwc(byte[] p0)
	{
		qjpch(p0, 0, p0.Length);
	}

	public void qjpch(byte[] p0, int p1, int p2)
	{
		while (p2 > 0 && p0[p1] == 0)
		{
			p1++;
			p2--;
		}
		if (p2 == 0 || false || p0[p1] >= 128)
		{
			hhmvg(p2 + 1);
			ywmoe(0);
		}
		else
		{
			hhmvg(p2);
		}
		orhzf(p0, p1, p2);
	}

	public void xyhzd(iergh p0)
	{
		p0.yazrq(this);
	}

	public bool nuehu()
	{
		return 0 != fhuaz();
	}

	public long femxa()
	{
		xyozc(pnoyi, 0, 8);
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			Array.Reverse(pnoyi, 0, 8);
		}
		return BitConverter.ToInt64(pnoyi, 0);
	}

	public ulong ufpij()
	{
		xyozc(pnoyi, 0, 8);
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			Array.Reverse(pnoyi, 0, 8);
		}
		return BitConverter.ToUInt64(pnoyi, 0);
	}

	public int nnram()
	{
		xyozc(pnoyi, 0, 4);
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			Array.Reverse(pnoyi, 0, 4);
		}
		return BitConverter.ToInt32(pnoyi, 0);
	}

	public int kopfa()
	{
		xyozc(pnoyi, 0, 4);
		return BitConverter.ToInt32(pnoyi, 0);
	}

	public uint jvrae()
	{
		xyozc(pnoyi, 0, 4);
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			Array.Reverse(pnoyi, 0, 4);
		}
		return BitConverter.ToUInt32(pnoyi, 0);
	}

	public uint jytwx()
	{
		xyozc(pnoyi, 0, 4);
		return BitConverter.ToUInt32(pnoyi, 0);
	}

	public short dycso()
	{
		xyozc(pnoyi, 0, 2);
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			Array.Reverse(pnoyi, 0, 2);
		}
		return BitConverter.ToInt16(pnoyi, 0);
	}

	public ushort mytfp()
	{
		xyozc(pnoyi, 0, 2);
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			Array.Reverse(pnoyi, 0, 2);
		}
		return BitConverter.ToUInt16(pnoyi, 0);
	}

	public byte[] jcckr()
	{
		int num = nnram();
		if (num < 0 || num > 1048576)
		{
			throw new teymq("Invalid length.");
		}
		byte[] array = new byte[num];
		xyozc(array, 0, num);
		return array;
	}

	public string mmajl()
	{
		int p;
		byte[] bytes = wsbwp(out p);
		int num = p;
		byte b;
		do
		{
			if (qlijb >= tdcwg)
			{
				throw new teymq("Not enough data available.");
			}
			b = tduxh[qlijb];
			qlijb++;
			num++;
		}
		while ((b != 0) ? true : false);
		return bhcuv.GetString(bytes, p, num - p - 1);
	}

	public string dmqqk()
	{
		return whjxw(p0: true);
	}

	private string whjxw(bool p0)
	{
		int num = nnram();
		if (num < 0 || num > 1048576)
		{
			throw new teymq("Invalid length");
		}
		int num2 = qlijb + num;
		if (num2 > tdcwg)
		{
			throw new teymq("Not enough data available.");
		}
		if (num == 0 || 1 == 0)
		{
			return "";
		}
		int p1;
		byte[] array = wsbwp(out p1);
		string result = ((!p0) ? bhcuv.GetString(array, p1, num) : EncodingTools.yyyrx(bhcuv, array, p1, num));
		qlijb = num2;
		return result;
	}

	private byte[] wsbwp(out int p0)
	{
		bool wzdji = tduxh.wzdji;
		byte[] result = ((wzdji ? true : false) ? tduxh.ooasp() : tduxh.lthjd);
		p0 = ((!wzdji || 1 == 0) ? (qlijb + tduxh.frlfs) : 0);
		return result;
	}

	public string xxkcx()
	{
		return whjxw(p0: false);
	}

	public nxtme<byte> uhfxr()
	{
		return tduxh;
	}

	public nxtme<byte> dduwu()
	{
		nxtme<byte> result = tduxh;
		tduxh = nxtme<byte>.gihlo;
		yubxo();
		return result;
	}

	public nxtme<byte> rtggk(nxtme<byte> p0)
	{
		nxtme<byte> result = tduxh;
		qlijb = 0;
		tdcwg = p0.tvoem;
		tduxh = p0;
		return result;
	}

	public string[] pgrem()
	{
		string text = dmqqk();
		return text.Split(',');
	}

	public byte[] sklfv()
	{
		int num = nnram();
		if (num < 0 || num > 4096)
		{
			throw new teymq("Invalid length.");
		}
		byte b = 0;
		do
		{
			if (num == 0 || 1 == 0)
			{
				return new byte[0];
			}
			b = fhuaz();
			num--;
		}
		while (b == 0);
		byte[] array = new byte[num + 1];
		array[0] = b;
		xyozc(array, 1, num);
		return array;
	}
}
