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
[XmlType(Namespace = "urn:statemachinexsd")]
public class statemachineSetStateType
{
	private statemachineStateType[] statemachine_stateField;

	[XmlElement("statemachine_state")]
	public statemachineStateType[] statemachine_state
	{
		get
		{
			return statemachine_stateField;
		}
		set
		{
			statemachine_stateField = value;
		}
	}
}
