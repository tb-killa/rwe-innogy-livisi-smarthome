using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcType;

namespace RWE.SmartHome.SHC.Core.TypeManager;

public interface IRestrictionManager
{
	List<ShcTypeParameterState> GetRestrictionState(ShcRestriction restriction);
}
