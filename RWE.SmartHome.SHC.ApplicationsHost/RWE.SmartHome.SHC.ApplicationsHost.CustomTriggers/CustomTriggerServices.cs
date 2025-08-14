using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.CoreEvents;
using RWE.SmartHome.SHC.Core;
using SmartHome.SHC.API;

namespace RWE.SmartHome.SHC.ApplicationsHost.CustomTriggers;

internal class CustomTriggerServices : ICustomTriggerServices
{
	private readonly IEventManager eventManager;

	public CustomTriggerServices(IEventManager eventManager)
	{
		this.eventManager = eventManager;
	}

	public void FireCustomTrigger(Guid triggerId)
	{
		eventManager.GetEvent<CustomTriggerEvent>().Publish(new CustomTriggerEventArgs(triggerId));
	}
}
