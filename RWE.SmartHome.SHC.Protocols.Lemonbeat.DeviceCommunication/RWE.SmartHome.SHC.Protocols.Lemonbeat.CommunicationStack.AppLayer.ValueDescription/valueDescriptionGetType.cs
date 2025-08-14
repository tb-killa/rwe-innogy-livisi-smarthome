using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.ValueDescription;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:value_descriptionxsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class valueDescriptionGetType
{
	private uint value_description_idField;

	private bool value_description_idFieldSpecified;

	[XmlAttribute]
	public uint value_description_id
	{
		get
		{
			return value_description_idField;
		}
		set
		{
			value_description_idField = value;
		}
	}

	[XmlIgnore]
	public bool value_description_idSpecified
	{
		get
		{
			return value_description_idFieldSpecified;
		}
		set
		{
			value_description_idFieldSpecified = value;
		}
	}
}
