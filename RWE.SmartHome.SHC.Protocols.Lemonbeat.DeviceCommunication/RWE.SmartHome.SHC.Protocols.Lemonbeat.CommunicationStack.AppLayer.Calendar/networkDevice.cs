using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calendar;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:calendarxsd")]
public class networkDevice
{
	private object[] itemsField;

	private ItemsChoiceType[] itemsElementNameField;

	private uint versionField;

	private uint device_idField;

	private bool device_idFieldSpecified;

	private uint go_to_sleepField;

	private bool go_to_sleepFieldSpecified;

	[XmlElement("calendar_report_timezone", typeof(calendarTimezoneReportType))]
	[XmlElement("calendar_report", typeof(calendarReportType))]
	[XmlElement("calendar_set_timezone", typeof(calendarTimezoneSetType))]
	[XmlElement("calendar_report_memory", typeof(calendarMemoryReportType))]
	[XmlChoiceIdentifier("ItemsElementName")]
	[XmlElement("calendar_set", typeof(calendarSetType))]
	[XmlElement("calendar_delete", typeof(calendarDeleteType))]
	[XmlElement("calendar_get", typeof(calendarGetType))]
	[XmlElement("calendar_get_memory", typeof(calendarMemoryGetType))]
	[XmlElement("calendar_get_timezone", typeof(calendarTimezoneGetType))]
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
