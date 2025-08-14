using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Messages;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalCommunication;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DataAccessInterfaces.Messages;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces.Events;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.MessagesAndAlerts;

public class MessagesAndAlertsManager : IMessagesAndAlertsManager, IService
{
	private enum UserNotificationOperation
	{
		Added,
		Deleted
	}

	private readonly Container container;

	private readonly IMessagePersistence messagePersistence;

	private IExternalCommandDispatcher externalCommandDispatcher;

	private readonly IEventManager eventManager;

	private readonly IRepository configurationRepository;

	private bool ownerHadUnreadAlerts = true;

	private bool ownerHadUnreadMessages = true;

	private SystemNotificationsManager systemNotificationsManager;

	private IScheduler scheduler;

	private string LoggingSource = "MessagesAndAlertsManager";

	private static XmlSerializer xmlSerializer = new XmlSerializer(typeof(Message));

	private SubscriptionToken startUpEventSubscriptionToken;

	private readonly int maxDaysForNewMessages;

	private readonly int maxDaysForReadMessages;

	private readonly int maxNumberOfMessages;

	public MessagesAndAlertsManager(Container container, Configuration aConfiguration)
	{
		this.container = container;
		messagePersistence = container.Resolve<IMessagePersistence>();
		eventManager = container.Resolve<IEventManager>();
		configurationRepository = container.Resolve<IRepository>();
		maxDaysForNewMessages = ((aConfiguration != null && aConfiguration.MaxDaysForNewMessages.HasValue) ? aConfiguration.MaxDaysForNewMessages.Value : 90);
		maxDaysForReadMessages = ((aConfiguration != null && aConfiguration.MaxDaysForReadMessages.HasValue) ? aConfiguration.MaxDaysForReadMessages.Value : 10);
		maxNumberOfMessages = ((aConfiguration != null && aConfiguration.MaxNumberOfMessages.HasValue) ? aConfiguration.MaxNumberOfMessages.Value : 80);
		systemNotificationsManager = new SystemNotificationsManager(container.Resolve<INotificationServiceClient>(), container.Resolve<IEmailSender>());
		scheduler = container.Resolve<IScheduler>();
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcInitializationFinished, (ShcStartupCompletedEventArgs a) => a.Progress == StartupProgress.CompletedRound2, ThreadOption.BackgroundThread, null);
	}

	public Message AddMessage(Message newUserNotification, MessageAddMode addMode)
	{
		if (newUserNotification.Id == Guid.Empty)
		{
			newUserNotification.Id = Guid.NewGuid();
		}
		if (!CheckMessageSerialization(newUserNotification))
		{
			return null;
		}
		switch (newUserNotification.Class)
		{
		case MessageClass.Message:
		case MessageClass.Alert:
			if (addMode == MessageAddMode.OverwriteExisting)
			{
				RemoveSimilarItem(newUserNotification);
			}
			else if (newUserNotification.Class == MessageClass.Alert)
			{
				Message message = GetAllMessages((Message m) => m.Class == MessageClass.Alert && AreEqualsMessages(m, newUserNotification)).FirstOrDefault();
				if (message != null)
				{
					return message;
				}
				RemoveSimilarItem(newUserNotification);
			}
			else
			{
				CleanupUserMessageBox();
			}
			AddUserNotification(newUserNotification);
			systemNotificationsManager.SendSystemNotification(newUserNotification);
			break;
		case MessageClass.CounterMessage:
			RemoveAlert(newUserNotification);
			break;
		}
		SignalOwnerMessageState();
		return newUserNotification;
	}

	private void OnShcInitializationFinished(ShcStartupCompletedEventArgs args)
	{
		RemoveOldMessages();
		DailySchedulerTask schedulerTask = new DailySchedulerTask(Guid.NewGuid(), RemoveOldMessages, ShcDateTime.UtcNow, 1);
		scheduler.AddSchedulerTask(schedulerTask);
	}

	private void RemoveOldMessages()
	{
		Log.Debug(Module.BusinessLogic, "Removing old messages.");
		Predicate<Message> toDeleteNewMessages = (Message m) => m.Class == MessageClass.Message && m.State == MessageState.New && ShcDateTime.UtcNow.Subtract(m.TimeStamp.ToUniversalTime()).Days >= maxDaysForNewMessages;
		Predicate<Message> toDeleteReadMessages = (Message m) => m.Class == MessageClass.Message && m.State == MessageState.Read && ShcDateTime.UtcNow.Subtract(m.TimeStamp.ToUniversalTime()).Days >= maxDaysForReadMessages;
		List<Message> list = GetAllMessages((Message m) => toDeleteNewMessages(m) || toDeleteReadMessages(m)).ToList();
		try
		{
			list.ForEach(delegate(Message m)
			{
				DeleteMessage(m.Id);
				Log.Debug(Module.BusinessLogic, $"Removed oldest message from {m.TimeStamp}, state was {m.State}");
			});
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, LoggingSource, "Error occurred while cleaning up old messages.");
		}
	}

	private void AddUserNotification(Message newUserNotification)
	{
		try
		{
			messagePersistence.Create(newUserNotification);
			NewMessageNotification newMessageNotification = new NewMessageNotification();
			newMessageNotification.Message = newUserNotification;
			newMessageNotification.Namespace = "core.RWE";
			NewMessageNotification notification = newMessageNotification;
			if (externalCommandDispatcher != null)
			{
				externalCommandDispatcher.SendNotification(notification);
			}
			LogUserNotification(newUserNotification, UserNotificationOperation.Added);
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, LoggingSource, "Error occurred while adding a new user notification.");
		}
	}

	private void RemoveAlert(Message newAlert)
	{
		try
		{
			List<Message> all = messagePersistence.GetAll(MessageClass.Alert, newAlert.Type);
			foreach (Message item in all)
			{
				if (CompareMessagesWithoutChangeableProperties(newAlert, item))
				{
					DeleteMessage(item.Id);
					LogUserNotification(item, UserNotificationOperation.Deleted);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, LoggingSource, "Error occurred while removing an alert.");
		}
	}

	private void RemoveSimilarItem(Message newUserNotification)
	{
		try
		{
			foreach (Message allMessage in GetAllMessages((Message m) => m.Class == newUserNotification.Class && CompareMessagesWithoutChangeableProperties(newUserNotification, m)))
			{
				DeleteMessage(allMessage.Id);
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, LoggingSource, "Error occurred while removing duplicate user notifications.");
		}
	}

	private bool CheckMessageSerialization(Message userNotification)
	{
		try
		{
			using XmlWriter xmlWriter = XmlWriter.Create(TextWriter.Null, new XmlWriterSettings
			{
				Indent = false,
				OmitXmlDeclaration = true
			});
			xmlSerializer.Serialize(xmlWriter, userNotification);
			xmlWriter.Close();
		}
		catch (Exception)
		{
			Log.Error(Module.BusinessLogic, "Unable to serialize message/alert. Rejecting it.");
			return false;
		}
		return true;
	}

	private bool AreEqualsMessages(Message newMessage, Message oldMsg)
	{
		return AreEqualsMessages(newMessage, oldMsg, null);
	}

	private bool AreEqualsMessages(Message newMessage, Message oldMessage, List<string> ignoreProperties)
	{
		if (newMessage.Type != oldMessage.Type || newMessage.AppId != oldMessage.AppId)
		{
			return false;
		}
		if (newMessage.Class == MessageClass.CounterMessage && newMessage.Type == MessageType.ApplicationLoadingError.ToString())
		{
			ignoreProperties = ignoreProperties ?? new List<string>();
			ignoreProperties.Add(MessageParameterKey.AppVersion.ToString());
		}
		if (AreEqualsParameterList(newMessage.Properties, oldMessage.Properties, ignoreProperties) && CompareDeviceLists(newMessage.BaseDeviceIds, oldMessage.BaseDeviceIds))
		{
			return CompareDeviceLists(newMessage.LogicalDeviceIds, oldMessage.LogicalDeviceIds);
		}
		return false;
	}

	private bool CompareMessagesWithoutChangeableProperties(Message newMessage, Message oldMsg)
	{
		return AreEqualsMessages(newMessage, oldMsg, new List<string>
		{
			MessageParameterKey.DeviceName.ToString(),
			MessageParameterKey.DeviceLocation.ToString(),
			MessageParameterKey.MemoryLoad.ToString(),
			"SerialNumber",
			"LocationName"
		});
	}

	private void SignalOwnerMessageState()
	{
		List<Message> all = messagePersistence.GetAll();
		bool flag = all.Any((Message mc) => mc.State == MessageState.New && mc.Class == MessageClass.Alert);
		bool flag2 = all.Any((Message mc) => mc.State == MessageState.New && mc.Class == MessageClass.Message);
		if (flag != ownerHadUnreadAlerts || flag2 != ownerHadUnreadMessages)
		{
			ownerHadUnreadAlerts = flag;
			ownerHadUnreadMessages = flag2;
			OwnerMessageBoxChangedEventArgs e = new OwnerMessageBoxChangedEventArgs();
			e.OwnerHasUnreadAlerts = flag;
			e.OwnerHasUnreadMessages = flag2;
			OwnerMessageBoxChangedEventArgs payload = e;
			eventManager.GetEvent<OwnerMessageBoxChangedEvent>().Publish(payload);
		}
	}

	private void OnShcStartup(ShcStartupCompletedEventArgs args)
	{
		switch (args.Progress)
		{
		case StartupProgress.DatabaseAvailable:
			SignalOwnerMessageState();
			break;
		case StartupProgress.CompletedRound1:
			externalCommandDispatcher = container.Resolve<IExternalCommandDispatcher>();
			break;
		}
	}

	public void DeleteMessage(Guid messageId)
	{
		try
		{
			Message message = messagePersistence.Get(messageId);
			if (message != null)
			{
				Log.Information(Module.BusinessLogic, $"Removing {message.Class} with ID {message.Id}: {GetUserNotificationDescription(message)}");
				messagePersistence.Delete(message.Id);
				SignalOwnerMessageState();
				if (externalCommandDispatcher != null)
				{
					externalCommandDispatcher.SendNotification(new MessageDeletionNotification
					{
						MessageId = messageId,
						Namespace = "core.RWE"
					});
				}
			}
			else
			{
				Log.Debug(Module.BusinessLogic, $"Message with id {messageId.ToString()} not found while trying to remove.");
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, LoggingSource, "Error occured while deleting message: " + ex.Message);
		}
	}

	public IEnumerable<Message> GetAllMessages(Predicate<Message> filter)
	{
		try
		{
			return from p in messagePersistence.GetAll()
				where filter(p)
				select p;
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, LoggingSource, "Error occured while getting messages: {0}" + ex.Message);
			return new List<Message>();
		}
	}

	public Message GetMessageById(Guid messageId)
	{
		try
		{
			return messagePersistence.Get(messageId);
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, LoggingSource, $"Error occured while getting the message with id {messageId}: {ex.Message}");
			return new Message();
		}
	}

	public void UpdateMessageState(Guid messageId, MessageState newState)
	{
		messagePersistence.UpdateState(messageId, newState);
		if (messagePersistence.ContainsMessage(messageId))
		{
			MessageStateChangedNotification messageStateChangedNotification = new MessageStateChangedNotification();
			messageStateChangedNotification.MessageId = messageId;
			messageStateChangedNotification.State = newState;
			messageStateChangedNotification.Namespace = "core.RWE";
			MessageStateChangedNotification notification = messageStateChangedNotification;
			if (externalCommandDispatcher != null)
			{
				externalCommandDispatcher.SendNotification(notification);
			}
			SignalOwnerMessageState();
		}
	}

	public void ReplaceMessage(Message newMessage, Func<Message, bool> func)
	{
		List<Message> list = messagePersistence.GetAll().Where(func).ToList();
		foreach (Message item in list)
		{
			DeleteMessage(item.Id);
		}
		AddMessage(newMessage, MessageAddMode.NewMessage);
	}

	public bool ContainsMessage(Func<Message, bool> func)
	{
		return messagePersistence.ContainsMessage(func);
	}

	public void DeleteAllMessages(Predicate<Message> filter)
	{
		try
		{
			List<Message> all = messagePersistence.GetAll();
			foreach (Message item in all)
			{
				if (filter(item))
				{
					DeleteMessage(item.Id);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, LoggingSource, "Error occured while deleting messages: " + ex.Message);
		}
	}

	internal static bool CompareDeviceLists(List<Guid> newDeviceList, List<Guid> oldDeviceList)
	{
		if ((newDeviceList == null && oldDeviceList != null && oldDeviceList.Count == 0) || (oldDeviceList == null && newDeviceList != null && newDeviceList.Count == 0) || (oldDeviceList == null && newDeviceList == null))
		{
			return true;
		}
		if (!newDeviceList.Except(oldDeviceList).Any())
		{
			return !oldDeviceList.Except(newDeviceList).Any();
		}
		return false;
	}

	private static bool AreEqualsParameterList(List<StringProperty> parameters1, List<StringProperty> parameters2, List<string> ignorePropertiesNames)
	{
		ignorePropertiesNames = ignorePropertiesNames ?? new List<string>();
		List<StringProperty> list = parameters1.Where((StringProperty m) => !ignorePropertiesNames.Contains(m.Name)).ToList();
		List<StringProperty> p2List = parameters2.Where((StringProperty m) => !ignorePropertiesNames.Contains(m.Name)).ToList();
		if (list.Count != p2List.Count)
		{
			return false;
		}
		int num = list.Select((StringProperty m) => m.Name).Distinct().Count();
		int num2 = p2List.Select((StringProperty m) => m.Name).Distinct().Count();
		if (num != list.Count || num2 != p2List.Count)
		{
			return false;
		}
		return list.All((StringProperty m) => p2List.Any((StringProperty k) => k.Name == m.Name) && p2List.First((StringProperty k) => k.Name == m.Name).Value == m.Value);
	}

	private bool GetAssociatedDeviceSerial(Message userNotification, out string serialNumber)
	{
		serialNumber = string.Empty;
		if (userNotification.BaseDeviceIds != null && userNotification.BaseDeviceIds.Count > 0)
		{
			Guid id = userNotification.BaseDeviceIds.FirstOrDefault();
			BaseDevice originalBaseDevice = configurationRepository.GetOriginalBaseDevice(id);
			if (originalBaseDevice == null)
			{
				return false;
			}
			serialNumber = originalBaseDevice.SerialNumber;
		}
		return true;
	}

	private void LogUserNotification(Message userNotification, UserNotificationOperation operation)
	{
		string serialNumber = string.Empty;
		if (GetAssociatedDeviceSerial(userNotification, out serialNumber))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("{0} with ID {1} was {2}: ", (userNotification.Class == MessageClass.Message) ? "Message" : "Alert", userNotification.Id, (operation == UserNotificationOperation.Added) ? "added" : "removed"));
			stringBuilder.Append(GetUserNotificationDescription(userNotification));
			if (userNotification.AppId == CoreConstants.CoreAppId && (userNotification.Type == MessageType.DeviceLowBattery.ToString() || userNotification.Type == MessageType.DeviceLowRfQuality.ToString() || userNotification.Type == MessageType.DeviceUnreachable.ToString() || userNotification.Type == MessageType.DeviceFreeze.ToString() || userNotification.Type == MessageType.DeviceMold.ToString() || userNotification.Type == MessageType.BidCosInclusionTimeout.ToString() || userNotification.Type == MessageType.DeviceFactoryReset.ToString()))
			{
				stringBuilder.Append(" Serial:" + serialNumber);
			}
			Log.Information(Module.ExternalCommandDispatcher, LoggingSource, stringBuilder.ToString());
		}
	}

	private string GetUserNotificationDescription(Message userNotification)
	{
		StringBuilder msg = new StringBuilder();
		if (userNotification.AppId != CoreConstants.CoreAppId)
		{
			msg.Append($"Custom application: [AppId]=[{userNotification.AppId}]  [Type]=[{userNotification.Type}]  ");
			if (userNotification.BaseDeviceIds != null && userNotification.BaseDeviceIds.Count > 0)
			{
				msg.Append(string.Format("BaseDeviceIds = [{0}]  ", string.Join(",", userNotification.BaseDeviceIds.Select((Guid id) => id.ToString()).ToArray())));
			}
			if (userNotification.LogicalDeviceIds != null && userNotification.LogicalDeviceIds.Count > 0)
			{
				msg.Append(string.Format("LogicalDeviceIds = [{0}]  ", string.Join(",", userNotification.LogicalDeviceIds.Select((Guid id) => id.ToString()).ToArray())));
			}
			userNotification.Properties.ForEach(delegate(StringProperty d)
			{
				msg.Append($"[{d.Name}]=[{d.Value}]  ");
			});
			return msg.ToString();
		}
		msg.Append(GetMsgDescriptionByType(userNotification));
		return msg.ToString();
	}

	private string GetMsgDescriptionByType(Message userNotification)
	{
		if (userNotification.Type == MessageType.ShcMandatoryUpdate.ToString())
		{
			return "Mandatory update for SHC pending";
		}
		if (userNotification.Type == MessageType.ShcRealTimeClockLost.ToString())
		{
			return "Real Time Clock lost on SHC";
		}
		if (userNotification.Type == MessageType.UserEmailAddressNotValidated.ToString())
		{
			return $"Email Address of user [{string.Empty}] was not validated yet.";
		}
		if (userNotification.Type == MessageType.DeviceLowBattery.ToString())
		{
			return "The Battery of the Device is Low.";
		}
		if (userNotification.Type == MessageType.DeviceLowRfQuality.ToString())
		{
			return "The Radio Quality of the Device is Low. ";
		}
		if (userNotification.Type == MessageType.DeviceUnreachable.ToString())
		{
			return "The Device is unreachable.";
		}
		if (userNotification.Type == MessageType.UserInvitiationAccepted.ToString())
		{
			string value = userNotification.Properties.Find((StringProperty param) => param.Name == MessageParameterKey.FriendlyName.ToString()).Value;
			return $"User [{value}] has accepted the invitation";
		}
		if (userNotification.Type == MessageType.ShcOnlineSwitchIsOff.ToString())
		{
			return "SHC Online Switch is off.";
		}
		if (userNotification.Type == MessageType.ShcNoConnectionToBackend.ToString())
		{
			return "SHC is not connected to Backend.";
		}
		if (userNotification.Type == MessageType.DeviceFactoryReset.ToString())
		{
			return "Device was Factory Reset.";
		}
		if (userNotification.Type == MessageType.DeviceFreeze.ToString())
		{
			return "DeviceFreeze occurred on Device.";
		}
		if (userNotification.Type == MessageType.DeviceMold.ToString())
		{
			return "DeviceMold occurred on Device.";
		}
		if (userNotification.Type == MessageType.BidCosInclusionTimeout.ToString())
		{
			return "BidCosInclusionTimeout occured on Device.";
		}
		if (userNotification.Type == MessageType.DeviceUpdateAvailable.ToString())
		{
			return "DeviceUpdateAvailable occured.";
		}
		if (userNotification.Type == MessageType.AddressCollision.ToString())
		{
			return "An address collision occcured.";
		}
		if (userNotification.Type == MessageType.BackendConfigOutOfSync.ToString())
		{
			return "Configuration persistence in backend failed.";
		}
		if (userNotification.Type == MessageType.ApplicationLoadingError.ToString())
		{
			return "The application failed to load.";
		}
		if (userNotification.Type == MessageType.AppTokenSyncFailure.ToString())
		{
			return "Could not retrieve the applications token.";
		}
		if (userNotification.Type == MessageType.SmokeDetected.ToString())
		{
			return "Smoke detected.";
		}
		if (userNotification.Type == MessageType.USBDeviceUnplugged.ToString())
		{
			StringBuilder strMsg = new StringBuilder();
			strMsg.Append("USB device unplugged.");
			userNotification.Properties.ForEach(delegate(StringProperty d)
			{
				strMsg.Append($"[{d.Name}]=[{d.Value}]  ");
			});
			return strMsg.ToString();
		}
		if (userNotification.Type == MessageType.InvalidAesKey.ToString())
		{
			return "Invalid AES key.";
		}
		if (userNotification.Type == MessageType.DeviceActivityLoggingEnabled.ToString())
		{
			return "Device activity logging was enabled.";
		}
		if (userNotification.Type == MessageType.MemoryShortage.ToString())
		{
			return "Memory shortage notification. Load " + userNotification.Properties.FirstOrDefault((StringProperty m) => m.Name == MessageParameterKey.MemoryLoad.ToString()).Value + "%";
		}
		if (userNotification.Type == MessageType.LemonBeatDongleInitializationFailed.ToString())
		{
			return "Lemonbeat dongle initialization failed.";
		}
		if (userNotification.Type == MessageType.RuleExecutionFailed.ToString())
		{
			return "Rule execution failed: " + userNotification.Properties.FirstOrDefault((StringProperty m) => m.Name == MessageParameterKey.RuleId.ToString()).Value;
		}
		if (userNotification.Type == MessageType.AppAddedToShc.ToString() || userNotification.Type == MessageType.AppUpdatedOnShc.ToString())
		{
			return "App provisioning changed for: " + userNotification.Properties.FirstOrDefault((StringProperty m) => m.Name == MessageParameterKey.Product.ToString()).Value;
		}
		if (userNotification.Type == MessageType.ProductUpdateAvailable.ToString())
		{
			return "ProductUpdateAvailable for: " + userNotification.Properties.FirstOrDefault((StringProperty m) => m.Name == MessageParameterKey.AppId.ToString()).Value;
		}
		return GenericMessageDescription(userNotification);
	}

	private string GenericMessageDescription(Message userNotification)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("( type: {0},", new object[1] { userNotification.Type });
		stringBuilder.AppendFormat(" class: {0},", new object[1] { userNotification.Class.ToString() });
		string text = string.Join(", ", userNotification.Properties.Select((StringProperty m) => m.ToString()).ToArray());
		stringBuilder.AppendFormat(" properties: [{0}] )", new object[1] { text });
		return stringBuilder.ToString();
	}

	private void CleanupUserMessageBox()
	{
		try
		{
			List<Message> all = messagePersistence.GetAll();
			while (all.Where((Message m) => m.Class == MessageClass.Message).Count() >= maxNumberOfMessages)
			{
				Message message = (from m in all
					where m.Class == MessageClass.Message
					orderby m.State descending, m.TimeStamp
					select m).First();
				all.Remove(message);
				DeleteMessage(message.Id);
				Log.Debug(Module.BusinessLogic, $"Removed oldest message from {message.TimeStamp}, state was {message.State}");
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.BusinessLogic, ex, LoggingSource, "Error occurred while cleaning up user message box.");
		}
	}

	public void Initialize()
	{
		if (startUpEventSubscriptionToken == null)
		{
			startUpEventSubscriptionToken = eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcStartup, null, ThreadOption.PublisherThread, null);
		}
	}

	public void Uninitialize()
	{
		if (startUpEventSubscriptionToken != null)
		{
			eventManager.GetEvent<ShcStartupCompletedEvent>().Unsubscribe(startUpEventSubscriptionToken);
			startUpEventSubscriptionToken = null;
		}
	}
}
