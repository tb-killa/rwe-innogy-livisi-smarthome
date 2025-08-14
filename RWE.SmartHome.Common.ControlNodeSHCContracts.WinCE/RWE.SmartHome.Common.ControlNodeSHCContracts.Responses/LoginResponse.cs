using System.Collections.Generic;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.GlobalContracts;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

public class LoginResponse : BaseResponse
{
	[XmlAttribute]
	public string SessionId { get; set; }

	public List<ShcRole> UserRoles { get; set; }

	[XmlAttribute]
	public int CurrentConfigurationVersion { get; set; }

	[XmlAttribute]
	public string TokenHash { get; set; }

	public BaseDevice ShcDevice { get; set; }
}
