using RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;
using RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager;

internal interface ICommunicationWrapper
{
	SIPcosFirmwareUpdateCommandHandlerExt FirmwareUpdateCommandHandler { get; }

	ICMPHandlerExt IcmpHandler { get; }

	CommandHandler CommandHandler { get; }

	SIPcosAnswerCommandHandlerExt AnswerCommandHandler { get; }

	SIPcosTimeInformationCommandHandlerExt TimeInfoHandler { get; }

	SIPcosStatusInfoCommandHandlerExt StatusInfoHandler { get; }

	SIPcosConfigurationCommandHandlerExt ConfigurationHandler { get; }

	SIPcosNetworkManagementCommandHandlerExt NetworkHandler { get; }

	ISendScheduler SendScheduler { get; }

	ISIPcosUnconditionalSwitchCommandHandlerExt UnconditionalSwitchCommandHandler { get; }

	ISIPcosConditionalSwitchCommandHandlerExt ConditionalSwitchCommandHandler { get; }

	SIPcosRouteManagementCommandHandlerExt RouteManagementCommandHandler { get; }

	SIPcosHandler SipCosHandler { get; }

	IDeviceList DeviceList { get; }

	void SendAppAck(byte[] destination, byte[] source, byte sequenceNumber, bool forceStayAwake);

	void SendAppAckWithoutExpectedReply(byte[] destination, byte[] source, byte sequenceNumber, bool forceStayAwake);

	void StartReception();
}
