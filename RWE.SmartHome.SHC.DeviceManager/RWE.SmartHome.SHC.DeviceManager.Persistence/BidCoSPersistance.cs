using RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager.Persistence;

internal class BidCoSPersistance : IBidCoSPersistence
{
	private readonly ISipCosPersistence persistence;

	public BidCoSPersistance(ISipCosPersistence presistence)
	{
		persistence = presistence;
	}

	public void Save(string bidcosSerializedData)
	{
		persistence.SaveBidCosMappingsInTransaction(bidcosSerializedData);
	}
}
