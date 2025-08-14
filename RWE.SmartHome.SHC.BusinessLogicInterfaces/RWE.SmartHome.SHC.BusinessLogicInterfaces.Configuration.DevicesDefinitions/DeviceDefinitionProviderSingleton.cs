namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

public sealed class DeviceDefinitionProviderSingleton
{
	private static readonly IDeviceDefinitionsProvider instance = new DeviceDefinitionsProvider();

	public static IDeviceDefinitionsProvider Instance => instance;

	private DeviceDefinitionProviderSingleton()
	{
	}
}
