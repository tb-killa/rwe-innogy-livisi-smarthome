using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

public class IPv6InterfaceManagement
{
	private const string EMACB1InterfaceName = "EMACB1";

	private bool interfaceDeleted;

	private readonly NetworkInterfaces networkInterfaces = new NetworkInterfaces();

	public void DeleteEmacb1Interface()
	{
		ThreadPool.QueueUserWorkItem(DeleteEmacb1InterfaceThread);
	}

	private void DeleteEmacb1InterfaceThread(object state)
	{
		try
		{
			if (!interfaceDeleted)
			{
				DeleteNetworkInterface("EMACB1");
				interfaceDeleted = true;
			}
			else
			{
				Log.Debug(RWE.SmartHome.SHC.Core.Module.LemonbeatProtocolAdapter, "IPv6 interface already deleted");
			}
		}
		catch (Exception ex)
		{
			Log.Exception(RWE.SmartHome.SHC.Core.Module.LemonbeatProtocolAdapter, ex, "Exception when deleting the EMACB1 interface(IPv6)");
		}
	}

	private void DeleteNetworkInterface(string interfaceName)
	{
		List<NetworkInterfaces.NetworkInterface> allIPv6Interfaces = networkInterfaces.GetAllIPv6Interfaces();
		NetworkInterfaces.NetworkInterface networkInterface = allIPv6Interfaces.FirstOrDefault((NetworkInterfaces.NetworkInterface m) => m.Name.Equals(interfaceName));
		if (networkInterface != null)
		{
			DeleteNetworkInterface(networkInterface.Index);
		}
		else
		{
			Log.Debug(RWE.SmartHome.SHC.Core.Module.LemonbeatProtocolAdapter, $"NetworkInterface not found (name: {interfaceName})");
		}
	}

	private void DeleteNetworkInterface(uint index)
	{
		string codeBase = Assembly.GetExecutingAssembly().GetName().CodeBase;
		string directoryName = Path.GetDirectoryName(codeBase);
		try
		{
			CreateIPV6Executable(directoryName);
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = string.Format("{0}\\{1}", directoryName, "ipv6.exe");
			processStartInfo.Arguments = $"ifd {index}";
			processStartInfo.UseShellExecute = false;
			Process process = Process.Start(processStartInfo);
			process.WaitForExit();
			if (process.HasExited)
			{
				Log.Debug(RWE.SmartHome.SHC.Core.Module.LemonbeatProtocolAdapter, $"The process for deleting network interface has terminated (exitcode: {process.ExitCode})");
			}
			else
			{
				Log.Error(RWE.SmartHome.SHC.Core.Module.LemonbeatProtocolAdapter, $"Something hapened wrong when trying to delete network interface (index: {index})");
			}
		}
		finally
		{
			DeleteIPV6Executable(directoryName);
		}
	}

	private void CreateIPV6Executable(string directory)
	{
		string sourceFileName = string.Format("{0}\\{1}", directory, "ipv6.dll");
		string destFileName = string.Format("{0}\\{1}", directory, "ipv6.exe");
		File.Copy(sourceFileName, destFileName);
	}

	private void DeleteIPV6Executable(string directory)
	{
		string path = string.Format("{0}\\{1}", directory, "ipv6.exe");
		File.Delete(path);
	}
}
