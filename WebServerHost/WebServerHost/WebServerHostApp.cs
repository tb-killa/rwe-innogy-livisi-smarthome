using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using WebServerHost.Handlers;
using WebServerHost.Web;
using WebServerHost.Web.Routing;

namespace WebServerHost;

public class WebServerHostApp : IMessageSender
{
	public const string ChannelIdName = "Core.Local";

	private WebServer webServer;

	private List<RequestHandler> handlers = new List<RequestHandler>();

	private AuthHandler authHandler;

	private ResourceHandler resHandler;

	private WebSocketHandler wsHandler;

	public WebServerConfiguration ServerConfiguration { get; private set; }

	public ServiceProvider ServicesProvider => ServiceProvider.Services;

	public bool IsRunning
	{
		get
		{
			if (webServer == null)
			{
				return false;
			}
			return webServer.Running;
		}
	}

	public WebServerHostApp()
	{
		ServerConfiguration = new WebServerConfiguration();
	}

	public void UseRouting(string endpointBaseRoute, Action<EndpointRoutes> endpoints)
	{
		handlers.Add(new ApiHandler(endpointBaseRoute, endpoints));
	}

	public void UseAuth(string authEndpointBase)
	{
		authHandler = new AuthHandler(authEndpointBase, ServiceProvider.Services.Get<IAuthorization>());
	}

	public void UseStaticResources()
	{
		resHandler = new ResourceHandler(ServerConfiguration);
	}

	public void UseWebSockets(string endpointRoute)
	{
		wsHandler = new WebSocketHandler(endpointRoute);
		handlers.Add(wsHandler);
		ServiceProvider.Services.AddSingleton((IMessageSender)this);
	}

	public void Run()
	{
		if (webServer != null)
		{
			if (webServer.Running)
			{
				Log.Debug(Module.WebServerHost, "Webserver already running");
			}
			else
			{
				webServer.Start();
			}
			return;
		}
		if (authHandler != null)
		{
			handlers.ForEach(delegate(RequestHandler h)
			{
				h.Authorization = authHandler.Authorization;
			});
			handlers.Insert(0, authHandler);
		}
		webServer = new WebServer(ServerConfiguration, resHandler, handlers);
		webServer.Start();
	}

	public void Stop()
	{
		if (webServer != null && webServer.Running)
		{
			webServer.Stop();
		}
	}

	public void SendMessage(string message)
	{
		if (webServer != null && webServer.Running)
		{
			webServer.SendOnWebsockets(message);
		}
		else
		{
			Log.Warning(Module.WebServerHost, "Webserver is stopped, dropping websocket message");
		}
	}
}
