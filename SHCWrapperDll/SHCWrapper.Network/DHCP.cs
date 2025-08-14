namespace SHCWrapper.Network;

public static class DHCP
{
	public const string DEFAULT_INTERFACE_ADAPTER_NAME = "EMACB1";

	public const string KITL_INTERFACE_ADAPTER_NAME = "VMINI1";

	public static bool IpRenewDHCP()
	{
		return PrivateWrapper.DhcpRenew("EMACB1");
	}

	public static bool IpRenewDHCP(string adpaterName)
	{
		return PrivateWrapper.DhcpRenew(adpaterName);
	}
}
