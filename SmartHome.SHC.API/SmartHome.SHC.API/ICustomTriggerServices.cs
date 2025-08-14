using System;

namespace SmartHome.SHC.API;

public interface ICustomTriggerServices
{
	void FireCustomTrigger(Guid triggerId);
}
