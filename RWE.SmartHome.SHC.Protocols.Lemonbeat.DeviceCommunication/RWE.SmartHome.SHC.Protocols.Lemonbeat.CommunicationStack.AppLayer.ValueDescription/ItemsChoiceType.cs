using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.ValueDescription;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:value_descriptionxsd", IncludeInSchema = false)]
public enum ItemsChoiceType
{
	value_description_add,
	value_description_delete,
	value_description_get,
	value_description_get_memory,
	value_description_report,
	value_description_report_memory
}
