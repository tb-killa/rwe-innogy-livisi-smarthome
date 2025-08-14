using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Mac;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:macxsd")]
public class mac_option_fragmentType
{
	private uint is_lastField;

	private uint offsetField;

	[XmlAttribute]
	public uint is_last
	{
		get
		{
			return is_lastField;
		}
		set
		{
			is_lastField = value;
		}
	}

	[XmlAttribute]
	public uint offset
	{
		get
		{
			return offsetField;
		}
		set
		{
			offsetField = value;
		}
	}
}
