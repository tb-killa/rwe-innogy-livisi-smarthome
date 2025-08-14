using System.Text;

namespace onrkn;

internal class zxixl : upend<oabdp>
{
	private StringBuilder iubon;

	internal zxixl(jfxnb owner)
		: base(owner)
	{
	}

	public oabdp kobgy(zccyb p0, string p1, string p2)
	{
		return tzsgr(p0, "SMTP", p1, p1, p2);
	}

	public oabdp tzsgr(zccyb p0, string p1, string p2, string p3, string p4)
	{
		return oaesx(new oabdp(base.bpotr, p0, p4, p3, p2, p1, base.cdyle.Count));
	}

	internal string plesp(zccyb p0)
	{
		if (iubon == null || 1 == 0)
		{
			iubon = new StringBuilder();
		}
		else
		{
			iubon.Length = 0;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0034;
		}
		goto IL_00c6;
		IL_0034:
		if (base.cdyle[num].sizjo == p0 && base.cdyle[num].ubimv != null && 0 == 0 && base.cdyle[num].ubimv.Length != 0 && 0 == 0)
		{
			if (iubon.Length > 0)
			{
				iubon.Append("; ");
			}
			iubon.Append(base.cdyle[num].ubimv);
		}
		num++;
		goto IL_00c6;
		IL_00c6:
		if (num < base.cdyle.Count)
		{
			goto IL_0034;
		}
		return iubon.ToString();
	}
}
