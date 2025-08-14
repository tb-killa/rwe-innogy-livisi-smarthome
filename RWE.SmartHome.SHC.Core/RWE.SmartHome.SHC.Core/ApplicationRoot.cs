using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Practices.Mobile.ContainerModel;
using Microsoft.Win32;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.CommonFunctionality.Implementations;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;
using RWE.SmartHome.SHC.Core.Certificates;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.TypeManager;
using SHCWrapper.Misc;
using SHCWrapper.Network;

namespace RWE.SmartHome.SHC.Core;

public sealed class ApplicationRoot
{
	private static class Singleton
	{
		internal static readonly ApplicationRoot Instance;

		static Singleton()
		{
			Instance = new ApplicationRoot();
		}
	}

	private const string DeleteDalDatabaseFileFlagPath = "\\NandFlash\\DeleteDalDatabaseFileFlag";

	private readonly Container container = new Container();

	public static ApplicationRoot Instance => Singleton.Instance;

	private ApplicationRoot()
	{
	}

	public void Run()
	{
		LocalUsbUpdate.HandleLocalCommUpdate();
		HandleFactoryReset();
		CheckCleanUpRequested();
		Configure();
		StartupPreparations();
		ModuleLoader.LoadModules(container);
		container.Resolve<ITaskManager>().Startup();
	}

	private void CheckCleanUpRequested()
	{
		if (File.Exists("\\NandFlash\\DeleteDalDatabaseFileFlag"))
		{
			File.Delete(DatabaseConnectionsPool.DalDatabaseFile);
			File.Delete("\\NandFlash\\DeleteDalDatabaseFileFlag");
		}
	}

	private void Configure()
	{
		IConfigurationManager configurationManager = ConfigurationManager.Instance;
		container.Register((Container c) => configurationManager).InitializedBy(delegate(Container c, IConfigurationManager v)
		{
			v.Initialize();
		}).ReusedWithin(ReuseScope.Container);
		container.Register((Func<Container, IEventManager>)((Container c) => new EventManager())).InitializedBy(delegate(Container c, IEventManager v)
		{
			v.Initialize();
		}).ReusedWithin(ReuseScope.Container);
		container.Register((Func<Container, ICertificateManager>)((Container c) => new CertificateManager(configurationManager))).InitializedBy(delegate(Container c, ICertificateManager v)
		{
			v.Initialize();
		}).ReusedWithin(ReuseScope.Container);
		container.Register((Func<Container, ITaskManager>)((Container c) => new TaskManager(c.Resolve<IEventManager>()))).InitializedBy(delegate(Container c, ITaskManager v)
		{
			v.Initialize();
		}).ReusedWithin(ReuseScope.Container);
		container.Register((Container c) => new DatabaseConnectionsPool("RWE.SmartHome.SHC.Database.sdf")).InitializedBy(delegate(Container c, DatabaseConnectionsPool v)
		{
			v.Initialize();
		}).ReusedWithin(ReuseScope.Container);
		container.Register((Func<Container, IShcTypeManager>)((Container c) => new ShcTypeManager())).InitializedBy(delegate(Container c, IShcTypeManager v)
		{
			v.Initialize();
		}).ReusedWithin(ReuseScope.Container);
		RWE.SmartHome.SHC.Core.Scheduler.Scheduler scheduler = new RWE.SmartHome.SHC.Core.Scheduler.Scheduler(new TimeSpan(0, 0, 0, 0, 500));
		container.Register((Func<Container, IScheduler>)((Container c) => scheduler)).ReusedWithin(ReuseScope.Container);
		container.Resolve<ITaskManager>().Register(scheduler);
		container.Register((Func<Container, IDateTimeProvider>)((Container c) => new ShcDateTimeImplementation())).ReusedWithin(ReuseScope.Container);
	}

	private void StartupPreparations()
	{
		ConfigurationProperties configurationProperties = new ConfigurationProperties(ConfigurationManager.Instance);
		bool flag = SetHostName(configurationProperties);
		flag = SetNtpServers(configurationProperties) || flag;
		flag = SetDhcpParameters() || flag;
		TimeZoneManager.RefreshTimeZone(configurationProperties.TimeZone);
		if (flag)
		{
			Reset();
		}
	}

	private static bool SetHostName(ConfigurationProperties cfgProperties)
	{
		bool result = false;
		HostNameDefinition hostNameDefinition = new HostNameDefinition(cfgProperties.HostnameFormatString, cfgProperties.NumberOfPossibleHostnames, cfgProperties.NameResolutionWait);
		if (hostNameDefinition.SetAvailableHostname(cfgProperties.ForcedHostname))
		{
			result = true;
		}
		return result;
	}

	private static bool SetNtpServers(ConfigurationProperties cfgProperties)
	{
		bool result = false;
		string nTPServers = cfgProperties.NTPServers;
		if (!string.IsNullOrEmpty(nTPServers))
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Services");
			using RegistryKey registryKey2 = registryKey.OpenSubKey("TIMESVC", writable: true);
			if (registryKey2 != null)
			{
				string[] array = nTPServers.Split(';');
				if (!(registryKey2.GetValue("server") is string[] first) || !first.SequenceEqual(array))
				{
					registryKey2.SetValue("server", array, RegistryValueKind.MultiString);
					registryKey2.Flush();
					NTP.Stop();
					NTP.Start();
					result = true;
				}
			}
		}
		return result;
	}

	private static bool SetDhcpParameters()
	{
		bool result = false;
		using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Comm"))
		{
			using RegistryKey registryKey2 = registryKey.OpenSubKey("EMACB1");
			using RegistryKey registryKey3 = registryKey2.OpenSubKey("Parms");
			using RegistryKey registryKey4 = registryKey3.OpenSubKey("TcpIp", writable: true);
			object value = registryKey4.GetValue("DhcpMaxRetry");
			object value2 = registryKey4.GetValue("DhcpAckWaitTimeout");
			if (value == null || (int)value != -1 || value2 == null || (int)value2 != 5000)
			{
				registryKey4.SetValue("DhcpMaxRetry", -1, RegistryValueKind.DWord);
				registryKey4.SetValue("DhcpAckWaitTimeout", 5000, RegistryValueKind.DWord);
				registryKey4.Flush();
				result = true;
			}
		}
		return result;
	}

	private static void Reset()
	{
		ResetManager.Reset();
		Thread.Sleep(int.MaxValue);
	}

	private static void HandleFactoryReset()
	{
		if (FactoryResetHandling.WasFactoryResetRequested(checkFactoryResetButton: true) == FactoryResetRequestedStatus.NotRequested)
		{
			return;
		}
		DeleteFileIfExists(DatabaseConnectionsPool.ConfigDatabaseFile);
		DeleteFileIfExists(DatabaseConnectionsPool.DalDatabaseFile);
		DeleteFileIfExists(LocalCommunicationConstants.IsShcLocalOnlyFlagPath);
		if (FilePersistence.FactoryResetStatus == FactoryResetRequestedStatus.RequestedThroughFactoryResetButton)
		{
			using (File.Create("\\NandFlash\\ManualReseted"))
			{
			}
		}
	}

	private static void DeleteFileIfExists(string filePath)
	{
		if (File.Exists(filePath))
		{
			Console.WriteLine("Delete the file: " + filePath);
			File.Delete(filePath);
		}
	}
}
