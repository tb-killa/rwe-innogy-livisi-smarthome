using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.DomainModel;

public class StringDProperty : ValueStorage<string>
{
	public StringDProperty(string name, string value)
		: base(name, value, DPropertyType.String, (DateTime?)DateTime.UtcNow)
	{
	}

	public StringDProperty(string name, string value, DateTime? timestamp)
		: base(name, value, DPropertyType.String, timestamp)
	{
	}

	public override RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property ToContracts()
	{
		RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty stringProperty = new RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.StringProperty();
		stringProperty.Name = base.Name;
		stringProperty.UpdateTimestamp = base.UpdateTimestamp;
		stringProperty.Value = base.Value;
		return stringProperty;
	}

	public override global::SmartHome.SHC.API.PropertyDefinition.Property ToDeviceSDK()
	{
		return new global::SmartHome.SHC.API.PropertyDefinition.StringProperty(base.Name, base.Value);
	}
}
