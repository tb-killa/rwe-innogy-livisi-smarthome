using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.Threading;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.BusinessLogic.Exceptions;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ErrorHandling;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.FileDownload;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;
using RWE.SmartHome.SHC.ErrorHandling;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;
using SHCWrapper.FirmwareUpdater;

namespace RWE.SmartHome.SHC.BusinessLogic.SoftwareUpdate;

public class SoftwareUpdateProcessor : Dispatcher, ISoftwareUpdateProcessor, IService
{
	private enum SWUCheckType
	{
		Full = 1,
		DownloadOnly
	}

	private const string FLASH_SHC_ZIP_FILE = "/NandFlash/shc.zip";

	private const string TEMP_FULL_IMAGE__FILE_NAME = "/NandFlash/update.bin";

	private const string TEMP_APPLICATION_ONLY_FILE_NAME = "/NandFlash/app_update.bin";

	private const string TEMP_DOWNLOAD_FILE_NAME = "/NandFlash/temp.bin";

	private const string LoggingSource = "SoftwareUpdateProcess";

	private const string moduleIdentifier = "SoftwareUpdateScanner";

	private readonly IScheduler scheduler;

	private Guid schedulerFullUpdateTaskId;

	private Guid schedulerDownloadOnlyTaskId;

	private readonly IEventManager eventManager;

	private readonly ICertificateManager certificateManager;

	private readonly ISoftwareUpdateClient softwareUpdateClient;

	private SubscriptionToken doSoftwareUpdateEventSubscriptionToken;

	private SubscriptionToken shcStartupCompletedEventSubscriptionToken;

	private SubscriptionToken connectivityChangedSubscriptionToken;

	private readonly IBackendPersistence backendPersistence;

	private readonly Configuration configuration;

	private readonly IDisplayManager displayManager;

	private readonly IRepository configurationRepository;

	private readonly IRegistrationService registrationService;

	private readonly object businessLogicMutex;

	private volatile bool lastUpdateFailed;

	private Guid clientId = Guid.NewGuid();

	public string AvailableUpdateVersion { get; private set; }

	public SoftwareUpdateProcessor(IScheduler scheduler, IEventManager eventManager, ICertificateManager certificateManager, ISoftwareUpdateClient softwareUpdateClient, IBackendPersistence backendPersistence, Configuration configuration, IDisplayManager displayManager, object businessLogicMutex, IRepository configurationRepository, IRegistrationService registrationService)
	{
		this.displayManager = displayManager;
		base.Name = "SoftwareUpdateProcessor";
		this.backendPersistence = backendPersistence;
		this.softwareUpdateClient = softwareUpdateClient;
		this.certificateManager = certificateManager;
		this.eventManager = eventManager;
		this.scheduler = scheduler;
		this.configuration = configuration;
		this.businessLogicMutex = businessLogicMutex;
		this.configurationRepository = configurationRepository;
		this.registrationService = registrationService;
	}

	private void ScheduleUpdateTasks()
	{
		schedulerFullUpdateTaskId = CreateSWUTask(configuration.SoftwareUpdateWindowStartTime, configuration.SoftwareUpdateWindowStopTime, SWUCheckType.Full);
		schedulerDownloadOnlyTaskId = CreateSWUTask(configuration.SoftwareUpdateDownloadOnlyStartTime, configuration.SoftwareUpdateDownloadOnlyEndTime, SWUCheckType.DownloadOnly);
	}

	private Guid CreateSWUTask(string startWindow, string endWindow, SWUCheckType checkType)
	{
		TimeSpan timeOfDay = ChoosePollingTime(startWindow, endWindow);
		Guid guid = Guid.NewGuid();
		FixedTimeSchedulerTask schedulerTask = new FixedTimeSchedulerTask(guid, delegate
		{
			TaskManagerAction(checkType);
		}, timeOfDay);
		scheduler.AddSchedulerTask(schedulerTask);
		return guid;
	}

