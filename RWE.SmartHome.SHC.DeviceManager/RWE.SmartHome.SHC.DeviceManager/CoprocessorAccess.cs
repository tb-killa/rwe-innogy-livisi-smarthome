using System;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;

namespace RWE.SmartHome.SHC.DeviceManager;

internal class CoprocessorAccess : ICoprocessorAccess
{
	private const int SendRetries = 3;

	private readonly ICommunicationWrapper communicationWrapper;

	public byte[] Version
	{
		get
		{
			byte[] version = null;
			CommandExecutor.ExecuteCommand(3, () => communicationWrapper.CommandHandler.GetVersion(out version), "GetVersion");
			return version;
		}
	}

	public byte[] Checksum
	{
		get
		{
			byte[] checksum = null;
			CommandExecutor.ExecuteCommand(3, () => communicationWrapper.CommandHandler.GetFlashCRC(out checksum), "GetFlashCRC");
			return checksum;
		}
	}

	public uint SecuritySequenceCounter
	{
		get
		{
			byte[] sequenceCounter = null;
			CommandExecutor.ExecuteCommand(3, () => communicationWrapper.CommandHandler.GetSequenceCount(out sequenceCounter), "GetSequenceCount");
			Array.Reverse(sequenceCounter);
			return BitConverter.ToUInt32(sequenceCounter, 0);
		}
		set
		{
			byte[] sequenceCounter = BitConverter.GetBytes(value);
			Array.Reverse(sequenceCounter);
			CommandExecutor.ExecuteCommand(3, () => communicationWrapper.CommandHandler.SetSequenceCount(sequenceCounter), "SetSequenceCount");
		}
	}

	public CoprocessorAccess(ICommunicationWrapper communicationWrapper)
	{
		this.communicationWrapper = communicationWrapper;
	}

	public void ResetCoprocessor()
	{
		CommandExecutor.ExecuteCommand(3, () => communicationWrapper.CommandHandler.FactoryReset(), "FactoryReset");
	}
}
