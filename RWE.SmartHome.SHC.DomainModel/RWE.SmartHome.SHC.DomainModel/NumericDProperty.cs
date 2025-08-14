using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.DomainModel;

public class NumericDProperty : ValueStorage<decimal?>
{
	public NumericDProperty(string name, decimal? value)
		: base(name, value, DPropertyType.Numeric, (DateTime?)DateTime.UtcNow)
	{
	}

	public NumericDProperty(string name, decimal? value, DateTime? timestamp)
		: base(name, value, DPropertyType.Numeric, timestamp)
	{
	}

	public override RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property ToContracts()
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.NumericProperty numericProperty = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.NumericProperty();
		numericProperty.Name = base.Name;
		numericProperty.UpdateTimestamp = base.UpdateTimestamp;
		numericProperty.Value = base.Value;
		return numericProperty;
	}

	public override global::SmartHome.SHC.API.PropertyDefinition.Property ToDeviceSDK()
	{
		return new global::SmartHome.SHC.API.PropertyDefinition.NumericProperty(base.Name, base.Value);
	}
}
