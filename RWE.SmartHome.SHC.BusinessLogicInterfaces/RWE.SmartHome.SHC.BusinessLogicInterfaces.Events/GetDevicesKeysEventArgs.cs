using System.Collections.Generic;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class GetDevicesKeysEventArgs
{
	public List<byte[]> Sgtins { get; set; }
}
