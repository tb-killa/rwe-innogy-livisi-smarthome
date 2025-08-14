using System;
using System.Runtime.InteropServices;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal class WinCENativeRas : IRasWrapper
{
	int IRasWrapper.DialParamsSize => 1464;

	int IRasWrapper.RasEntrySize => 3472;

	[DllImport("coredll")]
	internal static extern RasError RasDial(IntPtr dialExtensions, string phoneBook, byte[] rasDialParams, uint notifierType, IntPtr notifier, ref int connectionHandle);

	[DllImport("coredll")]
	internal static extern RasError RasHangUp(int connectionHandle);

	[DllImport("coredll")]
	internal static extern RasError RasGetConnectStatus(int connectionHandle, byte[] connectionStatus);

	[DllImport("coredll")]
	internal static extern RasError RasEnumEntries(string reserved, string phoneBook, byte[] entries, ref uint bufferByteCount, out uint numberOfEntries);

	[DllImport("coredll")]
	internal static extern RasError RasGetEntryProperties(string phoneBook, string entryName, byte[] entryBuffer, ref uint entrySize, byte[] deviceConfig, ref uint deviceConfigSize);

	[DllImport("coredll")]
	internal static extern RasError RasSetEntryProperties(string phoneBook, string entryName, byte[] entryBuffer, uint entrySize, byte[] deviceConfig, uint deviceConfigSize);

	[DllImport("coredll")]
	internal static extern RasError RasGetEntryDialParams(string phoneBook, byte[] rasDialParams, ref bool removePassword);

	[DllImport("coredll")]
	internal static extern RasError RasSetEntryDialParams(string phoneBook, byte[] rasDialParams, bool removePassword);

	[DllImport("coredll")]
	internal static extern RasError RasEnumDevices(byte[] rasDevInfoBuffer, ref uint bufferByteCount, ref uint numberOfDevices);

	[DllImport("coredll")]
	internal static extern RasError RasGetProjectionInfo(int connectionHandle, int projection, byte[] buffer, ref uint bufferSize);

	[DllImport("coredll")]
	internal static extern RasError RasEnumConnections(byte[] rasConnectionsBuffer, ref uint bufferByteCount, out uint numberOfConnections);

	RasError IRasWrapper.RasDial(byte[] rasDialParams, uint notifierType, IntPtr notifier, ref int connectionHandle)
	{
		return RasDial(IntPtr.Zero, null, rasDialParams, notifierType, notifier, ref connectionHandle);
	}

	RasError IRasWrapper.RasHangUp(int connectionHandle)
	{
		return RasHangUp(connectionHandle);
	}

	RasError IRasWrapper.RasGetConnectStatus(int connectionHandle, byte[] connectionStatus)
	{
		return RasGetConnectStatus(connectionHandle, connectionStatus);
	}

	RasError IRasWrapper.RasEnumEntries(byte[] entries, ref uint bufferByteCount, out uint numberOfEntries)
	{
		return RasEnumEntries(null, null, entries, ref bufferByteCount, out numberOfEntries);
	}

	RasError IRasWrapper.RasGetEntryProperties(string entryName, byte[] entryBuffer, ref uint entrySize, byte[] deviceConfig, ref uint deviceConfigSize)
	{
		return RasGetEntryProperties(null, entryName, entryBuffer, ref entrySize, deviceConfig, ref deviceConfigSize);
	}

	RasError IRasWrapper.RasSetEntryProperties(string entryName, byte[] entryBuffer, uint entrySize, byte[] deviceConfig, uint deviceConfigSize)
	{
		return RasSetEntryProperties(null, entryName, entryBuffer, entrySize, deviceConfig, deviceConfigSize);
	}

	RasError IRasWrapper.RasGetEntryDialParams(byte[] rasDialParams, ref bool removePassword)
	{
		return RasGetEntryDialParams(null, rasDialParams, ref removePassword);
	}

	RasError IRasWrapper.RasSetEntryDialParams(byte[] rasDialParams, bool removePassword)
	{
		return RasSetEntryDialParams(null, rasDialParams, removePassword);
	}

	RasError IRasWrapper.RasEnumDevices(byte[] rasDevInfoBuffer, ref uint bufferByteCount, ref uint numberOfDevices)
	{
		return RasEnumDevices(rasDevInfoBuffer, ref bufferByteCount, ref numberOfDevices);
	}

	RasError IRasWrapper.RasEnumConnections(byte[] rasConnectionsBuffer, ref uint bufferByteCount, out uint numberOfConnections)
	{
		return RasEnumConnections(rasConnectionsBuffer, ref bufferByteCount, out numberOfConnections);
	}

	RasError IRasWrapper.RasGetProjectionInfo(int connectionHandle, int projection, byte[] buffer, ref uint bufferSize)
	{
		return RasGetProjectionInfo(connectionHandle, projection, buffer, ref bufferSize);
	}
}
