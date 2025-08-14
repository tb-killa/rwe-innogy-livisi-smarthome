using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.Extensions;

public static class EntityMetadataExtensions
{
	private static readonly ILogger logger = LogManager.Instance.GetLogger(typeof(EntityMetadataExtensions));

	public static List<string> ToEntityLinks(this List<EntityMetadata> entityMetadata)
	{
		List<string> list = new List<string>();
		foreach (EntityMetadata entityMetadatum in entityMetadata)
		{
			switch (entityMetadatum.EntityType)
			{
			case EntityType.BaseDevice:
				list.Add(string.Format("/device/{0}", entityMetadatum.Id.ToString("N")));
				break;
			case EntityType.LogicalDevice:
				list.Add(string.Format("/capability/{0}", entityMetadatum.Id.ToString("N")));
				break;
			case EntityType.Interaction:
				list.Add(string.Format("/interaction/{0}", entityMetadatum.Id.ToString("N")));
				break;
			case EntityType.Location:
				list.Add(string.Format("/location/{0}", entityMetadatum.Id.ToString("N")));
				break;
			case EntityType.Home:
				list.Add(string.Format("/home/{0}", entityMetadatum.Id.ToString("N")));
				break;
			case EntityType.HomeSetup:
				list.Add(string.Format("/home/setup/{0}", entityMetadatum.Id.ToString("N")));
				break;
			case EntityType.Member:
				list.Add(string.Format("/home/member/{0}", entityMetadatum.Id.ToString("N")));
				break;
			default:
				logger.Warn($"Unsupported entity type in configuration saved/changed event list: {entityMetadatum.EntityType}. Ignoring entity...");
				break;
			}
		}
		return list;
	}
}
