using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.ModelTransformationService.ActionConverters;
using SmartHome.Common.Generic.LogManager;

namespace SmartHome.Common.API.ModelTransformationService;

public class ActionConverterService : IActionConverterService
{
	private readonly ILogger logger = LogManager.Instance.GetLogger(typeof(ActionConverterService));

	private static readonly IActionConverterFactory actionConverterFactory;

	private readonly IConversionContext context;

	static ActionConverterService()
	{
		actionConverterFactory = new ActionConverterFactory();
	}

	public ActionConverterService(IConversionContext context)
	{
		this.context = context;
	}

	public BaseRequest ToBaseRequest(Action anAction)
	{
		IActionConverter actionConverter = actionConverterFactory.GetActionConverter(anAction);
		logger.Debug($"Converting API Action with Type: {anAction.Type}");
		return actionConverter.ToBaseRequest(anAction, context);
	}

	public SmartHome.Common.API.Entities.Entities.ActionResponse FromBaseResponse(Action anAction, BaseResponse aResponse)
	{
		IActionConverter actionConverter = actionConverterFactory.GetActionConverter(anAction);
		logger.Debug($"Converting SH BaseResponse with Type: {aResponse.GetType()}");
		return actionConverter.FromBaseResponse(anAction, aResponse, context);
	}
}
