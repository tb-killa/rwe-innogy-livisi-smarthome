using System.Collections.Generic;

namespace RWE.SmartHome.SHC.Core.LocalCommunication;

public class ShcConfiguration
{
	public class Configuration
	{
		public string ShcSerial { get; set; }

		public string Timestamp { get; set; }

		public bool Active { get; set; }

		public string HardwareType { get; set; }

		public List<string> Archives { get; set; }
	}

	public Configuration Config { get; set; }
}
