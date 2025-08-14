using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Control;
using RWE.SmartHome.SHC.BusinessLogic.Exceptions;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations.Enums;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Database;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.CoreApiConverters;
using RWE.SmartHome.SHC.DomainModel.Actions;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.ConfigurationTransformation;

public sealed class ConfigurationProcessor : IConfigurationProcessor, IService
{
	private struct ProcessNewConfigurationData
	{
		public readonly List<Location> ModifiedLocations;

		public readonly List<LogicalDevice> ModifiedLogicalDevices;

		public readonly List<BaseDevice> ModifiedBaseDevices;

		public readonly List<Interaction> ModifiedInteractions;

		public readonly List<HomeSetup> ModifiedHomeSetups;

		public readonly List<BaseDevice> DeletedBaseDevices;

		public readonly List<EntityMetadata> InclusionReport;

		public readonly List<EntityMetadata> ModificationReport;

		public readonly List<EntityMetadata> DeletionReport;

		public readonly List<ErrorEntry> ErrorList;

		public readonly bool Success;

		public readonly SaveConfigurationError Error;

		private ProcessNewConfigurationData(ConfigurationUpdateReport configurationUpdateReport, bool success, SaveConfigurationError errorType, List<ErrorEntry> errors)
		{
			ModifiedLocations = configurationUpdateReport.ModifiedLocations;
			ModifiedLogicalDevices = configurationUpdateReport.ModifiedLogicalDevices;
			ModifiedBaseDevices = configurationUpdateReport.ModifiedBaseDevices;
			ModifiedInteractions = configurationUpdateReport.ModifiedInteractions;
			ModifiedHomeSetups = configurationUpdateReport.ModifiedHomeSetups;
			DeletedBaseDevices = configurationUpdateReport.DeletedBaseDevices;
			InclusionReport = configurationUpdateReport.NewEntities;
			ModificationReport = configurationUpdateReport.ModifiedEntities;
			DeletionReport = configurationUpdateReport.DeletedEntities;
			Success = success;
			Error = errorType;
			ErrorList = errors;
		}

		public ProcessNewConfigurationData(ConfigurationUpdateReport configurationUpdateReport)
			: this(configurationUpdateReport, success: true, SaveConfigurationError.Unknown, new List<ErrorEntry>())
		{
		}

		public ProcessNewConfigurationData(ConfigurationUpdateReport configurationUpdateReport, List<ErrorEntry> errors, SaveConfigurationError errorType)
			: this(configurationUpdateReport, success: false, errorType, errors)
		{
		}
	}

	private const int PersistenceActiveValue = 1;

	private const int PersistenceInactiveValue = 0;

	private const int MAX_PERSISTENCE_TRIALS = 5;

	private SubscriptionToken startUpSubscriptionToken;

	private SubscriptionToken restoredConfigSubscriptionToken;

	private SubscriptionToken configurationFinishedSubscriptionToken;

	private SubscriptionToken softwareUpdateStartToken;

	private SubscriptionToken dstChangedSubscriptionToken;

	private readonly IEventManager eventManager;

	private readonly IScheduler scheduler;

	private readonly IRepository configurationRepository;

	private readonly IBackendPersistence backendPersistence;

	private readonly List<Guid> schedulerTaskIds = new List<Guid>();

	private readonly RandomNumberGenerator randomNumberGenerator;

	private readonly ManualResetEvent backendPersistenceCancellationEvent = new ManualResetEvent(initialState: false);

	private readonly IProtocolMultiplexer protocolMultiplexer;

	private readonly ICommunicationChannel remoteChannel;

	private readonly IRegistrationService registrationService;

	private int backendPersistencePending;

	private int persistenceTrialsLeft;

	private bool isDstConfigChange;

	private Guid persistenceTaskId;

	private readonly object configurationProcessingPending = new object();

	private IRulesRepository rulesRepository;

	private readonly List<IConfigurationValidator> validators = new List<IConfigurationValidator>();

	private readonly DatabaseConnectionsPool databaseConnectionsPool;

	private UpdateReport updateReport;

	private ConfigurationUpdateReportLog configurationUpdateReportLogger;

