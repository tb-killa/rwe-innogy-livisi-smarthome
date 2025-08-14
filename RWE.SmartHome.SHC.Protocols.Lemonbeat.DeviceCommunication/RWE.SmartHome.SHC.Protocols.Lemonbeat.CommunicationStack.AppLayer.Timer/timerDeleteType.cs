using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Timer;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:timerxsd")]
public class timerDeleteType
{
	private uint timer_idField;

	private bool timer_idFieldSpecified;

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
}
