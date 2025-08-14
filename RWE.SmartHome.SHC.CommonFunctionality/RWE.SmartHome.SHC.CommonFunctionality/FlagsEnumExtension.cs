namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class FlagsEnumExtension
{
	public static bool HasFlag(this UpdatePerformedStatus item, UpdatePerformedStatus flag)
	{
		return (item & flag) == flag;
	}
}
