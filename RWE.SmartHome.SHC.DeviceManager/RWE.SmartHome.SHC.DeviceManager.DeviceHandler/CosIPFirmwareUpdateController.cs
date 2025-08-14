using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;
using RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using SerialAPI;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManager.DeviceHandler;

internal class CosIPFirmwareUpdateController : ICosIPFirmwareUpdateController
{
	private const string LoggingSource = "CosIPFirmwareUpdateController";

	private readonly CommunicationWrapper communicationWrapper;

	private readonly SIPcosFirmwareUpdateCommandHandler sipCosFirmwareUpdateCommandHandler;

	private readonly SIPcosConfigurationCommandHandlerExt sipCosConfigHandler;

	private readonly IEventManager eventManager;

	private readonly SortedList<Guid, Guid> sequenceLookup;

	private readonly uint[] devicesWithNoDutyCycle = new uint[4] { 4u, 8u, 3u, 13u };

	internal CosIPFirmwareUpdateController(CommunicationWrapper communicationWrapper, IEventManager eventManager)
	{
		this.eventManager = eventManager;
		this.communicationWrapper = communicationWrapper;
		sequenceLookup = new SortedList<Guid, Guid>();
		sipCosConfigHandler = communicationWrapper.ConfigurationHandler;
		sipCosFirmwareUpdateCommandHandler = communicationWrapper.FirmwareUpdateCommandHandler;
		sipCosFirmwareUpdateCommandHandler.ReceiveFirmwareAnswer += ReceiveFirmwareAnswer;
		communicationWrapper.SendScheduler.SequenceFinished += SequenceFinished;
	}

	private void SequenceFinished(object sender, SequenceFinishedEventArgs e)
	{
		if (sequenceLookup.ContainsKey(e.CorrelationId))
		{
			Guid deviceId = sequenceLookup[e.CorrelationId];
			sequenceLookup.Remove(e.CorrelationId);
			CosIPFirmwareUpdateStatusCode statusCode = ((e.State != SequenceState.Success) ? CosIPFirmwareUpdateStatusCode.Nak : CosIPFirmwareUpdateStatusCode.Ack);
			SipCosDeviceUpdateStatusEventArgs payload = new SipCosDeviceUpdateStatusEventArgs(deviceId, statusCode, 0);
			eventManager.GetEvent<SipCosDeviceUpdateStatusEvent>().Publish(payload);
		}
	}

	private void ReceiveFirmwareAnswer(FirmwareStatusFrame status)
	{
		communicationWrapper.SendScheduler.AcknowledgePacket(status.Header.Source, status.Header.SequenceNumber, SIPcosAnswerFrameStatus.ACK, isStatusInfo: true);
		IDeviceInformation deviceInformation = communicationWrapper.DeviceList[status.Header.IpSource];
		if (deviceInformation != null)
		{
			ushort nextFrame = 0;
			if (status.NextSequence != null)
			{
				Array.Reverse(status.NextSequence);
				nextFrame = BitConverter.ToUInt16(status.NextSequence, 0);
			}
			RemoveUpdatePackagesForDevice(deviceInformation.DeviceId);
			SipCosDeviceUpdateStatusEventArgs payload = new SipCosDeviceUpdateStatusEventArgs(deviceInformation.DeviceId, (CosIPFirmwareUpdateStatusCode)status.Status, nextFrame);
			eventManager.GetEvent<SipCosDeviceUpdateStatusEvent>().Publish(payload);
		}
		Log.Debug(Module.DeviceManager, "CosIPFirmwareUpdateController", $"Received firmware answer from {communicationWrapper.DeviceList.LogInfoByAddress(status.Header.IpSource)} with status {status.Status}", isPersisted: false);
	}

	public void SendStartUpdate(Guid physicalDeviceId, byte firmwareVersion, uint firmwareImageSize)
	{
		IDeviceInformation deviceInformation = communicationWrapper.DeviceList[physicalDeviceId];
		if (deviceInformation == null)
		{
			Log.Information(Module.SipCosProtocolAdapter, $"Device {physicalDeviceId} excluded during update. Update aborted.");
			return;
		}
		PacketSequence packetSequence = new PacketSequence(SequenceType.FirmwareUpdate);
		packetSequence.ForceStayAwake = true;
		PacketSequence packetSequence2 = packetSequence;
		DisableDutyCycle(deviceInformation, packetSequence2);
		byte[] bytes = BitConverter.GetBytes(firmwareImageSize);
		Array.Reverse(bytes);
		byte[] bytes2 = BitConverter.GetBytes(deviceInformation.ManufacturerCode);
		Array.Reverse(bytes2);
		byte[] bytes3 = BitConverter.GetBytes(deviceInformation.ManufacturerDeviceType);
		Array.Reverse(bytes3);
		SIPCOSMessage packet = sipCosFirmwareUpdateCommandHandler.GenerateStart(GenerateHeader(deviceInformation), bytes2, bytes3, firmwareVersion, bytes);
		packetSequence2.Add(packet);
		communicationWrapper.SendScheduler.Enqueue(packetSequence2);
	}

