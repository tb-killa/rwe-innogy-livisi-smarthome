using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceInclusion;
using RWE.SmartHome.SHC.CoreApiConverters;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.Protocols.CustomProtocol;
using SmartHome.SHC.API.Protocols.Lemonbeat;
using SmartHome.SHC.API.Protocols.wMBus;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceInclusion;

public class CapabilityInclusionFactory : ILogicalDeviceInclusionFactory
{
	private readonly IApplicationsHost appHost;

	private readonly IDeviceDefinitionsProvider deviceDefinitionsProvider;

	public CapabilityInclusionFactory(IApplicationsHost appHost, IDeviceDefinitionsProvider deviceDefinitionsProvider)
	{
		this.appHost = appHost;
		this.deviceDefinitionsProvider = deviceDefinitionsProvider;
	}

	public IEnumerable<LogicalDevice> CreateLogicalDevices(BaseDevice newDevice)
	{
		List<LogicalDevice> list = new List<LogicalDevice>();
		if (newDevice.ProtocolId == ProtocolIdentifier.Cosip)
		{
			list.AddRange(new CoreCapabilityFactory().CreateCapabilities(newDevice));
		}
		else if (newDevice.ProtocolId == ProtocolIdentifier.wMBus)
		{
			IWMBusDeviceHandler customDevice = appHost.GetCustomDevice<IWMBusDeviceHandler>(newDevice.AppId);
			list.AddRange((from d in customDevice.GenerateCapabilities(newDevice.ToApiBaseDevice())
				select d.ToCoreLogicalDevice() into logicalDevice
				where logicalDevice != null
				select logicalDevice).ToList());
		}
		else if (newDevice.ProtocolId == ProtocolIdentifier.Lemonbeat)
		{
			IDeviceHandler customDevice2 = appHost.GetCustomDevice<IDeviceHandler>(newDevice.AppId);
			list.AddRange((from d in customDevice2.GenerateCapabilities(newDevice.ToApiBaseDevice())
				select d.ToCoreLogicalDevice() into logicalDevice
				where logicalDevice != null
				select logicalDevice).ToList());
		}
		else if (newDevice.ProtocolId == ProtocolIdentifier.Virtual)
		{
			if (IsCoreDevice(newDevice))
			{
				list.AddRange(new CoreCapabilityFactory().CreateCapabilities(newDevice));
			}
			else
			{
				ICustomProtocolDeviceHandler customDevice3 = appHost.GetCustomDevice<ICustomProtocolDeviceHandler>(newDevice.AppId);
				if (customDevice3 != null)
				{
					list.AddRange((from d in customDevice3.GenerateCapabilities(newDevice.ToApiBaseDevice())
						select d.ToCoreLogicalDevice() into logicalDevice
						where logicalDevice != null
						select logicalDevice).ToList());
				}
			}
		}
		List<LogicalDeviceDefinition> source = (deviceDefinitionsProvider.GetLogicalDeviceDefinition(newDevice) ?? new List<LogicalDeviceDefinition>()).ToList();
		foreach (LogicalDevice item in list)
		{
			LogicalDevice ld = item;
			LogicalDeviceDefinition logicalDeviceDefinition = source.FirstOrDefault((LogicalDeviceDefinition ldDef) => ldDef.DeviceType == ld.DeviceType);
			if (logicalDeviceDefinition != null)
			{
				List<Property> collection = logicalDeviceDefinition.ConfigurationProperties.GenerateMissingProperties(item.GetAllProperties());
				item.Properties.AddRange(collection);
			}
		}
		return list;
	}

	private bool IsCoreDevice(BaseDevice newDevice)
	{
		return newDevice.AppId == CoreConstants.CoreAppId;
	}
}
