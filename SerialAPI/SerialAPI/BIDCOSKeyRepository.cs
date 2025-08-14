using System.Collections.Generic;
using System.Linq;

namespace SerialAPI;

public static class BIDCOSKeyRepository
{
	private class KeyDescription
	{
		public byte[] Address;

		public byte[] Key;
	}

	private static List<KeyDescription> keyDescriptions = new List<KeyDescription>();

	public static void AddKey(byte[] address, byte[] key)
	{
		if (address == null || key == null)
		{
			return;
		}
		lock (keyDescriptions)
		{
			keyDescriptions.RemoveAll((KeyDescription kd) => address.SequenceEqual(kd.Address));
			keyDescriptions.Add(new KeyDescription
			{
				Address = address.ToArray(),
				Key = key.ToArray()
			});
		}
	}

	public static byte[] GetKey(byte[] address)
	{
		if (address == null)
		{
			return new byte[0];
		}
		KeyDescription keyDescription;
		lock (keyDescriptions)
		{
			keyDescription = keyDescriptions.FirstOrDefault((KeyDescription k) => k.Address.SequenceEqual(address));
		}
		if (keyDescription == null)
		{
			return new byte[0];
		}
		return keyDescription.Key.ToArray();
	}
}
