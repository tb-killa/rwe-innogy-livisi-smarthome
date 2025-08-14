using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/11/08/Common")]
public class ShcRef
{
	private string refIdField;

	[XmlElement(Order = 0)]
	public string RefId
	{
		get
		{
			return refIdField;
		}
		set
		{
			refIdField = value;
		}
	}
}
