using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService;

public interface IStatusConverterService
{
	Status ToApiModel(ShcStatus shcStatus);
}
