using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Messages;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Messages;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService;
using WebServerHost.Helpers;
using WebServerHost.Web;

namespace WebServerHost.Controllers.ClientAPI;

[Route("message")]
public class MessageController : Controller
{
	private readonly IShcClient shcClient;

	private readonly IMessageConverterService messageConverter;

	public MessageController(IMessageConverterService messageConverter, IShcClient shcClient)
	{
		this.messageConverter = messageConverter;
		this.shcClient = shcClient;
	}

	[Route("")]
	[HttpGet]
	public IEnumerable<SmartHome.Common.API.Entities.Entities.Message> GetMessages(string tkey, string tval)
	{
		List<SmartHome.Common.API.Entities.Entities.Message> list = GetAllMessages();
		if (!string.IsNullOrEmpty(tkey) && string.IsNullOrEmpty(tval))
		{
			list = list.Where((SmartHome.Common.API.Entities.Entities.Message message) => message.Tags != null && message.Tags.Any((Property t) => t.Name == tkey && t.Value.Equals(tval))).ToList();
		}
		return list;
	}

	[Route("{id}")]
	[HttpGet]
	public SmartHome.Common.API.Entities.Entities.Message GetMessage([FromRoute] string id)
	{
		if (string.IsNullOrEmpty(id) || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		SmartHome.Common.API.Entities.Entities.Message message = GetAllMessages().FirstOrDefault((SmartHome.Common.API.Entities.Entities.Message existingMessage) => string.Equals(existingMessage.Id, id));
		if (message == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Message not found {id} with id: ");
		}
		return message;
	}

	[Route("{id}")]
	[HttpPut]
	public IResult Update([FromRoute] string id, [FromBody] SmartHome.Common.API.Entities.Entities.Message message)
	{
		return UpdateMessageReadState(id);
	}

	[HttpDelete]
	[Route("{id}")]
	public IResult Delete([FromRoute] string id)
	{
		if (string.IsNullOrEmpty(id) || !id.GuidTryParse(out var output))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		if (GetAllMessages().FirstOrDefault((SmartHome.Common.API.Entities.Entities.Message existingMessage) => string.Equals(existingMessage.Id, id)) == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Message not found with id: {id}");
		}
		DeleteMessageRequest deleteMessageRequest = ShcRequestHelper.NewRequest<DeleteMessageRequest>();
		deleteMessageRequest.MessageId = output;
		BaseResponse response = shcClient.GetResponse(deleteMessageRequest);
		if (response != null)
		{
			if (response is AcknowledgeResponse)
			{
				return Ok();
			}
			if (response is ErrorResponse errorResponse)
			{
				throw new ApiException(ErrorHelper.GetError(errorResponse));
			}
		}
		throw new ApiException(ErrorCode.InternalError, "Unkown Error");
	}

	private List<SmartHome.Common.API.Entities.Entities.Message> GetAllMessages()
	{
		List<SmartHome.Common.API.Entities.Entities.Message> list = new List<SmartHome.Common.API.Entities.Entities.Message>();
		GetMessageListRequest request = ShcRequestHelper.NewRequest<GetMessageListRequest>();
		BaseResponse response = shcClient.GetResponse(request);
		if (response != null && response is MessageListResponse messageListResponse)
		{
			foreach (RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message message in messageListResponse.MessageList)
			{
				list.Add(messageConverter.FromSmartHomeMessage(message));
			}
			if (response is ErrorResponse errorResponse)
			{
				throw new ApiException(ErrorHelper.GetError(errorResponse));
			}
		}
		return list;
	}

	private IResult UpdateMessageReadState(string messageId)
	{
		ChangeMessageStateRequest changeMessageStateRequest = ShcRequestHelper.NewRequest<ChangeMessageStateRequest>();
		if (messageId.GuidTryParse(out var output))
		{
			if (CheckMessageExists(messageId))
			{
				changeMessageStateRequest.MessageId = output;
				changeMessageStateRequest.NewState = MessageState.Read;
				BaseResponse response = shcClient.GetResponse(changeMessageStateRequest);
				if (response is AcknowledgeResponse)
				{
					return Ok();
				}
				throw new ApiException(ErrorCode.InternalError, $"Internal error occurred while settig message with Id = '{messageId}' as read");
			}
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Message not found with id: {messageId}");
		}
		throw new ApiException(ErrorCode.InvalidArgument, $"The value '{messageId}' is not valid");
	}

	private bool CheckMessageExists(string messageId)
	{
		List<SmartHome.Common.API.Entities.Entities.Message> allMessages = GetAllMessages();
		return allMessages.Any((SmartHome.Common.API.Entities.Entities.Message m) => m.Id == messageId);
	}

	private IResult CreateMessage(SmartHome.Common.API.Entities.Entities.Message message)
	{
		AddMessageRequest request = ShcRequestHelper.NewRequest<AddMessageRequest>();
		BaseResponse response = shcClient.GetResponse(request);
		if (response != null)
		{
			if (response is AcknowledgeResponse)
			{
				return Ok();
			}
			if (response is ErrorResponse errorResponse)
			{
				throw new ApiException(ErrorHelper.GetError(errorResponse));
			}
		}
		throw new ApiException(ErrorCode.InternalError, "Unkown Error");
	}
}
