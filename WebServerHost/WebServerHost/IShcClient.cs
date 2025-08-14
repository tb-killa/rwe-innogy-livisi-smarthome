using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;

namespace WebServerHost;

public interface IShcClient
{
	BaseResponse GetResponse(BaseRequest request);
}
