using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.StateMachine;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:statemachinexsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class statemachineType
{
	private stateType[] itemsField;

	private uint statemachine_idField;

	[XmlElement("state")]
	public stateType[] Items
	{
		get
		{
			return itemsField;
		}
		set
		{
			itemsField = value;
		}
	}

	[XmlAttribute]
	public uint statemachine_id
	{
		get
		{
			return statemachine_idField;
		}
		set
		{
			statemachine_idField = value;
		}
	}
}
