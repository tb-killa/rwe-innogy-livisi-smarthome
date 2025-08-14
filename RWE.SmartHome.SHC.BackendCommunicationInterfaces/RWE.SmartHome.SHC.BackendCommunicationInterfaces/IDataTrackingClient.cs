using System.Collections.Generic;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces;

public interface IDataTrackingClient
{
	bool StoreData(List<TrackData> deviceTrackingEntities);
}
