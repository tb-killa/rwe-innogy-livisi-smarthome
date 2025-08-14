using System;
using System.Collections.Generic;
using System.IO;

namespace onrkn;

internal class opjbe : xaxit, aviir<byte>, wqywh<byte>, ewsmx
{
	private sealed class vmxhz
	{
		public Stream pybqv;

		public void sesil(byte[] p0, int p1)
		{
			pybqv.Write(p0, 0, p1);
		}
	}

	private sealed class jamhw
	{
		public Action<ArraySegment<byte>> hhirp;

		public void wnnbq(byte[] p0, int p1)
		{
			hhirp(new ArraySegment<byte>(p0, 0, p1));
		}
	}

	public const int kzwyi = 1024;

	public const int aenid = 65536;

	private const int scprg = 1;

	private readonly List<byte[]> curek;

	private readonly int chjbg;

	private long giguv;

	private long nixlm;

	public override bool CanRead => true;

	public override bool CanSeek => true;

	public override bool CanWrite => true;

	public long hexxb => curek.Count * chjbg;

	public override long Position
	{
		get
		{
			return nixlm;
		}
		set
		{
			if (value < 0)
			{
				throw hifyx.nztrs("value", value, "Argument is out of range of valid values.");
			}
			nixlm = value;
		}
	}

	public override long Length => giguv;

	private int tdtds => (int)Length;

	byte aviir<byte>.this[int index] => oidsn(index);

	public opjbe()
		: this(65536)
	{
	}

	public opjbe(int chunkSize)
	{
		if (chunkSize <= 0 || ((chunkSize % 1024 != 0) ? true : false) || !visxc(chunkSize) || 1 == 0)
		{
			throw new ArgumentException("chunkSize");
		}
		chjbg = chunkSize;
		curek = new List<byte[]>();
		nixlm = 0L;
		giguv = 0L;
	}

	public override void Flush()
	{
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		switch (origin)
		{
		case SeekOrigin.Begin:
			Position = offset;
			break;
		case SeekOrigin.Current:
			Position += offset;
			break;
		case SeekOrigin.End:
			Position = Length + offset;
			break;
		}
		if (Position < 0)
		{
			throw hifyx.nztrs("offset", offset, "Argument is out of range of valid values.");
		}
		return Position;
	}

	public override void SetLength(long value)
	{
		if (value < 0)
		{
			throw hifyx.nztrs("value", value, "Argument is out of range of valid values.");
		}
		if (value == 0)
		{
			cmxic();
		}
		if (value > hexxb)
		{
			long num = value - hexxb;
			long p = num / chjbg + 1;
			qucrn(p);
		}
		if (value < hexxb)
		{
			long num2 = hexxb - value;
			long p2 = num2 / chjbg;
			fadbf(p2);
		}
		giguv = value;
		if (Position > Length - 1)
		{
			Position = ((Length == 0) ? 0 : (Length - 1));
		}
	}

	public virtual byte oidsn(long p0)
	{
		if (p0 < 0 || p0 > Length - 1)
		{
			throw hifyx.nztrs("index", p0, "Argument is out of range of valid values.");
		}
		byte[] array = adxoi(p0);
		int num = xsovi(p0);
		return array[num];
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (buffer == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0)
		{
			throw new ArgumentException("offset");
		}
		if (count < 0)
		{
			throw new ArgumentException("count");
		}
		if (!qitht(buffer, offset, count) || 1 == 0)
		{
			throw new ArgumentException("buffer");
		}
		int num = count;
		int num2 = offset;
		int num3 = 0;
		if (!equku() || false || nixlm >= giguv)
		{
			return num3;
		}
		byte[] array = rmgku();
		int num4 = qscxq();
		long num5 = Length - Position;
		while (array != null && 0 == 0 && num > 0 && num5 > 0)
		{
			int num6 = array.Length - num4;
			num6 = ((num5 > int.MaxValue) ? num6 : Math.Min(num6, Convert.ToInt32(num5)));
			int num7 = Math.Min(num, num6);
			Array.Copy(array, num4, buffer, num2, num7);
			num3 += num7;
			num -= num7;
			num2 += num7;
			nixlm += num7;
			num5 -= num7;
			num4 = 0;
			array = rmgku();
		}
		return num3;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (buffer == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0)
		{
			throw new ArgumentException("offset");
		}
		if (count < 0)
		{
			throw new ArgumentException("count");
		}
		if (Position > Length)
		{
			SetLength(Position + 1);
		}
		int num = offset;
		int num2 = count;
		int num3 = qscxq();
		while (num2 > 0)
		{
			byte[] array = umlga();
			int val = array.Length - num3;
			int num4 = Math.Min(num2, val);
			Array.Copy(buffer, num, array, num3, num4);
			num2 -= num4;
			num += num4;
			long num5 = nixlm + num4;
			if (giguv < num5)
			{
				giguv = num5;
			}
			nixlm = num5;
			num3 = 0;
		}
	}

