using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.StateMachine;

[Serializable]
[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(Namespace = "urn:statemachinexsd")]
[GeneratedCode("xsd", "2.0.50727.3038")]
public class stateType
{
	private transactionType[] transactionField;

	private uint state_idField;

	[XmlElement("transaction")]
	public transactionType[] transaction
	{
		get
		{
			return transactionField;
		}
		set
		{
			transactionField = value;
		}
	}

	[XmlAttribute]
	public uint state_id
	{
		get
		{
			return state_idField;
		}
		set
		{
			state_idField = value;
		}
	}
}
