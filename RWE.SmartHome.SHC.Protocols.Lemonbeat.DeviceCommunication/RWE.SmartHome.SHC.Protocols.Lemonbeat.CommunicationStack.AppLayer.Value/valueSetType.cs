using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Value;

[Serializable]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:valuexsd")]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class valueSetType
{
	private uint value_idField;

	private ulong timestampField;

	private double numberField;

	private bool numberFieldSpecified;

	private string stringField;

	private byte[] hexBinaryField;

	[XmlAttribute]
	public uint value_id
	{
		get
		{
			return value_idField;
		}
		set
		{
			value_idField = value;
		}
	}

	[XmlAttribute]
	public ulong timestamp
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
	public double number
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
	public byte[] hexBinary
	{
		get
		{
			return hexBinaryField;
		}
		set
		{
			hexBinaryField = value;
		}
	}
}
