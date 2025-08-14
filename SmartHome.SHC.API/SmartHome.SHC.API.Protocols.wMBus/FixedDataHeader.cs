namespace SmartHome.SHC.API.Protocols.wMBus;

public class FixedDataHeader : IDataHeader
{
	public byte[] ManufacturerId { get; private set; }

	public byte Version { get; private set; }

	public DeviceTypeIdentification Medium { get; private set; }

	public byte AccessNumber { get; private set; }

	public byte Status { get; private set; }

	public byte[] Signature { get; private set; }

	public int IdentNr { get; private set; }

	public FixedDataHeader(byte[] manufacturerId, byte version, DeviceTypeIdentification medium, byte accessNumber, byte status, byte[] signature, int identNr)
	{
		ManufacturerId = manufacturerId;
		Version = version;
		Medium = medium;
		AccessNumber = accessNumber;
		Status = status;
		Signature = signature;
		IdentNr = identNr;
	}

	public bool Equals(FixedDataHeader other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return ManufacturerId.Compare(other.ManufacturerId) && other.Version == Version && object.Equals(other.Medium, Medium) && other.AccessNumber == AccessNumber && other.Status == Status && Signature.Compare(other.Signature) && other.IdentNr == IdentNr;
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
		if ((object)obj.GetType() != typeof(FixedDataHeader))
		{
			return false;
		}
		return Equals((FixedDataHeader)obj);
	}

	public override int GetHashCode()
	{
		int num = ((ManufacturerId != null) ? ManufacturerId.GetHashCode() : 0);
		num = (num * 397) ^ Version.GetHashCode();
		num = (num * 397) ^ Medium.GetHashCode();
		num = (num * 397) ^ AccessNumber.GetHashCode();
		num = (num * 397) ^ Status.GetHashCode();
		num = (num * 397) ^ ((Signature != null) ? Signature.GetHashCode() : 0);
		return (num * 397) ^ IdentNr;
	}
}