	public IRulesRepository RulesRepository
	{
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("RulesRepository");
			}
			rulesRepository = value;
		}
	}

	public ConfigurationProcessor(IEventManager eventManager, IScheduler scheduler, IRepository repository, IBackendPersistence backendPersistence, IProtocolMultiplexer protocolMultiplexer, ICommunicationChannel remoteChannel, DatabaseConnectionsPool databaseConnectionsPool, IRegistrationService registrationService)
	{
		this.eventManager = eventManager;
		this.scheduler = scheduler;
		configurationRepository = repository;
		this.backendPersistence = backendPersistence;
		this.protocolMultiplexer = protocolMultiplexer;
		this.remoteChannel = remoteChannel;
		this.databaseConnectionsPool = databaseConnectionsPool;
		this.registrationService = registrationService;
		randomNumberGenerator = new RandomNumberGenerator();
		configurationUpdateReportLogger = new ConfigurationUpdateReportLog();
	}

	public void Initialize()
	{
		isDstConfigChange = false;
		if (startUpSubscriptionToken == null)
		{
			ShcStartupCompletedEvent shcStartupCompletedEvent = eventManager.GetEvent<ShcStartupCompletedEvent>();
			startUpSubscriptionToken = shcStartupCompletedEvent.Subscribe(DoInitialConfiguration, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound1, ThreadOption.PublisherThread, null);
		}
		if (restoredConfigSubscriptionToken == null)
		{
			ProcessRestoredConfigurationEvent processRestoredConfigurationEvent = eventManager.GetEvent<ProcessRestoredConfigurationEvent>();
			restoredConfigSubscriptionToken = processRestoredConfigurationEvent.Subscribe(ProcessRestoredConfiguration, null, ThreadOption.BackgroundThread, null);
		}
		if (configurationFinishedSubscriptionToken == null)
		{
			DeviceConfigurationFinishedEvent deviceConfigurationFinishedEvent = eventManager.GetEvent<DeviceConfigurationFinishedEvent>();
			configurationFinishedSubscriptionToken = deviceConfigurationFinishedEvent.Subscribe(DeviceConfigurationFinished, null, ThreadOption.PublisherThread, null);
		}
		if (softwareUpdateStartToken == null)
		{
			softwareUpdateStartToken = eventManager.GetEvent<SoftwareUpdateProgressEvent>().Subscribe(OnSoftwareUpdateStarted, null, ThreadOption.PublisherThread, null);
		}
		if (dstChangedSubscriptionToken == null)
		{
			dstChangedSubscriptionToken = eventManager.GetEvent<DSTChangedEvent>().Subscribe(OnDaylightSavingTimeChanged, null, ThreadOption.PublisherThread, null);
		}
	}

	public void ProcessNewConfiguration(RepositoryUpdateContextData updateContextData)
	{
		lock (configurationProcessingPending)
		{
			try
			{
				PerformanceMonitoring.PrintMemoryUsage("Before Process new configuration internal");
				ProcessNewConfigurationData data = ProcessNewConfigurationInternal(updateContextData);
				if (!data.Success)
				{
					PublishConfigurationFailed(data.ErrorList, data.Error);
					return;
				}
				PublishConfigurationProcessed(data, ConfigurationProcessedPhase.CompletedInternally, updateReport);
				PublishConfigurationProcessed(data, ConfigurationProcessedPhase.UINotified, updateReport);
			}
			catch (Exception ex)
			{
				Log.Error(Module.ConfigurationTransformation, $"ProcessNewConfiguration threw exception {ex.Message} with details: {ex}");
				PublishConfigurationFailed(null, SaveConfigurationError.Unknown);
			}
		}
	}

	public void Uninitialize()
	{
		if (startUpSubscriptionToken != null)
		{
			ShcStartupCompletedEvent shcStartupCompletedEvent = eventManager.GetEvent<ShcStartupCompletedEvent>();
			shcStartupCompletedEvent.Unsubscribe(startUpSubscriptionToken);
			startUpSubscriptionToken = null;
		}
		if (restoredConfigSubscriptionToken != null)
		{
			ProcessRestoredConfigurationEvent processRestoredConfigurationEvent = eventManager.GetEvent<ProcessRestoredConfigurationEvent>();
			processRestoredConfigurationEvent.Unsubscribe(restoredConfigSubscriptionToken);
			restoredConfigSubscriptionToken = null;
		}
		if (configurationFinishedSubscriptionToken != null)
		{
			DeviceConfigurationFinishedEvent deviceConfigurationFinishedEvent = eventManager.GetEvent<DeviceConfigurationFinishedEvent>();
			deviceConfigurationFinishedEvent.Unsubscribe(configurationFinishedSubscriptionToken);
			configurationFinishedSubscriptionToken = null;
		}
		if (softwareUpdateStartToken != null)
		{
			eventManager.GetEvent<SoftwareUpdateProgressEvent>().Unsubscribe(softwareUpdateStartToken);
			softwareUpdateStartToken = null;
		}
		if (dstChangedSubscriptionToken != null)
		{
			eventManager.GetEvent<DSTChangedEvent>().Unsubscribe(dstChangedSubscriptionToken);
			dstChangedSubscriptionToken = null;
		}
	}

	public void RegisterConfigurationValidator(IConfigurationValidator validator)
	{
		if (validator != null && !validators.Contains(validator))
		{
			validators.Add(validator);
		}
	}

	public void UnregisterConfigurationValidator(IConfigurationValidator validator)
	{
		if (validators.Contains(validator))
		{
			validators.Remove(validator);
		}
	}

	private void DoInitialConfiguration(ShcStartupCompletedEventArgs obj)
	{
		try
		{
			schedulerTaskIds.Clear();
			Log.Information(Module.ConfigurationTransformation, "Configuration transformation after reboot started...");
			TransformationProcessor transformationProcessor = CreateTransformation();
			RepositoryUpdateContextData repositoryUpdateContextData = new RepositoryUpdateContextData(ForcePushDeviceConfiguration.Yes);
			repositoryUpdateContextData.UpdateReport = new UpdateReport();
			repositoryUpdateContextData.UpdateReport.AddProtocolUpdateAll();
			List<ErrorEntry> list = transformationProcessor.Validate(configurationRepository, uint.MaxValue, protocolMultiplexer.GetHandledDevices(), repositoryUpdateContextData);
			if (list.Any())
			{
				Log.Error(Module.ConfigurationTransformation, "DoInitialConfiguration", "Initial check failed.");
				list.ForEach(delegate(ErrorEntry s)
				{
					Log.Information(Module.ConfigurationTransformation, "[Validation error]=> " + s);
				});
				AttemptIntergrityManagementFix(list);
				transformationProcessor = CreateTransformation();
				list = transformationProcessor.Validate(configurationRepository, uint.MaxValue, protocolMultiplexer.GetHandledDevices(), repositoryUpdateContextData);
				if (list.Any())
				{
					Log.Error(Module.ConfigurationTransformation, "DoInitialConfiguration", "Entity update fix failed.");
					list.ForEach(delegate(ErrorEntry s)
					{
						Log.Information(Module.ConfigurationTransformation, "[ValidationError error]=> " + s);
					});
					AttemptDeletionFix(list);
					transformationProcessor = CreateTransformation();
					list = transformationProcessor.Validate(configurationRepository, uint.MaxValue, protocolMultiplexer.GetHandledDevices(), repositoryUpdateContextData);
					if (list.Any())
					{
						Log.Error(Module.ConfigurationTransformation, "DoInitialConfiguration", "Delete fix failed.");
						list.ForEach(delegate(ErrorEntry s)
						{
							Log.Information(Module.ConfigurationTransformation, "[Transformation error]=> " + s);
						});
					}
				}
			}
			if (!list.Any())
			{
				list.AddRange(transformationProcessor.PrepareTransformation(configurationRepository, uint.MaxValue, protocolMultiplexer.GetHandledDevices(), repositoryUpdateContextData));
				if (list.Any())
				{
					Log.Error(Module.ConfigurationTransformation, "DoInitialConfiguration", "Initial PrepareTransformation failed.");
					list.ForEach(delegate(ErrorEntry s)
					{
						Log.Information(Module.ConfigurationTransformation, "[Validation error]=> " + s);
					});
					AttemptIntergrityManagementFix(list);
					transformationProcessor = CreateTransformation();
					list = transformationProcessor.Validate(configurationRepository, uint.MaxValue, protocolMultiplexer.GetHandledDevices(), repositoryUpdateContextData);
					if (list.Any())
					{
						Log.Error(Module.ConfigurationTransformation, "DoInitialConfiguration", "Entity update fix failed.");
						list.ForEach(delegate(ErrorEntry s)
						{
							Log.Information(Module.ConfigurationTransformation, "[Validation error]=> " + s);
						});
					}
					if (!list.Any())
					{
						list.AddRange(transformationProcessor.PrepareTransformation(configurationRepository, uint.MaxValue, protocolMultiplexer.GetHandledDevices(), repositoryUpdateContextData));
						if (list.Any())
						{
							Log.Error(Module.ConfigurationTransformation, "DoInitialConfiguration", "Post fix PrepareTransformation failed.");
							list.ForEach(delegate(ErrorEntry s)
							{
								Log.Information(Module.ConfigurationTransformation, "[Validation error]=> " + s);
							});
						}
					}
					if (list.Any())
					{
						AttemptDeletionFix(list);
						transformationProcessor = CreateTransformation();
						list = transformationProcessor.Validate(configurationRepository, uint.MaxValue, protocolMultiplexer.GetHandledDevices(), repositoryUpdateContextData);
						if (list.Any())
						{
							Log.Error(Module.ConfigurationTransformation, "DoInitialConfiguration", "Delete fix [2] failed.");
							list.ForEach(delegate(ErrorEntry s)
							{
								Log.Information(Module.ConfigurationTransformation, "[Validation error]=> " + s);
							});
						}
						if (!list.Any())
						{
							list.AddRange(transformationProcessor.PrepareTransformation(configurationRepository, uint.MaxValue, protocolMultiplexer.GetHandledDevices(), repositoryUpdateContextData));
							if (list.Any())
							{
								Log.Error(Module.ConfigurationTransformation, "DoInitialConfiguration", "Delete fix [3] failed.");
								list.ForEach(delegate(ErrorEntry s)
								{
									Log.Information(Module.ConfigurationTransformation, "[Validation error]=> " + s);
								});
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				Log.Error(Module.ConfigurationTransformation, "Configuration transformation after reboot failed with validation errors.");
				list.ForEach(delegate(ErrorEntry s)
				{
					Log.Information(Module.ConfigurationTransformation, "[Transformation error]=> " + s);
				});
				transformationProcessor.DiscardTransformationResults();
			}
			else
			{
				Log.Information(Module.ConfigurationTransformation, "Configuration transformation after reboot succeeded, now adding changes to SendScheduler...");
				List<Guid> baseDevicesInUIConfig = (from bd in configurationRepository.GetBaseDevices()
					select bd.Id).ToList();
				List<Guid> devicesToDelete = (from device in protocolMultiplexer.GetHandledDevices()
					where !baseDevicesInUIConfig.Contains(device)
					select device).ToList();
				CommitTechnicalConfiguration(transformationProcessor, devicesToDelete, new RepositoryUpdateContextData());
			}
			if (configurationRepository.IsDirty)
			{
				configurationRepository.Commit(CommitType.DirtyRepository);
				if (!registrationService.IsShcLocalOnly)
				{
					PersistToBackend(new ProcessNewConfigurationEventArgs(createRestorePoint: false), backendPersistenceCancellationEvent);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.ConfigurationTransformation, $"Configuration transformation after reboot threw exception {ex.Message} with details: {ex}");
		}
	}

	private TransformationProcessor CreateTransformation()
	{
		return new TransformationProcessor(protocolMultiplexer.GetProtocolSpecificTransformations(), rulesRepository, validators);
	}

	private void AttemptIntergrityManagementFix(IEnumerable<ErrorEntry> errors)
	{
		foreach (ErrorEntry error in errors)
		{
			EntityMetadata affectedEntity = error.AffectedEntity;
			if (affectedEntity == null)
			{
				Log.ErrorFormat(Module.ConfigurationTransformation, "AttemptIntergrityManagementFix", true, "Trying to fix error but related entity is null: " + error);
				continue;
			}
			Log.Information(Module.ConfigurationTransformation, "AttemptIntergrityManagementFix", "Trying to fix " + affectedEntity.Id);
			try
			{
				switch (affectedEntity.EntityType)
				{
				case EntityType.Location:
				{
					Location location = configurationRepository.GetLocation(affectedEntity.Id);
					configurationRepository.SetLocation(location);
					break;
				}
				case EntityType.BaseDevice:
				{
					BaseDevice baseDevice = configurationRepository.GetBaseDevice(affectedEntity.Id);
					configurationRepository.SetBaseDevice(baseDevice);
					break;
				}
				case EntityType.LogicalDevice:
				{
					LogicalDevice logicalDevice = configurationRepository.GetLogicalDevice(affectedEntity.Id);
					configurationRepository.SetLogicalDevice(logicalDevice);
					break;
				}
				case EntityType.Interaction:
				{
					Interaction interaction = configurationRepository.GetInteraction(affectedEntity.Id);
					configurationRepository.SetInteraction(interaction);
					break;
				}
				}
				configurationRepository.IsDirty = true;
			}
			catch (Exception ex)
			{
				Log.Exception(Module.ConfigurationTransformation, ex, "Error encountered while trying to fix (integrity manager): {0}.", error);
			}
		}
	}

	private void AttemptDeletionFix(IEnumerable<ErrorEntry> errors)
	{
		foreach (ErrorEntry error in errors)
		{
			EntityMetadata affectedEntity = error.AffectedEntity;
			if (affectedEntity == null)
			{
				Log.ErrorFormat(Module.ConfigurationTransformation, "AttemptDeletionFix", true, "Trying to fix error but related entity is null: " + error);
				continue;
			}
			try
			{
				switch (affectedEntity.EntityType)
				{
				case EntityType.Location:
					DeleteLocationFix(affectedEntity.Id);
					break;
				case EntityType.BaseDevice:
					DeleteBaseDeviceFix(affectedEntity.Id);
					break;
				case EntityType.LogicalDevice:
					DeleteLogicalDeviceFix(affectedEntity.Id);
					break;
				case EntityType.Interaction:
					DeleteInteractionFix(affectedEntity.Id);
					break;
				}
				configurationRepository.IsDirty = true;
			}
			catch (Exception ex)
			{
				Log.Exception(Module.ConfigurationTransformation, ex, "Error encountered while trying to fix (delete) {0}.", error);
			}
		}
	}

	private void DeleteLocationFix(Guid id)
	{
		Location location = configurationRepository.GetLocation(id);
		if (location != null)
		{
			configurationRepository.DeleteLocation(id);
			eventManager.GetEvent<ConfigFixEntityDeletedEvent>().Publish(new ConfigFixEntityDeletedEventArgs(location));
		}
	}

	private void DeleteBaseDeviceFix(Guid id)
	{
		BaseDevice baseDevice = configurationRepository.GetBaseDevice(id);
		if (baseDevice != null)
		{
			configurationRepository.DeleteBaseDevice(id);
			eventManager.GetEvent<ConfigFixEntityDeletedEvent>().Publish(new ConfigFixEntityDeletedEventArgs(baseDevice));
		}
	}

	private void DeleteLogicalDeviceFix(Guid id)
	{
		LogicalDevice logicalDevice = configurationRepository.GetLogicalDevice(id);
		if (logicalDevice != null)
		{
			BaseDevice baseDevice = configurationRepository.GetBaseDevice(logicalDevice.BaseDeviceId);
			if (baseDevice != null)
			{
				configurationRepository.DeleteBaseDevice(baseDevice.Id);
				eventManager.GetEvent<ConfigFixEntityDeletedEvent>().Publish(new ConfigFixEntityDeletedEventArgs(baseDevice));
			}
			else
			{
				Log.ErrorFormat(Module.ConfigurationTransformation, "DeleteLogicalDeviceFix", true, "Deleting capability {0} without basedevice", id.ToString());
				configurationRepository.DeleteLogicalDevice(id);
			}
		}
	}

	private void DeleteInteractionFix(Guid id)
	{
		Interaction interaction = configurationRepository.GetInteraction(id);
		if (interaction != null)
		{
			configurationRepository.DeleteInteraction(id);
			eventManager.GetEvent<ConfigFixEntityDeletedEvent>().Publish(new ConfigFixEntityDeletedEventArgs(interaction));
		}
	}

	private void CommitTechnicalConfiguration(TransformationProcessor transformation, IEnumerable<Guid> devicesToDelete, RepositoryUpdateContextData updateContextData)
	{
		transformation.CommitTransformationResults(devicesToDelete, updateContextData);
		foreach (LogicalDeviceState immediateStateChange in transformation.ImmediateStateChanges)
		{
			protocolMultiplexer.DeviceController.ExecuteAction(new ActionContext(ContextType.ConfigurationCommit, Guid.Empty), TemporaryConverters.FromActuatorState(immediateStateChange));
		}
		Log.Information(Module.ConfigurationTransformation, "Technical Configuration commited.");
	}

	private void DeviceConfigurationFinished(DeviceConfigurationFinishedEventArgs e)
	{
		BaseDevice originalBaseDevice = configurationRepository.GetOriginalBaseDevice(e.PhysicalDeviceId);
		if (originalBaseDevice != null)
		{
			LogicalDevice logicalDevice = configurationRepository.GetOriginalLogicalDevices().FirstOrDefault((LogicalDevice ld) => ld.BaseDeviceId == e.PhysicalDeviceId);
			string arg = ((logicalDevice == null) ? "[device not found in configuration]" : logicalDevice.Name);
			string arg2 = (e.Successful ? "successfully finished" : "aborted");
			Log.Information(Module.ConfigurationTransformation, $"The configuration update for device {protocolMultiplexer.GetDeviceDescription(originalBaseDevice.Id)} was {arg2}, logical device name: {arg}");
			protocolMultiplexer.PhysicalState.UpdateDeviceConfigurationState(originalBaseDevice.Id, (!e.Successful) ? DeviceConfigurationState.Pending : DeviceConfigurationState.Complete);
		}
	}

	private void ProcessRestoredConfiguration(ProcessRestoredConfigurationEventArgs args)
	{
		try
		{
			Log.Information(Module.ConfigurationTransformation, "Restoration of UI configuration from backend started...");
			if (!backendPersistence.RestoreUIConfigurationFromRestorePoint(args.RestorePointId))
			{
				Log.Error(Module.ConfigurationTransformation, $"RestoreUIConfigurationFromRestorePoint failed for restore point with ID {args.RestorePointId}");
				PublishConfigurationFailed(null, SaveConfigurationError.BackendRestorePointRetrievalError);
				return;
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.ConfigurationTransformation, $"RestoreUIConfigurationFromRestorePoint threw exception {ex.Message} with details: {ex}");
			PublishConfigurationFailed(null, SaveConfigurationError.Unknown);
			return;
		}
		finally
		{
			backendPersistence.ReleaseServiceClient();
		}
		ProcessNewConfiguration(new RepositoryUpdateContextData(null, args.CreateRestorePoint ? RestorePointCreationOptions.Yes : RestorePointCreationOptions.No));
	}

	private void PublishConfigurationProcessed(ProcessNewConfigurationData data, ConfigurationProcessedPhase configurationProcessedPhase, UpdateReport updateReport)
	{
		ConfigurationProcessedEventArgs payload = new ConfigurationProcessedEventArgs(configurationRepository.RepositoryVersion, data.ModifiedLocations, data.ModifiedLogicalDevices, data.ModifiedBaseDevices, data.ModifiedInteractions, data.ModifiedHomeSetups, data.DeletedBaseDevices, data.InclusionReport, data.DeletionReport, data.ModificationReport, configurationProcessedPhase, updateReport);
		eventManager.GetEvent<ConfigurationProcessedEvent>().Publish(payload);
	}

	private void PublishConfigurationFailed(List<ErrorEntry> errorList, SaveConfigurationError error)
	{
		ConfigurationProcessingFailedEventArgs payload = new ConfigurationProcessingFailedEventArgs(configurationRepository.RepositoryVersion, error, errorList);
		eventManager.GetEvent<ConfigurationProcessingFailedEvent>().Publish(payload);
	}

	private ProcessNewConfigurationData ProcessNewConfigurationInternal(RepositoryUpdateContextData args)
	{
		Log.Information(Module.ConfigurationTransformation, "Configuration transformation started...");
		ConfigurationUpdateReport configurationUpdateReport = configurationRepository.GetUpdateReport();
		configurationUpdateReportLogger.LogConfigurationUpdateReport(configurationUpdateReport);
		UpdateEvaluator updateEvaluator = new UpdateEvaluator(configurationRepository);
		updateReport = updateEvaluator.UpdateReport;
		if (updateReport.IsTechnicalConfigurationUpdateRequired || args.ForcePushDeviceConfiguration == ForcePushDeviceConfiguration.Yes)
		{
			updateReport.AddProtocolUpdateAll();
		}
		ProcessNewConfigurationData processNewConfigurationData = new ProcessNewConfigurationData(configurationUpdateReport);
		List<Guid> devicesToDelete = configurationUpdateReport.DeletedBaseDevices.Select((BaseDevice x) => x.Id).ToList();
		PublishConfigurationProcessed(processNewConfigurationData, ConfigurationProcessedPhase.Starting, updateReport);
		if (backendPersistencePending == 1)
		{
			backendPersistenceCancellationEvent.Set();
			Log.Information(Module.ConfigurationTransformation, "Backend persistence still pending. Cancelling now.");
		}
		TransformationProcessor transformationProcessor = CreateTransformation();
		List<ErrorEntry> list = new List<ErrorEntry>();
		if (updateReport.IsValidationRequired && args.SkipConfigurationValidation != SkipConfigurationValidation.Yes)
		{
			args.UpdateReport = updateReport;
			list.AddRange(transformationProcessor.Validate(configurationRepository, protocolMultiplexer.GetMaximumNumberOfHandledDevices(), protocolMultiplexer.GetHandledDevices(), args));
		}
		else
		{
			Log.Information(Module.ConfigurationTransformation, $"Skipping validation [IsValidationRequired = {updateReport.IsValidationRequired}, SkipConfigurationValidation = {args.SkipConfigurationValidation}]");
		}
		PerformanceMonitoring.PrintMemoryUsage("Transformation processor created.");
		if (list.Count == 0)
		{
			if (updateReport.IsTechnicalConfigurationUpdateRequired || args.ForcePushDeviceConfiguration == ForcePushDeviceConfiguration.Yes)
			{
				list.AddRange(transformationProcessor.PrepareTransformation(configurationRepository, protocolMultiplexer.GetMaximumNumberOfHandledDevices(), protocolMultiplexer.GetHandledDevices(), args));
			}
			else
			{
				Log.Information(Module.ConfigurationTransformation, "Skipping TechnicalConfiguration processing.");
			}
		}
		PerformanceMonitoring.PrintMemoryUsage("Transformation done.");
		if (list.Count > 0)
		{
			Log.Error(Module.ConfigurationTransformation, "Configuration transformation failed with validation errors.");
			list.ForEach(delegate(ErrorEntry s)
			{
				Log.Information(Module.ConfigurationTransformation, "[Transformation error]=> " + s);
			});
			transformationProcessor.DiscardTransformationResults();
			return new ProcessNewConfigurationData(configurationUpdateReport, list, SaveConfigurationError.ValidationError);
		}
		Log.Information(Module.ConfigurationTransformation, "Configuration transformation succeeded, now persisting locally...");
		PerformanceMonitoring.PrintMemoryUsage("New configuration data generated");
		PerformanceMonitoring.PrintMemoryUsage("Profile activation and deletion logged");
		using (DatabaseConnection databaseConnection = databaseConnectionsPool.GetConnection())
		{
			databaseConnection.BeginTransaction();
			if (processNewConfigurationData.ModificationReport.Count > 0 || processNewConfigurationData.DeletionReport.Count > 0)
			{
				try
				{
					configurationRepository.Commit(CommitType.DirtyRepository);
					Log.Information(Module.ConfigurationTransformation, "Local persistence of configuration succeeded, now adding changes to SendScheduler...");
					PerformanceMonitoring.PrintMemoryUsage("Commit of configuration repository");
				}
				catch (Exception ex)
				{
					Log.Information(Module.ConfigurationTransformation, $"Error in ConfigurationRepository.Commit, exception '{ex.Message}' with details: {ex}");
					transformationProcessor.DiscardTransformationResults();
					return new ProcessNewConfigurationData(configurationUpdateReport, list, SaveConfigurationError.LocalPersistenceError);
				}
			}
			PublishConfigurationProcessed(processNewConfigurationData, ConfigurationProcessedPhase.ValidatedInternally, updateReport);
			try
			{
				Log.Debug(Module.ConfigurationTransformation, "Adding changes to SendScheduler started.");
				if (updateReport.IsTechnicalConfigurationUpdateRequired || args.ForcePushDeviceConfiguration == ForcePushDeviceConfiguration.Yes)
				{
					CommitTechnicalConfiguration(transformationProcessor, devicesToDelete, args);
				}
				PerformanceMonitoring.PrintMemoryUsage("technical configuration set");
				List<Guid> list2 = (from dev in processNewConfigurationData.DeletionReport
					where dev.EntityType == EntityType.LogicalDevice
					select dev.Id).ToList();
				if (list2.Any())
				{
					eventManager.GetEvent<ConfigurationDevicesDeletedEvent>().Publish(new ConfigurationDevicesDeletedEventArgs
					{
						DeletedDeviceList = list2
					});
				}
			}
			catch (Exception ex2)
			{
				string message = $"Error applying new configuration, exception '{ex2.Message}' with details: {ex2}";
				Log.Error(Module.ConfigurationTransformation, message);
				CommitTechnicalConfigurationFailureEventArgs payload = new CommitTechnicalConfigurationFailureEventArgs(configurationRepository.RepositoryVersion, message);
				eventManager.GetEvent<CommitTechnicalConfigurationFailureEvent>().Publish(payload);
				databaseConnection.RollbackTransaction();
				configurationRepository.LoadAllEntities();
				return new ProcessNewConfigurationData(configurationUpdateReport, list, SaveConfigurationError.Unknown);
			}
			databaseConnection.CommitTransaction();
		}
		if (isDstConfigChange)
		{
			Log.Information(Module.ConfigurationTransformation, "Skipping backend configuration persistence for DST changes");
			isDstConfigChange = false;
			return processNewConfigurationData;
		}
		if (registrationService.IsShcLocalOnly)
		{
			Log.Debug(Module.ConfigurationTransformation, "SHC is not registered in backend, do not send configuration");
			return processNewConfigurationData;
		}
		if (processNewConfigurationData.ModificationReport.Count > 0 || processNewConfigurationData.DeletionReport.Count > 0 || args.CreateRestorePoint == RestorePointCreationOptions.Yes)
		{
			CancelBackendPersistence(65);
			backendPersistenceCancellationEvent.Reset();
			configurationRepository.IsDirty = true;
			Interlocked.Exchange(ref persistenceTrialsLeft, 5);
			ThreadPool.QueueUserWorkItem(delegate
			{
				PersistToBackend(new ProcessNewConfigurationEventArgs(args.CreateRestorePoint == RestorePointCreationOptions.Yes), backendPersistenceCancellationEvent);
			});
			Log.Information(Module.ConfigurationTransformation, "Persistence of configuration to backend started.");
		}
		return processNewConfigurationData;
	}

	private bool WasCancelled(ManualResetEvent cancellationEvent, ProcessNewConfigurationEventArgs args)
	{
		if (cancellationEvent.WaitOne(0, exitContext: false))
		{
			eventManager.GetEvent<ConfigurationPersistenceEvent>().Publish(new ConfigurationPersistenceEventArgs(BackendPersistenceResult.OperationCancelled));
			return true;
		}
		return false;
	}

	private bool IsRemoteChannelConnected()
	{
		try
		{
			return remoteChannel == null || remoteChannel.Connected;
		}
		catch
		{
			return true;
		}
	}

	private void PersistToBackend(ProcessNewConfigurationEventArgs args, ManualResetEvent cancellationEvent)
	{
		if (registrationService.IsShcLocalOnly)
		{
			Log.Debug(Module.ConfigurationTransformation, "SHC is not registered, cannot persist to backend");
			return;
		}
		if (!configurationRepository.IsDirty)
		{
			Log.Debug(Module.ConfigurationTransformation, "Configuration already synchronized.");
			return;
		}
		if (1 == Interlocked.CompareExchange(ref backendPersistencePending, 1, 0))
		{
			Log.Debug(Module.ConfigurationTransformation, "Configuration persistence already in progress.");
			return;
		}
		string text = "Persisting new configuration to backend failed";
		BackendPersistenceResult backendPersistenceResult;
		try
		{
			if (!IsRemoteChannelConnected())
			{
				Log.Debug(Module.ConfigurationTransformation, "Remote channel is not connected, bypass configuration saving.");
				throw new NoBackendAccessException();
			}
			if (WasCancelled(cancellationEvent, args))
			{
				return;
			}
			backendPersistenceResult = backendPersistence.BackupUIConfiguration(args.CreateRestorePoint, backendPersistenceCancellationEvent);
			if (backendPersistenceResult == BackendPersistenceResult.Success)
			{
				PerformanceMonitoring.PrintMemoryUsage("UI configuration backed up");
				if (WasCancelled(cancellationEvent, args))
				{
					return;
				}
				backendPersistenceResult = backendPersistence.BackupTechnicalConfiguration(backendPersistenceCancellationEvent);
				if (backendPersistenceResult == BackendPersistenceResult.Success)
				{
					PerformanceMonitoring.PrintMemoryUsage("Technical configuration backed up");
					if (WasCancelled(cancellationEvent, args))
					{
						return;
					}
				}
				else
				{
					text += " while persisting technical configuration";
				}
			}
			else
			{
				text += " while persisting UI configuration";
			}
		}
		catch (NoBackendAccessException)
		{
			backendPersistenceResult = BackendPersistenceResult.OperationPostponed;
		}
		catch (Exception ex2)
		{
			text = text + " with exception: " + ex2.Message;
			backendPersistenceResult = BackendPersistenceResult.ServiceAccessError;
		}
		finally
		{
			backendPersistence.ReleaseServiceClient();
			backendPersistencePending = 0;
		}
		if (backendPersistenceResult == BackendPersistenceResult.Success)
		{
			configurationRepository.IsDirty = false;
			Log.Debug(Module.ConfigurationTransformation, "Backend persistence completed successfully.");
		}
		else
		{
			if (WasCancelled(cancellationEvent, args))
			{
				return;
			}
			if (Interlocked.Decrement(ref persistenceTrialsLeft) > 0)
			{
				configurationRepository.IsDirty = true;
				if (backendPersistenceResult != BackendPersistenceResult.OperationPostponed)
				{
					Log.Information(Module.ConfigurationTransformation, $"Backend persistence failed: {text}. Will retry in 1 minute.");
				}
				persistenceTaskId = Guid.NewGuid();
				scheduler.AddSchedulerTask(new FixedTimeSpanSchedulerTask(persistenceTaskId, delegate
				{
					PersistToBackend(args, cancellationEvent);
				}, TimeSpan.FromMinutes(1.0), runOnce: true));
			}
			else
			{
				Log.Warning(Module.ConfigurationTransformation, "Backend persistence failed. Will not retry.");
			}
		}
		eventManager.GetEvent<ConfigurationPersistenceEvent>().Publish(new ConfigurationPersistenceEventArgs(backendPersistenceResult));
	}

	private void OnSoftwareUpdateStarted(SoftwareUpdateProgressEventArgs args)
	{
		if (args.State == SoftwareUpdateState.Started)
		{
			if (!CancelBackendPersistence(120))
			{
				Log.Error(Module.ConfigurationTransformation, "Failed to cancel the pending backend persistence prior to starting the software update.");
			}
			int num = 0;
			while (!Monitor.TryEnter(configurationProcessingPending))
			{
				Thread.Sleep(100);
				if (num++ > 600)
				{
					Log.Warning(Module.ConfigurationTransformation, "Failed to acquire critical section. Will not prevent configuration transformations during update!");
					break;
				}
			}
		}
		if (args.State == SoftwareUpdateState.Success || args.State == SoftwareUpdateState.Failed)
		{
			try
			{
				Monitor.Exit(configurationProcessingPending);
			}
			catch (ArgumentException)
			{
			}
		}
	}

	private bool CancelBackendPersistence(int timeoutSeconds)
	{
		scheduler.RemoveSchedulerTask(persistenceTaskId);
		backendPersistenceCancellationEvent.Set();
		int num = 0;
		while (num++ < timeoutSeconds * 10 && backendPersistencePending == 1)
		{
			Thread.Sleep(100);
		}
		Log.Debug(Module.ConfigurationTransformation, "Pending backend persistence cancel " + ((backendPersistencePending == 1) ? "failed." : "succeeded."));
		return backendPersistencePending == 0;
	}

	private void OnDaylightSavingTimeChanged(DSTChangedEventArgs obj)
	{
		isDstConfigChange = true;
	}
}
