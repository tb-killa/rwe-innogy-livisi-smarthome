using System;
using System.Text;

namespace SHCWrapper.Misc;

public static class SGTIN
{
	public static string GetSGTIN()
	{
		StringBuilder stringBuilder = new StringBuilder(26);
		if (Environment.OSVersion.Platform == PlatformID.WinCE && PrivateWrapper.GetSGTIN(stringBuilder, stringBuilder.Capacity))
		{
			return stringBuilder.ToString();
		}
		return string.Empty;
	}
}
