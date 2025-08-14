using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Logging;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.DeviceActivity.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher;

public class LoggingHandler : ICommandHandler, IBaseCommandHandler
{
	private readonly IEventManager eventManager;

	public LoggingHandler(IEventManager eventManager)
	{
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
		if (request is AdjustLogLevelRequest request2)
		{
			return HandleAdjustGroupLogLevelRequest(request2);
		}
		if (request is GetLogLevelRequest request3)
		{
			return HandleGetLogLevelRequest(request3);
		}
		return null;
	}

	private BaseResponse HandleAdjustGroupLogLevelRequest(AdjustLogLevelRequest request)
	{
		if (request.ExpireAfterMinutes < 0)
		{
			ErrorResponse errorResponse = new ErrorResponse();
			errorResponse.CorrespondingRequestId = request.RequestId;
			errorResponse.ErrorType = ErrorResponseType.ActionExecutionError;
			return errorResponse;
		}
		ModuleInfos.AdjustLogLevel(request.ExpireAfterMinutes);
		string value = string.Format("The logging level had been changed to DEBUG by [{0}].{1}", string.IsNullOrEmpty(request.Requester) ? "Administrator" : request.Requester, (!string.IsNullOrEmpty(request.Reason)) ? $"Reason: {request.Reason}" : string.Empty);
		Console.WriteLine(value);
		eventManager.GetEvent<LoggingLevelAdjustedEvent>().Publish(new LoggingLevelAdjustedEventArgs(TimeSpan.FromMinutes(request.ExpireAfterMinutes), request.Requester, request.Reason));
		AcknowledgeResponse acknowledgeResponse = new AcknowledgeResponse();
		acknowledgeResponse.CorrespondingRequestId = request.RequestId;
		return acknowledgeResponse;
	}

	private BaseResponse HandleGetLogLevelRequest(GetLogLevelRequest request)
	{
		LogLevelResponse logLevelResponse = new LogLevelResponse();
		logLevelResponse.CorrespondingRequestId = request.RequestId;
		logLevelResponse.RemainingTime = ModuleInfos.RemainingTime;
		return logLevelResponse;
	}
}
