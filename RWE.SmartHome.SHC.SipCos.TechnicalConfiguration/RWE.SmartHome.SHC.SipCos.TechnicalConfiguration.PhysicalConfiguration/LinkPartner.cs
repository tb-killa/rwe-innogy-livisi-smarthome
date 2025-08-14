using System;
using System.IO;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.PhysicalConfiguration;

public class LinkPartner : IComparable
{
	private int hash;

	public static readonly LinkPartner Empty = new LinkPartner(Guid.Empty, new byte[3], 0);

	public Guid DeviceId { get; private set; }

	public byte[] Address { get; private set; }

	public byte Channel { get; private set; }

	public LinkPartner(Guid deviceId, byte[] address, byte channel)
	{
		DeviceId = deviceId;
		Address = address;
		Channel = channel;
		CalculateHash();
	}

	private void CalculateHash()
	{
		int num = (Channel << 24) + (Address[0] << 16) + (Address[1] << 8) + Address[2];
		hash = DeviceId.GetHashCode();
		hash = (hash * 397) ^ num.GetHashCode();
	}

	public static bool operator ==(LinkPartner left, LinkPartner right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(LinkPartner left, LinkPartner right)
	{
		return !object.Equals(left, right);
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
		if ((object)obj.GetType() != typeof(LinkPartner))
		{
			return false;
		}
		LinkPartner linkPartner = (LinkPartner)obj;
		if (linkPartner.DeviceId == DeviceId && linkPartner.Address[0] == Address[0] && linkPartner.Address[1] == Address[1] && linkPartner.Address[2] == Address[2])
		{
			return linkPartner.Channel == Channel;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return hash;
	}

	public int CompareTo(object other)
	{
		return hash.CompareTo(other.GetHashCode());
	}

	internal LinkPartner(BinaryReader reader)
	{
		DeviceId = new Guid(reader.ReadBytes(16));
		Address = reader.ReadBytes(3);
		Channel = reader.ReadByte();
		CalculateHash();
	}

	internal void Save(BinaryWriter writer)
	{
		writer.Write(DeviceId.ToByteArray());
		writer.Write(Address);
		writer.Write(Channel);
	}

	internal LinkPartner Clone()
	{
		return new LinkPartner(DeviceId, Address, Channel);
	}
}
