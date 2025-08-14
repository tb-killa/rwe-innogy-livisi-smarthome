using System;

namespace onrkn;

internal class rmbax
{
	private const long ynmae = 60000L;

	private const long fqpax = 4294967295L;

	private static readonly rmbax bwpok = new rmbax();

	private readonly long skdcs;

	private readonly long bdnma;

	private long goquy;

	private readonly object lqzdr = new object();

	public static DateTime xhfqg => aqwjh(yvpof);

	public static long yvpof => bwpok.onslx;

	private static long qrohe => DateTime.Now.Ticks / 10000;

	public long onslx
	{
		get
		{
			lock (lqzdr)
			{
				long num = qrohe;
				long num2;
				while (true)
				{
					num2 = goquy + bjkuo;
					long num3 = num - num2;
					if (num3 < -skdcs)
					{
						throw new InvalidOperationException();
					}
					if (num3 < skdcs)
					{
						break;
					}
					goquy += bdnma;
				}
				return num2;
			}
		}
	}

	private long bjkuo => (uint)Environment.TickCount % bdnma;

	private rmbax()
		: this(60000L, 4294967295L)
	{
	}

	public rmbax(long epochDeltaMs, long epochLengthMs)
	{
		skdcs = epochDeltaMs;
		bdnma = epochLengthMs;
		goquy = qrohe - bjkuo;
	}

	public static DateTime aqwjh(long p0)
	{
		if (p0 == long.MaxValue)
		{
			return DateTime.MaxValue;
		}
		return new DateTime(p0 * 10000);
	}
}
