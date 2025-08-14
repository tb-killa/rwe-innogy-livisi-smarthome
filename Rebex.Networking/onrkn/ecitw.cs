using System;
using System.Text;

namespace onrkn;

internal class ecitw
{
	public static string cbyuh(string p0, string p1)
	{
		if (string.IsNullOrEmpty(p1) && 0 == 0)
		{
			throw new ArgumentException("Access token cannot be null or empty.", "password");
		}
		if (string.IsNullOrEmpty(p0) && 0 == 0)
		{
			return p1;
		}
		string s = brgjd.edcru("user={0}{1}auth=Bearer {2}{1}{1}", p0, '\u0001', p1);
		return Convert.ToBase64String(Encoding.ASCII.GetBytes(s));
	}
}
