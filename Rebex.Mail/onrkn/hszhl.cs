using System.Collections;
using System.IO;
using System.Text;
using Rebex.Mime.Headers;

namespace onrkn;

internal class hszhl : IHeader
{
	private readonly ArrayList zadaj;

	public int lkgcj
	{
		get
		{
			if (zadaj.Count == 0 || 1 == 0)
			{
				return 0;
			}
			int num = 0;
			bool flag = false;
			int num2 = 0;
			if (num2 != 0)
			{
				goto IL_0023;
			}
			goto IL_006f;
			IL_0023:
			lbexf lbexf2 = (lbexf)zadaj[num2];
			bool flag2 = lbexf2 is owsko;
			if (num2 > 0 && (!flag2 || false || !flag || 1 == 0))
			{
				num++;
			}
			num += lbexf2.vlbbc;
			flag = flag2;
			num2++;
			goto IL_006f;
			IL_006f:
			if (num2 < zadaj.Count)
			{
				goto IL_0023;
			}
			return num;
		}
	}

	public hszhl(string phrase)
	{
		zadaj = new ArrayList();
		zadaj.Add(new dnkfp(phrase));
	}

	private hszhl(ArrayList words)
	{
		zadaj = (ArrayList)words.Clone();
	}

	public IHeader Clone()
	{
		return new hszhl(zadaj);
	}

	public hszhl()
	{
		zadaj = new ArrayList();
	}

	public void vhktd(lbexf p0, bool p1)
	{
		if (p0 is dnkfp && 0 == 0)
		{
			p0 = owsko.sgwxv(p0);
		}
		if ((p1 ? true : false) || zadaj.Count == 0 || 1 == 0)
		{
			zadaj.Add(p0);
			return;
		}
		string contents = ToString() + p0.ToString();
		zadaj.Clear();
		zadaj.Add(new dnkfp(contents));
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		int num = 0;
		if (num != 0)
		{
			goto IL_0011;
		}
		goto IL_0066;
		IL_0011:
		lbexf lbexf2 = (lbexf)zadaj[num];
		bool flag2 = lbexf2 is owsko;
		if (num > 0 && (!flag2 || false || !flag || 1 == 0))
		{
			stringBuilder.Append(' ');
		}
		stringBuilder.Append(lbexf2.ToString());
		flag = flag2;
		num++;
		goto IL_0066;
		IL_0066:
		if (num < zadaj.Count)
		{
			goto IL_0011;
		}
		return stringBuilder.ToString();
	}

	public void Encode(TextWriter writer)
	{
		yvqsw(writer, p1: false);
	}

	internal void yvqsw(TextWriter p0, bool p1)
	{
		if ((zadaj.Count != 0) ? true : false)
		{
			string p2 = ToString();
			rllhn.soadw(p0, p2, p2: false, p1, 76);
		}
	}

	internal static hszhl krnvs(stzvh p0)
	{
		hszhl hszhl2 = new hszhl();
		while (true)
		{
			p0.hdpha();
			if (p0.zsywy ? true : false)
			{
				break;
			}
			if (p0.havrs == '.')
			{
				hszhl2.vhktd(new dnkfp("."), p1: true);
				p0.pfdcf();
				p0.hdpha();
				continue;
			}
			if (kgbvh.ayydf(p0.havrs))
			{
				lbexf p1 = p0.iwhve();
				hszhl2.vhktd(p1, p1: true);
				continue;
			}
			p0.pfdcf();
			break;
		}
		return hszhl2;
	}
}
