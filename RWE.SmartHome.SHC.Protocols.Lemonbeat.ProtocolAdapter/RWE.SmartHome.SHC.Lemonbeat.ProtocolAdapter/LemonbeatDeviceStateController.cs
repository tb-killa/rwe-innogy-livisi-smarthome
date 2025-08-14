using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.CoreApiConverters;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;
using SmartHome.SHC.API;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.Control;
using SmartHome.SHC.API.PropertyDefinition;
using SmartHome.SHC.API.Protocols.Lemonbeat;

namespace RWE.SmartHome.SHC.Lemonbeat.ProtocolAdapter;

internal class LemonbeatDeviceStateController : IProtocolSpecificDeviceController, IProtocolSpecificLogicalStateRequestor
{
	private readonly IValueService valueService;

	private readonly IDeviceList deviceList;

	private readonly IEventManager eventManager;

	private readonly IApplicationsHost appHost;

	private readonly ShcValueRepository shcValueRepository;

	private readonly IRepository configurationRepository;

	public ProtocolIdentifier ProtocolId => ProtocolIdentifier.Lemonbeat;

	public LemonbeatDeviceStateController(IEventManager eventManager, IApplicationsHost appHost, IValueService valueService, IDeviceList deviceList, ShcValueRepository shcValueRepository, IRepository configurationRepository)
	{
		this.configurationRepository = configurationRepository;
		this.eventManager = eventManager;
		this.appHost = appHost;
		this.deviceList = deviceList;
		this.valueService = valueService;
		this.valueService.ValueReportReceived += OnValueReportReceived;
		this.valueService.RequestStatusCompleted += OnRequestStatusCompleted;
		this.shcValueRepository = shcValueRepository;
		deviceList.DeviceReachabilityChanged += OnDeviceUnreachableChanged;
		deviceList.DeviceConfiguredStateChanged += OnDeviceConfiguredStateChanged;
		eventManager.GetEvent<LemonbeatCoreGatewayReadyEvent>().Subscribe(RequestStatusInfoForAllLemonbeatDevices, (LemonbeatCoreGatewayReadyEventArgs m) => true, ThreadOption.BackgroundThread, null);
	}

	private void RequestStatusInfoForAllLemonbeatDevices(LemonbeatCoreGatewayReadyEventArgs args)
	{
		List<BaseDevice> list = (from m in configurationRepository.GetBaseDevices()
			where m != null && m.ProtocolId == ProtocolIdentifier.Lemonbeat
			select m).ToList();
		foreach (BaseDevice item in list)
		{
			RequestState(item);
		}
		Log.Debug(Module.LemonbeatProtocolAdapter, string.Format("Requested state for all LemonbeatDevices [{0}]", string.Join(", ", list.Select((BaseDevice m) => m.Id.ToString()).ToArray())));
	}

	private void OnRequestStatusCompleted(object sender, RequestStatusCompletedEventArgs e)
	{
		if (e.UserState is DeviceInformation deviceInformation)
		{
			if (e.Error != null)
			{
				Log.Warning(Module.LemonbeatProtocolAdapter, $"Cannot retrieve status of device {deviceInformation.ToString()}. The following exception occured:\n{e.Error.Message}");
				return;
			}
			IEnumerable<StringValue> stringValues = e.Result.StringValues.Select((CoreStringValue sv) => sv.ToApiStringValue());
			IEnumerable<NumberValue> numberValues = e.Result.NumberValues.Select((CoreNumberValue nv) => nv.ToApiNumberValue());
			IEnumerable<HexBinaryValue> hexValues = e.Result.HexBinaryValues.Select((CoreHexBinaryValue nv) => nv.ToApiHexBinaryValue());
			HandleValues(deviceInformation.Identifier, stringValues, numberValues, hexValues);
		}
		else
		{
			Log.Warning(Module.LemonbeatProtocolAdapter, "Received state of unknown device");
		}
	}

	private void OnValueReportReceived(object sender, ValueReportReceivedArgs e)
	{
		IEnumerable<StringValue> stringValues = e.StringValues.Select((CoreStringValue sv) => sv.ToApiStringValue());
		IEnumerable<NumberValue> numberValues = e.NumberValues.Select((CoreNumberValue nv) => nv.ToApiNumberValue());
		IEnumerable<HexBinaryValue> hexValues = e.HexBinaryValues.Select((CoreHexBinaryValue nv) => nv.ToApiHexBinaryValue());
		HandleValues(e.DeviceIdentifier, stringValues, numberValues, hexValues);
	}

