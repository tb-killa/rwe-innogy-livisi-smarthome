using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.Extensions;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService;

public class MessageConverterService : IMessageConverterService
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(MessageConverterService));

	public SmartHome.Common.API.Entities.Entities.Message FromSmartHomeMessage(RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Messages.Message message)
	{
		logger.DebugEnterMethod("FromSmartHomeMessage");
		if (string.IsNullOrEmpty(message.AppId) || string.IsNullOrEmpty(message.AddinVersion) || string.IsNullOrEmpty(message.Type))
		{
			string message2 = $"Null or Empty value for some of parameters: [AppId={message.AppId}; AddinVersion={message.AddinVersion}; Type={message.Type}] for the shc massage [Id={message.Id}]";
			logger.Error(message2);
			throw new ArgumentException(message2);
		}
		SmartHome.Common.API.Entities.Entities.Message message3 = new SmartHome.Common.API.Entities.Entities.Message();
		message3.Id = message.Id.ToString("N");
		message3.Timestamp = message.TimeStamp;
		message3.Class = message.Class.ToString();
		message3.Type = message.Type;
		message3.Read = message.State == MessageState.Read;
		message3.Namespace = message.GetApplicationName();
		SmartHome.Common.API.Entities.Entities.Message message4 = message3;
		if (message.Properties != null && message.Properties.Any())
		{
			message4.Properties = message.Properties.ConvertAll((StringProperty p) => new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = p.Name,
				Value = p.Value.Replace("sh://", string.Empty)
			});
			logger.Debug("Properties conversion done!");
		}
		if (message.BaseDeviceIds != null && message.BaseDeviceIds.Any())
		{
			message4.Devices = new List<string>();
			foreach (Guid baseDeviceId in message.BaseDeviceIds)
			{
				message4.Devices.Add(string.Format("/device/{0}", baseDeviceId.ToString("N")));
			}
			logger.Debug("Base device links conversion done!");
		}
		if (message.LogicalDeviceIds != null && message.LogicalDeviceIds.Any())
		{
			message4.Capabilities = new List<string>();
			foreach (Guid logicalDeviceId in message.LogicalDeviceIds)
			{
				message4.Capabilities.Add(string.Format("/capability/{0}", logicalDeviceId.ToString("N")));
			}
			logger.Debug("Logical device links conversion done!");
		}
		if (message.Tags != null && message.Tags.Any())
		{
			message4.Tags = message.Tags.ConvertAll((Tag t) => new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = t.Name,
				Value = t.Value
			});
			logger.Debug("Tags conversion done!");
		}
		logger.DebugExitMethod("FromSmartHomeMessage");
		return message4;
	}
}
