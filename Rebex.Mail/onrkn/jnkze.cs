using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace onrkn;

internal class jnkze
{
	public static readonly Guid fowlc = new Guid("00020820-0000-0000-C000-000000000046");

	public static readonly Guid tvukv = new Guid("00020906-0000-0000-C000-000000000046");

	public static readonly Guid wiftm = new Guid("00020D0B-0000-0000-C000-000000000046");

	private byte[] gsumo;

	private List<jnkze> clmhi;

	private jnkze[] xpwep;

	private string glmja;

	private string hvmqg;

	private Guid lrsos;

	private kbosi sswjj;

	private int clsef;

	private int vuhuc;

	private int snuay;

	private int rhsrf;

	private long pjady;

	private xxolr pslfm;

	private lofzx uochn;

	private ptpnz wffxl;

	public string dclzl => hvmqg;

	public string ucwew => glmja;

	public long obvof => pjady;

	public bool qrqby => sswjj == kbosi.fcjwm;

	public bool msfii => clmhi != null;

	public Guid fqtok
	{
		get
		{
			return lrsos;
		}
		set
		{
			if (qrqby && 0 == 0)
			{
				throw new InvalidOperationException("Cannot set CLSID to file item.");
			}
			lrsos = value;
		}
	}

	internal int ikomv => clsef;

	internal int awjpu => vuhuc;

	internal int hucha => snuay;

	internal int gvryk => rhsrf;

	internal ptpnz hcsdt => wffxl;

	internal List<jnkze> ucvon => clmhi;

	internal xxolr edlkl => pslfm;

	internal xxolr gqqsr
	{
		get
		{
			if (pslfm.CanRead && 0 == 0)
			{
				return pslfm;
			}
			if (uochn == null || 1 == 0)
			{
				uochn = new lofzx(pslfm.ucrvs, pslfm.zqmsy, pslfm.Length, pslfm.wbedc);
			}
			return uochn;
		}
	}

	internal static jnkze ilurr(duqmg p0)
	{
		jnkze jnkze2 = dmvtn(p0, "Root Entry");
		jnkze2.sswjj = kbosi.udwax;
		jnkze2.glmja = string.Empty;
		jnkze2.pslfm = new xnxbw(p0, isShortStream: false);
		return jnkze2;
	}

	internal static jnkze dmvtn(duqmg p0, string p1)
	{
		return new jnkze(p0, p1, isDirectory: true, 0L);
	}

	internal static jnkze icssj(duqmg p0, string p1, long p2)
	{
		return new jnkze(p0, p1, isDirectory: false, p2);
	}

	private jnkze(duqmg owner, string path, bool isDirectory, long length)
	{
		if (isDirectory && 0 == 0)
		{
			sswjj = kbosi.avoza;
			clmhi = new List<jnkze>();
		}
		else
		{
			sswjj = kbosi.fcjwm;
			pslfm = new xnxbw(owner, length < owner.imrfe);
		}
		hvmqg = jxtqv.briaq(path);
		glmja = path;
		pjady = length;
		if (hvmqg.Length > 31)
		{
			throw new InvalidOperationException(brgjd.edcru("Name of the item {0} [from path: {1}] is too long (max. length is 31).", hvmqg, path));
		}
		wffxl = new ptpnz(this, owner.kuwso.Count);
		owner.kuwso.Add(this);
		owner.vsjhd.Add(path, this);
	}

