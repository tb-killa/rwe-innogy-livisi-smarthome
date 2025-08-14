using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;

namespace RWE.SmartHome.SHC.ApplicationsHost;

internal class AppHostMessageFactory
{
	private const string MemoryLimitProperty = "MemoryLimit";

	private const string CurrentMemoryLoadProperty = "CurrentMemoryLoad";

	private readonly List<Message> invalidAppMessages = new List<Message>();

	private readonly List<Message> downloadFailures = new List<Message>();

	private IMessagesAndAlertsManager messagesAndAlerts;

	internal AppHostMessageFactory(IMessagesAndAlertsManager messagesAndAlerts)
	{
		this.messagesAndAlerts = messagesAndAlerts;
	}

	internal void CreateUpdateAvailableMessage(ApplicationTokenEntry entry)
	{
		if (messagesAndAlerts != null)
		{
			Message message = CreateMessage(MessageClass.Message, MessageType.ProductUpdateAvailable, entry, null);
			if (!messagesAndAlerts.ContainsMessage((Message m) => m.Properties.Select((StringProperty p) => p.Value).Contains(entry.AppId) && m.Type == MessageType.ProductUpdateAvailable.ToString()))
			{
				messagesAndAlerts.AddMessage(message, MessageAddMode.NewMessage);
			}
		}
	}

	internal void CreateApplicationActivatedMessage(ApplicationTokenEntry entry, string initiator)
	{
		if (messagesAndAlerts != null)
		{
			Message msg = CreateMessage(MessageClass.Message, MessageType.ProductActivated, entry, new List<StringProperty>
			{
				new StringProperty(MessageParameterKey.AppActivationInitiator.ToString(), initiator)
			});
			AddMessageAndRemoveComplementaryMessage(entry, msg, MessageType.ProductDeactivated);
		}
	}

	internal void CreateApplicationDeactivatedMessage(ApplicationTokenEntry entry, string initiator)
	{
		if (messagesAndAlerts != null)
		{
			Message msg = CreateMessage(MessageClass.Message, MessageType.ProductDeactivated, entry, new List<StringProperty>
			{
				new StringProperty(MessageParameterKey.AppActivationInitiator.ToString(), initiator)
			});
			AddMessageAndRemoveComplementaryMessage(entry, msg, MessageType.ProductActivated);
		}
	}

	internal void CreateApplicationRemovedMessage(string appId)
	{
		if (messagesAndAlerts != null)
		{
			Message message = new Message(MessageClass.Message, MessageType.ProductRemoved.ToString(), new List<StringProperty>
			{
				new StringProperty(MessageParameterKey.AppId.ToString(), appId)
			});
			message.AppId = CoreConstants.CoreAppId;
			message.AddinVersion = "1.0";
			messagesAndAlerts.AddMessage(message, MessageAddMode.NewMessage);
		}
	}

	internal void UpdateApplicationLoadFailureAlert(ApplicationTokenEntry entry)
	{
		AddAppLoadFailureAlert(entry, null);
	}

	internal void AddMemoryLimitAppLoadFailureAlert(ApplicationTokenEntry entry, int memoryLoad, int memoryLoadThreshold)
	{
		AddAppLoadFailureAlert(entry, null);
	}

	internal void DeleteMemoryLimitAppLoadFailureAlert(string appId)
	{
		messagesAndAlerts.DeleteAllMessages((Message m) => m.Class == MessageClass.Alert && m.Type == MessageType.ApplicationLoadingError.ToString() && m.Properties.Any((StringProperty p) => p.Name == MessageParameterKey.AppId.ToString()) && m.Properties.First((StringProperty p) => p.Name == MessageParameterKey.AppId.ToString()).Value == appId && m.Properties.Any((StringProperty p) => p.Name == "CurrentMemoryLoad") && m.Properties.Any((StringProperty p) => p.Name == "MemoryLimit"));
	}

	private void AddAppLoadFailureAlert(ApplicationTokenEntry entry, List<StringProperty> additionalProperties)
	{
		if (messagesAndAlerts != null)
		{
			Message message = CreateMessage((!entry.ActiveOnShc) ? MessageClass.Alert : MessageClass.CounterMessage, MessageType.ApplicationLoadingError, entry, additionalProperties);
			message.AppId = CoreConstants.CoreAppId;
			message.AddinVersion = "1.0";
			messagesAndAlerts.AddMessage(message, MessageAddMode.NewMessage);
		}
	}

