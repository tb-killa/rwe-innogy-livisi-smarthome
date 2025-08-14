using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeleteEntitiesRequestValidation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceInclusion;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.ExternalCommandDispatcher.Helpers.ISR2Calibration;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

internal class RepositoryOperationsWrapper
{
	private readonly IRepository repository;

	private readonly IRepositorySync repositorySync;

	private readonly ISetEntitiesRequestValidator setEntitiesRequestValidator;

	private readonly IDeleteEntitiesRequestValidator deleteEntitiesRequestValidator;

	private readonly IDiscoveryController discoveryController;

	public RepositoryOperationsWrapper(IRepository repository, IRepositorySync repositorySync, IDiscoveryController discoveryController, ISetEntitiesRequestValidator setEntitiesRequestValidator, IDeleteEntitiesRequestValidator deleteEntitiesRequestValidator)
	{
		this.repository = repository;
		this.repositorySync = repositorySync;
		this.discoveryController = discoveryController;
		this.setEntitiesRequestValidator = setEntitiesRequestValidator;
		this.deleteEntitiesRequestValidator = deleteEntitiesRequestValidator;
	}

	public int GetRepositoryVersion()
	{
		return repository.RepositoryVersion;
	}

	private BaseResponse PerformSyncedRepositoryOperation(Guid requestId, Func<BaseResponse> operation)
	{
		RepositoryLockContext repositoryLockContext = null;
		BaseResponse baseResponse;
		try
		{
			repositoryLockContext = repositorySync.GetLockAsyncRelease("PerformSyncedRepositoryOperation", new RepositoryUpdateContextData(CoreConstants.CoreAppId));
			baseResponse = operation();
			repositoryLockContext.Commit = baseResponse is AcknowledgeResponse;
		}
		catch (RepositoryLockedException)
		{
			Log.Error(Module.ExternalCommandDispatcher, "SetEntities request handling failed: configuration locked.");
			baseResponse = new ErrorResponse(requestId, ErrorResponseType.ConfigurationAccessError);
		}
		finally
		{
			repositoryLockContext?.Dispose();
		}
		return baseResponse;
	}

	public BaseResponse SetEntitiesRequest(SetEntitiesRequest request, bool shouldValidate)
	{
		if (ISR2CalibrationHelper.IsRequestForRollerShutterCapability(request))
		{
			return null;
		}
		return PerformSyncedRepositoryOperation(request.RequestId, delegate
		{
			BaseResponse baseResponse = null;
			try
			{
				if (shouldValidate)
				{
					ValidationResult validationResult = setEntitiesRequestValidator.ValidateRequest(request.Interactions, request.Locations, request.BaseDevices, request.LogicalDevices, request.HomeSetups);
					if (!validationResult.Valid)
					{
						Log.Error(Module.ExternalCommandDispatcher, "SetEntities Validation failed because " + validationResult.Reason);
						baseResponse = new ErrorResponse(request.RequestId, ErrorResponseType.ConfigurationUpdateError, ((IEnumerable<string>)validationResult.Errors).Select((Func<string, Property>)((string x) => new StringProperty("ValidationError", x))).ToArray());
					}
				}
				if (baseResponse == null)
				{
					request.Interactions.ForEach(repository.SetInteraction);
					request.Locations.ForEach(repository.SetLocation);
					request.HomeSetups.ForEach(repository.SetHomeSetup);
					UpdateBaseDevices(request.BaseDevices, shouldValidate);
					UpdateLogicalDevices(request.LogicalDevices, shouldValidate);
					baseResponse = new AcknowledgeResponse();
				}
			}
			catch (Exception ex)
			{
				Log.Exception(Module.ExternalCommandDispatcher, ex, "SetEntities request handling failed");
				baseResponse = new ErrorResponse(request.RequestId, ErrorResponseType.ConfigurationUpdateError);
			}
			return baseResponse;
		});
	}

