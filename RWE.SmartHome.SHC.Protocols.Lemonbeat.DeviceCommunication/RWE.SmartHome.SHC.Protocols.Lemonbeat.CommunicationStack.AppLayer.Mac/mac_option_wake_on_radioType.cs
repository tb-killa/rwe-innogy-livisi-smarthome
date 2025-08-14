using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Mac;

[Serializable]
[XmlType(Namespace = "urn:macxsd")]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
public class mac_option_wake_on_radioType
{
	private uint timestampField;

	private uint fractionField;

	private uint intervalField;

	private uint channelField;

	[XmlAttribute]
	public uint timestamp
	{
		get
		{
			return timestampField;
		}
		set
		{
			timestampField = value;
		}
	}

	[XmlAttribute]
	public uint fraction
	{
		get
		{
			return fractionField;
		}
		set
		{
			fractionField = value;
		}
	}

	[XmlAttribute]
	public uint interval
	{
		get
		{
			return intervalField;
		}
		set
		{
			intervalField = value;
		}
	}

	[XmlAttribute]
	public uint channel
	{
		get
		{
			return channelField;
		}
		set
		{
			channelField = value;
		}
	}
}
