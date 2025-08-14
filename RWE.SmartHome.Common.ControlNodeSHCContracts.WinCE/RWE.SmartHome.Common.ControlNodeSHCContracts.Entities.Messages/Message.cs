using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;

public class Message
{
	[XmlAttribute]
	public Guid Id { get; set; }

	[XmlAttribute]
	public MessageClass Class { get; set; }

	[XmlAttribute]
	public string Type { get; set; }

	[XmlAttribute]
	public DateTime TimeStamp { get; set; }

	[XmlAttribute]
	public MessageState State { get; set; }

	[XmlAttribute]
	public string AppId { get; set; }

	[XmlAttribute]
	public string AddinVersion { get; set; }

	public List<StringProperty> Properties { get; set; }

	public List<Guid> BaseDeviceIds { get; set; }

	public List<Guid> LogicalDeviceIds { get; set; }

	public List<Tag> Tags { get; set; }

	public Message()
	{
		Id = Guid.NewGuid();
		Properties = new List<StringProperty>();
		TimeStamp = DateTime.UtcNow;
		Tags = new List<Tag>();
	}

	public Message(MessageClass messageClass, string type, List<StringProperty> properties)
	{
		Id = Guid.NewGuid();
		Class = messageClass;
		Type = type;
		TimeStamp = DateTime.UtcNow;
		Properties = properties ?? new List<StringProperty>();
		Tags = new List<Tag>();
	}

	public Message Clone()
	{
		Message message = new Message(Class, Type, new List<StringProperty>());
		message.Id = Id;
		message.TimeStamp = TimeStamp;
		message.AppId = ((AppId == null) ? null : string.Copy(AppId));
		message.State = State;
		message.AddinVersion = ((AddinVersion == null) ? null : string.Copy(AddinVersion));
		Message message2 = message;
		if (BaseDeviceIds != null)
		{
			message2.BaseDeviceIds = new List<Guid>(BaseDeviceIds);
		}
		if (LogicalDeviceIds != null)
		{
			message2.LogicalDeviceIds = new List<Guid>(LogicalDeviceIds);
		}
		message2.Tags.AddRange(Tags.Select((Tag tg) => tg.Clone()));
		if (Properties != null)
		{
			foreach (StringProperty property in Properties)
			{
				message2.Properties.Add(property.Clone());
			}
		}
		else
		{
			message2.Properties = null;
		}
		return message2;
	}
}