	public BaseResponse DeleteConfiguration(DeleteEntitiesRequest request)
	{
		return PerformSyncedRepositoryOperation(request.RequestId, delegate
		{
			BaseResponse baseResponse = null;
			try
			{
				ValidationResult validationResult = deleteEntitiesRequestValidator.ValidateDeleteRequest(request.Entities);
				if (!validationResult.Valid)
				{
					Log.Error(Module.ExternalCommandDispatcher, "DeleteEntities Validation failed because " + validationResult.Reason);
					baseResponse = new ErrorResponse(request.RequestId, ErrorResponseType.ConfigurationUpdateError, ((IEnumerable<string>)validationResult.Errors).Select((Func<string, Property>)((string x) => new StringProperty("Validate delete error", x))).ToArray());
				}
				if (baseResponse == null)
				{
					foreach (EntityMetadata entity in request.Entities)
					{
						switch (entity.EntityType)
						{
						case EntityType.Interaction:
							repository.DeleteInteraction(entity.Id);
							break;
						case EntityType.Location:
							repository.DeleteLocation(entity.Id);
							break;
						case EntityType.LogicalDevice:
							repository.DeleteLogicalDevice(entity.Id);
							break;
						case EntityType.BaseDevice:
							repository.DeleteBaseDevice(entity.Id);
							break;
						case EntityType.HomeSetup:
							repository.DeleteHomeSetup(entity.Id);
							break;
						case EntityType.Configuration:
							throw new ConfigurationException();
						}
					}
					baseResponse = new AcknowledgeResponse();
				}
			}
			catch (Exception ex)
			{
				Log.Exception(Module.ExternalCommandDispatcher, ex, "DeleteEntities request handling failed");
				baseResponse = new ErrorResponse(request.RequestId, ErrorResponseType.ConfigurationUpdateError);
			}
			return baseResponse;
		});
	}

	public BaseResponse GetConfiguration(GetEntitiesRequest request)
	{
		GetEntitiesResponse getEntitiesResponse = new GetEntitiesResponse();
		getEntitiesResponse.ConfigurationVersion = repository.RepositoryVersion;
		GetEntitiesResponse getEntitiesResponse2 = getEntitiesResponse;
		if (request.EntityType == EntityType.Location || request.EntityType == EntityType.Configuration)
		{
			getEntitiesResponse2.Locations = repository.GetOriginalLocations();
		}
		if (request.EntityType == EntityType.LogicalDevice || request.EntityType == EntityType.Configuration)
		{
			getEntitiesResponse2.LogicalDevices = repository.GetOriginalLogicalDevices();
		}
		if (request.EntityType == EntityType.BaseDevice || request.EntityType == EntityType.Configuration)
		{
			getEntitiesResponse2.BaseDevices = repository.GetOriginalBaseDevices();
		}
		if (request.EntityType == EntityType.Interaction || request.EntityType == EntityType.Configuration)
		{
			getEntitiesResponse2.Interactions = repository.GetOriginalInteractions();
		}
		if (request.EntityType == EntityType.HomeSetup || request.EntityType == EntityType.Configuration)
		{
			getEntitiesResponse2.HomeSetups = repository.GetOriginalHomeSetups();
		}
		return getEntitiesResponse2;
	}

	public BaseResponse GetConfiguration(GetEntitiesByVersionRequest getEntitiesByVersionRequest)
	{
		int changesSinceVersion = getEntitiesByVersionRequest.ChangesSinceVersion;
		EntitiesUpdateResponse entitiesUpdateResponse = new EntitiesUpdateResponse();
		entitiesUpdateResponse.ConfigurationVersion = repository.RepositoryVersion;
		entitiesUpdateResponse.Interactions = repository.GetOriginalInteractions(changesSinceVersion);
		entitiesUpdateResponse.Locations = repository.GetOriginalLocations(changesSinceVersion);
		entitiesUpdateResponse.LogicalDevices = repository.GetOriginalLogicalDevices(changesSinceVersion);
		entitiesUpdateResponse.HomeSetups = repository.GetOriginalHomeSetups(changesSinceVersion);
		entitiesUpdateResponse.BaseDevices = repository.GetOriginalBaseDevices(changesSinceVersion);
		entitiesUpdateResponse.DeletedEntities = repository.GenerateRepositoryCommittedDeletionReport(changesSinceVersion);
		return entitiesUpdateResponse;
	}

