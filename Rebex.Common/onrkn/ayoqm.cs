using System;
using System.IO;

namespace onrkn;

internal class ayoqm : eiuaf, mhrzn
{
	private readonly hhfei invgj;

	private readonly mecsr kuoyz = new mecsr();

	private byte[] zvxri;

	private int xcjrj;

	private int tmgks;

	private ndkio.fhwwk sqcpc;

	private yosfy oznec;

	private uint rqcoc;

	private MemoryStream vlkyq;

	private int kchao;

	private ndkio.jypsy ldkvx;

	private string ogjif;

	private DateTime? issop;

	private string fhkiz;

	public string laepk => ogjif;

	public DateTime? llqan => issop;

	public string bafxj => fhkiz;

	public bool wngjx => invgj.wngjx;

	public yosfy lotbz => oznec;

	public bool vjaux
	{
		get
		{
			if (oznec != yosfy.aljno)
			{
				return sqcpc == ndkio.fhwwk.sfpjy;
			}
			return true;
		}
	}

	public ayoqm()
	{
		kchao = 10;
		invgj = new hhfei();
		oznec = yosfy.drxjq;
		sqcpc = ndkio.fhwwk.sfpjy;
	}

	private bool sryyw(byte[] p0, ref int p1, ref int p2)
	{
		int num = ((kchao < p2) ? kchao : p2);
		vlkyq.Write(p0, p1, num);
		kchao -= num;
		p1 += num;
		p2 -= num;
		return kchao == 0;
	}

	private void dhekw()
	{
		if (sqcpc < ndkio.fhwwk.rsacs && (ldkvx & ndkio.jypsy.ncofh) != ndkio.jypsy.apvpn && 0 == 0)
		{
			kchao = 2;
			sqcpc = ndkio.fhwwk.rsacs;
		}
		else if (sqcpc < ndkio.fhwwk.wwoxh && (ldkvx & ndkio.jypsy.gtpzn) != ndkio.jypsy.apvpn && 0 == 0)
		{
			xcjrj = (int)vlkyq.Position;
			sqcpc = ndkio.fhwwk.wwoxh;
		}
		else if (sqcpc < ndkio.fhwwk.twlqh && (ldkvx & ndkio.jypsy.oergj) != ndkio.jypsy.apvpn && 0 == 0)
		{
			xcjrj = (int)vlkyq.Position;
			sqcpc = ndkio.fhwwk.twlqh;
		}
		else if (sqcpc < ndkio.fhwwk.vqnhm && (ldkvx & ndkio.jypsy.megxg) != ndkio.jypsy.apvpn && 0 == 0)
		{
			kchao = 2;
			sqcpc = ndkio.fhwwk.vqnhm;
		}
		else
		{
			sqcpc = ndkio.fhwwk.iozqt;
		}
	}

