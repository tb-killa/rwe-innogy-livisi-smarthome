using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using SmartHome.SHC.API.Configuration;
using SmartHome.SHC.API.Messaging;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.CoreApiConverters;

public static class MessageConverters
{
	public static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message ToCoreMessage(this global::SmartHome.SHC.API.Messaging.Message apiMessage, RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.MessageClass messageClass, string appId, string addinVersion)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message message = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message();
		message.AppId = appId;
		message.AddinVersion = addinVersion;
		message.Class = messageClass;
		message.Type = apiMessage.Type;
		message.Properties = apiMessage.Properties.Select((global::SmartHome.SHC.API.PropertyDefinition.StringProperty prop) => prop.ToCoreProperty()).ToList();
		message.BaseDeviceIds = ExtractDeviceIds(apiMessage.AssociatedEntities);
		message.LogicalDeviceIds = ExtractCapabilityIds(apiMessage.AssociatedEntities);
		return message;
	}

	private static List<Guid> ExtractDeviceIds(Link[] link)
	{
		List<Guid> list = new List<Guid>();
		if (link != null)
		{
			try
			{
				list.AddRange(from l in link
					where l.Type == LinkType.Device
					select new Guid(l.Id));
			}
			catch (Exception)
			{
			}
		}
		return list;
	}

	private static List<Guid> ExtractCapabilityIds(Link[] link)
	{
		List<Guid> list = new List<Guid>();
		if (link != null)
		{
			try
			{
				list.AddRange(from l in link
					where l.Type == LinkType.Capability
					select new Guid(l.Id));
			}
			catch (Exception)
			{
			}
		}
		return list;
	}

	public static global::SmartHome.SHC.API.Messaging.Message ToApiMessage(this RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message coreMessage)
	{
		return new global::SmartHome.SHC.API.Messaging.Message(coreMessage.Class.ToApi(), coreMessage.Id, coreMessage.TimeStamp, coreMessage.Type, coreMessage.Properties.Select((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty p) => p.ToApiProperty()).ToArray(), AsLinks(coreMessage.BaseDeviceIds, coreMessage.LogicalDeviceIds));
	}

	private static IEnumerable<Link> AsLinks(List<Guid> baseDevices, List<Guid> logicalDevices)
	{
		List<Link> list = new List<Link>();
		if (baseDevices != null)
		{
			list.AddRange(baseDevices.Select((Guid id) => new Link(LinkType.Device, id)));
		}
		if (logicalDevices != null)
		{
			list.AddRange(logicalDevices.Select((Guid id) => new Link(LinkType.Capability, id)));
		}
		return list;
	}

	public static global::SmartHome.SHC.API.Messaging.MessageClass ToApi(this RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.MessageClass messageClass)
	{
		return messageClass switch
		{
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.MessageClass.Alert => global::SmartHome.SHC.API.Messaging.MessageClass.Alert, 
			RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.MessageClass.Message => global::SmartHome.SHC.API.Messaging.MessageClass.Message, 
			_ => throw new InvalidCastException($"Invalid cast: not an API entity: {messageClass}"), 
		};
	}

	public static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.MessageClass ToCore(this global::SmartHome.SHC.API.Messaging.MessageClass messageClass)
	{
		return messageClass switch
		{
			global::SmartHome.SHC.API.Messaging.MessageClass.Alert => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.MessageClass.Alert, 
			global::SmartHome.SHC.API.Messaging.MessageClass.Message => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.MessageClass.Message, 
			_ => throw new InvalidCastException($"Invalid cast: not an API entity: {messageClass}"), 
		};
	}
}
