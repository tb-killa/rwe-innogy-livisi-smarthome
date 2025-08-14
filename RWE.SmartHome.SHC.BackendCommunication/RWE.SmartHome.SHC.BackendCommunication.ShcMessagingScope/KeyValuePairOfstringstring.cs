using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[Serializable]
[XmlType(Namespace = "http://schemas.datacontract.org/2004/07/System.Collections.Generic")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class KeyValuePairOfstringstring
{
	private string keyField;

	private string valueField;

	[XmlElement(IsNullable = true, Order = 0)]
	public string key
	{
		get
		{
			return keyField;
		}
		set
		{
			keyField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 1)]
	public string value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
		}
	}
}
