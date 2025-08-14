using SmartHome.SHC.API.Configuration;

namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

public class TriggerCalculations
{
	public PartnerCalculation TriggerOnCalculation { get; set; }

	public PartnerCalculation TriggerOffCalculation { get; set; }

	public Trigger Trigger { get; set; }
}
