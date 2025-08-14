using System;
using System.IO;
using System.Threading;
using CommonFunctionality.Encryption;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.BusinessLogic.Resources;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;
using SHCWrapper.FirmwareUpdater;

namespace RWE.SmartHome.SHC.BusinessLogic.CoprocessorUpdate;

public class CoprocessorUpdater : ICoprocessorUpdater
{
	private const string LoggingSource = "CoprocessorUpdater";

	private readonly ICoprocessorAccess coprocessorAccess;

	private readonly IDisplayManager displayManager;

	private readonly Configuration configuration;

	private volatile bool isFlashingSuccessful;

	private readonly IProtocolSpecificDataBackup sequenceCounterBackup;

	private volatile bool coproUpdateDone;

	public CoprocessorUpdater(Container container, IProtocolSpecificDataBackup sequenceCounterBackup)
	{
		configuration = new Configuration(container.Resolve<IConfigurationManager>());
		coprocessorAccess = container.Resolve<ICoprocessorAccess>();
		displayManager = container.Resolve<IDisplayManager>();
		this.sequenceCounterBackup = sequenceCounterBackup;
		coproUpdateDone = false;
	}

	public bool DoUpdate()
	{
		Log.Information(Module.BusinessLogic, "CoprocessorUpdater", $"Do update. Thread id: {Thread.CurrentThread.ManagedThreadId}");
		if (configuration.CoprocessorUpdateEnabled.Value)
		{
			bool flag = IsUpdateNeeded();
			if (flag)
			{
				displayManager.WorkflowProceeded(Workflow.CoprocessorUpdate, WorkflowMessage.UpdatingCoprocessor, forceDisplay: false);
				try
				{
					CheckCoprocImageIntegrity();
					sequenceCounterBackup.Backup();
					int num = 0;
					while (flag)
					{
						if (num++ == configuration.CoprocessorUpdateRetryCount.Value)
						{
							throw new CoprocessorUpdateException($"Flashing coprocessor failed after {configuration.CoprocessorUpdateRetryCount.Value} attempts.");
						}
						Thread thread = new Thread(FlashCoprocessor);
						thread.Start();
						bool flag2 = thread.Join(configuration.CoprocessorUpdateTimeout.Value);
						Thread.Sleep(2000);
						if (!flag2)
						{
							thread.Abort();
							Log.Warning(Module.BusinessLogic, "CoprocessorUpdater", $"Flashing coprocessor timed out at attempt {num}.");
						}
						else if (!isFlashingSuccessful)
						{
							Log.Warning(Module.BusinessLogic, "CoprocessorUpdater", $"Flashing coprocessor failed at attempt {num}.");
						}
						else if (!IsUpdateNeeded())
						{
							flag = false;
							sequenceCounterBackup.Restore(restoreDefaults: true);
							displayManager.WorkflowFinished(Workflow.CoprocessorUpdate);
						}
					}
				}
				catch (Exception arg)
				{
					Log.Error(Module.BusinessLogic, "CoprocessorUpdater", $"Failed to perform coprocessor update. See details: [{arg}]");
					displayManager.WorkflowFailed(Workflow.CoprocessorUpdate, WorkflowError.CoprocessorUpdateFailed);
				}
			}
		}
		else
		{
			Log.Information(Module.BusinessLogic, "CoprocessorUpdater", "Coprocessor update is disabled");
		}
		return coproUpdateDone;
	}

	private bool IsUpdateNeeded()
	{
		try
		{
			string currentVersion = GetCurrentVersion();
			Log.Information(Module.BusinessLogic, "CoprocessorUpdater", $"Coprocessor version: {currentVersion}");
			bool flag = currentVersion != configuration.TargetCoprocessorVersion;
			if (!flag && currentVersion != configuration.V1CoprocessorVersion)
			{
				string currentChecksum = GetCurrentChecksum();
				Log.Information(Module.BusinessLogic, "CoprocessorUpdater", $"Coprocessor checksum: {currentChecksum}, Target checksum: {configuration.TargetCoprocessorChecksum}");
				flag = currentChecksum != configuration.TargetCoprocessorChecksum;
			}
			Log.Information(Module.BusinessLogic, "CoprocessorUpdater", $"IsUpdateNeeded returned {flag}.");
			return flag;
		}
		catch (Exception ex)
		{
			Log.Warning(Module.BusinessLogic, "CoprocessorUpdater", $"Error checking need for update, returning true: {ex.Message}");
			return true;
		}
	}

	private void FlashCoprocessor()
	{
		try
		{
			using MemoryStream data_to_write = new MemoryStream(RWE.SmartHome.SHC.BusinessLogic.Resources.Resources.Serial);
			Log.Information(Module.BusinessLogic, "CoprocessorUpdater", "Starting coproc flashing...");
			isFlashingSuccessful = AVRFirmwareManager.UpdateAVR(data_to_write);
			Log.Information(Module.BusinessLogic, "CoprocessorUpdater", "Coproc flashing exited...");
			if (isFlashingSuccessful)
			{
				coproUpdateDone = true;
			}
		}
		catch (Exception arg)
		{
			Log.Error(Module.BusinessLogic, "CoprocessorUpdater", $"Coproc flashing threw exception: {arg}");
			isFlashingSuccessful = false;
		}
	}

	private void CheckCoprocImageIntegrity()
	{
		crc crc = new crc();
		crc.CRC16_init();
		byte[] serial = RWE.SmartHome.SHC.BusinessLogic.Resources.Resources.Serial;
		byte[] array = serial;
		foreach (byte val in array)
		{
			crc.CRC16_update(val);
		}
		string text = crc.CRC16_High.ToString("X2") + crc.CRC16_Low.ToString("X2");
		Log.Information(Module.BusinessLogic, "CoprocessorUpdater", $"Checksum of the image file: {text}, target checksum: {configuration.TargetCoprocessorChecksum}");
		if (text != configuration.TargetCoprocessorChecksum)
		{
			throw new CoprocessorUpdateException($"Checksum of the image to flash, {text}, does not correspond with the expected checksum, {configuration.TargetCoprocessorChecksum}");
		}
	}

	private string GetCurrentVersion()
	{
		return coprocessorAccess.Version.ToReadable();
	}

	private string GetCurrentChecksum()
	{
		return coprocessorAccess.Checksum.ToReadable();
	}
}
