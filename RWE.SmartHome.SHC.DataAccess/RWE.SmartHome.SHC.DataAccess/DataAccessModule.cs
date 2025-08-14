using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts.API;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DataAccess.Applications;
using RWE.SmartHome.SHC.DataAccess.Configuration;
using RWE.SmartHome.SHC.DataAccess.DeviceActivity;
using RWE.SmartHome.SHC.DataAccess.Messages;
using RWE.SmartHome.SHC.DataAccess.ProtocolSpecificData;
using RWE.SmartHome.SHC.DataAccess.TechnicalConfiguration;
using RWE.SmartHome.SHC.DataAccessInterfaces.Applications;
using RWE.SmartHome.SHC.DataAccessInterfaces.Configuration;
using RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;
using RWE.SmartHome.SHC.DataAccessInterfaces.Messages;
using RWE.SmartHome.SHC.DataAccessInterfaces.ProtocolSpecificData;
using RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;

namespace RWE.SmartHome.SHC.DataAccess;

public class DataAccessModule : IModule
{
	public void Configure(Container container)
	{
		try
		{
			container.Register((Func<Container, IMessagePersistence>)((Container c) => new MessagesAndAlertsPersistence(container))).InitializedBy(delegate(Container c, IMessagePersistence v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			container.Resolve<IMessagePersistence>();
			Log.Information(Module.DataAccess, "Initialization of MessagePersistence successful");
		}
		catch (Exception ex)
		{
			Log.Information(Module.DataAccess, $"Initialization of MessagePersistence failed: {ex.Message}");
		}
		try
		{
			ConfigurationPersistence configPersistence = new ConfigurationPersistence(container);
			container.Register((Func<Container, IConfigurationPersistence>)((Container c) => configPersistence)).InitializedBy(delegate(Container c, IConfigurationPersistence v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			container.Register((IConfigurationSettingsPersistence)configPersistence);
			Log.Information(Module.DataAccess, "Initialization of ConfigurationPersistence successful");
		}
		catch (Exception ex2)
		{
			Log.Error(Module.DataAccess, $"Initialization of ConfigurationPersistence failed: {ex2.Message}");
		}
		ConfigurationRepositoryProxy configurationProxy = new ConfigurationRepositoryProxy(container.Resolve<IConfigurationPersistence>(), container.Resolve<IConfigurationSettingsPersistence>());
		container.Register((Func<Container, IRepository>)((Container c) => configurationProxy)).ReusedWithin(ReuseScope.Container);
		container.Register((Func<Container, IProxyRepository>)((Container c) => configurationProxy)).ReusedWithin(ReuseScope.Container);
		container.Register((Func<Container, IIntegrityHandlerAggregator>)((Container ia) => configurationProxy));
		container.Register((Func<Container, IIntegrityManagementControl>)((Container ic) => configurationProxy));
		Resolver.EntityCache = container.Resolve<IRepository>();
		try
		{
			container.Register((Func<Container, ITechnicalConfigurationPersistence>)((Container c) => new TechnicalConfigurationPersistence(container))).InitializedBy(delegate(Container c, ITechnicalConfigurationPersistence v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			Log.Information(Module.DataAccess, "Initialization of TechnicalConfigurationPersistence successful");
		}
		catch (Exception ex3)
		{
			Log.Error(Module.DataAccess, $"Initialization of TechnicalConfigurationPersistence failed: {ex3.Message}");
		}
		try
		{
			container.Register((Func<Container, IProtocolSpecificDataPersistence>)((Container c) => new ProtocolSpecificDataPersistence(container.Resolve<DatabaseConnectionsPool>(), container.Resolve<IEventManager>()))).InitializedBy(delegate(Container c, IProtocolSpecificDataPersistence v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			Log.Information(Module.DataAccess, "Initialization of DeviceListPersistence successful");
		}
		catch (Exception ex4)
		{
			Log.Error(Module.DataAccess, $"Initialization of DeviceListPersistence failed: {ex4.Message}");
		}
		try
		{
			container.Register((Func<Container, IApplicationsSettings>)((Container c) => new ApplicationsSettingsPersistence(container.Resolve<DatabaseConnectionsPool>()))).InitializedBy(delegate(Container c, IApplicationsSettings v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			Log.Information(Module.DataAccess, "Initialization of ApplicationsSettingsPersistence successful");
		}
		catch (Exception ex5)
		{
			Log.Error(Module.DataAccess, $"Initialization of ApplicationsSettingsPersistence failed: {ex5.Message}");
		}
		try
		{
			container.Register((Func<Container, IApplicationsTokenPersistence>)((Container c) => new ApplicationsTokenPersistence(container.Resolve<DatabaseConnectionsPool>()))).InitializedBy(delegate(Container c, IApplicationsTokenPersistence v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			Log.Information(Module.DataAccess, "Initialization of ApplicationsTokenPersistence successful");
		}
		catch (Exception ex6)
		{
			Log.Error(Module.DataAccess, $"Initialization of ApplicationsTokenPersistence failed: {ex6.Message}");
		}
		try
		{
			container.Register((Func<Container, IDeviceActivityPersistence>)((Container c) => new DeviceActivityPersistence(container.Resolve<DatabaseConnectionsPool>()))).InitializedBy(delegate(Container c, IDeviceActivityPersistence v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			Log.Information(Module.DataAccess, "Initialization of device activity logging persistence successful");
		}
		catch (Exception ex7)
		{
			Log.Error(Module.DataAccess, $"Initialization of device activity logging persistence failed: {ex7.Message}");
		}
		try
		{
			container.Register((Func<Container, IDalUsbStorage>)((Container c) => new DalUsbStorage()));
			Log.Information(Module.DataAccess, "Initialization of usb DAL storage successful");
		}
		catch (Exception ex8)
		{
			Log.Error(Module.DataAccess, $"Initialization of usb DAL storage failed: {ex8.Message}");
		}
		try
		{
			container.Register((Func<Container, ITrackDataPersistence>)((Container c) => new TrackDataPersistence(c.Resolve<IConfigurationManager>(), container.Resolve<IDalUsbStorage>(), container.Resolve<IEventManager>()))).InitializedBy(delegate(Container c, ITrackDataPersistence v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			Log.Information(Module.DataAccess, "Initialization of device activity logging persistence successful");
		}
		catch (Exception ex9)
		{
			Log.Error(Module.DataAccess, $"Initialization of device activity logging persistence failed: {ex9.Message}");
		}
		try
		{
			container.Register((Func<Container, IUtilityDataPersistence>)((Container c) => new UtilityDataPersistence(c.Resolve<IConfigurationManager>(), container.Resolve<IDalUsbStorage>(), container.Resolve<IEventManager>()))).InitializedBy(delegate(Container c, IUtilityDataPersistence v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			Log.Information(Module.DataAccess, "Initialization of utility data persistence successful");
		}
		catch (Exception ex10)
		{
			Log.Error(Module.DataAccess, $"Initialization of device activity logging persistence failed: {ex10.Message}");
		}
	}
}
