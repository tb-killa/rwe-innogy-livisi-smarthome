using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;

namespace RWE.SmartHome.SHC.Lemonbeat.ProtocolAdapter.Interfaces;

public interface IConfigurationController
{
	void ConfigureDevices(IEnumerable<Guid> devicesToRemove, IEnumerable<Guid> devicesToInclude, IDictionary<Guid, PhysicalConfiguration> configurationsToApply);

	void ResetDevicePartners(Guid deviceId, IRepository configRepository);
}
