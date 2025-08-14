using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

public class NumberFormat : IEquatable<NumberFormat>
{
	public string Unit { get; set; }

	public double Min { get; set; }

	public double Max { get; set; }

	public double Step { get; set; }

	public NumberFormat()
	{
	}

	public NumberFormat(string unit, double min, double max, double step)
	{
		Unit = unit;
		Min = min;
		Max = max;
		Step = step;
	}

	public override bool Equals(object obj)
	{
		if (obj == null || (object)GetType() != obj.GetType())
		{
			return false;
		}
		NumberFormat other = (NumberFormat)obj;
		return Equals(other);
	}

	public override int GetHashCode()
	{
		return Unit.GetHashCode() ^ Min.GetHashCode() ^ Max.GetHashCode() ^ Step.GetHashCode();
	}

	public bool Equals(NumberFormat other)
	{
		if (other != null && Min == other.Min && Max == other.Max && Unit == other.Unit)
		{
			return Step == other.Step;
		}
		return false;
	}
}
