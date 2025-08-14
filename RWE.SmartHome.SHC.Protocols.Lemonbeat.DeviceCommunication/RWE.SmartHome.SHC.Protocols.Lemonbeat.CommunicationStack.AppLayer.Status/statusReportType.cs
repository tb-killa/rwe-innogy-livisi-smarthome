using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Status;

[Serializable]
[XmlType(Namespace = "urn:statusxsd")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class statusReportType
{
	private uint type_idField;

	private uint codeField;

	private uint levelField;

	private byte[] dataField;

	[XmlAttribute]
	public uint type_id
	{
		get
		{
			return type_idField;
		}
		set
		{
			type_idField = value;
		}
	}

	[XmlAttribute]
	public uint code
	{
		get
		{
			return codeField;
		}
		set
		{
			codeField = value;
		}
	}

	[XmlAttribute]
	public uint level
	{
		get
		{
			return levelField;
		}
		set
		{
			levelField = value;
		}
	}

	[XmlAttribute(DataType = "hexBinary")]
	public byte[] data
	{
		get
		{
			return dataField;
		}
		set
		{
			dataField = value;
		}
	}
}
