using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Mac;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:macxsd")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class mac_option_channel_mapType
{
	private uint typeField;

	private byte[] mapField;

	[XmlAttribute]
	public uint type
	{
		get
		{
			return typeField;
		}
		set
		{
			typeField = value;
		}
	}

	[XmlAttribute(DataType = "hexBinary")]
	public byte[] map
	{
		get
		{
			return mapField;
		}
		set
		{
			mapField = value;
		}
	}
}
