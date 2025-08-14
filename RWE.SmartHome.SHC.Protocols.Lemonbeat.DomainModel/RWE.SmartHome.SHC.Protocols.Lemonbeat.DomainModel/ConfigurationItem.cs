using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public abstract class ConfigurationItem : IEquatable<ConfigurationItem>
{
	public uint Id { get; set; }

	public abstract bool Equals(ConfigurationItem other);

	public override bool Equals(object other)
	{
		if (!(other is ConfigurationItem))
		{
			return false;
		}
		return Equals(other as ConfigurationItem);
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}
}
