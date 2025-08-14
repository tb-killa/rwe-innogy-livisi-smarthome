using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface ITimerService
{
	IEnumerable<LemonbeatTimer> GetAllTimers(DeviceIdentifier identifier);

	void SetAndDeleteTimers(DeviceIdentifier identifier, IEnumerable<LemonbeatTimer> timersToSet, IEnumerable<uint> timersToDelete);
}
