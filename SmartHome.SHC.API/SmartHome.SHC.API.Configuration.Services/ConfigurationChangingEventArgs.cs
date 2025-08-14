using System;
using System.Collections.Generic;

namespace SmartHome.SHC.API.Configuration.Services;

public class ConfigurationChangingEventArgs : EventArgs
{
	public IEnumerable<Capability> Capabilities { get; private set; }

	public IEnumerable<Device> Devices { get; private set; }

	public IEnumerable<ActionDescription> ActionDescriptions { get; private set; }

	public IEnumerable<Trigger> Triggers { get; private set; }

	public List<ConfigurationError> Errors { get; private set; }

	public ConfigurationChangingEventArgs(IEnumerable<Capability> capabilities, IEnumerable<Device> devices, IEnumerable<ActionDescription> actionDescriptions, IEnumerable<Trigger> triggers)
	{
		Capabilities = capabilities;
		Devices = devices;
		ActionDescriptions = actionDescriptions;
		Triggers = triggers;
		Errors = new List<ConfigurationError>();
	}
}
