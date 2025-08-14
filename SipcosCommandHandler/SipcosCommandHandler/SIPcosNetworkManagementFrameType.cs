namespace SipcosCommandHandler;

public enum SIPcosNetworkManagementFrameType : byte
{
	DeviceInfoFrame,
	ForwardedDeviceInfoFrame,
	NetworkInfoFrame,
	ForwardedNetworkInfoFrame,
	NetworkAcceptFrame,
	ForwardedNetworkAcceptFrame,
	NetworkExcludeFrame
}
