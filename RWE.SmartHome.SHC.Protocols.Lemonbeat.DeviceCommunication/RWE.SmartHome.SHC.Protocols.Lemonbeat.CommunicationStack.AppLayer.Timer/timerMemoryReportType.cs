using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Timer;

[Serializable]
[XmlType(Namespace = "urn:timerxsd")]
[DesignerCategory("code")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
public class timerMemoryReportType
{
	private uint countField;

	private uint free_countField;

	[XmlAttribute]
	public uint count
	{
		get
		{
			return countField;
		}
		set
		{
			countField = value;
		}
	}

	[XmlAttribute]
	public uint free_count
	{
		get
		{
			return free_countField;
		}
		set
		{
			free_countField = value;
		}
	}
}
