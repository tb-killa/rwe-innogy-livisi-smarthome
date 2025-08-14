using System;
using System.IO;
using Rebex.OutlookMessages;

namespace onrkn;

internal class jjvcf : upend<wyfgf>
{
	internal jjvcf(jfxnb owner)
		: base(owner)
	{
	}

	public wyfgf kjlms(jfxnb p0)
	{
		return rgcby(p0, null);
	}

	public wyfgf rgcby(jfxnb p0, string p1)
	{
		return oaesx(new wyfgf(base.bpotr, p0, base.cdyle.Count, p1));
	}

	public wyfgf oxxkc(string p0)
	{
		return zyhax(p0, null);
	}

	public wyfgf zyhax(string p0, string p1)
	{
		FileStream fileStream = File.OpenRead(p0);
		try
		{
			return kppar(fileStream, Path.GetFileName(p0), p1);
		}
		finally
		{
			if (fileStream != null && 0 == 0)
			{
				((IDisposable)fileStream).Dispose();
			}
		}
	}

	public wyfgf qhqnv(Stream p0)
	{
		string p1 = vtdxm.zkrhb(p0);
		return kppar(p0, p1, null);
	}

	public wyfgf kppar(Stream p0, string p1, string p2)
	{
		return oaesx(new wyfgf(base.bpotr, p0, base.cdyle.Count, p1, p2));
	}

	public wyfgf aqffd(Stream p0, string p1, string p2, string p3, string p4, string p5)
	{
		return oaesx(new wyfgf(base.bpotr, p0, base.cdyle.Count, p1, p2, p3, p4, p5));
	}

	public wyfgf derde(Stream p0, string p1, string p2, string p3, string p4, string p5, string p6)
	{
		return oaesx(new wyfgf(base.bpotr, p0, base.cdyle.Count, p1, p2, p3, p4, p5, p6));
	}

	public wyfgf jgmtn(Stream p0, string p1, string p2, string p3, string p4)
	{
		wyfgf wyfgf2 = new wyfgf(base.bpotr, p0, base.cdyle.Count, p1, null, p2, p3, p4);
		wyfgf2.vbjpq.dossp(MsgPropertyTag.AttachmentHidden, p1: true);
		return oaesx(wyfgf2);
	}

	public wyfgf ihdlz(Stream p0, string p1, string p2, string p3, string p4, string p5)
	{
		wyfgf wyfgf2 = new wyfgf(base.bpotr, p0, base.cdyle.Count, p1, null, p2, p3, p4, p5);
		wyfgf2.vbjpq.dossp(MsgPropertyTag.AttachmentHidden, p1: true);
		return oaesx(wyfgf2);
	}
}
