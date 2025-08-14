using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.ChannelScan;

[Serializable]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:channel_scanxsd")]
[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class channelScanReportType
{
	private byte[] rssi_valuesField;

	private byte[] channel_mapField;

	[XmlElement(DataType = "hexBinary")]
	public byte[] rssi_values
	{
		get
		{
			return rssi_valuesField;
		}
		set
		{
			rssi_valuesField = value;
		}
	}

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
