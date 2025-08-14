using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.ChannelScan;

[Serializable]
[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:channel_scanxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class channelScanGetType
{
	private byte[] channel_mapField;

	[XmlAttribute(DataType = "hexBinary")]
	public byte[] channel_map
	{
		get
		{
			return channel_mapField;
		}
		set
		{
			channel_mapField = value;
		}
	}
}
