using System;
using System.IO;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.SwitchDelegate;

public class LinkEndpoint : IEquatable<LinkEndpoint>, IComparable
{
	public byte[] Address { get; set; }

	public byte Channel { get; set; }

	public LinkEndpoint(byte[] address, byte channel)
	{
		Address = address;
		Channel = channel;
	}

	public bool Equals(LinkEndpoint other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		if (other.Address.Compare(Address))
		{
			return other.Channel == Channel;
		}
		return false;
	}

	public override bool Equals(object other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		if ((object)other.GetType() != typeof(LinkEndpoint))
		{
			return false;
		}
		return Equals((LinkEndpoint)other);
	}

	public override int GetHashCode()
	{
		return ((Channel << 24) + (Address[0] << 16) + (Address[1] << 8) + Address[2]).GetHashCode();
	}

	public int CompareTo(object other)
	{
		return GetHashCode().CompareTo(other.GetHashCode());
	}

	public static bool operator ==(LinkEndpoint left, LinkEndpoint right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(LinkEndpoint left, LinkEndpoint right)
	{
		return !object.Equals(left, right);
	}

	public LinkEndpoint Clone()
	{
		return new LinkEndpoint(Address, Channel);
	}

	public void SaveToStream(Stream stream)
	{
		stream.Write(Address, 0, 3);
		stream.WriteByte(Channel);
	}

	public static LinkEndpoint ReadFromStream(Stream stream)
	{
		byte[] array = new byte[3];
		stream.Read(array, 0, 3);
		byte channel = (byte)stream.ReadByte();
		return new LinkEndpoint(array, channel);
	}
}
