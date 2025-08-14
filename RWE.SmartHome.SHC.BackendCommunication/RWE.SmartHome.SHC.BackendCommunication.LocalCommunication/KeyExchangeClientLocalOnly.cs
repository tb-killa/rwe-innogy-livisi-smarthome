using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.LocalCommunication;

internal class KeyExchangeClientLocalOnly : IKeyExchangeClient
{
	public KeyExchangeResult EncryptNetworkKey(string certificateThumbprint, byte[] sgtin, byte[] secNumber, byte[] encOnceNetworkKey, string deviceFirmwareVersion, out byte[] encTwiceNetworkKey, out byte[] keyAuthentication)
	{
		encTwiceNetworkKey = null;
		keyAuthentication = null;
		return KeyExchangeResult.DeviceNotFound;
	}

	public KeyExchangeResult GetDeviceKey(string certificateThumbprint, byte[] sgtin, out byte[] deviceKey)
	{
		deviceKey = null;
		return KeyExchangeResult.DeviceNotFound;
	}

	public KeyExchangeResult GetMasterKey(string certificateThumbprint, out string masterKey)
	{
		masterKey = null;
		return KeyExchangeResult.DeviceNotFound;
	}

	public KeyExchangeResult GetDevicesKeys(string certificateThumbprint, byte[][] sgtins, out ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary[] deviceKeys)
	{
		deviceKeys = null;
		return KeyExchangeResult.DeviceNotFound;
	}
}
