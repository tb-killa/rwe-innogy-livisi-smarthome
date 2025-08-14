using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.FirmwareUpdate;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

internal class DeviceFirmwareUpdateHandler : ICommandHandler, IBaseCommandHandler
{
	private readonly IEventManager eventManager;

	private readonly IDeviceFirmwareManager deviceFirmwareManager;

	private readonly IProtocolMultiplexer protocolMultiplexer;

	public DeviceFirmwareUpdateHandler(IEventManager eventManager, IDeviceFirmwareManager deviceFirmwareManager, IProtocolMultiplexer protocolMultiplexer)
	{
		this.protocolMultiplexer = protocolMultiplexer;
		this.eventManager = eventManager;
		this.deviceFirmwareManager = deviceFirmwareManager;
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}

	public BaseResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction)
	{
		if (request is TriggerFirmwareUpdateRequest request2)
		{
			return TriggerFirmwareUpdate(request2);
		}
		return null;
	}

	private BaseResponse TriggerFirmwareUpdate(TriggerFirmwareUpdateRequest request)
	{
		deviceFirmwareManager.PerformUpdate(request.DevicesToBeUpdated);
		return new AcknowledgeResponse();
	}
}
