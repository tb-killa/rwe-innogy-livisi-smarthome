namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.HEC;

public class HECTemperatureDeviceSettings : HECSettings
{
	public decimal PositiveTemperatureOffset { get; set; }

	public decimal NegativeTemperatureOffset { get; set; }

	public decimal PowerPerDegree { get; set; }

	protected override HECSettings CreateInstance()
	{
		return new HECTemperatureDeviceSettings();
	}

	protected override void TransferProperties(HECSettings instance)
	{
		base.TransferProperties(instance);
		((HECTemperatureDeviceSettings)instance).PositiveTemperatureOffset = PositiveTemperatureOffset;
		((HECTemperatureDeviceSettings)instance).NegativeTemperatureOffset = NegativeTemperatureOffset;
		((HECTemperatureDeviceSettings)instance).PowerPerDegree = PowerPerDegree;
	}
}
