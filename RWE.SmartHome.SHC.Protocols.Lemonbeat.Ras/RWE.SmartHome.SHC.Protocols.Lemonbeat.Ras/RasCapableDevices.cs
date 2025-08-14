namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal class RasCapableDevices
{
	private const int StructSize = 296;

	public static RasError GetDeviceList(out RasDevice[] devices)
	{
		devices = new RasDevice[0];
		uint numberOfDevices = 0u;
		uint bufferByteCount = 0u;
		RasError rasError = NativeRasWrapper.RasEnumDevices(null, ref bufferByteCount, ref numberOfDevices);
		if (RasError.BufferTooSmall != rasError && rasError != RasError.Success)
		{
			return rasError;
		}
		byte[] array = new byte[bufferByteCount];
		array.SetUInt(0, 296u);
		rasError = NativeRasWrapper.RasEnumDevices(array, ref bufferByteCount, ref numberOfDevices);
		if (rasError == RasError.Success)
		{
			devices = new RasDevice[numberOfDevices];
			for (int i = 0; i < numberOfDevices; i++)
			{
				devices[i] = new RasDevice(array, 296 * i);
			}
		}
		return rasError;
	}
}
