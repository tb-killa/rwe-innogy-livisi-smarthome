using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calculation;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:calculationxsd", IncludeInSchema = false)]
public enum ItemsChoiceType
{
	calculation_delete,
	calculation_get,
	calculation_get_memory,
	calculation_report,
	calculation_report_memory,
	calculation_set
}
