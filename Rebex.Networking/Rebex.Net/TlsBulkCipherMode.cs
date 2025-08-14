using System;
using System.ComponentModel;
using onrkn;

namespace Rebex.Net;

public enum TlsBulkCipherMode
{
	None = 0,
	CBC = 1,
	Stream = 2,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("TlsBulkCipherMode.GCM has been superseded by TlsBulkCipherMode.AEAD.", false)]
	[wptwl(false)]
	GCM = 4,
	AEAD = 4
}
