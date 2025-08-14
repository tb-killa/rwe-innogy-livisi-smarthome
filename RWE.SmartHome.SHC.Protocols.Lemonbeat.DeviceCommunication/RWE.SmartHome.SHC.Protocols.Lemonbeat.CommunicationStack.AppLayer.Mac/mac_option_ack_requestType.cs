using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Mac;

[Serializable]
[XmlType(Namespace = "urn:macxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class mac_option_ack_requestType
{
	private uint nr_retriesField;

	[XmlAttribute]
	public uint nr_retries
	{
		get
		{
			return nr_retriesField;
		}
		set
		{
			nr_retriesField = value;
		}
	}
}
