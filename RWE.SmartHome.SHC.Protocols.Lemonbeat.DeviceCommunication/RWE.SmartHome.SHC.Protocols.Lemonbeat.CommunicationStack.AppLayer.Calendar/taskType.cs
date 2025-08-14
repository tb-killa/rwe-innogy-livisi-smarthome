using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calendar;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:calendarxsd")]
[DesignerCategory("code")]
[DebuggerStepThrough]
public class taskType
{
	private uint task_idField;

	private ulong startField;

	private uint action_idField;

	private bool action_idFieldSpecified;

	private ulong endField;

	private bool endFieldSpecified;

	private uint repeatField;

	private bool repeatFieldSpecified;

	private byte weekdaysField;

	private bool weekdaysFieldSpecified;

	[XmlAttribute]
	public uint task_id
	{
		get
		{
			return task_idField;
		}
		set
		{
			task_idField = value;
		}
	}

	[XmlAttribute]
	public ulong start
	{
		get
		{
			return startField;
		}
		set
		{
			startField = value;
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

	[XmlAttribute]
	public ulong end
	{
		get
		{
			return endField;
		}
		set
		{
			endField = value;
		}
	}

	[XmlIgnore]
	public bool endSpecified
	{
		get
		{
			return endFieldSpecified;
		}
		set
		{
			endFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public uint repeat
	{
		get
		{
			return repeatField;
		}
		set
		{
			repeatField = value;
		}
	}

	[XmlIgnore]
	public bool repeatSpecified
	{
		get
		{
			return repeatFieldSpecified;
		}
		set
		{
			repeatFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public byte weekdays
	{
		get
		{
			return weekdaysField;
		}
		set
		{
			weekdaysField = value;
		}
	}

	[XmlIgnore]
	public bool weekdaysSpecified
	{
		get
		{
			return weekdaysFieldSpecified;
		}
		set
		{
			weekdaysFieldSpecified = value;
		}
	}
}
