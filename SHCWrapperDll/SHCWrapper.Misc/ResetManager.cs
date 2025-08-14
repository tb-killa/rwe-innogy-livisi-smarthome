namespace SHCWrapper.Misc;

public static class ResetManager
{
	public static void Reset()
	{
		PrivateWrapper.Reset();
	}

	public static bool IsFactoryReset()
	{
		return PrivateWrapper.IsFactoryReset();
	}
}
