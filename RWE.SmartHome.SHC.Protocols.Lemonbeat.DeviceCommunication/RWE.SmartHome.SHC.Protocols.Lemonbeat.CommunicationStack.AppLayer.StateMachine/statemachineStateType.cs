using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.StateMachine;

[Serializable]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:statemachinexsd")]
[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class statemachineStateType
{
	private uint statemachine_idField;

	private uint valueField;

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

	[XmlText]
	public uint Value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
		}
	}
}
