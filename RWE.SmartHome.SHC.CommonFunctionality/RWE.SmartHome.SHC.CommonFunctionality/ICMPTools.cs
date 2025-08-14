using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class ICMPTools
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	private struct ICMP_OPTIONS
	{
		public byte Ttl;

		public byte Tos;

		public byte Flags;

		public byte OptionsSize;

		public IntPtr OptionsData;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	private struct ICMP_ECHO_REPLY
	{
		public int Address;

		public int Status;

		public int RoundTripTime;

		public short DataSize;

		public short Reserved;

		public IntPtr DataPtr;

		public ICMP_OPTIONS Options;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 250)]
		public string Data;
	}

	private enum ICMPStatus
	{
		IP_SUCCESS = 0,
		IP_BUF_TOO_SMALL = 11001,
		IP_DEST_NET_UNREACHABLE = 11002,
		IP_DEST_HOST_UNREACHABLE = 11003,
		IP_DEST_PROT_UNREACHABLE = 11004,
		IP_DEST_PORT_UNREACHABLE = 11005,
		IP_NO_RESOURCES = 11006,
		IP_BAD_OPTION = 11007,
		IP_HW_ERROR = 11008,
		IP_PACKET_TOO_BIG = 11009,
		IP_REQ_TIMED_OUT = 11010,
		IP_BAD_REQ = 11011,
		IP_BAD_ROUTE = 11012,
		IP_TTL_EXPIRED_TRANSIT = 11013,
		IP_TTL_EXPIRED_REASSEM = 11014,
		IP_PARAM_PROBLEM = 11015,
		IP_SOURCE_QUENCH = 11016,
		IP_OPTION_TOO_BIG = 11017,
		IP_BAD_DESTINATION = 11018,
		IP_GENERAL_FAILURE = 11050
	}

	private static byte MAX_HOPS = 30;

	private static List<string> logLines = new List<string>();

	[DllImport("CommonFunctionalityNative.dll", CharSet = CharSet.Unicode)]
	private static extern bool IsNetworkCableConnected(string adapterName);

	[DllImport("iphlpapi.dll", SetLastError = true)]
	private static extern IntPtr IcmpCreateFile();

	[DllImport("iphlpapi.dll", SetLastError = true)]
	private static extern bool IcmpCloseHandle(IntPtr handle);

	[DllImport("iphlpapi.dll", SetLastError = true)]
	private static extern int IcmpSendEcho(IntPtr icmpHandle, int destinationAddress, IntPtr requestData, short requestSize, IntPtr requestOptions, IntPtr replyBuffer, int replySize, int timeout);

	private static void TraceRoute(IPAddress IP, byte maxHops)
	{
		byte[] array = new byte[4] { 1, 2, 3, 4 };
		ICMP_OPTIONS iCMP_OPTIONS = default(ICMP_OPTIONS);
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ICMP_OPTIONS)));
		ICMP_ECHO_REPLY iCMP_ECHO_REPLY = default(ICMP_ECHO_REPLY);
		IntPtr intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ICMP_ECHO_REPLY)) + array.Length);
		IntPtr intPtr3 = Marshal.AllocHGlobal(Marshal.SizeOf((object)array));
		Marshal.Copy(array, 0, intPtr3, Marshal.SizeOf((object)array));
		int num = 0;
		logLines.Add($"SHC: Starting traceroute to: {IP.ToString()} over maximum {maxHops} hops");
		IntPtr intPtr4 = IcmpCreateFile();
		int destinationAddress = BitConverter.ToInt32(IP.GetAddressBytes(), 0);
		for (byte b = 0; b < maxHops; b++)
		{
			iCMP_OPTIONS.Ttl++;
			Marshal.StructureToPtr((object)iCMP_OPTIONS, intPtr, fDeleteOld: true);
			int num2 = IcmpSendEcho(intPtr4, destinationAddress, intPtr3, (short)array.Length, intPtr, intPtr2, Marshal.SizeOf(typeof(ICMP_ECHO_REPLY)) + array.Length, 5000);
			iCMP_ECHO_REPLY = (ICMP_ECHO_REPLY)Marshal.PtrToStructure(intPtr2, typeof(ICMP_ECHO_REPLY));
			if (num2 > 0)
			{
				logLines.Add($"Roundtrip time {iCMP_ECHO_REPLY.RoundTripTime}ms from [{new IPAddress(BitConverter.GetBytes(iCMP_ECHO_REPLY.Address)).ToString()}]   ");
			}
			else
			{
				logLines.Add($"*   ");
			}
			if (iCMP_ECHO_REPLY.Status == 0)
			{
				break;
			}
			if (iCMP_ECHO_REPLY.Status == 11010)
			{
				num++;
				if (num > 4)
				{
					logLines.Add("Too many timed out requests. Giving up.");
					break;
				}
			}
		}
		IcmpCloseHandle(intPtr4);
		Marshal.FreeHGlobal(intPtr3);
		Marshal.FreeHGlobal(intPtr2);
		Marshal.FreeHGlobal(intPtr);
		logLines.Add($"Traceroute complete.");
	}

	public static List<string> TraceRoute(string destinationAddress)
	{
		lock (logLines)
		{
			logLines.Clear();
			if (string.IsNullOrEmpty(destinationAddress))
			{
				logLines.Add($"TraceRoute failure: null or empty parameter");
				return logLines;
			}
			IPAddress iPAddress;
			try
			{
				iPAddress = IPAddress.Parse(destinationAddress);
			}
			catch (FormatException)
			{
				iPAddress = null;
			}
			if (iPAddress == null)
			{
				try
				{
					iPAddress = Dns.GetHostEntry(new Uri(destinationAddress).Host).AddressList[0];
				}
				catch (Exception ex2)
				{
					iPAddress = null;
					logLines.Add($"DNS problems. Could not resolve destination address. Details {ex2.ToString()}");
					if (ex2.InnerException != null)
					{
						logLines[logLines.Count - 1] = $"{logLines[logLines.Count - 1]}.\nInnerException: {ex2.InnerException.ToString()}";
					}
				}
			}
			if (iPAddress != null)
			{
				try
				{
					TraceRoute(iPAddress, MAX_HOPS);
				}
				catch (Exception ex3)
				{
					logLines.Add("Error occured while performing Trace Route! {0}" + ex3.ToString());
				}
			}
			return logLines;
		}
	}

	public static bool IsCableConnected()
	{
		return IsNetworkCableConnected("EMACB1");
	}
}
