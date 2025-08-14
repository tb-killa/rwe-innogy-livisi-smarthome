using System.Collections;
using Rebex;

namespace onrkn;

internal class hpmsr : xlbfv
{
	private readonly Hashtable uoebg;

	private readonly ArrayList unnez;

	public hpmsr()
	{
		uoebg = new Hashtable();
		unnez = new ArrayList();
	}

	public string[] zzcnh()
	{
		return (string[])unnez.ToArray(typeof(string));
	}

	protected override void iabst(byte[] p0, int p1, int p2, bool p3)
	{
		if (p3 && 0 == 0 && p2 > 3 && p0[p1] == 45 && p0[p1 + 1] == 45)
		{
			if (p2 > 80)
			{
				p2 = 80;
			}
			p2 -= 2;
			string text = EncodingTools.Default.GetString(p0, p1 + 2, p2 - 2);
			if (!uoebg.ContainsKey(text) || 1 == 0)
			{
				uoebg[text] = typeof(object);
				unnez.Add(text);
			}
		}
	}
}
