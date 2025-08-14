using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Actions;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface IActionService
{
	IEnumerable<LemonbeatAction> GetAllActions(DeviceIdentifier identifier);

	void SetAndDeleteActions(DeviceIdentifier identifier, ICollection<LemonbeatAction> actionsToSet, IEnumerable<uint> actionsToDelete);
}
