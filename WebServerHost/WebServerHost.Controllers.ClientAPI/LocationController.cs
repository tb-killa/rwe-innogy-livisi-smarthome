using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Configuration;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService;
using WebServerHost.Helpers;
using WebServerHost.Web;

namespace WebServerHost.Controllers.ClientAPI;

[Route("location")]
public class LocationController : Controller
{
	private const RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType TypeOfEntity = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Location;

	private ILocationConverterService locationConverter;

	private IShcClient shcClient;

	public LocationController(ILocationConverterService locationConverter, IShcClient shcClient)
	{
		this.locationConverter = locationConverter;
		this.shcClient = shcClient;
	}

	[Route("")]
	[HttpGet]
	public IEnumerable<SmartHome.Common.API.Entities.Entities.Location> GetLocations(string tkey, string tval)
	{
		IEnumerable<SmartHome.Common.API.Entities.Entities.Location> enumerable = GetAllLocations();
		if (tkey.IsNotEmptyOrNull() && tval.IsNotEmptyOrNull())
		{
			enumerable = enumerable.Where((SmartHome.Common.API.Entities.Entities.Location l) => l.Tags != null && l.Tags.Any((SmartHome.Common.API.Entities.Entities.Property t) => t.Name == tkey && t.Value.Equals(tval)));
		}
		return enumerable;
	}

	private IEnumerable<SmartHome.Common.API.Entities.Entities.Location> GetAllLocations()
	{
		GetEntitiesRequest getEntitiesRequest = ShcRequestHelper.NewRequest<GetEntitiesRequest>();
		getEntitiesRequest.EntityType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Location;
		BaseResponse response = shcClient.GetResponse(getEntitiesRequest);
		if (response != null)
		{
			if (response is GetEntitiesResponse getEntitiesResponse)
			{
				return getEntitiesResponse.Locations.Select((RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots.Location l) => locationConverter.FromSmartHomeLocation(l));
			}
			if (response is ErrorResponse errorResponse)
			{
				throw new ApiException(ErrorHelper.GetError(errorResponse));
			}
		}
		return new List<SmartHome.Common.API.Entities.Entities.Location>();
	}

	[HttpGet]
	[Route("{id}")]
	public SmartHome.Common.API.Entities.Entities.Location Get([FromRoute] string id)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		SmartHome.Common.API.Entities.Entities.Location location = GetLocations(null, null).FirstOrDefault((SmartHome.Common.API.Entities.Entities.Location l) => l.Id == id);
		if (location == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Location not found {id}");
		}
		return location;
	}

	[HttpPost]
	[Route("")]
	public IResult Create([FromBody] SmartHome.Common.API.Entities.Entities.Location location)
	{
		if (location == null)
		{
			throw new ApiException(ErrorCode.EntityMalformedContent, $"Location Invalid");
		}
		if (GetLocations(null, null).FirstOrDefault((SmartHome.Common.API.Entities.Entities.Location l) => l.Id == location.Id) != null)
		{
			throw new ApiException(ErrorCode.EntityAlreadyExists, $"Location aleready exists {location.Id}");
		}
		return SetLocation(location);
	}

	private IResult SetLocation(SmartHome.Common.API.Entities.Entities.Location location)
	{
		SetEntitiesRequest setEntitiesRequest = ShcRequestHelper.NewRequest<SetEntitiesRequest>();
		setEntitiesRequest.Locations.Add(locationConverter.ToSmartHomeLocation(location));
		BaseResponse response = shcClient.GetResponse(setEntitiesRequest);
		if (response != null)
		{
			if (response is AcknowledgeResponse)
			{
				return Ok();
			}
			if (response is ErrorResponse errorResponse)
			{
				throw new ApiException(ErrorHelper.GetError(errorResponse));
			}
		}
		throw new ApiException(ErrorCode.InternalError, "Unkown Error");
	}

	[Route("{id}")]
	[HttpPut]
	public IResult Update([FromRoute] string id, [FromBody] SmartHome.Common.API.Entities.Entities.Location location)
	{
		if (location == null || id != location.Id)
		{
			throw new ApiException(ErrorCode.EntityMalformedContent, $"Location Invalid");
		}
		if (GetLocations(null, null).FirstOrDefault((SmartHome.Common.API.Entities.Entities.Location l) => l.Id == location.Id) == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Location not found {id}");
		}
		return SetLocation(location);
	}

	[Route("{id}")]
	[HttpDelete]
	public IResult Delete([FromRoute] string id)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		if (GetLocations(null, null).FirstOrDefault((SmartHome.Common.API.Entities.Entities.Location l) => l.Id == id) == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Location not found {id}");
		}
		DeleteEntitiesRequest deleteEntitiesRequest = ShcRequestHelper.NewRequest<DeleteEntitiesRequest>();
		deleteEntitiesRequest.Entities.Add(new EntityMetadata
		{
			EntityType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.Location,
			Id = id.ToGuid()
		});
		BaseResponse response = shcClient.GetResponse(deleteEntitiesRequest);
		if (response != null)
		{
			if (response is AcknowledgeResponse)
			{
				return Ok();
			}
			if (response is ErrorResponse errorResponse)
			{
				throw new ApiException(ErrorHelper.GetError(errorResponse));
			}
		}
		throw new ApiException(ErrorCode.InternalError, "Unkown Error");
	}
}
