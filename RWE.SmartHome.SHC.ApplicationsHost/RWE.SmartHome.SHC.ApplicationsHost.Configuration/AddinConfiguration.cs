using System.Collections.Generic;
using System.Linq;
using SmartHome.SHC.API.Configuration;

namespace RWE.SmartHome.SHC.ApplicationsHost.Configuration;

public class AddinConfiguration
{
	public string AppId { get; private set; }

	public List<Device> Devices { get; private set; }

	public List<Capability> Capabilities { get; private set; }

	public List<ActionDescription> ActionDescriptions { get; private set; }

	public List<Trigger> Triggers { get; private set; }

	public List<CustomTrigger> CustomTriggers { get; private set; }

	public AddinConfiguration(string appId)
	{
		AppId = appId;
		ActionDescriptions = new List<ActionDescription>();
		Capabilities = new List<Capability>();
		CustomTriggers = new List<CustomTrigger>();
		Devices = new List<Device>();
		Triggers = new List<Trigger>();
	}

	internal AddinConfiguration Clone()
	{
		AddinConfiguration addinConfiguration = new AddinConfiguration(AppId);
		addinConfiguration.Devices = Devices.ToList();
		addinConfiguration.Capabilities = Capabilities.ToList();
		addinConfiguration.ActionDescriptions = ActionDescriptions.ToList();
		addinConfiguration.Triggers = Triggers.ToList();
		addinConfiguration.CustomTriggers = CustomTriggers.ToList();
		return addinConfiguration;
	}
}
