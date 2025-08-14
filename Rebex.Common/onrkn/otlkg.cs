namespace onrkn;

internal class otlkg
{
	private int horhs;

	private int wusxl;

	public static otlkg dvznf(string p0, int p1)
	{
		int num = ppwlx(p0, p1);
		if (num < 0)
		{
			return null;
		}
		otlkg otlkg2 = new otlkg();
		otlkg2.horhs = num;
		otlkg2.wusxl = ppwlx(p0, num + 1);
		if (otlkg2.wusxl > otlkg2.horhs)
		{
			return otlkg2;
		}
		throw new ufgee("Provided data string is not in correct format, unclosed string quotes found.");
	}

	private static int ppwlx(string p0, int p1)
	{
		if (p1 >= p0.Length)
		{
			return -1;
		}
		int num = p1;
		while (true)
		{
			num = p0.IndexOf('"', num);
			if (num < 0 || num == 0 || false || (num == p0.Length - 1 && !hlvpf(p0, num)) || ((!mxxkf(p0, num) || 1 == 0) && !hlvpf(p0, num)))
			{
				break;
			}
			if (hlvpf(p0, num) && 0 == 0)
			{
				num++;
			}
			else
			{
				if (!mxxkf(p0, num))
				{
					throw new ufgee("This is ");
				}
				num += 2;
			}
			if (num < p0.Length)
			{
				continue;
			}
			return -1;
		}
		return num;
	}

	private static bool hlvpf(string p0, int p1)
	{
		int num = p1 - 1;
		if (num >= 0)
		{
			return p0[num] == '\\';
		}
		return false;
	}

	private static bool mxxkf(string p0, int p1)
	{
		int num = p1 + 1;
		if (num < p0.Length)
		{
			return p0[num] == '"';
		}
		return false;
	}

	public bool vqdrq(int p0)
	{
		if (horhs <= p0)
		{
			return p0 <= wusxl;
		}
		return false;
	}

	public bool guobu(int p0)
	{
		return wusxl < p0;
	}

	public otlkg kmnbc(string p0)
	{
		return dvznf(p0, wusxl + 1);
	}
}
