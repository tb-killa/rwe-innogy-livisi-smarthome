using System;
using System.Runtime.InteropServices;

namespace onrkn;

[StructLayout(LayoutKind.Sequential, Size = 1)]
internal struct gfqoc : IEquatable<gfqoc>, IComparable<gfqoc>
{
	private const string ahbqw = "Unit";

	public static readonly gfqoc jusmc = default(gfqoc);

	public override int GetHashCode()
	{
		return 0;
	}

	public override bool Equals(object obj)
	{
		return obj is gfqoc;
	}

	public bool Equals(gfqoc other)
	{
		return true;
	}

	public int CompareTo(gfqoc other)
	{
		return 0;
	}

	public override string ToString()
	{
		return "Unit";
	}

	public static bool operator ==(gfqoc lhs, gfqoc rhs)
	{
		return true;
	}

	public static bool operator !=(gfqoc lhs, gfqoc rhs)
	{
		return false;
	}

	public static bool operator >(gfqoc lhs, gfqoc rhs)
	{
		return false;
	}

	public static bool operator >=(gfqoc lhs, gfqoc rhs)
	{
		return true;
	}

	public static bool operator <(gfqoc lhs, gfqoc rhs)
	{
		return false;
	}

	public static bool operator <=(gfqoc lhs, gfqoc rhs)
	{
		return true;
	}
}
