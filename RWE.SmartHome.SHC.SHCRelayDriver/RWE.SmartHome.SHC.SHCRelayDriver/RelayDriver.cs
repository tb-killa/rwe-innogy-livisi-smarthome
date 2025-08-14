using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication.Events;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using RWE.SmartHome.SHC.WebServiceHost;
using SmartHome.Common.Generic.Contracts.BackendShc;

namespace RWE.SmartHome.SHC.SHCRelayDriver;

public class RelayDriver : Task, ICommunicationChannel, IBaseChannel, IService
{
	private const int MaxCommands = 10;

	private const int DelayBeforeSignalingDisconnectMs = 5000;

	private const int DelayBeforeRetryLogonMs = 5000;

	private const int DelayBeforeLogonWhenBusyMs = 20000;

	private static readonly TimeSpan WaitingTimeGetAllMessages = TimeSpan.FromMinutes(3.0);

	private readonly Configuration configuration;

	private readonly ICertificateManager certificateManager;

	private ManualResetEvent shutdownEvent;

	private ManualResetEvent shutdownDoneEvent;

	private readonly RequestDeserializer requestDeserializer;

	private readonly WebSocketManager webSocketManager;

	private readonly RelayDriverReader relayDriverReader;

	private int processedRequestsIndex;

	private readonly List<Guid> processedRequests;

	private readonly List<IRequestProcessor> requestProcessors;

	private readonly IEventManager eventManager;

	private readonly RelaySendThread sendThread;

	private readonly bool useCertificateAuthentication;

	private SubscriptionToken startUpEventSubscriptionToken;

	private readonly ManualResetEvent startupSignal = new ManualResetEvent(initialState: false);

	private readonly ManualResetEvent internetAccessActivated = new ManualResetEvent(initialState: false);

	private SubscriptionToken internetAccessAllowedChangedSubscriptionToken;

	private readonly INetworkingMonitor networkingMonitor;

	private readonly IDisplayManager displayManager;

	private readonly string relayServiceAddress;

	private readonly IRegistrationService registrationService;

	public string ChannelId => "Core.RemoteOcs";

	public ChannelType ChannelType => ChannelType.Remote;

	public bool Connected { get; private set; }

	public RelayDriver(Container container)
	{
		base.Name = "RelayDriver";
		eventManager = container.Resolve<IEventManager>();
		configuration = new Configuration(container.Resolve<IConfigurationManager>());
		certificateManager = container.Resolve<ICertificateManager>();
		networkingMonitor = container.Resolve<INetworkingMonitor>();
		displayManager = container.Resolve<IDisplayManager>();
		registrationService = container.Resolve<IRegistrationService>();
		sendThread = new RelaySendThread(new NotificationSendParameters(this, configuration.NotificationSendDelay.Value));
		relayServiceAddress = configuration.RelayServiceAddress;
		useCertificateAuthentication = configuration.UseCertificateAuthentication.Value;
		processedRequests = new List<Guid>(10);
		for (int i = 0; i < 10; i++)
		{
			processedRequests.Insert(i, Guid.Empty);
		}
		requestProcessors = new List<IRequestProcessor>();
		requestDeserializer = new RequestDeserializer();
		webSocketManager = new WebSocketManager(relayServiceAddress, certificateManager, ConnectionIsSuccsessful);
		relayDriverReader = new RelayDriverReader(webSocketManager);
		eventManager.GetEvent<ShcIsLocalOnlyEvent>().Subscribe(OnShcIsLocalOnlyEvent, null, ThreadOption.PublisherThread, null);
	}

	public void Initialize()
	{
		if (networkingMonitor.InternetAccessAllowed)
		{
			internetAccessActivated.Set();
		}
		if (internetAccessAllowedChangedSubscriptionToken == null)
		{
			internetAccessAllowedChangedSubscriptionToken = eventManager.GetEvent<InternetAccessAllowedChangedEvent>().Subscribe(ChangeInternetAccessAllowed, null, ThreadOption.PublisherThread, null);
		}
		if (startUpEventSubscriptionToken == null)
		{
			ShcStartupCompletedEvent shcStartupCompletedEvent = eventManager.GetEvent<ShcStartupCompletedEvent>();
			startUpEventSubscriptionToken = shcStartupCompletedEvent.Subscribe(ReleaseLock, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound2, ThreadOption.PublisherThread, null);
		}
	}

	public void Uninitialize()
	{
		if (startUpEventSubscriptionToken != null)
		{
			ShcStartupCompletedEvent shcStartupCompletedEvent = eventManager.GetEvent<ShcStartupCompletedEvent>();
			shcStartupCompletedEvent.Unsubscribe(startUpEventSubscriptionToken);
			startUpEventSubscriptionToken = null;
		}
		if (internetAccessAllowedChangedSubscriptionToken != null)
		{
			eventManager.GetEvent<InternetAccessAllowedChangedEvent>().Unsubscribe(internetAccessAllowedChangedSubscriptionToken);
			internetAccessAllowedChangedSubscriptionToken = null;
		}
	}

