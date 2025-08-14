using SmartHome.SHC.API.Configuration.Services;
using SmartHome.SHC.API.DeviceActivityLogging;
using SmartHome.SHC.API.Events;
using SmartHome.SHC.API.Logging;
using SmartHome.SHC.API.Messaging;
using SmartHome.SHC.API.Settings;
using SmartHome.SHC.API.Storage;
using SmartHome.SHC.API.SystemServices;
using SmartHome.SHC.API.TaskScheduler;

namespace SmartHome.SHC.API;

public interface ICoreServices
{
	IIsolatedStorage ApplicationStorage { get; }

	IFilesystemStorage FilesystemStorage { get; }

	IMessageService ApplicationMessages { get; }

	IEventService EventsService { get; }

	ILogger ApplicationLogger { get; }

	IConfigurationProvider ConfigurationProvider { get; }

	ISettingsProvider SettingsProvider { get; }

	ISystemInformation ShcSystemInformation { get; }

	ITaskScheduler TaskScheduler { get; }

	ISystemServices SystemServices { get; }

	IActivityLoggingService ActivityLoggingService { get; }

	ICustomTriggerServices CustomTriggerServices { get; }
}
