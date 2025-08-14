using System;
using System.Linq;
using System.Text;
using System.Threading;
using RWE.SmartHome.SHC.CommonFunctionality.P2PMessageQueue;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public class HostNameDefinition
{
	private enum NameResolutionResult
	{
		Ok,
		Fail,
		Updated,
		Busy
	}

	private readonly int numberOfPossibleHostnames;

	private readonly string hostnameFormatString;

	private string[] possibleHostnames;

	private readonly int queueReadTimeout;

	public string[] PossibleHostnames
	{
		get
		{
			if (possibleHostnames == null)
			{
				possibleHostnames = new string[numberOfPossibleHostnames];
				for (int i = 0; i < numberOfPossibleHostnames; i++)
				{
					possibleHostnames[i] = string.Format(hostnameFormatString, i + 1);
				}
			}
			return possibleHostnames;
		}
	}

	public HostNameDefinition(string formatString, int? nrOfPossibleHostnames, int? nameResolutionWait)
	{
		hostnameFormatString = formatString;
		numberOfPossibleHostnames = nrOfPossibleHostnames.Value;
		if (numberOfPossibleHostnames < 1)
		{
			numberOfPossibleHostnames = 1;
		}
		queueReadTimeout = nameResolutionWait.Value;
	}

	public bool SetAvailableHostname(string forcedHostname)
	{
		if (string.IsNullOrEmpty(forcedHostname))
		{
			try
			{
				switch (CheckHostName(queueReadTimeout))
				{
				case NameResolutionResult.Updated:
					Console.WriteLine("[HostNameDefinition] Host name updated, will reboot.");
					return true;
				default:
					Console.WriteLine("[HostNameDefinition] Error: failed to check host name.");
					break;
				case NameResolutionResult.Ok:
					break;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("[HostNameDefinition] {0}: {1}", ((object)ex).GetType(), ex.Message);
			}
			return false;
		}
		string hostName = NetworkTools.GetHostName();
		if (string.Compare(hostName, forcedHostname, ignoreCase: true) != 0)
		{
			if (PossibleHostnames.Contains(forcedHostname))
			{
				NetworkTools.SetHostName(forcedHostname);
				Thread.Sleep(100);
				return true;
			}
			Console.WriteLine("[HostNameDefinition] Error: Forced hostname [{0}] does not match the valid pattern", forcedHostname);
		}
		return false;
	}

	private static NameResolutionResult CheckHostName(int timeout)
	{
		RWE.SmartHome.SHC.CommonFunctionality.P2PMessageQueue.P2PMessageQueue p2PMessageQueue = new RWE.SmartHome.SHC.CommonFunctionality.P2PMessageQueue.P2PMessageQueue(forReading: true, "smarthome", 512, 1);
		try
		{
			Message message = new Message();
			ReadWriteResult readWriteResult = p2PMessageQueue.Receive(message, timeout);
			switch (readWriteResult)
			{
			case ReadWriteResult.Timeout:
				throw new TimeoutException("Timed out reading from queue.");
			default:
				throw new InvalidOperationException($"Failed to read from queue: {readWriteResult}");
			case ReadWriteResult.OK:
			{
				string value = Encoding.Unicode.GetString(message.MessageBytes, 0, message.MessageBytes.Length);
				return (NameResolutionResult)Enum.Parse(typeof(NameResolutionResult), value, ignoreCase: true);
			}
			}
		}
		finally
		{
			p2PMessageQueue.Close();
		}
	}
}
