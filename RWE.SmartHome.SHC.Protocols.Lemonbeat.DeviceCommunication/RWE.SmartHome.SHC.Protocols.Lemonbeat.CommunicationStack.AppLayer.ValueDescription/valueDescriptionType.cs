using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.ValueDescription;

[Serializable]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:value_descriptionxsd")]
[DebuggerStepThrough]
public class valueDescriptionType
{
	private object itemField;

	private uint value_idField;

	private uint type_idField;

	private uint modeField;

	private uint persistentField;

	private string nameField;

	private uint min_log_intervalField;

	private bool min_log_intervalFieldSpecified;

	private uint max_log_valuesField;

	private bool max_log_valuesFieldSpecified;

	private uint virtualField;

	private bool virtualFieldSpecified;

	[XmlElement("number_format", typeof(numberFormatType))]
	[XmlElement("string_format", typeof(stringFormatType))]
	[XmlElement("hexBinary_format", typeof(hexBinaryFormatType))]
	public object Item
	{
		get
		{
			return itemField;
		}
		set
		{
			itemField = value;
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
	public uint mode
	{
		get
		{
			return modeField;
		}
		set
		{
			modeField = value;
		}
	}

	[XmlAttribute]
	public uint persistent
	{
		get
		{
			return persistentField;
		}
		set
		{
			persistentField = value;
		}
	}

	[XmlAttribute]
	public string name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	[XmlAttribute]
	public uint min_log_interval
	{
		get
		{
			return min_log_intervalField;
		}
		set
		{
			min_log_intervalField = value;
		}
	}

	[XmlIgnore]
	public bool min_log_intervalSpecified
	{
		get
		{
			return min_log_intervalFieldSpecified;
		}
		set
		{
			min_log_intervalFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public uint max_log_values
	{
		get
		{
			return max_log_valuesField;
		}
		set
		{
			max_log_valuesField = value;
		}
	}

	[XmlIgnore]
	public bool max_log_valuesSpecified
	{
		get
		{
			return max_log_valuesFieldSpecified;
		}
		set
		{
			max_log_valuesFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public uint @virtual
	{
		get
		{
			return virtualField;
		}
		set
		{
			virtualField = value;
		}
	}

	[XmlIgnore]
	public bool virtualSpecified
	{
		get
		{
			return virtualFieldSpecified;
		}
		set
		{
			virtualFieldSpecified = value;
		}
	}
}
