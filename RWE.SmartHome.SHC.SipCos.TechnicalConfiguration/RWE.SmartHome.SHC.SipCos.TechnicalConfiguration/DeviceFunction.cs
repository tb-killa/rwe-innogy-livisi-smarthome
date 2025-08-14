using System;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration;

public class DeviceFunction : IEquatable<DeviceFunction>
{
	public Guid LogicalDeviceId { get; private set; }

	public int Function { get; set; }

	public DeviceFunction(Guid logicalDeviceId, int function)
	{
		LogicalDeviceId = logicalDeviceId;
		Function = function;
	}

	public bool Equals(DeviceFunction other)
	{
		if (other.LogicalDeviceId.Equals(LogicalDeviceId))
		{
			return other.Function == Function;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (LogicalDeviceId.GetHashCode() * 397) ^ Function;
	}

	public static bool operator ==(DeviceFunction left, DeviceFunction right)
	{
		if (left != null)
		{
			return left.Equals(right);
		}
		return false;
	}

	public static bool operator !=(DeviceFunction left, DeviceFunction right)
	{
		if (left != null)
		{
			return !left.Equals(right);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj))
		{
			return false;
		}
		if ((object)obj.GetType() != typeof(DeviceFunction))
		{
			return false;
		}
		return Equals((DeviceFunction)obj);
	}
}
