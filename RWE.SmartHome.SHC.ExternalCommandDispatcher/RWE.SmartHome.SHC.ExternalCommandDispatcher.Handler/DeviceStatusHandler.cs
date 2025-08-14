using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.DeviceState;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.DeviceState;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler.DeviceNotifications;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

internal class DeviceStatusHandler : ICommandHandler, IBaseCommandHandler
{
	private readonly IEventManager eventManager;

	private readonly INotificationHandler notificationHandler;

	private readonly IRepository repository;

	private readonly ILogicalDeviceStateRepository logicalDeviceStateRepository;

	private readonly IProtocolMultiplexer protocolMultiplexer;

	private readonly ExternalCommandDispatcher externalCommandDispatcher;

	private readonly DeviceStateChangeNotifier deviceStateChangeNotifier;

	private readonly CapabilityStateChangeNotifier capabilityStateChangeNotifier;

	public DeviceStatusHandler(IEventManager eventManager, INotificationHandler notificationHandler, IRepository repository, ILogicalDeviceStateRepository logicalDeviceStateRepository, IProtocolMultiplexer protocolMultiplexer, ExternalCommandDispatcher externalCommandDispatcher)
	{
		this.protocolMultiplexer = protocolMultiplexer;
		this.logicalDeviceStateRepository = logicalDeviceStateRepository;
		this.notificationHandler = notificationHandler;
		this.eventManager = eventManager;
		this.repository = repository;
		this.externalCommandDispatcher = externalCommandDispatcher;
		deviceStateChangeNotifier = new DeviceStateChangeNotifier(eventManager, protocolMultiplexer, notificationHandler);
		capabilityStateChangeNotifier = new CapabilityStateChangeNotifier(eventManager, protocolMultiplexer, notificationHandler, repository);
	}

	public void Initialize()
	{
		deviceStateChangeNotifier.Init();
		capabilityStateChangeNotifier.Init();
	}

	public void Uninitialize()
	{
		deviceStateChangeNotifier.Uninit();
		capabilityStateChangeNotifier.Uninit();
	}

	public BaseResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction)
	{
		if (request is GetAllLogicalDeviceStatesRequest request2)
		{
			return GetAllLogicalDeviceStates(request2);
		}
		if (request is GetLogicalDeviceStateRequest request3)
		{
			return GetLogicalDeviceState(request3);
		}
		if (request is TriggerSensorRequest request4)
		{
			return TriggerSensor(request4);
		}
		if (request is RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.DeviceState.GetAllPhysicalDeviceStatesRequest)
		{
			return GetAllPhysicalDeviceStatesRequest();
		}
		if (request is GetPhysicalDeviceStateRequest getPhysicalDeviceStateRequest)
		{
			return GetPhysicalDeviceStateRequest(getPhysicalDeviceStateRequest);
		}
		return null;
	}

	private BaseResponse GetAllLogicalDeviceStates(GetAllLogicalDeviceStatesRequest request)
	{
		GetAllLogicalDeviceStatesResponse getAllLogicalDeviceStatesResponse = new GetAllLogicalDeviceStatesResponse();
		getAllLogicalDeviceStatesResponse.Result = ControlResult.Ok;
		getAllLogicalDeviceStatesResponse.States = ((request.DeviceIds != null) ? logicalDeviceStateRepository.GetAllLogicalDeviceStates(request.DeviceIds.ToArray()) : logicalDeviceStateRepository.GetAllLogicalDeviceStates());
		return getAllLogicalDeviceStatesResponse;
	}

	private BaseResponse GetLogicalDeviceState(GetLogicalDeviceStateRequest request)
	{
		GetLogicalDeviceStateResponse getLogicalDeviceStateResponse = new GetLogicalDeviceStateResponse();
		getLogicalDeviceStateResponse.Result = ControlResult.Ok;
		getLogicalDeviceStateResponse.State = logicalDeviceStateRepository.GetLogicalDeviceState(request.LogicalDeviceId);
		return getLogicalDeviceStateResponse;
	}

	private BaseResponse TriggerSensor(TriggerSensorRequest request)
	{
		ControlResultResponse controlResultResponse = new ControlResultResponse();
		ActionContext context = new ActionContext(ContextType.ClientRequest, request.SensorGuid);
		controlResultResponse.Result = protocolMultiplexer.DeviceController.TriggerSensorRequest(context, request.SensorGuid, request.ButtonId);
		return controlResultResponse;
	}

	private BaseResponse GetAllPhysicalDeviceStatesRequest()
	{
		GetAllPhysicalDeviceStatesResponse getAllPhysicalDeviceStatesResponse = new GetAllPhysicalDeviceStatesResponse();
		getAllPhysicalDeviceStatesResponse.DeviceStates = protocolMultiplexer.PhysicalState.GetAll();
		return getAllPhysicalDeviceStatesResponse;
	}

	private BaseResponse GetPhysicalDeviceStateRequest(GetPhysicalDeviceStateRequest getPhysicalDeviceStateRequest)
	{
		GetPhysicalDeviceStateResponse getPhysicalDeviceStateResponse = new GetPhysicalDeviceStateResponse();
		getPhysicalDeviceStateResponse.DeviceState = protocolMultiplexer.PhysicalState.Get(getPhysicalDeviceStateRequest.PhysicalDeviceId);
		GetPhysicalDeviceStateResponse getPhysicalDeviceStateResponse2 = getPhysicalDeviceStateResponse;
		if (getPhysicalDeviceStateResponse2.DeviceState != null)
		{
			getPhysicalDeviceStateResponse2.DeviceState.PhysicalDeviceId = getPhysicalDeviceStateRequest.PhysicalDeviceId;
		}
		return getPhysicalDeviceStateResponse2;
	}
}
