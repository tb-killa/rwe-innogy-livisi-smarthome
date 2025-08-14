using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Logging;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeleteEntitiesRequestValidation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceInclusion;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Authentication;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.TypeManager;
using RWE.SmartHome.SHC.DataAccessInterfaces.Configuration;
using RWE.SmartHome.SHC.DeviceActivity.DeviceActivityInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Calibrators;
using RWE.SmartHome.SHC.DomainModel.Constants;
using RWE.SmartHome.SHC.ErrorHandling;
using RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;
using RWE.SmartHome.SHC.RuleEngineInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher;

public class ExternalCommandDispatcher : IExternalCommandDispatcher, IRequestProcessor, IService
{
	private const string ModuleName = "ExternalCommandDispatcher";

	private readonly ConfigurationProperties configuration;

	private readonly IUserManager userManagement;

	private readonly IEventManager eventManager;

	private readonly IDeviceFirmwareManager deviceFirmwareManager;

	private readonly IMessagesAndAlertsManager messagesAndAlerts;

	private readonly IChannelMultiplexer channelMultiplexer;

	private readonly MessagesAndAlertsHandler messageAndAlertsHandler;

	private readonly NotificationHandler notificationHandler;

	private readonly IRepository configurationRepository;

	private readonly IApplicationsHost applicationsHost;

	private readonly string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

	private bool softwareUpdateInProgress;

	private readonly object syncRoot = new object();

	private readonly IProtocolMultiplexer protocolMultiplexer;

	private readonly ITokenCache tokenCache;

	private readonly IBusinessLogic businessLogic;

	private readonly IActionExecuter actionExecuter;

	private readonly CommandHandlersManager commandHandlersManager;

	public ExternalCommandDispatcher(Container container)
	{
		userManagement = container.Resolve<IUserManager>();
		eventManager = container.Resolve<IEventManager>();
		deviceFirmwareManager = container.Resolve<IDeviceFirmwareManager>();
		channelMultiplexer = container.Resolve<IChannelMultiplexer>();
		configurationRepository = container.Resolve<IRepository>();
		applicationsHost = container.Resolve<IApplicationsHost>();
		protocolMultiplexer = container.Resolve<IProtocolMultiplexer>();
		businessLogic = container.Resolve<IBusinessLogic>();
		ILogicalDeviceStateRepository logicalDeviceStateRepository = container.Resolve<ILogicalDeviceStateRepository>();
		messagesAndAlerts = container.Resolve<IMessagesAndAlertsManager>();
		actionExecuter = container.Resolve<IActionExecuter>();
		configuration = new ConfigurationProperties(container.Resolve<IConfigurationManager>());
		eventManager.GetEvent<SoftwareUpdateProgressEvent>().Subscribe(SoftwareUpdateProgressChanged, null, ThreadOption.PublisherThread, null);
		notificationHandler = new NotificationHandler(channelMultiplexer);
		container.Register((INotificationHandler)notificationHandler);
		messageAndAlertsHandler = new MessagesAndAlertsHandler(messagesAndAlerts);
		commandHandlersManager = new CommandHandlersManager();
		tokenCache = container.Resolve<ITokenCache>();
		RegisterHandlers(container, logicalDeviceStateRepository);
		channelMultiplexer.SubscribeRequestProcessor(this);
	}

