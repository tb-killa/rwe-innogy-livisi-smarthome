using System;
using System.IO;

namespace onrkn;

internal class dnvsy : mhrzn
{
	private readonly ifjbk tjjov;

	private readonly mecsr sqgag = new mecsr();

	private ndkio.fhwwk hdrmu;

	private yosfy rsmoq;

	private uint bvavn;

	private byte[] bzsvd;

	private int jsiwx;

	private int iyniq;

	private string iwpmu;

	private DateTime? yftgd;

	private string depqp;

	private bool lvkjm;

	private bool grkej;

	internal bool qrrxd
	{
		get
		{
			return grkej;
		}
		set
		{
			grkej = value;
		}
	}

	public string hnkzw
	{
		get
		{
			return iwpmu;
		}
		set
		{
			if (lvkjm && 0 == 0)
			{
				throw new InvalidOperationException("File name can be set only in initial state.");
			}
			if (value != null && 0 == 0 && value.IndexOf('\0') >= 0)
			{
				throw new ArgumentException("Illegal characters in path.", "value");
			}
			iwpmu = value;
		}
	}

	public DateTime? jhtam
	{
		get
		{
			return yftgd;
		}
		set
		{
			if (lvkjm && 0 == 0)
			{
				throw new InvalidOperationException("Last write time can be set only in initial state.");
			}
			if (value.HasValue && 0 == 0 && value.Value.ToUniversalTime() < ndkio.fhyts && 0 == 0)
			{
				throw new ArgumentException("Invalid date (must be greater than 1970-1-1).", "value");
			}
			yftgd = value;
		}
	}

	public string btyyq
	{
		get
		{
			return depqp;
		}
		set
		{
			if (lvkjm && 0 == 0)
			{
				throw new InvalidOperationException("Comment can be set only in initial state.");
			}
			if (value != null && 0 == 0 && value.IndexOf('\0') >= 0)
			{
				throw new ArgumentException("Invalid characters in comment.", "value");
			}
			depqp = value;
		}
	}

	public yosfy lotbz => rsmoq;

	public dnvsy()
		: this(6)
	{
	}

	public dnvsy(int compressionLevel)
		: this(compressionLevel, 49152, null, null, null)
	{
	}

	public dnvsy(int compressionLevel, int blockSize, string fileName, DateTime? lastWriteTime, string comment)
	{
		if (fileName != null && 0 == 0 && fileName.IndexOf('\0') >= 0)
		{
			throw new ArgumentException("Illegal characters in path.", "fileName");
		}
		if (lastWriteTime.HasValue && 0 == 0 && lastWriteTime.Value.ToUniversalTime() < ndkio.fhyts && 0 == 0)
		{
			throw new ArgumentException("Invalid date (must be greater than 1970-1-1).", "lastWriteTime");
		}
		if (comment != null && 0 == 0 && comment.IndexOf('\0') >= 0)
		{
			throw new ArgumentException("Invalid characters in comment.", "comment");
		}
		iwpmu = fileName;
		yftgd = lastWriteTime;
		depqp = comment;
		tjjov = new ifjbk(useEnhancedDeflate: false, compressionLevel, blockSize, 9);
		rsmoq = yosfy.drxjq;
		hdrmu = ndkio.fhwwk.sfpjy;
	}

	private void jodvn()
	{
		int num = 12;
		byte[] array = null;
		if (iwpmu != null && 0 == 0)
		{
			array = ndkio.jexav.GetBytes(iwpmu);
			num += array.Length + 1;
		}
		uint num2 = 0u;
		if (yftgd.HasValue && 0 == 0)
		{
			num2 = (uint)(yftgd.Value.ToUniversalTime() - ndkio.fhyts).TotalSeconds;
		}
		byte[] array2 = null;
		if (depqp != null && 0 == 0)
		{
			array2 = ndkio.jexav.GetBytes(depqp.Replace("\r\n", "\n").Replace('\r', '\n'));
			num += array2.Length + 1;
		}
		if (bzsvd == null || false || bzsvd.Length < num)
		{
			bzsvd = new byte[num];
		}
		bzsvd[0] = 31;
		bzsvd[1] = 139;
		bzsvd[2] = 8;
		bzsvd[3] = 0;
		if (qrrxd && 0 == 0)
		{
			bzsvd[3] |= 2;
		}
		if (iwpmu != null && 0 == 0)
		{
			bzsvd[3] |= 8;
		}
		if (depqp != null && 0 == 0)
		{
			bzsvd[3] |= 16;
		}
		bzsvd[4] = (byte)num2;
		bzsvd[5] = (byte)(num2 >> 8);
		bzsvd[6] = (byte)(num2 >> 16);
		bzsvd[7] = (byte)(num2 >> 24);
		switch (tjjov.ppzjg)
		{
		case 0:
		case 1:
			bzsvd[8] = 4;
			break;
		case 9:
			bzsvd[8] = 2;
			break;
		default:
			bzsvd[8] = 3;
			break;
		}
		bzsvd[9] = byte.MaxValue;
		int num3 = 10;
		if (iwpmu != null && 0 == 0)
		{
			Array.Copy(array, 0, bzsvd, num3, array.Length);
			num3 += array.Length + 1;
		}
		if (depqp != null && 0 == 0)
		{
			Array.Copy(array2, 0, bzsvd, num3, array2.Length);
			num3 += array2.Length + 1;
		}
		if (qrrxd && 0 == 0)
		{
			sqgag.Process(bzsvd, 0, num3);
			bzsvd[num3++] = (byte)sqgag.hxtqz;
			bzsvd[num3++] = (byte)(sqgag.hxtqz >> 8);
		}
		sqgag.Reset();
		jsiwx = 0;
		iyniq = num3;
		lvkjm = true;
	}

