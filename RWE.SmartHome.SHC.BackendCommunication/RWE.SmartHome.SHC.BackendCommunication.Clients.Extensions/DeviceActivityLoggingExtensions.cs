using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;
using RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;

internal static class DeviceActivityLoggingExtensions
{
	public static DeviceActivityLogEntry ToShcDalEntry(this DeviceActivityLog logentry)
	{
		try
		{
			DeviceActivityLogEntry deviceActivityLogEntry = new DeviceActivityLogEntry();
			deviceActivityLogEntry.ActivityType = ToShcActivityType(logentry.ActivityType);
			deviceActivityLogEntry.DeviceId = logentry.DeviceId;
			deviceActivityLogEntry.EventType = (logentry.EventTypeSpecified ? ToShcEventType(logentry.EventType) : RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging.EventType.DalEntry);
			deviceActivityLogEntry.NewState = logentry.NewState;
			deviceActivityLogEntry.Timestamp = (logentry.TimestampSpecified ? logentry.Timestamp : default(DateTime));
			return deviceActivityLogEntry;
		}
		catch
		{
			return null;
		}
	}

	private static RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging.EventType ToShcEventType(RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope.EventType serverEventType)
	{
		return serverEventType switch
		{
			RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope.EventType.Both => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging.EventType.Both, 
			RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope.EventType.DalEntry => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging.EventType.DalEntry, 
			RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope.EventType.ShcTrackingEvent => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging.EventType.ShcTrackingEvent, 
			_ => RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging.EventType.DalEntry, 
		};
	}

	private static ActivityType ToShcActivityType(string serverActivityType)
	{
		ActivityType result = ActivityType.LogicalDeviceStateChanged;
		if (Enum.IsDefined(typeof(ActivityType), serverActivityType))
		{
			result = (ActivityType)Enum.Parse(typeof(ActivityType), serverActivityType, ignoreCase: true);
		}
		return result;
	}
}
