using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage")]
public class DeviceActivityLog
{
	private string activityTypeField;

	private string deviceIdField;

	private EventType eventTypeField;

	private bool eventTypeFieldSpecified;

	private string newStateField;

	private DateTime timestampField;

	private bool timestampFieldSpecified;

	[XmlElement(IsNullable = true, Order = 0)]
	public string ActivityType
	{
		get
		{
			return activityTypeField;
		}
		set
		{
			activityTypeField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 1)]
	public string DeviceId
	{
		get
		{
			return deviceIdField;
		}
		set
		{
			deviceIdField = value;
		}
	}

	[XmlElement(Order = 2)]
	public EventType EventType
	{
		get
		{
			return eventTypeField;
		}
		set
		{
			eventTypeField = value;
		}
	}

	[XmlIgnore]
	public bool EventTypeSpecified
	{
		get
		{
			return eventTypeFieldSpecified;
		}
		set
		{
			eventTypeFieldSpecified = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 3)]
	public string NewState
	{
		get
		{
			return newStateField;
		}
		set
		{
			newStateField = value;
		}
	}

	[XmlElement(Order = 4)]
	public DateTime Timestamp
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

	[XmlIgnore]
	public bool TimestampSpecified
	{
		get
		{
			return timestampFieldSpecified;
		}
		set
		{
			timestampFieldSpecified = value;
		}
	}
}
