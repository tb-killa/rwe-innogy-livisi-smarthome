using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.CommunicationStack.AppLayer.Calculation;

[Serializable]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "urn:calculationxsd")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class calculationType
{
	private calSubType leftField;

	private calSubType rightField;

	private uint calculation_idField;

	private uint method_idField;

	public calSubType left
	{
		get
		{
			return leftField;
		}
		set
		{
			leftField = value;
		}
	}

	public calSubType right
	{
		get
		{
			return rightField;
		}
		set
		{
			rightField = value;
		}
	}

	[XmlAttribute]
	public uint calculation_id
	{
		get
		{
			return calculation_idField;
		}
		set
		{
			calculation_idField = value;
		}
	}

	[XmlAttribute]
	public uint method_id
	{
		get
		{
			return method_idField;
		}
		set
		{
			method_idField = value;
		}
	}
}
