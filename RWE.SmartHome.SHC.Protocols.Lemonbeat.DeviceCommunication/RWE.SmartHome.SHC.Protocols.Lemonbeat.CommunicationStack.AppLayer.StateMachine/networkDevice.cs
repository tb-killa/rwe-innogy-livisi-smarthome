using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.StateMachine;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "urn:statemachinexsd")]
public class networkDevice
{
	private object[] itemsField;

	private ItemsChoiceType[] itemsElementNameField;

	private uint versionField;

	private uint device_idField;

	private bool device_idFieldSpecified;

	private uint go_to_sleepField;

	private bool go_to_sleepFieldSpecified;

	[XmlElement("statemachine_set", typeof(statemachineSetType))]
	[XmlElement("statemachine_report_state_memory", typeof(statemachineMemoryReportType))]
	[XmlChoiceIdentifier("ItemsElementName")]
	[XmlElement("statemachine_set_state", typeof(statemachineSetStateType))]
	[XmlElement("statemachine_report_state", typeof(statemachineReportStateType))]
	[XmlElement("statemachine_delete", typeof(statemachineDeleteType))]
	[XmlElement("statemachine_get", typeof(statemachineGetType))]
	[XmlElement("statemachine_get_memory", typeof(statemachineMemoryGetType))]
	[XmlElement("statemachine_get_state", typeof(statemachineGetStateType))]
	[XmlElement("statemachine_get_state_memory", typeof(statemachineMemoryGetType))]
	[XmlElement("statemachine_report", typeof(statemachineReportType))]
	[XmlElement("statemachine_report_memory", typeof(statemachineMemoryReportType))]
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
