using System.Collections.Generic;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

namespace RWE.SmartHome.SHC.BackendCommunication.LocalCommunication;

internal class DataTrackingClientLocalOnly : IDataTrackingClient
{
	public bool StoreData(List<TrackData> deviceTrackingEntities)
	{
		return false;
	}
}
