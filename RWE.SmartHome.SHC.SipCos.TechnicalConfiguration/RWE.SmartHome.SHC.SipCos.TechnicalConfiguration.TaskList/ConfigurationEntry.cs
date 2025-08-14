using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.TaskList;

internal class ConfigurationEntry
{
	public DeviceConfiguration ReferenceConfig { get; set; }

	public DeviceConfiguration CurrentConfig { get; set; }

	public DeviceConfigurationDiff ReferenceToCurrentDiff { get; set; }

	public List<LinkUpdatePendingPatch> LinkUpdatePendingConfig { get; set; }

	public byte[] Address { get; set; }

	public Dictionary<Guid, ConfigurationChange> PendingChanges { get; private set; }

	public ConfigurationEntry()
	{
		PendingChanges = new Dictionary<Guid, ConfigurationChange>();
	}
}
