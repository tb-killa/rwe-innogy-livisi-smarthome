namespace RWE.SmartHome.SHC.WebSocketsService.Util;

public static class StringUtil
{
	public static bool IsNullOrEmpty(string value)
	{
		if (value != null && value.Length != 0)
		{
			return false;
		}
		return true;
	}
}
