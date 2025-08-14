using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.HardwareSupport;

internal class DriverSupport
{
	private class DongleHardwareIds
	{
		public ushort Vid { get; private set; }

		public ushort Pid { get; private set; }

		public string RegistryKey { get; private set; }

		public DongleHardwareIds(ushort vid, ushort pid)
		{
			Vid = vid;
			Pid = pid;
			RegistryKey = $"Drivers\\USB\\ClientDrivers\\CP210xVCP\\Port0\\{Vid}_{Pid}";
		}
	}

	private readonly List<DongleHardwareIds> donglesHardwareIds = new List<DongleHardwareIds>
	{
		new DongleHardwareIds(7535, 16),
		new DongleHardwareIds(4292, 35740)
	};

	public DriverSupport()
	{
		RegisterSupportedHardware();
	}

	public bool IsSupportedDevice(string driverKey)
	{
		return donglesHardwareIds.Any((DongleHardwareIds m) => m.RegistryKey.Equals(driverKey));
	}

	private void RegisterSupportedHardware()
	{
		try
		{
			ReconcileDriverEntries();
		}
		catch (Exception ex)
		{
			Log.Error(Module.LemonbeatProtocolAdapter, string.Format("Error occurred while reconciling USB driver entries:", ex.Message));
		}
	}

	private void ReconcileDriverEntries()
	{
		using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Drivers\\USB");
		using RegistryKey registryKey2 = registryKey.OpenSubKey("LoadClients", writable: true);
		using RegistryKey registryKey3 = registryKey.OpenSubKey("ClientDrivers\\CP210xVCP\\Port0", writable: true);
		string[] subKeyNames = registryKey2.GetSubKeyNames();
		string[] subKeyNames2 = registryKey3.GetSubKeyNames();
		foreach (DongleHardwareIds donglesHardwareId in donglesHardwareIds)
		{
			string dongleId = $"{donglesHardwareId.Vid}_{donglesHardwareId.Pid}";
			if (!subKeyNames.Any((string subkey) => subkey == dongleId))
			{
				AddLoadClientReference(registryKey2, dongleId);
			}
			if (!subKeyNames2.Any((string subkey) => subkey == dongleId))
			{
				AddDriverReference(registryKey3, dongleId);
			}
		}
	}

	private void AddLoadClientReference(RegistryKey loadClients, string dongleId)
	{
		RegistryKey parentNode = loadClients.CreateSubKey(dongleId + "\\Default\\Default\\CP210xVCP");
		AddDllReference(parentNode);
	}

	private void AddDriverReference(RegistryKey port0, string dongleId)
	{
		RegistryKey registryKey = port0.CreateSubKey(dongleId);
		AddDllReference(registryKey);
		AddModem(registryKey);
	}

	private void AddDllReference(RegistryKey parentNode)
	{
		parentNode.SetValue("Dll", "CP210xVCP.DLL");
		parentNode.SetValue("Prefix", "COM");
		parentNode.Flush();
	}

	private void AddModem(RegistryKey driverKey)
	{
		RegistryKey registryKey = driverKey.CreateSubKey("Unimodem");
		registryKey.SetValue("Tsp", "unimodem.dll");
		registryKey.SetValue("DeviceType", 0);
		registryKey.SetValue("FriendlyName", "NEXUS USB Dongle");
		registryKey.Flush();
		AddModemConfig(registryKey);
	}

	private void AddModemConfig(RegistryKey unimodem)
	{
		RegistryKey registryKey = unimodem.CreateSubKey("config");
		registryKey.SetValue("EnableFlowSoft", 0, RegistryValueKind.DWord);
		registryKey.SetValue("EnableFlowHard", 1, RegistryValueKind.DWord);
		registryKey.SetValue("BaudRate", 115200, RegistryValueKind.DWord);
		registryKey.Flush();
	}
}
