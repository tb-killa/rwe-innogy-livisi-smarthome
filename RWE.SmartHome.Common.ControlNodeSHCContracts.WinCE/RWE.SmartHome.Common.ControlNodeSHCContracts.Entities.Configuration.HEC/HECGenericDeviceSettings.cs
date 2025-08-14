using System.Collections.Generic;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.HEC;

public class HECGenericDeviceSettings : HECSettings
{
	public List<Property> Settings { get; private set; }

	public HECGenericDeviceSettings()
	{
		Settings = new List<Property>();
	}

	protected override HECSettings CreateInstance()
	{
		return new HECGenericDeviceSettings();
	}

	protected override void TransferProperties(HECSettings instance)
	{
		base.TransferProperties(instance);
		Settings.ForEach(delegate(Property p)
		{
			((HECGenericDeviceSettings)instance).Settings.Add(p.Clone());
		});
	}
}
