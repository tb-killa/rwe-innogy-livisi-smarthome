using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calendar;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:calendarxsd")]
public class calendarGetType
{
	private uint task_idField;

	private bool task_idFieldSpecified;

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

	[XmlIgnore]
	public bool task_idSpecified
	{
		get
		{
			return task_idFieldSpecified;
		}
		set
		{
			task_idFieldSpecified = value;
		}
	}
}
