using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Action;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:actionxsd", IncludeInSchema = false)]
public enum ItemsChoiceType1
{
	action_delete,
	action_get,
	action_get_memory,
	action_invoke,
	action_report,
	action_report_memory,
	action_set
}
