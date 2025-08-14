using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Value;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:valuexsd")]
public class valueGetType
{
	private uint value_idField;

	private bool value_idFieldSpecified;

	[XmlAttribute]
	public uint value_id
	{
		get
		{
			return value_idField;
		}
		set
		{
			value_idField = value;
		}
	}

	[XmlIgnore]
	public bool value_idSpecified
	{
		get
		{
			return value_idFieldSpecified;
		}
		set
		{
			value_idFieldSpecified = value;
		}
	}
}
