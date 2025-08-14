using System.CodeDom.Compiler;

namespace SmartHome.SHC.BackendCommunication.KeyExchangeScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public interface IKeyExchangeService
{
	KeyExchangeResult EncryptNetworkKey(byte[] sgtin, byte[] secNumber, byte[] encOnceNetworkKey, string deviceFirmwareVersion, out byte[] encTwiceNetworkKey, out byte[] keyAuthentication);

	KeyExchangeResult GetDeviceKey(byte[] sgtin, out byte[] deviceKey);

	KeyExchangeResult GetMasterKey(out string masterKey);

	KeyExchangeResult GetDevicesKeys(byte[][] sgtins, out ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary[] devicesKeys);
}
