using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Action;

[Serializable]
[XmlType(Namespace = "urn:actionxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class actionType
{
	private object[] itemsField;

	private ItemsChoiceType[] itemsElementNameField;

	private uint action_idField;

	[XmlChoiceIdentifier("ItemsElementName")]
	[XmlElement("report", typeof(reportType))]
	[XmlElement("set", typeof(setType))]
	[XmlElement("timer_stop", typeof(timerType))]
	[XmlElement("get", typeof(getType))]
	[XmlElement("invoke", typeof(invokeType))]
	[XmlElement("timer_start", typeof(timerType))]
	public object[] Items
	{
		get
		{
			return itemsField;
		}
		set
		{
			itemsField = value;
		}
	}

	[XmlIgnore]
	[XmlElement("ItemsElementName")]
	public ItemsChoiceType[] ItemsElementName
	{
		get
		{
			return itemsElementNameField;
		}
		set
		{
			itemsElementNameField = value;
		}
	}

	[XmlAttribute]
	public uint action_id
	{
		get
		{
			return action_idField;
		}
		set
		{
			action_idField = value;
		}
	}
}
