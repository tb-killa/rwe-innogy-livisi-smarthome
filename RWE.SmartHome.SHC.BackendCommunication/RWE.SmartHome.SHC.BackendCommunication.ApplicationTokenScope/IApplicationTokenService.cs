using System.CodeDom.Compiler;

namespace RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public interface IApplicationTokenService
{
	ApplicationsToken GetApplicationToken();

	string GetApplicationTokenHash();
}
