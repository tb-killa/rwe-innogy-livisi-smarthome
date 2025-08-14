namespace Org.Mentalis.Security.Ssl.Shared;

internal enum ContentType : byte
{
	ChangeCipherSpec = 20,
	Alert,
	Handshake,
	ApplicationData
}
