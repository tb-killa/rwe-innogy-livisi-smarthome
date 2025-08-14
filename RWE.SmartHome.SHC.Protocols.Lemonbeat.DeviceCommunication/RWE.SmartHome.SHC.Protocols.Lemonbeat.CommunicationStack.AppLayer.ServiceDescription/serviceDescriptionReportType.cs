using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.ServiceDescription;

[Serializable]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:service_descriptionxsd")]
[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class serviceDescriptionReportType
{
	private serviceType[] serviceField;

	[XmlElement("service")]
	public serviceType[] service
	{
		get
		{
			return serviceField;
		}
		set
		{
			serviceField = value;
		}
	}
}
