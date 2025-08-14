using System;
using System.IO;
using System.Text;
using onrkn;

namespace Rebex.Mime.Headers;

public class MailAddress : IHeader
{
	private readonly mqucj nujfg;

	private readonly hszhl ftvox;

	private readonly bool dwkgg;

	private readonly bool ndvbb;

	internal bool hnfcn => dwkgg;

	internal bool nwayk
	{
		get
		{
			if (ftvox == null || 1 == 0)
			{
				return nujfg.tgwcz;
			}
			return false;
		}
	}

	public string DisplayName
	{
		get
		{
			if (ftvox == null || 1 == 0)
			{
				return "";
			}
			return ftvox.ToString();
		}
	}

	public string Address => nujfg.ToString();

	public string User
	{
		get
		{
			if (nujfg.vffsa == null || 1 == 0)
			{
				return "";
			}
			return nujfg.vffsa;
		}
	}

	public string Host
	{
		get
		{
			if (nujfg.eeaqt == null || 1 == 0)
			{
				return "";
			}
			return nujfg.eeaqt;
		}
	}

	internal mqucj qqrmz => nujfg;

	public MailAddress(string address)
		: this(address, omitBrackets: false)
	{
	}

	public MailAddress(string address, bool omitBrackets)
	{
		ndvbb = omitBrackets;
		if (address == null || false || address.Length == 0 || 1 == 0)
		{
			nujfg = mqucj.veixg;
		}
		else
		{
			hxoyc(new stzvh(address), out nujfg, out ftvox, out dwkgg);
		}
	}

	public MailAddress(string address, string displayName)
	{
		if (address == null || false || address.Length == 0 || 1 == 0)
		{
			nujfg = mqucj.veixg;
		}
		else if ((!address.EndsWith(":;") || 1 == 0) && (!address.EndsWith(":") || 1 == 0))
		{
			nujfg = mqucj.xdgws(new stzvh(address));
			if (nujfg == null || 1 == 0)
			{
				nujfg = mqucj.veixg;
			}
		}
		else
		{
			if (address.EndsWith(":") && 0 == 0)
			{
				address += ";";
			}
			nujfg = new mqucj(address, null);
			dwkgg = true;
		}
		if (displayName != null && 0 == 0 && displayName.Length > 0)
		{
			ftvox = new hszhl(displayName);
		}
	}

	private MailAddress(mqucj address, hszhl displayName, bool group, bool omitAddressBrackets)
	{
		nujfg = address;
		ftvox = displayName;
		dwkgg = group;
		ndvbb = omitAddressBrackets;
	}

	public static implicit operator MailAddress(string address)
	{
		if (address == null || 1 == 0)
		{
			throw new ArgumentNullException("address");
		}
		return new MailAddress(address);
	}

	public IHeader Clone()
	{
		return new MailAddress(nujfg, ftvox, dwkgg, ndvbb);
	}

	private static string viohz(string p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0037;
		IL_0006:
		char c = p0[num];
		if (c <= '\u007f' && c != ' ' && (!kgbvh.ynmbt(c) || 1 == 0))
		{
			return kgbvh.rqlyl(p0, '"', '"');
		}
		num++;
		goto IL_0037;
		IL_0037:
		if (num < p0.Length)
		{
			goto IL_0006;
		}
		return p0;
	}

