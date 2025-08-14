using System;
using Rebex.IO;

namespace onrkn;

internal class lspmt
{
	private const char zpzyc = '\uffff';

	private hyygw wijgn;

	private ijwiq vmkyq;

	private string ubiyi;

	private string inpeb;

	public bool blvgv;

	public bool nqhex;

	public bool iakqs;

	public int tnfhc;

	public ijwiq Item => vmkyq;

	public hyygw migcp => wijgn;

	public string rvimb => vmkyq.Path;

	public string nuttj
	{
		get
		{
			return ubiyi;
		}
		set
		{
			ubiyi = value;
		}
	}

	public string ovfjk => inpeb;

	public lspmt(ijwiq item, hyygw fileSet, string markedPath)
	{
		vmkyq = item;
		wijgn = fileSet;
		inpeb = markedPath;
	}

	public bool lcogx(char[] p0, FileSetMatchMode p1)
	{
		return wijgn.omemp(vmkyq.Path, p0, p1);
	}

	public void pgbfk(ijwiq p0)
	{
		vmkyq = p0;
		if (p0.IsLink && 0 == 0 && (inpeb == null || 1 == 0))
		{
			inpeb = p0.Path;
		}
	}

	public void okqrv(char[] p0)
	{
		inpeb = inpeb.TrimEnd(p0);
		if (inpeb.Length == 0 || false || inpeb[inpeb.Length - 1] != '\uffff')
		{
			inpeb += '\uffff';
		}
	}

	public bool sviua(char[] p0)
	{
		int num = brgjd.pkosy(inpeb, p0);
		if (num <= 0)
		{
			return false;
		}
		string value = inpeb.Substring(num);
		string text = inpeb.Substring(0, num);
		int num2 = text.LastIndexOf(value, StringComparison.Ordinal);
		if (num2 <= 0)
		{
			return false;
		}
		string text2 = text.Substring(num2);
		string text3 = text.Substring(0, num2);
		int num3 = text3.LastIndexOf(value, StringComparison.Ordinal);
		if (num3 < 0)
		{
			return false;
		}
		string text4 = text3.Substring(num3);
		return text2 == text4;
	}

	public override string ToString()
	{
		return vmkyq.Path;
	}
}
