using System;
using System.ComponentModel;
using onrkn;

namespace Rebex.Net;

[Flags]
[wptwl(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public enum SmtpOptions
{
	None = 0,
	AllowNullSender = 1,
	SendWithNoBuffer = 2
}
