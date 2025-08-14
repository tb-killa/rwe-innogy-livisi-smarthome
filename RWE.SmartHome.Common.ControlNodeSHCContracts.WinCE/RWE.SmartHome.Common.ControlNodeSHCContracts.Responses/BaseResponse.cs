using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public abstract class BaseResponse
{
	[XmlAttribute]
	public string Version { get; set; }

	[XmlAttribute]
	public Guid CorrespondingRequestId { get; set; }

	protected BaseResponse()
	{
		Version = "3.00";
	}
}
