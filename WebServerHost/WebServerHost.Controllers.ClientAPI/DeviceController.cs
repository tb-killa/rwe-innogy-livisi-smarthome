using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.DeviceState;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.DeviceState;
using SmartHome.Common.API.Entities.Entities;
using SmartHome.Common.API.Entities.ErrorHandling;
using SmartHome.Common.API.Entities.Extensions;
using SmartHome.Common.API.ModelTransformationService;
using WebServerHost.Helpers;
using WebServerHost.Web;

namespace WebServerHost.Controllers.ClientAPI;

[Route("device")]
public class DeviceController : Controller
{
	private const RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType BaseDeviceEntityType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.BaseDevice;

	private const RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType LogicalDeviceEntityType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.LogicalDevice;

	private IShcClient shcClient;

	private IDeviceConverterService deviceConverter;

	private ICapabilityConverterService capabilityConverter;

	public DeviceController(IDeviceConverterService deviceConverter, ICapabilityConverterService capabilityConverter, IShcClient shcClient)
	{
		this.deviceConverter = deviceConverter;
		this.capabilityConverter = capabilityConverter;
		this.shcClient = shcClient;
	}

	[Route("")]
	[HttpGet]
	public IEnumerable<Device> GetDevices(string tkey, string tval)
	{
		IEnumerable<Device> enumerable = GetAllDevices();
		if (tkey.IsNotEmptyOrNull() && tval.IsNotEmptyOrNull())
		{
			enumerable = enumerable.Where((Device device) => device.Tags != null && device.Tags.Any((SmartHome.Common.API.Entities.Entities.Property t) => t.Name == tkey && t.Value.Equals(tval))).ToList();
		}
		return enumerable;
	}

