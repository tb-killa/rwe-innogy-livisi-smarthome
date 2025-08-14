using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.ServiceDescription;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

public class ServiceDescriptionService : SenderService<network>, IServiceDescriptionService
{
	private const string DefaultNamespace = "urn:service_descriptionxsd";

	public ServiceDescriptionService(ILemonbeatCommunication aggregator)
		: base(aggregator, ServiceType.ServiceDescription, "urn:service_descriptionxsd")
	{
	}

	public List<ServiceDescription> GetServiceDescriptions(DeviceIdentifier identifier)
	{
		network network = CreateNetworkMessage();
		network.device[0].Item = new serviceDescriptionGetType();
		network.device[0].device_idSpecified = identifier.SubDeviceId.HasValue;
		network.device[0].device_id = identifier.SubDeviceId ?? 0;
		network network2 = SendRequest(identifier, network);
		List<ServiceDescription> list = new List<ServiceDescription>();
		if (network2 != null && network2.device != null)
		{
			uint subDeviceId = identifier.SubDeviceId ?? 0;
			networkDevice networkDevice = network2.device.Where((networkDevice d) => d.device_id == subDeviceId).FirstOrDefault();
			if (networkDevice != null && networkDevice.Item != null)
			{
				serviceDescriptionReportType serviceDescriptionReportType = (serviceDescriptionReportType)networkDevice.Item;
				serviceType[] service = serviceDescriptionReportType.service;
				foreach (serviceType description in service)
				{
					ServiceDescription serviceDescription = Convert(description);
					if (serviceDescription != null)
					{
						list.Add(serviceDescription);
					}
				}
			}
		}
		return list;
	}

	private network CreateNetworkMessage()
	{
		network network = new network();
		network.version = 1u;
		network.device = new networkDevice[1]
		{
			new networkDevice
			{
				version = 1u
			}
		};
		return network;
	}

	private ServiceDescription Convert(serviceType description)
	{
		ServiceDescription result = null;
		if (Enum.IsDefined(typeof(ServiceType), (int)description.service_id))
		{
			result = new ServiceDescription((ServiceType)description.service_id, description.version);
		}
		else
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Unknown Service ID received from device: " + description.service_id);
		}
		return result;
	}
}
