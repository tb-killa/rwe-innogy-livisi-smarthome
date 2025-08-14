using System.ComponentModel;
using onrkn;

namespace Rebex.Net;

[EditorBrowsable(EditorBrowsableState.Never)]
[wptwl(false)]
public enum TlsDebugEventGroup
{
	Info,
	HandshakeMessage,
	Alert,
	StateChange,
	CipherSpec
}
