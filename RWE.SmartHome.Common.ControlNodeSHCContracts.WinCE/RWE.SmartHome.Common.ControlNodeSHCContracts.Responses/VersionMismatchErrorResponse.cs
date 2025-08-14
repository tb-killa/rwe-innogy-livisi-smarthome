using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public class VersionMismatchErrorResponse : ErrorResponse
{
	[XmlAttribute]
	public string DetectedVersion { get; set; }

	[XmlAttribute]
	public string ExpectedVersion { get; set; }

	public VersionMismatchErrorResponse()
	{
	}

	public VersionMismatchErrorResponse(string detectedVersion, string expectedVersion)
	{
		DetectedVersion = detectedVersion;
		ExpectedVersion = expectedVersion;
	}
}
