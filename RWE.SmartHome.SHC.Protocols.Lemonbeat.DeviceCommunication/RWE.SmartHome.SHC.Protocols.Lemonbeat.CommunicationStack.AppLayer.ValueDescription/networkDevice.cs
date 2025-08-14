using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.ValueDescription;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "urn:value_descriptionxsd")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class networkDevice
{
	private object[] itemsField;

	private ItemsChoiceType[] itemsElementNameField;

	private uint versionField;

	private uint device_idField;

	private bool device_idFieldSpecified;

	private uint go_to_sleepField;

	private bool go_to_sleepFieldSpecified;

	[XmlElement("value_description_delete", typeof(valueDescriptionDeleteType))]
	[XmlChoiceIdentifier("ItemsElementName")]
	[XmlElement("value_description_get_memory", typeof(valueDescriptionMemoryGetType))]
	[XmlElement("value_description_report", typeof(valueDescriptionReportType))]
	[XmlElement("value_description_add", typeof(valueDescriptionAddType))]
	[XmlElement("value_description_report_memory", typeof(valueDescriptionMemoryReportType))]
	[XmlElement("value_description_get", typeof(valueDescriptionGetType))]
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
