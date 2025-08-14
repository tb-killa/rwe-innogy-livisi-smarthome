using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;

public interface ITrackDataPersistence : IService
{
	void Add(TrackData data);

	List<TrackData> GetAllEvents();

	List<TrackData> GetEventsForEntity(string entityType, string entityId);

	List<TrackData> GetEventsForEntity(string entityType, DateTime start, DateTime end, int offset, int limit, string dataPattern);

	List<TrackData> GetEventsForEntity(string entityType, string entityId, DateTime start, DateTime end, int offset, int limit, string dataPattern);

	List<TrackData> GetEvents(string entityType, string eventType, DateTime start, DateTime end, int offset, int limit, string dataPattern);

	List<TrackData> GetEvents(string entityType, string entityId, string eventType, DateTime start, DateTime end, int offset, int limit, string dataPattern);

	void DeleteAll();
}
