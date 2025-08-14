namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

internal static class LemonbeatCrc
{
	internal static ushort CalcCrc(byte[] data, int size)
	{
		ushort num = 0;
		if (size == 0)
		{
			size = data.Length;
		}
		for (int i = 0; i < size; i++)
		{
			num ^= (ushort)((data[i] << 8) & 0xFF00);
			for (int j = 0; j < 8; j++)
			{
				num = (((num & 0x8000) == 0) ? ((ushort)(num << 1)) : ((ushort)((num << 1) ^ 0x1021)));
			}
		}
		return num;
	}
}
