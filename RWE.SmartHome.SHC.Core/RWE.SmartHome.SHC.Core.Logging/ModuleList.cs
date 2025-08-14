namespace RWE.SmartHome.SHC.Core.Logging;

public static class ModuleList
{
	private static Module[] modules = new Module[23]
	{
		Module.ApplicationsHost,
		Module.Authentication,
		Module.BackendCommunication,
		Module.BusinessLogic,
		Module.ChannelMultiplexer,
		Module.Core,
		Module.CustomApp,
		Module.DataAccess,
		Module.DeviceActivity,
		Module.DeviceManager,
		Module.DisplayManager,
		Module.ExternalCommandDispatcher,
		Module.Logging,
		Module.NetworkMonitoring,
		Module.LemonbeatProtocolAdapter,
		Module.RelayDriver,
		Module.RuleEngine,
		Module.SerialCommunication,
		Module.SipCosProtocolAdapter,
		Module.StartupLogic,
		Module.VirtualProtocolAdapter,
		Module.wMBusProtocolAdapter,
		Module.WebServerHost
	};

	public static Module[] Modules => modules;
}
