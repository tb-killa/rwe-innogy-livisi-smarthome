namespace SmartHome.SHC.API.Protocols.wMBus;

public class DeviceDescription
{
	public string Manufacturer { get; set; }

	public DeviceTypeIdentification DeviceTypeIdentification { get; set; }

	public byte VersionIdentification { get; set; }

	public byte[] Identification { get; set; }

	public bool Equals(DeviceDescription other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return object.Equals(other.Manufacturer, Manufacturer) && object.Equals(other.DeviceTypeIdentification, DeviceTypeIdentification) && other.VersionIdentification == VersionIdentification && other.Identification.Compare(Identification);
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
		if ((object)obj.GetType() != typeof(DeviceDescription))
		{
			return false;
		}
		return Equals((DeviceDescription)obj);
	}

	public override int GetHashCode()
	{
		int num = ((Manufacturer != null) ? Manufacturer.GetHashCode() : 0);
		num = (num * 397) ^ DeviceTypeIdentification.GetHashCode();
		num = (num * 397) ^ VersionIdentification.GetHashCode();
		return (num * 397) ^ ((Identification != null) ? Identification.GetHashCode() : 0);
	}
}
