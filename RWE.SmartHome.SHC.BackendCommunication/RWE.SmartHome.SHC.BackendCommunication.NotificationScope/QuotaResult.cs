using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.NotificationScope;

[Serializable]
[DesignerCategory("code")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/11/08/Common/Notifications")]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
public class QuotaResult
{
	private int remainingQuotaField;

	private bool remainingQuotaFieldSpecified;

	private NotificationSendResult resultField;

	private bool resultFieldSpecified;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
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
