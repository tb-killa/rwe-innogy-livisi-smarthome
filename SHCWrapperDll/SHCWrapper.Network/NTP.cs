namespace SHCWrapper.Network;

public static class NTP
{
	public static bool Start()
	{
		return PrivateWrapper.StartSNTPService();
	}

	public static bool Stop()
	{
		return PrivateWrapper.StopSNTPService();
	}
}