	[HttpGet]
	[Route("{id}")]
	public Device GetDevice([FromRoute] string id)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		Device device = GetAllDevices().FirstOrDefault((Device baseDevice) => baseDevice.Id == id);
		if (device == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Device not found {id}");
		}
		return device;
	}

	[Route("")]
	[HttpPost]
	public IResult Create([FromBody] Device device)
	{
		if (device == null)
		{
			throw new ApiException(ErrorCode.EntityMalformedContent, $"Device Invalid");
		}
		if (GetAllDevices().FirstOrDefault((Device existingDevice) => string.Equals(existingDevice.Id, device.Id)) != null)
		{
			throw new ApiException(ErrorCode.EntityAlreadyExists, $"Device aleready exists with id: {device.Id}");
		}
		return CreateDevice(device);
	}

	[Route("{id}")]
	[HttpPut]
	public IResult Update([FromRoute] string id, [FromBody] Device device)
	{
		if (device == null || id != device.Id)
		{
			throw new ApiException(ErrorCode.EntityMalformedContent, $"Device Invalid");
		}
		if (GetAllDevices().FirstOrDefault((Device existingDevice) => string.Equals(existingDevice.Id, device.Id)) == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Device not found with id: {id}");
		}
		return CreateDevice(device);
	}

	[Route("{id}")]
	[HttpDelete]
	public IResult Delete([FromRoute] string id)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		if (GetAllDevices().FirstOrDefault((Device existingDevice) => existingDevice.Id == id) == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Device not found with id: {id}");
		}
		DeleteEntitiesRequest deleteEntitiesRequest = ShcRequestHelper.NewRequest<DeleteEntitiesRequest>();
		deleteEntitiesRequest.Entities.Add(new EntityMetadata
		{
			EntityType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.BaseDevice,
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

	[HttpGet]
	[Route("{id}/devices")]
	public IEnumerable<string> GetLinkedDevices([FromRoute] string id)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		Device device = GetAllDevices().FirstOrDefault((Device existingDevice) => existingDevice.Id == id);
		if (device == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Device not found with id: {id}");
		}
		if (device.Devices == null)
		{
			return new List<string>();
		}
		return device.Devices;
	}

	[Route("{id}/capabilities")]
	[HttpGet]
	public IEnumerable<Capability> GetCapabilities([FromRoute] string id)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var guidId))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		Device device = GetAllDevices().FirstOrDefault((Device existingDevice) => existingDevice.Id == id);
		if (device == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Device not found with id: {id}");
		}
		GetEntitiesRequest getEntitiesRequest = ShcRequestHelper.NewRequest<GetEntitiesRequest>();
		getEntitiesRequest.EntityType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.LogicalDevice;
		BaseResponse response = shcClient.GetResponse(getEntitiesRequest);
		if (response != null)
		{
			if (response is GetEntitiesResponse getEntitiesResponse)
			{
				return (from logicalDevices in getEntitiesResponse.LogicalDevices
					where logicalDevices.BaseDeviceId == guidId
					select capabilityConverter.FromSmartHomeLogicalDevice(logicalDevices)).ToList();
			}
			if (response is ErrorResponse errorResponse)
			{
				throw new ApiException(ErrorHelper.GetError(errorResponse));
			}
		}
		return new List<Capability>();
	}

	[Route("{id}/capability/state")]
	[HttpGet]
	public EntityState GetCapabilitiesState([FromRoute] string id)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var guidId))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		BaseDevice baseDevice = GetAllBaseDevices().FirstOrDefault((BaseDevice existingDevice) => existingDevice.Id == guidId);
		if (baseDevice == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Device not found with id: {id}");
		}
		EntityState entityState = new EntityState();
		List<SmartHome.Common.API.Entities.Entities.Property> list = new List<SmartHome.Common.API.Entities.Entities.Property>();
		GetAllLogicalDeviceStatesRequest getAllLogicalDeviceStatesRequest = ShcRequestHelper.NewRequest<GetAllLogicalDeviceStatesRequest>();
		List<string> deviceIds = baseDevice.LogicalDeviceIds.ConvertAll((Guid c) => c.ToString("N"));
		getAllLogicalDeviceStatesRequest.DeviceIds = deviceIds;
		BaseResponse response = shcClient.GetResponse(getAllLogicalDeviceStatesRequest);
		if (response != null && response is GetAllLogicalDeviceStatesResponse getAllLogicalDeviceStatesResponse)
		{
			foreach (LogicalDeviceState state in getAllLogicalDeviceStatesResponse.States)
			{
				list.AddRange(capabilityConverter.FromSmartHomeLogicalDeviceState(state));
			}
			entityState.Id = baseDevice.Id.ToString();
			entityState.State = list;
			return entityState;
		}
		return entityState;
	}

	[HttpGet]
	[Route("{id}/config")]
	public List<SmartHome.Common.API.Entities.Entities.Property> GetDeviceConfig([FromRoute] string id, string name)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		Device device = GetAllDevices().FirstOrDefault((Device existingDevice) => existingDevice.Id == id);
		if (device == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Device not found with id: {id}");
		}
		if (device.Config == null)
		{
			return new List<SmartHome.Common.API.Entities.Entities.Property>();
		}
		if (string.IsNullOrEmpty(name))
		{
			return device.Config;
		}
		return device.Config.Where((SmartHome.Common.API.Entities.Entities.Property configName) => configName.Name.ToLower() == name.ToLower()).ToList();
	}

	[HttpGet]
	[Route("{id}/tag")]
	public List<SmartHome.Common.API.Entities.Entities.Property> GetDeviceTag([FromRoute] string id, string name)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		Device device = GetAllDevices().FirstOrDefault((Device existingDevice) => existingDevice.Id == id);
		if (device == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Device not found with id: {id}");
		}
		if (device.Tags == null)
		{
			return new List<SmartHome.Common.API.Entities.Entities.Property>();
		}
		if (string.IsNullOrEmpty(name))
		{
			return device.Tags;
		}
		return device.Tags.Where((SmartHome.Common.API.Entities.Entities.Property tagName) => tagName.Name.ToLower() == name.ToLower()).ToList();
	}

	[Route("{id}/state")]
	[HttpGet]
	public EntityState GetDeviceState([FromRoute] string id, string name)
	{
		if (id.IsNullOrEmpty() || !id.GuidTryParse(out var _))
		{
			throw new ApiException(ErrorCode.InvalidArgument, $"The value '{id}' is not valid");
		}
		Device device = GetAllDevices().FirstOrDefault((Device existingDevice) => existingDevice.Id == id);
		if (device == null)
		{
			throw new ApiException(ErrorCode.EntityDoesNotExist, $"Device not found with id: {id}");
		}
		GetPhysicalDeviceStateRequest getPhysicalDeviceStateRequest = ShcRequestHelper.NewRequest<GetPhysicalDeviceStateRequest>();
		getPhysicalDeviceStateRequest.PhysicalDeviceId = device.Id.ToGuid();
		BaseResponse response = shcClient.GetResponse(getPhysicalDeviceStateRequest);
		if (response != null && response is GetPhysicalDeviceStateResponse getPhysicalDeviceStateResponse)
		{
			List<SmartHome.Common.API.Entities.Entities.Property> state = deviceConverter.FromSmartHomeDeviceState(getPhysicalDeviceStateResponse.DeviceState);
			EntityState entityState = new EntityState();
			entityState.Id = id;
			entityState.State = state;
			return entityState;
		}
		return null;
	}

	[Route("states")]
	[HttpGet]
	public List<EntityState> GetAllDeviceStates()
	{
		new EntityState();
		List<EntityState> list = new List<EntityState>();
		GetAllPhysicalDeviceStatesRequest request = ShcRequestHelper.NewRequest<GetAllPhysicalDeviceStatesRequest>();
		BaseResponse response = shcClient.GetResponse(request);
		if (response != null && response is GetAllPhysicalDeviceStatesResponse getAllPhysicalDeviceStatesResponse)
		{
			foreach (PhysicalDeviceState deviceState in getAllPhysicalDeviceStatesResponse.DeviceStates)
			{
				List<SmartHome.Common.API.Entities.Entities.Property> list2 = new List<SmartHome.Common.API.Entities.Entities.Property>();
				list2.AddRange(deviceConverter.FromSmartHomeDeviceState(deviceState));
				list.Add(new EntityState
				{
					Id = deviceState.PhysicalDeviceId.ToString("N"),
					State = list2
				});
			}
		}
		return list;
	}

	private IEnumerable<Device> GetAllDevices()
	{
		return from devices in GetAllBaseDevices()
			select deviceConverter.FromSmartHomeBaseDevice(devices, includeCapabilities: true);
	}

	private IEnumerable<BaseDevice> GetAllBaseDevices()
	{
		GetEntitiesRequest getEntitiesRequest = ShcRequestHelper.NewRequest<GetEntitiesRequest>();
		getEntitiesRequest.EntityType = RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.EntityType.BaseDevice;
		BaseResponse response = shcClient.GetResponse(getEntitiesRequest);
		if (response != null)
		{
			if (response is GetEntitiesResponse getEntitiesResponse)
			{
				return getEntitiesResponse.BaseDevices;
			}
			if (response is ErrorResponse errorResponse)
			{
				throw new ApiException(ErrorHelper.GetError(errorResponse));
			}
		}
		return new List<BaseDevice>();
	}

	private IResult CreateDevice(Device device)
	{
		SetEntitiesRequest setEntitiesRequest = ShcRequestHelper.NewRequest<SetEntitiesRequest>();
		setEntitiesRequest.BaseDevices.Add(deviceConverter.ToSmartHomeBaseDevice(device));
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
}