	public void njguo(Stream p0)
	{
		vmxhz vmxhz = new vmxhz();
		vmxhz.pybqv = p0;
		if (vmxhz.pybqv == null || 1 == 0)
		{
			throw new ArgumentNullException("stream");
		}
		bbmih(vmxhz.sesil);
	}

	public virtual void bbmih(Action<byte[], int> p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("action");
		}
		dxyxz();
		long num = Length;
		long val = chjbg;
		using List<byte[]>.Enumerator enumerator = curek.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			byte[] current = enumerator.Current;
			int num2 = Convert.ToInt32(Math.Min(num, val));
			p0(current, num2);
			num -= num2;
		}
	}

	public void vmdbf(Action<ArraySegment<byte>> p0)
	{
		jamhw jamhw = new jamhw();
		jamhw.hhirp = p0;
		if (jamhw.hhirp == null || 1 == 0)
		{
			throw new ArgumentNullException("action");
		}
		bbmih(jamhw.wnnbq);
	}

	public virtual byte[] urpqw()
	{
		if (!equku() || 1 == 0)
		{
			return new byte[0];
		}
		dxyxz();
		long num = Length;
		byte[] array = new byte[num];
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0031;
		}
		goto IL_005e;
		IL_0031:
		int count = ((num > int.MaxValue) ? int.MaxValue : Convert.ToInt32(num));
		int num3 = Read(array, num2, count);
		num2 += num3;
		num -= num3;
		goto IL_005e;
		IL_005e:
		if (num > 0)
		{
			goto IL_0031;
		}
		return array;
	}

	public veasp uhxjo(long p0, long p1)
	{
		return new veasp(this, p0, p1, veasp.hnzas.hjipd);
	}

	public veasp obqdy(long p0, long p1)
	{
		return new veasp(this, p0, p1, veasp.hnzas.ructu);
	}

	private void dxyxz()
	{
		nixlm = 0L;
	}

	private void fadbf(long p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0027;
		IL_0006:
		curek.RemoveAt(curek.Count - 1);
		num++;
		goto IL_0027;
		IL_0027:
		if (num >= p0)
		{
			return;
		}
		goto IL_0006;
	}

	private void cmxic()
	{
		curek.Clear();
	}

	private void qucrn(long p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0025;
		IL_0006:
		curek.Add(new byte[chjbg]);
		num++;
		goto IL_0025;
		IL_0025:
		if (num >= p0)
		{
			return;
		}
		goto IL_0006;
	}

	private byte[] umlga()
	{
		byte[] array = rmgku();
		if (array != null && 0 == 0)
		{
			return array;
		}
		qucrn(1L);
		return curek[curek.Count - 1];
	}

	private bool visxc(int p0)
	{
		return (p0 & (p0 - 1)) == 0;
	}

	private int qscxq()
	{
		return xsovi(nixlm);
	}

	private int xsovi(long p0)
	{
		return Convert.ToInt32(p0 % chjbg);
	}

	private byte[] rmgku()
	{
		return adxoi(nixlm);
	}

	private byte[] adxoi(long p0)
	{
		if (!equku() || 1 == 0)
		{
			return null;
		}
		long num = p0 / chjbg;
		if (num >= curek.Count)
		{
			return null;
		}
		return curek[Convert.ToInt32(num)];
	}

	private bool equku()
	{
		return curek.Count > 0;
	}

	private bool qitht(byte[] p0, int p1, int p2)
	{
		return p0.Length >= p1 + p2;
	}
}
