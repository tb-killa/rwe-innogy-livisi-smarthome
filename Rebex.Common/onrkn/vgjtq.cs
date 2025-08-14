using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace onrkn;

internal static class vgjtq
{
	private sealed class twwcp : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private string hbjwe;

		private int ynidb;

		private int omtrn;

		public string qlrdi;

		public string iwgqk;

		public int ppwyi;

		public int hjzus;

		public int fshdb;

		public int vcvpw;

		public int oxnxj;

		public int kzirn;

		private string vkvaj => hbjwe;

		private object ronkm => hbjwe;

		private IEnumerator<string> xigcu()
		{
			twwcp twwcp;
			if (Thread.CurrentThread.ManagedThreadId == omtrn && ynidb == -2)
			{
				ynidb = 0;
				twwcp = this;
			}
			else
			{
				twwcp = new twwcp(0);
			}
			twwcp.qlrdi = iwgqk;
			twwcp.ppwyi = hjzus;
			twwcp.fshdb = vcvpw;
			return twwcp;
		}

		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in xigcu
			return this.xigcu();
		}

		private IEnumerator nnyvy()
		{
			return xigcu();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in nnyvy
			return this.nnyvy();
		}

		private bool qfzol()
		{
			switch (ynidb)
			{
			case 0:
				ynidb = -1;
				oxnxj = qlrdi.Length - ppwyi;
				goto IL_0083;
			case 1:
				ynidb = -1;
				oxnxj -= ppwyi;
				goto IL_0083;
			case 2:
				{
					ynidb = -1;
					break;
				}
				IL_0083:
				if (oxnxj >= fshdb)
				{
					hbjwe = qlrdi.Substring(oxnxj, ppwyi);
					ynidb = 1;
					return true;
				}
				kzirn = (qlrdi.Length - fshdb) % ppwyi;
				if (kzirn > 0)
				{
					hbjwe = qlrdi.Substring(fshdb, kzirn);
					ynidb = 2;
					return true;
				}
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in qfzol
			return this.qfzol();
		}

		private void djelh()
		{
			throw new NotSupportedException();
		}

		void IEnumerator.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in djelh
			this.djelh();
		}

		private void ojifa()
		{
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ojifa
			this.ojifa();
		}

		public twwcp(int _003C_003E1__state)
		{
			ynidb = _003C_003E1__state;
			omtrn = Thread.CurrentThread.ManagedThreadId;
		}
	}

	private static readonly uint[] xvkbr = new uint[0];

	public static void bgybj(uint[] p0, int p1, nxtme<byte> p2)
	{
		if (p2.tvoem % 4 > 0)
		{
			throw new NotSupportedException("Output count must be divisible by 4");
		}
		if (p2.tvoem / 4 > p0.Length - p1)
		{
			throw new ArgumentOutOfRangeException("input", "Insufficient arr length");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_003c;
		}
		goto IL_0062;
		IL_0062:
		if (num >= p2.tvoem / 4)
		{
			return;
		}
		goto IL_003c;
		IL_003c:
		jlfbq.mfljd(p2.lthjd, p2.frlfs + num * 4, p0[p1 + num]);
		num++;
		goto IL_0062;
	}

	public static uint[] krjyb(string p0, int? p1 = null)
	{
		bool flag = p0.StartsWith("0x");
		int val = (p0.Length + 7 - ((flag ? true : false) ? 2 : 0)) / 8;
		int num = Math.Max(p1.GetValueOrDefault(), val);
		uint[] array = new uint[num];
		evxoq(p0, array);
		return array;
	}

	public static void evxoq(string p0, uint[] p1)
	{
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		bool flag = p0.StartsWith("0x");
		int num = (p0.Length + 7 - ((flag ? true : false) ? 2 : 0)) / 8;
		if (p1.Length < num)
		{
			throw new ArgumentException("output is too short - the array must be at least " + num + " items long");
		}
		int num2 = 0;
		IEnumerator<string> enumerator = ggsqy(p0, 8, (flag ? true : false) ? 2 : 0).GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				string current = enumerator.Current;
				uint num3 = uint.Parse(current, NumberStyles.HexNumber);
				p1[num2++] = num3;
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
	}

	private static IEnumerable<string> ggsqy(string p0, int p1, int p2)
	{
		twwcp twwcp = new twwcp(-2);
		twwcp.iwgqk = p0;
		twwcp.hjzus = p1;
		twwcp.vcvpw = p2;
		return twwcp;
	}

	public static uint xuzoz(ulong[] p0, int p1)
	{
		int num = p1 / 64;
		ulong num2 = p0[num];
		int num3 = p1 % 64;
		if (num3 < 32)
		{
			return (uint)(num2 >> num3);
		}
		ulong num4 = ((num + 1 < p0.Length) ? p0[num + 1] : 0);
		return (uint)((num2 >> p1) | (num4 << 64 - p1));
	}

	public static ulong huxax(ulong[] p0, int p1)
	{
		int num = p1 / 64;
		ulong num2 = p0[num];
		int num3 = p1 % 64;
		if (num3 == 0 || 1 == 0)
		{
			return num2;
		}
		ulong num4 = ((num + 1 >= p0.Length) ? 0 : p0[num + 1]);
		return (num2 >> num3) | (num4 << 64 - num3);
	}

	public static uint[] gksta(byte[] p0, int p1, int p2)
	{
		uint[] array = new uint[(p2 + 3) / 4];
		vbnwt(p0, p1, p2, array);
		return array;
	}

	public static void vbnwt(byte[] p0, int p1, int p2, uint[] p3)
	{
		int num = p2 / 4;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_000a;
		}
		goto IL_001c;
		IL_000a:
		p3[num2] = jlfbq.ubcts(p0, p1 + num2 * 4);
		num2++;
		goto IL_001c;
		IL_001c:
		if (num2 >= num)
		{
			int num3 = p2 % 4;
			if (num3 > 0)
			{
				p3[num2] = jlfbq.ucuck(p0, p1 + num * 4, num3);
			}
			return;
		}
		goto IL_000a;
	}

	public static uint[] tdbba(byte[] p0, int p1, int p2)
	{
		int num = p2;
		while (num > 0 && p0[p1 + num - 1] == 0)
		{
			num--;
		}
		if (num == 0 || 1 == 0)
		{
			return xvkbr;
		}
		return gksta(p0, p1, num);
	}

	public static uint[] hhtxd(byte[] p0, int p1, int p2, out int p3)
	{
		if (p2 == 0 || 1 == 0)
		{
			p3 = 0;
			return xvkbr;
		}
		int num = p1 + p2 - 1;
		byte b = p0[num];
		if ((b & 0x80) <= 0 || 1 == 0)
		{
			p3 = 1;
			return tdbba(p0, p1, p2);
		}
		p3 = -1;
		uint[] array = gksta(p0, p1, p2);
		jxopn.vyhkr(array);
		uint num2 = (uint)((1L << ((p2 - 1) % 4 + 1) * 8) - 1);
		array[array.Length - 1] &= num2;
		zkucb.tnhun(array, 1u, out var p4);
		if (p4 == 0 || 1 == 0)
		{
			return array;
		}
		uint[] array2 = new uint[array.Length + 1];
		Array.Copy(array, array2, array.Length);
		array2[array.Length] = p4;
		return array2;
	}

	public static void senkn(uint[] p0, byte[] p1, int p2, int p3)
	{
		if (p3 % 4 != 0 && 0 == 0)
		{
			throw new ArgumentException("Length must be divisible by 4");
		}
		int num = Math.Min(p3, p0.Length * 4);
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0029;
		}
		goto IL_003b;
		IL_003b:
		if (num2 >= num)
		{
			if (num < p3)
			{
				Array.Clear(p1, p2 + num, p3 - num);
			}
			return;
		}
		goto IL_0029;
		IL_0029:
		jlfbq.mfljd(p1, p2 + num2, p0[num2 / 4]);
		num2 += 4;
		goto IL_003b;
	}
}
