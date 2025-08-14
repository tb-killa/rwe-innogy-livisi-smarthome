using System.Collections.Generic;

namespace SmartHome.SHC.API;

public class AddInConfiguration
{
	public IEnumerable<KeyValuePair<string, string>> Parameters { get; set; }
}
