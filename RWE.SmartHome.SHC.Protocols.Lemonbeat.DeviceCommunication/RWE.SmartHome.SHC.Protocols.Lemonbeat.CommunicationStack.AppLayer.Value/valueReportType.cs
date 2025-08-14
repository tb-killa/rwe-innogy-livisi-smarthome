using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Value;

[Serializable]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:valuexsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
public class valueReportType
{
	private ulong timestampField;

	private uint value_idField;

	private bool value_idFieldSpecified;

	private double numberField;

	private bool numberFieldSpecified;

	private string stringField;

	private byte[] hexBinaryField;

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

	[XmlIgnore]
	public bool value_idSpecified
	{
		get
		{
			return value_idFieldSpecified;
		}
		set
		{
			value_idFieldSpecified = value;
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
