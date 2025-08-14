using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.FirmwareUpdate;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:firmware_updatexsd")]
[DesignerCategory("code")]
public class firmwareDataType
{
	private byte[] chunkField;

	private uint offsetField;

	[XmlElement(DataType = "hexBinary")]
	public byte[] chunk
	{
		get
		{
			return chunkField;
		}
		set
		{
			chunkField = value;
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
