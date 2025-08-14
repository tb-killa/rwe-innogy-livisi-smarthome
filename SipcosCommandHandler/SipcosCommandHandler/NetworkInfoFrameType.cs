namespace SipcosCommandHandler;

public enum NetworkInfoFrameType : byte
{
	AddressCollision,
	RequestedNetworkPropertiesNotSupported,
	NetworkAcceptPending,
	NetworkAcceptDenied
}
