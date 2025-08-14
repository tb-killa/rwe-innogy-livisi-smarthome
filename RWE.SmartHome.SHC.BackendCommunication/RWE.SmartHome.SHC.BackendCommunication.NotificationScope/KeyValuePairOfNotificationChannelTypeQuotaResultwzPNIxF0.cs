using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.NotificationScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.datacontract.org/2004/07/System.Collections.Generic")]
[DebuggerStepThrough]
public class KeyValuePairOfNotificationChannelTypeQuotaResultwzPNIxF0
{
	private NotificationChannelType keyField;

	private QuotaResult valueField;

	[XmlElement(Order = 0)]
	public NotificationChannelType key
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
	public QuotaResult value
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