	private void lhiyx()
	{
		int num = (int)sqgag.hxtqz;
		bzsvd[0] = (byte)num;
		bzsvd[1] = (byte)(num >> 8);
		bzsvd[2] = (byte)(num >> 16);
		bzsvd[3] = (byte)(num >> 24);
		bzsvd[4] = (byte)bvavn;
		bzsvd[5] = (byte)(bvavn >> 8);
		bzsvd[6] = (byte)(bvavn >> 16);
		bzsvd[7] = (byte)(bvavn >> 24);
		jsiwx = 0;
		iyniq = 8;
	}

	private int dlfxz(byte[] p0, ref int p1, ref int p2)
	{
		int num = ((iyniq < p2) ? iyniq : p2);
		Array.Copy(bzsvd, jsiwx, p0, p1, num);
		jsiwx += num;
		iyniq -= num;
		p1 += num;
		p2 -= num;
		return num;
	}

	public void eanoq(byte[] p0, int p1, int p2, dzmpf p3)
	{
		if (rsmoq != yosfy.drxjq)
		{
			throw new InvalidOperationException("SetInput method can be called only when State is CompressorState.NewInput.");
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
		if (!lvkjm || 1 == 0)
		{
			jodvn();
		}
		bvavn += (uint)p2;
		sqgag.Process(p0, p1, p2);
		tjjov.eanoq(p0, p1, p2, p3);
		rsmoq = tjjov.lotbz;
	}

	public int zohfz(byte[] p0, int p1, int p2)
	{
		if (rsmoq != yosfy.muyhp)
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
		int num = 0;
		if (num != 0)
		{
			goto IL_0081;
		}
		goto IL_0146;
		IL_0081:
		switch (hdrmu)
		{
		case ndkio.fhwwk.sfpjy:
			num += dlfxz(p0, ref p1, ref p2);
			if (iyniq == 0 || 1 == 0)
			{
				hdrmu = ndkio.fhwwk.iozqt;
			}
			break;
		case ndkio.fhwwk.iozqt:
		{
			int num2 = tjjov.zohfz(p0, p1, p2);
			num += num2;
			if (tjjov.lotbz == yosfy.aljno)
			{
				p1 += num2;
				p2 -= num2;
				lhiyx();
				hdrmu = ndkio.fhwwk.ynurq;
				break;
			}
			rsmoq = tjjov.lotbz;
			return num;
		}
		case ndkio.fhwwk.ynurq:
			num += dlfxz(p0, ref p1, ref p2);
			if (iyniq == 0 || 1 == 0)
			{
				rsmoq = yosfy.aljno;
			}
			return num;
		}
		goto IL_0146;
		IL_0146:
		if (p2 > 0)
		{
			goto IL_0081;
		}
		return num;
	}

	public int pmhat()
	{
		if (rsmoq != yosfy.aljno)
		{
			throw new InvalidOperationException("Finish method can be called only when State is CompressorState.Finish.");
		}
		bzsvd = null;
		sqgag.Reset();
		bvavn = 0u;
		hdrmu = ndkio.fhwwk.sfpjy;
		rsmoq = yosfy.drxjq;
		lvkjm = false;
		return tjjov.pmhat();
	}

	public void mwvqu(Stream p0)
	{
		if (hdrmu != ndkio.fhwwk.sfpjy && 0 == 0)
		{
			throw new InvalidOperationException("WriteHeader method can be called only when header was not flushed already.");
		}
		if (!lvkjm || 1 == 0)
		{
			jodvn();
		}
		p0.Write(bzsvd, jsiwx, iyniq);
		iyniq = 0;
		hdrmu = ndkio.fhwwk.iozqt;
	}
}
