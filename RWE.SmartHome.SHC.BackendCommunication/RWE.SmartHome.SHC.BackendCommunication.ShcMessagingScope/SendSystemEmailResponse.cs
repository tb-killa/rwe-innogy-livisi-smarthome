using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcMessagingScope;

[XmlRoot(ElementName = "SendSystemEmailResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class SendSystemEmailResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public MessageAppResultCode SendSystemEmailResult;

	public SendSystemEmailResponse()
	{
	}

	public SendSystemEmailResponse(MessageAppResultCode SendSystemEmailResult)
	{
		this.SendSystemEmailResult = SendSystemEmailResult;
	}
}
