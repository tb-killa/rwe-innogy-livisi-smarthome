using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Configuration;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class ConfigurationService : SenderService<network>, IConfigurationService
{
	private enum ConfigurationMode : uint
	{
		Rollback,
		SaveAndReset,
		SaveAndPreserve,
		SetDefault,
		Clear
	}

	public ConfigurationService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.Configuration, string.Empty)
	{
	}

	public void CommitDeviceConfiguration(DeviceIdentifier aDevice)
	{
		SendMessage(aDevice, CreateNetworkMessage(aDevice, ConfigurationMode.SaveAndReset), TransportType.Connection);
	}

	public void RollbackConfiguration(DeviceIdentifier aDevice)
	{
		SendMessage(aDevice, CreateNetworkMessage(aDevice, ConfigurationMode.Rollback), TransportType.Connection);
	}

	private network CreateNetworkMessage(DeviceIdentifier identifier, ConfigurationMode newMode)
	{
		network network = new network();
		network.version = 1u;
		network.device = new networkDevice[1]
		{
			new networkDevice
			{
				version = 1u,
				device_id = (identifier.SubDeviceId.HasValue ? identifier.SubDeviceId.Value : 0u),
				device_idSpecified = identifier.SubDeviceId.HasValue,
				Items = new configModeSetType[1]
				{
					new configModeSetType
					{
						mode = (uint)newMode
					}
				}
			}
		};
		return network;
	}
}
