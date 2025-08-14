using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Action;

[Serializable]
[XmlType(Namespace = "urn:actionxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class actionInvokeType
{
	private uint action_idField;

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
}
