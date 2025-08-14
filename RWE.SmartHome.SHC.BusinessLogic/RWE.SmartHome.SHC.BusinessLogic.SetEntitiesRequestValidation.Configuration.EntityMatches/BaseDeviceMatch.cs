using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.EntityMatches;

public class BaseDeviceMatch : EntityMatch<BaseDevice>
{
	[XmlAttribute("type")]
	public string Type { get; set; }

	public override bool Match(BaseDevice repositoryEntity)
	{
		if (repositoryEntity == null)
		{
			return false;
		}
		return IsAttributeMatched(Type, repositoryEntity.DeviceType);
	}
}
