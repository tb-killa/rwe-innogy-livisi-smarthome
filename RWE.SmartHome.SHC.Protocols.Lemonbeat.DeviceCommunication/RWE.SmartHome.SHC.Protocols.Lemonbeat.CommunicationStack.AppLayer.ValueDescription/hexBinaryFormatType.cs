using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.ValueDescription;

[Serializable]
[XmlType(Namespace = "urn:value_descriptionxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class hexBinaryFormatType
{
	private uint max_lengthField;

	[XmlAttribute]
	public uint max_length
	{
		get
		{
			return max_lengthField;
		}
		set
		{
			max_lengthField = value;
		}
	}
}
