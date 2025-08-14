using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Control;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceInclusion;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

internal class ConfigurationHandler : ICommandHandler, IBaseCommandHandler
{
	private const string LoggingSource = "ConfigurationHandler";

	private readonly IEventManager eventManager;

	private readonly INotificationHandler notificationHandler;

	private SubscriptionToken configurationProcessingFailedSubscriptionToken;

	private SubscriptionToken configurationSaveFailureSubscriptionToken;

	private SubscriptionToken configurationValidatedSubscriptionToken;

	private SubscriptionToken deviceDiscoveryFailureSubscriptionToken;

	private readonly bool disableSetEntitiesRequestsValidation;

	private readonly IProtocolMultiplexer protocolMultiplexer;

	private readonly IDiscoveryController discoveryController;

	private readonly RepositoryOperationsWrapper repoOperationsWrapper;

	public ConfigurationHandler(IEventManager eventManager, INotificationHandler notificationHandler, IProtocolMultiplexer protocolMultiplexer, IDiscoveryController discoveryController, RepositoryOperationsWrapper repoOperationsWrapper, bool disableSetEntitiesRequestsValidation)
	{
		this.eventManager = eventManager;
		this.notificationHandler = notificationHandler;
		this.protocolMultiplexer = protocolMultiplexer;
		this.discoveryController = discoveryController;
		this.repoOperationsWrapper = repoOperationsWrapper;
		this.disableSetEntitiesRequestsValidation = disableSetEntitiesRequestsValidation;
	}

	public void Initialize()
	{
		if (configurationSaveFailureSubscriptionToken == null)
		{
			configurationSaveFailureSubscriptionToken = eventManager.GetEvent<CommitTechnicalConfigurationFailureEvent>().Subscribe(HandleConfigurationFailure, null, ThreadOption.PublisherThread, null);
		}
		if (configurationValidatedSubscriptionToken == null)
		{
			configurationValidatedSubscriptionToken = eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(ConfigurationValidated, (ConfigurationProcessedEventArgs args) => args.ConfigurationPhase == ConfigurationProcessedPhase.ValidatedInternally, ThreadOption.PublisherThread, null);
		}
		if (configurationProcessingFailedSubscriptionToken == null)
		{
			configurationProcessingFailedSubscriptionToken = eventManager.GetEvent<ConfigurationProcessingFailedEvent>().Subscribe(ConfigurationProcessingFailed, null, ThreadOption.PublisherThread, null);
		}
		if (deviceDiscoveryFailureSubscriptionToken == null)
		{
			deviceDiscoveryFailureSubscriptionToken = eventManager.GetEvent<DeviceDiscoveryFailedEvent>().Subscribe(OnDeviceDiscoveryFailed, null, ThreadOption.PublisherThread, null);
		}
	}

	public void Uninitialize()
	{
		if (configurationSaveFailureSubscriptionToken != null)
		{
			eventManager.GetEvent<CommitTechnicalConfigurationFailureEvent>().Unsubscribe(configurationSaveFailureSubscriptionToken);
			configurationSaveFailureSubscriptionToken = null;
		}
		if (configurationValidatedSubscriptionToken != null)
		{
			eventManager.GetEvent<ConfigurationProcessedEvent>().Unsubscribe(configurationValidatedSubscriptionToken);
			configurationValidatedSubscriptionToken = null;
		}
		if (configurationProcessingFailedSubscriptionToken != null)
		{
			eventManager.GetEvent<ConfigurationProcessingFailedEvent>().Unsubscribe(configurationProcessingFailedSubscriptionToken);
			configurationProcessingFailedSubscriptionToken = null;
		}
		if (deviceDiscoveryFailureSubscriptionToken != null)
		{
			eventManager.GetEvent<DeviceDiscoveryFailedEvent>().Unsubscribe(deviceDiscoveryFailureSubscriptionToken);
			deviceDiscoveryFailureSubscriptionToken = null;
		}
	}

