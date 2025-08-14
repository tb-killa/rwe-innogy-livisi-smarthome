using System;
using System.Net;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.Core.NetworkMonitoring;

internal sealed class NetworkingMonitor : INetworkingMonitor, IService
{
	private const int defaultBeHealthCheckMinutesPeriod = 10;

	private Guid HealthCheckTaskId;

	private TimeSpan HealthCheckPeriod;

	private readonly IEventManager eventManager;

	private readonly IScheduler scheduler;

	private readonly IConfigurationManager configManager;

	private SubscriptionToken shcStartedSubscriptionToken;

	public bool InternetAccessAllowed { get; private set; }

	public NetworkingMonitor(IEventManager eventManager, IScheduler scheduler, IConfigurationManager configManager)
	{
		this.eventManager = eventManager;
		this.scheduler = scheduler;
		this.configManager = configManager;
		HealthCheckTaskId = default(Guid);
		int? num = configManager["RWE.SmartHome.SHC.Core"].GetInt("BeHealthCheckMinutesPeriod");
		if (num.HasValue)
		{
			HealthCheckPeriod = new TimeSpan(0, num.Value, 0);
		}
		else
		{
			HealthCheckPeriod = new TimeSpan(0, 10, 0);
		}
	}

	private void FireInternetAccessAllowedChangedEvent(bool allowed)
	{
		Log.Debug(Module.Core, $"NetworkMonitor InternetAccessAllowed changed to = {allowed}");
		eventManager.GetEvent<InternetAccessAllowedChangedEvent>().Publish(new InternetAccessAllowedChangedEventArgs
		{
			InternetAccessAllowed = allowed
		});
	}

	public void Initialize()
	{
		InternetAccessAllowed = true;
		if (shcStartedSubscriptionToken == null)
		{
			shcStartedSubscriptionToken = eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(ShcStarted, null, ThreadOption.PublisherThread, null);
		}
		string value = configManager["RWE.SmartHome.SHC.Core"].GetString("BEHealthcheckUrl");
		if (!string.IsNullOrEmpty(value))
		{
			HealthCheckTask();
			scheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(HealthCheckTaskId, HealthCheckTask, HealthCheckPeriod, runOnce: false));
		}
		else
		{
			Log.Warning(Module.Core, "BE health check url not configured");
		}
	}

	private void ShcStarted(ShcStartupCompletedEventArgs obj)
	{
		if (obj.Progress == StartupProgress.DatabaseAvailable)
		{
			FireInternetAccessAllowedChangedEvent(InternetAccessAllowed);
		}
	}

	private void HealthCheckTask()
	{
		Log.Debug(Module.Core, "NetworkMonitor: BE Health check");
		bool flag = false;
		try
		{
			string requestUriString = configManager["RWE.SmartHome.SHC.Core"].GetString("BEHealthcheckUrl");
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			httpWebResponse.Close();
		}
		catch (WebException ex)
		{
			HttpWebResponse httpWebResponse2 = (HttpWebResponse)ex.Response;
			if (httpWebResponse2 != null)
			{
				flag = httpWebResponse2.StatusCode == HttpStatusCode.Gone;
			}
		}
		catch (Exception arg)
		{
			Log.Debug(Module.Core, $"NetworkMonitor: exception cought on BE Health check {arg}");
		}
		finally
		{
			if (flag == InternetAccessAllowed)
			{
				InternetAccessAllowed = !flag;
				FireInternetAccessAllowedChangedEvent(InternetAccessAllowed);
			}
		}
	}

	public void Uninitialize()
	{
		if (shcStartedSubscriptionToken != null)
		{
			eventManager.GetEvent<ShcStartupCompletedEvent>().Unsubscribe(shcStartedSubscriptionToken);
		}
		scheduler.RemoveSchedulerTask(HealthCheckTaskId);
	}
}
