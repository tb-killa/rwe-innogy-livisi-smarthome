using System.IO;

namespace RWE.SmartHome.SHC.BusinessLogic.SoftwareUpdate;

internal class FirmwareImage
{
	public static FirmwareImageState CheckFirmwareImage(string filename)
	{
		try
		{
			byte[] array = new byte[1024];
			using FileStream fileStream = File.Open(filename, FileMode.Open);
			long length = fileStream.Length;
			if (256 != fileStream.Read(array, 0, 256))
			{
				return FirmwareImageState.InvalidImageSize;
			}
			if (15 != fileStream.Read(array, 0, 15))
			{
				return FirmwareImageState.InvalidImageSize;
			}
			if (!CheckNKSignatureHeader(array))
			{
				return FirmwareImageState.InvalidImageData;
			}
			uint num = (uint)((array[10] << 24) | (array[9] << 16) | (array[8] << 8) | array[7]);
			uint num2 = (uint)((array[14] << 24) | (array[13] << 16) | (array[12] << 8) | array[11]);
			int num3 = 271;
			uint num4 = 0u;
			while (num3 < length)
			{
				if (12 != fileStream.Read(array, 0, 12))
				{
					return FirmwareImageState.InvalidImageSize;
				}
				uint num5 = (uint)((array[3] << 24) | (array[2] << 16) | (array[1] << 8) | array[0]);
				uint num6 = (uint)((array[7] << 24) | (array[6] << 16) | (array[5] << 8) | array[4]);
				uint num7 = (uint)((array[11] << 24) | (array[10] << 16) | (array[9] << 8) | array[8]);
				num3 += 12;
				if (num5 == 0 && num7 == 0)
				{
					break;
				}
				fileStream.Seek(num6, SeekOrigin.Current);
				if ((int)num6 < 0)
				{
					return FirmwareImageState.InvalidImageData;
				}
				num3 += (int)num6;
				num4 = num5 + num6;
			}
			if (num3 != length || num4 - num != num2)
			{
				return FirmwareImageState.InvalidImageSize;
			}
			return FirmwareImageState.Complete;
		}
		catch
		{
		}
		return FirmwareImageState.InvalidImageData;
	}

	private static bool CheckNKSignatureHeader(byte[] buffer)
	{
		if (buffer[0] == 66 && buffer[1] == 48 && buffer[2] == 48 && buffer[3] == 48 && buffer[4] == 70 && buffer[5] == 70)
		{
			return buffer[6] == 10;
		}
		return false;
	}
}
