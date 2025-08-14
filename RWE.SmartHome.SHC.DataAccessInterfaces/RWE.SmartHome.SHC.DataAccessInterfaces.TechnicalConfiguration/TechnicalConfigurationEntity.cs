using System;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;

public class TechnicalConfigurationEntity
{
	public Guid Id { get; set; }

	public byte[] Data { get; set; }
}
