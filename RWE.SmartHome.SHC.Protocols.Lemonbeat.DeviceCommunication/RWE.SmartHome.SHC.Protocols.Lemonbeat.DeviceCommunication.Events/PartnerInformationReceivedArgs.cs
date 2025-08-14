using System;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;

public class PartnerInformationReceivedArgs : EventArgs
{
	public PartnerInformations PartnerInformations { get; set; }
}
