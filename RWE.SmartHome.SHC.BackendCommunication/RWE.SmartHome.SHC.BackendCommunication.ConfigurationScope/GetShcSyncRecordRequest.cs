using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "GetShcSyncRecord", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class GetShcSyncRecordRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerial;

	public GetShcSyncRecordRequest()
	{
	}

	public GetShcSyncRecordRequest(string shcSerial)
	{
		this.shcSerial = shcSerial;
	}
}
