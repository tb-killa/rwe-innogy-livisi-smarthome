using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class TrackData
{
	private string deviceIdField;

	private string entityIdField;

	private string entityTypeField;

	private string eventTypeField;

	private Property[] propertiesField;

	private DateTime timestampField;

	private bool timestampFieldSpecified;

	[XmlElement(IsNullable = true, Order = 0)]
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

	[XmlElement(IsNullable = true, Order = 1)]
	public string EntityId
	{
		get
		{
			return entityIdField;
		}
		set
		{
			entityIdField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 2)]
	public string EntityType
	{
		get
		{
			return entityTypeField;
		}
		set
		{
			entityTypeField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 3)]
	public string EventType
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

	[XmlArray(IsNullable = true, Order = 4)]
	public Property[] Properties
	{
		get
		{
			return propertiesField;
		}
		set
		{
			propertiesField = value;
		}
	}

	[XmlElement(Order = 5)]
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
