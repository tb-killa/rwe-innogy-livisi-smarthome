using System.Text;
using Rebex;

namespace onrkn;

internal class vesyi : jcgcz
{
	private Encoding zomav;

	private string xfbrh;

	public string dcokg => xfbrh;

	public vesyi(rmkkr type, byte[] data)
		: base(type, data)
	{
	}

	public vesyi(rmkkr type, Encoding enc, string value)
		: base(type, enc.GetBytes(value))
	{
		zomav = enc;
		xfbrh = value;
	}

	public vesyi()
	{
	}

	public override void zkxnk(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.ssvuz, p0, p1);
		switch (p0)
		{
		case rmkkr.jgutu:
		case rmkkr.dzwiy:
			zomav = EncodingTools.ASCII;
			break;
		case rmkkr.pcxmz:
			zomav = Encoding.BigEndianUnicode;
			break;
		case rmkkr.xiwym:
			zomav = Encoding.UTF8;
			break;
		case rmkkr.lojrb:
			zomav = EncodingTools.GetEncoding("iso-8859-1");
			break;
		default:
			zomav = null;
			break;
		}
		base.zkxnk(p0, p1, p2);
	}

	public override void somzq()
	{
		base.somzq();
		if (zomav != null && 0 == 0)
		{
			xfbrh = zomav.GetString(base.rtrhq, 0, base.rtrhq.Length);
		}
	}

	public override string ToString()
	{
		if (xfbrh == null || 1 == 0)
		{
			return "";
		}
		return xfbrh;
	}

	public static vesyi onwas(byte[] p0)
	{
		vesyi vesyi2 = new vesyi();
		hfnnn.oalpn(vesyi2, p0, 0, p0.Length);
		return vesyi2;
	}
}
