using Org.Mentalis.Security.Certificates;

namespace Org.Mentalis.Security.Ssl;

public delegate void CertVerifyEventHandler(SecureSocket socket, Certificate remote, CertificateChain chain, VerifyEventArgs e);
