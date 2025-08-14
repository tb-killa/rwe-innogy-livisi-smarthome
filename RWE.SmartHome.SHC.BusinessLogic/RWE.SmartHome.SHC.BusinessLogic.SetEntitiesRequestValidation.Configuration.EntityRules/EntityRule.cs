using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.EntityRules;

[XmlInclude(typeof(BaseDeviceRule))]
[XmlInclude(typeof(InteractionRule))]
[XmlInclude(typeof(LogicalDeviceRule))]
public abstract class EntityRule<T> where T : Entity
{
	public abstract ValidationResult Check(T entity, T repositoryEntity);
}
