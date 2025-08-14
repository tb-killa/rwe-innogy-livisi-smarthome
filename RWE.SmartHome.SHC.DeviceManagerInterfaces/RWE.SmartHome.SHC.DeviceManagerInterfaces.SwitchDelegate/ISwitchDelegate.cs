using System.Collections.Generic;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.SwitchDelegate;

public interface ISwitchDelegate
{
	void SetLookupTable(IEnumerable<KeyValuePair<LinkEndpoint, IEnumerable<byte[]>>> lookupTable);
}
