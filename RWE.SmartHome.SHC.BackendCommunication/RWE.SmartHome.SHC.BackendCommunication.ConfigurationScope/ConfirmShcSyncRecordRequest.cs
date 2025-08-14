using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[XmlRoot(ElementName = "ConfirmShcSyncRecord", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class ConfirmShcSyncRecordRequest
{
	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public string shcSerial;

	public ConfirmShcSyncRecordRequest()
	{
	}

	public ConfirmShcSyncRecordRequest(string shcSerial)
	{
		this.shcSerial = shcSerial;
	}
}
