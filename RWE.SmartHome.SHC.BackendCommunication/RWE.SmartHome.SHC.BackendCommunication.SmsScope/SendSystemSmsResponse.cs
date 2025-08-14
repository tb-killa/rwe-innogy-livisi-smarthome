using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.SmsScope;

[XmlRoot(ElementName = "SendSystemSmsResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class SendSystemSmsResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public MessageAppResultCode SendSystemSmsResult;

	public SendSystemSmsResponse()
	{
	}

	public SendSystemSmsResponse(MessageAppResultCode SendSystemSmsResult)
	{
		this.SendSystemSmsResult = SendSystemSmsResult;
	}
}
