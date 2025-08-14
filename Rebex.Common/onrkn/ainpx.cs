namespace onrkn;

internal class ainpx
{
	public static void sraoa(int p0, string p1)
	{
		if (p0 < 64 || p0 > 128)
		{
			throw hifyx.nztrs(p1, p0, "Authentication tag size has to be a number between 64 and 128.");
		}
		if (p0 % 8 != 0 && 0 == 0)
		{
			throw hifyx.nztrs(p1, p0, "Authentication tag size has to be a multiple of 8.");
		}
	}

	public static bool jaqer(int p0, int p1, int p2)
	{
		if (p0 == p2)
		{
			return false;
		}
		int num = p0 + p1;
		int num2 = p2 + p1;
		if (p0 < num2)
		{
			return p2 < num;
		}
		return false;
	}
}
