using System.Text;

namespace SHCWrapper.Misc;

public static class HWVersion
{
	public static string GetHWVersion()
	{
		StringBuilder stringBuilder = new StringBuilder(6);
		if (PrivateWrapper.GetHWVersion(stringBuilder, stringBuilder.Capacity))
		{
			return stringBuilder.ToString();
		}
		return string.Empty;
	}
}
