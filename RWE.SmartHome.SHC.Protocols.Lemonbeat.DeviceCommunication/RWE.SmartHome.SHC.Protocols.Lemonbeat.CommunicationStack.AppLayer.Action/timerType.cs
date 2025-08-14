using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Action;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:actionxsd")]
[DebuggerStepThrough]
public class timerType
{
	private uint timer_idField;

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
}
