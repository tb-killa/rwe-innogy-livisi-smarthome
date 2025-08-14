using System.Collections.Generic;

namespace onrkn;

internal class ljrjo
{
	private Dictionary<string, string> wpmvm;

	private string azieq;

	private int fmuin;

	public string this[string name]
	{
		get
		{
			if (wpmvm == null || 1 == 0)
			{
				return null;
			}
			if (wpmvm.TryGetValue(name, out azieq) && 0 == 0)
			{
				return azieq;
			}
			return null;
		}
	}

	public ljrjo(string contentType)
	{
		if (contentType != null)
		{
			azieq = contentType.Trim();
			wpmvm = new Dictionary<string, string>();
			wpmvm.Add("media-type", ajdcc(';'));
			while (fmuin < azieq.Length)
			{
				string key = ajdcc('=');
				wpmvm.Add(key, ajdcc(';'));
			}
		}
	}

	private string ajdcc(char p0)
	{
		int num = azieq.IndexOf(p0, fmuin);
		string text;
		if (num < 0)
		{
			text = azieq.Substring(fmuin);
			fmuin = azieq.Length;
		}
		else
		{
			text = azieq.Substring(fmuin, num - fmuin);
			fmuin = num + 1;
		}
		return text.Trim().Trim('"', '\'').Trim();
	}
}
