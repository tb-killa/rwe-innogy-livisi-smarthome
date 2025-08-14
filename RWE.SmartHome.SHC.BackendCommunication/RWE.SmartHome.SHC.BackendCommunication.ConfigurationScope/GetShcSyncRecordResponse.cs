using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ConfigurationScope;

[DebuggerStepThrough]
[XmlRoot(ElementName = "GetShcSyncRecordResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class GetShcSyncRecordResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public ConfigurationResultCode GetShcSyncRecordResult;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public ShcSyncRecord syncRecord;

	public GetShcSyncRecordResponse()
	{
	}

	public GetShcSyncRecordResponse(ConfigurationResultCode GetShcSyncRecordResult, ShcSyncRecord syncRecord)
	{
		this.GetShcSyncRecordResult = GetShcSyncRecordResult;
		this.syncRecord = syncRecord;
	}
}
