using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalCommunication;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LogicalDeviceStateRepository.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DomainModel;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices.NotificationSender;

public class NotificationSenderActionHandler : IVirtualCoreActionHandler
{
	private const string SmsValidationContext = "NotificationSenderContext/SmsValidation";

	private readonly IRepository configRepo;

	private readonly IEventManager eventManager;

	private readonly INotificationServiceClient notificationServiceClient;

	private readonly IMessagesAndAlertsManager messagesAndAlerts;

	private readonly SentNotificationsCache sentNotificationsCache = new SentNotificationsCache();

	private readonly IScheduler scheduler;

	private readonly ITokenCache tokenCache;

	private readonly ISmsClient smsClient;

	private readonly IEmailSender emailSender;

	private readonly IRegistrationService registrationService;

	private readonly string certThumbprint;

	private readonly string shcSerialNumber;

	private readonly object notificationSenderLock = new object();

	private bool smsQuotaReached;

	private int currentSmsQuota;

	public NotificationSenderActionHandler(IRepository configRepo, IEventManager eventManager, ICertificateManager certManager, INotificationServiceClient notificationServiceClient, IMessagesAndAlertsManager messagesAndAlerts, IScheduler scheduler, ITokenCache tokenCache, ISmsClient smsClient, IEmailSender emailSender, IRegistrationService registrationService)
	{
		this.configRepo = configRepo;
		this.eventManager = eventManager;
		this.notificationServiceClient = notificationServiceClient;
		certThumbprint = TryGetCertificateThumbprint(certManager, "Can not get certificate thumbprint. Can not use NotificationSender.");
		this.messagesAndAlerts = messagesAndAlerts;
		this.scheduler = scheduler;
		this.tokenCache = tokenCache;
		this.smsClient = smsClient;
		this.emailSender = emailSender;
		this.registrationService = registrationService;
		shcSerialNumber = SHCSerialNumber.SerialNumber();
		smsQuotaReached = false;
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcInitializationFinished, (ShcStartupCompletedEventArgs a) => a.Progress == StartupProgress.CompletedRound2, ThreadOption.BackgroundThread, null);
		eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(OnConfigurationChanged, (ConfigurationProcessedEventArgs arg) => arg.ConfigurationPhase == ConfigurationProcessedPhase.UINotified, ThreadOption.BackgroundThread, null);
		eventManager.GetEvent<TokenCacheUpdateEvent>().Subscribe(OnTokenRefresh, null, ThreadOption.PublisherThread, null);
	}

	public void RequestState(Guid deviceId)
	{
	}

	public ExecutionResult ExecuteAction(ActionContext context, ActionDescription action)
	{
		InitializeOrUpdateSendLimitCounters(action);
		ExecutionResult executionResult = ValidateAction(action, context);
		if (executionResult.Status == ExecutionStatus.Success)
		{
			int retryCounter = 5;
			NotificationResponse notificationResponse = TryCallBackendToSendNotification(notificationServiceClient, context, action, retryCounter);
			executionResult = ((notificationResponse != null) ? ((notificationResponse.NotificationSendResult == NotificationSendResult.Success) ? new ExecutionResult(ExecutionStatus.Success, new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>()) : CreateFailureResponse(notificationResponse.NotificationSendResult.ToString())) : new ExecutionResult(ExecutionStatus.Success, new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>()));
		}
		return executionResult;
	}

	private void ProcessResultFromBackend(NotificationResponse resultFromBackend, CustomNotification customNotification, Guid cacheId)
	{
		if (resultFromBackend.NotificationSendResult == NotificationSendResult.Success)
		{
			CheckStateForRemainingQuotaMessage();
			if (customNotification.ChannelSpecified && customNotification.Channel == NotificationChannelType.SMS && resultFromBackend.RemainingQuota <= 0)
			{
				smsQuotaReached = true;
			}
			sentNotificationsCache.IncrementCurrentMessagesCount(cacheId, GetMessagesToBeSentCount(customNotification));
		}
	}

	private NotificationResponse TryCallBackendToSendNotification(INotificationServiceClient notificationClient, ActionContext context, ActionDescription action, int retryCounter)
	{
		NotificationResponse notificationResponse = null;
		CustomNotification customNotification = GetCustomNotification(action, context);
		if (retryCounter > 0)
		{
			try
			{
				notificationResponse = CallSynchronizedSendNotification(notificationClient, customNotification);
				ProcessResultFromBackend(notificationResponse, customNotification, action.Id);
			}
			catch (Exception ex)
			{
				Log.Exception(Module.BusinessLogic, ex, "Exception while sending message, will retry later...");
				retryCounter--;
				RescheduleSendNotificationTask(notificationClient, context, action, retryCounter);
			}
			if (notificationResponse != null)
			{
				if (notificationResponse.NotificationSendResult != NotificationSendResult.Success)
				{
					Log.Error(Module.BusinessLogic, $"Notification service error: {notificationResponse.NotificationSendResult.ToString()}");
				}
				Log.Debug(Module.BusinessLogic, $"Notification service: [Quotas: {GetQuotasLogString(notificationResponse.Quotas)} / remaining quota {notificationResponse.RemainingQuota}]");
			}
			return notificationResponse;
		}
		SendMessageCouldNotBeSentIn10Min(context, action.Target.EntityIdAsGuid());
		return null;
	}

	private void SendMessageCouldNotBeSentIn10Min(ActionContext context, Guid targetEntityId)
	{
		string value = string.Empty;
		string value2 = string.Empty;
		if (context != null)
		{
			_ = context.AssociatedId;
			value = context.InteractionName;
			value2 = context.AssociatedId.ToString("N");
		}
		List<StringProperty> list = new List<StringProperty>();
		list.Add(new StringProperty("InteractionName", value));
		list.Add(new StringProperty("RuleId", value2));
		List<StringProperty> properties = list;
		Message message = new Message(MessageClass.Message, "MessageCouldNotBeSentIn10Minutes", properties);
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		Message message2 = message;
		LogicalDevice logicalDevice = configRepo.GetLogicalDevice(targetEntityId);
		if (logicalDevice != null)
		{
			message2.BaseDeviceIds = new List<Guid> { logicalDevice.BaseDeviceId };
			message2.LogicalDeviceIds = new List<Guid> { logicalDevice.Id };
		}
		messagesAndAlerts.AddMessage(message2, MessageAddMode.NewMessage);
	}

	private string GetQuotasLogString(List<KeyValuePair<NotificationChannelType, int>> quotas)
	{
		return string.Join(",", quotas.Select((KeyValuePair<NotificationChannelType, int> q) => $"{q.Key.ToString()}={q.Value}").ToArray());
	}

	private void RescheduleSendNotificationTask(INotificationServiceClient notificationClient, ActionContext context, ActionDescription action, int retryCounter)
	{
		Guid id = Guid.NewGuid();
		TimeSpan timeOfDay = ShcDateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(2.0));
		Action taskAction = delegate
		{
			TryCallBackendToSendNotification(notificationClient, context, action, retryCounter);
		};
		scheduler.AddSchedulerTask(new FixedTimeSchedulerTask(id, taskAction, timeOfDay, runOnce: true));
	}

	private NotificationResponse CallSynchronizedSendNotification(INotificationServiceClient notificationServiceClient, CustomNotification customNotifications)
	{
		if (customNotifications.Channel == NotificationChannelType.Email)
		{
			emailSender.SendCustomEmail(customNotifications.Body);
		}
		if (registrationService.IsShcLocalOnly)
		{
			return new NotificationResponse(NotificationSendResult.Success, 1, new List<KeyValuePair<NotificationChannelType, int>>());
		}
		lock (notificationSenderLock)
		{
			return notificationServiceClient.SendNotifications(certThumbprint, customNotifications);
		}
	}

	internal void OnConfigurationChanged(ConfigurationProcessedEventArgs args)
	{
		CleanupRemovedActionsRelatedData();
		UpdateActionsRelatedData(GetActionsOnNotificationSender(args));
	}

	private void OnShcInitializationFinished(ShcStartupCompletedEventArgs args)
	{
		currentSmsQuota = GetSmsQuota();
		messagesAndAlerts.DeleteAllMessages((Message m) => m.Type == "SendMessageLimitExceeded");
		messagesAndAlerts.DeleteAllMessages((Message m) => m.Type == "MessageCouldNotBeSentIn10Minutes");
		CheckStateForRemainingQuotaMessage();
	}

	private void OnTokenRefresh(TokenCacheUpdateEventArgs args)
	{
		int smsQuota = GetSmsQuota();
		if (smsQuota > currentSmsQuota)
		{
			smsQuotaReached = false;
			currentSmsQuota = smsQuota;
		}
		CheckStateForRemainingQuotaMessage();
	}

	private int GetSmsQuota()
	{
		using TokenCacheContext tokenCacheContext = tokenCache.GetAndLockCurrentToken("NotificationSenderContext/SmsValidation");
		if (tokenCacheContext == null || tokenCacheContext.AppsToken == null || tokenCacheContext.AppsToken.Entries == null)
		{
			return 0;
		}
		ApplicationTokenEntry applicationTokenEntry = tokenCacheContext.AppsToken.Entries.FirstOrDefault((ApplicationTokenEntry tk) => tk.AppId != null && tk.AppId.Equals("sh://SMSContingent.RWE"));
		if (applicationTokenEntry == null || applicationTokenEntry.Parameters == null)
		{
			return 0;
		}
		ApplicationParameter applicationParameter = applicationTokenEntry.Parameters.FirstOrDefault((ApplicationParameter p) => p.Key == "APP_QUOTA");
		if (applicationParameter == null)
		{
			return 0;
		}
		try
		{
			return int.Parse(applicationParameter.Value);
		}
		catch (Exception)
		{
			return 0;
		}
	}

	private void CleanupRemovedActionsRelatedData()
	{
		IEnumerable<Guid> unmodifiedSendActionIds = from a in configRepo.GetInteractions().SelectMany((Interaction i) => i.Rules).SelectMany((Rule i) => i.Actions)
			where NotificationSenderConstants.IsSendAction(a.ActionType) && sentNotificationsCache.IsEntryInCache(a.Id)
			select a.Id;
		Predicate<Guid> predicate = (Guid x) => !unmodifiedSendActionIds.Contains(x);
		sentNotificationsCache.DeleteCachedData(predicate);
		DeleteRelatedMessages(predicate);
	}

	private void DeleteRelatedMessages(Predicate<Guid> hasIdBeenModified)
	{
		messagesAndAlerts.DeleteAllMessages((Message m) => m.Class == MessageClass.Alert && IsMessageRelatedToNotificationSender(m) && hasIdBeenModified(m.Id));
	}

	private bool IsMessageRelatedToNotificationSender(Message m)
	{
		Guid? notificationSenderBdId = GetNotificationSenderBdId();
		if (m.BaseDeviceIds == null || !m.BaseDeviceIds.Any(delegate(Guid id)
		{
			Guid? guid = notificationSenderBdId;
			return id == guid;
		}))
		{
			return false;
		}
		return true;
	}

	private Guid? GetNotificationSenderBdId()
	{
		return configRepo.GetBaseDevices().FirstOrDefault((BaseDevice d) => d.DeviceType == BuiltinPhysicalDeviceType.NotificationSender.ToString())?.Id;
	}

	private void UpdateActionsRelatedData(IEnumerable<ActionDescription> actions)
	{
		foreach (ActionDescription action in actions)
		{
			Message actionMessage = messagesAndAlerts.GetMessageById(action.Id);
			if (actionMessage == null || !(actionMessage.Type == "SendMessageLimitExceeded"))
			{
				continue;
			}
			int newLimitCount = int.Parse(GetStringValue(action.Data, "SendLimitCount"));
			string stringValue = GetStringValue(action.Data, "SendLimitInterval");
			string valueAsString = actionMessage.Properties.First((StringProperty m) => m.Name == "SendLimitInterval").GetValueAsString();
			int oldLimitCount = int.Parse(actionMessage.Properties.First((StringProperty m) => m.Name == "SendLimit").GetValueAsString());
			if (IsNewLimitMoreRelaxed(action.Id, oldLimitCount, NotificationSenderConstants.GetSendInterval(valueAsString), newLimitCount, NotificationSenderConstants.GetSendInterval(stringValue)))
			{
				messagesAndAlerts.DeleteAllMessages((Message m) => m.Id == action.Id && actionMessage.Type == "SendMessageLimitExceeded");
			}
			else
			{
				UpdateLimitExceededMessage(actionMessage, stringValue, newLimitCount);
			}
		}
	}

	private void UpdateLimitExceededMessage(Message message, string newLimitInterval, int newLimitCount)
	{
		List<StringProperty> properties = message.Properties.Select((StringProperty p) => p.Name switch
		{
			"SendLimitInterval" => new StringProperty("SendLimitInterval", newLimitInterval), 
			"SendLimit" => new StringProperty("SendLimit", newLimitCount.ToString()), 
			_ => p, 
		}).ToList();
		Message message2 = new Message(message.Class, message.Type, properties);
		message2.Id = message.Id;
		messagesAndAlerts.ReplaceMessage(message2, (Message m) => m.Id == message.Id);
	}

	private bool IsNewLimitMoreRelaxed(Guid actionId, int oldLimitCount, SendInterval? oldLimitInterval, int newLimitCount, SendInterval? newLimitInterval)
	{
		if (newLimitCount > oldLimitCount)
		{
			return true;
		}
		if (oldLimitInterval.HasValue && newLimitInterval.HasValue && newLimitInterval < oldLimitInterval)
		{
			int num = newLimitCount;
			if (oldLimitInterval == SendInterval.Month)
			{
				num = ((newLimitInterval == SendInterval.Week) ? (newLimitCount * 4) : (newLimitCount * 30));
			}
			SendInterval? sendInterval = oldLimitInterval;
			if (sendInterval == SendInterval.Week && sendInterval.HasValue && newLimitInterval == SendInterval.Day)
			{
				num = newLimitCount * 7;
			}
			if (num > oldLimitCount)
			{
				sentNotificationsCache.DeleteCachedData((Guid x) => x == actionId);
				return true;
			}
		}
		return false;
	}

	private int GetMessagesToBeSentCount(CustomNotification notification)
	{
		int num = 0;
		if (notification.CustomRecipients != null)
		{
			num += notification.CustomRecipients.Count((string x) => !string.IsNullOrEmpty(x));
		}
		if (notification.UserNames != null)
		{
			num += notification.UserNames.Count((string x) => !string.IsNullOrEmpty(x));
		}
		return num;
	}

	private void InitializeOrUpdateSendLimitCounters(ActionDescription action)
	{
		string limitInterval = GetStringValue(action.Data, "SendLimitInterval") ?? SendInterval.Month.ToString();
		sentNotificationsCache.InitializeOrUpdateSentLimitCountersForAction(action.Id, limitInterval);
	}

	private ExecutionResult ValidateAction(ActionDescription action, ActionContext context)
	{
		if (action.ActionType != "SendNotification")
		{
			return new ExecutionResult(ExecutionStatus.NotApplicable, new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>());
		}
		bool flag = IsSmsTarget(action.Target);
		if (flag && !SmsContingentProvisioned())
		{
			return new ExecutionResult(ExecutionStatus.NotApplicable, new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>
			{
				new StringProperty("Reason", "SMS_CONTNGENT_NOT_PROVISIONED")
			});
		}
		if (flag && smsQuotaReached)
		{
			return CreateFailureResponse("SMS quota reached");
		}
		LogicalDevice logicalDevice = configRepo.GetLogicalDevices().FirstOrDefault((LogicalDevice d) => d.Id == action.Target.EntityIdAsGuid());
		if (logicalDevice == null || (!(logicalDevice.DeviceType == "PushNotificationActuator") && !(logicalDevice.DeviceType == "SmsActuator") && !(logicalDevice.DeviceType == "EmailActuator")))
		{
			return CreateFailureResponse("Invalid Target Capability");
		}
		if (!action.Data.Any((Parameter x) => x.Name == "NotificationBody"))
		{
			return CreateFailureResponse("Can not send notification without a body");
		}
		if (logicalDevice.DeviceType == "PushNotificationActuator" && !action.Data.Any((Parameter x) => x.Name == "RecipientAccountNames"))
		{
			return CreateFailureResponse("Can not send push notifications without user names");
		}
		string text = GetStringValue(action.Data, "SendLimitCount");
		if (string.IsNullOrEmpty(text))
		{
			text = 100.ToString();
			action.Data.Add(GetMissingParameter("SendLimitCount"));
		}
		string stringValue = GetStringValue(action.Data, "SendLimitInterval");
		if (string.IsNullOrEmpty(stringValue))
		{
			action.Data.Add(GetMissingParameter("SendLimitInterval"));
		}
		CustomNotification customNotification = GetCustomNotification(action, context);
		Message messageLimitExceededForAction = messagesAndAlerts.GetAllMessages((Message m) => m.Type == "SendMessageLimitExceeded" && m.Id == action.Id).FirstOrDefault();
		if (sentNotificationsCache.GetCurrentMessagesCount(action.Id) + GetMessagesToBeSentCount(customNotification) > int.Parse(text))
		{
			Message orCreateLimitExceededMessage = GetOrCreateLimitExceededMessage(action, text, messageLimitExceededForAction, context);
			List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> list = new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>();
			list.Add(new StringProperty("Reason", "SendMessageLimitExceeded"));
			List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property> list2 = list;
			list2.AddRange(((IEnumerable<StringProperty>)orCreateLimitExceededMessage.Properties).Select((Func<StringProperty, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>)((StringProperty x) => x)));
			return new ExecutionResult(ExecutionStatus.Failure, list2);
		}
		if (messageLimitExceededForAction != null)
		{
			messagesAndAlerts.DeleteAllMessages((Message m) => m.Id == messageLimitExceededForAction.Id);
		}
		return new ExecutionResult(ExecutionStatus.Success, new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>());
	}

	private bool SmsContingentProvisioned()
	{
		using (TokenCacheContext tokenCacheContext = tokenCache.GetAndLockCurrentToken("NotificationSenderContext/SmsValidation"))
		{
			if (tokenCacheContext != null && tokenCacheContext.AppsToken != null && tokenCacheContext.AppsToken.Entries != null && tokenCacheContext.AppsToken.Entries.Any((ApplicationTokenEntry tk) => tk.AppId != null && tk.AppId.Equals("sh://SMSContingent.RWE")))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsSmsTarget(LinkBinding target)
	{
		if (target != null && target.LinkType == EntityType.LogicalDevice)
		{
			Guid id = target.EntityIdAsGuid();
			LogicalDevice logicalDevice = configRepo.GetLogicalDevice(id);
			if (logicalDevice != null)
			{
				return logicalDevice.DeviceType == "SmsActuator";
			}
			return false;
		}
		return false;
	}

	private Parameter GetMissingParameter(string paramName)
	{
		Parameter parameter = new Parameter();
		parameter.Name = paramName;
		switch (paramName)
		{
		case "SendLimitCount":
			parameter.Value = new ConstantNumericBinding
			{
				Value = 100m
			};
			break;
		case "SendLimitInterval":
			parameter.Value = new ConstantStringBinding
			{
				Value = SendInterval.Month.ToString()
			};
			break;
		}
		return parameter;
	}

	private Message GetOrCreateLimitExceededMessage(ActionDescription action, string limitCount, Message messageLimitExceededForAction, ActionContext context)
	{
		Message message;
		if (messageLimitExceededForAction != null)
		{
			message = messageLimitExceededForAction;
		}
		else
		{
			List<Guid> list = new List<Guid>();
			List<Guid> list2 = new List<Guid>();
			if (action.Target.LinkType == EntityType.LogicalDevice)
			{
				LogicalDevice logicalDevice = configRepo.GetLogicalDevice(action.Target.EntityIdAsGuid());
				if (logicalDevice != null)
				{
					list2.Add(logicalDevice.Id);
					list.Add(logicalDevice.BaseDeviceId);
				}
			}
			message = new Message(MessageClass.Message, "SendMessageLimitExceeded", new List<StringProperty>
			{
				new StringProperty("SendLimit", limitCount.ToString()),
				new StringProperty("SendLimitInterval", GetStringValue(action.Data, "SendLimitInterval"))
			});
			message.AppId = CoreConstants.CoreAppId;
			message.AddinVersion = "1.0";
			message.BaseDeviceIds = list;
			message.LogicalDeviceIds = list2;
			message.Id = action.Id;
			if (context != null && context.Type == ContextType.RuleExecution)
			{
				message.Properties.Add(new StringProperty("InteractionId", context.AssociatedId.ToString("N")));
				message.Properties.Add(new StringProperty("InteractionName", context.InteractionName));
			}
			messagesAndAlerts.AddMessage(message, MessageAddMode.NewMessage);
		}
		return message;
	}

	private ExecutionResult CreateFailureResponse(string message)
	{
		return new ExecutionResult(ExecutionStatus.Failure, new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>
		{
			new StringProperty("Reason", message)
		});
	}

	private CustomNotification GetCustomNotification(ActionDescription action, ActionContext context)
	{
		LogicalDevice logicalDevice = configRepo.GetLogicalDevices().First((LogicalDevice d) => d.Id == action.Target.EntityIdAsGuid());
		string stringValue = GetStringValue(action.Data, "RecipientAccountNames");
		string text = ((logicalDevice.DeviceType == "PushNotificationActuator") ? null : GetStringValue(action.Data, "CustomRecipients"));
		CustomNotification customNotification = new CustomNotification();
		customNotification.Title = GetNotificationTitle(action.Data, logicalDevice.DeviceType);
		customNotification.Body = GetNotificationBody(action.Data, context);
		customNotification.Channel = GetChannel(logicalDevice.DeviceType).Value;
		customNotification.ChannelSpecified = true;
		customNotification.UserNames = (string.IsNullOrEmpty(stringValue) ? null : stringValue.Split(','));
		customNotification.CustomRecipients = (string.IsNullOrEmpty(text) ? null : text.Split(','));
		return customNotification;
	}

	private string GetNotificationBody(List<Parameter> properties, ActionContext context)
	{
		string text = GetStringValue(properties, "NotificationBody");
		if (GetBooleanValueWithFallback(properties, "AppendTriggerInfo", fallback: true) && context != null && context.ReferenceTrigger != null)
		{
			Location location = TryGetLocation(context.ReferenceTrigger);
			string text2 = ((location != null) ? (" (" + location.Name + ")") : string.Empty);
			text = text + " >> " + GetDeviceName(context.ReferenceTrigger) + text2;
		}
		return text;
	}

	private string GetDeviceName(LinkBinding link)
	{
		BaseDevice baseDevice = GetBaseDevice(link);
		if (baseDevice != null)
		{
			return baseDevice.Name;
		}
		return string.Empty;
	}

	private Location TryGetLocation(LinkBinding link)
	{
		switch (link.LinkType)
		{
		case EntityType.LogicalDevice:
		{
			LogicalDevice logicalDevice = configRepo.GetLogicalDevice(link.EntityIdAsGuid());
			if (logicalDevice != null)
			{
				return logicalDevice.Location ?? logicalDevice.BaseDevice.Location;
			}
			break;
		}
		case EntityType.BaseDevice:
		{
			BaseDevice baseDevice = configRepo.GetBaseDevice(link.EntityIdAsGuid());
			if (baseDevice != null)
			{
				return baseDevice.Location;
			}
			break;
		}
		}
		return null;
	}

	private BaseDevice GetBaseDevice(LinkBinding link)
	{
		if (link.LinkType == EntityType.BaseDevice)
		{
			return configRepo.GetBaseDevice(link.EntityIdAsGuid());
		}
		if (link.LinkType == EntityType.LogicalDevice)
		{
			return configRepo.GetLogicalDevice(link.EntityIdAsGuid())?.BaseDevice;
		}
		return null;
	}

	private string GetNotificationTitle(List<Parameter> list, string deviceType)
	{
		return deviceType switch
		{
			"PushNotificationActuator" => GetStringValue(list, "Title"), 
			"EmailActuator" => GetStringValue(list, "Subject"), 
			_ => string.Empty, 
		};
	}

	private NotificationChannelType? GetChannel(string deviceType)
	{
		return deviceType switch
		{
			"PushNotificationActuator" => NotificationChannelType.Push, 
			"SmsActuator" => NotificationChannelType.SMS, 
			"EmailActuator" => NotificationChannelType.Email, 
			_ => null, 
		};
	}

	private string GetStringValue(List<Parameter> properties, string propertyName)
	{
		string text = properties.GetStringValue(propertyName);
		if (string.IsNullOrEmpty(text))
		{
			decimal? numericValue = properties.GetNumericValue(propertyName);
			if (numericValue.HasValue)
			{
				text = numericValue.Value.ToString();
			}
		}
		return text;
	}

	private bool GetBooleanValueWithFallback(List<Parameter> properties, string propertyName, bool fallback)
	{
		bool? flag = properties.GetBooleanValue(propertyName);
		if (!flag.HasValue)
		{
			flag = fallback;
		}
		return flag.Value;
	}

	private string TryGetCertificateThumbprint(ICertificateManager certManager, string logMessage)
	{
		string personalCertificateThumbprint = certManager.PersonalCertificateThumbprint;
		if (string.IsNullOrEmpty(personalCertificateThumbprint))
		{
			Log.Information(Module.BusinessLogic, logMessage);
		}
		return personalCertificateThumbprint;
	}

	private List<ActionDescription> GetActionsOnNotificationSender(ConfigurationProcessedEventArgs args)
	{
		IEnumerable<ActionDescription> source = args.ModifiedInteractions.SelectMany((Interaction i) => i.Rules).SelectMany((Rule r) => r.Actions);
		source = source.Where((ActionDescription a) => a.ActionType.Contains("SendNotification"));
		return source.ToList();
	}

	private void CheckStateForRemainingQuotaMessage()
	{
		try
		{
			int remainingMessages = 0;
			if (SmsContingentProvisioned() && smsClient.GetSmsRemainingQuota(certThumbprint, shcSerialNumber, out var remainingQuota) == MessageAppResultCode.Success && remainingQuota.HasValue)
			{
				remainingMessages = remainingQuota.Value;
			}
			SendStateForQuotaMessages(remainingMessages);
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, "Failed to update state for sms quota remaining");
		}
	}

	private void SendStateForQuotaMessages(int remainingMessages)
	{
		LogicalDevice logicalDevice = configRepo.GetLogicalDevices().FirstOrDefault((LogicalDevice m) => m.DeviceType == "SmsActuator");
		if (logicalDevice != null)
		{
			Guid id = logicalDevice.Id;
			GenericDeviceState genericDeviceState = new GenericDeviceState();
			genericDeviceState.LogicalDevice = logicalDevice;
			genericDeviceState.LogicalDeviceId = id;
			genericDeviceState.Properties = new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property>
			{
				new NumericProperty
				{
					Name = "RemainingQuota",
					Value = remainingMessages
				}
			};
			GenericDeviceState deviceState = genericDeviceState;
			eventManager.GetEvent<RawLogicalDeviceStateChangedEvent>().Publish(new RawLogicalDeviceStateChangedEventArgs(id, deviceState));
		}
	}
}