	internal jnkze(zypui owner, lofzx reader, int dirId, string basePath)
	{
		bool flag = dirId == 0;
		reader.Position = dirId << 7;
		gsumo = new byte[64];
		reader.hxmiq(gsumo, 0, 64);
		int num = reader.ecwzy();
		if (num < 2 || num > 64 || (num & 1) == 1)
		{
			throw new uwkib("Invalid Name length.");
		}
		hvmqg = Encoding.Unicode.GetString(gsumo, 0, num - 2);
		gsumo = null;
		if (flag && 0 == 0)
		{
			glmja = string.Empty;
		}
		else
		{
			glmja = jxtqv.motnn(basePath, hvmqg);
		}
		int num2 = (int)(sswjj = (kbosi)reader.ReadByte());
		if (num2 < 0 || num2 > 5 || (flag && 0 == 0 && sswjj != kbosi.udwax) || ((!flag || 1 == 0) && sswjj == kbosi.udwax))
		{
			throw new uwkib("Invalid Directory entry type.");
		}
		reader.ReadByte();
		clsef = reader.wfljb();
		vuhuc = reader.wfljb();
		snuay = reader.wfljb();
		if (clsef < -1 || vuhuc < -1 || snuay < -1)
		{
			throw new uwkib("Invalid Directory ID.");
		}
		switch (sswjj)
		{
		case kbosi.udwax:
			if (clsef != -1 || vuhuc != -1 || snuay == -1)
			{
				throw new uwkib("Invalid Directory ID in the Root element.");
			}
			clmhi = new List<jnkze>();
			break;
		case kbosi.avoza:
			clmhi = new List<jnkze>();
			break;
		default:
			if (snuay != -1)
			{
				throw new uwkib("Invalid Directory ID for non-storage element.");
			}
			break;
		}
		if (qrqby && 0 == 0)
		{
			reader.Seek(16L, SeekOrigin.Current);
		}
		else
		{
			gsumo = new byte[16];
			reader.hvees(gsumo);
			lrsos = new Guid(gsumo);
			gsumo = null;
		}
		reader.Seek(20L, SeekOrigin.Current);
		rhsrf = reader.wfljb();
		pjady = reader.pfeoc();
		kbosi kbosi2 = sswjj;
		if (kbosi2 == kbosi.fcjwm || kbosi2 == kbosi.udwax)
		{
			if (rhsrf < 0 && pjady > 0)
			{
				throw new uwkib("Invalid Sector ID definition.");
			}
			if (sswjj == kbosi.udwax)
			{
				pslfm = new lofzx(owner, rhsrf, pjady, isShortStream: false);
			}
			else
			{
				pslfm = new lofzx(owner, rhsrf, pjady, pjady < owner.imrfe);
			}
		}
		else
		{
			if (rhsrf > 0)
			{
				throw new uwkib("Invalid Sector ID definition.");
			}
			if (pjady != 0)
			{
				throw new uwkib("Invalid Length definition.");
			}
		}
	}

	internal void tdhod(xnxbw p0)
	{
		p0.Position = wffxl.neojh << 7;
		byte[] bytes = Encoding.Unicode.GetBytes(hvmqg);
		p0.Write(bytes, 0, bytes.Length);
		p0.jxrks(64 - bytes.Length);
		p0.ttjsz((ushort)(bytes.Length + 2));
		p0.wukmn((byte)sswjj);
		p0.wukmn(1);
		p0.fptyf((wffxl.txwje == null) ? (-1) : wffxl.txwje.neojh);
		p0.fptyf((wffxl.vaabt == null) ? (-1) : wffxl.vaabt.neojh);
		p0.fptyf((wffxl.ykbcs == null) ? (-1) : wffxl.ykbcs.neojh);
		if (!qrqby || 1 == 0)
		{
			p0.Write(fqtok.ToByteArray(), 0, 16);
		}
		else
		{
			p0.jxrks(16);
		}
		p0.jxrks(20);
		switch (sswjj)
		{
		case kbosi.avoza:
			p0.fptyf(0);
			p0.guutg(0L);
			break;
		case kbosi.fcjwm:
			p0.fptyf(pslfm.zqmsy);
			p0.guutg(pslfm.Length);
			break;
		case kbosi.udwax:
		{
			p0.fptyf(pslfm.zqmsy);
			int num = (int)pslfm.Length & (pslfm.ucrvs.mhpfn - 1);
			if (num == 0 || 1 == 0)
			{
				p0.guutg(pslfm.Length);
			}
			else
			{
				p0.guutg(pslfm.Length + pslfm.ucrvs.mhpfn - num);
			}
			break;
		}
		default:
			throw new InvalidOperationException("Unknown item type.");
		}
	}

	internal static void ffcqb(xnxbw p0)
	{
		p0.jxrks(68);
		p0.fmgjd(12);
		p0.jxrks(48);
	}

	internal void jfvgo(jnkze p0)
	{
		int num = clmhi.BinarySearch(p0, daaba.wtjus);
		if (num < 0)
		{
			clmhi.Insert(~num, p0);
			return;
		}
		throw new uwkib(brgjd.edcru("Invalid or corrupted file (multiple occurrence of item '{0}').", p0.ucwew));
	}

	public xxolr mzhde()
	{
		if (sswjj != kbosi.fcjwm)
		{
			throw new InvalidOperationException("Item is not a file.");
		}
		gqqsr.Position = 0L;
		return gqqsr;
	}

	public jnkze[] hmtsk()
	{
		if (clmhi == null || 1 == 0)
		{
			throw new InvalidOperationException("Item is not a directory.");
		}
		if (xpwep == null || false || xpwep.Length != clmhi.Count)
		{
			xpwep = clmhi.ToArray();
		}
		return xpwep;
	}

	public override string ToString()
	{
		return brgjd.edcru("{0} ({2}) [{1}]", glmja, pjady, sswjj);
	}
}
