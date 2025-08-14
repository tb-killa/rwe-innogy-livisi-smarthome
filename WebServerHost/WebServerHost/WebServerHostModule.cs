using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.WebServerHost;

namespace WebServerHost;

public class WebServerHostModule : IModule
{
	public void Configure(Container container)
	{
		try
		{
			container.Register("WebServerHost", (Func<Container, IService>)delegate(Container c)
			{
				RWE.SmartHome.SHC.WebServerHost.WebServerHost webServerHost = new RWE.SmartHome.SHC.WebServerHost.WebServerHost(c);
				c.Resolve<ITaskManager>().Register(webServerHost);
				c.Resolve<IChannelMultiplexer>().AddCommunicationChannel(webServerHost);
				return webServerHost;
			}).InitializedBy(delegate(Container c, IService w)
			{
				w.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			container.ResolveNamed<IService>("WebServerHost");
			Log.Information(Module.WebServerHost, "WebServerHost configured and initialized successfully");
		}
		catch (Exception ex)
		{
			Log.Error(Module.WebServerHost, $"Initialization of WebServerHost failed: {ex.Message}");
		}
	}
}
