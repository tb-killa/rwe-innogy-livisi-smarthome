using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using Rebex.Security.Cryptography;

namespace onrkn;

internal class bdjih : IComparable
{
	public const int uwfeh = 1;

	public const int kjxmd = -1;

	private readonly uint[] incup;

	private readonly int tjwqo;

	public static readonly bdjih sehjk = new bdjih(null, 0);

	public static readonly bdjih uvfcb = new bdjih(new uint[1] { 1u }, 1);

	public static readonly bdjih xbxmo = new bdjih(new uint[1] { 2u }, 1);

	private static uint[] gdwwk = new uint[32]
	{
		1u, 3u, 7u, 15u, 31u, 63u, 127u, 255u, 511u, 1023u,
		2047u, 4095u, 8191u, 16383u, 32767u, 65535u, 131071u, 262143u, 524287u, 1048575u,
		2097151u, 4194303u, 8388607u, 16777215u, 33554431u, 67108863u, 134217727u, 268435455u, 536870911u, 1073741823u,
		2147483647u, 4294967295u
	};

	private static RandomNumberGenerator rzjws = CryptoHelper.CreateRandomNumberGenerator();

	public bool acgax
	{
		get
		{
			if (incup != null && 0 == 0)
			{
				return (incup[0] & 1) == 0;
			}
			return true;
		}
	}

	private bdjih(uint[] data, int sign)
	{
		incup = data;
		tjwqo = sign;
	}

	public static bdjih dowrf(int p0)
	{
		if (p0 == 0 || 1 == 0)
		{
			return sehjk;
		}
		if (p0 == 1)
		{
			return uvfcb;
		}
		if (p0 <= 0)
		{
			return new bdjih(new uint[1] { (uint)(-p0) }, -1);
		}
		return new bdjih(new uint[1] { (uint)p0 }, 1);
	}

	public static bdjih shwvc(uint p0, int p1 = 1)
	{
		if (p0 == 0 || 1 == 0)
		{
			return sehjk;
		}
		if (p0 == 1 && p1 == 1)
		{
			return uvfcb;
		}
		bkjaz(p1);
		return new bdjih(new uint[1] { p0 }, p1);
	}

	public static bdjih arwqt(long p0)
	{
		if (p0 == long.MinValue)
		{
			return new bdjih(new uint[2] { 0u, 2147483648u }, -1);
		}
		if (p0 >= 0)
		{
			return roiht((ulong)p0);
		}
		return roiht((ulong)(-p0), -1);
	}

	public static bdjih roiht(ulong p0, int p1 = 1)
	{
		switch (p0)
		{
		case 0uL:
			return sehjk;
		case 1uL:
			if (p1 == 1)
			{
				return uvfcb;
			}
			break;
		}
		bkjaz(p1);
		if (p0 > uint.MaxValue)
		{
			return new bdjih(new uint[2]
			{
				(uint)p0,
				(uint)(p0 >> 32)
			}, p1);
		}
		return new bdjih(new uint[1] { (uint)p0 }, p1);
	}

	private static void bkjaz(int p0)
	{
	}

	public static bdjih foxoi(byte[] p0)
	{
		return xhqeh(p0, p1: false);
	}

