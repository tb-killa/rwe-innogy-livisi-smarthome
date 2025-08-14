using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.StateMachine;

[Serializable]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:statemachinexsd")]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class statemachineGetStateType
{
	private uint statemachine_idField;

	private bool statemachine_idFieldSpecified;

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
}
