using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.CoreApiConverters;
using SmartHome.SHC.API.Control;
using SmartHome.SHC.API.PropertyDefinition;
using SmartHome.SHC.API.Protocols.CustomProtocol;

namespace RWE.SmartHome.SHC.Virtual.ProtocolAdapter;

public class VirtualCapabilityStateHandler
{
	private IEventManager eventManager;

	private IApplicationsHost applicationsHost;

	public VirtualCapabilityStateHandler(IApplicationsHost applicationsHost, IEventManager eventManager)
	{
		this.eventManager = eventManager;
		this.applicationsHost = applicationsHost;
		SubscribeToEvents();
	}

	private void SubscribeToEvents()
	{
		applicationsHost.ApplicationStateChanged += OnApplicationActivated;
		applicationsHost.ApplicationStateChanged += OnApplicationDeactivated;
	}

	private void OnApplicationActivated(ApplicationLoadStateChangedEventArgs args)
	{
		if (args != null && args.Application != null && (args.ApplicationState == ApplicationStates.ApplicationActivated || args.ApplicationState == ApplicationStates.ApplicationUpdated) && args.Application is ICustomProtocolCapabilityStateHandler customProtocolCapabilityStateHandler)
		{
			customProtocolCapabilityStateHandler.StateChanged += PublishVirtualDeviceStateChangedEvent;
		}
	}

	private void OnApplicationDeactivated(ApplicationLoadStateChangedEventArgs args)
	{
		if (args != null && args.Application != null && (args.ApplicationState == ApplicationStates.ApplicationsUninstalled || args.ApplicationState == ApplicationStates.ApplicationDeactivated) && args.Application is ICustomProtocolCapabilityStateHandler customProtocolCapabilityStateHandler)
		{
			customProtocolCapabilityStateHandler.StateChanged -= PublishVirtualDeviceStateChangedEvent;
		}
	}

	private void PublishVirtualDeviceStateChangedEvent(object sender, CapabilityStateChangedEventArgs args)
	{
		if (args.State == null)
		{
			Log.Debug(Module.VirtualProtocolAdapter, $"Null state received for capability {args.DeviceId} from {sender.ToString()}");
			return;
		}
		if (args.State.IsTransient)
		{
			eventManager.GetEvent<DeviceEventDetectedEvent>().Publish(new DeviceEventDetectedEventArgs(args.DeviceId, "StateChanged", args.State.Properties.ToList().ConvertAll((Property apd) => apd.ToCoreProperty(includeTimestamp: true))));
			return;
		}
		GenericDeviceState genericDeviceState = args.State.ToCoreDeviceState(args.DeviceId);
		if (genericDeviceState == null)
		{
			Log.Warning(Module.VirtualProtocolAdapter, $"Could not convert state for device {args.DeviceId}");
		}
		else
		{
			eventManager.GetEvent<RawLogicalDeviceStateChangedEvent>().Publish(new RawLogicalDeviceStateChangedEventArgs(args.DeviceId, genericDeviceState));
		}
	}
}
