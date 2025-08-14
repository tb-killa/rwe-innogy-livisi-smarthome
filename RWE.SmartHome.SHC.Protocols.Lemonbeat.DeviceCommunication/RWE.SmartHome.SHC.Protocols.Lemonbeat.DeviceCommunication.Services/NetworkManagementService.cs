using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.NetworkManagement;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class NetworkManagementService : SenderService<network>, INetworkManagementService
{
	private const string DefaultNamespace = "urn:network_managementxsd";

	public NetworkManagementService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.NetworkManagement, "urn:network_managementxsd")
	{
	}

	public void SetInclusionData(DeviceIdentifier destination, string inclusionData)
	{
		SetInclusionData(destination, inclusionData, null);
	}

	public void SetInclusionData(DeviceIdentifier destination, string inclusionData, byte addressSize)
	{
		SetInclusionData(destination, inclusionData, addressSize);
	}

	private void SetInclusionData(DeviceIdentifier destination, string inclusionData, byte? addressSize)
	{
		network network = new network();
		network.version = 1u;
		network.device = new networkDevice[1]
		{
			new networkDevice
			{
				version = 1u,
				Item = new network_include
				{
					Value = inclusionData.ToByteArray(),
					address_sizeSpecified = addressSize.HasValue,
					address_size = (addressSize ?? 0)
				},
				device_idSpecified = destination.SubDeviceId.HasValue,
				device_id = (destination.SubDeviceId ?? 0)
			}
		};
		network message = network;
		SendMessage(destination, message, TransportType.Connection);
	}
}