	private void HandleValues(DeviceIdentifier deviceIdentifier, IEnumerable<StringValue> stringValues, IEnumerable<NumberValue> numberValues, IEnumerable<HexBinaryValue> hexValues)
	{
		PhysicalDeviceState physicalDeviceState = new PhysicalDeviceState();
		physicalDeviceState.HexBinaryValues = hexValues.ToList();
		physicalDeviceState.StringValues = stringValues.ToList();
		physicalDeviceState.NumberValues = numberValues.ToList();
		PhysicalDeviceState state = physicalDeviceState;
		HandleValueReport(deviceIdentifier, state);
	}

	private void HandleValueReport(DeviceIdentifier identifier, PhysicalDeviceState state)
	{
		DeviceInformation deviceInformation = deviceList[identifier];
		if (deviceInformation == null || deviceInformation.DeviceInclusionState != LemonbeatDeviceInclusionState.Included || deviceInformation.DeviceTypeIdentifier == null || !(appHost.GetLemonbeatDeviceHandler(deviceInformation.DeviceTypeIdentifier) is ICurrentStateHandler currentStateHandler))
		{
			return;
		}
		try
		{
			PhysicalStateTransformationResult physicalStateTransformationResult = currentStateHandler.HandlePhysicalState(deviceInformation.DeviceId, state);
			if (physicalStateTransformationResult == null)
			{
				return;
			}
			foreach (KeyValuePair<Guid, CapabilityState> capabilityState in physicalStateTransformationResult.CapabilityStates)
			{
				if (capabilityState.Value.IsTransient)
				{
					eventManager.GetEvent<DeviceEventDetectedEvent>().Publish(new DeviceEventDetectedEventArgs(capabilityState.Key, string.Empty, capabilityState.Value.Properties.ToList().ConvertAll((global::SmartHome.SHC.API.PropertyDefinition.Property apd) => apd.ToCoreProperty(includeTimestamp: true))));
				}
				else
				{
					eventManager.GetEvent<RawLogicalDeviceStateChangedEvent>().Publish(new RawLogicalDeviceStateChangedEventArgs(capabilityState.Key, capabilityState.Value.ToCoreDeviceState(capabilityState.Key)));
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Error invoking the add-in for processing the technical state:\n" + ex.ToString());
		}
	}

	public List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> CreateTriggerEvent(LogicalDevice logicalDevice, int buttonId)
	{
		return new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>();
	}

	public void RequestState(LogicalDevice logicalDevice)
	{
		if (logicalDevice.BaseDevice == null || logicalDevice.BaseDevice.ProtocolId != ProtocolIdentifier.Lemonbeat || appHost.GetCustomDevice<ICurrentStateHandler>(logicalDevice.BaseDevice.AppId) == null)
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, $"LemonbeatStateController: State of device {logicalDevice.Name} cannot be retrieved.");
		}
		else
		{
			RequestState(logicalDevice.BaseDevice);
		}
	}

	public void RequestState(BaseDevice baseDevice)
	{
		Guid id = baseDevice.Id;
		DeviceInformation deviceInformation = deviceList[id];
		if (deviceInformation != null && deviceInformation.IsReachable)
		{
			valueService.RequestStatusAsync(deviceInformation.Identifier, deviceInformation);
		}
	}

