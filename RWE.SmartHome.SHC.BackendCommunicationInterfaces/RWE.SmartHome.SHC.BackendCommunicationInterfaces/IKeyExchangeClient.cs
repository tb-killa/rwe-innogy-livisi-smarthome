using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces;

public interface IKeyExchangeClient
{
	KeyExchangeResult EncryptNetworkKey(string certificateThumbprint, byte[] sgtin, byte[] secNumber, byte[] encOnceNetworkKey, string deviceFirmwareVersion, out byte[] encTwiceNetworkKey, out byte[] keyAuthentication);

	KeyExchangeResult GetDeviceKey(string certificateThumbprint, byte[] sgtin, out byte[] deviceKey);

	KeyExchangeResult GetMasterKey(string certificateThumbprint, out string masterKey);

	KeyExchangeResult GetDevicesKeys(string certificateThumbprint, byte[][] sgtins, out ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary[] deviceKeys);
}
