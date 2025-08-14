using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.ValueDescription;

[Serializable]
[XmlType(Namespace = "urn:value_descriptionxsd")]
[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
public class stringFormatType
{
	private string[] valid_valueField;

	private uint max_lengthField;

	[XmlElement("valid_value")]
	public string[] valid_value
	{
		get
		{
			return valid_valueField;
		}
		set
		{
			valid_valueField = value;
		}
	}

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
