using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.FirmwareUpdate;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:firmware_updatexsd")]
[DebuggerStepThrough]
public class firmwareInformationReportType
{
	private uint sizeField;

	private uint firmware_idField;

	private uint received_sizeField;

	private uint chunk_sizeField;

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

	[XmlAttribute]
	public uint received_size
	{
		get
		{
			return received_sizeField;
		}
		set
		{
			received_sizeField = value;
		}
	}

	[XmlAttribute]
	public uint chunk_size
	{
		get
		{
			return chunk_sizeField;
		}
		set
		{
			chunk_sizeField = value;
		}
	}
}
