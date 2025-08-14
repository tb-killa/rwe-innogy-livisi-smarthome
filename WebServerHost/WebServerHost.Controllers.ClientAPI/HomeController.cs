using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Configuration;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.ModelTransformationService;
using WebServerHost.Helpers;
using WebServerHost.Web;

namespace WebServerHost.Controllers.ClientAPI;

[Route("home")]
public class HomeController : Controller
{
	private IShcClient shcClient;

	private IHomeConverterService homeConverter;

	private IHomeSetupConverterService homeSetupConverter;

	public HomeController(IShcClient shcClient, IHomeConverterService homeConverter, IHomeSetupConverterService homeSetupConverter)
	{
		this.shcClient = shcClient;
		this.homeConverter = homeConverter;
		this.homeSetupConverter = homeSetupConverter;
	}

	[Route("")]
	[HttpGet]
	public IResult Get()
	{
		GetEntitiesRequest getEntitiesRequest = ShcRequestHelper.NewRequest<GetEntitiesRequest>();
		getEntitiesRequest.EntityType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Home;
		BaseResponse response = shcClient.GetResponse(getEntitiesRequest);
		if (response is GetEntitiesResponse getEntitiesResponse)
		{
			if (getEntitiesResponse.Homes.Any())
			{
				Home data = homeConverter.ToApiEntity(getEntitiesResponse.Homes.First());
				return Ok(data);
			}
			return Ok(null);
		}
		if (response is ErrorResponse errorResponse)
		{
			throw new ApiException(ErrorHelper.GetError(errorResponse));
		}
		throw new ApiException(ErrorCode.InternalError);
	}

	[HttpPut]
	[Route("{id}")]
	public IResult UpdateHome([FromBody] Home home)
	{
		if (home == null)
		{
			throw new ApiException(ErrorCode.EntityMalformedContent);
		}
		SetEntitiesRequest setEntitiesRequest = ShcRequestHelper.NewRequest<SetEntitiesRequest>();
		setEntitiesRequest.Homes.Add(homeConverter.ToShcEntity(home));
		BaseResponse response = shcClient.GetResponse(setEntitiesRequest);
		if (response is AcknowledgeResponse)
		{
			return Ok();
		}
		if (response is ErrorResponse errorResponse)
		{
			throw new ApiException(ErrorHelper.GetError(errorResponse));
		}
		throw new ApiException(ErrorCode.InternalError, "Unkown Error");
	}

	[HttpGet]
	[Route("setup")]
	public IResult GetHomeSteup()
	{
		GetEntitiesRequest getEntitiesRequest = ShcRequestHelper.NewRequest<GetEntitiesRequest>();
		getEntitiesRequest.EntityType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.HomeSetup;
		BaseResponse response = shcClient.GetResponse(getEntitiesRequest);
		if (response is GetEntitiesResponse getEntitiesResponse)
		{
			if (getEntitiesResponse.HomeSetups.Any())
			{
				HomeSetup data = homeSetupConverter.ToApiEntity(getEntitiesResponse.HomeSetups.First());
				return Ok(data);
			}
			return Ok(new object());
		}
		if (response is ErrorResponse errorResponse)
		{
			throw new ApiException(ErrorHelper.GetError(errorResponse));
		}
		throw new ApiException(ErrorCode.InternalError);
	}

	[HttpPut]
	[Route("setup/{id}")]
	public IResult UpdateHomeSetup([FromBody] HomeSetup homeSetup)
	{
		if (homeSetup == null)
		{
			throw new ApiException(ErrorCode.EntityMalformedContent);
		}
		SetEntitiesRequest setEntitiesRequest = ShcRequestHelper.NewRequest<SetEntitiesRequest>();
		setEntitiesRequest.HomeSetups.Add(homeSetupConverter.ToShcEntity(homeSetup));
		BaseResponse response = shcClient.GetResponse(setEntitiesRequest);
		if (response is AcknowledgeResponse)
		{
			return Ok();
		}
		if (response is ErrorResponse errorResponse)
		{
			throw new ApiException(ErrorHelper.GetError(errorResponse));
		}
		throw new ApiException(ErrorCode.InternalError);
	}
}
