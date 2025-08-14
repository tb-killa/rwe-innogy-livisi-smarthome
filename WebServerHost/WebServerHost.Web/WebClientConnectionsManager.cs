using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using SmartHome.Common.Generic.LogManager;

namespace WebServerHost.Web;

internal class WebClientConnectionsManager : IDisposable
{
	private static ILogger logger = LogManager.Instance.GetLogger<WebClientConnectionsManager>();

	private List<WebClientConnection> managedWebConnections = new List<WebClientConnection>();

	private Dictionary<WebClientConnection, Action<WebClientConnection, object>> readHandlers = new Dictionary<WebClientConnection, Action<WebClientConnection, object>>();

	private object locker = new object();

	private Thread monitorThread;

	private WebServerConfiguration configuration;

	private volatile bool monitoring;

	public WebClientConnectionsManager(WebServerConfiguration configuration)
	{
		this.configuration = configuration;
		monitorThread = new Thread(ManageConnections);
		monitoring = true;
		monitorThread.Start();
	}

	public void Dispose()
	{
		monitoring = false;
		monitorThread.Join(100);
		monitorThread = null;
		lock (locker)
		{
			foreach (WebClientConnection managedWebConnection in managedWebConnections)
			{
				managedWebConnection.Dispose();
			}
			managedWebConnections.Clear();
			readHandlers.Clear();
		}
	}

	private void ManageConnections()
	{
		while (monitoring)
		{
			List<WebClientConnection> list = new List<WebClientConnection>();
			DateTime now = DateTime.Now;
			lock (locker)
			{
				foreach (WebClientConnection managedWebConnection in managedWebConnections)
				{
					if (managedWebConnection.Connected && now - managedWebConnection.LastActivity < TimeSpan.FromSeconds(configuration.KeepAliveTimeout))
					{
						managedWebConnection.ReceiveAsync(OnClientReception);
					}
					else
					{
						list.Add(managedWebConnection);
					}
				}
			}
			list.ForEach(Remove);
			Thread.Sleep(100);
		}
	}

	private void OnClientReception(WebClientConnection client, object received)
	{
		if (!readHandlers.ContainsKey(client))
		{
			return;
		}
		Action<WebClientConnection, object> action = readHandlers[client];
		if (action == null)
		{
			return;
		}
		try
		{
			action(client, received);
		}
		catch (Exception ex)
		{
			if (!(ex is SocketException))
			{
				logger.Error("Error cought: ", ex);
			}
		}
	}

	public void Remove(WebClientConnection connection)
	{
		Remove(connection, dispose: true);
	}

	public void Remove(WebClientConnection connection, bool dispose)
	{
		logger.Debug(string.Format("Closing client connection {0}{1}", connection.RemoteEndPoint, connection.isWebSocket ? " websocket" : string.Empty));
		lock (locker)
		{
			managedWebConnections.Remove(connection);
			logger.Info($"active client connections {managedWebConnections.Count}");
			if (readHandlers.ContainsKey(connection))
			{
				readHandlers.Remove(connection);
			}
			if (dispose)
			{
				connection.Dispose();
			}
		}
	}

	public void Add(WebClientConnection connection, Action<WebClientConnection, object> handler)
	{
		lock (locker)
		{
			if (!managedWebConnections.Contains(connection))
			{
				managedWebConnections.Add(connection);
				logger.Info($"active connections {managedWebConnections.Count}");
			}
			readHandlers[connection] = handler;
		}
	}

	public void SendOnWebsockets(string message)
	{
		List<WebClientConnection> list;
		lock (locker)
		{
			list = managedWebConnections.Where((WebClientConnection c) => c.isWebSocket).ToList();
		}
		foreach (WebClientConnection item in list)
		{
			try
			{
				item.SendTextOnWebSocket(message);
			}
			catch (Exception exception)
			{
				logger.Warn($"could not send message on websocket (remote {item.RemoteEndPoint}):", exception);
			}
		}
	}
}
