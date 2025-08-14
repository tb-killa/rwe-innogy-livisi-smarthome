using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.ServiceDescription;

[Serializable]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:service_descriptionxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
public class serviceType
{
	private uint service_idField;

	private uint versionField;

	[XmlAttribute]
	public uint service_id
	{
		get
		{
			return service_idField;
		}
		set
		{
			service_idField = value;
		}
	}

	[XmlAttribute]
	public uint version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}
}
