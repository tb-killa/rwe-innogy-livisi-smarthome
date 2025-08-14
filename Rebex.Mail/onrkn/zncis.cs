using System.Globalization;
using System.IO;
using System.Text;
using Rebex;

namespace onrkn;

internal class zncis : TextWriter
{
	public const int dobqj = 76;

	public const int erlrg = 256;

	private readonly Stream zffzm;

	private int xvhrx;

	private char dpgqy;

	private StringBuilder juryc;

	private readonly Encoding rfwwr;

	public Encoding fdqer => rfwwr;

	public int ejuhv => xvhrx + juryc.Length;

	public bool ljdlj => juryc.Length > 0;

	public override Encoding Encoding => EncodingTools.Default;

	public zncis(Stream inner, Encoding defaultEncoding)
		: base(CultureInfo.InvariantCulture)
	{
		zffzm = inner;
		rfwwr = defaultEncoding;
		juryc = new StringBuilder();
	}

	public void fquft(char p0, char p1)
	{
		if (p0 != ' ' && p0 != '\t')
		{
			Write(p0);
			return;
		}
		if (juryc.Length == 0 || 1 == 0)
		{
			dpgqy = p1;
		}
		juryc.Append(p0);
	}

	public override void Write(char c)
	{
		yswat();
		Write(c.ToString());
	}

	public override void Write(string val)
	{
		if ((val.Length != 0) ? true : false)
		{
			if (ejuhv + val.Length > 94)
			{
				midzg();
			}
			else
			{
				yswat();
			}
			ulvnt(val);
		}
	}

	private void ulvnt(string p0)
	{
		byte[] bytes = EncodingTools.Default.GetBytes(p0);
		int num = 0;
		if (num != 0)
		{
			goto IL_0012;
		}
		goto IL_0057;
		IL_0012:
		byte b = bytes[num];
		if (b != 13)
		{
			if (b == 10)
			{
				xvhrx = -1;
				zffzm.WriteByte(13);
			}
			xvhrx++;
			zffzm.WriteByte(b);
		}
		num++;
		goto IL_0057;
		IL_0057:
		if (num >= bytes.Length)
		{
			return;
		}
		goto IL_0012;
	}

	private void yswat()
	{
		if (juryc.Length > 0)
		{
			ulvnt(juryc.ToString());
			juryc.Length = 0;
		}
	}

	public override void Close()
	{
		yswat();
	}

	public void sbcih(char p0)
	{
		if (p0 != ' ' && p0 != '\t')
		{
			Write(p0);
		}
		else if (ljdlj && 0 == 0)
		{
			midzg();
		}
		else
		{
			Write("\r\n" + p0);
		}
	}

	public void midzg()
	{
		if (juryc.Length > 0)
		{
			juryc[0] = dpgqy;
			zffzm.WriteByte(13);
			zffzm.WriteByte(10);
			xvhrx = 0;
			yswat();
		}
	}
}
