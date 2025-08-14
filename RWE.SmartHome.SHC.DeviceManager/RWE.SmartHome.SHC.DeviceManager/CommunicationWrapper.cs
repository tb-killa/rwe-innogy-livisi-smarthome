using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;
using RWE.SmartHome.SHC.DeviceManager.Configuration;
using RWE.SmartHome.SHC.DeviceManager.Persistence;
using RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;
using SerialAPI;
using SerialApiInterfaces;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManager;

internal class CommunicationWrapper : ICommunicationWrapper
{
	private const string LoggingSource = "CommunicationWrapper";

	private const string CoprocessorDriverKey = "Drivers\\BuiltIn\\Serial1";

	private readonly ISIPcosConditionalSwitchCommandHandlerExt conditionalSwitchCommandHandler;

	private readonly ISIPcosUnconditionalSwitchCommandHandlerExt unconditionalSwitchCommandHandler;

	private readonly SIPcosNetworkManagementCommandHandlerExt networkHandler;

	private readonly SIPcosConfigurationCommandHandlerExt configurationHandler;

	private readonly SIPcosStatusInfoCommandHandlerExt statusInfoHandler;

	private readonly SIPcosTimeInformationCommandHandlerExt timeInfoHandler;

	private readonly SIPcosAnswerCommandHandlerExt answerCommandHandler;

	private readonly SIPcosFirmwareUpdateCommandHandlerExt firmwareUpdateCommandHandler;

	private readonly SIPcosRouteManagementCommandHandlerExt routeManagementCommandHandler;

	private readonly CommandHandler commandHandler;

	private readonly SIPcosHandler sipCosHandler;

	private readonly ISendScheduler sendScheduler;

	private readonly IDeviceList deviceList;

	private readonly ICMPHandlerExt icmpHandler;

	private readonly RWE.SmartHome.SHC.DeviceManager.Configuration.Configuration configuration;

	private readonly FrameEvaluator frameEvaluator;

	private readonly SerialAPI.Core core;

	public SIPcosFirmwareUpdateCommandHandlerExt FirmwareUpdateCommandHandler => firmwareUpdateCommandHandler;

	public ICMPHandlerExt IcmpHandler => icmpHandler;

	public CommandHandler CommandHandler => commandHandler;

	public SIPcosAnswerCommandHandlerExt AnswerCommandHandler => answerCommandHandler;

	public SIPcosTimeInformationCommandHandlerExt TimeInfoHandler => timeInfoHandler;

	public SIPcosStatusInfoCommandHandlerExt StatusInfoHandler => statusInfoHandler;

	public SIPcosConfigurationCommandHandlerExt ConfigurationHandler => configurationHandler;

	public SIPcosNetworkManagementCommandHandlerExt NetworkHandler => networkHandler;

	public ISendScheduler SendScheduler => sendScheduler;

	public ISIPcosUnconditionalSwitchCommandHandlerExt UnconditionalSwitchCommandHandler => unconditionalSwitchCommandHandler;

	public ISIPcosConditionalSwitchCommandHandlerExt ConditionalSwitchCommandHandler => conditionalSwitchCommandHandler;

	public SIPcosRouteManagementCommandHandlerExt RouteManagementCommandHandler => routeManagementCommandHandler;

	public SIPcosHandler SipCosHandler => sipCosHandler;

	public IDeviceList DeviceList => deviceList;

