using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[DebuggerStepThrough]
[XmlRoot(ElementName = "ConfirmShcSyncRecordResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class ConfirmShcSyncRecordResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public ConfigurationResultCode ConfirmShcSyncRecordResult;

	public ConfirmShcSyncRecordResponse()
	{
	}

	public ConfirmShcSyncRecordResponse(ConfigurationResultCode ConfirmShcSyncRecordResult)
	{
		this.ConfirmShcSyncRecordResult = ConfirmShcSyncRecordResult;
	}
}
