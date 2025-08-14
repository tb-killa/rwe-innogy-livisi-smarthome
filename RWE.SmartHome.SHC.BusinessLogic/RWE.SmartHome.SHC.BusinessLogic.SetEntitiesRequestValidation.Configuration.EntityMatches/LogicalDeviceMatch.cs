using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.EntityMatches;

public class LogicalDeviceMatch : EntityMatch<LogicalDevice>
{
	public BaseDeviceMatch BaseDevice { get; set; }

	[XmlAttribute]
	public string Type { get; set; }

	public override bool Match(LogicalDevice repositoryEntity)
	{
		if (repositoryEntity == null)
		{
			return false;
		}
		if (!IsAttributeMatched(Type, repositoryEntity.DeviceType))
		{
			return false;
		}
		if (BaseDevice != null && (repositoryEntity.BaseDevice == null || !BaseDevice.Match(repositoryEntity.BaseDevice)))
		{
			return false;
		}
		return true;
	}
}
