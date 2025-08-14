using System;

namespace SmartHome.Common.API.Entities;

[Serializable]
public class ShcInfoCache
{
	public string Scope { get; set; }

	public string HardwareType { get; set; }

	public string ConfigVersion { get; set; }

	public override string ToString()
	{
		return $"Scope: {Scope}, ShcVersion: {HardwareType}, ConfigVersion: {ConfigVersion}";
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj))
		{
			return false;
		}
		if (object.ReferenceEquals(this, obj))
		{
			return true;
		}
		if ((object)obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((ShcInfoCache)obj);
	}

	protected bool Equals(ShcInfoCache other)
	{
		if (string.Equals(Scope, other.Scope) && string.Equals(HardwareType, other.HardwareType))
		{
			return string.Equals(ConfigVersion, other.ConfigVersion);
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = ((Scope != null) ? Scope.GetHashCode() : 0);
		num = (num * 397) ^ ((HardwareType != null) ? HardwareType.GetHashCode() : 0);
		return (num * 397) ^ ((ConfigVersion != null) ? ConfigVersion.GetHashCode() : 0);
	}
}
