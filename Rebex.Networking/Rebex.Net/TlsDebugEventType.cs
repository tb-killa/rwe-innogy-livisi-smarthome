using System.ComponentModel;
using onrkn;

namespace Rebex.Net;

[wptwl(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public enum TlsDebugEventType
{
	ResumingCachedSession = 1,
	UnknownMessageType = 2,
	UnexpectedException = 4,
	HelloRequest = 69632,
	ClientHello = 69633,
	ServerHello = 69634,
	Certificate = 69643,
	ServerKeyExchange = 69644,
	CertificateRequest = 69645,
	ServerHelloDone = 69646,
	CertificateVerify = 69647,
	ClientKeyExchange = 69648,
	Finished = 69652,
	UnknownHandshakeMessage = 73727,
	Alert = 131073,
	Negotiating = 200705,
	Secured = 200706,
	Closed = 200707,
	ChangeCipherSpec = 262145
}