	public void SubscribeRequestProcessor(IRequestProcessor processor)
	{
		if (processor == null)
		{
			throw new ArgumentNullException("processor");
		}
		requestProcessors.Add(processor);
	}

	public void QueueNotification(BaseNotification notification)
	{
		sendThread.QueueMessage(notification);
	}

	private void ReleaseLock(ShcStartupCompletedEventArgs args)
	{
		startupSignal.Set();
	}

	private void ShutDownPolling()
	{
		shutdownEvent.Set();
		internetAccessActivated.Set();
		shutdownDoneEvent.WaitOne(3000, exitContext: false);
	}

	private void ChangeInternetAccessAllowed(InternetAccessAllowedChangedEventArgs obj)
	{
		if (obj.InternetAccessAllowed)
		{
			internetAccessActivated.Set();
		}
		else
		{
			internetAccessActivated.Reset();
		}
	}

	private void RelayServicePollingThread()
	{
		int num = 0;
		bool flag = false;
		bool first = true;
		DateTime? dateTime = null;
		relayDriverReader.StartReceiving();
		while (!shutdownEvent.WaitOne(num, exitContext: false))
		{
			if (!internetAccessActivated.WaitOne(0, exitContext: false))
			{
				HandleConnectivityLost(ref first);
				internetAccessActivated.WaitOne();
			}
			try
			{
				ProcessMessages();
				if (!internetAccessActivated.WaitOne(0, exitContext: false))
				{
					HandleConnectivityLost(ref first);
					internetAccessActivated.WaitOne();
				}
				num = 0;
				if (flag)
				{
					displayManager.WorkflowFinished(Workflow.RelayServicePolling);
				}
				dateTime = null;
			}
			catch (Exception ex)
			{
				bool flag2 = true;
				if (dateTime.HasValue && DateTime.Now - dateTime.Value < TimeSpan.FromHours(6.0))
				{
					flag2 = false;
				}
				if (flag2)
				{
					Log.Information(Module.RelayDriver, $"Connectivity lost: {ex.Message}");
					dateTime = DateTime.Now;
					if (ICMPTools.IsCableConnected())
					{
						Log.Information(Module.RelayDriver, "Network cable is PLUGGED!");
						Log.Information(Module.RelayDriver, $"Attempting trace route to {relayServiceAddress}...");
						List<string> list = ICMPTools.TraceRoute(relayServiceAddress);
						foreach (string item in list)
						{
							SyncLog.Information(Module.RelayDriver, $"TRACERT: {item}");
						}
					}
					else
					{
						Log.Error(Module.RelayDriver, "Network cable is UNPLUGGED!");
					}
				}
				DateTime now = DateTime.Now;
				WorkflowError? workflowError = ToWorkflowError(ex);
				if (workflowError.HasValue)
				{
					displayManager.WorkflowFailed(Workflow.RelayServicePolling, workflowError.Value);
					flag = true;
				}
				HandleConnectivityLost(ref first);
				num = (int)(5000.0 - (DateTime.Now - now).TotalMilliseconds);
				if (num < 0)
				{
					num = 0;
				}
			}
		}
		relayDriverReader.StopReceivingMessages();
		webSocketManager.CloseSocket();
		shutdownDoneEvent.Set();
		shutdownEvent.Reset();
	}

	private void ConnectionIsSuccsessful()
	{
		if (!Connected)
		{
			FireConnectivityChangedEvent(connected: true);
			Connected = true;
		}
	}

	private void HandleConnectivityLost(ref bool first)
	{
		if (Connected || first)
		{
			FireConnectivityChangedEvent(connected: false);
			Connected = false;
			first = false;
		}
	}

	private void ProcessMessages()
	{
		if (shutdownEvent.WaitOne(0, exitContext: false))
		{
			return;
		}
		List<MessageReceivedInfo> allMessages = relayDriverReader.GetAllMessages(WaitingTimeGetAllMessages);
		if (shutdownEvent.WaitOne(0, exitContext: false) || allMessages == null)
		{
			return;
		}
		if (!Connected)
		{
			FireConnectivityChangedEvent(connected: true);
			Connected = true;
		}
		try
		{
			HandleReceivedMessages(allMessages);
		}
		catch (Exception arg)
		{
			Log.Error(Module.RelayDriver, $"An unexpected exception occurred while processing messages: {arg}");
		}
	}

	private WorkflowError? ToWorkflowError(Exception networkException)
	{
		string host = new Uri(relayServiceAddress).Host;
		NetworkProblem networkProblem = NetworkTools.DiagnoseNetworkProblem(host, networkException);
		WorkflowError? result = null;
		switch (networkProblem)
		{
		case NetworkProblem.NetworkAdapterNotOperational:
			result = WorkflowError.NetworkAdapterNotOperational;
			break;
		case NetworkProblem.NoDhcpIpAddress:
			result = WorkflowError.NoDhcpIpAddress;
			break;
		case NetworkProblem.NoDhcpDefaultGateway:
			result = WorkflowError.NoDhcpDefaultGateway;
			break;
		case NetworkProblem.NameResolutionFailed:
			result = WorkflowError.NameResolutionFailed;
			break;
		case NetworkProblem.NameResolutionFailedNetworkDown:
			result = WorkflowError.NameResolutionFailedNetworkDown;
			break;
		}
		return result;
	}

