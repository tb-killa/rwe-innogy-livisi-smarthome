using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;

public class RestoreConfigurationRequest : BaseRequest
{
	[XmlAttribute]
	public Guid RestorePointId { get; set; }

	[XmlAttribute]
	public bool CreateRestorePoint { get; set; }
}
