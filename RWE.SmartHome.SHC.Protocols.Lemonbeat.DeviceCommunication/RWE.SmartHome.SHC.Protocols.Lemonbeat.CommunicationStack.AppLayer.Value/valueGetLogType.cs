using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Value;

[Serializable]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:valuexsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
public class valueGetLogType
{
	private uint value_idField;

	private bool value_idFieldSpecified;

	private ulong start_timeField;

	private bool start_timeFieldSpecified;

	private uint log_countField;

	private bool log_countFieldSpecified;

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
	public ulong start_time
	{
		get
		{
			return start_timeField;
		}
		set
		{
			start_timeField = value;
		}
	}

	[XmlIgnore]
	public bool start_timeSpecified
	{
		get
		{
			return start_timeFieldSpecified;
		}
		set
		{
			start_timeFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public uint log_count
	{
		get
		{
			return log_countField;
		}
		set
		{
			log_countField = value;
		}
	}

	[XmlIgnore]
	public bool log_countSpecified
	{
		get
		{
			return log_countFieldSpecified;
		}
		set
		{
			log_countFieldSpecified = value;
		}
	}
}
