using System.Collections.Generic;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Persistence;

public class AddInSpecificShcValues
{
	public string AppId { get; set; }

	public List<ShcValue> ShcValues { get; set; }
}
