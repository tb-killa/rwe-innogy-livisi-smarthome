using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Timer;

[Serializable]
[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:timerxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class executeType
{
	private uint timer_idField;

	private uint afterField;

	private uint calculation_idField;

	private bool calculation_idFieldSpecified;

	private uint action_idField;

	private bool action_idFieldSpecified;

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

	[XmlAttribute]
	public uint after
	{
		get
		{
			return afterField;
		}
		set
		{
			afterField = value;
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
	public uint action_id
	{
		get
		{
			return action_idField;
		}
		set
		{
			action_idField = value;
		}
	}

	[XmlIgnore]
	public bool action_idSpecified
	{
		get
		{
			return action_idFieldSpecified;
		}
		set
		{
			action_idFieldSpecified = value;
		}
	}
}
