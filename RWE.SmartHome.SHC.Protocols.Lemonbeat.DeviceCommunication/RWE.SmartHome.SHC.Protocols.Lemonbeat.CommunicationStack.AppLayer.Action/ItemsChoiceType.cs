using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Action;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:actionxsd", IncludeInSchema = false)]
public enum ItemsChoiceType
{
	get,
	invoke,
	report,
	set,
	timer_start,
	timer_stop
}
