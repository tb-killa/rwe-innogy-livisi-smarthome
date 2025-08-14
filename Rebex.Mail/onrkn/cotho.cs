using System.IO;

namespace onrkn;

internal class cotho : xlbfv
{
	private const int zgxlr = 76;

	private static readonly byte[] qcelu = new byte[3] { 61, 13, 10 };

	private static readonly byte[] xxhth = new byte[2] { 13, 10 };

	private readonly byte[] vhtzv;

	private readonly Stream vfggl;

	private readonly bool vidca;

	private int calqd;

	private bool ecbzj;

	private bool fhxbk;

	public cotho(Stream inner, bool binary, bool encodeProblematicSequences)
	{
		vhtzv = new byte[3] { 61, 0, 0 };
		vfggl = inner;
		vidca = binary;
		fhxbk = encodeProblematicSequences;
	}

	protected override void julnt()
	{
		ecbzj = true;
		base.julnt();
		vfggl.Close();
	}

	public override void Flush()
	{
		vfggl.Flush();
	}

	protected override void iabst(byte[] p0, int p1, int p2, bool p3)
	{
		int num = 76;
		if (!p3 || 1 == 0)
		{
			num--;
		}
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_001c;
		}
		goto IL_0068;
		IL_0068:
		if (num2 >= p2)
		{
			if (p3 && 0 == 0)
			{
				vfggl.Write(xxhth, 0, xxhth.Length);
				calqd = 0;
			}
			return;
		}
		goto IL_001c;
		IL_001c:
		byte b = p0[p1];
		p1++;
		bool p4 = vmfxc(b);
		if ((b == 32 || b == 9) && (((!p3 || 1 == 0) && !ecbzj) || num2 < p2 - 1))
		{
			p4 = true;
		}
		rpnix(b, p4);
		num2++;
		goto IL_0068;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (!vidca || 1 == 0)
		{
			base.Write(buffer, offset, count);
			return;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_001f;
		}
		goto IL_0041;
		IL_0041:
		if (num >= count)
		{
			return;
		}
		goto IL_001f;
		IL_001f:
		byte p = buffer[offset];
		offset++;
		rpnix(p, vmfxc(p));
		num++;
		goto IL_0041;
	}

	private void rpnix(byte p0, bool p1)
	{
		if (p1 && 0 == 0)
		{
			if (calqd >= 75)
			{
				vfggl.Write(qcelu, 0, qcelu.Length);
				calqd = 1;
			}
			else
			{
				calqd++;
			}
			if (fhxbk && 0 == 0 && calqd == 1 && (p0 == 70 || p0 == 46 || p0 == 45))
			{
				vhtzv[1] = uphld(p0 >> 4);
				vhtzv[2] = uphld(p0 & 0xF);
				vfggl.Write(vhtzv, 0, 3);
				calqd += 2;
			}
			else
			{
				vfggl.WriteByte(p0);
			}
		}
		else
		{
			if (calqd >= 73)
			{
				vfggl.Write(qcelu, 0, qcelu.Length);
				calqd = 3;
			}
			else
			{
				calqd += 3;
			}
			vhtzv[1] = uphld(p0 >> 4);
			vhtzv[2] = uphld(p0 & 0xF);
			vfggl.Write(vhtzv, 0, 3);
		}
	}

	private static bool vmfxc(byte p0)
	{
		if (33 > p0 || p0 > 60)
		{
			if (62 <= p0)
			{
				return p0 <= 126;
			}
			return false;
		}
		return true;
	}

	private static byte uphld(int p0)
	{
		return (byte)((p0 < 10) ? (48 + p0) : (65 + p0 - 10));
	}
}
