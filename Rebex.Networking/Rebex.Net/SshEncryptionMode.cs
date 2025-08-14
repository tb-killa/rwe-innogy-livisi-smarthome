using System;
using System.ComponentModel;
using onrkn;

namespace Rebex.Net;

[Flags]
public enum SshEncryptionMode
{
	None = 0,
	CBC = 1,
	CTR = 2,
	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("SshEncryptionMode.GCM has been superseded by SshEncryptionMode.AEAD.", false)]
	GCM = 4,
	AEAD = 4,
	Any = 7
}
