using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Action;

[Serializable]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:actionxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
public class setType
{
	private uint value_idField;

	private double numberField;

	private bool numberFieldSpecified;

	private string stringField;

	private byte[] hexBinaryField;

	private uint partner_idField;

	private bool partner_idFieldSpecified;

	private uint calculation_idField;

	private bool calculation_idFieldSpecified;

	private uint transport_modeField;

	private bool transport_modeFieldSpecified;

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

	[XmlAttribute]
	public uint partner_id
	{
		get
		{
			return partner_idField;
		}
		set
		{
			partner_idField = value;
		}
	}

	[XmlIgnore]
	public bool partner_idSpecified
	{
		get
		{
			return partner_idFieldSpecified;
		}
		set
		{
			partner_idFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public uint calculation_id
	{
		get
		{
			return calculation_idField;
		}
		set
		{
			calculation_idField = value;
		}
	}

	[XmlIgnore]
	public bool calculation_idSpecified
	{
		get
		{
			return calculation_idFieldSpecified;
		}
		set
		{
			calculation_idFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public uint transport_mode
	{
		get
		{
			return transport_modeField;
		}
		set
		{
			transport_modeField = value;
		}
	}

	[XmlIgnore]
	public bool transport_modeSpecified
	{
		get
		{
			return transport_modeFieldSpecified;
		}
		set
		{
			transport_modeFieldSpecified = value;
		}
	}
}
