using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.CommonFunctionality;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService;
using WebServerHost.Helpers;
using WebServerHost.Web;

namespace WebServerHost.Controllers.ClientAPI;

[Route("status")]
public class StatusController : Controller
{
	private IShcClient shcClient;

	private IStatusConverterService statusConverter;

	public StatusController(IShcClient shcClient, IStatusConverterService statusConverter)
	{
		this.shcClient = shcClient;
		this.statusConverter = statusConverter;
	}

	[Route("")]
	[HttpGet]
	public IResult Get()
	{
		GetShcStatusRequest request = ShcRequestHelper.NewRequest<GetShcStatusRequest>();
		BaseResponse response = shcClient.GetResponse(request);
		if (response is GetShcStatusResponse getShcStatusResponse)
		{
			Status status = statusConverter.ToApiModel(getShcStatusResponse.Status);
			status.GatewayStatus.Connected = true;
			status.GatewayStatus.SerialNumber = SHCSerialNumber.SerialNumber();
			status.GatewayStatus.ControllerType = "Classic";
			return Ok(status.GatewayStatus);
		}
		return Ok();
	}
}
