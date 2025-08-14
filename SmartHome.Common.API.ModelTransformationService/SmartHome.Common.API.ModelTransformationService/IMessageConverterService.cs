using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using SmartHome.Common.API.Entities.Entities;

namespace SmartHome.Common.API.ModelTransformationService;

public interface IMessageConverterService
{
	SmartHome.Common.API.Entities.Entities.Message FromSmartHomeMessage(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message message);
}
