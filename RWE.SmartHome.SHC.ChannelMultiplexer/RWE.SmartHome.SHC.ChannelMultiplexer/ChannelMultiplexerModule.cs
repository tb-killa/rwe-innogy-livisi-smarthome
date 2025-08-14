using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.ChannelMultiplexer;

public class ChannelMultiplexerModule : IModule
{
	public void Configure(Container container)
	{
		container.Register((Func<Container, IChannelMultiplexer>)((Container c) => new ChannelMultiplexer())).ReusedWithin(ReuseScope.Container);
		container.Resolve<IChannelMultiplexer>();
	}
}
