namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration.ValueDescriptions;

public class NumberFormat
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
}
