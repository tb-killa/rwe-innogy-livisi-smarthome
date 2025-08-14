using System;

namespace onrkn;

internal static class peekn
{
	public const int uzuit = 7;

	public const int lqrzd = 3;

	public static bool iatkb = false;

	private static readonly sxztb<ulong> kqttg = sxztb<ulong>.adgoe(1048576, 50);

	private static readonly sxztb<uint> vmrln = sxztb<uint>.adgoe(1048576, 50);

	private static readonly sxztb<byte> jushy = sxztb<byte>.adgoe(1048576, 50);

	public static ayjol nmjtk(byte[] p0, int p1, int p2, bool p3)
	{
		if (p0 == null || false || p2 == 0 || 1 == 0)
		{
			return default(ayjol);
		}
		if (!iatkb || 1 == 0)
		{
			return roipc(p0, p1, p2, p3);
		}
		if ((p1 & 7) != 0 && 0 == 0)
		{
			return roipc(p0, p1, p2, p3);
		}
		rbznd rbznd2 = new rbznd
		{
			uvrvn = p0
		};
		return new ayjol
		{
			rjqep = rbznd2.rvevi
		};
	}

	public static otspa txkpr(ulong[] p0, int p1, int p2, bool p3)
	{
		if (p0 == null || false || p2 == 0 || 1 == 0)
		{
			return default(otspa);
		}
		return tbhmr(p0, p1, p2, p3);
	}

	public static otspa ahheu(nxtme<ulong> p0, bool p1)
	{
		return txkpr(p0.lthjd, p0.frlfs, p0.hvvsm, p1);
	}

	public static qetva dquvs(byte[] p0, int p1, int p2, bool p3)
	{
		if (p0 == null || false || p2 == 0 || 1 == 0)
		{
			return default(qetva);
		}
		if (!iatkb || 1 == 0)
		{
			return ojgwj(p0, p1, p2, p3);
		}
		if ((p1 & 3) != 0 && 0 == 0)
		{
			return ojgwj(p0, p1, p2, p3);
		}
		rbznd rbznd2 = new rbznd
		{
			uvrvn = p0
		};
		return new qetva
		{
			naxcq = rbznd2.twsfb
		};
	}

	private static ayjol roipc(byte[] p0, int p1, int p2, bool p3)
	{
		int p4 = p2 >> 3;
		ulong[] array = kqttg.vfhlp(p4);
		if (p3 && 0 == 0)
		{
			Buffer.BlockCopy(p0, p1, array, 0, p2);
		}
		return new ayjol
		{
			rjqep = array,
			ciiua = kqttg
		};
	}

	private static qetva ojgwj(byte[] p0, int p1, int p2, bool p3)
	{
		int p4 = p2 >> 2;
		uint[] array = vmrln.vfhlp(p4);
		if (p3 && 0 == 0)
		{
			Buffer.BlockCopy(p0, p1, array, 0, p2);
		}
		return new qetva
		{
			naxcq = array,
			lssrg = vmrln
		};
	}

	private static otspa tbhmr(ulong[] p0, int p1, int p2, bool p3)
	{
		int num = p2 << 3;
		byte[] array = jushy.vfhlp(num);
		if (p3 && 0 == 0)
		{
			Buffer.BlockCopy(p0, p1, array, 0, num);
		}
		return new otspa
		{
			mjnob = array,
			kowby = jushy
		};
	}
}
