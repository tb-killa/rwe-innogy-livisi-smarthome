using System;

public abstract class DimmerActionHandler
{
	public byte Channel { get; private set; }

	public DimmerActionHandler(byte channel)
	{
		Channel = channel;
	}

	internal static byte FromPercent(int minimum, int maximum, int dimLevel)
	{
		if (dimLevel <= 0)
		{
			return 0;
		}
		int num = Math.Min(100, dimLevel);
		return (byte)Math.Round(2.0 * ((double)(num - 1) * (1.0 / 99.0) * (double)(maximum - minimum) + (double)minimum));
	}
}
