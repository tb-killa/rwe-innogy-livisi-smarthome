using System;
using Rebex.Net;
using Rebex.Security.Cryptography;

namespace onrkn;

internal abstract class crosq : mfhby
{
	private readonly ifjbk wppwa;

	private bool heidj;

	private bool wxtov;

	public override int ptjhi => 0;

	public crosq(agxpx previousState, IHashTransform mac, int? compressionLevel)
		: base(previousState, mac)
	{
		if (compressionLevel.HasValue && 0 == 0)
		{
			wppwa = new ifjbk(compressionLevel.Value);
			if (previousState is crosq crosq2 && 0 == 0)
			{
				heidj = crosq2.heidj;
			}
		}
	}

	public override void lhpip()
	{
		if (wppwa != null && 0 == 0)
		{
			heidj = true;
		}
	}

	public override int iadch(byte[] p0, int p1, int p2)
	{
		throw new NotSupportedException();
	}

	public override int lyhbb(byte[] p0, int p1, int p2)
	{
		throw new NotSupportedException();
	}

	public override int keqao(ArraySegment<byte> p0, out byte[] p1)
	{
		throw new NotSupportedException();
	}

	protected int lzctp(byte[] p0, int p1, byte[] p2, int p3)
	{
		if (heidj && 0 == 0)
		{
			try
			{
				wppwa.eanoq(p0, 0, p1, dzmpf.dqswj);
				if (!wxtov || 1 == 0)
				{
					p2[p3 + 5] = 120;
					p2[p3 + 6] = 156;
					p1 = wppwa.zohfz(p2, p3 + 7, p2.Length - 7 - p3);
					p1 += 2;
					wxtov = true;
				}
				else
				{
					p1 = wppwa.zohfz(p2, p3 + 5, p2.Length - 5 - p3);
				}
				if (wppwa.lotbz != yosfy.drxjq)
				{
					throw new SshException(tcpjq.bxwwb, "Error while compressing data.", new InvalidOperationException("Compressed block size too long."));
				}
			}
			catch (Exception inner)
			{
				throw new SshException(tcpjq.bxwwb, "Error while compressing data.", inner);
			}
		}
		else
		{
			Array.Copy(p0, 0, p2, p3 + 5, p1);
		}
		return p1;
	}
}
