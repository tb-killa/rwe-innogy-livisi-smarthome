using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService;
using WebServerHost.Web;

namespace WebServerHost.Controllers.ClientAPI;

[Route("action")]
public class ActionController : Controller
{
	private readonly IShcClient shcClient;

	private readonly IActionConverterService actionConverter;

	public ActionController(IActionConverterService actionConverter, IShcClient shcClient)
	{
		this.actionConverter = actionConverter;
		this.shcClient = shcClient;
	}

	[Route("{id}")]
	[HttpPost]
	public SmartHome.Common.API.Entities.Entities.ActionResponse Execute([FromRoute] string id, [FromBody] Action action)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		return Execute(action);
	}

	[HttpPost]
	[Route("")]
	public SmartHome.Common.API.Entities.Entities.ActionResponse Execute([FromBody] Action action)
	{
		if (action == null)
		{
			throw new ApiException(ErrorCode.EntityMalformedContent, $"Action Invalid");
		}
		BaseRequest request = actionConverter.ToBaseRequest(action);
		BaseResponse response = shcClient.GetResponse(request);
		return actionConverter.FromBaseResponse(action, response);
	}
}
