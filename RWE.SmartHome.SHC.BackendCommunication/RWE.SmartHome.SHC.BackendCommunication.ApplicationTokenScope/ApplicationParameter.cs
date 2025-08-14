using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2011/11/15/ApplicationManagement")]
public class ApplicationParameter
{
	private string keyField;

	private string valueField;

	[XmlElement(IsNullable = true, Order = 0)]
	public string Key
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
	public string Value
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