	public RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult ExecuteAction(ActionContext context, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules.ActionDescription actionDescription)
	{
		BaseDevice targetBasedevice = GetTargetBasedevice(configurationRepository, actionDescription.Target);
		if (targetBasedevice == null)
		{
			return RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult.Error("Invalid base device");
		}
		DeviceInformation deviceInformation = deviceList[targetBasedevice.Id];
		if (deviceInformation == null)
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, "SetActuatorState: Received actuator state does not match any Lemonbeat device");
			return RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult.Error("Invalid base device");
		}
		if (!deviceInformation.IsReachable)
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, "SetActuatorState: Device is not reachable, no command will be sent");
			return RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult.Error("Device unreachable");
		}
		global::SmartHome.SHC.API.Protocols.Lemonbeat.IActionExecuterHandler customDevice = appHost.GetCustomDevice<global::SmartHome.SHC.API.Protocols.Lemonbeat.IActionExecuterHandler>(targetBasedevice.AppId);
		if (customDevice == null)
		{
			Log.Debug(Module.LemonbeatProtocolAdapter, "SetActuatorState: No add-in found for id: " + targetBasedevice.AppId);
			return RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult.Error("Badly registered base device");
		}
		ControlMessage controlMessage;
		try
		{
			controlMessage = customDevice.TranslateAction(actionDescription.ToApi(), new ExecutionContext
			{
				Details = new global::SmartHome.SHC.API.PropertyDefinition.Property[0],
				Source = ExecutionSource.DirectExecution
			});
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Processing profile settings returned an error:\n" + ex.ToString());
			return RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult.Error("Lemonbeat failure: " + ex.Message);
		}
		SendControlMessage(deviceInformation, controlMessage, targetBasedevice.AppId);
		return new RWE.SmartHome.SHC.DomainModel.Actions.ExecutionResult(RWE.SmartHome.SHC.DomainModel.Actions.ExecutionStatus.Success, new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>());
	}

	public static BaseDevice GetTargetBasedevice(IRepository repository, LinkBinding link)
	{
		if (link.LinkType == EntityType.LogicalDevice)
		{
			LogicalDevice logicalDevice = repository.GetLogicalDevice(link.EntityIdAsGuid());
			if (logicalDevice != null)
			{
				return logicalDevice.BaseDevice;
			}
		}
		else if (link.LinkType == EntityType.BaseDevice)
		{
			return repository.GetBaseDevice(link.EntityIdAsGuid());
		}
		return null;
	}

	private void SendControlMessage(DeviceInformation device, ControlMessage controlMessage, string appId)
	{
		uint valueId;
		if (controlMessage is SetValuesMessage setValuesMessage)
		{
			IEnumerable<CoreNumberValue> numberValues = ((setValuesMessage.NumberValues != null) ? setValuesMessage.NumberValues.Select((NumberValue nv) => nv.ToCoreNumberValue()) : null);
			IEnumerable<CoreStringValue> stringValues = ((setValuesMessage.StringValues != null) ? setValuesMessage.StringValues.Select((StringValue nv) => nv.ToCoreStringValue()) : null);
			IEnumerable<CoreHexBinaryValue> hexBinaryValues = ((setValuesMessage.HexBinaryValues != null) ? setValuesMessage.HexBinaryValues.Select((HexBinaryValue nv) => nv.ToCoreHexBinaryValue()) : null);
			valueService.SetValueAsync(device.Identifier, numberValues, stringValues, hexBinaryValues, controlMessage.Transport.ToCoreTransport(), device);
		}
		else if (controlMessage is ShcValueReportMessage shcValueReportMessage && shcValueRepository.TryGetValueId(appId, shcValueReportMessage.RegisteredValueName, out valueId))
		{
			ReportShcValue(device, valueId, shcValueReportMessage.ValueToBeReported, controlMessage.Transport.ToCoreTransport());
		}
	}

	private void ReportShcValue(DeviceInformation device, uint valueId, decimal value, RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Transport transportMode)
	{
		valueService.ReportValueAsync(device.Identifier, valueId, (double)value, transportMode, device);
	}

	private void OnDeviceUnreachableChanged(object sender, DeviceReachabilityChangedEventArgs args)
	{
		if (args.IsReachable && args.Device.DeviceInclusionState == LemonbeatDeviceInclusionState.Included)
		{
			valueService.RequestStatusAsync(args.Device.Identifier, args.Device);
		}
	}

	private void OnDeviceConfiguredStateChanged(object sender, DeviceConfiguredEventArgs args)
	{
		if (args.State == DeviceConfigurationState.Complete)
		{
			DeviceInformation deviceInformation = deviceList[args.DeviceId];
			if (deviceInformation != null)
			{
				valueService.RequestStatusAsync(deviceInformation.Identifier, deviceInformation);
			}
		}
	}
}
