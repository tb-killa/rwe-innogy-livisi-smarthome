using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Logging;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceActivity.DeviceActivityInterfaces;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

internal class DeviceActivityLoggingHandler : ICommandHandler, IBaseCommandHandler
{
	private IDeviceActivityLogger deviceActivityLogger;

	private INotificationHandler notificationHandler;

	private IEventManager eventManager;

	internal DeviceActivityLoggingHandler(IDeviceActivityLogger deviceActivityLogger, INotificationHandler notificationHandler, IEventManager eventManager)
	{
		this.deviceActivityLogger = deviceActivityLogger;
		this.notificationHandler = notificationHandler;
		this.eventManager = eventManager;
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}

	public BaseResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction)
	{
		BaseResponse baseResponse = null;
		try
		{
			return HandleSimpleRequest(request as DeleteActivityLogRequest, delegate
			{
				deviceActivityLogger.DeleteDeviceActivityData();
			});
		}
		catch (Exception arg)
		{
			Log.Error(Module.ExternalCommandDispatcher, $"Failed to handle logging request: {arg}");
			return new ErrorResponse(request.RequestId, ErrorResponseType.GenericError, new StringProperty("Module", Module.DeviceActivity.ToString()));
		}
	}

	private BaseResponse HandleSimpleRequest<T>(T request, Action<T> action) where T : BaseRequest
	{
		if (request == null)
		{
			return null;
		}
		action(request);
		AcknowledgeResponse acknowledgeResponse = new AcknowledgeResponse();
		acknowledgeResponse.CorrespondingRequestId = request.RequestId;
		return acknowledgeResponse;
	}
}
