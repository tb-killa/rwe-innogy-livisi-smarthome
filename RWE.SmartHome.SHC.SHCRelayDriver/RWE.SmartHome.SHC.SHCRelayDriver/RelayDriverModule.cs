using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.SHCRelayDriver;

public class RelayDriverModule : IModule
{
	public void Configure(Container container)
	{
		try
		{
			container.Register("RelayDriver", (Func<Container, IService>)delegate(Container c)
			{
				RelayDriver relayDriver = new RelayDriver(container);
				c.Resolve<ITaskManager>().Register(relayDriver);
				c.Resolve<IChannelMultiplexer>().AddCommunicationChannel(relayDriver);
				return relayDriver;
			}).InitializedBy(delegate(Container c, IService v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			container.ResolveNamed<IService>("RelayDriver");
			Log.Information(Module.RelayDriver, "Initialization of Relay driver successful");
		}
		catch (Exception ex)
		{
			Log.Error(Module.RelayDriver, $"Initialization of Relay driver failed: {ex.Message}");
		}
	}
}
