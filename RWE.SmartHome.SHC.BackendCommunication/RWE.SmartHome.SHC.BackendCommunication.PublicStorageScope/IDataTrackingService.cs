using System.CodeDom.Compiler;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public interface IDataTrackingService
{
	bool StoreData(TrackData deviceTrackingEntity);

	bool StoreListData(TrackData[] deviceTrackingEntities);
}
