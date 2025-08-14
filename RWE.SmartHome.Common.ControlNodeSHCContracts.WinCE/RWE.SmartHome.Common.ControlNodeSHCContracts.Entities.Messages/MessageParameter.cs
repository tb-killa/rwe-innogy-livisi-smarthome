using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;

public class MessageParameter
{
	public const string MESSAGE_PARAMETER_ID_COLUMN = "Id";

	public const string MESSAGE_PARAMETER_MESSAGE_ID_COLUMN = "MessageId";

	public const string MESSAGE_PARAMETER_KEY_COLUMN = "Key";

	public const string MESSAGE_PARAMETER_VALUE_COLUMN = "Value";

	public const string TABLE = "MessageParameters";

	public const string PRIMARY_KEY = "MessagesParameters_PKey";

	[XmlIgnore]
	public Guid Id { get; set; }

	[XmlAttribute]
	public MessageParameterKey Key { get; set; }

	[XmlAttribute]
	public string Value { get; set; }

	public MessageParameter()
	{
		Id = Guid.NewGuid();
	}

	public MessageParameter(MessageParameterKey key, string value)
	{
		Id = Guid.NewGuid();
		Key = key;
		Value = value;
	}

	public MessageParameter Clone()
	{
		MessageParameter messageParameter = new MessageParameter(Key, Value);
		messageParameter.Id = Id;
		return messageParameter;
	}
}
