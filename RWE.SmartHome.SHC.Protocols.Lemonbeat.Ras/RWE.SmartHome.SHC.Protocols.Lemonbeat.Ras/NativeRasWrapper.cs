using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal class NativeRasWrapper
{
	private static IRasWrapper instance = new WinCENativeRas();

	internal static int DialParamsSize => instance.DialParamsSize;

	internal static int RasEntrySize => instance.RasEntrySize;

	internal static void OverrideImplementation(IRasWrapper implementation)
	{
		instance = implementation;
	}

	internal static RasError RasDial(byte[] rasDialParams, uint notifierType, IntPtr notifier, ref int connectionHandle)
	{
		return instance.RasDial(rasDialParams, notifierType, notifier, ref connectionHandle);
	}

	internal static RasError RasHangUp(int connectionHandle)
	{
		return instance.RasHangUp(connectionHandle);
	}

	internal static RasError RasGetConnectStatus(int connectionHandle, byte[] connectionStatus)
	{
		return instance.RasGetConnectStatus(connectionHandle, connectionStatus);
	}

	internal static RasError RasEnumEntries(byte[] entries, ref uint bufferByteCount, out uint numberOfEntries)
	{
		return instance.RasEnumEntries(entries, ref bufferByteCount, out numberOfEntries);
	}

	internal static RasError RasGetEntryProperties(string entryName, byte[] entryBuffer, ref uint entrySize, byte[] deviceConfig, ref uint deviceConfigSize)
	{
		return instance.RasGetEntryProperties(entryName, entryBuffer, ref entrySize, deviceConfig, ref deviceConfigSize);
	}

	internal static RasError RasSetEntryProperties(string entryName, byte[] entryBuffer, uint entrySize, byte[] deviceConfig, uint deviceConfigSize)
	{
		return instance.RasSetEntryProperties(entryName, entryBuffer, entrySize, deviceConfig, deviceConfigSize);
	}

	internal static RasError RasGetEntryDialParams(byte[] rasDialParams, ref bool passwordRetrieved)
	{
		return instance.RasGetEntryDialParams(rasDialParams, ref passwordRetrieved);
	}

	internal static RasError RasSetEntryDialParams(byte[] rasDialParams, bool removePassword)
	{
		return instance.RasSetEntryDialParams(rasDialParams, removePassword);
	}

	internal static RasError RasEnumDevices(byte[] rasDevInfoBuffer, ref uint bufferByteCount, ref uint numberOfDevices)
	{
		return instance.RasEnumDevices(rasDevInfoBuffer, ref bufferByteCount, ref numberOfDevices);
	}

	internal static RasError RasGetProjectionInfo(int connectionHandle, int projection, byte[] buffer, ref uint bufferSize)
	{
		return instance.RasGetProjectionInfo(connectionHandle, projection, buffer, ref bufferSize);
	}

	internal static RasError RasEnumConnections(byte[] rasConnectionsBuffer, ref uint bufferByteCount, out uint numberOfConnections)
	{
		return instance.RasEnumConnections(rasConnectionsBuffer, ref bufferByteCount, out numberOfConnections);
	}
}
