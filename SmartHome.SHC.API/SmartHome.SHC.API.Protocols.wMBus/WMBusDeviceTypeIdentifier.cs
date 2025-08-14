using System;

namespace SmartHome.SHC.API.Protocols.wMBus;

public class WMBusDeviceTypeIdentifier : IEquatable<WMBusDeviceTypeIdentifier>
{
	public string Manufacturer { get; private set; }

	public DeviceTypeIdentification Medium { get; private set; }

	public int? Version { get; set; }

	public WMBusDeviceTypeIdentifier(string manufacturer, DeviceTypeIdentification medium)
	{
		Manufacturer = manufacturer;
		Medium = medium;
		Version = null;
	}

	public WMBusDeviceTypeIdentifier(string manufacturer, DeviceTypeIdentification medium, int version)
	{
		Manufacturer = manufacturer;
		Medium = medium;
		Version = version;
	}

	public bool Equals(WMBusDeviceTypeIdentifier other)
	{
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (other.Manufacturer != Manufacturer || other.Medium != Medium)
		{
			return false;
		}
		return Nullable.Compare(other.Version, Version) == 0;
	}

	public override int GetHashCode()
	{
		int num = (((Manufacturer != null) ? Manufacturer.GetHashCode() : 0) * 397) ^ Medium.GetHashCode();
		if (Version.HasValue)
		{
			num = (num * 397) ^ Version.Value;
		}
		return num;
	}
}
