using System;

namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public class LemonbeatDeviceTypeIdentifier : IEquatable<LemonbeatDeviceTypeIdentifier>
{
	public ulong ManufacturerId { get; private set; }

	public uint ProductId { get; private set; }

	public LemonbeatDeviceTypeIdentifier(ulong manufacturer, uint product)
	{
		ManufacturerId = manufacturer;
		ProductId = product;
	}

	public override bool Equals(object other)
	{
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		LemonbeatDeviceTypeIdentifier other2 = other as LemonbeatDeviceTypeIdentifier;
		return Equals(other2);
	}

	public override int GetHashCode()
	{
		return (ManufacturerId.GetHashCode() * 397) ^ ProductId.GetHashCode();
	}

	public static bool operator ==(LemonbeatDeviceTypeIdentifier identifier1, LemonbeatDeviceTypeIdentifier identifier2)
	{
		if (object.ReferenceEquals(identifier1, null))
		{
			return object.Equals(identifier1, identifier2);
		}
		return identifier1.Equals(identifier2);
	}

	public static bool operator !=(LemonbeatDeviceTypeIdentifier identifier1, LemonbeatDeviceTypeIdentifier identifier2)
	{
		if (identifier1 == null)
		{
			return !object.Equals(identifier1, identifier2);
		}
		return !identifier1.Equals(identifier2);
	}

	public bool Equals(LemonbeatDeviceTypeIdentifier other)
	{
		return other != null && other.ManufacturerId == ManufacturerId && other.ProductId == ProductId;
	}
}
