using System;
using System.Net;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Persistence;

internal class ProfileAddressEntity
{
	internal Guid ProfileId { get; set; }

	internal IPAddress AssignedMulticastAddress { get; set; }
}
