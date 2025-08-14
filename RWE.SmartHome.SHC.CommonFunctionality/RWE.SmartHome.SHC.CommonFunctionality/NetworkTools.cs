using System;
using System.Net;
using RWE.SmartHome.SHC.CommonFunctionality.Implementations;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class NetworkTools
{
	private static INetworkTools Instance = new NetworkToolsImplementation();

	internal static void OverrideImplementation(INetworkTools implementation)
	{
		Instance = implementation;
	}

	public static NetworkProblem DiagnoseNetworkProblem(string host, Exception networkException)
	{
		return Instance.DiagnoseNetworkProblem(host, networkException);
	}

	public static string GetDeviceIp()
	{
		return Instance.GetDeviceIp();
	}

	public static string GetDeviceMacAddress()
	{
		return Instance.GetDeviceMacAddress();
	}

	public static NetworkSpecifics GetNetworkSpecifics()
	{
		return Instance.GetNetworkSpecifics();
	}

	public static string GetHostName()
	{
		return Instance.GetHostName();
	}

	public static void SetHostName(string newHostName)
	{
		Instance.SetHostName(newHostName);
	}

	public static string GetIpAddress(string hostName)
	{
		return Instance.GetIpAddress(hostName);
	}

	public static bool ValidateIPAddress(IPAddress addrToValdiate)
	{
		return Instance.ValidateIPAddress(addrToValdiate);
	}
}