	private void CheckDownloadedUpdate()
	{
		Log.Information(Module.BusinessLogic, "Check for already downloaded firmware");
		if (File.Exists("/NandFlash/update.bin"))
		{
			Log.Information(Module.BusinessLogic, "Full firmware file found. Trying to apply update");
			if (FirmwareImage.CheckFirmwareImage("/NandFlash/update.bin") != FirmwareImageState.Complete)
			{
				throw new InvalidDataException("Firmware file is invalid");
			}
			if (!PrepareSoftwareUpdate())
			{
				throw new InvalidOperationException("Cannot prepare for software update.");
			}
			UpdatePerformedHandling.ReleaseUpdatePerformedState();
			UpdateFirmware();
		}
		if (File.Exists("/NandFlash/shc.zip"))
		{
			Log.Information(Module.BusinessLogic, "App firmware file found. Trying to apply update");
			if (!PrepareSoftwareUpdate())
			{
				throw new InvalidOperationException("Cannot prepare for software update.");
			}
			UpdatePerformedHandling.SetUpdatePerformedState(UpdatePerformedStatus.Controlled | UpdatePerformedStatus.ApplicationOnly, flush: true);
			PersistLog();
			ResetPlatform();
		}
		Log.Information(Module.BusinessLogic, "No already downloaded firmware found");
	}

	private bool IsEnoughSpaceAvailableToUpdate(int configurationLimit)
	{
		int num = (int)PerformanceMonitoring.GetFreeDiskMemory();
		if (num > configurationLimit)
		{
			eventManager.GetEvent<NotifyUserDiskSpaceForUpdateEvent>().Publish(new NotifyUserDiskSpaceForUpdateEventArgs(isEnoughSpace: true));
			return true;
		}
		int num2 = configurationLimit - num;
		NotifyUserDiskSpaceForUpdateEventArgs e = new NotifyUserDiskSpaceForUpdateEventArgs(isEnoughSpace: false);
		e.DiskSpaceNecessaryToEmpty = num2.ToString();
		eventManager.GetEvent<NotifyUserDiskSpaceForUpdateEvent>().Publish(e);
		return false;
	}

	private UpdateCheckResult CheckForSoftwareUpdate(SWUCheckType updateCheckType)
	{
		UpdateCheckResultCode resultCode = UpdateCheckResultCode.AlreadyLatest;
		string newVersion = null;
		FireOSStateChangedEvent(OSState.Normal);
		try
		{
			CheckDownloadedUpdate();
			bool connectionAvailable;
			UpdateInfo updateInfo = GetUpdateInfo(usePersonalCertificate: true, suppressErrorOutput: true, out connectionAvailable);
			if (updateInfo != null)
			{
				Log.Information(Module.BusinessLogic, "SoftwareUpdateProcess", $"Newer {updateInfo.Type} version {updateInfo.Version} of {updateInfo.Category} available for download");
				newVersion = updateInfo.Version;
				if (updateInfo.Type == UpdateType.Forced || (updateInfo.Type == UpdateType.Mandatory && updateInfo.UpdateDeadline < ShcDateTime.Now))
				{
					if (DoUpdate(SoftwareUpdateModifier.ForceUpdate, usePersonalCertificate: true, saveDatabase: true, suppressErrorOutput: true, updateCheckType == SWUCheckType.DownloadOnly))
					{
						resultCode = UpdateCheckResultCode.UpdateAvailable;
					}
				}
				else
				{
					PublishSoftwareUpdateAvailableEvent(updateInfo);
					resultCode = UpdateCheckResultCode.UpdateAvailable;
				}
			}
			else if (connectionAvailable)
			{
				Log.Information(Module.BusinessLogic, "SoftwareUpdateProcess", "No update available");
				PublishSoftwareUpdateNotAvailableEvent();
				resultCode = UpdateCheckResultCode.AlreadyLatest;
			}
			else
			{
				resultCode = UpdateCheckResultCode.ErrorServiceNotAccessible;
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, "SoftwareUpdateProcess", $"Unexpected error occured while checking for software update: {ex.Message}");
			resultCode = UpdateCheckResultCode.ErrorUnknown;
		}
		return new UpdateCheckResult(resultCode, newVersion);
	}

	private void PublishSoftwareUpdateAvailableEvent(UpdateInfo updateInfo)
	{
		SoftwareUpdateAvailableEventArgs payload = new SoftwareUpdateAvailableEventArgs((SoftwareUpdateType)updateInfo.Type, updateInfo.UpdateDeadline, updateInfo.Version);
		eventManager.GetEvent<SoftwareUpdateAvailableEvent>().Publish(payload);
	}

