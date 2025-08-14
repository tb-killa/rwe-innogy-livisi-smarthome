using System;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Messages;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Messages;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.ExternalCommandDispatcher.ErrorHandling;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

internal class MessagesAndAlertsHandler : ICommandHandler, IBaseCommandHandler
{
	private const string LoggingSource = "MessagesAndAlertsHandler";

	private readonly IMessagesAndAlertsManager messagesAndAlerts;

	public MessagesAndAlertsHandler(IMessagesAndAlertsManager messagesAndAlerts)
	{
		this.messagesAndAlerts = messagesAndAlerts;
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}

	public BaseResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction)
	{
		if (request is GetMessageListRequest request2)
		{
			return RequestGetMessageList(request2);
		}
		if (request is AddMessageRequest request3)
		{
			return RequestAddMessage(request3);
		}
		if (request is ChangeMessageStateRequest request4)
		{
			return RequestChangeMessageState(request4);
		}
		if (request is DeleteMessageRequest deleteMessageRequest)
		{
			return DeleteMessage(deleteMessageRequest);
		}
		return null;
	}

	private BaseResponse RequestAddMessage(AddMessageRequest request)
	{
		string message = $"Adding a Message with ID [{request.Message.Id}] of class [{request.Message.Class}] and Type [{request.Message.Type}].";
		Log.Information(Module.ExternalCommandDispatcher, "MessagesAndAlertsHandler", message);
		if (request.Message == null || request.Message.Properties == null || request.Message.Properties.Any((StringProperty mp) => mp.Value == null))
		{
			return new ErrorResponse(request.RequestId, ErrorResponseType.GenericError, new StringProperty("Assembly", "ExternalCommandDispatcher"), new StringProperty("Module", "MessagesAndAlertsHandler"), new StringProperty("Code", ErrorCode.InvalidRequest.ToString()));
		}
		if (messagesAndAlerts.AddMessage(request.Message, MessageAddMode.NewMessage) != null)
		{
			return new AcknowledgeResponse();
		}
		return new ErrorResponse(request.RequestId, ErrorResponseType.GenericError, new StringProperty("Assembly", "ExternalCommandDispatcher"), new StringProperty("Module", "MessagesAndAlertsHandler"), new StringProperty("Code", ErrorCode.UnexpectedErrorOccurred.ToString()));
	}

	private BaseResponse DeleteMessage(DeleteMessageRequest deleteMessageRequest)
	{
		Message messageById = messagesAndAlerts.GetMessageById(deleteMessageRequest.MessageId);
		if (messageById != null && messageById.Class == MessageClass.Alert)
		{
			return new ErrorResponse(deleteMessageRequest.RequestId, ErrorResponseType.GenericError, new StringProperty("Reason", "Alerts cannot be deleted."), new StringProperty("Assembly", "ExternalCommandDispatcher"), new StringProperty("Module", "MessagesAndAlertsHandler"), new StringProperty("Code", ErrorCode.InvalidRequest.ToString()));
		}
		messagesAndAlerts.DeleteAllMessages((Message msg) => msg.Id == deleteMessageRequest.MessageId);
		return new AcknowledgeResponse();
	}

	private BaseResponse RequestChangeMessageState(ChangeMessageStateRequest request)
	{
		messagesAndAlerts.UpdateMessageState(request.MessageId, request.NewState);
		return new AcknowledgeResponse();
	}

	private BaseResponse RequestGetMessageList(GetMessageListRequest request)
	{
		MessageListResponse messageListResponse = new MessageListResponse();
		messageListResponse.MessageList = messagesAndAlerts.GetAllMessages((Message m) => true).ToList();
		return messageListResponse;
	}
}
