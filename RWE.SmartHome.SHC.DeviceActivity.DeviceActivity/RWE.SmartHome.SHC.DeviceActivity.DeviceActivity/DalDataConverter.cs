using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Sensors;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.DeviceActivity.DeviceActivity;

public class DalDataConverter
{
	private const string DiscriminantPropertyName = "Discriminant";

	private readonly IRepository config;

	public DalDataConverter(IRepository config)
	{
		this.config = config;
	}

	public TrackData ConvertToTrackData(DeviceActivityLogEntry dalEntry)
	{
		TrackData result = new TrackData();
		if (dalEntry.ActivityType == ActivityType.DeviceExcluded)
		{
			return ComputeTrackDataForExcludedDevice(dalEntry);
		}
		LogicalDevice logicalDevice = config.GetLogicalDevice(new Guid(dalEntry.DeviceId));
		if (logicalDevice != null)
		{
			result = ComputeTrackDataForLogicalDevice(dalEntry, logicalDevice);
		}
		else
		{
			BaseDevice baseDevice = config.GetBaseDevice(new Guid(dalEntry.DeviceId));
			if (baseDevice != null)
			{
				result = ComputeTrackDataForBaseDevice(dalEntry, baseDevice);
			}
		}
		return result;
	}

	private TrackData ComputeTrackDataForExcludedDevice(DeviceActivityLogEntry dalEntry)
	{
		TrackData trackData = new TrackData();
		trackData.EntityId = GetEntityIdForExcludedDevice(dalEntry);
		trackData.EventType = ConvertActivityTypeToCorrespondingEventType(dalEntry.ActivityType);
		trackData.EntityType = "device";
		trackData.Timestamp = dalEntry.Timestamp;
		trackData.DeviceId = dalEntry.DeviceId;
		trackData.Properties = new List<RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property>();
		return trackData;
	}

	private TrackData ComputeTrackDataForLogicalDevice(DeviceActivityLogEntry dalEntry, LogicalDevice ld)
	{
		string sourceString = ld.PrimaryPropertyName;
		if (ld is PushButtonSensor)
		{
			sourceString = "LastPressedButtonIndex";
		}
		if (ld is MotionDetectionSensor)
		{
			sourceString = "MotionDetectedCount";
		}
		TrackData trackData = new TrackData();
		trackData.EntityId = GetEntityIdForLogicalDevice(ld);
		trackData.EventType = "StateChanged";
		trackData.EntityType = "capability";
		trackData.Timestamp = dalEntry.Timestamp;
		trackData.DeviceId = dalEntry.DeviceId;
		trackData.Properties = new List<RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property>
		{
			new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property
			{
				Name = GetCamelCaseString(sourceString),
				Value = dalEntry.NewState
			}
		};
		return trackData;
	}

	private TrackData ComputeTrackDataForBaseDevice(DeviceActivityLogEntry dalEntry, BaseDevice bd)
	{
		TrackData trackData = new TrackData();
		trackData.EntityId = GetEntityIdForBaseDevice(bd);
		trackData.EventType = ConvertActivityTypeToCorrespondingEventType(dalEntry.ActivityType);
		trackData.EntityType = "device";
		trackData.Timestamp = dalEntry.Timestamp;
		trackData.DeviceId = dalEntry.DeviceId;
		trackData.Properties = GetTrackDataProperties(dalEntry, bd);
		return trackData;
	}

	private string ConvertActivityTypeToCorrespondingEventType(ActivityType activityType)
	{
		switch (activityType)
		{
		case ActivityType.ShcMemoryLoadExceeded:
		case ActivityType.ShcMemoryLoadOK:
			return "ShcMemoryThreshold";
		case ActivityType.ShcDeviceUnReachable:
		case ActivityType.ShcDeviceReachable:
			return "DeviceReachabilityChanged";
		default:
			return activityType.ToString();
		}
	}

	private List<RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property> GetTrackDataProperties(DeviceActivityLogEntry dalEntry, BaseDevice bd)
	{
		List<RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property> list = new List<RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property>();
		switch (dalEntry.ActivityType)
		{
		case ActivityType.DeviceActive:
			list.Add(new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property
			{
				Name = "product",
				Value = bd.AppId
			});
			list.Add(new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property
			{
				Name = "reason",
				Value = dalEntry.NewState
			});
			break;
		case ActivityType.ShcMemoryLoadExceeded:
			list.Add(new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property
			{
				Name = "threshold",
				Value = dalEntry.NewState
			});
			list.Add(new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property
			{
				Name = "status",
				Value = "aboveThreshold"
			});
			break;
		case ActivityType.ShcMemoryLoadOK:
			list.Add(new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property
			{
				Name = "threshold",
				Value = dalEntry.NewState
			});
			list.Add(new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property
			{
				Name = "status",
				Value = "belowThreshold"
			});
			break;
		case ActivityType.ShcDeviceReachable:
			list.Add(new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property
			{
				Name = "isReachable",
				Value = true
			});
			break;
		case ActivityType.ShcDeviceUnReachable:
			list.Add(new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property
			{
				Name = "isReachable",
				Value = false
			});
			break;
		case ActivityType.ShcSoftwareUpdate:
			list.Add(new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property
			{
				Name = "updateStatus",
				Value = (Convert.ToBoolean(dalEntry.NewState) ? "successful" : "failed")
			});
			break;
		case ActivityType.ShcConnectivityStateChanged:
			list.Add(new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property
			{
				Name = "connectivityStatus",
				Value = (Convert.ToBoolean(dalEntry.NewState) ? "connected" : "disconnected")
			});
			break;
		case ActivityType.LocalCommunicationStatus:
			list.Add(new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property
			{
				Name = "status",
				Value = dalEntry.NewState
			});
			break;
		}
		return list;
	}

	private string GetEntityIdForBaseDevice(BaseDevice bd)
	{
		return $"{bd.Manufacturer}.{bd.DeviceType}.{bd.SerialNumber}";
	}

	private string GetEntityIdForLogicalDevice(LogicalDevice ld)
	{
		StringProperty stringProperty = ld.Properties.OfType<StringProperty>().FirstOrDefault((StringProperty p) => p.Name == "Discriminant");
		string manufacturer = ld.BaseDevice.Manufacturer;
		string deviceType = ld.BaseDevice.DeviceType;
		string serialNumber = ld.BaseDevice.SerialNumber;
		string deviceType2 = ld.DeviceType;
		string text = stringProperty?.Value;
		if (string.IsNullOrEmpty(text))
		{
			return $"{manufacturer}.{deviceType}.{serialNumber}.{deviceType2}";
		}
		return $"{manufacturer}.{deviceType}.{serialNumber}.{deviceType2}.{text}";
	}

	private string GetEntityIdForExcludedDevice(DeviceActivityLogEntry dalEntry)
	{
		string[] array = dalEntry.NewState.Split(';');
		return $"{array[4]}.{array[1]}.{array[3]}";
	}

	private string GetCamelCaseString(string sourceString)
	{
		if (sourceString.Length > 2 && char.IsUpper(sourceString[0]) && char.IsUpper(sourceString[1]))
		{
			return sourceString;
		}
		char[] array = sourceString.ToCharArray();
		array[0] = char.ToLowerInvariant(array[0]);
		return new string(array);
	}
}
