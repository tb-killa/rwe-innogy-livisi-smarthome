using System;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Interfaces;

internal interface IWMBusManager
{
	IDeviceList DeviceList { get; }

	void IncludeDevice(Guid deviceId, byte[] decryptionKey);

	void ExcludeDevice(Guid deviceId);
}
