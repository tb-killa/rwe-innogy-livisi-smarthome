using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.PartnerInformation;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:partner_informationxsd")]
[DebuggerStepThrough]
public class networkDevice
{
	private object[] itemsField;

	private uint versionField;

	private uint device_idField;

	private bool device_idFieldSpecified;

	private uint go_to_sleepField;

	private bool go_to_sleepFieldSpecified;

	[XmlElement("partner_information_get_memory", typeof(partnerInformationMemoryGetType))]
	[XmlElement("partner_information_delete", typeof(partnerInformationDeleteType))]
	[XmlElement("partner_information_get", typeof(partnerInformationGetType))]
	[XmlElement("partner_information_report", typeof(partnerInformationReportType))]
	[XmlElement("partner_information_report_memory", typeof(partnerInformationMemoryReportType))]
	[XmlElement("partner_information_set", typeof(partnerInformationSetType))]
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
