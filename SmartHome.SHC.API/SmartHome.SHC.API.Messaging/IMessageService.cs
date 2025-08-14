using System;
using System.Collections.Generic;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.Messaging;

public interface IMessageService
{
	Message CreateMessage(MessageClass messageClass, string type, IEnumerable<StringProperty> properties, IEnumerable<Link> associatedEntities);

	Message CreateMessage(MessageClass messageClass, string type, IEnumerable<StringProperty> properties);

	Message CreateMessage(MessageClass messageClass, string type, IEnumerable<Link> associatedEntities);

	Message CreateMessage(MessageClass messageClass, string type);

	IEnumerable<Message> GetAllMessages(MessageClass messageClass);

	Message GetMessage(Guid id);

	bool RemoveMessage(Guid id);
}
