using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.MemoryInformation;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class MemoryInformationService : SenderService<network>, IMemoryInformationService
{
	private const string DefaultNamespace = "urn:memory_informationxsd";

	public MemoryInformationService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.MemoryDescription, "urn:memory_informationxsd")
	{
	}

	public List<MemoryInformation> GetMemoryInformation(DeviceIdentifier identifier)
	{
		network network = CreateNetworkMessage(identifier);
		network.device[0].Items = new object[1]
		{
			new memoryInformationGetType()
		};
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network answer = SendRequest(identifier, network);
		return Convert(answer);
	}

	private network CreateNetworkMessage(DeviceIdentifier identifier)
	{
		network network = new network();
		network.version = 1u;
		network.device = new networkDevice[1]
		{
			new networkDevice
			{
				version = 1u,
				device_id = (identifier.SubDeviceId ?? 0)
			}
		};
		return network;
	}

	private List<MemoryInformation> Convert(network answer)
	{
		if (!(answer.device[0].Items[0] is memoryInformationReportType { memory_information: not null } memoryInformationReportType))
		{
			return null;
		}
		List<MemoryInformation> list = new List<MemoryInformation>();
		memoryInformationType[] memory_information = memoryInformationReportType.memory_information;
		foreach (memoryInformationType memoryInformationType in memory_information)
		{
			if (Enum.IsDefined(typeof(MemoryType), (int)memoryInformationType.memory_id))
			{
				list.Add(new MemoryInformation
				{
					MemoryType = (MemoryType)memoryInformationType.memory_id,
					Count = memoryInformationType.count,
					Free = memoryInformationType.free_count
				});
			}
			else
			{
				Log.Error(Module.LemonbeatProtocolAdapter, "Unknown Memory Information ID received from device: " + memoryInformationType.memory_id);
			}
		}
		return list;
	}
}
