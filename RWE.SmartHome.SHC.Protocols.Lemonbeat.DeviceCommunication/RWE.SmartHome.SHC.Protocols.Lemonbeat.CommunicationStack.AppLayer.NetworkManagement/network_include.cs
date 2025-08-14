using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.NetworkManagement;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:network_managementxsd")]
[DebuggerStepThrough]
public class network_include
{
	private byte address_sizeField;

	private bool address_sizeFieldSpecified;

	private uint inclusion_countField;

	private bool inclusion_countFieldSpecified;

	private byte[] valueField;

	[XmlAttribute]
	public byte address_size
	{
		get
		{
			return address_sizeField;
		}
		set
		{
			address_sizeField = value;
		}
	}

	[XmlIgnore]
	public bool address_sizeSpecified
	{
		get
		{
			return address_sizeFieldSpecified;
		}
		set
		{
			address_sizeFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public uint inclusion_count
	{
		get
		{
			return inclusion_countField;
		}
		set
		{
			inclusion_countField = value;
		}
	}

	[XmlIgnore]
	public bool inclusion_countSpecified
	{
		get
		{
			return inclusion_countFieldSpecified;
		}
		set
		{
			inclusion_countFieldSpecified = value;
		}
	}

	[XmlText(DataType = "hexBinary")]
	public byte[] Value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
		}
	}
}
