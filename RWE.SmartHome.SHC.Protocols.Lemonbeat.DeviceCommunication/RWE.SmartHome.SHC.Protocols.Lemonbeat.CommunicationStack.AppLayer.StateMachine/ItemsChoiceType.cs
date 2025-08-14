using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.StateMachine;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:statemachinexsd", IncludeInSchema = false)]
public enum ItemsChoiceType
{
	statemachine_delete,
	statemachine_get,
	statemachine_get_memory,
	statemachine_get_state,
	statemachine_get_state_memory,
	statemachine_report,
	statemachine_report_memory,
	statemachine_report_state,
	statemachine_report_state_memory,
	statemachine_set,
	statemachine_set_state
}
