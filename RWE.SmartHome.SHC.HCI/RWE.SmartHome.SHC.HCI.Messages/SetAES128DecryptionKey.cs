using System;

namespace RWE.SmartHome.SHC.HCI.Messages;

public class SetAES128DecryptionKey
{
	public byte TableIndex { get; set; }

	public byte[] ManufacturerID { get; set; }

	public byte[] DeviceId { get; set; }

	public byte Version { get; set; }

	public byte DeviceType { get; set; }

	public byte[] EncryptionKey { get; set; }

	public SetAES128DecryptionKey()
	{
	}

	public SetAES128DecryptionKey(byte[] buffer)
	{
		if (buffer.Length != 25)
		{
			throw new ArgumentException("The buffer size must be 25 bytes.");
		}
		TableIndex = buffer[0];
		ManufacturerID = new byte[2];
		Array.Copy(buffer, 1, ManufacturerID, 0, 2);
		DeviceId = new byte[4];
		Array.Copy(buffer, 3, DeviceId, 0, 4);
		Version = buffer[7];
		DeviceType = buffer[8];
		EncryptionKey = new byte[16];
		Array.Copy(buffer, 9, EncryptionKey, 0, 16);
	}

	public byte[] GetBytes()
	{
		byte[] array = new byte[25];
		array[0] = TableIndex;
		Array.Copy(ManufacturerID, 0, array, 1, 2);
		Array.Copy(DeviceId, 0, array, 3, 4);
		array[7] = Version;
		array[8] = DeviceType;
		Array.Copy(EncryptionKey, 0, array, 9, 16);
		return array;
	}
}
