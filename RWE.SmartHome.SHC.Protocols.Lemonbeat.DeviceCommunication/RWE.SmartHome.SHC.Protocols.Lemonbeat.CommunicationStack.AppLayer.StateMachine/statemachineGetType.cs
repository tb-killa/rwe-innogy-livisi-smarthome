using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.StateMachine;

[Serializable]
[XmlType(Namespace = "urn:statemachinexsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class statemachineGetType
{
	private uint statemachine_idField;

	private bool statemachine_idFieldSpecified;

	private uint state_idField;

	private bool state_idFieldSpecified;

	[XmlAttribute]
	public uint statemachine_id
	{
		get
		{
			return statemachine_idField;
		}
		set
		{
			statemachine_idField = value;
		}
	}

	[XmlIgnore]
	public bool statemachine_idSpecified
	{
		get
		{
			return statemachine_idFieldSpecified;
		}
		set
		{
			statemachine_idFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public uint state_id
	{
		get
		{
			return state_idField;
		}
		set
		{
			state_idField = value;
		}
	}

	[XmlIgnore]
	public bool state_idSpecified
	{
		get
		{
			return state_idFieldSpecified;
		}
		set
		{
			state_idFieldSpecified = value;
		}
	}
}
