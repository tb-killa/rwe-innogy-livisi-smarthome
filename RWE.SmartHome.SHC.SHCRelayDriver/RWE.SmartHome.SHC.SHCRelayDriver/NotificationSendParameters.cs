namespace RWE.SmartHome.SHC.SHCRelayDriver;

internal class NotificationSendParameters
{
	internal RelayDriver Driver { get; private set; }

	internal int SendDelay { get; private set; }

	internal NotificationSendParameters(RelayDriver aDriver, int aDelay)
	{
		Driver = aDriver;
		SendDelay = aDelay;
	}
}
