using RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;

public static class TrackDataExtensions
{
	public static RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope.TrackData ToBeTrackData(this RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.TrackData td)
	{
		RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope.TrackData trackData = new RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope.TrackData();
		trackData.EntityId = td.EntityId;
		trackData.EntityType = td.EntityType;
		trackData.EventType = td.EventType;
		trackData.Timestamp = td.Timestamp;
		trackData.TimestampSpecified = true;
		trackData.DeviceId = td.DeviceId;
		trackData.Properties = ((td.Properties == null) ? new RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope.Property[0] : td.Properties.ConvertAll((RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property p) => p.ToBeProperty()).ToArray());
		return trackData;
	}

	public static RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope.Property ToBeProperty(this RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.Property p)
	{
		RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope.Property property = new RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope.Property();
		property.Name = p.Name;
		property.Value = p.Value;
		return property;
	}
}
