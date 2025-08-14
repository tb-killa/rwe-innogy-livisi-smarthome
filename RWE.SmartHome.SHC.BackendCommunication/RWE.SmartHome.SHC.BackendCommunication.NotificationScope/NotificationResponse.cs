using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.NotificationScope;

[Serializable]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/11/08/DomainObjects")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
public class NotificationResponse
{
	private KeyValuePairOfNotificationChannelTypeQuotaResultwzPNIxF0[] quotaResultField;

	private int remainingQuotaField;

	private bool remainingQuotaFieldSpecified;

	private NotificationSendResult resultField;

	private bool resultFieldSpecified;

	[XmlArray(IsNullable = true, Order = 0)]
	[XmlArrayItem(Namespace = "http://schemas.datacontract.org/2004/07/System.Collections.Generic", IsNullable = false)]
	public KeyValuePairOfNotificationChannelTypeQuotaResultwzPNIxF0[] QuotaResult
	{
		get
		{
			return quotaResultField;
		}
		set
		{
			quotaResultField = value;
		}
	}

	[XmlElement(Order = 1)]
	public int RemainingQuota
	{
		get
		{
			return remainingQuotaField;
		}
		set
		{
			remainingQuotaField = value;
		}
	}

	[XmlIgnore]
	public bool RemainingQuotaSpecified
	{
		get
		{
			return remainingQuotaFieldSpecified;
		}
		set
		{
			remainingQuotaFieldSpecified = value;
		}
	}

	[XmlElement(Order = 2)]
	public NotificationSendResult Result
	{
		get
		{
			return resultField;
		}
		set
		{
			resultField = value;
		}
	}

	[XmlIgnore]
	public bool ResultSpecified
	{
		get
		{
			return resultFieldSpecified;
		}
		set
		{
			resultFieldSpecified = value;
		}
	}
}