	private void PublishSoftwareUpdateNotAvailableEvent()
	{
		eventManager.GetEvent<SoftwareUpdateNotAvailableEvent>().Publish(new SoftwareUpdateNotAvailableEventArgs());
	}

	private void CheckForUpdateAction(SWUCheckType checkType)
	{
		CheckForSoftwareUpdate(checkType);
	}

	private void TaskManagerAction(SWUCheckType swuCheckType)
	{
		Dispatch(new Executable<SWUCheckType>
		{
			Action = CheckForUpdateAction,
			Argument = swuCheckType
		});
		eventManager.GetEvent<SyncUsersEvent>().Publish(new SyncUsersEventArgs());
	}

	public void Initialize()
	{
		if (doSoftwareUpdateEventSubscriptionToken == null)
		{
			doSoftwareUpdateEventSubscriptionToken = eventManager.GetEvent<DoSoftwareUpdateEvent>().Subscribe(delegate
			{
				DoUpdate(SoftwareUpdateModifier.ForceUpdate, usePersonalCertificate: true, saveDatabase: true, suppressErrorOutput: true, downloadOnly: false);
			}, null, ThreadOption.SubscriberThread, this);
		}
		if (shcStartupCompletedEventSubscriptionToken == null)
		{
			shcStartupCompletedEventSubscriptionToken = eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(delegate
			{
				ScheduleUpdateTasks();
			}, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound1, ThreadOption.SubscriberThread, this);
		}
		if (connectivityChangedSubscriptionToken == null)
		{
			connectivityChangedSubscriptionToken = eventManager.GetEvent<ChannelConnectivityChangedEvent>().Subscribe(delegate
			{
				CheckDownloadedUpdate();
			}, (ChannelConnectivityChangedEventArgs args) => args.Connected && lastUpdateFailed, ThreadOption.SubscriberThread, this);
		}
	}

	public void Uninitialize()
	{
		if (shcStartupCompletedEventSubscriptionToken != null)
		{
			eventManager.GetEvent<ShcStartupCompletedEvent>().Unsubscribe(shcStartupCompletedEventSubscriptionToken);
			shcStartupCompletedEventSubscriptionToken = null;
			scheduler.RemoveSchedulerTask(schedulerFullUpdateTaskId);
			scheduler.RemoveSchedulerTask(schedulerDownloadOnlyTaskId);
		}
		if (doSoftwareUpdateEventSubscriptionToken != null)
		{
			eventManager.GetEvent<DoSoftwareUpdateEvent>().Unsubscribe(doSoftwareUpdateEventSubscriptionToken);
			doSoftwareUpdateEventSubscriptionToken = null;
		}
		if (connectivityChangedSubscriptionToken != null)
		{
			eventManager.GetEvent<ChannelConnectivityChangedEvent>().Unsubscribe(connectivityChangedSubscriptionToken);
			connectivityChangedSubscriptionToken = null;
		}
	}

	private TimeSpan ChoosePollingTime(string windowStartTime, string windowEndTime)
	{
		TimeSpan result = RandomTimeGenerator.GenerateTimeBetween(windowStartTime, windowEndTime);
		Log.Information(Module.BusinessLogic, "SoftwareUpdateProcess", $"The SHC will automatically check every day at {result.Hours:D2}:{result.Minutes:D2} for a software update");
		return result;
	}

