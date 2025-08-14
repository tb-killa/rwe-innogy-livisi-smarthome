using System;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal abstract class aicnd : mfhby
{
	private readonly hhfei aseer;

	private bool dsskv;

	private byte[] dbckz;

	private bool plkpg;

	public aicnd(agxpx previousState, IHashTransform mac, bool zlib)
		: base(previousState, mac)
	{
		dbckz = new byte[0];
		if (zlib && 0 == 0)
		{
			aseer = new hhfei();
			if (previousState is aicnd aicnd2 && 0 == 0)
			{
				dsskv = aicnd2.dsskv;
			}
		}
	}

	public override void lhpip()
	{
		if (aseer != null && 0 == 0)
		{
			dsskv = true;
		}
	}

	protected int vinqr(ArraySegment<byte> p0, out byte[] p1, int p2)
	{
		if (!dsskv || 1 == 0)
		{
			if (dbckz.Length < p2)
			{
				dbckz = new byte[(p2 + 8191) & -4096];
			}
			Array.Copy(p0.Array, p0.Offset + 5, dbckz, 0, p2);
			p1 = dbckz;
			return p2;
		}
		try
		{
			if (!plkpg || 1 == 0)
			{
				aseer.eanoq(p0.Array, p0.Offset + 7, p2 - 2, dzmpf.iksen);
				plkpg = true;
			}
			else
			{
				aseer.eanoq(p0.Array, p0.Offset + 5, p2, dzmpf.iksen);
			}
			int num = 0;
			while (true)
			{
				num += aseer.zohfz(dbckz, num, dbckz.Length - num);
				if (aseer.lotbz == yosfy.drxjq)
				{
					break;
				}
				if (num == dbckz.Length)
				{
					byte[] sourceArray = dbckz;
					dbckz = new byte[(num + 8191) & -4096];
					Array.Copy(sourceArray, 0, dbckz, 0, num);
				}
			}
			p1 = dbckz;
			return num;
		}
		catch (Exception inner)
		{
			throw new SshException(tcpjq.hmhzl, "Error while decompressing data.", inner);
		}
	}

	public override int vagtd(byte[] p0, int p1, byte[] p2)
	{
		throw new NotSupportedException();
	}
}
