namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal class RasConnectionEntries
{
	public static string[] GetEntryNames()
	{
		uint bufferByteCount = 0u;
		uint numberOfEntries;
		RasError rasError = NativeRasWrapper.RasEnumEntries(null, ref bufferByteCount, out numberOfEntries);
		if (RasError.BufferTooSmall != rasError)
		{
			return new string[0];
		}
		byte[] array = new byte[bufferByteCount];
		int num = (int)((numberOfEntries == 0) ? 48 : (bufferByteCount / numberOfEntries));
		array.SetUInt(0, (uint)num);
		if (NativeRasWrapper.RasEnumEntries(array, ref bufferByteCount, out numberOfEntries) != RasError.Success)
		{
			return new string[0];
		}
		string[] array2 = new string[numberOfEntries];
		for (int i = 0; i < numberOfEntries; i++)
		{
			array2[i] = array.GetString(num * i + 4, (num - 4) / 2);
		}
		return array2;
	}

	public static RasError GetEntryProperties(string entryName, out RasEntry entry, out byte[] deviceConfig)
	{
		entry = null;
		uint entrySize = 0u;
		uint deviceConfigSize = 0u;
		deviceConfig = new byte[0];
		RasError rasError = NativeRasWrapper.RasGetEntryProperties(entryName, null, ref entrySize, null, ref deviceConfigSize);
		if (RasError.BufferTooSmall != rasError && rasError != RasError.Success)
		{
			return rasError;
		}
		byte[] array = new byte[entrySize];
		array.SetUInt(0, entrySize);
		deviceConfig = new byte[deviceConfigSize];
		rasError = NativeRasWrapper.RasGetEntryProperties(entryName, array, ref entrySize, deviceConfig, ref deviceConfigSize);
		if (RasError.BufferTooSmall == rasError)
		{
			deviceConfig = new byte[deviceConfigSize];
			rasError = NativeRasWrapper.RasGetEntryProperties(entryName, array, ref entrySize, deviceConfig, ref deviceConfigSize);
		}
		if (rasError == RasError.Success)
		{
			entry = new RasEntry(array);
		}
		return rasError;
	}

	public static RasError SetEntryProperties(RasEntry entry, string entryName, byte[] deviceConfig, string username, string password)
	{
		uint deviceConfigSize = ((deviceConfig != null) ? ((uint)deviceConfig.Length) : 0u);
		RasError rasError = NativeRasWrapper.RasSetEntryProperties(entryName, entry.Data, (uint)entry.Data.Length, deviceConfig, deviceConfigSize);
		if (rasError != RasError.Success)
		{
			return rasError;
		}
		RasDialParams rasDialParams = new RasDialParams();
		rasDialParams.EntryName = entryName;
		rasDialParams.UserName = username;
		rasDialParams.Password = password;
		RasDialParams rasDialParams2 = rasDialParams;
		return NativeRasWrapper.RasSetEntryDialParams(rasDialParams2.Data, removePassword: false);
	}
}
