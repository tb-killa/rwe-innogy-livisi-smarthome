using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.Calculations;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface ICalculationService
{
	IEnumerable<Calculation> GetAllCalculations(DeviceIdentifier identifier);

	void SetAndDeleteCalculations(DeviceIdentifier identifier, IEnumerable<Calculation> toSet, IEnumerable<uint> toDelete);
}
