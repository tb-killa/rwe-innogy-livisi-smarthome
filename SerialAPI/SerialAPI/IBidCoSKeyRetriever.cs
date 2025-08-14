using System;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace SerialAPI;

public interface IBidCoSKeyRetriever
{
	void GetDeviceKey(SGTIN96 deviceSgtin, Action<byte[]> keyRetrieved, Action keyRetrievalFailed, int timeout);

	void StoreDeviceKeyInCache(SGTIN96 deviceSgtin);
}