	public bool DoUpdate(SoftwareUpdateModifier updateModifier, bool usePersonalCertificate, bool saveDatabase, bool suppressErrorOutput, bool downloadOnly)
	{
		lock (businessLogicMutex)
		{
			bool result = false;
			try
			{
				FireOSStateChangedEvent(OSState.Updating);
				CheckDownloadedUpdate();
				bool connectionAvailable;
				UpdateInfo updateInfo = GetUpdateInfo(usePersonalCertificate, suppressErrorOutput, out connectionAvailable);
				if (updateInfo != null)
				{
					if (!IsEnoughSpaceAvailableToUpdate(configuration.MinimumMemoryNecessaryForUpdateApplication.Value))
					{
						throw new NotEnoughDiskSpaceException("Cannot begin the software update because the SHC does not have sufficient memory.");
					}
					if (!IsEnoughSpaceAvailableToUpdate(configuration.MinimumMemoryNecessaryForUpdateOS.Value))
					{
						throw new NotEnoughDiskSpaceException("There is not enough disk space to update the OS");
					}
					if (updateModifier == SoftwareUpdateModifier.OnlyMandatory && updateInfo.Type != UpdateType.Forced && (updateInfo.Type != UpdateType.Mandatory || !(updateInfo.UpdateDeadline < ShcDateTime.Now)))
					{
						Log.Information(Module.BusinessLogic, "SoftwareUpdateProcess", "No forced or due mandatory update available");
						displayManager.WorkflowFinished(Workflow.ShcSoftwareUpdate);
						return true;
					}
					Uri url = CreateDownloadUrl(updateInfo);
					long num = Environment.TickCount;
					if (!DownloadFile(url, "/NandFlash/temp.bin", updateInfo.Category, updateInfo.DownloadUser, updateInfo.DownloadPassword, suppressErrorOutput))
					{
						throw new Exception("Error downloading file from the server");
					}
					Log.Debug(Module.BusinessLogic, $"SWU Download duration {(Environment.TickCount - num) / 1000} s");
					if (!downloadOnly)
					{
						if (saveDatabase && !PrepareSoftwareUpdate())
						{
							throw new InvalidOperationException("Failed to prepare for software updated.");
						}
						UpdatePerformedHandling.ReleaseUpdatePerformedState();
						switch (updateInfo.Category)
						{
						case UpdateCategory.ShcFirmware:
							UpdateFirmware();
							break;
						case UpdateCategory.ShcApplication:
							UpdateApplication(downloadOnly: false);
							break;
						default:
							throw new ArgumentException("Invalid update category");
						}
					}
					else if (updateInfo.Category == UpdateCategory.ShcApplication)
					{
						UpdateApplication(downloadOnly: true);
					}
				}
				else if (connectionAvailable)
				{
					if (updateModifier == SoftwareUpdateModifier.ForceUpdate)
					{
						eventManager.GetEvent<SoftwareUpdateProgressEvent>().Publish(new SoftwareUpdateProgressEventArgs(SoftwareUpdateState.NotAvailable));
					}
					Log.Information(Module.BusinessLogic, "SoftwareUpdateProcess", "No update available");
					displayManager.WorkflowFinished(Workflow.ShcSoftwareUpdate);
				}
				result = true;
			}
			catch (NotEnoughDiskSpaceException ex)
			{
				Log.Error(Module.BusinessLogic, "SoftwareUpdateProcess", ex.Message);
				result = true;
				eventManager.GetEvent<SoftwareUpdateProgressEvent>().Publish(new SoftwareUpdateProgressEventArgs(SoftwareUpdateState.Failed));
				FireOSStateChangedEvent(OSState.Normal);
			}
			catch (Exception arg)
			{
				eventManager.GetEvent<SoftwareUpdateProgressEvent>().Publish(new SoftwareUpdateProgressEventArgs(SoftwareUpdateState.Failed));
				Log.Error(Module.BusinessLogic, "SoftwareUpdateProcess", $"Failed to perform software update: {arg}");
				Log.Error(Module.BusinessLogic, "SoftwareUpdateProcess", $"Cleaning up downloaded files...");
				if (File.Exists("/NandFlash/shc.zip"))
				{
					File.Delete("/NandFlash/shc.zip");
				}
				if (File.Exists("/NandFlash/update.bin"))
				{
					File.Delete("/NandFlash/update.bin");
				}
				if (File.Exists("/NandFlash/app_update.bin"))
				{
					File.Delete("/NandFlash/app_update.bin");
				}
				if (File.Exists("/NandFlash/temp.bin"))
				{
					File.Delete("/NandFlash/temp.bin");
				}
			}
			finally
			{
				if (suppressErrorOutput)
				{
					displayManager.WorkflowFinished(Workflow.ShcSoftwareUpdate);
				}
			}
			return result;
		}
	}

