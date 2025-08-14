using System.Collections.Generic;
using System.Threading;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;
using RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.SwitchDelegate;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager.SwitchDelegate;

internal sealed class SwitchDelegate : ISwitchDelegate
{
	private const string LoggingSource = "SwitchDelegate";

	private Dictionary<LinkEndpoint, SwitchDelegateEntry> lookupTable;

	private object lookupTableSync = new object();

	private readonly CommunicationWrapper communicationWrapper;

	private readonly IDeviceManager deviceManager;

	internal SwitchDelegate(CommunicationWrapper communicationWrapper, IDeviceManager deviceManager)
	{
		this.deviceManager = deviceManager;
		this.communicationWrapper = communicationWrapper;
		communicationWrapper.ConditionalSwitchCommandHandler.ReceivedConditionalSwitchCommand += ReceivedSwitchCommand;
		communicationWrapper.UnconditionalSwitchCommandHandler.ReceivedUnconditionalSwitchCommand += ReceivedSwitchCommand;
	}

	private void ReceivedSwitchCommand(SIPcosHeader header, SwitchCommand switchCommand)
	{
		Log.Debug(Module.DeviceManager, "SwitchDelegate", $"Received switch command from device {header.IpSource.ToReadable()} to address {header.IpDestination.ToReadable()}");
		if (!header.Destination.Compare(SipCosAddress.AllDevices))
		{
			lock (lookupTableSync)
			{
				if (lookupTable != null)
				{
					if (lookupTable.TryGetValue(new LinkEndpoint(header.IpSource, switchCommand.KeyChannelNumber), out var entry))
					{
						bool flag = entry.KeyPressCounter != switchCommand.KeyStrokeCounter;
						if (header.BiDi)
						{
							communicationWrapper.SendAppAck(header.IpSource, header.IpDestination, header.SequenceNumber, flag);
						}
						if (!switchCommand.ActivationTime && flag)
						{
							entry.KeyPressCounter = switchCommand.KeyStrokeCounter;
							ThreadPool.QueueUserWorkItem(delegate
							{
								SendRoutedCommands(header, switchCommand, entry);
							});
						}
					}
				}
			}
		}
		communicationWrapper.SendScheduler.UpdateLastOnTimeOfDevice(header.IpSource, header.BiDi ? AwakeModifier.Bidi : AwakeModifier.None);
	}

	private void SendRoutedCommands(SIPcosHeader header, SwitchCommand switchCommand, SwitchDelegateEntry entry)
	{
		Thread.Sleep(100);
		foreach (byte[] destinationAddress in entry.DestinationAddresses)
		{
			SIPcosHeader sIPcosHeader = new SIPcosHeader();
			sIPcosHeader.AddressExtensionType = AddressExtensionType.LAST_ROUTED;
			sIPcosHeader.BiDi = false;
			sIPcosHeader.MacDestination = destinationAddress;
			sIPcosHeader.FrameType = header.FrameType;
			sIPcosHeader.IpDestination = destinationAddress;
			sIPcosHeader.IpSource = header.IpSource;
			sIPcosHeader.MacSource = deviceManager.DefaultShcAddress;
			SIPcosHeader header2 = sIPcosHeader;
			PacketSequence packetSequence = new PacketSequence(SequenceType.DirectExecution);
			packetSequence.Add(new SIPCOSMessage(header2, switchCommand.ToArray()));
			IDeviceInformation deviceInformation = deviceManager.DeviceList[destinationAddress];
			if (deviceInformation != null)
			{
				communicationWrapper.SendScheduler.RemoveSequencesConditionally(deviceInformation.DeviceId, (PacketSequence s) => ShouldRemove(s, SequenceType.DirectExecution, header.Source), SequenceState.Aborted);
				communicationWrapper.SendScheduler.Enqueue(packetSequence);
			}
		}
	}

	private static bool ShouldRemove(PacketSequence sequence, SequenceType sequenceType, byte[] sourceAddress)
	{
		if (sequence.SequenceType != sequenceType)
		{
			return false;
		}
		if (sequence.Current.Header is SIPcosHeader sIPcosHeader)
		{
			byte[] message = sequence.Current.Message;
			if (message != null && (sIPcosHeader.FrameType == SIPcosFrameType.CONDITIONAL_SWITCH_COMMAND || sIPcosHeader.FrameType == SIPcosFrameType.UNCONDITIONAL_SWITCH_COMMAND))
			{
				return sourceAddress.Compare(sIPcosHeader.IpSource);
			}
		}
		return false;
	}

	public void SetLookupTable(IEnumerable<KeyValuePair<LinkEndpoint, IEnumerable<byte[]>>> list)
	{
		if (list == null)
		{
			return;
		}
		Dictionary<LinkEndpoint, SwitchDelegateEntry> dictionary = new Dictionary<LinkEndpoint, SwitchDelegateEntry>();
		foreach (KeyValuePair<LinkEndpoint, IEnumerable<byte[]>> item in list)
		{
			dictionary.Add(item.Key, new SwitchDelegateEntry(item.Value));
		}
		lock (lookupTableSync)
		{
			lookupTable = dictionary;
		}
	}
}