	private void RegisterHandlers(Container container, ILogicalDeviceStateRepository logicalDeviceStateRepository)
	{
		RegisterCommandHandler(new StatusHandler(eventManager));
		RegisterCommandHandler(notificationHandler);
		RegisterCommandHandler(messageAndAlertsHandler);
		RepositoryOperationsWrapper repoOperationsWrapper = new RepositoryOperationsWrapper(container.Resolve<IRepository>(), container.Resolve<IRepositorySync>(), container.Resolve<IDiscoveryController>(), container.Resolve<ISetEntitiesRequestValidator>(), container.Resolve<IDeleteEntitiesRequestValidator>());
		RegisterCommandHandler(new ConfigurationHandler(eventManager, notificationHandler, protocolMultiplexer, container.Resolve<IDiscoveryController>(), repoOperationsWrapper, configuration.DisableSetEntitiesRequestsValidation));
		RegisterSerializedResponseHandler(new GetSerializedEntitiesHandler(container.Resolve<IConfigurationPersistence>(), container.Resolve<IRepository>()));
		RegisterCommandHandler(new ShcInformationHandler(container.Resolve<IShcTypeManager>()));
		RegisterCommandHandler(new LoggingHandler(eventManager));
		RegisterCommandHandler(new ActionRequestHandler(actionExecuter));
		RegisterCommandHandler(new DeviceStatusHandler(eventManager, notificationHandler, configurationRepository, logicalDeviceStateRepository, protocolMultiplexer, this));
		RegisterCommandHandler(new DeviceFirmwareUpdateHandler(eventManager, deviceFirmwareManager, protocolMultiplexer));
		RegisterCommandHandler(new SoftwareUpdateHandler(eventManager));
		RegisterCommandHandler(new CalibrationHandler(eventManager, notificationHandler, configurationRepository, container.Resolve<IRepositorySync>(), container.Resolve<ISetEntitiesRequestValidator>(), container.Resolve<IRollerShutterCalibrator>()));
		RegisterCommandHandler(new DeviceActivityLoggingHandler(container.Resolve<IDeviceActivityLogger>(), notificationHandler, eventManager));
		RegisterCommandHandler(new ApplicationsTokenHandler(tokenCache, applicationsHost, notificationHandler, businessLogic));
	}

	private void RegisterCommandHandler(ICommandHandler commandHandler)
	{
		commandHandlersManager.AddCommandHandler(commandHandler);
	}

	private void RegisterSerializedResponseHandler(ISerializedResponseHandler serializedResponseHandler)
	{
		commandHandlersManager.AddSerializedResponseHandler(serializedResponseHandler);
	}

	public void Initialize()
	{
		commandHandlersManager.Initialize();
		eventManager.GetEvent<DeviceEventDetectedEvent>().Subscribe(OnDeviceEventDetectedEvent, null, ThreadOption.BackgroundThread, null);
	}

	public void Uninitialize()
	{
		commandHandlersManager.Uninitialize();
	}

	public void SendNotification(BaseNotification notification)
	{
		((INotificationHandler)notificationHandler).SendNotification(notification);
	}

	public bool ProcessRequest(ChannelContext context, BaseRequest request, out string response, out Action postSendAction)
	{
		postSendAction = null;
		response = null;
		try
		{
			BaseResponse response2;
			if (context.ChannelId != "Core.Local" && request is GetEntitiesRequest)
			{
				StringCollection stringCollection = commandHandlersManager.HandleSerializedRespondersSerialized(context, request, ref postSendAction);
				response = stringCollection.ToString();
			}
			else if (ProcessRequest(context, request, out response2, out postSendAction))
			{
				response = ResponseHelper.SerializeResponse(response2);
			}
		}
		catch (Exception e)
		{
			response = ResponseHelper.SerializeResponse(HandleRequestException(e, request));
		}
		return true;
	}

