using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.CoreApiConverters;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.Messaging;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.ApplicationsHost.Messaging;

public class ApplicationsMessageHandler : IMessageService
{
	private readonly IMessagesAndAlertsManager messagesAndAlerts;

	private readonly string appId;

	private readonly string addInVersion;

	public ApplicationsMessageHandler(string appId, string addInVersion, IMessagesAndAlertsManager messagesAndAlerts)
	{
		this.messagesAndAlerts = messagesAndAlerts;
		this.appId = appId;
		this.addInVersion = addInVersion;
	}

	public global::SmartHome.SHC.API.Messaging.Message CreateMessage(global::SmartHome.SHC.API.Messaging.MessageClass messageClass, string type, IEnumerable<global::SmartHome.SHC.API.PropertyDefinition.StringProperty> properties, IEnumerable<Link> associatedEntities)
	{
		List<Link> links = ((associatedEntities != null) ? associatedEntities.ToList() : new List<Link>());
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message message = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message();
		message.AppId = appId;
		message.AddinVersion = addInVersion;
		message.Class = messageClass.ToCore();
		message.Type = type;
		message.Properties = ((properties != null) ? properties.Select((global::SmartHome.SHC.API.PropertyDefinition.StringProperty prop) => prop.ToCoreProperty()).ToList() : new List<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty>());
		message.BaseDeviceIds = GetLinkedEntityIds(links, LinkType.Device);
		message.LogicalDeviceIds = GetLinkedEntityIds(links, LinkType.Capability);
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message message2 = message;
		return messagesAndAlerts.AddMessage(message2, MessageAddMode.NewMessage)?.ToApiMessage();
	}

	public global::SmartHome.SHC.API.Messaging.Message CreateMessage(global::SmartHome.SHC.API.Messaging.MessageClass messageClass, string type, IEnumerable<global::SmartHome.SHC.API.PropertyDefinition.StringProperty> properties)
	{
		return CreateMessage(messageClass, type, properties, null);
	}

	public global::SmartHome.SHC.API.Messaging.Message CreateMessage(global::SmartHome.SHC.API.Messaging.MessageClass messageClass, string type, IEnumerable<Link> associatedEntities)
	{
		return CreateMessage(messageClass, type, null, associatedEntities);
	}

	public global::SmartHome.SHC.API.Messaging.Message CreateMessage(global::SmartHome.SHC.API.Messaging.MessageClass messageClass, string type)
	{
		return CreateMessage(messageClass, type, null, null);
	}

	public IEnumerable<global::SmartHome.SHC.API.Messaging.Message> GetAllMessages(global::SmartHome.SHC.API.Messaging.MessageClass messageClass)
	{
		return (from msg in GetAllMessagesForAddin(messageClass.ToCore())
			select msg.ToApiMessage()).ToArray();
	}

	public global::SmartHome.SHC.API.Messaging.Message GetMessage(Guid id)
	{
		return GetCoreMessageById(id)?.ToApiMessage();
	}

	public bool RemoveMessage(Guid id)
	{
		bool result = false;
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message coreMessageById = GetCoreMessageById(id);
		if (coreMessageById != null && coreMessageById.Class == RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.MessageClass.Alert)
		{
			messagesAndAlerts.DeleteAllMessages((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message m) => m.Id == id);
			result = true;
		}
		return result;
	}

	private RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message GetCoreMessageById(Guid id)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message messageById = messagesAndAlerts.GetMessageById(id);
		return (messageById != null && messageById.Id == id && messageById.AppId == appId) ? messageById : null;
	}

	private List<Guid> GetLinkedEntityIds(IEnumerable<Link> links, LinkType linkType)
	{
		List<Guid> list = new List<Guid>();
		if (links == null)
		{
			return list;
		}
		try
		{
			list.AddRange(from link in links
				where link.Type == linkType
				select new Guid(link.Id));
		}
		catch (Exception)
		{
			Log.Error(Module.ApplicationsHost, "Invalid link definition");
		}
		return list;
	}

	private IEnumerable<RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message> GetAllMessagesForAddin(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.MessageClass messageClass)
	{
		return messagesAndAlerts.GetAllMessages((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message msg) => msg.AppId == appId && msg.Class == messageClass);
	}

	internal void Uninstall()
	{
		try
		{
			messagesAndAlerts.DeleteAllMessages((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message msg) => msg.AppId == appId);
		}
		catch (Exception ex)
		{
			Log.Exception(Module.ApplicationsHost, ex, "Error while removing messages and alerts for add-in [{0}]", appId);
		}
	}
}
