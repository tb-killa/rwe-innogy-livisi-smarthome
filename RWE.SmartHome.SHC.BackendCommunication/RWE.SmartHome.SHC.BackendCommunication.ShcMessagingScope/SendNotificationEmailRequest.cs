using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "SendNotificationEmail", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
public class SendNotificationEmailRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerialNo;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public EmailTemplates emailTemplate;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public DateTime localDate;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public int shcTimeOffset;

	[XmlArray(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 4)]
	[XmlArrayItem(Namespace = "http://schemas.datacontract.org/2004/07/System.Collections.Generic", IsNullable = false)]
	public KeyValuePairOfstringstring[] templateParameters;

	public SendNotificationEmailRequest()
	{
	}

	public SendNotificationEmailRequest(string shcSerialNo, EmailTemplates emailTemplate, DateTime localDate, int shcTimeOffset, KeyValuePairOfstringstring[] templateParameters)
	{
		this.shcSerialNo = shcSerialNo;
		this.emailTemplate = emailTemplate;
		this.localDate = localDate;
		this.shcTimeOffset = shcTimeOffset;
		this.templateParameters = templateParameters;
	}
}
