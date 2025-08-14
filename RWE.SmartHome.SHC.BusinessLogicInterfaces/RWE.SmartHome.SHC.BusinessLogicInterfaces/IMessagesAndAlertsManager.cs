using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces;

public interface IMessagesAndAlertsManager : IService
{
	Message AddMessage(Message message, MessageAddMode addMode);

	void DeleteAllMessages(Predicate<Message> filter);

	IEnumerable<Message> GetAllMessages(Predicate<Message> filter);

	Message GetMessageById(Guid messageId);

	void UpdateMessageState(Guid messageId, MessageState newState);

	void ReplaceMessage(Message newMessage, Func<Message, bool> func);

	bool ContainsMessage(Func<Message, bool> func);
}
