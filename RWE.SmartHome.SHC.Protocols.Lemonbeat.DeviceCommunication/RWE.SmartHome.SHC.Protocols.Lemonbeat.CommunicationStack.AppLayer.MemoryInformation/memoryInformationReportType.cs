using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.MemoryInformation;

[Serializable]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:memory_informationxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
public class memoryInformationReportType
{
	private memoryInformationType[] memory_informationField;

	[XmlElement("memory_information")]
	public memoryInformationType[] memory_information
	{
		get
		{
			return memory_informationField;
		}
		set
		{
			memory_informationField = value;
		}
	}
}
