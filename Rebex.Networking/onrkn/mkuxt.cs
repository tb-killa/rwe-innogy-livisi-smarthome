using System;
using System.Text;

namespace onrkn;

internal abstract class mkuxt
{
	private bool kcznc;

	public bool lmnkf
	{
		get
		{
			return kcznc;
		}
		set
		{
			kcznc = value;
		}
	}

	public virtual void jfjrs(tndeg p0)
	{
		throw new NotSupportedException();
	}

	public byte[] wavkv(Encoding p0)
	{
		tndeg tndeg2 = new tndeg(p0);
		jfjrs(tndeg2);
		return tndeg2.ToArray();
	}

	public static void agnqw(tndeg p0, byte p1)
	{
		p0.WriteByte(p1);
	}

	public static void duaqa(tndeg p0, bool p1)
	{
		p0.WriteByte((byte)((p1 ? true : false) ? 1 : 0));
	}

	public static void ebmel(tndeg p0, uint p1)
	{
		p0.WriteByte((byte)((p1 >> 24) & 0xFF));
		p0.WriteByte((byte)((p1 >> 16) & 0xFF));
		p0.WriteByte((byte)((p1 >> 8) & 0xFF));
		p0.WriteByte((byte)(p1 & 0xFF));
	}

	public static void kwnor(tndeg p0, long p1)
	{
		ebmel(p0, (uint)((ulong)p1 >> 32));
		ebmel(p0, (uint)(p1 & 0xFFFFFFFFu));
	}

	public static void mkahh(tndeg p0, byte[] p1)
	{
		int num = p1.Length;
		if (num == 0 || false || p1[0] >= 128)
		{
			ebmel(p0, (uint)(num + 1));
			p0.WriteByte(0);
		}
		else
		{
			ebmel(p0, (uint)num);
		}
		if (p1 != null && 0 == 0)
		{
			p0.Write(p1, 0, p1.Length);
		}
	}

	public static void lcbhj(tndeg p0, byte[] p1, bool p2)
	{
		if (!p2 || 1 == 0)
		{
			ebmel(p0, (uint)p1.Length);
		}
		p0.Write(p1, 0, p1.Length);
	}

	public static void girvs(tndeg p0, byte[] p1, int p2, int p3, bool p4)
	{
		if (p2 < 0)
		{
			throw hifyx.nztrs("offset", p2, brgjd.edcru("Negative offset ({0}).", p2));
		}
		if (p3 < 0)
		{
			throw hifyx.nztrs("count", p3, brgjd.edcru("Negative byte count ({0}).", p3));
		}
		if (!p4 || 1 == 0)
		{
			ebmel(p0, (uint)p3);
		}
		p0.Write(p1, p2, p3);
	}

	public static void ijaon(tndeg p0, string[] p1)
	{
		excko(p0, string.Join(",", p1));
	}

	public static void excko(tndeg p0, string p1)
	{
		byte[] bytes = p0.sdfgh.GetBytes(p1);
		lcbhj(p0, bytes, p2: false);
	}

	public static void qxras(tndeg p0, object[] p1)
	{
		if (p1 == null || 1 == 0)
		{
			return;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0014;
		}
		goto IL_0061;
		IL_0014:
		object obj = p1[num];
		if (obj is uint && 0 == 0)
		{
			ebmel(p0, (uint)obj);
		}
		else
		{
			if (!(obj is string p2))
			{
				throw new NotSupportedException();
			}
			excko(p0, p2);
		}
		num++;
		goto IL_0061;
		IL_0061:
		if (num >= p1.Length)
		{
			return;
		}
		goto IL_0014;
	}

	public static string zhkby(byte[] p0)
	{
		string text = BitConverter.ToString(p0).Replace("-", "").TrimStart('0');
		if (text.Length == 0 || 1 == 0)
		{
			return "0";
		}
		return text;
	}
}