	private BaseResponse HandleRequestException(Exception e, BaseRequest request)
	{
		BaseResponse baseResponse = null;
		if (e is ShcException)
		{
			ShcException ex = e as ShcException;
			baseResponse = new ErrorResponse(request.RequestId, ErrorResponseType.GenericError, new StringProperty("Assembly", ex.Assembly), new StringProperty("Module", ex.Module), new NumericProperty
			{
				Name = "Code",
				Value = ex.ErrorCode
			}, new StringProperty("Message", ex.Message));
			Log.Error(RWE.SmartHome.SHC.Core.Module.ExternalCommandDispatcher, ex.Message);
		}
		else
		{
			baseResponse = new ErrorResponse(request.RequestId, ErrorResponseType.GenericError, new StringProperty("Assembly", assemblyName), new StringProperty("Module", "ExternalCommandDispatcher"), new NumericProperty
			{
				Name = "Code",
				Value = 2m
			}, new StringProperty("Message", e.Message));
			Log.Error(RWE.SmartHome.SHC.Core.Module.ExternalCommandDispatcher, $"Error during command processing. Send ErrorResponse with the following data: {e}");
		}
		if (baseResponse == null)
		{
			baseResponse = new ErrorResponse(request.RequestId, ErrorResponseType.GenericError, new StringProperty("Assembly", assemblyName), new StringProperty("Module", "ExternalCommandDispatcher"), new NumericProperty
			{
				Name = "Code",
				Value = 3m
			});
		}
		return baseResponse;
	}

	public bool ProcessRequest(ChannelContext context, BaseRequest request, out BaseResponse response, out Action postSendAction)
	{
		postSendAction = null;
		try
		{
			lock (syncRoot)
			{
				if (softwareUpdateInProgress)
				{
					response = new ErrorResponse(request.RequestId, ErrorResponseType.SoftwareUpdateInProgress);
				}
				else if (request is MultipleRequest multipleRequest)
				{
					response = ProcessMultipleRequest(context, ref postSendAction, multipleRequest);
				}
				else
				{
					response = ProcessRequestSingle(context, request, ref postSendAction);
				}
			}
		}
		catch (Exception e)
		{
			response = HandleRequestException(e, request);
		}
		return true;
	}

	private MultipleResponse ProcessMultipleRequest(ChannelContext context, ref Action postSendAction, MultipleRequest multipleRequest)
	{
		MultipleResponse multipleResponse = new MultipleResponse();
		multipleResponse.CorrespondingRequestId = multipleRequest.RequestId;
		MultipleResponse multipleResponse2 = multipleResponse;
		foreach (BaseRequest request in multipleRequest.RequestList)
		{
			Action postSendAction2 = null;
			BaseResponse item = ProcessRequestSingle(context, request, ref postSendAction2);
			multipleResponse2.ResponseList.Add(item);
			if (postSendAction2 != null)
			{
				if (postSendAction == null)
				{
					postSendAction = postSendAction2;
				}
				else
				{
					postSendAction = (Action)Delegate.Combine(postSendAction, postSendAction2);
				}
			}
		}
		return multipleResponse2;
	}

	private BaseResponse ProcessRequestSingle(ChannelContext context, BaseRequest request, ref Action postSendAction)
	{
		BaseResponse result = null;
		if (request != null)
		{
			result = ((!(request is LoginRequest request2)) ? ((!(request is ProbeShcRequest)) ? ProcessPrivilegedRequest(context, request, ref postSendAction) : ProbeShcRequest(request.RequestId)) : LoginRequest(context, request2));
		}
		return result;
	}

	private LoginResponse LoginRequest(ChannelContext context, LoginRequest request)
	{
		using TokenCacheContext tokenCacheContext = tokenCache.GetAndLockCurrentToken("UserLogin");
		LoginResponse loginResponse = new LoginResponse();
		loginResponse.CorrespondingRequestId = request.RequestId;
		loginResponse.CurrentConfigurationVersion = configurationRepository.RepositoryVersion;
		loginResponse.TokenHash = ((tokenCacheContext.AppsToken == null) ? string.Empty : tokenCacheContext.AppsToken.GetHash());
		loginResponse.ShcDevice = GetShcDevice();
		loginResponse.SessionId = Guid.NewGuid().ToString();
		LoginResponse result = loginResponse;
		List<Guid> deviceIds = protocolMultiplexer.GetHandledDevices();
		messagesAndAlerts.DeleteAllMessages((Message msg) => msg.Properties.Where((StringProperty p) => p.Name == MessageParameterKey.DeviceId.ToString() && !deviceIds.Contains(new Guid(p.Value))).Any());
		return result;
	}

