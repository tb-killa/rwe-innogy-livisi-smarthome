using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;
using RWE.SmartHome.SHC.RuleEngineInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

internal class ActionRequestHandler : ICommandHandler, IBaseCommandHandler
{
	private readonly IActionExecuter actionExecuter;

	public ActionRequestHandler(IActionExecuter actionExecuter)
	{
		this.actionExecuter = actionExecuter;
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}

	public BaseResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction)
	{
		if (request is ActionRequest actionRequest)
		{
			ExecutionResult executionResult = actionExecuter.Execute(new ActionContext(ContextType.ClientRequest, request.RequestId), actionRequest.ActionDescription);
			if (executionResult == null)
			{
				executionResult = ExecutionResult.UnknownFailure();
			}
			if (executionResult.Status != ExecutionStatus.Success)
			{
				return new ErrorResponse(request.RequestId, ErrorResponseType.ActionExecutionError, executionResult.Parameters.ToArray());
			}
			ActionResponse actionResponse = new ActionResponse();
			actionResponse.CorrespondingRequestId = actionRequest.RequestId;
			actionResponse.Payload = executionResult.Parameters;
			actionResponse.ApplicationId = actionRequest.ActionDescription.Target.EntityId;
			return actionResponse;
		}
		return null;
	}
}
