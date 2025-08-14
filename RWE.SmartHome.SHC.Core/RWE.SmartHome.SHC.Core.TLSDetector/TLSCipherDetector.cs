using System;
using System.Text.RegularExpressions;
using RWE.SmartHome.SHC.Core.Logging;
using Rebex;

namespace RWE.SmartHome.SHC.Core.TLSDetector;

public class TLSCipherDetector
{
	private static readonly Regex cipherRegex = new Regex("^Connection secured using cipher.*");

	private readonly string serviceName;

	private string lastMessageReceived = string.Empty;

	public TLSCipherDetector(string serviceName)
	{
		this.serviceName = serviceName;
	}

	public void CheckCipherLog(LogLevel level, Type objectType, int objectId, string area, string message, byte[] buffer, int offset, int length)
	{
		if (cipherRegex.IsMatch(message) && lastMessageReceived != message)
		{
			lastMessageReceived = message;
			Log.Information(Module.BackendCommunication, $"Service {serviceName}: {lastMessageReceived}");
		}
	}
}
