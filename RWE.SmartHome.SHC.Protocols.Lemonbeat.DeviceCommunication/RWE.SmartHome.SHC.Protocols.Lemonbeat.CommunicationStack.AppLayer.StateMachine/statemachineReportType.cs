using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.StateMachine;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:statemachinexsd")]
[DesignerCategory("code")]
public class statemachineReportType
{
	private statemachineType[] statemachineField;

	[XmlElement("statemachine")]
	public statemachineType[] statemachine
	{
		get
		{
			return statemachineField;
		}
		set
		{
			statemachineField = value;
		}
	}
}
