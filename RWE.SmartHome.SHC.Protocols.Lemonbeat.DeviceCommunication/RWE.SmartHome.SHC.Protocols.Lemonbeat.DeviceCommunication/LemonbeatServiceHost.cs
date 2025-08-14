using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication;

public class LemonbeatServiceHost : IServiceLocator
{
	private readonly List<ILemonbeatService> services = new List<ILemonbeatService>();

	public void AddService(ILemonbeatService LemonbeatService)
	{
		services.Add(LemonbeatService);
	}

	public T GetService<T>() where T : class
	{
		return services.Find((ILemonbeatService service) => service is T) as T;
	}
}
