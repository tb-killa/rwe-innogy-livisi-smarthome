using System.Text.RegularExpressions;
using Rebex.Net;

namespace onrkn;

internal sealed class dqids
{
	private dqids()
	{
	}

	public static string tjhiv(SmtpResponse p0)
	{
		if (p0.Raw == null || 1 == 0)
		{
			return null;
		}
		string pattern = brgjd.edcru("(?:^{0}(?:-{1}| )|[ \t\r]*$)", p0.Code, "");
		Regex regex = new Regex(pattern, RegexOptions.Multiline);
		return regex.Replace(p0.Raw, "");
	}
}
