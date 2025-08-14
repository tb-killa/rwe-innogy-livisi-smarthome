using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;

public abstract class BaseRequest
{
	[XmlAttribute]
	public string Version { get; set; }

	[XmlAttribute]
	public Guid RequestId { get; set; }

	protected BaseRequest()
	{
		RequestId = Guid.NewGuid();
		Version = "3.00";
	}
}
