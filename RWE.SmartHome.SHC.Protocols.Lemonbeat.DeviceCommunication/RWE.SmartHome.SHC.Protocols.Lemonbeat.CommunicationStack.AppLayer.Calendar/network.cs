using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calendar;

[Serializable]
[XmlRoot(Namespace = "urn:calendarxsd", IsNullable = false)]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:calendarxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
public class network
{
	private networkDevice[] deviceField;

	private uint versionField;

	[XmlElement("device")]
	public networkDevice[] device
	{
		get
		{
			return deviceField;
		}
		set
		{
			deviceField = value;
		}
	}

	[XmlAttribute]
	public uint version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}
}
