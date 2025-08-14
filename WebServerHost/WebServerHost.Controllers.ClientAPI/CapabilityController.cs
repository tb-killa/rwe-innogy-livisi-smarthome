using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Control;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService;
using WebServerHost.Helpers;
using WebServerHost.Web;

namespace WebServerHost.Controllers.ClientAPI;

[Route("capability")]
public class CapabilityController : Controller
{
	private const RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType TypeOfEntity = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.LogicalDevice;

	private IShcClient shcClient;

	private ICapabilityConverterService capabilityConverter;

	public CapabilityController(ICapabilityConverterService capabilityConverter, IShcClient shcClient)
	{
		this.capabilityConverter = capabilityConverter;
		this.shcClient = shcClient;
	}

	[Route("")]
	[HttpGet]
	public IEnumerable<Capability> GetCapabilities(string tkey, string tval)
	{
		IEnumerable<Capability> enumerable = GetAllCapabilities();
		if (tkey.IsNotEmptyOrNull() && tval.IsNotEmptyOrNull())
		{
			enumerable = enumerable.Where((Capability capability) => capability.Tags != null && capability.Tags.Any((SmartHome.Common.API.Entities.Entities.Property t) => t.Name == tkey && t.Value.Equals(tval))).ToList();
		}
		return enumerable;
	}

	private IEnumerable<Capability> GetAllCapabilities()
	{
		return (from capabilities in GetAllLogicalDevices()
			select capabilityConverter.FromSmartHomeLogicalDevice(capabilities)).ToList();
	}

	private List<LogicalDevice> GetAllLogicalDevices()
	{
		GetEntitiesRequest getEntitiesRequest = ShcRequestHelper.NewRequest<GetEntitiesRequest>();
		getEntitiesRequest.EntityType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.LogicalDevice;
		BaseResponse response = shcClient.GetResponse(getEntitiesRequest);
		if (response != null)
		{
			if (response is GetEntitiesResponse getEntitiesResponse)
			{
				return getEntitiesResponse.LogicalDevices;
			}
			if (response is ErrorResponse errorResponse)
			{
				throw new ApiException(ErrorHelper.GetError(errorResponse));
			}
		}
		return new List<LogicalDevice>();
	}

	[HttpGet]
	[Route("{id}")]
	public Capability GetCapability([FromRoute] string id)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		Capability capability = GetAllCapabilities().FirstOrDefault((Capability existingCapability) => existingCapability.Id == id);
		if (capability == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Capability not found {id}");
		}
		return capability;
	}

	[Route("{id}")]
	[HttpPut]
	public IResult Update([FromRoute] string id, [FromBody] Capability capability)
	{
		if (capability == null || id != capability.Id)
		{
			throw new ApiException(ErrorCode.EntityMalformedContent, $"Capability Invalid");
		}
		if (GetAllCapabilities().FirstOrDefault((Capability existingCapability) => string.Equals(existingCapability.Id, capability.Id)) == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Capability not found with id: {id}");
		}
		return CreateCapability(capability);
	}

	private IResult CreateCapability(Capability capability)
	{
		SetEntitiesRequest setEntitiesRequest = ShcRequestHelper.NewRequest<SetEntitiesRequest>();
		setEntitiesRequest.LogicalDevices.Add(capabilityConverter.ToSmartHomeLogicalDevice(capability));
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

	[HttpGet]
	[Route("{id}/config")]
	public IEnumerable<SmartHome.Common.API.Entities.Entities.Property> GetConfig([FromRoute] string id, string name)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		Capability capability = GetAllCapabilities().FirstOrDefault((Capability existingCapability) => existingCapability.Id == id);
		if (capability == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Capability not found {id}");
		}
		if (string.IsNullOrEmpty(name))
		{
			return capability.Config;
		}
		return capability.Config.Where((SmartHome.Common.API.Entities.Entities.Property configName) => configName.Name.ToLower() == name.ToLower());
	}

	[Route("{id}/state")]
	[HttpGet]
	public List<SmartHome.Common.API.Entities.Entities.Property> GetState([FromRoute] string id, string name)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var capabilityId))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		LogicalDevice logicalDevice = GetAllLogicalDevices().FirstOrDefault((LogicalDevice existingLogicalDevice) => existingLogicalDevice.Id == capabilityId);
		if (logicalDevice == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Capability not found {id}");
		}
		return GetLogicalDeviceState(logicalDevice);
	}

	private List<SmartHome.Common.API.Entities.Entities.Property> GetLogicalDeviceState(LogicalDevice logicalDevice)
	{
		GetLogicalDeviceStateRequest getLogicalDeviceStateRequest = ShcRequestHelper.NewRequest<GetLogicalDeviceStateRequest>();
		getLogicalDeviceStateRequest.LogicalDeviceId = logicalDevice.Id;
		BaseResponse response = shcClient.GetResponse(getLogicalDeviceStateRequest);
		if (response != null)
		{
			if (response is GetLogicalDeviceStateResponse getLogicalDeviceStateResponse)
			{
				return capabilityConverter.FromSmartHomeLogicalDeviceState(getLogicalDeviceStateResponse.State);
			}
			if (response is ErrorResponse errorResponse)
			{
				throw new ApiException(ErrorHelper.GetError(errorResponse));
			}
		}
		return new List<SmartHome.Common.API.Entities.Entities.Property>();
	}

	[HttpGet]
	[Route("{id}/tag")]
	public IEnumerable<SmartHome.Common.API.Entities.Entities.Property> GetTag([FromRoute] string id, string name)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		Capability capability = GetAllCapabilities().FirstOrDefault((Capability existingCapability) => existingCapability.Id == id);
		if (capability == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Capability not found {id}");
		}
		if (capability.Tags == null)
		{
			return new List<SmartHome.Common.API.Entities.Entities.Property>();
		}
		if (string.IsNullOrEmpty(name))
		{
			return capability.Tags;
		}
		return capability.Tags.Where((SmartHome.Common.API.Entities.Entities.Property tagName) => tagName.Name.EqualsIgnoreCase(name));
	}

	[HttpGet]
	[Route("states")]
	public List<EntityState> GetStates()
	{
		List<EntityState> list = new List<EntityState>();
		GetAllLogicalDeviceStatesRequest request = ShcRequestHelper.NewRequest<GetAllLogicalDeviceStatesRequest>();
		BaseResponse response = shcClient.GetResponse(request);
		if (response != null && response is GetAllLogicalDeviceStatesResponse getAllLogicalDeviceStatesResponse)
		{
			foreach (LogicalDeviceState state in getAllLogicalDeviceStatesResponse.States)
			{
				list.Add(new EntityState
				{
					Id = state.LogicalDeviceId.ToString("N"),
					State = capabilityConverter.FromSmartHomeLogicalDeviceState(state)
				});
			}
		}
		return list;
	}
}
