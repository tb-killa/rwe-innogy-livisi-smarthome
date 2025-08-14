using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Action;

[Serializable]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:actionxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
public class networkDevice
{
	private object[] itemsField;

	private ItemsChoiceType1[] itemsElementNameField;

	private uint versionField;

	private uint device_idField;

	private bool device_idFieldSpecified;

	private uint go_to_sleepField;

	private bool go_to_sleepFieldSpecified;

	[XmlElement("action_report", typeof(actionReportType))]
	[XmlElement("action_set", typeof(actionSetType))]
	[XmlChoiceIdentifier("ItemsElementName")]
	[XmlElement("action_report_memory", typeof(actionMemoryReportType))]
	[XmlElement("action_delete", typeof(actionDeleteType))]
	[XmlElement("action_invoke", typeof(actionInvokeType))]
	[XmlElement("action_get_memory", typeof(actionMemoryGetType))]
	[XmlElement("action_get", typeof(actionGetType))]
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
	public ItemsChoiceType1[] ItemsElementName
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
	public uint version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}

	[XmlAttribute]
	public uint device_id
	{
		get
		{
			return device_idField;
		}
		set
		{
			device_idField = value;
		}
	}

	[XmlIgnore]
	public bool device_idSpecified
	{
		get
		{
			return device_idFieldSpecified;
		}
		set
		{
			device_idFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public uint go_to_sleep
	{
		get
		{
			return go_to_sleepField;
		}
		set
		{
			go_to_sleepField = value;
		}
	}

	[XmlIgnore]
	public bool go_to_sleepSpecified
	{
		get
		{
			return go_to_sleepFieldSpecified;
		}
		set
		{
			go_to_sleepFieldSpecified = value;
		}
	}
}
