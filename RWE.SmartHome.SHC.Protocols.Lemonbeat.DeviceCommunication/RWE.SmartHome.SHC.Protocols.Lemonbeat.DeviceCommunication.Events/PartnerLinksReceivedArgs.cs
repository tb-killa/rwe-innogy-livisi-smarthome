using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;

public class PartnerLinksReceivedArgs : EventArgs
{
	public IEnumerable<Link> Links { get; set; }
}
