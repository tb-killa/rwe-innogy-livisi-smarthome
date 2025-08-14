using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.CommonFunctionality.Implementations;

internal class NetworkToolsImplementation : INetworkTools
{
	private const int WSAHOST_NOT_FOUND = 11001;

	private const int WSAENETDOWN = 10050;

	private static readonly string DEFAULT_ADAPTER_NAME = "EMACB1";

	[DllImport("CommonFunctionalityNative.dll", CharSet = CharSet.Unicode)]
	private static extern bool IsNetworkAdapterOperational(string adapterName);

	private static bool HasTcpIpParameter(string valueName)
	{
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Comm");
			using RegistryKey registryKey2 = registryKey.OpenSubKey(DEFAULT_ADAPTER_NAME);
			using RegistryKey registryKey3 = registryKey2.OpenSubKey("Parms");
			using RegistryKey registryKey4 = registryKey3.OpenSubKey("TcpIp");
			object value = registryKey4.GetValue(valueName);
			if (value is string[])
			{
				string[] array = (string[])value;
				return array.Length > 0 && !string.IsNullOrEmpty(array[0]);
			}
			if (value is string)
			{
				string value2 = (string)value;
				return !string.IsNullOrEmpty(value2);
			}
			return false;
		}
		catch
		{
			return false;
		}
	}

	public NetworkProblem DiagnoseNetworkProblem(string host, Exception networkException)
	{
		if (!IsNetworkAdapterOperational(DEFAULT_ADAPTER_NAME))
		{
			return NetworkProblem.NetworkAdapterNotOperational;
		}
		if (!HasTcpIpParameter("DhcpIPAddress"))
		{
			return NetworkProblem.NoDhcpIpAddress;
		}
		if (!HasTcpIpParameter("DhcpDefaultGateway"))
		{
			return NetworkProblem.NoDhcpDefaultGateway;
		}
		if (!string.IsNullOrEmpty(host))
		{
			try
			{
				Dns.GetHostEntry(host);
			}
			catch (SocketException ex)
			{
				int errorCode = ex.ErrorCode;
				if (errorCode == 10050)
				{
					return NetworkProblem.NameResolutionFailedNetworkDown;
				}
				return NetworkProblem.NameResolutionFailed;
			}
			catch (Exception ex2)
			{
				Console.WriteLine(ex2.Message);
			}
		}
		return NetworkProblem.Other;
	}

	public string GetDeviceIp()
	{
		string hostName = Dns.GetHostName();
		return GetIpAddress(hostName);
	}

	public string GetHostName()
	{
		string result = null;
		using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Ident"))
		{
			if (registryKey != null)
			{
				result = registryKey.GetValue("Name").ToString();
			}
		}
		return result;
	}

	public void SetHostName(string newHostName)
	{
		using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Ident", writable: true);
		if (registryKey != null)
		{
			registryKey.SetValue("Name", newHostName, RegistryValueKind.String);
			registryKey.Flush();
		}
	}

	public string GetIpAddress(string hostName)
	{
		string result = null;
		try
		{
			IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
			if (hostEntry != null && hostEntry.AddressList != null && hostEntry.AddressList.Length > 0)
			{
				IPAddress iPAddress = hostEntry.AddressList.FirstOrDefault((IPAddress ipa) => ipa.AddressFamily == AddressFamily.InterNetwork);
				if (iPAddress != null)
				{
					result = iPAddress.ToString();
				}
				else
				{
					iPAddress = hostEntry.AddressList.FirstOrDefault((IPAddress ipa) => ipa.AddressFamily == AddressFamily.InterNetworkV6);
					if (iPAddress != null)
					{
						result = iPAddress.ToString().ToUpper();
					}
				}
			}
		}
		catch (SocketException)
		{
		}
		return result;
	}

	public string GetDeviceMacAddress()
	{
		string result = string.Empty;
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Comm");
			using RegistryKey registryKey2 = registryKey.OpenSubKey(DEFAULT_ADAPTER_NAME);
			using RegistryKey registryKey3 = registryKey2.OpenSubKey("Parms");
			object value = registryKey3.GetValue("NetworkAddress");
			if (value is string)
			{
				result = (string)value;
			}
		}
		catch
		{
		}
		return result;
	}

	public NetworkSpecifics GetNetworkSpecifics()
	{
		NetworkSpecifics result = default(NetworkSpecifics);
		string text = Registry.LocalMachine.Name + "\\Comm\\EMACB1\\Parms\\TcpIp";
		string registryKey;
		string registryKey2;
		string registryKey3;
		if ((int)Registry.GetValue(text, "EnableDhcp", 0) == 1)
		{
			registryKey = "DhcpSubnetMask";
			registryKey2 = "DhcpDefaultGateway";
			registryKey3 = "DhcpDNS";
			result.DhcpServer = GetArrayRegistryKey(text, "DhcpServer");
		}
		else
		{
			registryKey = "Subnetmask";
			registryKey2 = "DefaultGateway";
			registryKey3 = "DNS";
			result.DhcpServer = string.Empty;
		}
		result.SubnetMask = GetArrayRegistryKey(text, registryKey);
		result.DefaultGateway = GetArrayRegistryKey(text, registryKey2);
		result.DnsServer = GetArrayRegistryKey(text, registryKey3);
		return result;
	}

	private static string GetArrayRegistryKey(string keyPath, string registryKey)
	{
		return string.Join(", ", (string[])Registry.GetValue(keyPath, registryKey, new string[1] { string.Empty }));
	}

	public bool ValidateIPAddress(IPAddress addrToValidate)
	{
		bool result = false;
		try
		{
			IPAddress subnetMask = IPAddress.Parse(GetNetworkSpecifics().SubnetMask);
			IPAddress address = IPAddress.Parse(GetDeviceIp());
			IPAddress networkAddress = GetNetworkAddress(address, subnetMask);
			IPAddress networkAddress2 = GetNetworkAddress(addrToValidate, subnetMask);
			result = networkAddress.Equals(networkAddress2);
		}
		catch (Exception)
		{
		}
		return result;
	}

	private static IPAddress GetNetworkAddress(IPAddress address, IPAddress subnetMask)
	{
		byte[] addressBytes = address.GetAddressBytes();
		byte[] addressBytes2 = subnetMask.GetAddressBytes();
		if (addressBytes.Length != addressBytes2.Length)
		{
			throw new ArgumentException("Lengths of IP address and subnet mask do not match.");
		}
		byte[] array = new byte[addressBytes.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)(addressBytes[i] & addressBytes2[i]);
		}
		return new IPAddress(array);
	}
}