	public CommunicationWrapper(ISerialPort serialPort, IDeviceList deviceList, IEventManager eventManager, ISipCosPersistence presistence, IConfigurationManager configurationManager, IScheduler scheduler, IDeviceKeyRepository deviceKeyRepository)
	{
		this.deviceList = deviceList;
		core = new SerialAPI.Core(serialPort);
		core.ReceiveSequenceProblem += OnReceiveSequenceProblem;
		string comPort = GetComPort();
		if (!core.Open(comPort, Baudrate.Baudrate115200))
		{
			Log.Error(Module.DeviceManager, $"Unable to open port {comPort}@115200bps");
		}
		deviceList.ForceDetectionOfRouters = true;
		frameEvaluator = new FrameEvaluator(deviceList);
		configuration = new RWE.SmartHome.SHC.DeviceManager.Configuration.Configuration(configurationManager);
		commandHandler = new CommandHandler(core);
		sipCosHandler = new SIPcosHandler(core, new BidCoSKeyRetriever(eventManager, deviceKeyRepository), new BidCoSPersistance(presistence), eventManager, scheduler);
		timeInfoHandler = new SIPcosTimeInformationCommandHandlerExt(SipCosHandler);
		conditionalSwitchCommandHandler = new SIPcosConditionalSwitchCommandHandlerExt(SipCosHandler, frameEvaluator);
		unconditionalSwitchCommandHandler = new SIPcosUnconditionalSwitchCommandHandlerExt(SipCosHandler, frameEvaluator);
		answerCommandHandler = new SIPcosAnswerCommandHandlerExt(SipCosHandler);
		configurationHandler = new SIPcosConfigurationCommandHandlerExt(SipCosHandler, frameEvaluator);
		networkHandler = new SIPcosNetworkManagementCommandHandlerExt(SipCosHandler);
		statusInfoHandler = new SIPcosStatusInfoCommandHandlerExt(SipCosHandler);
		icmpHandler = new ICMPHandlerExt(core);
		firmwareUpdateCommandHandler = new SIPcosFirmwareUpdateCommandHandlerExt(SipCosHandler);
		routeManagementCommandHandler = new SIPcosRouteManagementCommandHandlerExt(SipCosHandler);
		sendScheduler = new SendScheduler(new SIPcosHandlerProxy(SipCosHandler), icmpHandler, deviceList, eventManager, scheduler, configuration);
		RegisterDebugData();
	}

	private string GetComPort()
	{
		using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Drivers\\Active"))
		{
			List<string> list = registryKey.GetSubKeyNames().ToList();
			foreach (string item in list)
			{
				try
				{
					RegistryKey registryKey2 = registryKey.OpenSubKey(item);
					if ("Drivers\\BuiltIn\\Serial1".Equals((string)registryKey2.GetValue("Key")))
					{
						return (string)registryKey2.GetValue("Name");
					}
				}
				catch
				{
				}
			}
		}
		Log.Error(Module.DeviceManager, "COM port for communication with coprocessor not found. Using the default COM2: (but this might cause other problems)");
		return "COM2:";
	}

	public void SendAppAck(byte[] destination, byte[] source, byte sequenceNumber, bool forceStayAwake)
	{
		if (deviceList.Contains(destination))
		{
			SIPcosHeader sIPcosHeader = new SIPcosHeader();
			sIPcosHeader.Destination = destination;
			sIPcosHeader.MacSource = source;
			sIPcosHeader.SequenceNumber = sequenceNumber;
			SIPcosHeader header = sIPcosHeader;
			SIPCOSMessage message = AnswerCommandHandler.GenerateAnswerFrame(header, SIPcosAnswerFrameStatus.ACK);
			PacketSequence packetSequence = new PacketSequence(message, SequenceType.Ack);
			packetSequence.ForceStayAwake = forceStayAwake;
			SendScheduler.Enqueue(packetSequence);
		}
	}

	public void SendAppAckWithoutExpectedReply(byte[] destination, byte[] source, byte sequenceNumber, bool forceStayAwake)
	{
		if (deviceList.Contains(destination))
		{
			SIPcosHeader sIPcosHeader = new SIPcosHeader();
			sIPcosHeader.Destination = destination;
			sIPcosHeader.MacSource = source;
			sIPcosHeader.SequenceNumber = sequenceNumber;
			SIPcosHeader header = sIPcosHeader;
			SIPCOSMessage sIPCOSMessage = AnswerCommandHandler.GenerateAnswerFrame(header, SIPcosAnswerFrameStatus.ACK);
			PacketSequence packetSequence = new PacketSequence(sIPCOSMessage, SequenceType.Ack);
			packetSequence.ForceStayAwake = forceStayAwake;
			sIPCOSMessage.ExpectReply = false;
			SendScheduler.Enqueue(packetSequence);
		}
	}

	public void StartReception()
	{
		core.Start();
	}

	private void RegisterDebugData()
	{
		if (0 >= (int)ModuleInfos.GetLogLevel(Module.DeviceManager))
		{
			SipCosHandler.ReceiveDebugData += ReceiveDebugData;
		}
	}

	private void ReceiveDebugData(string data)
	{
		Log.Debug(Module.DeviceManager, "CommunicationWrapper", $"Debug data from SerialAPI: {data}");
	}

	private void OnReceiveSequenceProblem(byte[] Address, uint OldSequenceCount, uint NewSequenceCount)
	{
		Log.Information(Module.DeviceManager, $"Received old message from address: {Address.ToReadable()}. OldSequenceCount: {OldSequenceCount}. NewSequenceCount: {NewSequenceCount}");
	}
}