	public bool AnnounceUpdate()
	{
		ShcUpdateAnnouncementResultCode shcUpdateAnnouncementResultCode = ShcUpdateAnnouncementResultCode.TemporaryFailure;
		while (shcUpdateAnnouncementResultCode == ShcUpdateAnnouncementResultCode.TemporaryFailure)
		{
			try
			{
				shcUpdateAnnouncementResultCode = softwareUpdateClient.AnnounceShcUpdate(certificateManager.PersonalCertificateThumbprint, new ShcVersionInfo
				{
					ApplicationVersion = SHCVersion.ApplicationVersion,
					FirmwareVersion = SHCVersion.OsVersion,
					HardwareVersion = SHCVersion.HardwareVersion
				});
			}
			catch
			{
				shcUpdateAnnouncementResultCode = ShcUpdateAnnouncementResultCode.TemporaryFailure;
			}
			if (shcUpdateAnnouncementResultCode != ShcUpdateAnnouncementResultCode.TemporaryFailure)
			{
				break;
			}
			Log.Information(Module.BusinessLogic, "Temporary failure when trying to announce upgrade. Will retry.");
			Thread.Sleep(10000);
		}
		if (shcUpdateAnnouncementResultCode == ShcUpdateAnnouncementResultCode.PermanentFailure)
		{
			Log.Information(Module.BusinessLogic, "Failed to announce upgrade.");
		}
		return shcUpdateAnnouncementResultCode == ShcUpdateAnnouncementResultCode.Success;
	}

	private bool PrepareSoftwareUpdate()
	{
		lastUpdateFailed = true;
		FireOSStateChangedEvent(OSState.Rebooting);
		eventManager.GetEvent<SoftwareUpdateProgressEvent>().Publish(new SoftwareUpdateProgressEventArgs(SoftwareUpdateState.Started));
		try
		{
			if (registrationService.IsShcLocalOnly)
			{
				return true;
			}
			if (backendPersistence.BackupDeviceList(null) != BackendPersistenceResult.Success)
			{
				throw new ShcException(ErrorStrings.FailedToBackupDeviceList, "SoftwareUpdateScanner", 1);
			}
			if (backendPersistence.BackupTechnicalConfiguration(null) != BackendPersistenceResult.Success)
			{
				throw new ShcException(ErrorStrings.FailedToBackupTechnicalConfiguration, "SoftwareUpdateScanner", 2);
			}
			if (backendPersistence.BackupUIConfiguration(createRestorePoint: false, null) != BackendPersistenceResult.Success)
			{
				throw new ShcException(ErrorStrings.FailedToBackupUiConfiguration, "SoftwareUpdateScanner", 3);
			}
			if (backendPersistence.BackupMessagesAndAlerts(null) != BackendPersistenceResult.Success)
			{
				throw new ShcException(ErrorStrings.FailedToBackupMessagesAndAlerts, "SoftwareUpdateScanner", 4);
			}
			if (backendPersistence.BackupCustomApplicationsSettings(null) != BackendPersistenceResult.Success)
			{
				throw new ShcException(ErrorStrings.FailedToBackupCustomApplicationsData, "SoftwareUpdateScanner", 5);
			}
			lastUpdateFailed = false;
		}
		catch
		{
			eventManager.GetEvent<SoftwareUpdateProgressEvent>().Publish(new SoftwareUpdateProgressEventArgs(SoftwareUpdateState.Failed));
			FireOSStateChangedEvent(OSState.Normal);
			throw;
		}
		finally
		{
			backendPersistence.ReleaseServiceClient();
		}
		return true;
	}

	private void UpdateApplication(bool downloadOnly)
	{
		File.Delete("/NandFlash/shc.zip");
		File.Delete("/NandFlash/update.bin");
		File.Move("/NandFlash/app_update.bin", "/NandFlash/shc.zip");
		if (!downloadOnly)
		{
			UpdatePerformedHandling.SetUpdatePerformedState(UpdatePerformedStatus.Controlled | UpdatePerformedStatus.ApplicationOnly, flush: true);
			PersistLog();
			ResetPlatform();
		}
	}

	private void ResetPlatform()
	{
		Log.Information(Module.BusinessLogic, "SoftwareUpdateProcess", "Reset");
		eventManager.GetEvent<PerformResetEvent>().Publish(new PerformResetEventArgs());
	}

