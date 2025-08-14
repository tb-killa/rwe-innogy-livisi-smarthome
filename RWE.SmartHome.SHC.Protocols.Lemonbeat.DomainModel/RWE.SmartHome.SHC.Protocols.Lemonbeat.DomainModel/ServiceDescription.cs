namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class ServiceDescription
{
	public ServiceType ServiceType { get; set; }

	public uint Version { get; set; }

	public ServiceDescription()
	{
	}

	public ServiceDescription(ServiceType serviceType, uint version)
	{
		ServiceType = serviceType;
		Version = version;
	}

	public ServiceDescription(ServiceType serviceType, byte version)
	{
		ServiceType = serviceType;
		Version = version;
	}
}
