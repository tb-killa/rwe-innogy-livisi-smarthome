using System;
using System.IO;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.FirmwareUpdate;

internal class Crc16
{
	private ushort crc16;

	public Crc16(Stream fileStream)
	{
		try
		{
			crc16 = 0;
			int num = 0;
			int num2 = (int)fileStream.Length;
			fileStream.Position = 0L;
			num = (int)fileStream.Position;
			BinaryReader binaryReader = new BinaryReader(fileStream);
			byte[] array;
			for (; num < num2; num += array.Length)
			{
				array = binaryReader.ReadBytes(16384);
				crc16 = Compute_crc16_checksum(array);
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Unexpected error while calculatiing CRC: " + ex.Message);
		}
	}

	public static implicit operator ushort(Crc16 crc)
	{
		return crc.crc16;
	}

	private ushort Compute_crc16_checksum(byte[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			crc16 = (ushort)(crc16 ^ ((data[i] << 8) & 0xFF00));
			for (int j = 0; j < 8; j++)
			{
				if ((crc16 & 0x8000) != 0)
				{
					crc16 = (ushort)((crc16 << 1) ^ 0x1021);
				}
				else
				{
					crc16 <<= 1;
				}
			}
		}
		return crc16;
	}
}
