using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.StateMachine;

[Serializable]
[XmlType(Namespace = "urn:statemachinexsd")]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
public class transactionType
{
	private uint calculation_idField;

	private bool calculation_idFieldSpecified;

	private uint action_idField;

	private bool action_idFieldSpecified;

	private uint goto_state_idField;

	private bool goto_state_idFieldSpecified;

	[XmlAttribute]
	public uint calculation_id
	{
		get
		{
			return calculation_idField;
		}
		set
		{
			calculation_idField = value;
		}
	}

	[XmlIgnore]
	public bool calculation_idSpecified
	{
		get
		{
			return calculation_idFieldSpecified;
		}
		set
		{
			calculation_idFieldSpecified = value;
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

	[XmlIgnore]
	public bool action_idSpecified
	{
		get
		{
			return action_idFieldSpecified;
		}
		set
		{
			action_idFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public uint goto_state_id
	{
		get
		{
			return goto_state_idField;
		}
		set
		{
			goto_state_idField = value;
		}
	}

	[XmlIgnore]
	public bool goto_state_idSpecified
	{
		get
		{
			return goto_state_idFieldSpecified;
		}
		set
		{
			goto_state_idFieldSpecified = value;
		}
	}
}
