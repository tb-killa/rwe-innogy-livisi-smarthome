using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.ApplicationsHostInterfaces;

public class AddinUpdateData
{
	public string AddinFilePath { get; set; }

	public string Type { get; set; }

	public string ManifestFile { get; set; }

	public ApplicationsToken AppsToken { get; set; }
}
