using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.NotificationScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2010/11/08/DomainObjects")]
public class CustomNotification
{
	private string bodyField;

	private NotificationChannelType channelField;

	private bool channelFieldSpecified;

	private string[] customRecipientsField;

	private string titleField;

	private string[] userNamesField;

	[XmlElement(IsNullable = true, Order = 0)]
	public string Body
	{
		get
		{
			return bodyField;
		}
		set
		{
			bodyField = value;
		}
	}

	[XmlElement(Order = 1)]
	public NotificationChannelType Channel
	{
		get
		{
			return channelField;
		}
		set
		{
			channelField = value;
		}
	}

	[XmlIgnore]
	public bool ChannelSpecified
	{
		get
		{
			return channelFieldSpecified;
		}
		set
		{
			channelFieldSpecified = value;
		}
	}

	[XmlArrayItem(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
	[XmlArray(IsNullable = true, Order = 2)]
	public string[] CustomRecipients
	{
		get
		{
			return customRecipientsField;
		}
		set
		{
			customRecipientsField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 3)]
	public string Title
	{
		get
		{
			return titleField;
		}
		set
		{
			titleField = value;
		}
	}

	[XmlArray(IsNullable = true, Order = 4)]
	[XmlArrayItem(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
	public string[] UserNames
	{
		get
		{
			return userNamesField;
		}
		set
		{
			userNamesField = value;
		}
	}
}
