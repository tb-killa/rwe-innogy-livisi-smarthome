using System;
using System.Collections.Generic;
using System.Reflection;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.GlobalContracts;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.ExternalCommandDispatcher.ErrorHandling;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

internal class SoftwareUpdateHandler : ICommandHandler, IBaseCommandHandler
{
	private SoftwareUpdateRequest requestInProcess;

	private SubscriptionToken updateProcessedSubscriptionToken;

	private readonly IEventManager eventManager;

	private readonly List<Guid> ownerList = new List<Guid> { DefaultRoles.ShcOwner.Id };

	public SoftwareUpdateHandler(IEventManager eventManager)
	{
		this.eventManager = eventManager;
	}

	public void Initialize()
	{
		updateProcessedSubscriptionToken = eventManager.GetEvent<SoftwareUpdateProgressEvent>().Subscribe(SoftwareUpdateProcessed, null, ThreadOption.PublisherThread, null);
	}

	public void Uninitialize()
	{
		eventManager.GetEvent<SoftwareUpdateProgressEvent>().Unsubscribe(updateProcessedSubscriptionToken);
	}

	public BaseResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction)
	{
		if (request is SoftwareUpdateRequest softwareUpdateRequest)
		{
			return HandleSoftwareUpdateRequest(softwareUpdateRequest);
		}
		return null;
	}

	private BaseResponse HandleSoftwareUpdateRequest(SoftwareUpdateRequest softwareUpdateRequest)
	{
		if (requestInProcess != null)
		{
			return new ErrorResponse(softwareUpdateRequest.RequestId, ErrorResponseType.GenericError, new StringProperty("Assembly", Assembly.GetExecutingAssembly().GetName().Name), new StringProperty("Module", "SoftwareUpdateHandler"), new StringProperty("Code", ErrorCode.SoftwareUpdateInProgress.ToString()));
		}
		requestInProcess = softwareUpdateRequest;
		eventManager.GetEvent<DoSoftwareUpdateEvent>().Publish(new DoSoftwareUpdateEventArgs());
		return new AcknowledgeResponse();
	}

	private void SoftwareUpdateProcessed(SoftwareUpdateProgressEventArgs eventArgs)
	{
		if (eventArgs.State != SoftwareUpdateState.Started)
		{
			requestInProcess = null;
		}
	}
}
