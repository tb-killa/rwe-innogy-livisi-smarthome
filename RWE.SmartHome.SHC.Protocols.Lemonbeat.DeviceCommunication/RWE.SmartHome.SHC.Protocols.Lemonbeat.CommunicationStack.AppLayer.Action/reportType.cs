using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Action;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:actionxsd")]
public class reportType
{
	private uint my_value_idField;

	private uint partner_idField;

	private bool partner_idFieldSpecified;

	private uint transport_modeField;

	private bool transport_modeFieldSpecified;

	[XmlAttribute]
	public uint my_value_id
	{
		get
		{
			return my_value_idField;
		}
		set
		{
			my_value_idField = value;
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
