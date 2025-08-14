using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Configuration;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.Serializers;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService.EventConverters;

public class SaveConfigurationErrorNotificationConverter : IEventConverter
{
	private readonly ILogger logger = LogManager.Instance.GetLogger<SaveConfigurationErrorNotificationConverter>();

	public List<Event> FromNotification(BaseNotification notification)
	{
		List<Event> result = new List<Event>();
		if (!(notification is SaveConfigurationErrorNotification saveConfigurationErrorNotification))
		{
			return result;
		}
		if (saveConfigurationErrorNotification.ErrorEntries.Any((ErrorEntry ee) => ee.ErrorCode == ValidationErrorCode.MaximumNumberOfDevicesReached))
		{
			List<Event> list = new List<Event>();
			list.Add(ToLicenseLimitationReachedEvent(saveConfigurationErrorNotification));
			return list;
		}
		List<Event> list2 = new List<Event>();
		list2.Add(ToSaveConfigErrorEvent(saveConfigurationErrorNotification));
		return list2;
	}

	private Event ToSaveConfigErrorEvent(SaveConfigurationErrorNotification saveConfigurationErrorNotification)
	{
		Event obj = new Event();
		obj.Type = "SaveConfigError";
		obj.Timestamp = saveConfigurationErrorNotification.Timestamp;
		obj.Link = string.Format("/desc/device/{0}.{1}/{2}", BuiltinPhysicalDeviceType.SHC, "RWE", "1.0");
		obj.Properties = new List<SmartHome.Common.API.Entities.Entities.Property>
		{
			new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = "ConfigurationVersion",
				Value = saveConfigurationErrorNotification.ConfigurationVersion
			},
			new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = "ErrorReason",
				Value = saveConfigurationErrorNotification.ErrorReason.ToString()
			}
		};
		obj.SequenceNumber = saveConfigurationErrorNotification.SequenceNumber;
		obj.Namespace = saveConfigurationErrorNotification.Namespace;
		Event obj2 = obj;
		foreach (ErrorEntry errorEntry in saveConfigurationErrorNotification.ErrorEntries)
		{
			obj2.Properties.Add(GetErrorEntryProperty(errorEntry));
		}
		return obj2;
	}

	private Event ToLicenseLimitationReachedEvent(SaveConfigurationErrorNotification saveConfigurationErrorNotification)
	{
		ErrorEntry errorEntry = saveConfigurationErrorNotification.ErrorEntries.FirstOrDefault((ErrorEntry ee) => ee.ErrorCode == ValidationErrorCode.MaximumNumberOfDevicesReached);
		Event obj = new Event();
		obj.Type = "LicensingError";
		obj.Timestamp = saveConfigurationErrorNotification.Timestamp;
		obj.Link = string.Format("/desc/device/{0}.{1}/{2}", BuiltinPhysicalDeviceType.SHC, "RWE", "1.0");
		obj.Properties = new List<SmartHome.Common.API.Entities.Entities.Property>
		{
			new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = "ConfigurationVersion",
				Value = saveConfigurationErrorNotification.ConfigurationVersion
			},
			new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = "ErrorReason",
				Value = errorEntry.ErrorCode.ToString()
			},
			new SmartHome.Common.API.Entities.Entities.Property
			{
				Name = "LicensingStateQuota",
				Value = errorEntry.ErrorParameters.FirstOrDefault((ErrorParameter ep) => ep.Key == ErrorParameterKey.LicenseState).Value
			}
		};
		obj.SequenceNumber = saveConfigurationErrorNotification.SequenceNumber;
		Event obj2 = obj;
		foreach (ErrorEntry item in saveConfigurationErrorNotification.ErrorEntries.Where((ErrorEntry ee) => ee.ErrorCode != ValidationErrorCode.MaximumNumberOfDevicesReached))
		{
			obj2.Properties.Add(GetErrorEntryProperty(item));
		}
		logger.DebugExitMethod("ToLicenseLimitationReachedEvent");
		return obj2;
	}

	private SmartHome.Common.API.Entities.Entities.Property GetErrorEntryProperty(ErrorEntry errorEntry)
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType? entityType = null;
		string id = null;
		if (errorEntry.AffectedEntity != null)
		{
			entityType = errorEntry.AffectedEntity.EntityType;
			id = errorEntry.AffectedEntity.Id.ToString("N");
		}
		SmartHome.Common.API.Entities.Entities.Property property = new SmartHome.Common.API.Entities.Entities.Property();
		property.Name = "ErrorEntry";
		property.Value = ApiJsonSerializer.Serialize(new
		{
			ErrorCode = errorEntry.ErrorCode,
			EntityType = entityType,
			Id = id,
			ErrorParameters = errorEntry.ErrorParameters
		});
		return property;
	}
}
