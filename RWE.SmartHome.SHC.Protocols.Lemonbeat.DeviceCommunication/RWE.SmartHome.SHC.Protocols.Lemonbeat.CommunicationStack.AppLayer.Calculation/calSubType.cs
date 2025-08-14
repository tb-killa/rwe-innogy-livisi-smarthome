using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calculation;

[Serializable]
[XmlType(Namespace = "urn:calculationxsd")]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
public class calSubType
{
	private uint value_idField;

	private bool value_idFieldSpecified;

	private uint calculation_idField;

	private bool calculation_idFieldSpecified;

	private uint partner_idField;

	private bool partner_idFieldSpecified;

	private uint statemachine_idField;

	private bool statemachine_idFieldSpecified;

	private uint timer_idField;

	private bool timer_idFieldSpecified;

	private uint calender_idField;

	private bool calender_idFieldSpecified;

	private byte is_updatedField;

	private bool is_updatedFieldSpecified;

	private double constant_numberField;

	private bool constant_numberFieldSpecified;

	private string constant_stringField;

	private byte[] constant_hexBinaryField;

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
	public uint statemachine_id
	{
		get
		{
			return statemachine_idField;
		}
		set
		{
			statemachine_idField = value;
		}
	}

	[XmlIgnore]
	public bool statemachine_idSpecified
	{
		get
		{
			return statemachine_idFieldSpecified;
		}
		set
		{
			statemachine_idFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public uint timer_id
	{
		get
		{
			return timer_idField;
		}
		set
		{
			timer_idField = value;
		}
	}

	[XmlIgnore]
	public bool timer_idSpecified
	{
		get
		{
			return timer_idFieldSpecified;
		}
		set
		{
			timer_idFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public uint calender_id
	{
		get
		{
			return calender_idField;
		}
		set
		{
			calender_idField = value;
		}
	}

	[XmlIgnore]
	public bool calender_idSpecified
	{
		get
		{
			return calender_idFieldSpecified;
		}
		set
		{
			calender_idFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public byte is_updated
	{
		get
		{
			return is_updatedField;
		}
		set
		{
			is_updatedField = value;
		}
	}

	[XmlIgnore]
	public bool is_updatedSpecified
	{
		get
		{
			return is_updatedFieldSpecified;
		}
		set
		{
			is_updatedFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public double constant_number
	{
		get
		{
			return constant_numberField;
		}
		set
		{
			constant_numberField = value;
		}
	}

	[XmlIgnore]
	public bool constant_numberSpecified
	{
		get
		{
			return constant_numberFieldSpecified;
		}
		set
		{
			constant_numberFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public string constant_string
	{
		get
		{
			return constant_stringField;
		}
		set
		{
			constant_stringField = value;
		}
	}

	[XmlAttribute(DataType = "hexBinary")]
	public byte[] constant_hexBinary
	{
		get
		{
			return constant_hexBinaryField;
		}
		set
		{
			constant_hexBinaryField = value;
		}
	}
}
