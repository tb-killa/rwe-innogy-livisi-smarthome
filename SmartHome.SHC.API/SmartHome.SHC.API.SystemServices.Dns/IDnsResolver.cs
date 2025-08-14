using System;
using System.Collections.Generic;

namespace SmartHome.SHC.API.SystemServices.Dns;

public interface IDnsResolver
{
	event EventHandler<DnsPacketReceivedEventArgs> DnsPacketReceived;

	void Resolve(IEnumerable<DnsQuery> queries, TimeSpan duration);

	void ForceDiscoveryFinished();
}
