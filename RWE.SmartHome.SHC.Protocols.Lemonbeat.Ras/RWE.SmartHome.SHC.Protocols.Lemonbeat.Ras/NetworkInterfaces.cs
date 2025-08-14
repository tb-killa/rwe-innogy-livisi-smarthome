using System.Collections.Generic;
using System.Linq;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

public class NetworkInterfaces
{
	public class NetworkInterface
	{
		public string Name { get; set; }

		public uint Index { get; set; }
	}

	private readonly AdaptersMonitor adaptersMonitor = new AdaptersMonitor();

	public List<NetworkInterface> GetAllIPv6Interfaces()
	{
		return (from m in adaptersMonitor.GetAllIPv6Interfaces()
			select new NetworkInterface
			{
				Name = m.Key,
				Index = m.Value
			}).ToList();
	}
}
