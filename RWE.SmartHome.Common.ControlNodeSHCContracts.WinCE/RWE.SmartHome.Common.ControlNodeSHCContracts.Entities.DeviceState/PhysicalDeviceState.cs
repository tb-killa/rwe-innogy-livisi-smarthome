using System;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;

public class PhysicalDeviceState
{
	private PropertyBag deviceProperties = new PropertyBag();

	[XmlAttribute]
	public Guid PhysicalDeviceId { get; set; }

	public PropertyBag DeviceProperties
	{
		get
		{
			return deviceProperties;
		}
		set
		{
			deviceProperties = value;
		}
	}
}
