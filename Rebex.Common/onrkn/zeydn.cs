using System.Collections.Generic;

namespace onrkn;

internal class zeydn
{
	public static byte[] oabhp(string p0)
	{
		if (p0 == null || false || p0.Length == 0 || 1 == 0)
		{
			return new byte[0];
		}
		int num = 0;
		int num2 = hhcen(p0, ',', 0);
		suzxs suzxs2 = new suzxs();
		Stack<hjdlb> stack = new Stack<hjdlb>();
		while (num2 >= num)
		{
			stack.Push(apyil(p0.Substring(num, num2 - num)));
			num = zejwk(p0, num2);
			num2 = hhcen(p0, ',', num);
		}
		stack.Push(apyil(p0.Substring(num)));
		while (stack.Count > 0)
		{
			suzxs2.Add(stack.Pop());
		}
		return fxakl.kncuz(suzxs2);
	}

	private static hjdlb apyil(string p0)
	{
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ufgee("Provided data string is not in correct format, contains empty 'Relative Distinguished Name' part.");
		}
		int num = 0;
		int num2 = hhcen(p0, '+', 0);
		hjdlb hjdlb2 = new hjdlb();
		while (num2 >= num)
		{
			hjdlb2.Add(uzrtf(p0.Substring(num, num2 - num)));
			num = zejwk(p0, num2);
			num2 = hhcen(p0, '+', num);
		}
		hjdlb2.Add(uzrtf(p0.Substring(num)));
		return hjdlb2;
	}

	private static wusby uzrtf(string p0)
	{
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ufgee("Provided data string is not in correct format, contains empty 'Attribute Type and Value' part.");
		}
		int num = hhcen(p0, '=', 0);
		if (num < 0)
		{
			throw new ufgee("Provided data string is not in correct format, missing non-escaped '=' in 'Attribute Type and Value' part.");
		}
		int num2 = eruev(p0);
		if (num2 < 0)
		{
			throw new ufgee(brgjd.edcru("Provided data string is not in correct format, contains empty 'Value' after oid {0}.", p0.Substring(0, num)));
		}
		string text = p0.Substring(0, num);
		string p1 = p0.Substring(num + 1, num2 - num);
		ubfew ubfew2 = ((text.IndexOf('.') < 0) ? ubfew.yzylr(text) : ubfew.umnjs(text));
		return new wusby(ubfew2.ahwky, ubfew2.qbevv(p1));
	}

	private static int zejwk(string p0, int p1)
	{
		int i;
		for (i = p1 + 1; i < p0.Length && p0[i] == ' '; i++)
		{
		}
		return i;
	}

	private static int eruev(string p0)
	{
		if (p0.Trim().Length == 0 || 1 == 0)
		{
			return -1;
		}
		int num = p0.Length - 1;
		while (num >= 0 && p0[num] == ' ')
		{
			num--;
		}
		if (p0[num] == '\\')
		{
			num++;
		}
		return num;
	}

	private static int hhcen(string p0, char p1, int p2)
	{
		if (p2 >= p0.Length)
		{
			return -1;
		}
		int startIndex = p2;
		otlkg otlkg2 = otlkg.dvznf(p0, p2);
		while (true)
		{
			startIndex = p0.IndexOf(p1, startIndex);
			if (startIndex < 0 || startIndex == 0)
			{
				break;
			}
			if (otlkg2 != null && 0 == 0 && otlkg2.guobu(startIndex) && 0 == 0)
			{
				otlkg2 = otlkg2.kmnbc(p0);
			}
			if (!mbzjf(startIndex, p0, otlkg2))
			{
				break;
			}
			startIndex++;
		}
		return startIndex;
	}

	private static bool mbzjf(int p0, string p1, otlkg p2)
	{
		bool flag = p1[p0 - 1] == '\\';
		if (p2 != null && 0 == 0)
		{
			flag |= p2.vqdrq(p0);
		}
		return flag;
	}
}
