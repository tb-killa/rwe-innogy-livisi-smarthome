using System;
using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Control;

public class TriggerSensorRequest : BaseRequest
{
	[XmlAttribute]
	public Guid SensorGuid { get; set; }

	[XmlAttribute]
	public int ButtonId { get; set; }
}