	private static bdjih xhqeh(byte[] p0, bool p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0009;
		}
		goto IL_001a;
		IL_0009:
		if (p0[num] == 0 || 1 == 0)
		{
			num++;
			goto IL_001a;
		}
		goto IL_0020;
		IL_0058:
		uint[] array;
		int num2;
		int num3;
		array[num2] = (uint)(p0[num3] + (p0[num3 - 1] << 8) + (p0[num3 - 2] << 16) + (p0[num3 - 3] << 24));
		num3 -= 4;
		num2++;
		goto IL_0089;
		IL_0089:
		int num4;
		if (num2 >= num4)
		{
			if (num3 >= num)
			{
				array[num2] += p0[num3];
				if (num3 - 1 >= num)
				{
					array[num2] += (uint)(p0[num3 - 1] << 8);
					if (num3 - 2 >= num)
					{
						array[num2] += (uint)(p0[num3 - 2] << 16);
						if (num3 - 3 >= num)
						{
							array[num2] += (uint)(p0[num3 - 3] << 24);
						}
					}
				}
			}
			bdjih bdjih2 = new bdjih(array, 1);
			if (p1 && 0 == 0 && (p0[0] & 0x80) != 0 && 0 == 0)
			{
				num4 = p0.Length;
				bdjih bdjih3 = dowrf(1 << 8 * (num4 % 4)).zggmx(num4 / 4);
				bdjih3 = bdjih2 - bdjih3;
				return new bdjih(bdjih3.incup, -1);
			}
			return bdjih2;
		}
		goto IL_0058;
		IL_001a:
		if (num < p0.Length)
		{
			goto IL_0009;
		}
		goto IL_0020;
		IL_0020:
		num4 = p0.Length - num >> 2;
		int num5 = p0.Length - num + 3 >> 2;
		if (num5 == 0 || 1 == 0)
		{
			return sehjk;
		}
		array = new uint[num5];
		num3 = p0.Length - 1;
		num2 = 0;
		if (num2 != 0)
		{
			goto IL_0058;
		}
		goto IL_0089;
	}

	public static bdjih alcom(byte[] p0)
	{
		return xhqeh(p0, p1: true);
	}

	public static bdjih snrby(byte[] p0)
	{
		return zlbhm(p0, 0, p0.Length);
	}

	public static bdjih rcrky(uint[] p0, int p1 = 1)
	{
		return navxp(p0, 0, p0.Length, p1);
	}

	public static bdjih zlbhm(byte[] p0, int p1, int p2, int p3 = 1)
	{
		uint[] array = vgjtq.tdbba(p0, p1, p2);
		if (array.Length == 0 || 1 == 0)
		{
			return sehjk;
		}
		return new bdjih(array, p3);
	}

	public static bdjih skbfn(byte[] p0, int p1, int p2)
	{
		int p3;
		uint[] array = vgjtq.hhtxd(p0, p1, p2, out p3);
		if (array.Length == 0 || 1 == 0)
		{
			return sehjk;
		}
		return new bdjih(array, p3);
	}

	private static bdjih navxp(uint[] p0, int p1, int p2, int p3)
	{
		if (p2 == 0 || 1 == 0)
		{
			return sehjk;
		}
		int num = p2;
		while (num >= 1 && p0[p1 + num - 1] == 0)
		{
			num--;
		}
		if (num == 0 || 1 == 0)
		{
			return sehjk;
		}
		if (p1 > 0 || num < p2 || p0.Length > num)
		{
			uint[] array = new uint[num];
			Array.Copy(p0, p1, array, 0, num);
			return new bdjih(array, p3);
		}
		return new bdjih(p0, p3);
	}

	public static bdjih sqezg(string p0)
	{
		uint[] p1 = vgjtq.krjyb(p0);
		return rcrky(p1);
	}

	public static bdjih ofwyz(int p0)
	{
		if (p0 < 0)
		{
			throw new ArgumentOutOfRangeException("pow", "Parameter pow must be non-negative.");
		}
		if (p0 == 0 || 1 == 0)
		{
			return uvfcb;
		}
		if (p0 == 1)
		{
			return xbxmo;
		}
		int num = p0 / 32 + 1;
		uint[] array = new uint[num];
		array[num - 1] = (uint)(1 << p0 % 32);
		return new bdjih(array, 1);
	}

	public static implicit operator bdjih(int i)
	{
		return dowrf(i);
	}

	public static implicit operator bdjih(uint i)
	{
		return shwvc(i);
	}

	public static implicit operator bdjih(ulong n)
	{
		return roiht(n);
	}

	public static implicit operator bdjih(long n)
	{
		return arwqt(n);
	}

	public static bdjih operator +(bdjih a, bdjih b)
	{
		return a.ysvxl(b);
	}

	public static bdjih operator -(bdjih m, bdjih n)
	{
		return m.vrfey(n);
	}

	public static bdjih operator -(bdjih m)
	{
		if (m.tjwqo == 0 || 1 == 0)
		{
			return sehjk;
		}
		return new bdjih(m.incup, -m.tjwqo);
	}

	public static bdjih operator *(bdjih a, bdjih b)
	{
		if (a.tjwqo == 0 || false || b.tjwqo == 0 || 1 == 0)
		{
			return sehjk;
		}
		if (a == uvfcb && 0 == 0)
		{
			return b;
		}
		if (b == uvfcb && 0 == 0)
		{
			return a;
		}
		uint[] array = new uint[a.incup.Length + b.incup.Length];
		egqnn.oddei(a.incup, b.incup, array);
		return rcrky(array, a.tjwqo * b.tjwqo);
	}

	public static bdjih tolxn(bdjih p0, bdjih p1)
	{
		if (p0.tjwqo == 0 || false || p1.tjwqo == 0 || 1 == 0)
		{
			return sehjk;
		}
		if (p0 == 1 && 0 == 0)
		{
			return p1;
		}
		if (p1 == 1 && 0 == 0)
		{
			return p0;
		}
		uint[] array = new uint[p0.incup.Length + p1.incup.Length];
		egqnn.oddei(p0.incup, p1.incup, array);
		return rcrky(array, p0.tjwqo * p1.tjwqo);
	}

	public static bdjih operator /(bdjih a, bdjih b)
	{
		return a.vbcoa(b);
	}

	public static bdjih operator %(bdjih a, bdjih b)
	{
		szzlb(a, b, out var _, out var p2);
		return p2;
	}

	public static bdjih operator &(bdjih a, bdjih b)
	{
		return hbzdf(a, b);
	}

	public static bdjih operator >>(bdjih num, int bits)
	{
		if (num.tjwqo == 0 || 1 == 0)
		{
			return sehjk;
		}
		uint[] array = num.incup;
		int num2 = bits / 32;
		bits &= 0x1F;
		int num3 = 32 - bits;
		if (array.Length <= num2)
		{
			return 0;
		}
		uint[] array2 = new uint[array.Length - num2];
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_0051;
		}
		goto IL_0074;
		IL_0051:
		array2[num4] = (array[num4 + num2] >> bits) + (array[num4 + num2 + 1] << num3);
		num4++;
		goto IL_0074;
		IL_0074:
		if (num4 >= array2.Length - 1)
		{
			array2[num4] = array[num4 + num2] >> bits;
			if (num4 + num2 + 1 < array.Length)
			{
				array2[num4] += array[num4 + num2 + 1] << num3;
			}
			return rcrky(array2, num.tjwqo);
		}
		goto IL_0051;
	}

	public static bool operator >(bdjih m, bdjih n)
	{
		return ysrsk(m, n) > 0;
	}

	public static bool operator <(bdjih m, bdjih n)
	{
		return ysrsk(m, n) < 0;
	}

	public static bool operator >=(bdjih m, bdjih n)
	{
		if ((object)m == null || false || (object)n == null)
		{
			return false;
		}
		return ysrsk(m, n) >= 0;
	}

	public static bool operator <=(bdjih m, bdjih n)
	{
		return ysrsk(m, n) <= 0;
	}

	public static bool operator ==(bdjih m, bdjih n)
	{
		return ysrsk(m, n) == 0;
	}

	public static bool operator !=(bdjih m, bdjih n)
	{
		return ysrsk(m, n) != 0;
	}

	public static bool operator ==(bdjih m, int n)
	{
		if ((object)m == null || 1 == 0)
		{
			return false;
		}
		if (n == 0 || 1 == 0)
		{
			return m.tjwqo == 0;
		}
		if (m.tjwqo == 0 || 1 == 0)
		{
			return false;
		}
		if (m.incup.Length > 1)
		{
			return false;
		}
		if (m.tjwqo > 0 && n < 0)
		{
			return false;
		}
		if (m.tjwqo < 0 && n > 0)
		{
			return false;
		}
		uint num = (uint)Math.Abs((long)n);
		return m.incup[0] == num;
	}

	public static bool operator !=(bdjih m, int n)
	{
		if ((object)m == null || 1 == 0)
		{
			return false;
		}
		return !(m == n);
	}

	public static int aesaq(bdjih p0, object p1)
	{
		if (p1 == null || 1 == 0)
		{
			if ((object)p0 != null && 0 == 0)
			{
				return 1;
			}
			return 0;
		}
		if ((object)p0 == null || 1 == 0)
		{
			return -1;
		}
		bdjih bdjih2 = p1 as bdjih;
		if (!object.ReferenceEquals(bdjih2, null) || 1 == 0)
		{
			return ysrsk(p0, bdjih2);
		}
		throw new ArgumentException("Type " + p1.GetType().FullName + " is not supported for comparison with BigInt");
	}

	public static int ysrsk(bdjih p0, bdjih p1)
	{
		if (object.ReferenceEquals(p0, p1) && 0 == 0)
		{
			return 0;
		}
		if (object.ReferenceEquals(p0, null) && 0 == 0)
		{
			return -1;
		}
		if (object.ReferenceEquals(p1, null) && 0 == 0)
		{
			return 1;
		}
		int num = p0.tjwqo.CompareTo(p1.tjwqo);
		if (num != 0 && 0 == 0)
		{
			return num;
		}
		if (p0.tjwqo == 0 || 1 == 0)
		{
			return 0;
		}
		uint[] array = p0.incup;
		uint[] array2 = p1.incup;
		num = array.Length.CompareTo(array2.Length);
		if (num != 0 && 0 == 0)
		{
			return num * p0.tjwqo;
		}
		for (int num2 = array.Length - 1; num2 >= 0; num2--)
		{
			num = array[num2].CompareTo(array2[num2]);
			if (num != 0 && 0 == 0)
			{
				return num * p0.tjwqo;
			}
		}
		return 0;
	}

	public int CompareTo(object o)
	{
		return aesaq(this, o);
	}

	public override bool Equals(object o)
	{
		if (object.ReferenceEquals(o, null) && 0 == 0)
		{
			return false;
		}
		bdjih bdjih2 = o as bdjih;
		if (!object.ReferenceEquals(bdjih2, null) || 1 == 0)
		{
			return CompareTo(bdjih2) == 0;
		}
		throw new ArgumentException();
	}

	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}

	public bdjih tkbpw()
	{
		if (tjwqo >= 0)
		{
			return this;
		}
		return new bdjih(incup, 1);
	}

	public bdjih yleym()
	{
		if (tjwqo == 0 || 1 == 0)
		{
			return this;
		}
		return new bdjih(incup, -tjwqo);
	}

	public bdjih ysvxl(bdjih p0)
	{
		if (p0 == 0 && 0 == 0)
		{
			return this;
		}
		if (tjwqo == 0 || 1 == 0)
		{
			return p0;
		}
		if (tjwqo != p0.tjwqo)
		{
			if (tjwqo > 0)
			{
				return vrfey(p0.yleym());
			}
			return p0.vrfey(yleym());
		}
		uint[] array = incup;
		uint[] array2 = p0.incup;
		uint[] array3;
		if (array.Length < array2.Length)
		{
			array3 = array;
			array = array2;
			array2 = array3;
		}
		array3 = new uint[array.Length + 1];
		ulong num = 0uL;
		int i = 0;
		if (i != 0)
		{
			goto IL_008a;
		}
		goto IL_00ae;
		IL_008a:
		num += array[i];
		num += array2[i];
		array3[i] = (uint)(num & 0xFFFFFFFFu);
		num >>= 32;
		i++;
		goto IL_00ae;
		IL_00ae:
		if (i >= array2.Length)
		{
			for (; i < array.Length; i++)
			{
				if (num == 0)
				{
					break;
				}
				num += array[i];
				array3[i] = (uint)(num & 0xFFFFFFFFu);
				num >>= 32;
			}
			for (; i < array.Length; i++)
			{
				array3[i] = array[i];
			}
			array3[i] = (uint)(num & 0xFFFFFFFFu);
			return rcrky(array3, tjwqo);
		}
		goto IL_008a;
	}

	public static bdjih hbzdf(bdjih p0, bdjih p1)
	{
		if (((p0 == sehjk) ? true : false) || p1 == sehjk)
		{
			return sehjk;
		}
		int num = Math.Min(p0.incup.Length, p1.incup.Length);
		uint num2 = 0u;
		if (num2 != 0)
		{
			goto IL_004e;
		}
		goto IL_0064;
		IL_0064:
		if (--num >= 0)
		{
			goto IL_004e;
		}
		goto IL_006c;
		IL_004e:
		if ((num2 = p0.incup[num] & p1.incup[num]) == 0)
		{
			goto IL_0064;
		}
		goto IL_006c;
		IL_006c:
		if (num == -1)
		{
			return sehjk;
		}
		uint[] array = new uint[num + 1];
		array[num] = num2;
		while (--num >= 0)
		{
			array[num] = p0.incup[num] & p1.incup[num];
		}
		return new bdjih(array, 1);
	}

	public bdjih vrfey(bdjih p0)
	{
		if (p0 == 0 && 0 == 0)
		{
			return this;
		}
		if (tjwqo == 0 || 1 == 0)
		{
			return p0.yleym();
		}
		if (tjwqo != p0.tjwqo)
		{
			return ysvxl(p0.yleym());
		}
		int p1;
		uint[] p2;
		uint[] p3;
		if (this > p0 == tjwqo > 0)
		{
			p1 = tjwqo;
			p2 = incup;
			p3 = p0.incup;
		}
		else
		{
			p1 = -tjwqo;
			p2 = p0.incup;
			p3 = incup;
		}
		uint[] p4 = bjqgl(p2, p3);
		return rcrky(p4, p1);
	}

	private static uint[] bjqgl(uint[] p0, uint[] p1)
	{
		uint[] array = new uint[p0.Length];
		long num = 0L;
		int i = 0;
		if (i != 0)
		{
			goto IL_0015;
		}
		goto IL_0034;
		IL_0015:
		num += p0[i];
		num -= p1[i];
		array[i] = (uint)(num & 0xFFFFFFFFu);
		num >>= 32;
		i++;
		goto IL_0034;
		IL_0034:
		if (i >= p1.Length)
		{
			for (; i < p0.Length; i++)
			{
				if (num >= 0)
				{
					break;
				}
				num += p0[i];
				array[i] = (uint)(num & 0xFFFFFFFFu);
				num >>= 32;
			}
			for (; i < p0.Length; i++)
			{
				array[i] = p0[i];
			}
			return array;
		}
		goto IL_0015;
	}

	public static void szzlb(bdjih p0, bdjih p1, out bdjih p2, out bdjih p3)
	{
		if (p1.tjwqo == 0 || 1 == 0)
		{
			throw new DivideByZeroException();
		}
		if (p0.tjwqo == 0 || 1 == 0)
		{
			p2 = sehjk;
			p3 = sehjk;
			return;
		}
		if (p0.incup.Length < p1.incup.Length)
		{
			p2 = sehjk;
			p3 = p0;
			return;
		}
		uint[] array = (uint[])p0.incup.Clone();
		int p4 = array.Length;
		uint[] array2 = new uint[p0.incup.Length - p1.incup.Length + 1];
		ohmbc.tcunw(array, ref p4, p1.incup, p1.incup.Length, array2, out var p5);
		p3 = navxp(array, 0, p4, p0.tjwqo);
		p2 = navxp(array2, 0, p5, p1.tjwqo * p0.tjwqo);
	}

	public bdjih vbcoa(bdjih p0)
	{
		szzlb(this, p0, out var p1, out var _);
		return p1;
	}

	public bdjih fvbmy(bdjih p0)
	{
		bdjih bdjih2 = this % p0;
		if (bdjih2.tjwqo < 0)
		{
			bdjih2 += p0;
		}
		return bdjih2;
	}

	public static void fmpak(bdjih p0, bdjih p1, out bdjih p2, out bdjih p3)
	{
		Stack stack = new Stack();
		while (true)
		{
			bdjih bdjih2 = p0.fvbmy(p1);
			if ((bdjih2 == 0) ? true : false)
			{
				break;
			}
			stack.Push(p0);
			p0 = p1;
			p1 = bdjih2;
		}
		p2 = 0;
		p3 = 1;
		while (stack.Count > 0)
		{
			p1 = p0;
			p0 = (bdjih)stack.Pop();
			bdjih bdjih3 = p3;
			p3 = p2 - bdjih3 * (p0 / p1);
			p2 = bdjih3;
		}
	}

	public bdjih rjdow(bdjih p0)
	{
		if (p0.tjwqo != 1)
		{
			throw new ArithmeticException("Modulus must be positive.");
		}
		fmpak(this, p0, out var p1, out var _);
		if (p1.tjwqo == -1)
		{
			return p1 + p0;
		}
		return p1;
	}

	public ulong zgovv()
	{
		if (acgax && 0 == 0)
		{
			throw new ArithmeticException("Number is not odd.");
		}
		bdjih p = 4294967296L;
		bdjih bdjih2 = yleym().fvbmy(p).rjdow(p);
		if (bdjih2.incup == null || false || bdjih2.tjwqo == 0 || 1 == 0)
		{
			throw new ArithmeticException("Inverse modulus is zero.");
		}
		ulong result = bdjih2.incup.Length switch
		{
			1 => bdjih2.incup[0], 
			2 => (ulong)((long)bdjih2.incup[0] + (long)bdjih2.incup[1] << 32), 
			_ => throw new ArithmeticException("Unexpected inverse modulus value."), 
		};
		if (tjwqo <= 0)
		{
			throw new NotSupportedException();
		}
		return result;
	}

	public int jaioo()
	{
		if (incup == null || 1 == 0)
		{
			return 0;
		}
		uint num = 0u;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_001c;
		}
		goto IL_003f;
		IL_009e:
		return 0;
		IL_008c:
		int num3;
		if (num3 < 0)
		{
			goto IL_009e;
		}
		goto IL_006e;
		IL_001c:
		num = incup[incup.Length - 1 - num2];
		if (num == 0 || 1 == 0)
		{
			num2++;
			goto IL_003f;
		}
		goto IL_004a;
		IL_004a:
		num2 = incup.Length - 1 - num2;
		uint num4;
		if (num != 0 && 0 == 0)
		{
			num4 = 2147483648u;
			num3 = 31;
			if (num3 == 0)
			{
				goto IL_006e;
			}
			goto IL_008c;
		}
		goto IL_009e;
		IL_006e:
		if ((num & num4) != 0 && 0 == 0)
		{
			return num2 * 32 + num3 + 1;
		}
		num4 >>= 1;
		num3--;
		goto IL_008c;
		IL_003f:
		if (num2 < incup.Length)
		{
			goto IL_001c;
		}
		goto IL_004a;
	}

	public int txrcx()
	{
		if (incup == null || 1 == 0)
		{
			return -1;
		}
		uint num = 0u;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_001c;
		}
		goto IL_0034;
		IL_003f:
		if (num == 0 || 1 == 0)
		{
			return -1;
		}
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_0051;
		}
		goto IL_006f;
		IL_0034:
		if (num2 < incup.Length)
		{
			goto IL_001c;
		}
		goto IL_003f;
		IL_001c:
		num = incup[num2];
		if (num == 0 || 1 == 0)
		{
			num2++;
			goto IL_0034;
		}
		goto IL_003f;
		IL_0051:
		if ((num & gdwwk[num3]) != 0 && 0 == 0)
		{
			return num2 * 32 + num3;
		}
		num3++;
		goto IL_006f;
		IL_006f:
		if (num3 < 32)
		{
			goto IL_0051;
		}
		return -1;
	}

	public bdjih dletx(int p0)
	{
		p0--;
		int num = p0 / 32;
		p0 &= 0x1F;
		if (incup == null || 1 == 0)
		{
			return this;
		}
		if (incup.Length < num)
		{
			return this;
		}
		uint[] array = new uint[num + 1];
		Array.Copy(incup, 0, array, 0, num);
		array[num] = incup[num] & gdwwk[p0];
		return rcrky(array);
	}

	public bdjih zggmx(int p0)
	{
		if (p0 == 0 || 1 == 0)
		{
			return this;
		}
		uint[] array = new uint[incup.Length + p0];
		incup.CopyTo(array, p0);
		return new bdjih(array, tjwqo);
	}

	public bdjih hbhui(bdjih p0, bdjih p1)
	{
		if (this == 0 && 0 == 0)
		{
			return this;
		}
		if (p0.tjwqo == 0 || 1 == 0)
		{
			return uvfcb.fvbmy(p1);
		}
		if (p1.acgax && 0 == 0)
		{
			throw new NotSupportedException("ModPow not implemented for even modulo.");
		}
		ulong p2 = p1.zgovv();
		return fydfl(p0, p1, p2);
	}

	private bdjih fydfl(bdjih p0, bdjih p1, ulong p2)
	{
		int num = p1.incup.Length;
		bdjih bdjih2 = zggmx(num).fvbmy(p1);
		if (bdjih2 == 0 && 0 == 0)
		{
			return 0;
		}
		uint[] p3 = new uint[num + 2];
		uint[] p4 = p1.jeoht(num);
		uint[] p5 = uvfcb.jeoht(num);
		uint[] array = bdjih2.jeoht(num);
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0064;
		}
		goto IL_0121;
		IL_00f9:
		bool flag;
		if (flag && 0 == 0)
		{
			return 0;
		}
		int num3 = num3 + 1;
		goto IL_0112;
		IL_0121:
		if (num2 < p0.incup.Length)
		{
			goto IL_0064;
		}
		return rcrky(p5);
		IL_0064:
		uint num4 = p0.incup[num2];
		num3 = 0;
		if (num3 != 0)
		{
			goto IL_0079;
		}
		goto IL_0112;
		IL_0079:
		if (num4 != 0 && 0 == 0)
		{
			if ((num4 & 1) != 0 && 0 == 0)
			{
				jggbm(p5, array, p4, p3, p2);
			}
			num4 >>= 1;
		}
		int num5;
		if (((num4 != 0) ? true : false) || num2 != p0.incup.Length - 1)
		{
			jggbm(array, array, p4, p3, p2);
			flag = true;
			num5 = 0;
			if (num5 != 0)
			{
				goto IL_00d5;
			}
			goto IL_00f1;
		}
		goto IL_011b;
		IL_00d5:
		if (array[num5] != 0 && 0 == 0)
		{
			flag = false;
			if (!flag)
			{
				goto IL_00f9;
			}
		}
		num5++;
		goto IL_00f1;
		IL_011b:
		num2++;
		goto IL_0121;
		IL_0112:
		if (num3 < 32)
		{
			goto IL_0079;
		}
		goto IL_011b;
		IL_00f1:
		if (num5 < array.Length)
		{
			goto IL_00d5;
		}
		goto IL_00f9;
	}

	private uint[] jeoht(int p0)
	{
		if (incup == null || 1 == 0)
		{
			return null;
		}
		if (p0 < incup.Length)
		{
			throw new InvalidOperationException("Not enough space in the requested buffer.");
		}
		if (p0 == incup.Length)
		{
			return incup;
		}
		uint[] array = new uint[p0];
		incup.CopyTo(array, 0);
		return array;
	}

	private static void xknge(uint[] p0, ulong p1, uint[] p2)
	{
		int i = 0;
		ulong num = 0uL;
		for (; i < p2.Length; i++)
		{
			ulong num2 = p1 * p2[i];
			ulong num3 = p0[i] + (num2 & 0xFFFFFFFFu) + (num & 0xFFFFFFFFu);
			p0[i] = (uint)(num3 & 0xFFFFFFFFu);
			num >>= 32;
			num += (num2 >> 32) + (num3 >> 32);
		}
		num += p0[i];
		p0[i] = (uint)(num & 0xFFFFFFFFu);
		p0[i + 1] = (uint)(num >> 32);
	}

	public static void jggbm(uint[] p0, uint[] p1, uint[] p2, uint[] p3, ulong p4)
	{
		int num = p2.Length;
		Array.Clear(p3, 0, p3.Length);
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0017;
		}
		goto IL_006a;
		IL_0017:
		ulong num3 = p0[num2];
		ulong p5 = ((p3[0] + num3 * p1[0]) * p4) & 0xFFFFFFFFu;
		xknge(p3, num3, p1);
		xknge(p3, p5, p2);
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_0045;
		}
		goto IL_0055;
		IL_0045:
		p3[num4] = p3[num4 + 1];
		num4++;
		goto IL_0055;
		IL_0055:
		if (num4 < p3.Length - 1)
		{
			goto IL_0045;
		}
		p3[p3.Length - 1] = 0u;
		num2++;
		goto IL_006a;
		IL_006a:
		if (num2 >= num)
		{
			if (((p3[p3.Length - 2] != 0) ? true : false) || p3[p3.Length - 3] > p2[p2.Length - 1])
			{
				p3 = bjqgl(p3, p2);
			}
			Array.Copy(p3, 0, p0, 0, num);
			return;
		}
		goto IL_0017;
	}

	public int pcwqf()
	{
		if (tjwqo == 0 || 1 == 0)
		{
			return 0;
		}
		if (incup.Length > 1 || incup[0] > int.MaxValue)
		{
			if (tjwqo < 0)
			{
				return int.MinValue;
			}
			return int.MaxValue;
		}
		if (tjwqo < 0)
		{
			return (int)(0L - (long)incup[0]);
		}
		return (int)incup[0];
	}

	public void dzgdd(byte[] p0, int p1, int p2)
	{
		vgjtq.senkn(incup, p0, p1, p2);
	}

	public byte[] kskce(bool p0)
	{
		if (incup == null || false || tjwqo == 0 || 1 == 0)
		{
			return new byte[1];
		}
		int num = incup.Length - 1;
		uint num2 = incup[num];
		int num3 = num * 4;
		if (num2 != 0)
		{
			num3 = ((num2 <= 255) ? (num3 + 1) : ((num2 <= 65535) ? (num3 + 2) : ((num2 > 16777215) ? (num3 + 4) : (num3 + 3))));
		}
		int num4 = (num3 & 3) switch
		{
			3 => (int)((incup[num] >> 16) & 0xFF), 
			2 => (int)((incup[num] >> 8) & 0xFF), 
			1 => (int)(incup[num] & 0xFF), 
			_ => (int)((incup[num] >> 24) & 0xFF), 
		};
		byte[] array2;
		if (tjwqo < 0)
		{
			if (!p0 || 1 == 0)
			{
				throw new CryptographicException("Negative numbers not allowed.");
			}
			bdjih bdjih2 = dowrf(1 << 8 * (num3 % 4)).zggmx(num3 / 4);
			bdjih2 += this;
			byte[] array = bdjih2.kskce(p0: false);
			if ((array[0] & 0x80) != 0 && 0 == 0)
			{
				return array;
			}
			array2 = new byte[array.Length + 1];
			array2[0] = byte.MaxValue;
			array.CopyTo(array2, 1);
			return array2;
		}
		int num5;
		if (num4 >= 128 && p0 && 0 == 0)
		{
			array2 = new byte[num3 + 1];
			num5 = 1;
			if (num5 != 0)
			{
				goto IL_0191;
			}
		}
		array2 = new byte[num3];
		num5 = 0;
		goto IL_0191;
		IL_0191:
		switch (num3 & 3)
		{
		case 3:
			array2[num5] = (byte)((incup[num] >> 16) & 0xFF);
			num5++;
			goto case 2;
		case 2:
			array2[num5] = (byte)((incup[num] >> 8) & 0xFF);
			num5++;
			goto case 1;
		case 1:
			array2[num5] = (byte)(incup[num] & 0xFF);
			num5++;
			num--;
			break;
		}
		while (num5 < array2.Length)
		{
			array2[num5] = (byte)((incup[num] >> 24) & 0xFF);
			array2[num5 + 1] = (byte)((incup[num] >> 16) & 0xFF);
			array2[num5 + 2] = (byte)((incup[num] >> 8) & 0xFF);
			array2[num5 + 3] = (byte)(incup[num] & 0xFF);
			num5 += 4;
			num--;
		}
		return array2;
	}

	public override string ToString()
	{
		return zklqn();
	}

	public string zklqn()
	{
		if (tjwqo == 0 || 1 == 0)
		{
			return "0";
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int num = incup.Length - 1; num >= 0; num--)
		{
			stringBuilder.Append(incup[num].ToString("x8"));
		}
		string text = stringBuilder.ToString().TrimStart('0').emrfc();
		if (tjwqo <= 0)
		{
			return "-" + text;
		}
		return text;
	}

	public static bdjih izifm(bdjih p0, bdjih p1)
	{
		if (p0 == p1 && 0 == 0)
		{
			return p0;
		}
		if (p0 > p1 && 0 == 0)
		{
			throw new ArithmeticException("Empty interval.");
		}
		bdjih bdjih2 = p1 - p0;
		byte[] array = new byte[bdjih2.incup.Length * 4];
		rzjws.GetBytes(array);
		bdjih bdjih3 = foxoi(array);
		if (bdjih3 > bdjih2 && 0 == 0)
		{
			bdjih3 = bdjih3.fvbmy(bdjih2);
		}
		return p0 + bdjih3;
	}

	public bool fsxwh(int p0)
	{
		if (incup == null || false || incup.Length == 0 || 1 == 0)
		{
			return false;
		}
		if (incup[0] == 2)
		{
			return true;
		}
		if (incup[0] == 1)
		{
			return false;
		}
		if ((incup[0] & 1) != 1)
		{
			return false;
		}
		bdjih bdjih2 = this - 1;
		int num = bdjih2.txrcx();
		bdjih p1 = bdjih2 >> num;
		ulong p2 = zgovv();
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0080;
		}
		goto IL_012b;
		IL_0080:
		bdjih bdjih3 = izifm(xbxmo, bdjih2);
		bdjih bdjih4 = bdjih3.fydfl(p1, this, p2);
		bool flag;
		int num3;
		if ((!(bdjih4 == 1) || 1 == 0) && (!(bdjih4 == bdjih2) || 1 == 0))
		{
			flag = false;
			num3 = 1;
			if (num3 == 0)
			{
				goto IL_00ce;
			}
			goto IL_0112;
		}
		goto IL_0125;
		IL_0117:
		if (!flag || 1 == 0)
		{
			return false;
		}
		goto IL_0125;
		IL_0112:
		if (num3 <= num)
		{
			goto IL_00ce;
		}
		goto IL_0117;
		IL_0125:
		num2++;
		goto IL_012b;
		IL_00ce:
		bdjih4 = bdjih4.fydfl(xbxmo, this, p2);
		if (bdjih4 == bdjih2 && 0 == 0)
		{
			flag = true;
			if (flag)
			{
				goto IL_0117;
			}
		}
		if (bdjih4 == 1 && 0 == 0)
		{
			return false;
		}
		num3++;
		goto IL_0112;
		IL_012b:
		if (num2 <= p0)
		{
			goto IL_0080;
		}
		return true;
	}
}