	private ProbeShcResponse ProbeShcRequest(Guid correspondingRequestId)
	{
		Log.Information(RWE.SmartHome.SHC.Core.Module.ExternalCommandDispatcher, "Probe SHC request");
		ProbeShcResponse probeShcResponse = new ProbeShcResponse();
		probeShcResponse.ShcInformation = new ShcInfo
		{
			Serial = SHCSerialNumber.SerialNumber(),
			ShcName = NetworkTools.GetHostName(),
			FriendlyName = configuration.ShcFriendlyName,
			SoftwareVersion = SHCVersion.ApplicationVersion
		};
		probeShcResponse.CorrespondingRequestId = correspondingRequestId;
		return probeShcResponse;
	}

	private BaseResponse ProcessPrivilegedRequest(ChannelContext context, BaseRequest request, ref Action postSendAction)
	{
		BaseResponse baseResponse = null;
		if (request is GetShcStatusRequest)
		{
			baseResponse = GetShcStatus();
		}
		else if (request is GetLoggedInUsersRequest)
		{
			baseResponse = GetLoggedInUsersRequest();
		}
		else if (request is LogoutRequest)
		{
			baseResponse = LogoutRequest();
		}
		else if (request is UploadLogRequest)
		{
			baseResponse = UploadLog();
		}
		else if (request is BackendSynchronizationRequest)
		{
			baseResponse = BackendSynchronization();
		}
		else if (request is FactoryResetRequest)
		{
			baseResponse = PerformFactoryReset(ref postSendAction);
		}
		else if (request is DeleteAllDataRequest)
		{
			baseResponse = DeleteAllLocalData();
		}
		else if (request is SHCRestartRequest restartRequest)
		{
			baseResponse = PerformSHCReset(restartRequest);
		}
		if (baseResponse != null)
		{
			baseResponse.CorrespondingRequestId = request.RequestId;
		}
		else
		{
			baseResponse = commandHandlersManager.HandleResponders(context, request, ref postSendAction);
		}
		return baseResponse;
	}

	private BaseResponse GetShcStatus()
	{
		Log.Information(RWE.SmartHome.SHC.Core.Module.ExternalCommandDispatcher, "Command from Backend: Request Status of SHC.");
		IEnumerable<ICommunicationChannel> channels = channelMultiplexer.GetChannels();
		bool connected = (from list in channels
			where list.ChannelType == ChannelType.Remote
			select list.Connected).FirstOrDefault();
		GetShcStatusResponse getShcStatusResponse = new GetShcStatusResponse();
		getShcStatusResponse.Status = new ShcStatus
		{
			Connected = connected,
			ConfigVersion = configurationRepository.RepositoryVersion.ToString(),
			AppVersion = SHCVersion.ApplicationVersion,
			OsVersion = SHCVersion.OsVersion
		};
		return getShcStatusResponse;
	}

	private BaseResponse GetLoggedInUsersRequest()
	{
		Log.Information(RWE.SmartHome.SHC.Core.Module.ExternalCommandDispatcher, "Command from Backend: Request Users that are currently logged in.");
		return new GetLoggedInUsersResponse();
	}

	private BaseResponse LogoutRequest()
	{
		return new AcknowledgeResponse();
	}

	private BaseResponse UploadLog()
	{
		Log.Information(RWE.SmartHome.SHC.Core.Module.ExternalCommandDispatcher, "Upload log file");
		eventManager.GetEvent<UploadLogEvent>().Publish(new UploadLogEventArgs());
		return new AcknowledgeResponse();
	}

	private BaseResponse BackendSynchronization()
	{
		Log.Information(RWE.SmartHome.SHC.Core.Module.ExternalCommandDispatcher, "Command from Backend: Synchronize with backend");
		eventManager.GetEvent<SyncUsersEvent>().Publish(new SyncUsersEventArgs());
		return new AcknowledgeResponse();
	}

