namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.HEC;

public class HECElectricDeviceSettings : HECSettings
{
	public HECOperations AllowedOperations { get; set; }

	public decimal AveragePower { get; set; }

	public decimal StandbyPower { get; set; }

	public decimal OnStartTime { get; set; }

	public decimal OnEndTimeOffset { get; set; }

	public decimal MinContinuousOnTime { get; set; }

	public decimal MinTotalOnTime { get; set; }

	protected override HECSettings CreateInstance()
	{
		return new HECElectricDeviceSettings();
	}

	protected override void TransferProperties(HECSettings instance)
	{
		base.TransferProperties(instance);
		((HECElectricDeviceSettings)instance).AllowedOperations = AllowedOperations;
		((HECElectricDeviceSettings)instance).AveragePower = AveragePower;
		((HECElectricDeviceSettings)instance).StandbyPower = StandbyPower;
		((HECElectricDeviceSettings)instance).OnStartTime = OnStartTime;
		((HECElectricDeviceSettings)instance).OnEndTimeOffset = OnEndTimeOffset;
		((HECElectricDeviceSettings)instance).MinContinuousOnTime = MinContinuousOnTime;
		((HECElectricDeviceSettings)instance).MinTotalOnTime = MinTotalOnTime;
	}
}
