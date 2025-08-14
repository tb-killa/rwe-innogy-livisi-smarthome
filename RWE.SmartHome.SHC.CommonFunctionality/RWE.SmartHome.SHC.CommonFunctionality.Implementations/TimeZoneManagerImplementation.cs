using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.CommonFunctionality.Implementations;

internal class TimeZoneManagerImplementation : ITimeZoneManager
{
	[DllImport("coredll")]
	private static extern bool SetTimeZoneInformation([In] ref TimeZoneInformation lpTimeZoneInformation);

	[DllImport("coredll", CharSet = CharSet.Auto)]
	private static extern int GetTimeZoneInformation(out TimeZoneInformation lpTimeZoneInformation);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool SetSystemTime(ref SystemTime lpSystemTime);

	[DllImport("coredll.dll", CharSet = CharSet.Unicode)]
	private static extern void GetSystemTime(ref SystemTime lpSystemTime);

	public string GetShcTimeZoneName()
	{
		TimeZoneInformation lpTimeZoneInformation;
		if (Environment.OSVersion.Platform == PlatformID.WinCE)
		{
			GetTimeZoneInformation(out lpTimeZoneInformation);
		}
		else
		{
			lpTimeZoneInformation = default(TimeZoneInformation);
		}
		return GetRegistryKey(lpTimeZoneInformation.standardName);
	}

	public bool RefreshTimeZone(string defaultTzName)
	{
		bool result = false;
		string text = defaultTzName;
		string timeZoneName = FilePersistence.TimeZoneName;
		if (!string.IsNullOrEmpty(timeZoneName))
		{
			text = timeZoneName;
		}
		if (GetShcTimeZoneName() != text)
		{
			result = SetRunningTimeZone(text) | SetColdBootTimeZone(text);
		}
		else
		{
			Console.WriteLine("Time zone " + text + " already set. No change needed");
		}
		return result;
	}

	public bool SetTimeZone(string tzname)
	{
		FilePersistence.TimeZoneName = tzname;
		return SetColdBootTimeZone(tzname) | SetRunningTimeZone(tzname);
	}

	public TimeSpan GetShcUtcOffset()
	{
		return TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
	}

	private static bool SetColdBootTimeZone(string timeZone)
	{
		bool result = false;
		if (timeZone != null)
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Time Zones", writable: true);
			if (registryKey != null)
			{
				string text = registryKey.GetValue("Default") as string;
				if (text == null || text != timeZone)
				{
					registryKey.SetValue("Default", timeZone);
					registryKey.Flush();
					result = true;
					Console.WriteLine("Changing cold boot time zone from '{0}' to '{1}'.", text, timeZone);
				}
			}
			else
			{
				Console.WriteLine("WARNING: Time Zones registry key could not be found");
			}
		}
		return result;
	}

	private static bool SetRunningTimeZone(string tzname)
	{
		bool result = false;
		CorrectTimeZoneInfoIfMoskow(tzname);
		RegistryKey registryKey = OpenRegTzInfo(tzname, write: false);
		if (registryKey != null)
		{
			object value = registryKey.GetValue("TZI");
			byte[] array = value as byte[];
			int num = array.Length;
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.Copy(array, 0, intPtr, num);
			REGTZI rEGTZI = (REGTZI)Marshal.PtrToStructure(intPtr, typeof(REGTZI));
			Marshal.FreeHGlobal(intPtr);
			TimeZoneInformation lpTimeZoneInformation = new TimeZoneInformation
			{
				bias = rEGTZI.Bias,
				standardBias = rEGTZI.StandardBias,
				daylightBias = rEGTZI.DaylightBias,
				standardDate = rEGTZI.StandardDate,
				daylightDate = rEGTZI.DaylightDate,
				standardName = (string)registryKey.GetValue("Std"),
				daylightName = (string)registryKey.GetValue("Dlt")
			};
			SystemTime lpSystemTime = default(SystemTime);
			GetSystemTime(ref lpSystemTime);
			if (!SetTimeZoneInformation(ref lpTimeZoneInformation))
			{
				Console.WriteLine("WARNING: Time zone " + tzname + " could not be set");
			}
			Registry.LocalMachine.Flush();
			SetSystemTime(ref lpSystemTime);
			CultureInfo.CurrentCulture.ClearCachedData();
			Ntp.ForceTimeSync();
			Console.WriteLine("RunningTimeZone changed to: " + tzname);
			result = true;
		}
		else
		{
			Console.WriteLine("WARNING: Time zone data for " + tzname + " could not be found in the registry");
		}
		return result;
	}

	private static string GetRegistryKey(string standardName)
	{
		string text;
		if ((text = standardName) != null && text == "Coordinated Universal Time")
		{
			return "UTC";
		}
		return standardName;
	}

	private static RegistryKey OpenRegTzInfo(string tzname, bool write)
	{
		RegistryKey result = null;
		RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Time Zones");
		if (registryKey != null)
		{
			RegistryKey registryKey2 = registryKey.OpenSubKey(tzname, write);
			result = registryKey2;
		}
		else
		{
			Console.WriteLine("WARNING: Time Zones registry key could not be found");
		}
		return result;
	}

	private static void CorrectTimeZoneInfoIfMoskow(string tzname)
	{
		if (!(tzname != "Russian Standard Time"))
		{
			RegistryKey registryKey = OpenRegTzInfo(tzname, write: true);
			if (registryKey != null)
			{
				REGTZI rEGTZI = new REGTZI
				{
					Bias = -180,
					DaylightBias = 0,
					StandardBias = 0,
					DaylightDate = new SystemTime
					{
						wDay = 0,
						wDayOfWeek = 0,
						wHour = 0,
						wMilliseconds = 0,
						wMinute = 0,
						wMonth = 0,
						wSecond = 0,
						wYear = 0
					},
					StandardDate = new SystemTime
					{
						wDay = 0,
						wDayOfWeek = 0,
						wHour = 0,
						wMilliseconds = 0,
						wMinute = 0,
						wMonth = 0,
						wSecond = 0,
						wYear = 0
					}
				};
				int num = Marshal.SizeOf((object)rEGTZI);
				IntPtr intPtr = Marshal.AllocHGlobal(num);
				byte[] array = new byte[num];
				Marshal.StructureToPtr((object)rEGTZI, intPtr, fDeleteOld: true);
				Marshal.Copy(intPtr, array, 0, num);
				Marshal.FreeHGlobal(intPtr);
				registryKey.SetValue("TZI", array, RegistryValueKind.Binary);
			}
			registryKey.Close();
		}
	}
}
