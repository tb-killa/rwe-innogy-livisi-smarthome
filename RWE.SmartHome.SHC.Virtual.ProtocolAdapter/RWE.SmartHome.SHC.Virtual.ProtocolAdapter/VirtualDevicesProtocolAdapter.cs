using System;
using System.Collections.Generic;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.Virtual.ProtocolAdapter;

public class VirtualDevicesProtocolAdapter : IProtocolAdapter
{
	private readonly IProtocolSpecificDeviceController virtualDeviceController;

	private readonly IProtocolSpecificLogicalStateRequestor stateRequester;

	private readonly IRepository configurationRepository;

	private readonly IProtocolSpecificTransformation transformation;

	private readonly IEventManager eventManager;

	private readonly IApplicationsHost applicationsHost;

	private readonly IProtocolSpecificPhysicalStateHandler physicalStateHandler;

	private readonly List<IVirtualCoreActionHandler> coreStateHandlers = new List<IVirtualCoreActionHandler>();

	public ProtocolIdentifier ProtocolId => ProtocolIdentifier.Virtual;

	public IProtocolSpecificLogicalStateRequestor LogicalState => stateRequester;

	public IProtocolSpecificPhysicalStateHandler PhysicalState => physicalStateHandler;

	public IProtocolSpecificDeviceController DeviceController => virtualDeviceController;

	public IProtocolSpecificTransformation Transformation => transformation;

	public IProtocolSpecificDataBackup DataBackup => null;

	public VirtualDevicesProtocolAdapter(Container container)
	{
		configurationRepository = container.Resolve<IRepository>();
		applicationsHost = container.Resolve<IApplicationsHost>();
		eventManager = container.Resolve<IEventManager>();
		IRepository repository = container.Resolve<IRepository>();
		IApplicationsHost appHost = container.Resolve<IApplicationsHost>();
		virtualDeviceController = new VirtualDeviceController(configurationRepository, applicationsHost, coreStateHandlers);
		stateRequester = new VirtualDeviceStateRequester(configurationRepository, applicationsHost, eventManager, coreStateHandlers);
		physicalStateHandler = new VirtualDevicesPhysicalStateHandler(eventManager, repository, appHost);
		transformation = new VirtualDevicesTransformation(configurationRepository, container.Resolve<IShcBaseDeviceWatchers>());
		new VirtualCapabilityStateHandler(appHost, eventManager);
		new VirtualDeviceHandler(appHost, eventManager);
		eventManager.GetEvent<VirtualStateHandlerAvailableEvent>().Subscribe(OnVirtualStateHandlerAvailable, null, ThreadOption.PublisherThread, null);
	}

	private void OnVirtualStateHandlerAvailable(VirtualStateHandlerAvailableEventArgs args)
	{
		lock (coreStateHandlers)
		{
			if (args.Handler != null && !coreStateHandlers.Contains(args.Handler))
			{
				coreStateHandlers.Add(args.Handler);
			}
		}
	}

	public IEnumerable<Guid> GetHandledDevices()
	{
		return null;
	}

	public string GetDeviceDescription(Guid deviceId)
	{
		LogicalDevice logicalDevice = configurationRepository.GetLogicalDevice(deviceId);
		if (logicalDevice == null)
		{
			return string.Empty;
		}
		return logicalDevice.Name;
	}

	public void ResetDeviceInclusionState(Guid deviceId)
	{
	}

	public void DropDiscoveredDevices(BaseDevice[] devices)
	{
		applicationsHost.DropDiscoveredDevices(devices);
	}

	public ProtocolSpecificInformation GetProtocolSpecificInformation()
	{
		return null;
	}
}
