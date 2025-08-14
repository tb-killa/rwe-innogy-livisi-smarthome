namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities;

public class ShcStatus
{
	public bool Connected { get; set; }

	public string AppVersion { get; set; }

	public string OsVersion { get; set; }

	public string ConfigVersion { get; set; }
}
