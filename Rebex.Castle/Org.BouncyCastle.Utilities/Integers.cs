namespace Org.BouncyCastle.Utilities;

public abstract class Integers
{
	public static int RotateLeft(int i, int distance)
	{
		return (i << distance) ^ (i >>> -distance);
	}

	public static int RotateRight(int i, int distance)
	{
		return (i >>> distance) ^ (i << -distance);
	}
}
