using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.DeviceManager;

public static class SyncWords
{
	private static readonly byte[][] syncWords = new byte[26][]
	{
		new byte[2] { 165, 150 },
		new byte[2] { 196, 138 },
		new byte[2] { 86, 226 },
		new byte[2] { 87, 26 },
		new byte[2] { 103, 104 },
		new byte[2] { 107, 182 },
		new byte[2] { 165, 169 },
		new byte[2] { 197, 114 },
		new byte[2] { 49, 181 },
		new byte[2] { 81, 81 },
		new byte[2] { 81, 110 },
		new byte[2] { 87, 37 },
		new byte[2] { 92, 72 },
		new byte[2] { 149, 219 },
		new byte[2] { 164, 81 },
		new byte[2] { 164, 110 },
		new byte[2] { 174, 196 },
		new byte[2] { 196, 181 },
		new byte[2] { 54, 57 },
		new byte[2] { 58, 216 },
		new byte[2] { 58, 231 },
		new byte[2] { 106, 113 },
		new byte[2] { 153, 58 },
		new byte[2] { 162, 37 },
		new byte[2] { 169, 119 },
		new byte[2] { 200, 84 }
	};

	public static byte[] GetRandomSyncWord()
	{
		byte[] array = RandomByteGenerator.Instance.GenerateRandomByteSequence(1u);
		int num = array[0] % 26;
		return syncWords[num];
	}
}
