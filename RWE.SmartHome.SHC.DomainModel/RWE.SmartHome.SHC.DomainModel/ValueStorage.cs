using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using SmartHome.SHC.API.PropertyDefinition;

namespace RWE.SmartHome.SHC.DomainModel;

public abstract class ValueStorage<T> : IDProperty
{
	public T Value { get; private set; }

	public DateTime? UpdateTimestamp { get; private set; }

	public string Name { get; private set; }

	public DPropertyType Type { get; private set; }

	public ValueStorage(string name, T value, DPropertyType type, DateTime? timestamp)
	{
		Value = value;
		Name = name;
		UpdateTimestamp = timestamp;
		Type = type;
	}

	public string ValueToString()
	{
		if (Value == null)
		{
			return string.Empty;
		}
		return Value.ToString();
	}

	public abstract RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Property ToContracts();

	public abstract global::SmartHome.SHC.API.PropertyDefinition.Property ToDeviceSDK();
}