	public BaseResponse GetConfiguration(GetEntitiesModificationRequest request)
	{
		ConfigurationUpdateReport updateReport = repository.GetUpdateReport();
		EntitiesUpdateResponse entitiesUpdateResponse = new EntitiesUpdateResponse();
		entitiesUpdateResponse.ConfigurationVersion = repository.RepositoryVersion;
		entitiesUpdateResponse.Interactions = updateReport.ModifiedInteractions;
		entitiesUpdateResponse.Locations = updateReport.ModifiedLocations;
		entitiesUpdateResponse.LogicalDevices = updateReport.ModifiedLogicalDevices;
		entitiesUpdateResponse.BaseDevices = updateReport.ModifiedBaseDevices;
		entitiesUpdateResponse.DeletedEntities = updateReport.DeletedEntities;
		return entitiesUpdateResponse;
	}

	public EntityDeploymentNotification BuildEntityDeploymentNotification(IEnumerable<EntityMetadata> repositoryModificationReport)
	{
		EntityDeploymentNotification entityDeploymentNotification = new EntityDeploymentNotification();
		entityDeploymentNotification.EntityDeployments = new List<EntityDeploymentInformation>();
		entityDeploymentNotification.Namespace = "core.RWE";
		EntityDeploymentNotification entityDeploymentNotification2 = entityDeploymentNotification;
		foreach (EntityMetadata item2 in repositoryModificationReport)
		{
			if (item2.EntityType == EntityType.LogicalDevice && repository.GetLogicalDevice(item2.Id) != null)
			{
				EntityDeploymentInformation entityDeploymentInformation = new EntityDeploymentInformation();
				entityDeploymentInformation.Entity = item2;
				entityDeploymentInformation.State = DeploymentState.Deployed;
				EntityDeploymentInformation item = entityDeploymentInformation;
				entityDeploymentNotification2.EntityDeployments.Add(item);
			}
		}
		return entityDeploymentNotification2;
	}

	private void UpdateBaseDevices(List<BaseDevice> baseDevices, bool shouldValidate)
	{
		baseDevices.ForEach(delegate(BaseDevice x)
		{
			if (shouldValidate)
			{
				BaseDevice baseDevice = repository.GetBaseDevice(x.Id);
				if (baseDevice == null)
				{
					BaseDevice discoveredBaseDevice = discoveryController.GetDiscoveredBaseDevice(x.Id);
					if (discoveredBaseDevice == null)
					{
						throw new Exception("Attempted to include undiscovered device.");
					}
					x.AppId = discoveredBaseDevice.AppId;
					x.DeviceType = discoveredBaseDevice.DeviceType;
					x.DeviceVersion = discoveredBaseDevice.DeviceVersion;
					x.Manufacturer = discoveredBaseDevice.Manufacturer;
					x.ProtocolId = discoveredBaseDevice.ProtocolId;
					x.SerialNumber = discoveredBaseDevice.SerialNumber;
					x.TimeOfDiscovery = discoveredBaseDevice.TimeOfDiscovery;
					x.TimeOfAcceptance = ShcDateTime.UtcNow;
					repository.SetBaseDevice(x);
				}
				else
				{
					repository.SetBaseDevice(x);
				}
			}
			else
			{
				repository.SetBaseDevice(x);
			}
		});
	}

	private void UpdateLogicalDevices(List<LogicalDevice> logicalDevices, bool shouldValidate)
	{
		logicalDevices.ForEach(delegate(LogicalDevice x)
		{
			if (shouldValidate)
			{
				LogicalDevice logicalDevice = repository.GetLogicalDevice(x.Id);
				if (logicalDevice != null)
				{
					x.PrimaryPropertyName = logicalDevice.PrimaryPropertyName;
					repository.SetLogicalDevice(x);
				}
			}
			else
			{
				repository.SetLogicalDevice(x);
			}
		});
	}
}
