using System;
using System.IO;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.BusinessLogic.USBLogExporter.Export;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;
using RWE.SmartHome.SHC.DisplayManagerInterfaces.Exceptions;

namespace RWE.SmartHome.SHC.BusinessLogic.USBLogExporter;

internal class UsbStickLogExport : Task, IService
{
	private const string MOUNT_DIR = "/Hard Disk";

	private const string LOG_FILENAME = "shc.log";

	private readonly IFileLogger fileLogger;

	private readonly Container containerAccess;

	private Task consoleLogger;

	private readonly IEventManager eventManager;

	private readonly IDisplayManager displayManager;

	private readonly ICertificateManager certificateManager;

	public UsbStickLogExport(Container container, IFileLogger fileLogger, IEventManager eventManager, IDisplayManager displayManager, ICertificateManager certificateManager)
	{
		base.Name = "UsbStickLogExport";
		this.fileLogger = fileLogger;
		containerAccess = container;
		this.eventManager = eventManager;
		this.displayManager = displayManager;
		this.certificateManager = certificateManager;
		eventManager.GetEvent<USBDriveNotificationEvent>().Subscribe(OnUSBChangedEvent, (USBDriveNotificationEventArgs p) => p.DeviceName == "DSK2:", ThreadOption.PublisherThread, null);
	}

	public void Initialize()
	{
	}

	public void Uninitialize()
	{
	}

	protected override void Run()
	{
	}

	public override void Stop()
	{
	}

	private void OnUSBChangedEvent(USBDriveNotificationEventArgs args)
	{
		bool flag = false;
		try
		{
			if (!args.Attached)
			{
				displayManager.WorkflowFinished(Workflow.UsbLogFileExport);
				return;
			}
			if (!args.Mounted)
			{
				displayManager.WorkflowFailed(Workflow.UsbLogFileExport, WorkflowError.UsbStickLogExport_UnsupportedFileSystem);
				return;
			}
			try
			{
				displayManager.WorkflowProceeded(Workflow.UsbLogFileExport, WorkflowMessage.UsbBusy, forceDisplay: false);
				if (Directory.Exists("Hard Disk\\EnableSerialLog") && !FilePersistence.EnableSerialLogging)
				{
					StartSerialLogging();
					Log.Information(Module.Logging, "\"EnableSerialLog\" was found on USB. Serial logging enabled!");
					Directory.Delete("Hard Disk\\EnableSerialLog");
					FilePersistence.EnableSerialLogging = true;
				}
				if (Directory.Exists("Hard Disk\\DisableSerialLog"))
				{
					Log.Information(Module.Logging, "\"DisableSerialLog\" was found on USB. Stopping serial logging...");
					StopSerialLogging();
					Directory.Delete("Hard Disk\\DisableSerialLog");
					FilePersistence.EnableSerialLogging = false;
				}
				ExportLogFile();
				displayManager.WorkflowProceeded(Workflow.UsbLogFileExport, WorkflowMessage.UsbDone, forceDisplay: false);
			}
			catch (ExceptionWithWorkflowError exceptionWithWorkflowError)
			{
				Log.Error(Module.Logging, exceptionWithWorkflowError.InnerException.ToString());
				displayManager.WorkflowFailed(Workflow.UsbLogFileExport, exceptionWithWorkflowError.WorkflowError);
			}
		}
		catch (Exception ex)
		{
			if (!flag)
			{
				flag = true;
				Log.Error(Module.Logging, ex.ToString());
			}
			displayManager.WorkflowFailed(Workflow.UsbLogFileExport, WorkflowError.UsbStickLogExport_OtherError);
		}
	}

	private void StartSerialLogging()
	{
		try
		{
			if (consoleLogger == null)
			{
				containerAccess.Register("ConsoleLogging", (Func<Container, IService>)delegate(Container c)
				{
					ConsoleLogging consoleLogging = new ConsoleLogging(c);
					c.Resolve<ITaskManager>().Register(consoleLogging);
					return consoleLogging;
				}).InitializedBy(delegate(Container c, IService v)
				{
					v.Initialize();
				}).ReusedWithin(ReuseScope.Container);
				consoleLogger = (ConsoleLogging)containerAccess.ResolveNamed<IService>("ConsoleLogging");
			}
			consoleLogger.Start();
		}
		catch (Exception ex)
		{
			Log.Error(Module.Logging, $"Starting of ConsoleLogging failed: {ex.Message}");
		}
	}

	private void StopSerialLogging()
	{
		try
		{
			if (consoleLogger != null)
			{
				consoleLogger.Stop();
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.Logging, $"Stopping of ConsoleLogging failed: {ex.Message}");
		}
	}

	private void ExportLogFile()
	{
		try
		{
			eventManager.GetEvent<LogSystemInformationEvent>().Publish(new LogSystemInformationEventArgs());
			string path = Path.Combine("/Hard Disk", "shc.log");
			LogExporter logExporter = new LogExporter(fileLogger, certificateManager);
			logExporter.ExportLog(path);
		}
		catch (ExceptionWithWorkflowError)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new ExceptionWithWorkflowError(innerException, WorkflowError.UsbStickLogExport_OtherError);
		}
	}
}
