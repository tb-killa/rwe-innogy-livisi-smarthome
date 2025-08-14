using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.FirmwareUpdate;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:firmware_updatexsd")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class firmwareInitType
{
	private uint sizeField;

	private byte[] checksumField;

	private uint firmware_idField;

	[XmlAttribute]
	public uint size
	{
		get
		{
			return sizeField;
		}
		set
		{
			sizeField = value;
		}
	}

	[XmlAttribute(DataType = "hexBinary")]
	public byte[] checksum
	{
		get
		{
			return checksumField;
		}
		set
		{
			checksumField = value;
		}
	}

	[XmlAttribute]
	public uint firmware_id
	{
		get
		{
			return firmware_idField;
		}
		set
		{
			firmware_idField = value;
		}
	}
}