	public BaseResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction)
	{
		if (request is GetEntitiesRequest request2 && channelContext.ChannelId == "Core.Local")
		{
			return repoOperationsWrapper.GetConfiguration(request2);
		}
		if (request is SetEntitiesRequest request3)
		{
			return repoOperationsWrapper.SetEntitiesRequest(request3, ShouldRequestBeChecked(channelContext.ChannelId));
		}
		if (request is GetEntitiesByVersionRequest getEntitiesByVersionRequest)
		{
			return repoOperationsWrapper.GetConfiguration(getEntitiesByVersionRequest);
		}
		if (request is GetEntitiesModificationRequest request4)
		{
			return repoOperationsWrapper.GetConfiguration(request4);
		}
		if (request is DeleteEntitiesRequest request5)
		{
			return repoOperationsWrapper.DeleteConfiguration(request5);
		}
		if (request is RestoreConfigurationRequest request6)
		{
			return HandleRestoreConfiguration(request6);
		}
		if (request is DeviceFactoryResetRequest factoryResetRequest)
		{
			return HandleFactoryResetOfDevice(factoryResetRequest);
		}
		if (request is ActivateDeviceDiscoveryRequest activateDeviceDiscoveryRequest)
		{
			return HandleDeviceDiscoveryActivation(activateDeviceDiscoveryRequest);
		}
		if (request is DeactivateDeviceDiscoveryRequest)
		{
			return HandleDeviceDiscoveryDeactivation();
		}
		return null;
	}

	private BaseResponse HandleRestoreConfiguration(RestoreConfigurationRequest request)
	{
		eventManager.GetEvent<ProcessRestoredConfigurationEvent>().Publish(new ProcessRestoredConfigurationEventArgs(request.RestorePointId.ToString(), request.CreateRestorePoint));
		return new AcknowledgeResponse();
	}

	private BaseResponse HandleFactoryResetOfDevice(DeviceFactoryResetRequest factoryResetRequest)
	{
		protocolMultiplexer.ResetDeviceInclusionState(factoryResetRequest.DeviceId);
		if (factoryResetRequest.Command == DeviceFactoryResetCommand.IgnoreDevice)
		{
			Log.Information(Module.ExternalCommandDispatcher, "ConfigurationHandler", $"Device {protocolMultiplexer.GetDeviceDescription(factoryResetRequest.DeviceId)} successfully excluded after factory reset.");
		}
		notificationHandler.SendNotification(new SaveConfigurationNotification
		{
			ConfigurationVersion = repoOperationsWrapper.GetRepositoryVersion() + 1,
			Namespace = "core.RWE",
			Locations = new List<Location>(),
			BaseDevices = new List<BaseDevice>(),
			LogicalDevices = new List<LogicalDevice>(),
			Interactions = new List<Interaction>(),
			DeletedEntities = new List<EntityMetadata>()
		});
		return new AcknowledgeResponse();
	}

	private BaseResponse HandleDeviceDiscoveryActivation(ActivateDeviceDiscoveryRequest activateDeviceDiscoveryRequest)
	{
		discoveryController.StartDiscovery(activateDeviceDiscoveryRequest.AppIds);
		return new AcknowledgeResponse();
	}

	private BaseResponse HandleDeviceDiscoveryDeactivation()
	{
		discoveryController.StopDiscovery();
		return new AcknowledgeResponse();
	}

	private bool ShouldRequestBeChecked(string channelId)
	{
		if (channelId != "Internal")
		{
			return !disableSetEntitiesRequestsValidation;
		}
		return false;
	}

	private void ConfigurationValidated(ConfigurationProcessedEventArgs args)
	{
		notificationHandler.SendNotification(new SaveConfigurationNotification
		{
			ConfigurationVersion = args.ConfigurationVersion,
			Namespace = "core.RWE",
			Locations = args.ModifiedLocations,
			BaseDevices = args.ModifiedBaseDevices,
			LogicalDevices = args.ModifiedLogicalDevices,
			Interactions = args.ModifiedInteractions,
			DeletedEntities = args.RepositoryDeletionReport,
			HomeSetups = args.ModifiedHomeSetups
		});
	}

	private void HandleConfigurationFailure(CommitTechnicalConfigurationFailureEventArgs args)
	{
		notificationHandler.SendNotification(new CommitConfigurationErrorNotification
		{
			ConfigurationVersion = args.ConfigurationVersion,
			Message = args.Message,
			Namespace = "core.RWE"
		});
	}

	private void ConfigurationProcessingFailed(ConfigurationProcessingFailedEventArgs args)
	{
		SaveConfigurationErrorNotification saveConfigurationErrorNotification = new SaveConfigurationErrorNotification();
		saveConfigurationErrorNotification.ConfigurationVersion = args.ConfigurationVersion;
		saveConfigurationErrorNotification.ErrorReason = args.ErrorReason;
		saveConfigurationErrorNotification.ErrorEntries = args.ErrorList;
		saveConfigurationErrorNotification.Namespace = "core.RWE";
		SaveConfigurationErrorNotification notification = saveConfigurationErrorNotification;
		notificationHandler.SendNotification(notification);
	}

	private void OnDeviceDiscoveryFailed(DeviceDiscoveryFailedEventArgs args)
	{
		DeviceDiscoveryFailureNotification deviceDiscoveryFailureNotification = new DeviceDiscoveryFailureNotification();
		deviceDiscoveryFailureNotification.NotificationId = Guid.NewGuid();
		deviceDiscoveryFailureNotification.AppIds = args.AppIds;
		deviceDiscoveryFailureNotification.Namespace = "core.RWE";
		DeviceDiscoveryFailureNotification notification = deviceDiscoveryFailureNotification;
		notificationHandler.SendNotification(notification);
	}
}
