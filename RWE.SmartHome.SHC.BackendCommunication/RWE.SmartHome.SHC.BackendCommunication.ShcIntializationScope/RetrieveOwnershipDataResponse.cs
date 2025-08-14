using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ShcIntializationScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[DebuggerStepThrough]
[XmlRoot(ElementName = "RetrieveOwnershipDataResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class RetrieveOwnershipDataResponse
{
	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 0)]
	public InitializationErrorCode RetrieveOwnershipDataResult;

	[XmlElement(IsNullable = true, Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 1)]
	public ShcSyncRecord shcSyncRecord;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 2)]
	public bool furtherPollingRequired;

	[XmlElement(Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices", Order = 3)]
	public int pollAfterSeconds;

	public RetrieveOwnershipDataResponse()
	{
	}

	public RetrieveOwnershipDataResponse(InitializationErrorCode RetrieveOwnershipDataResult, ShcSyncRecord shcSyncRecord, bool furtherPollingRequired, int pollAfterSeconds)
	{
		this.RetrieveOwnershipDataResult = RetrieveOwnershipDataResult;
		this.shcSyncRecord = shcSyncRecord;
		this.furtherPollingRequired = furtherPollingRequired;
		this.pollAfterSeconds = pollAfterSeconds;
	}
}
