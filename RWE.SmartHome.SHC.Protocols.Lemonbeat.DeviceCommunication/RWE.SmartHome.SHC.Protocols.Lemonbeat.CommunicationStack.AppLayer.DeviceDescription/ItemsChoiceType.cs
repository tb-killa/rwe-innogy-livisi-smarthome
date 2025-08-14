using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.DeviceDescription;

[Serializable]
[XmlType(Namespace = "urn:device_descriptionxsd", IncludeInSchema = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
public enum ItemsChoiceType
{
	device_description_get,
	device_description_report,
	device_description_set
}
