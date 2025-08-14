using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class HexBinaryFormat : IEquatable<HexBinaryFormat>
{
	public uint MaximumLength { get; set; }

	public HexBinaryFormat()
	{
	}

	public HexBinaryFormat(uint maximumLength)
	{
		MaximumLength = maximumLength;
	}

	public override bool Equals(object obj)
	{
		if (obj == null || (object)GetType() != obj.GetType())
		{
			return false;
		}
		HexBinaryFormat other = (HexBinaryFormat)obj;
		return Equals(other);
	}

	public override int GetHashCode()
	{
		return MaximumLength.GetHashCode();
	}

	public bool Equals(HexBinaryFormat other)
	{
		if (other != null)
		{
			return MaximumLength == other.MaximumLength;
		}
		return false;
	}
}