	private void UpdateFirmware()
	{
		if (FirmwareImage.CheckFirmwareImage("/NandFlash/update.bin") != FirmwareImageState.Complete)
		{
			throw new InvalidDataException("Firmware file is invalid");
		}
		Log.Information(Module.BusinessLogic, "SoftwareUpdateProcess", "Writing update to flash");
		eventManager.GetEvent<LogSuspendEvent>().Publish(new LogSuspendEventArgs());
		Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
		displayManager.WorkflowProceeded(Workflow.ShcSoftwareUpdate, WorkflowMessage.UpdatingSHCFirmware, forceDisplay: false);
		if (WinCEFirmwareManager.WriteRawPartition("/NandFlash/update.bin"))
		{
			UpdatePerformedHandling.SetUpdatePerformedState(UpdatePerformedStatus.Controlled, flush: true);
			Log.Information(Module.BusinessLogic, "SoftwareUpdateProcess", string.Format("Removing update file {0}", "/NandFlash/update.bin"));
			File.Delete("/NandFlash/update.bin");
			PersistLog();
			displayManager.WorkflowDynamicMessage(Workflow.ShcSoftwareUpdate, "REBOOT");
			Thread.Sleep(1000);
			ResetPlatform();
			return;
		}
		displayManager.WorkflowFailed(Workflow.ShcSoftwareUpdate, WorkflowError.DirectFlashWriteError);
		File.Delete("/NandFlash/update.bin");
		throw new ShcException(ErrorStrings.FailedToWriteToFlash, "SoftwareUpdateScanner", 0);
	}

	private void PersistLog()
	{
		eventManager.GetEvent<PersistDataBeforeSoftwareUpdateEvent>().Publish(new PersistDataBeforeSoftwareUpdateEventArgs());
	}

	private UpdateInfo GetUpdateInfo(bool usePersonalCertificate, bool suppressErrorOutput, out bool connectionAvailable)
	{
		try
		{
			string shcSerial = SHCSerialNumber.SerialNumber();
			ShcVersionInfo shcVersionInfo = new ShcVersionInfo();
			shcVersionInfo.ApplicationVersion = SHCVersion.ApplicationVersion;
			shcVersionInfo.FirmwareVersion = SHCVersion.OsVersion;
			shcVersionInfo.HardwareVersion = SHCVersion.HardwareVersion;
			ShcVersionInfo shcVersionInfo2 = shcVersionInfo;
			UpdateInfo updateInfo = null;
			if (usePersonalCertificate)
			{
				if (!string.IsNullOrEmpty(certificateManager.PersonalCertificateThumbprint))
				{
					Log.Information(Module.BusinessLogic, "SoftwareUpdateProcess", "Calling CheckForSoftwareUpdate with Personalized Certificate");
					updateInfo = CheckForSoftwareUpdate(shcSerial, shcVersionInfo2, certificateManager.PersonalCertificateThumbprint);
				}
			}
			else
			{
				Log.Information(Module.BusinessLogic, "SoftwareUpdateProcess", "Calling CheckForSoftwareUpdate with Default Certificate");
				updateInfo = CheckForSoftwareUpdateWithRetries(shcSerial, shcVersionInfo2, certificateManager.DefaultCertificateThumbprint);
			}
			connectionAvailable = true;
			AvailableUpdateVersion = ((updateInfo != null) ? updateInfo.Version : string.Empty);
			if (!string.IsNullOrEmpty(AvailableUpdateVersion))
			{
				PublishSoftwareUpdateAvailableEvent(updateInfo);
			}
			else
			{
				PublishSoftwareUpdateNotAvailableEvent();
			}
			return updateInfo;
		}
		catch (Exception ex)
		{
			if (ex is CommunicationException || ex is WebException)
			{
				if (!suppressErrorOutput)
				{
					displayManager.WorkflowFailed(Workflow.ShcSoftwareUpdate, WorkflowError.SoftwareUpdateServiceUnavailable);
				}
				Log.Error(Module.BusinessLogic, "SoftwareUpdateProcess", $"Failed to connect to backend for software update: {ex.Message}");
				connectionAvailable = false;
				return null;
			}
			Log.Error(Module.BusinessLogic, "SoftwareUpdateProcess", $"Failed to perform software update: {ex}");
			throw;
		}
	}

