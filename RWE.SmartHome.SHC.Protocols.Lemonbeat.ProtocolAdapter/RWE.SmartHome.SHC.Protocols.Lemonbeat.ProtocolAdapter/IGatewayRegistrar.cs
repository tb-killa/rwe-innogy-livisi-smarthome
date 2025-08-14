using SmartHome.SHC.API.Protocols.Lemonbeat.Gateway;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

public interface IGatewayRegistrar
{
	void RegisterGateway(ILemonbeatGateway gw);
}
