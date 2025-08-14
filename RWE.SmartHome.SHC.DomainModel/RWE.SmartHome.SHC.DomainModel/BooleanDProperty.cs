using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.DomainModel;

public class BooleanDProperty : ValueStorage<bool?>
{
	public BooleanDProperty(string name, bool? value)
		: base(name, value, DPropertyType.Boolean, (DateTime?)DateTime.UtcNow)
	{
	}

	public BooleanDProperty(string name, bool? value, DateTime? timestamp)
		: base(name, value, DPropertyType.Boolean, timestamp)
	{
	}

	public override RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property ToContracts()
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.BooleanProperty booleanProperty = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.BooleanProperty();
		booleanProperty.Name = base.Name;
		booleanProperty.UpdateTimestamp = base.UpdateTimestamp;
		booleanProperty.Value = base.Value;
		return booleanProperty;
	}

	public override global::SmartHome.SHC.API.PropertyDefinition.Property ToDeviceSDK()
	{
		return new global::SmartHome.SHC.API.PropertyDefinition.BooleanProperty(base.Name, base.Value);
	}
}
