using System;

namespace onrkn;

internal static class hifyx
{
	public const string xhtrf = "Unable to asynchronously call non-existent method {0}.";

	public const string iaxsl = "The IAsyncResult object supplied to End{0} was not returned from the corresponding Begin{0} method on this class.";

	public const string rdgvt = "End{0} can only be called once for each asynchronous operation.";

	public const string paaln = "Another asynchronous operation is pending.";

	public const string cchzq = "Asynchronous call exception.";

	public const string klbuc = "Collection already contains item '{0}'.";

	public const string zvjgk = "Collection does not contain the item.";

	public const string mlwwv = "Illegal characters in mask.";

	public const string xydqk = "Invalid argument.";

	public const string udgot = "Buffer cannot be null.";

	public const string viabq = "Socket cannot be null.";

	public const string rsugg = "Path cannot be null.";

	public const string waale = "Pattern cannot be null.";

	public const string nmkia = "Extension cannot be null.";

	public const string uisxj = "Mask cannot be null.";

	public const string abshe = "Stream cannot be null.";

	public const string aqpnz = "String cannot be null.";

	public const string rfmtk = "Hostname cannot be null.";

	public const string vthth = "Collection cannot be null.";

	public const string qeubq = "Value cannot be null.";

	public const string upxwc = "Set cannot be null.";

	public const string gxaip = "Password cannot be null.";

	public const string mncvc = "Item name cannot be null.";

	public const string nhsot = "Path cannot be empty.";

	public const string taadt = "Pattern cannot be empty.";

	public const string mkkug = "Extension cannot be empty.";

	public const string rxttl = "Mask cannot be empty.";

	public const string ktmjy = "Stream cannot be empty.";

	public const string bxlms = "String cannot have zero length.";

	public const string ubuim = "Hostname cannot be empty.";

	public const string jnggu = "Message set cannot be empty.";

	public const string dolob = "Collection cannot be empty.";

	public const string mngkc = "Password cannot be empty.";

	public const string vxydh = "Item name cannot be empty.";

	public const string efxxm = "Invalid path.";

	public const string qkbkb = "Invalid path '{0}'.";

	public const string gfvos = "Hostname is invalid.";

	public const string flrtp = "Invalid offset.";

	public const string quyfi = "Invalid count.";

	public const string peiqp = "Count is greater than the number of elements from index to the end of the array.";

	public const string wyqib = "Invalid length.";

	public const string emnhw = "Argument must not be negative number.";

	public const string hxftm = "Argument must be positive number.";

	public const string qmuqx = "Argument is out of range of valid values.";

	public const string kusie = "Offset is out of range of valid values.";

	public const string mexwl = "Count is out of range of valid values.";

	public const string thhto = "Index is out of the bounds of an array.";

	public const string xuqor = "Timeout is out of range of valid values.";

	public const string iijni = "Port is out of range of valid values.";

	public const string jzycu = "Unsupported key size.";

	public const string mayzp = "Command has zero length.";

	public const string iwijo = "Illegal characters in path.";

	public const string vflug = "Illegal use of wildcards in path.";

	public const string pfxvj = "Unsupported credential type.";

	public const string gstdp = "Prompt and echo arrays must be of same size.";

	public const string axoij = "Ambiguous usage of path and mode.";

	public const string bnury = "Unsupported option.";

	public const string rztoz = "Supplied type does not provide a suitable API.";

	public const string glnyc = "Local path is not a file path or you do not have permission to access it.";

	public const string wubjx = "Local path is not a file path or a part of the path doesn't exists.";

	public const string fcsms = "Local path is not a file but a directory.";

	public const string xmqkm = "Stream is not seekable.";

	public const string jussk = "Stream is not writable.";

	public const string jdujb = "Stream is not readable.";

	public const string gsjrm = "Operation was canceled.";

	public const string swxao = "Another operation is pending.";

	public const string jegfh = "Invalid OAuth response.";

	public const string aryew = "No IP address records found in the host entry.";

	public const string iqdjz = "This component has been compiled for {0}. Please use assemblies compiled for your platform.";

	public const string wpdxw = "This component has been compiled for .NET Compact Framework 3.5 or higher, but you are trying to use it in .NET Compact Framework 2.0 or lower. Please use assemblies compiled for your platform.";

	public const string dqdyq = "This component has been compiled for .NET Compact Framework 2.0, but you are trying to use it in .NET Compact Framework 3.9. Please use assemblies compiled for your platform.";

	public const string vctlx = "SSPI authentication is not supported on non-Windows systems.";

	public const string noqaf = "Password is wrong.";

	public const string mjmiw = "Single sign-on authentication is not supported on non-Windows systems.";

	public const string lflfl = "This method is no longer supported.";

	public const string bazpq = "Method is not supported on this platform.";

	public const string bcmvo = "The operation has timed out.";

	private const string ligyu = "Trial version of Rebex {0}{1} has expired. To continue using it, please purchase a license at https://www.rebex.net/shop.";

	public static string jpgac(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			return brgjd.edcru("Trial version of Rebex {0}{1} has expired. To continue using it, please purchase a license at https://www.rebex.net/shop.", "component", string.Empty);
		}
		return brgjd.edcru("Trial version of Rebex {0}{1} has expired. To continue using it, please purchase a license at https://www.rebex.net/shop.", p0, " for .NET");
	}

	public static ArgumentOutOfRangeException nztrs(string p0, object p1, string p2)
	{
		return new ArgumentOutOfRangeException(p0, p2);
	}
}
