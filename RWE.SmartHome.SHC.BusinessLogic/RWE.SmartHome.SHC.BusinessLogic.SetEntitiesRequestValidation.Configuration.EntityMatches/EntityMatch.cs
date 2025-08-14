using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.EntityMatches;

[XmlInclude(typeof(LogicalDeviceMatch))]
[XmlInclude(typeof(BaseDeviceMatch))]
public abstract class EntityMatch<T> where T : Entity
{
	public abstract bool Match(T repositoryEntity);

	protected bool IsAttributeMatched(string matchAttribute, string propertyAttribute)
	{
		if (string.IsNullOrEmpty(matchAttribute) || matchAttribute == "*")
		{
			return true;
		}
		if (matchAttribute == propertyAttribute)
		{
			return true;
		}
		return false;
	}
}
