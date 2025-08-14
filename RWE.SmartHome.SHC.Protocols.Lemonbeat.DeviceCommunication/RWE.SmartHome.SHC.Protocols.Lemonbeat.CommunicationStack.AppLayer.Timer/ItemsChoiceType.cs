using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Timer;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:timerxsd", IncludeInSchema = false)]
public enum ItemsChoiceType
{
	timer_delete,
	timer_get,
	timer_get_memory,
	timer_report,
	timer_report_memory,
	timer_set
}
