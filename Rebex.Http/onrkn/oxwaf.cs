using System.Net;

namespace onrkn;

internal class oxwaf : pjyrs
{
	private readonly WebHeaderCollection nipfm;

	private readonly bool qvqhi;

	public oxwaf(WebHeaderCollection inner)
	{
		nipfm = inner;
		string[] allKeys = inner.AllKeys;
		int num = 0;
		if (num != 0)
		{
			goto IL_0016;
		}
		goto IL_002c;
		IL_0016:
		string text = allKeys[num];
		imhki(text, inner[text]);
		num++;
		goto IL_002c;
		IL_002c:
		if (num < allKeys.Length)
		{
			goto IL_0016;
		}
		qvqhi = true;
	}

	protected override void lfngx(string p0, string p1)
	{
		if (qvqhi ? true : false)
		{
			nipfm.Add(p0, p1);
		}
	}

	protected override void sohkq(string p0, string p1)
	{
		if (qvqhi ? true : false)
		{
			nipfm[p0] = p1;
		}
	}

	protected override void bkqck(string p0)
	{
		if (qvqhi ? true : false)
		{
			nipfm.Remove(p0);
		}
	}

	protected override void qpfkw()
	{
		if (qvqhi ? true : false)
		{
			nipfm.Clear();
		}
	}
}
