using System;
using System.Linq;
using System.Net;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication;

public class RasConnectionManager : IDisposable
{
	private const string chosenEntryName = "Lemonbeat";

	private const string modemName = "NEXUS USB Dongle";

	private readonly RasConnection rasConnection = new RasConnection();

	public IPAddress LocalAddress { get; private set; }

	public IPAddress PeerAddress { get; private set; }

	public bool IsConnected => rasConnection.IsConnected();

	public RasConnectionManager()
	{
		LocalAddress = IPAddress.None;
		PeerAddress = IPAddress.None;
	}

	public void Dispose()
	{
		if (rasConnection != null && rasConnection.IsConnected())
		{
			rasConnection.Disconnect();
		}
	}

	public bool IsDongleConnected()
	{
		return GetLemonbeatDongle() != null;
	}

	private RasDevice GetLemonbeatDongle()
	{
		if (RasCapableDevices.GetDeviceList(out var devices) == RasError.Success)
		{
			RasDevice rasDevice = devices.ToList().FindLast((RasDevice d) => d.DeviceName == "NEXUS USB Dongle");
			if (rasDevice != null)
			{
				return rasDevice;
			}
		}
		return null;
	}

	private void CreateLemonbeatPhonebookEntry(string deviceName, string deviceType)
	{
		RasError entryProperties = RasConnectionEntries.GetEntryProperties(string.Empty, out var entry, out var _);
		if (entryProperties != RasError.Success)
		{
			throw new Exception("GetEntryProperties failed, error code: " + entryProperties);
		}
		entry.Options = OptionFlags.IpHeaderCompression | OptionFlags.DisableLcpExtensions | OptionFlags.NetworkLogon;
		entry.DeviceType = deviceType;
		entry.DeviceName = deviceName;
		entry.FramingProtocol = 1u;
		entry.NetProtocols = 8u;
		entry.LocalPhoneNumber = "123";
		entryProperties = RasConnectionEntries.SetEntryProperties(entry, "Lemonbeat", null, null, null);
		if (entryProperties != RasError.Success)
		{
			throw new Exception("SetEntryProperties failed, error code: " + entryProperties);
		}
	}

	public void Connect()
	{
		if (!rasConnection.IsConnected())
		{
			RasDevice lemonbeatDongle = GetLemonbeatDongle();
			if (lemonbeatDongle == null)
			{
				throw new Exception(string.Format("Failed to load modem {0}.", "NEXUS USB Dongle"));
			}
			string[] entryNames = RasConnectionEntries.GetEntryNames();
			if (!entryNames.ToList().Contains("Lemonbeat"))
			{
				CreateLemonbeatPhonebookEntry(lemonbeatDongle.DeviceName, lemonbeatDongle.DeviceType);
			}
			Log.Information(Module.LemonbeatProtocolAdapter, string.Format("Connecting with phonebook entry {0}...", "Lemonbeat"));
			RasError rasError = rasConnection.Connect("Lemonbeat", null, null);
			if (rasError != RasError.Success)
			{
				throw new Exception("Connection failed, error code :" + rasError);
			}
			rasError = rasConnection.GetIpInfo(ipv6: true, out var localIp, out var peerIp);
			if (rasError != RasError.Success)
			{
				throw new Exception("Failed to get ipv6 address :" + rasError);
			}
			LocalAddress = localIp;
			PeerAddress = peerIp;
			Log.Debug(Module.LemonbeatProtocolAdapter, $"Lemonbeat will use following addresses: LOCAL: {localIp.ToString()}, PEER: {peerIp.ToString()}");
			RasConnectionStatus connectionStatus = rasConnection.GetConnectionStatus();
			Log.Information(Module.LemonbeatProtocolAdapter, $"Connection state: State={connectionStatus.State}, Result={connectionStatus.Error}, DeviceType={connectionStatus.DeviceType}, DeviceName={connectionStatus.DeviceName}");
			AdapterBindingControl.UnbindProtocol("NEXUS USB Dongle", "TcpIp");
		}
	}

	public void Disconnect()
	{
		if (rasConnection.IsConnected())
		{
			rasConnection.Disconnect();
			LocalAddress = IPAddress.None;
			PeerAddress = IPAddress.None;
		}
	}
}