	private BaseResponse PerformFactoryReset(ref Action postSendAction)
	{
		Log.Information(RWE.SmartHome.SHC.Core.Module.ExternalCommandDispatcher, "Command from Backend: Perform factory reset");
		postSendAction = PerformFactoryResetAfterSend;
		return new AcknowledgeResponse();
	}

	private void PerformFactoryResetAfterSend()
	{
		((INotificationHandler)notificationHandler).SendNotification((BaseNotification)new LogoutNotification
		{
			Reason = LogoutNotificationReason.FactoryReset,
			Namespace = "core.RWE"
		});
		eventManager.GetEvent<PerformFactoryResetEvent>().Publish(new PerformFactoryResetEventArgs());
	}

	private BaseResponse PerformSHCReset(SHCRestartRequest restartRequest)
	{
		TimeSpan rebootDelay = TimeSpan.FromSeconds(10.0);
		Log.Information(RWE.SmartHome.SHC.Core.Module.ExternalCommandDispatcher, "Command from Backend or UserPortal: Reboot SHC");
		eventManager.GetEvent<ShcRebootScheduledEvent>().Publish(new ShcRebootScheduledEventArgs(rebootDelay, restartRequest.Requester, restartRequest.Reason));
		try
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				Log.Information(RWE.SmartHome.SHC.Core.Module.ExternalCommandDispatcher, "Restarting SHC in 10 seconds...");
				Thread.Sleep((int)rebootDelay.TotalMilliseconds);
				((INotificationHandler)notificationHandler).SendNotification((BaseNotification)new LogoutNotification
				{
					Reason = LogoutNotificationReason.RebootShc,
					Namespace = "core.RWE"
				});
				eventManager.GetEvent<PerformResetEvent>().Publish(new PerformResetEventArgs());
			});
		}
		catch (Exception)
		{
			Log.Error(RWE.SmartHome.SHC.Core.Module.ExternalCommandDispatcher, "Out-of-memory error: could not queue the SHC restart event!");
		}
		return new AcknowledgeResponse();
	}

	private BaseResponse DeleteAllLocalData()
	{
		eventManager.GetEvent<DeleteAllLocalDataEvent>().Publish(new DeleteAllLocalDataEventArgs());
		return new AcknowledgeResponse();
	}

	private void SoftwareUpdateProgressChanged(SoftwareUpdateProgressEventArgs args)
	{
		lock (syncRoot)
		{
			softwareUpdateInProgress = args.State == SoftwareUpdateState.Started;
			if (softwareUpdateInProgress)
			{
				((INotificationHandler)notificationHandler).SendNotification((BaseNotification)new LogoutNotification
				{
					Reason = LogoutNotificationReason.PerfomSoftwareUpdate,
					Namespace = "core.RWE"
				});
			}
		}
	}

	private BaseDevice GetShcDevice()
	{
		BaseDevice baseDevice = configurationRepository.GetBaseDevices().FirstOrDefault((BaseDevice bd) => bd.Manufacturer == "RWE" && bd.DeviceType == BuiltinPhysicalDeviceType.SHC.ToString());
		if (baseDevice != null)
		{
			return baseDevice;
		}
		Log.Warning(RWE.SmartHome.SHC.Core.Module.ExternalCommandDispatcher, "SHC base device not found on login request - returning null...");
		return null;
	}

	private void OnDeviceEventDetectedEvent(DeviceEventDetectedEventArgs args)
	{
		GenericNotification genericNotification = new GenericNotification();
		genericNotification.EventType = args.EventType;
		genericNotification.Namespace = "core.RWE";
		genericNotification.EntityId = args.LogicalDeviceId.ToString("N");
		genericNotification.EntityType = EntityType.LogicalDevice;
		genericNotification.Timestamp = DateTime.Now;
		genericNotification.Data = new PropertyBag();
		GenericNotification genericNotification2 = genericNotification;
		genericNotification2.Data.Properties.AddRange(args.EventDetails.Where((Property a) => a.Name != EventConstants.EventTypePropertyName));
		SendNotification(genericNotification2);
	}
}
