using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalCommunication;

public class SmtpEmail
{
	public Guid DeviceId { get; set; }

	public DateTime SendingDate { get; set; }
}
