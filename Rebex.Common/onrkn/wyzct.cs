using System.Runtime.InteropServices;

namespace onrkn;

internal struct wyzct
{
	[MarshalAs(UnmanagedType.LPWStr)]
	internal string zsykd;

	internal int ivbny;

	[MarshalAs(UnmanagedType.LPWStr)]
	internal string nipxo;

	internal int eilok;

	[MarshalAs(UnmanagedType.LPWStr)]
	internal string lntpu;

	internal int thrts;

	internal int auatq;

	internal wyzct(string userName, string password, string domain)
	{
		zsykd = userName;
		ivbny = ((userName != null && 0 == 0) ? userName.Length : 0);
		lntpu = password;
		thrts = ((password != null && 0 == 0) ? password.Length : 0);
		nipxo = domain;
		eilok = ((domain != null && 0 == 0) ? domain.Length : 0);
		auatq = 2;
	}
}
