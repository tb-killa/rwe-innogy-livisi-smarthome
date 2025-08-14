using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[Serializable]
[DesignerCategory("code")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2012/04/15/ShcTableStorage")]
[DebuggerStepThrough]
public class Property
{
	private string nameField;

	private object valueField;

	[XmlElement(IsNullable = true, Order = 0)]
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

	[XmlElement(IsNullable = true, Order = 1)]
	public object Value
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
