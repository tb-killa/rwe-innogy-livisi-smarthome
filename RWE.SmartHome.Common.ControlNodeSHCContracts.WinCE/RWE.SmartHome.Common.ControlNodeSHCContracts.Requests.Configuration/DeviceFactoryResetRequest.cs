using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;

public class DeviceFactoryResetRequest : BaseRequest
{
	[XmlAttribute]
	public Guid DeviceId { get; set; }

	[XmlAttribute]
	public DeviceFactoryResetCommand Command { get; set; }
}
