using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.BackendCommunication.NotificationScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;

internal static class NotificationServiceExtensions
{
	internal static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationResponse Convert(this RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationResponse beResponse)
	{
		RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationSendResult notificationSendResult = RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationSendResult.Success;
		return new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationResponse(beResponse.Result switch
		{
			RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationSendResult.Success => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationSendResult.Success, 
			RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationSendResult.Unauthorized => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationSendResult.Unauthorized, 
			RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationSendResult.UnexpectedFailure => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationSendResult.UnexpectedFailure, 
			RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationSendResult.NoContingentProvisioned => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationSendResult.NoContingentProvisioned, 
			RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationSendResult.InvalidDestination => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationSendResult.InvalidDestination, 
			_ => throw new Exception("Unsupported ResultType " + beResponse), 
		}, beResponse.RemainingQuota, beResponse.QuotaResult.Convert());
	}

	internal static RWE.SmartHome.SHC.BackendCommunication.NotificationScope.CustomNotification Convert(this RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.CustomNotification toConvert)
	{
		RWE.SmartHome.SHC.BackendCommunication.NotificationScope.CustomNotification customNotification = new RWE.SmartHome.SHC.BackendCommunication.NotificationScope.CustomNotification();
		customNotification.Title = toConvert.Title;
		customNotification.Body = toConvert.Body;
		customNotification.Channel = toConvert.Channel.Convert();
		customNotification.ChannelSpecified = toConvert.ChannelSpecified;
		customNotification.CustomRecipients = toConvert.CustomRecipients;
		customNotification.UserNames = toConvert.UserNames;
		return customNotification;
	}

	internal static RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationChannelType Convert(this RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationChannelType toConvert)
	{
		return toConvert switch
		{
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationChannelType.Email => RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationChannelType.Email, 
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationChannelType.SMS => RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationChannelType.SMS, 
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationChannelType.Push => RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationChannelType.Push, 
			_ => throw new Exception("Unsupported NotificationChannelType " + toConvert), 
		};
	}

	internal static RWE.SmartHome.SHC.BackendCommunication.NotificationScope.SystemNotification Convert(this RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.SystemNotification toConvert)
	{
		RWE.SmartHome.SHC.BackendCommunication.NotificationScope.SystemNotification systemNotification = new RWE.SmartHome.SHC.BackendCommunication.NotificationScope.SystemNotification();
		systemNotification.ProductId = toConvert.ProductId;
		systemNotification.Parameteres = toConvert.Parameters.Convert();
		systemNotification.Class = toConvert.Class;
		systemNotification.Type = toConvert.Type;
		return systemNotification;
	}

	internal static KeyValuePairOfstringstring[] Convert(this List<KeyValuePair<string, string>> toConvert)
	{
		if (toConvert != null)
		{
			return toConvert.Select((KeyValuePair<string, string> x) => new KeyValuePairOfstringstring
			{
				key = x.Key,
				value = x.Value
			}).ToArray();
		}
		return new KeyValuePairOfstringstring[0];
	}

	internal static List<KeyValuePair<RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationChannelType, int>> Convert(this KeyValuePairOfNotificationChannelTypeQuotaResultwzPNIxF0[] toConvert)
	{
		if (toConvert != null)
		{
			return toConvert.Select((KeyValuePairOfNotificationChannelTypeQuotaResultwzPNIxF0 pair) => new KeyValuePair<RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationChannelType, int>(pair.key.Convert(), pair.value.RemainingQuota)).ToList();
		}
		return new List<KeyValuePair<RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationChannelType, int>>();
	}

	internal static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationChannelType Convert(this RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationChannelType toConvert)
	{
		return toConvert switch
		{
			RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationChannelType.Email => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationChannelType.Email, 
			RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationChannelType.SMS => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationChannelType.SMS, 
			RWE.SmartHome.SHC.BackendCommunication.NotificationScope.NotificationChannelType.Push => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender.NotificationChannelType.Push, 
			_ => throw new Exception("Unsupported NotificationChannelType " + toConvert), 
		};
	}
}
