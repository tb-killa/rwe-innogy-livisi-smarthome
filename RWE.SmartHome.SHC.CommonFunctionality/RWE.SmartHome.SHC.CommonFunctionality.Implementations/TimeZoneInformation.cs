using System.Runtime.InteropServices;

namespace RWE.SmartHome.SHC.CommonFunctionality.Implementations;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct TimeZoneInformation
{
	public int bias;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string standardName;

	public SystemTime standardDate;

	public int standardBias;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string daylightName;

	public SystemTime daylightDate;

	public int daylightBias;
}
