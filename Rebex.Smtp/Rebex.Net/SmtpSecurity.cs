using System;
using System.ComponentModel;
using onrkn;

namespace Rebex.Net;

[EditorBrowsable(EditorBrowsableState.Never)]
[wptwl(false)]
[Obsolete("This enum has been deprecated and will be removed. Use SslMode instead.", true)]
public enum SmtpSecurity
{
	Unsecure = 0,
	Implicit = 1,
	Explicit = 2,
	Secure = 2
}
