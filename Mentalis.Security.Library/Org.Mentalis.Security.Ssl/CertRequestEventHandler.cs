using Org.Mentalis.Security.Certificates;

namespace Org.Mentalis.Security.Ssl;

public delegate void CertRequestEventHandler(SecureSocket socket, DistinguishedNameList acceptable, RequestEventArgs e);