	internal void UpdateApplicationsTokenLoadFailureAlert(bool success)
	{
		if (messagesAndAlerts != null)
		{
			Message message = new Message((!success) ? MessageClass.Alert : MessageClass.CounterMessage, MessageType.AppTokenSyncFailure.ToString(), new List<StringProperty>());
			message.AppId = CoreConstants.CoreAppId;
			message.AddinVersion = "1.0";
			Message message2 = message;
			messagesAndAlerts.AddMessage(message2, MessageAddMode.NewMessage);
		}
	}

	internal void CreateInvalidApplicationMessage(ApplicationTokenEntry entry)
	{
		if (messagesAndAlerts != null && !invalidAppMessages.Any((Message m) => m.AppId == entry.AppId && m.AddinVersion == entry.Version))
		{
			Message message = CreateMessage(MessageClass.Message, MessageType.InvalidCustomApp, entry, null);
			messagesAndAlerts.AddMessage(message, MessageAddMode.NewMessage);
			invalidAppMessages.Add(message);
		}
	}

	internal void CreateDownloadFailureMessage(ApplicationTokenEntry entry)
	{
		if (messagesAndAlerts != null && !downloadFailures.Any((Message m) => m.AppId == entry.AppId && m.AddinVersion == entry.Version))
		{
			Message message = CreateMessage(MessageClass.Message, MessageType.AppDownloadFailed, entry, null);
			messagesAndAlerts.AddMessage(message, MessageAddMode.NewMessage);
			downloadFailures.Add(message);
		}
	}

	internal void CreateApplicationUpdatedMessage(ApplicationTokenEntry entry, bool success)
	{
		if (messagesAndAlerts != null)
		{
			Message message = CreateMessage(MessageClass.Message, success ? MessageType.CustomAppWasUpgraded : MessageType.CustomAppUpgradeFailed, entry, null);
			message.AppId = CoreConstants.CoreAppId;
			message.AddinVersion = "1.0";
			if (!AppUpdateMsgAlreadyExists(message))
			{
				AddMessageAndRemoveComplementaryMessage(entry, message, MessageType.ProductUpdateAvailable);
			}
		}
	}

	internal bool AppUpdateMsgAlreadyExists(Message msg)
	{
		bool result = false;
		try
		{
			result = messagesAndAlerts.GetAllMessages((Message filter) => filter.AddinVersion == msg.AddinVersion && filter.AppId == msg.AppId && filter.Type == msg.Type).Any();
		}
		catch
		{
		}
		return result;
	}

	internal Message CreateMessage(MessageClass messageClass, MessageType messageType, ApplicationTokenEntry entry, List<StringProperty> additionalProperties)
	{
		List<StringProperty> properties = new List<StringProperty>
		{
			new StringProperty(MessageParameterKey.AppName.ToString(), entry.Name),
			new StringProperty(MessageParameterKey.AppId.ToString(), entry.AppId),
			new StringProperty(MessageParameterKey.AppVersion.ToString(), entry.Version ?? string.Empty)
		};
		if (additionalProperties != null && additionalProperties.Any())
		{
			additionalProperties.ForEach(delegate(StringProperty p)
			{
				properties.Add(p);
			});
		}
		Message message = new Message(messageClass, messageType.ToString(), properties);
		message.AppId = CoreConstants.CoreAppId;
		message.AddinVersion = "1.0";
		return message;
	}

	internal void AddMessageAndRemoveComplementaryMessage(ApplicationTokenEntry entry, Message msg, MessageType complementaryMessageType)
	{
		Predicate<Message> predicate = (Message m) => m.Properties.Select((StringProperty x) => x.Value).Contains(entry.AppId) && m.Type == complementaryMessageType.ToString();
		Message message = messagesAndAlerts.GetAllMessages(predicate).SingleOrDefault();
		if (message == null)
		{
			messagesAndAlerts.AddMessage(msg, MessageAddMode.NewMessage);
			return;
		}
		messagesAndAlerts.ReplaceMessage(msg, (Message m) => predicate(m));
	}

	internal void DeleteAllMessages(Predicate<Message> predicate)
	{
		messagesAndAlerts.DeleteAllMessages(predicate);
	}

	internal IMessagesAndAlertsManager GetMessagesManager()
	{
		return messagesAndAlerts;
	}
}
