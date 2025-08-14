using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calendar;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:calendarxsd", IncludeInSchema = false)]
public enum ItemsChoiceType
{
	calendar_delete,
	calendar_get,
	calendar_get_memory,
	calendar_get_timezone,
	calendar_report,
	calendar_report_memory,
	calendar_report_timezone,
	calendar_set,
	calendar_set_timezone
}
