using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using SerialAPI;
using SerialAPI.BidCosLayer.CommandFrames;

namespace RWE.SmartHome.SHC.DeviceManager.DeviceHandler;

internal class BidCosConfigurator : IBidCosConfigurator
{
	private readonly ISendScheduler sendScheduler;

	private readonly List<PacketSequence> sequence;

	public BidCosConfigurator(ISendScheduler sendScheduler)
	{
		this.sendScheduler = sendScheduler;
		sequence = new List<PacketSequence>();
	}

	public Guid Configure(byte[] sourceAddress, byte[] destinationAddress, byte channel, byte[] parameters)
	{
		SIPcosHeader sIPcosHeader = new SIPcosHeader();
		sIPcosHeader.Source = sourceAddress;
		sIPcosHeader.Destination = destinationAddress;
		sIPcosHeader.BiDi = true;
		sIPcosHeader.FrameType = SIPcosFrameType.VIRTUAL_BIDCOS_COMMAND;
		SIPcosHeader header = sIPcosHeader;
		VirtualConfigParamsCommand virtualConfigParamsCommand = new VirtualConfigParamsCommand(channel, destinationAddress);
		virtualConfigParamsCommand.Parameters = parameters;
		byte[] message = virtualConfigParamsCommand.ToArray();
		Packet packet = new Packet(header, message);
		PacketSequence packetSequence = new PacketSequence(packet, SequenceType.Configuration);
		sequence.Add(packetSequence);
		return packetSequence.CorrelationId;
	}

	public void Flush()
	{
		sendScheduler.Enqueue(sequence);
		sequence.Clear();
	}
}