	public override string ToString()
	{
		if (dwkgg && 0 == 0)
		{
			return nujfg.ToString();
		}
		if (ftvox == null || 1 == 0)
		{
			if (nujfg.tgwcz && 0 == 0)
			{
				return "<>";
			}
			return nujfg.ToString();
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(viohz(ftvox.ToString()));
		if (!nujfg.tgwcz || 1 == 0)
		{
			stringBuilder.Append(" <");
			stringBuilder.Append(nujfg.ToString());
			stringBuilder.Append('>');
		}
		return stringBuilder.ToString();
	}

	public void Encode(TextWriter writer)
	{
		if (writer == null || 1 == 0)
		{
			throw new ArgumentNullException("writer");
		}
		if (dwkgg && 0 == 0)
		{
			nujfg.cygyr(writer, p1: false);
			return;
		}
		if (ftvox == null || 1 == 0)
		{
			nujfg.cygyr(writer, !ndvbb);
			return;
		}
		ftvox.yvqsw(writer, p1: true);
		if (!nujfg.tgwcz || 1 == 0)
		{
			rllhn.btprl(writer);
			nujfg.cygyr(writer, p1: true);
		}
	}

	private static void hxoyc(stzvh p0, out mqucj p1, out hszhl p2, out bool p3)
	{
		p2 = null;
		p3 = false;
		hszhl hszhl = new hszhl();
		hszhl hszhl2 = null;
		p0.hdpha();
		int bauax = p0.bauax;
		int num = 0;
		if (num != 0)
		{
			goto IL_002d;
		}
		goto IL_020c;
		IL_002d:
		if (p0.havrs == '<')
		{
			p1 = mqucj.xdgws(p0);
			if (p1 == null || 1 == 0)
			{
				p1 = mqucj.veixg;
			}
			if (hszhl.lkgcj > 0)
			{
				p2 = hszhl;
			}
			return;
		}
		if (p0.havrs != '@')
		{
			if (p0.havrs == ';')
			{
				if (hszhl2 != null && 0 == 0)
				{
					string text = hszhl2.ToString();
					if (text.Length > 0)
					{
						p1 = new mqucj(text + ":;", null);
						p3 = true;
					}
					else
					{
						p1 = mqucj.veixg;
					}
				}
				else
				{
					string text2 = hszhl.ToString();
					if (text2.Length > 0)
					{
						p1 = new mqucj(text2, null);
					}
					else
					{
						p1 = mqucj.veixg;
					}
				}
				return;
			}
			if (p0.havrs == ',')
			{
				p0.hdpha(',');
				p1 = mqucj.veixg;
				if (hszhl.lkgcj > 0)
				{
					p2 = hszhl;
				}
				return;
			}
			if (p0.havrs == ':')
			{
				p0.hdpha(':');
				hszhl2 = hszhl;
				hszhl = new hszhl();
			}
			else
			{
				int bauax2 = p0.bauax;
				if ((!kgbvh.ayydf(p0.havrs) || 1 == 0) && p0.havrs < '\u0080')
				{
					hszhl = (((hszhl.lkgcj != 0) ? true : false) ? ((num == bauax2) ? new hszhl(hszhl.ToString() + p0.pfdcf()) : new hszhl(hszhl.ToString() + ' ' + p0.pfdcf())) : new hszhl(p0.pfdcf().ToString()));
				}
				else
				{
					lbexf p4 = p0.iwhve();
					if (num != bauax2)
					{
						hszhl.vhktd(p4, p1: true);
					}
					else
					{
						hszhl.vhktd(p4, p1: false);
					}
				}
				num = p0.bauax;
				p0.hdpha();
			}
			goto IL_020c;
		}
		goto IL_021f;
		IL_020c:
		if (!p0.zsywy)
		{
			goto IL_002d;
		}
		goto IL_021f;
		IL_021f:
		if (p0.zsywy && 0 == 0)
		{
			p1 = null;
		}
		else
		{
			hszhl = new hszhl();
			p0.bauax = bauax;
			p1 = mqucj.xdgws(p0);
			if (!p0.zsywy || 1 == 0)
			{
				if (p0.havrs == '<')
				{
					mqucj mqucj = mqucj.xdgws(p0);
					if (mqucj != null && 0 == 0)
					{
						if (p1 != null && 0 == 0)
						{
							hszhl = new hszhl(p1.ToString());
						}
						p1 = mqucj;
					}
				}
				else if (p0.havrs == '(')
				{
					string text3 = p0.vgeze();
					text3 = text3.Trim(' ', '(', ')');
					hszhl.vhktd(owsko.sgwxv(new dnkfp(text3)), p1: true);
				}
			}
		}
		if (p1 == null || 1 == 0)
		{
			p1 = mqucj.veixg;
		}
		if (hszhl.lkgcj > 0)
		{
			p2 = hszhl;
		}
	}

	internal MailAddress(stzvh reader)
	{
		hxoyc(reader, out nujfg, out ftvox, out dwkgg);
	}

	internal static IHeader kemeh(stzvh p0)
	{
		return new MailAddress(p0);
	}
}
