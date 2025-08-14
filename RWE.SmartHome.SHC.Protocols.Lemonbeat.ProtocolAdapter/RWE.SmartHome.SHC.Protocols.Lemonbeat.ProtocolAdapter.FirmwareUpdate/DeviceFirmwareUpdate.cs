using System;
using System.IO;
using System.Linq;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.FirmwareUpdate;

public class DeviceFirmwareUpdate
{
	private Stream updateFileStream;

	private BinaryReader updateFileReader;

	private int chunkSize = 256;

	public int NumberOfChunks { get; private set; }

	public byte[] Checksum { get; set; }

	public uint FirmwareID { get; set; }

	public uint Size { get; set; }

	public DeviceFirmwareUpdate(Stream updateStream, int updateChunkSize)
	{
		chunkSize = updateChunkSize;
		updateFileStream = updateStream;
		updateFileStream.Position = 0L;
		InitializeUpdate();
	}

	public byte[] GetGetChunkData(uint chunkIndex)
	{
		try
		{
			if (updateFileReader == null)
			{
				updateFileReader = new BinaryReader(updateFileStream);
			}
			if (chunkIndex != updateFileReader.BaseStream.Position / chunkSize)
			{
				updateFileReader.BaseStream.Position = GetChunkOffset(chunkIndex);
			}
			return updateFileReader.ReadBytes(chunkSize);
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Error occured while reading update file: " + ex.Message);
		}
		return null;
	}

	public uint GetChunkIndexByOffset(int offset)
	{
		try
		{
			return (uint)(offset / chunkSize);
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Unexpected error while calculating chunk index: " + ex.Message);
		}
		return 0u;
	}

	private void InitializeUpdate()
	{
		try
		{
			Checksum = GetCRC();
			FirmwareID = 1u;
			Size = (uint)updateFileStream.Length;
			NumberOfChunks = ((int)Size + chunkSize) / chunkSize;
			updateFileStream.Position = 0L;
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, "Unexpected error while initializing update: " + ex.Message);
		}
	}

	private byte[] GetCRC()
	{
		return BitConverter.GetBytes(new Crc16(updateFileStream)).Reverse().ToArray();
	}

	private uint GetChunkOffset(uint chunkIndex)
	{
		if (chunkIndex > NumberOfChunks)
		{
			throw new ArgumentOutOfRangeException("Invalid chunk index [" + chunkIndex + "]. Total size [" + NumberOfChunks + "]");
		}
		return chunkIndex * (uint)chunkSize;
	}
}
