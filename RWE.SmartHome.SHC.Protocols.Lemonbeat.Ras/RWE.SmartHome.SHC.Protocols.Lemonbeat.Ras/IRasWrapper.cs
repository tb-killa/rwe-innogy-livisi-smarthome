using System;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal interface IRasWrapper
{
	int DialParamsSize { get; }

	int RasEntrySize { get; }

	RasError RasDial(byte[] rasDialParams, uint notifierType, IntPtr notifier, ref int connectionHandle);

	RasError RasHangUp(int connectionHandle);

	RasError RasGetConnectStatus(int connectionHandle, byte[] connectionStatus);

	RasError RasEnumEntries(byte[] entries, ref uint bufferByteCount, out uint numberOfEntries);

	RasError RasGetEntryProperties(string entryName, byte[] entryBuffer, ref uint entrySize, byte[] deviceConfig, ref uint deviceConfigSize);

	RasError RasSetEntryProperties(string entryName, byte[] entryBuffer, uint entrySize, byte[] deviceConfig, uint deviceConfigSize);

	RasError RasGetEntryDialParams(byte[] rasDialParams, ref bool passwordRetrieved);

	RasError RasSetEntryDialParams(byte[] rasDialParams, bool removePassword);

	RasError RasEnumDevices(byte[] rasDevInfoBuffer, ref uint bufferByteCount, ref uint numberOfDevices);

	RasError RasGetProjectionInfo(int connectionHandle, int projection, byte[] buffer, ref uint bufferSize);

	RasError RasEnumConnections(byte[] rasConnectionsBuffer, ref uint bufferByteCount, out uint numberOfConnections);
}
