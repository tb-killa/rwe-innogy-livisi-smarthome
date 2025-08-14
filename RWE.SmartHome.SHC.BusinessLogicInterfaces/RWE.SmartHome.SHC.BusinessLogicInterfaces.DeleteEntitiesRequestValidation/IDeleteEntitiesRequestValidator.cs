using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.DeleteEntitiesRequestValidation;

public interface IDeleteEntitiesRequestValidator
{
	ValidationResult ValidateDeleteRequest(IEnumerable<EntityMetadata> entities);
}
