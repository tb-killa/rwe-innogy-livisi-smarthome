using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.ShcType;

namespace RWE.SmartHome.SHC.Core.TypeManager;

public interface IShcTypeManager : IService
{
	bool IsUpdated { get; }

	RestrictionData GetRestrictionData(ShcRestriction restriction, string applicationId);

	void UpdateShcTypeData(Dictionary<string, Dictionary<string, string>> applicationParameters, long shcType);

	void SubscribeRestrictionManager(ShcRestriction restriction, IRestrictionManager restrictionManager);

	RestrictionState GetRestrictionState(ShcRestriction restriction);
}