	public void eanoq(byte[] p0, int p1, int p2, dzmpf p3)
	{
		if (oznec != yosfy.drxjq)
		{
			throw new InvalidOperationException("SetInput method can be called only when State is CompressorState.NewInput.");
		}
		if (p3 != dzmpf.iksen && 0 == 0)
		{
			throw new ArgumentException("No flags are required in Decompressor.", "flag");
		}
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("input", "Input buffer cannot be null.");
		}
		if (p1 < 0)
		{
			throw hifyx.nztrs("offset", p1, "Offset is negative.");
		}
		if (p2 < 0)
		{
			throw hifyx.nztrs("count", p2, "Count is negative.");
		}
		if (p1 + p2 > p0.Length)
		{
			throw new ArgumentException("The sum of offset and count is larger than the buffer length.");
		}
		while (p2 > 0)
		{
			switch (sqcpc)
			{
			case ndkio.fhwwk.sfpjy:
			{
				if (vlkyq == null || 1 == 0)
				{
					vlkyq = new MemoryStream(kchao);
				}
				if (!sryyw(p0, ref p1, ref p2))
				{
					break;
				}
				byte[] buffer = vlkyq.GetBuffer();
				if (buffer[0] != 31 || buffer[1] != 139 || buffer[2] != 8)
				{
					if (buffer[0] == 80 && buffer[1] == 75)
					{
						throw new sjylk("Archive seems to be using ZIP format. Please use ZipArchive class instead.");
					}
					throw new sjylk("Invalid GZIP header check.");
				}
				ldkvx = (ndkio.jypsy)buffer[3];
				uint num4 = BitConverter.ToUInt32(buffer, 4);
				if (num4 != 0 && 0 == 0)
				{
					issop = ndkio.fhyts.AddSeconds(num4);
				}
				dhekw();
				break;
			}
			case ndkio.fhwwk.rsacs:
				if (sryyw(p0, ref p1, ref p2) && 0 == 0)
				{
					kchao = BitConverter.ToUInt16(vlkyq.GetBuffer(), 10);
					sqcpc = ndkio.fhwwk.paepc;
				}
				break;
			case ndkio.fhwwk.paepc:
				if (sryyw(p0, ref p1, ref p2) && 0 == 0)
				{
					dhekw();
				}
				break;
			case ndkio.fhwwk.wwoxh:
			{
				int num5 = Array.IndexOf(p0, (byte)0, p1, p2);
				if (num5 < 0)
				{
					vlkyq.Write(p0, p1, p2);
					return;
				}
				kchao = num5 + 1 - p1;
				sryyw(p0, ref p1, ref p2);
				ogjif = ndkio.jexav.GetString(vlkyq.GetBuffer(), xcjrj, (int)vlkyq.Position - 1 - xcjrj);
				dhekw();
				break;
			}
			case ndkio.fhwwk.twlqh:
			{
				int num6 = Array.IndexOf(p0, (byte)0, p1, p2);
				if (num6 < 0)
				{
					vlkyq.Write(p0, p1, p2);
					return;
				}
				kchao = num6 + 1 - p1;
				sryyw(p0, ref p1, ref p2);
				fhkiz = ndkio.jexav.GetString(vlkyq.GetBuffer(), xcjrj, (int)vlkyq.Position - 1 - xcjrj);
				dhekw();
				break;
			}
			case ndkio.fhwwk.vqnhm:
				if (sryyw(p0, ref p1, ref p2) && 0 == 0)
				{
					int num2 = (int)vlkyq.Position - 2;
					kuoyz.Process(vlkyq.GetBuffer(), 0, num2);
					short num3 = (short)kuoyz.hxtqz;
					if (BitConverter.ToInt16(vlkyq.GetBuffer(), num2) != num3)
					{
						throw new sjylk("GZIP header CRC check failed.");
					}
					kuoyz.Reset();
					sqcpc = ndkio.fhwwk.iozqt;
				}
				break;
			case ndkio.fhwwk.iozqt:
				zvxri = p0;
				xcjrj = p1;
				tmgks = p2;
				invgj.eanoq(p0, p1, p2, p3);
				oznec = invgj.lotbz;
				return;
			case ndkio.fhwwk.ynurq:
				if (sryyw(p0, ref p1, ref p2) && 0 == 0)
				{
					int num = (int)kuoyz.hxtqz;
					if (BitConverter.ToInt32(vlkyq.GetBuffer(), 0) != num)
					{
						throw new sjylk("CRC32 check failed.");
					}
					if (BitConverter.ToUInt32(vlkyq.GetBuffer(), 4) != rqcoc)
					{
						throw new sjylk("Length of the decompressed data doesn't correspond to the length specified in the GZIP header.");
					}
					oznec = yosfy.aljno;
					xcjrj = p1;
					tmgks = p2;
				}
				return;
			}
		}
	}

	public int zohfz(byte[] p0, int p1, int p2)
	{
		if (oznec != yosfy.muyhp)
		{
			throw new InvalidOperationException("Process method can be called only when State is CompressorState.MoreOutput.");
		}
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("output", "Output buffer cannot be null.");
		}
		if (p1 < 0)
		{
			throw hifyx.nztrs("offset", p1, "Offset is negative.");
		}
		if (p2 < 0)
		{
			throw hifyx.nztrs("count", p2, "Count is negative.");
		}
		if (p1 + p2 > p0.Length)
		{
			throw new ArgumentException("The sum of offset and count is larger than the buffer length.");
		}
		int num = invgj.zohfz(p0, p1, p2);
		oznec = invgj.lotbz;
		kuoyz.Process(p0, p1, num);
		rqcoc += (uint)num;
		if (oznec == yosfy.aljno)
		{
			vlkyq.Position = 0L;
			kchao = 8;
			sqcpc = ndkio.fhwwk.ynurq;
			oznec = yosfy.drxjq;
			int num2 = invgj.pmhat();
			eanoq(zvxri, xcjrj + tmgks - num2, num2, dzmpf.iksen);
		}
		return num;
	}

	public int pmhat()
	{
		if (oznec != yosfy.aljno)
		{
			throw new InvalidOperationException("Finish method can be called only when State is CompressorState.Finish.");
		}
		ogjif = null;
		issop = null;
		fhkiz = null;
		zvxri = null;
		vlkyq.Close();
		vlkyq = null;
		kuoyz.Reset();
		rqcoc = 0u;
		kchao = 10;
		oznec = yosfy.drxjq;
		sqcpc = ndkio.fhwwk.sfpjy;
		return tmgks;
	}

	public bool ippjn(Stream p0, byte[] p1)
	{
		if (oznec == yosfy.aljno)
		{
			byte[] p2 = zvxri;
			int num = xcjrj + tmgks;
			int num2 = pmhat();
			eanoq(p2, num - num2, num2, dzmpf.iksen);
		}
		else if (sqcpc != ndkio.fhwwk.sfpjy && 0 == 0)
		{
			throw new InvalidOperationException("The method can be called only when a header was not read already.");
		}
		while (sqcpc != ndkio.fhwwk.iozqt)
		{
			int num3 = p0.Read(p1, 0, p1.Length);
			if (num3 <= 0)
			{
				return false;
			}
			eanoq(p1, 0, num3, dzmpf.iksen);
		}
		return true;
	}
}
