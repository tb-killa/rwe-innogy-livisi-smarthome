namespace SmartHome.SHC.API.Protocols.wMBus;

public class BasicDataHeader : IDataHeader
{
	public byte AccessNumber { get; private set; }

	public byte Status { get; private set; }

	public byte[] Signature { get; private set; }

	public BasicDataHeader(byte accessNumber, byte status, byte[] signature)
	{
		Signature = signature;
		Status = status;
		AccessNumber = accessNumber;
	}

	public bool Equals(BasicDataHeader other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		return other.AccessNumber == AccessNumber && other.Status == Status && Signature.Compare(other.Signature);
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
		if ((object)obj.GetType() != typeof(BasicDataHeader))
		{
			return false;
		}
		return Equals((BasicDataHeader)obj);
	}

	public override int GetHashCode()
	{
		int hashCode = AccessNumber.GetHashCode();
		hashCode = (hashCode * 397) ^ Status.GetHashCode();
		return (hashCode * 397) ^ ((Signature != null) ? Signature.GetHashCode() : 0);
	}
}
