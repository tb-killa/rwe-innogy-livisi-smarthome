using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface IMemoryInformationService
{
	List<MemoryInformation> GetMemoryInformation(DeviceIdentifier identifier);
}
