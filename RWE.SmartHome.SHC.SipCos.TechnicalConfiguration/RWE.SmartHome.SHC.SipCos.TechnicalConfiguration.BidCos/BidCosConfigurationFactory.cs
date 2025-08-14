using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos.Configurations;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.BidCos;

public static class BidCosConfigurationFactory
{
	public static BaseBidCosConfigurator GetConfiguration(LogicalDevice ld, byte[] sourceAddress, byte[] deviceAdress)
	{
		BaseBidCosConfigurator result = null;
		if (ld.DeviceType == "SirenActuator")
		{
			result = new SirenActuatorConfigurator(ld, sourceAddress, deviceAdress);
		}
		return result;
	}
}
