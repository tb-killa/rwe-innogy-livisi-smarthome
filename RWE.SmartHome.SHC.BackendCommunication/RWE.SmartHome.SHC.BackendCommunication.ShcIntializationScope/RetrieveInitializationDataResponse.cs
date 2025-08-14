using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
[XmlRoot(ElementName = "RetrieveInitializationDataResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class RetrieveInitializationDataResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public InitializationErrorCode RetrieveInitializationDataResult;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public string issuedCertificate;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public ShcSyncRecord shcSyncRecord;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public bool furtherPollingRequired;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 4)]
	public int pollAfterSeconds;

	public RetrieveInitializationDataResponse()
	{
	}

	public RetrieveInitializationDataResponse(InitializationErrorCode RetrieveInitializationDataResult, string issuedCertificate, ShcSyncRecord shcSyncRecord, bool furtherPollingRequired, int pollAfterSeconds)
	{
		this.RetrieveInitializationDataResult = RetrieveInitializationDataResult;
		this.issuedCertificate = issuedCertificate;
		this.shcSyncRecord = shcSyncRecord;
		this.furtherPollingRequired = furtherPollingRequired;
		this.pollAfterSeconds = pollAfterSeconds;
	}
}