	private UpdateInfo CheckForSoftwareUpdateWithRetries(string shcSerial, ShcVersionInfo shcVersionInfo, string certificateThumbprint)
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			try
			{
				return CheckForSoftwareUpdate(shcSerial, shcVersionInfo, certificateThumbprint);
			}
			catch (Exception)
			{
				if (4 != i - 1)
				{
					num += 20;
					Log.Warning(Module.BusinessLogic, $"Check for software update failed, will retry in {num} seconds");
					Thread.Sleep(num * 1000);
				}
			}
		}
		Log.Error(Module.BusinessLogic, $"Check for software update failed");
		return null;
	}

	private UpdateInfo CheckForSoftwareUpdate(string shcSerial, ShcVersionInfo shcVersionInfo, string certificateThumbprint)
	{
		UpdateInfo updateInfo;
		SwUpdateResultCode swUpdateResultCode = softwareUpdateClient.CheckForSoftwareUpdate(certificateThumbprint, shcSerial, shcVersionInfo, out updateInfo);
		if (swUpdateResultCode != SwUpdateResultCode.NewerVersionAvailable)
		{
			updateInfo = null;
			if (swUpdateResultCode != SwUpdateResultCode.AlreadyLatestVersion)
			{
				Log.Error(Module.BusinessLogic, "SoftwareUpdateProcess", $"Failed to check for software update: {swUpdateResultCode}");
			}
		}
		return updateInfo;
	}

	private static Uri CreateDownloadUrl(UpdateInfo updateInfo)
	{
		if (updateInfo.DownloadLocation == null)
		{
			throw new ArgumentNullException("DownloadLocation");
		}
		return new Uri(updateInfo.DownloadLocation);
	}

	private bool DownloadFile(Uri url, string fileName, UpdateCategory updateCategory, string username, string password, bool suppressErrorOutput)
	{
		DateTime utcNow = DateTime.UtcNow;
		bool result = false;
		int num = 0;
		while (!result && DateTime.UtcNow - utcNow < TimeSpan.FromSeconds(45.0))
		{
			num++;
			Log.Debug(Module.BusinessLogic, "Download software update file: Retry #" + num);
			FileDownloader fileDownloader = new FileDownloader();
			result = true;
			fileDownloader.DownloadStarted = delegate
			{
				displayManager.WorkflowProceeded(Workflow.ShcSoftwareUpdate, WorkflowMessage.DownloadingUpdate, forceDisplay: true);
				Log.Information(Module.BusinessLogic, "SoftwareUpdateProcess", $"Downloading {url.AbsoluteUri}");
				FireOSStateChangedEvent(OSState.Downloading);
			};
			fileDownloader.DownloadServerUnavailable = delegate(string message)
			{
				Log.Error(Module.BusinessLogic, "SoftwareUpdateProcess", $"Download failed: URL: [{url.ToString()}] {message}");
				result = false;
				if (!suppressErrorOutput)
				{
					displayManager.WorkflowFailed(Workflow.ShcSoftwareUpdate, WorkflowError.SoftwareDownloadServiceUnavailable);
				}
			};
			fileDownloader.DownloadInvalidResponse = delegate(string message)
			{
				Log.Error(Module.BusinessLogic, "SoftwareUpdateProcess", $"Failed to retrieve server response URL: [{url.ToString()}] {message}");
				result = false;
				if (!suppressErrorOutput)
				{
					displayManager.WorkflowFailed(Workflow.ShcSoftwareUpdate, WorkflowError.SoftwareDownloadServiceResponseInvalid);
				}
			};
			fileDownloader.DownloadCompleted = delegate
			{
				displayManager.WorkflowFinished(Workflow.ShcSoftwareUpdate);
				Log.Information(Module.BusinessLogic, "SoftwareUpdateProcess", "Download complete");
				string destFileName = ((updateCategory == UpdateCategory.ShcApplication) ? "/NandFlash/app_update.bin" : "/NandFlash/update.bin");
				File.Move("/NandFlash/temp.bin", destFileName);
			};
			fileDownloader.DownloadFile(url, fileName, username, password);
			if (!result)
			{
				Thread.Sleep(5000);
				FireOSStateChangedEvent(OSState.Normal);
			}
		}
		lastUpdateFailed = false;
		return result;
	}

	public UpdateCheckResult CheckForUpdate()
	{
		return CheckForSoftwareUpdate(SWUCheckType.Full);
	}

	private void FireOSStateChangedEvent(OSState newOSState)
	{
		if (eventManager != null)
		{
			eventManager.GetEvent<OSStateChangedEvent>().Publish(new OSStateChangedEventArgs(newOSState));
		}
	}
}
