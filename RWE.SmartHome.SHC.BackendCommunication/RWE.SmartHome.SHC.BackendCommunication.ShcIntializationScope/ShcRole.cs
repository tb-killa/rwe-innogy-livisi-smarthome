using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[Serializable]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.datacontract.org/2004/07/RWE.SmartHome.Common.GlobalContracts")]
[DebuggerStepThrough]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
public class ShcRole
{
	private string idField;

	private string nameField;

	[XmlElement(Order = 0)]
	public string Id
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 1)]
	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}
}
