using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Messages;

public class MessageListResponse : BaseResponse
{
	public List<Message> MessageList { get; set; }

	public MessageListResponse(List<Message> messageList)
	{
		MessageList = messageList;
	}

	public MessageListResponse()
	{
		MessageList = new List<Message>();
	}
}
