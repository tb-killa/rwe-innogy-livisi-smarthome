using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.DomainModel;

public class DateTimeDProperty : ValueStorage<DateTime?>
{
	public DateTimeDProperty(string name, DateTime? value)
		: base(name, value, DPropertyType.DateTime, (DateTime?)DateTime.UtcNow)
	{
	}

	public DateTimeDProperty(string name, DateTime? value, DateTime? timestamp)
		: base(name, value, DPropertyType.DateTime, timestamp)
	{
	}

	public override RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property ToContracts()
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.DateTimeProperty dateTimeProperty = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.DateTimeProperty();
		dateTimeProperty.Name = base.Name;
		dateTimeProperty.UpdateTimestamp = base.UpdateTimestamp;
		dateTimeProperty.Value = base.Value;
		return dateTimeProperty;
	}

	public override global::SmartHome.SHC.API.PropertyDefinition.Property ToDeviceSDK()
	{
		return new global::SmartHome.SHC.API.PropertyDefinition.DateTimeProperty(base.Name, base.Value);
	}
}
