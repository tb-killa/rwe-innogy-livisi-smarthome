using System.IO;

namespace onrkn;

internal struct hmvwx
{
	private readonly string pruow;

	private readonly string utdqa;

	private readonly evdac gibms;

	private string fqqkc;

	public string zvrbf => pruow;

	public string grqde
	{
		get
		{
			string text = fqqkc;
			if (text == null || 1 == 0)
			{
				string text2 = pruow;
				if (text2.StartsWith("/") && 0 == 0)
				{
					text2 = text2.Substring(1);
				}
				text = Path.Combine(utdqa, text2);
				text = (fqqkc = text.Replace(gibms.uyupw, gibms.xwsda));
			}
			return text;
		}
	}

	public hmvwx(string path, string virtualRoot, evdac info)
	{
		pruow = path;
		utdqa = virtualRoot;
		gibms = info;
		fqqkc = null;
	}
}
