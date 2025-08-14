using JsonLite;

namespace RWE.SmartHome.SHC.Core.LocalCommunication;

public class ShcInitialization
{
	[JsonProperty("tacAccepted")]
	public bool TearmsOfAgreementAccepted { get; set; }

	[JsonProperty("configName")]
	public string ConfigurationBackUpFile { get; set; }
}