	private void DisableDutyCycle(IDeviceInformation deviceInformation, PacketSequence packetSequence)
	{
		if (!devicesWithNoDutyCycle.Contains(deviceInformation.ManufacturerDeviceType))
		{
			SIPcosConfigurationCommandHandlerExt sIPcosConfigurationCommandHandlerExt = sipCosConfigHandler;
			SIPcosHeader header = GenerateHeader(deviceInformation);
			byte[] partner = new byte[3];
			SIPCOSMessage packet = sIPcosConfigurationCommandHandlerExt.GenerateStartConfiguration(header, 0, partner, 0, 0, 0);
			SIPCOSMessage packet2 = sipCosConfigHandler.GenerateIndexConfiguration(GenerateHeader(deviceInformation), 0, new byte[2] { 8, 0 });
			SIPCOSMessage packet3 = sipCosConfigHandler.GenerateEndConfiguration(GenerateHeader(deviceInformation), 0);
			packetSequence.Add(packet);
			packetSequence.Add(packet2);
			packetSequence.Add(packet3);
		}
	}

	public void EnableDutyCycle(Guid physicalDeviceId)
	{
		IDeviceInformation deviceInformation = communicationWrapper.DeviceList[physicalDeviceId];
		if (deviceInformation == null || devicesWithNoDutyCycle.Contains(deviceInformation.ManufacturerDeviceType))
		{
			SipCosDeviceUpdateStatusEventArgs payload = new SipCosDeviceUpdateStatusEventArgs(physicalDeviceId, CosIPFirmwareUpdateStatusCode.Ack, 0);
			eventManager.GetEvent<SipCosDeviceUpdateStatusEvent>().Publish(payload);
			return;
		}
		PacketSequence packetSequence = new PacketSequence(SequenceType.FirmwareUpdate);
		SIPcosConfigurationCommandHandlerExt sIPcosConfigurationCommandHandlerExt = sipCosConfigHandler;
		SIPcosHeader header = GenerateHeader(deviceInformation);
		byte[] partner = new byte[3];
		SIPCOSMessage packet = sIPcosConfigurationCommandHandlerExt.GenerateStartConfiguration(header, 0, partner, 0, 0, 0);
		SIPCOSMessage packet2 = sipCosConfigHandler.GenerateIndexConfiguration(GenerateHeader(deviceInformation), 0, new byte[2] { 8, 90 });
		SIPCOSMessage packet3 = sipCosConfigHandler.GenerateEndConfiguration(GenerateHeader(deviceInformation), 0);
		packetSequence.Add(packet);
		packetSequence.Add(packet2);
		packetSequence.Add(packet3);
		Guid key = communicationWrapper.SendScheduler.Enqueue(packetSequence);
		sequenceLookup.Add(key, physicalDeviceId);
	}

	public void SendFirmwareUpdateData(Guid physicalDeviceId, ushort sequenceNumber, byte[] firmwareData)
	{
		IDeviceInformation deviceInformation = communicationWrapper.DeviceList[physicalDeviceId];
		byte[] bytes = BitConverter.GetBytes(sequenceNumber);
		Array.Reverse(bytes);
		SIPCOSMessage message = sipCosFirmwareUpdateCommandHandler.GenerateUpdateData(GenerateHeader(deviceInformation), bytes, firmwareData);
		communicationWrapper.SendScheduler.Enqueue(new PacketSequence(message, SequenceType.FirmwareUpdate)
		{
			ForceStayAwake = true
		});
	}

	public void SendEndUpdate(Guid physicalDeviceId)
	{
		IDeviceInformation deviceInformation = communicationWrapper.DeviceList[physicalDeviceId];
		PacketSequence packetSequence = new PacketSequence(SequenceType.FirmwareUpdate);
		SIPCOSMessage packet = sipCosFirmwareUpdateCommandHandler.GenerateEnd(GenerateHeader(deviceInformation));
		packetSequence.Add(packet);
		communicationWrapper.SendScheduler.Enqueue(packetSequence);
	}

	public void SendDoUpdate(Guid physicalDeviceId)
	{
		IDeviceInformation deviceInformation = communicationWrapper.DeviceList[physicalDeviceId];
		SIPCOSMessage message = sipCosFirmwareUpdateCommandHandler.GenerateDoUpdate(GenerateHeader(deviceInformation));
		communicationWrapper.SendScheduler.Enqueue(new PacketSequence(message, SequenceType.FirmwareUpdate));
	}

	public bool DeviceSupportsDutyCycle(IDeviceInformation deviceInformation)
	{
		return !devicesWithNoDutyCycle.Contains(deviceInformation.ManufacturerDeviceType);
	}

	private static SIPcosHeader GenerateHeader(IDeviceInformation deviceInformation)
	{
		SIPcosHeader sIPcosHeader = new SIPcosHeader();
		sIPcosHeader.Destination = deviceInformation.Address;
		return sIPcosHeader;
	}

	public void RemoveUpdatePackagesForDevice(Guid physicalDeviceId)
	{
		communicationWrapper.SendScheduler.RemoveSequencesConditionally(physicalDeviceId, (PacketSequence ps) => ps.SequenceType == SequenceType.FirmwareUpdate, SequenceState.Aborted);
	}
}