	private void FireConnectivityChangedEvent(bool connected)
	{
		eventManager.GetEvent<NetworkCableAttachedEvent>().Publish(new NetworkCableAttachedEventArgs
		{
			IsAttached = connected
		});
		Log.Information(Module.RelayDriver, "Connectivity status changed: " + (connected ? "connected to " : "disconnected from ") + "the backend");
		eventManager.GetEvent<ChannelConnectivityChangedEvent>().Publish(new ChannelConnectivityChangedEventArgs
		{
			ChannelId = ChannelId,
			Connected = connected
		});
	}

	private void HandleReceivedMessages(List<MessageReceivedInfo> messages)
	{
		if (messages == null || messages.Count == 0)
		{
			return;
		}
		foreach (MessageReceivedInfo message in messages)
		{
			if (message == null)
			{
				Log.Warning(Module.RelayDriver, "Invalid message information received.");
				continue;
			}
			string errorResponse;
			BaseRequest requestAndCheckVersion = requestDeserializer.GetRequestAndCheckVersion(message.Message, out errorResponse);
			if (requestAndCheckVersion != null && errorResponse != null)
			{
				SendResponse(message.SenderUri, errorResponse, requestAndCheckVersion.RequestId);
			}
			else
			{
				if (requestAndCheckVersion == null)
				{
					continue;
				}
				NotificationAckRequest notificationAckRequest = requestAndCheckVersion as NotificationAckRequest;
				if (notificationAckRequest != null || processedRequests.Contains(requestAndCheckVersion.RequestId))
				{
					continue;
				}
				if (requestAndCheckVersion is AuthenticationRequest authenticationRequest)
				{
					Log.Debug(Module.RelayDriver, $"Received AuthenticationRequest: Channel: [{ChannelId}] Client: [{message.SenderUri}], User: [{authenticationRequest.UserName}], RequestType: [{authenticationRequest.GetType()}], Message: [{message.Message}]");
				}
				else if (requestAndCheckVersion is LoginRequest)
				{
					LoginRequest loginRequest = requestAndCheckVersion as LoginRequest;
					Log.Debug(Module.RelayDriver, $"Received LoginRequest: Channel: [{ChannelId}] Client: [{message.SenderUri}], User: [{loginRequest.UserName}], RequestType: [{loginRequest.GetType()}]");
				}
				else
				{
					Log.Debug(Module.RelayDriver, $"Received Request: Channel: [{ChannelId}] Client: [{message.SenderUri}] Message: {message.Message}");
				}
				foreach (IRequestProcessor requestProcessor in requestProcessors)
				{
					try
					{
						requestProcessor.ProcessRequest(new ChannelContext(message.SenderUri, ChannelId), requestAndCheckVersion, out errorResponse, out Action postSendAction);
						SendResponse(message.SenderUri, errorResponse, requestAndCheckVersion.RequestId);
						postSendAction?.Invoke();
					}
					catch (Exception arg)
					{
						Log.Error(Module.RelayDriver, $"Exception thrown in request handler {arg}");
					}
				}
				processedRequestsIndex = (processedRequestsIndex + 1) % 10;
			}
		}
	}

	private void SendResponse(string destination, string response, Guid requestId)
	{
		Log.Debug(Module.RelayDriver, $"Send Response. Channel: [{ChannelId}] Client: [{destination}] Message: {response}");
		processedRequests[processedRequestsIndex] = requestId;
		SendMessageToClient(response);
	}

	internal void SendMessageToAllClients(string message)
	{
		SendMessageToClient(message);
	}

	internal void SendMessageToClient(string message)
	{
		if (!Connected)
		{
			Log.Debug(Module.RelayDriver, $"Cannot send {message} because SHC is not connected to backend.");
		}
		else
		{
			webSocketManager.Send(message);
		}
	}

	public override void Start()
	{
		if (registrationService.IsShcLocalOnly)
		{
			Log.Information(Module.RelayDriver, "The SHC is not registered in backend, do not start RelayDriver");
			return;
		}
		base.Start();
		Log.Information(Module.RelayDriver, "Relay Driver started.");
	}

	public override void Stop()
	{
		ShutDownPolling();
		Log.Information(Module.RelayDriver, "Relay Driver stopped.");
	}

	protected override void Run()
	{
		shutdownEvent = new ManualResetEvent(initialState: false);
		shutdownDoneEvent = new ManualResetEvent(initialState: false);
		startupSignal.WaitOne();
		if (registrationService.IsShcLocalOnly)
		{
			Log.Information(Module.RelayDriver, "The SHC is not registered in backend, do not run RelayDriver");
			return;
		}
		Log.Information(Module.RelayDriver, "Relay Driver ready to run.");
		RelayServicePollingThread();
	}

	private void OnShcIsLocalOnlyEvent(ShcIsLocalOnlyEventArgs eventArgs)
	{
		Stop();
	}
}
