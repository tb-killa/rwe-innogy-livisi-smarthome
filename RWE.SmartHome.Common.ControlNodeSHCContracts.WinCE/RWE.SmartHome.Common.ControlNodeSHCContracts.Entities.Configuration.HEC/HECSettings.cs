using System.Xml.Serialization;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.HEC;

[XmlInclude(typeof(HECTemperatureDeviceSettings))]
[XmlInclude(typeof(HECElectricDeviceSettings))]
[XmlInclude(typeof(HECGenericDeviceSettings))]
public class HECSettings
{
	public bool HECEnabled { get; set; }

	public HECSettings Clone()
	{
		HECSettings hECSettings = CreateInstance();
		TransferProperties(hECSettings);
		return hECSettings;
	}

	protected virtual HECSettings CreateInstance()
	{
		return new HECSettings();
	}

	protected virtual void TransferProperties(HECSettings instance)
	{
		instance.HECEnabled = HECEnabled;
	}
}
