using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager.DeviceHandler;

internal class SipcosConfigurator : ISipcosConfigurator
{
	private const int MaxSizeForIndexConfiguration = 30;

	private readonly List<PacketSequence> sequences = new List<PacketSequence>();

	private ICommunicationWrapper communicationWrapper;

	public SipcosConfigurator(ICommunicationWrapper communicationWrapper)
	{
		this.communicationWrapper = communicationWrapper;
	}

	public Guid CreateLink(byte[] targetDeviceAddress, byte targetChannel, byte[] partnerDeviceAddress, byte partnerChannel, byte operationMode)
	{
		SIPCOSMessage sipCosMessage = communicationWrapper.ConfigurationHandler.GenerateSubscribe(new SIPcosHeader
		{
			Destination = targetDeviceAddress
		}, targetChannel, partnerDeviceAddress, operationMode, new byte[2] { partnerChannel, 0 });
		return QueueSequence(sipCosMessage);
	}

	public Guid RemoveLink(byte[] targetDeviceAddress, byte targetChannel, byte[] partnerDeviceAddress, byte partnerChannel)
	{
		SIPCOSMessage sipCosMessage = communicationWrapper.ConfigurationHandler.GenerateUnsubscribe(new SIPcosHeader
		{
			Destination = targetDeviceAddress
		}, targetChannel, partnerDeviceAddress, new byte[2] { partnerChannel, 0 });
		return QueueSequence(sipCosMessage);
	}

	public Guid ConfigureLink(byte[] targetDeviceAddress, byte targetChannel, byte[] partnerDeviceAddress, byte partnerChannel, byte parameterListNo, byte[] parameters, byte operationMode, bool withCreate)
	{
		SIPcosHeader sIPcosHeader = new SIPcosHeader();
		sIPcosHeader.Destination = targetDeviceAddress;
		SIPcosHeader header = sIPcosHeader;
		PacketSequence packetSequence = new PacketSequence(SequenceType.Configuration);
		if (withCreate)
		{
			packetSequence.Add(communicationWrapper.ConfigurationHandler.GenerateSubscribe(header, targetChannel, partnerDeviceAddress, operationMode, new byte[2] { partnerChannel, 0 }));
		}
		packetSequence.Add(communicationWrapper.ConfigurationHandler.GenerateStartConfiguration(header, targetChannel, partnerDeviceAddress, operationMode, partnerChannel, parameterListNo));
		int i = 0;
		byte[] array = new byte[30];
		for (; parameters.Length - i > 30; i += 30)
		{
			parameters.CopySubArray(array, i, 30);
			packetSequence.Add(communicationWrapper.ConfigurationHandler.GenerateIndexConfiguration(header, targetChannel, array));
		}
		byte[] array2 = new byte[parameters.Length - i];
		parameters.CopySubArray(array2, i, parameters.Length - i);
		packetSequence.Add(communicationWrapper.ConfigurationHandler.GenerateIndexConfiguration(header, targetChannel, array2));
		packetSequence.Add(communicationWrapper.ConfigurationHandler.GenerateEndConfiguration(header, targetChannel));
		sequences.Add(packetSequence);
		return packetSequence.CorrelationId;
	}

	public void FlushToSendQueue()
	{
		communicationWrapper.SendScheduler.Enqueue(sequences);
		sequences.Clear();
	}

	private Guid QueueSequence(SIPCOSMessage sipCosMessage)
	{
		PacketSequence packetSequence = new PacketSequence(new Packet(sipCosMessage), SequenceType.Configuration);
		sequences.Add(packetSequence);
		return packetSequence.CorrelationId;
	}
}
