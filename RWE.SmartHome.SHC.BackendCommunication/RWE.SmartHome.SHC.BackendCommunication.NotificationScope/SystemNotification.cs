using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.NotificationScope;

[Serializable]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/11/08/DomainObjects")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class SystemNotification
{
	private string classField;

	private KeyValuePairOfstringstring[] parameteresField;

	private string productIdField;

	private string typeField;

	[XmlElement(IsNullable = true, Order = 0)]
	public string Class
	{
		get
		{
			return classField;
		}
		set
		{
			classField = value;
		}
	}

	[XmlArrayItem(Namespace = "http://schemas.datacontract.org/2004/07/System.Collections.Generic", IsNullable = false)]
	[XmlArray(IsNullable = true, Order = 1)]
	public KeyValuePairOfstringstring[] Parameteres
	{
		get
		{
			return parameteresField;
		}
		set
		{
			parameteresField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 2)]
	public string ProductId
	{
		get
		{
			return productIdField;
		}
		set
		{
			productIdField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 3)]
	public string Type
	{
		get
		{
			return typeField;
		}
		set
		{
			typeField = value;
		}
	}
}
