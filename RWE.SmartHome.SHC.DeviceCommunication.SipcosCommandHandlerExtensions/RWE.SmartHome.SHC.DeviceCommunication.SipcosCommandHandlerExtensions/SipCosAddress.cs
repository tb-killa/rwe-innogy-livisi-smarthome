namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public static class SipCosAddress
{
	public static readonly byte[] InvalidAddress;

	public static readonly byte[] LocalLoopback;

	public static readonly byte[] AnyRouter;

	public static readonly byte[] AnySnc;

	public static readonly byte[] AllDevices;

	public static readonly byte[] AllRouters;

	public static readonly byte[] AllThermostats;

	public static readonly byte[] AllMainsPoweredDevices;

	public static readonly byte[] AllBurstListeningDevices;

	public static readonly byte[] AllTripleBurstListeningDevices;

	static SipCosAddress()
	{
		byte[] invalidAddress = new byte[3];
		InvalidAddress = invalidAddress;
		LocalLoopback = new byte[3] { 0, 0, 1 };
		AnyRouter = new byte[3] { 224, 0, 2 };
		AnySnc = new byte[3] { 224, 0, 3 };
		AllDevices = new byte[3] { 240, 0, 1 };
		AllRouters = new byte[3] { 240, 0, 2 };
		AllThermostats = new byte[3] { 240, 0, 3 };
		AllMainsPoweredDevices = new byte[3] { 240, 0, 4 };
		AllBurstListeningDevices = new byte[3] { 240, 0, 5 };
		AllTripleBurstListeningDevices = new byte[3] { 240, 0, 6 };
	}
}
