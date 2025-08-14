namespace SmartHome.SHC.API.Protocols.wMBus;

internal static class ByteArrayExtension
{
	public static bool Compare(this byte[] me, byte[] other)
	{
		if (me == null && other == null)
		{
			return true;
		}
		if (me == null)
		{
			return false;
		}
		if (other == null)
		{
			return false;
		}
		if (other.Length != me.Length)
		{
			return false;
		}
		for (int i = 0; i < me.Length; i++)
		{
			if (me[i] != other[i])
			{
				return false;
			}
		}
		return true;
	}
}
