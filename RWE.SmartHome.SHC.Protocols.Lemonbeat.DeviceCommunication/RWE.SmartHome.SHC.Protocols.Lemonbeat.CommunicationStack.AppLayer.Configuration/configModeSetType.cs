using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Configuration;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:configurationxsd")]
public class configModeSetType
{
	private uint modeField;

	[XmlAttribute]
	public uint mode
	{
		get
		{
			return modeField;
		}
		set
		{
			modeField = value;
		}
	}
}
