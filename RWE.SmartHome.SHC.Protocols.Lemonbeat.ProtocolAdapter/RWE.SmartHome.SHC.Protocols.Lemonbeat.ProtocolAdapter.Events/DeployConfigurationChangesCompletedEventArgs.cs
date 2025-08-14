using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Configuration;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;

public class DeployConfigurationChangesCompletedEventArgs : EventArgs
{
	public Guid DeviceId { get; private set; }

	public List<ServiceType> DeployedServices { get; private set; }

	public PhysicalConfigurationDifference DeployedDifference { get; private set; }

	public DeployConfigurationChangesCompletedEventArgs(Guid deviceId, List<ServiceType> deployedServices, PhysicalConfigurationDifference deployedDifference)
	{
		DeviceId = deviceId;
		DeployedServices = deployedServices;
		DeployedDifference = deployedDifference;
	}
}
