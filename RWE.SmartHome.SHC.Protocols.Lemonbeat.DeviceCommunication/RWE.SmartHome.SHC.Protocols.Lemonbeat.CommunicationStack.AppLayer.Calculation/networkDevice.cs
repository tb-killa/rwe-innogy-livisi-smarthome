using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calculation;

[Serializable]
[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "urn:calculationxsd")]
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

	[XmlElement("calculation_set", typeof(calculationSetType))]
	[XmlChoiceIdentifier("ItemsElementName")]
	[XmlElement("calculation_get", typeof(calculationGetType))]
	[XmlElement("calculation_report_memory", typeof(calculationMemoryReportType))]
	[XmlElement("calculation_delete", typeof(calculationDeleteType))]
	[XmlElement("calculation_get_memory", typeof(calculationMemoryGetType))]
	[XmlElement("calculation_report", typeof(calculationReportType))]
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

	[XmlElement("ItemsElementName")]
	[XmlIgnore]
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
