using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.Calibration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Calibration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Requests.Configuration;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Responses;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.RepositoryOperations;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.SetEntitiesRequestValidation;
using RWE.SmartHome.SHC.ChannelInterfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Calibrators;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.ExternalCommandDispatcher.Helpers.ISR2Calibration;
using RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Handler;

internal class CalibrationHandler : ICommandHandler, IBaseCommandHandler
{
	private const int RetryContextMS = 5000;

	private readonly IRepository repository;

	private readonly IRepositorySync repositorySync;

	private readonly INotificationHandler notificationHandler;

	private readonly IRollerShutterCalibrator rollerShutterCalibrator;

	private readonly ISetEntitiesRequestValidator setEntitiesRequestValidator;

	private readonly CalibrationHandlerHelper calibrationHandlerHelper;

	public CalibrationHandler(IEventManager eventManager, INotificationHandler notificationHandler, IRepository repository, IRepositorySync repositorySync, ISetEntitiesRequestValidator setEntitiesRequestValidator, IRollerShutterCalibrator rollerShutterCalibrator)
	{
		this.repository = repository;
		this.repositorySync = repositorySync;
		this.notificationHandler = notificationHandler;
		this.rollerShutterCalibrator = rollerShutterCalibrator;
		this.setEntitiesRequestValidator = setEntitiesRequestValidator;
		calibrationHandlerHelper = new CalibrationHandlerHelper(repository);
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnDatabaseAvailable, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.DatabaseAvailable, ThreadOption.BackgroundThread, null);
		eventManager.GetEvent<DeviceCalibrationStateChangedEvent>().Subscribe(OnDeviceCalibrationStateChanged, null, ThreadOption.BackgroundThread, null);
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}

	public BaseResponse HandleRequest(ChannelContext channelContext, BaseRequest request, ref Action postSendAction)
	{
		if (request is ResetCalibrationRequest resetCalibrationRequest)
		{
			return HandleResetCalibration(resetCalibrationRequest);
		}
		if (request is SetEntitiesRequest setEntitiesRequest && ISR2CalibrationHelper.IsRequestForRollerShutterCapability(setEntitiesRequest))
		{
			return HandleRollerShutterCapabilityRequest(setEntitiesRequest);
		}
		return null;
	}

	private BaseResponse HandleResetCalibration(ResetCalibrationRequest resetCalibrationRequest)
	{
		rollerShutterCalibrator.ResetCalibration(GetPhysicalDeviceId(resetCalibrationRequest.LogicalDeviceId));
		return new AcknowledgeResponse();
	}

	private void OnDeviceCalibrationStateChanged(DeviceCalibrationStateChangedEventArgs obj)
	{
		if (obj.CalibrationState != CalibrationState.WaitingForUnicastDown && obj.CalibrationState != CalibrationState.WaitingForUnicastUp)
		{
			LogicalDevice calibratingDevice = GetCalibratingDevice(obj.DeviceId);
			if (calibratingDevice != null)
			{
				CalibrationNotification calibrationNotification = new CalibrationNotification();
				calibrationNotification.CalibrationStep = ConvertToCalibrationStep(obj.CalibrationState);
				calibrationNotification.LogicalDeviceId = calibratingDevice.Id;
				calibrationNotification.Value = obj.Value;
				calibrationNotification.Namespace = "core.RWE";
				CalibrationNotification notification = calibrationNotification;
				notificationHandler.SendNotification(notification);
			}
			else
			{
				Log.Error(Module.ExternalCommandDispatcher, "Unable to publish calibration notification: cannot obtain calibrating device. Corrupted configuration?");
			}
		}
	}

	private void OnDatabaseAvailable(ShcStartupCompletedEventArgs args)
	{
		IEnumerable<RollerShutterActuator> enumerable = repository.GetLogicalDevices().OfType<RollerShutterActuator>();
		foreach (RollerShutterActuator item in enumerable)
		{
			calibrationHandlerHelper.InitRollerShutterState(item);
		}
	}

	private static CalibrationStep ConvertToCalibrationStep(CalibrationState calibrationState)
	{
		CalibrationStep result = CalibrationStep.None;
		switch (calibrationState)
		{
		case CalibrationState.Initialized:
			result = CalibrationStep.Initialized;
			break;
		case CalibrationState.MeasuredFullDown:
			result = CalibrationStep.MeasuredFullDown;
			break;
		case CalibrationState.MeasuredFullUp:
			result = CalibrationStep.MeasuredFullUp;
			break;
		default:
			throw new ArgumentOutOfRangeException("calibrationState");
		case CalibrationState.None:
		case CalibrationState.WaitingForUnicastDown:
		case CalibrationState.WaitingForUnicastUp:
			break;
		}
		return result;
	}

	private LogicalDevice GetCalibratingDevice(Guid physicalDeviceId)
	{
		return repository.GetLogicalDevices().OfType<RollerShutterActuator>().FirstOrDefault((RollerShutterActuator ld) => ld.BaseDevice != null && ld.BaseDevice.Id == physicalDeviceId);
	}

	private Guid GetPhysicalDeviceId(Guid logicalDeviceId)
	{
		LogicalDevice logicalDevice = repository.GetLogicalDevice(logicalDeviceId);
		if (logicalDevice != null && logicalDevice.BaseDevice != null)
		{
			return logicalDevice.BaseDevice.Id;
		}
		throw new ArgumentException($"No PhysicalDevice found for LogicalDeviceID {logicalDeviceId}.");
	}

	private BaseResponse PerformSyncedRepositoryOperation(Guid requestId, int getLockRetries, Func<BaseResponse> operation)
	{
		RepositoryLockContext repositoryLockContext = null;
		BaseResponse baseResponse;
		try
		{
			repositoryLockContext = GetLockContext(getLockRetries);
			baseResponse = operation();
			repositoryLockContext.Commit = baseResponse is AcknowledgeResponse;
		}
		catch (Exception ex)
		{
			Log.Exception(Module.ExternalCommandDispatcher, ex, "CalibrationHandler SyncedRepositoryOperation failed {0}.", ex);
			baseResponse = new ErrorResponse(requestId, ErrorResponseType.ConfigurationAccessError);
		}
		finally
		{
			repositoryLockContext?.Dispose();
		}
		return baseResponse;
	}

	private RepositoryLockContext GetLockContext(int retries)
	{
		for (int i = 1; i <= retries; i++)
		{
			try
			{
				string reason = "PerformCalibrationRepositoryOperation";
				return repositorySync.GetLockAsyncRelease(reason, new RepositoryUpdateContextData(CoreConstants.CoreAppId));
			}
			catch (RepositoryLockedException)
			{
				Log.Warning(Module.ExternalCommandDispatcher, $"Calibration configuration locked. Remains {retries - i} retry.");
				if (i == retries)
				{
					throw;
				}
				Thread.Sleep(5000);
			}
		}
		return null;
	}

	private BaseResponse HandleRollerShutterCapabilityRequest(SetEntitiesRequest setEntitiesRequest)
	{
		RollerShutterActuator capability = setEntitiesRequest.LogicalDevices[0] as RollerShutterActuator;
		return PerformSyncedRepositoryOperation(setEntitiesRequest.RequestId, 3, delegate
		{
			BaseResponse baseResponse = null;
			ValidationResult validationResult = ValidateCapability(capability);
			if (validationResult.Valid)
			{
				calibrationHandlerHelper.HandleRollerShutterCapability(capability);
				return new AcknowledgeResponse();
			}
			Log.Error(Module.ExternalCommandDispatcher, $"SetEntities Validation failed because {validationResult.Reason}");
			StringProperty[] data = validationResult.Errors.Select((string x) => new StringProperty("ValidationError", x)).ToArray();
			return new ErrorResponse(setEntitiesRequest.RequestId, ErrorResponseType.ConfigurationUpdateError, data);
		});
	}

	private ValidationResult ValidateCapability(LogicalDevice capability)
	{
		return setEntitiesRequestValidator.ValidateRequest(Enumerable.Empty<Interaction>(), Enumerable.Empty<Location>(), Enumerable.Empty<BaseDevice>(), new List<LogicalDevice> { capability }, Enumerable.Empty<HomeSetup>());
	}
}
