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
public class actionGetType
{
	private uint action_idField;

	private bool action_idFieldSpecified;

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
}
