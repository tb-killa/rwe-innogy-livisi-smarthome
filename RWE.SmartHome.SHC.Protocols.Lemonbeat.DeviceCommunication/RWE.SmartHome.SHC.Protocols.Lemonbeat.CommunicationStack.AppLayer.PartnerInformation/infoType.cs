using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.PartnerInformation;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:partner_informationxsd")]
[DebuggerStepThrough]
public class infoType
{
	private uint type_idField;

	private ulong numberField;

	private bool numberFieldSpecified;

	private string stringField;

	private byte[] hexField;

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
	public ulong number
	{
		get
		{
			return numberField;
		}
		set
		{
			numberField = value;
		}
	}

	[XmlIgnore]
	public bool numberSpecified
	{
		get
		{
			return numberFieldSpecified;
		}
		set
		{
			numberFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public string @string
	{
		get
		{
			return stringField;
		}
		set
		{
			stringField = value;
		}
	}

	[XmlAttribute(DataType = "hexBinary")]
	public byte[] hex
	{
		get
		{
			return hexField;
		}
		set
		{
			hexField = value;
		}
	}
}
