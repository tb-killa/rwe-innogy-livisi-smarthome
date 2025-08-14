using System;
using System.Collections.Generic;
using System.Linq;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace SmartHome.SHC.API.Messaging;

public class Message
{
	public MessageClass Class { get; private set; }

	public Guid Id { get; private set; }

	public string Type { get; private set; }

	public DateTime TimeStamp { get; private set; }

	public StringProperty[] Properties { get; private set; }

	public Link[] AssociatedEntities { get; private set; }

	internal Message(MessageClass messageClass, Guid assignedId, DateTime timestamp, string type, IEnumerable<StringProperty> properties, IEnumerable<Link> links)
	{
		Class = messageClass;
		Id = assignedId;
		TimeStamp = timestamp;
		Type = type;
		Properties = ((properties != null) ? properties.ToArray() : new StringProperty[0]);
		AssociatedEntities = ((links != null) ? links.ToArray() : new Link[0]);
	}
}
