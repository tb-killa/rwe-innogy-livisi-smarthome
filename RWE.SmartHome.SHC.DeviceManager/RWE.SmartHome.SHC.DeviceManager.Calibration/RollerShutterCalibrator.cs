using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Calibrators;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager.Calibration;

internal sealed class RollerShutterCalibrator : IRollerShutterCalibrator
{
	private const string LoggingSource = "RollerShutterCalibrator";

	private readonly IEventManager eventManager;

	private readonly ICommunicationWrapper communicationWrapper;

	private List<CalibrationTarget> calibrationTargets;

	private readonly object syncRoot = new object();

	private DateTime calibrationHeartbeatTime = ShcDateTime.UtcNow;

	private volatile Thread monitoringThread;

	public ICollection<CalibrationTarget> CalibrationTargets => new List<CalibrationTarget>(calibrationTargets);

	public RollerShutterCalibrator(ICommunicationWrapper communicationWrapper, IEventManager eventManager)
	{
		this.eventManager = eventManager;
		this.communicationWrapper = communicationWrapper;
		calibrationTargets = new List<CalibrationTarget>();
		this.communicationWrapper.UnconditionalSwitchCommandHandler.ReceivedUnconditionalSwitchCommand += ReceivedSwitchCommandHandler;
		eventManager.GetEvent<DeviceConfigurationFinishedEvent>().Subscribe(DeviceConfiguredSuccessfully, (DeviceConfigurationFinishedEventArgs args) => args.Successful, ThreadOption.PublisherThread, null);
	}

	private void ReceivedSwitchCommandHandler(SIPcosHeader header, SwitchCommand switchCommand)
	{
		lock (syncRoot)
		{
			try
			{
				HandleReceivedSwitchCommand(header, switchCommand.KeyChannelNumber);
			}
			catch (Exception ex)
			{
				Log.Error(Module.DeviceManager, "RollerShutterCalibrator", $"Failed to handle switch command: {ex.Message}, {ex.StackTrace}");
			}
		}
	}

	private void StartCalibrationMonitoringThread()
	{
		monitoringThread = new Thread(CheckCalibrationTargets);
		monitoringThread.Start();
	}

	private void CheckCalibrationTargets()
	{
		Log.Debug(Module.DeviceManager, "RollerShutterCalibrator", "Calibration thread started.");
		FireTrafficControlEvent(CosIPTrafficState.Suspend);
		try
		{
			DateTime value;
			do
			{
				lock (syncRoot)
				{
					value = calibrationHeartbeatTime;
					foreach (CalibrationTarget calibrationTarget in calibrationTargets)
					{
						if (calibrationTarget.MulticastStopWatch.IsRunning && calibrationTarget.MulticastStopWatch.ElapsedMilliseconds > 800)
						{
							Log.Warning(Module.DeviceManager, "RollerShutterCalibrator", $"Device {calibrationTarget.Id} has not sent any multicast during the last {calibrationTarget.MulticastStopWatch.ElapsedMilliseconds} ms.");
							CompleteCalibrationPhase(calibrationTarget);
						}
					}
				}
				Thread.Sleep(200);
			}
			while (ShcDateTime.UtcNow.Subtract(value).TotalMinutes < 15.0);
			lock (syncRoot)
			{
				monitoringThread = null;
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.DeviceManager, "Error occured at calibration: " + ex.Message);
		}
		Log.Debug(Module.DeviceManager, "RollerShutterCalibrator", "Calibration thread exiting.");
		FireTrafficControlEvent(CosIPTrafficState.Resume);
	}

	private void FireTrafficControlEvent(CosIPTrafficState desiredState)
	{
		eventManager.GetEvent<CosIPTrafficControlEvent>().Publish(new CosIPTrafficControlEventArgs
		{
			TrafficState = desiredState
		});
	}

	private void HandleReceivedSwitchCommand(SIPcosHeader header, byte channelNumber)
	{
		CalibrationTarget calibrationTarget = calibrationTargets.FirstOrDefault((CalibrationTarget x) => communicationWrapper.DeviceList[header.Source] != null && x.Id == communicationWrapper.DeviceList[header.Source].DeviceId);
		if (calibrationTarget != null)
		{
			ShutterDirection shutterDirection = GetShutterDirection(channelNumber);
			switch (calibrationTarget.State)
			{
			case CalibrationState.WaitingForUnicastDown:
			case CalibrationState.WaitingForUnicastUp:
				OnUnicastOrMulticastReceived(calibrationTarget, header, channelNumber, shutterDirection);
				break;
			case CalibrationState.Initialized:
			case CalibrationState.MeasuredFullDown:
			case CalibrationState.MeasuredFullUp:
				OnMulticastReceived(calibrationTarget, header, channelNumber, shutterDirection);
				break;
			case CalibrationState.None:
				break;
			}
		}
	}

	private void OnMulticastReceived(CalibrationTarget currentCalibrationTarget, SIPcosHeader header, byte channelNumber, ShutterDirection direction)
	{
		if (!header.Destination.Compare(SipCosAddress.AllDevices))
		{
			return;
		}
		DateTime utcNow = ShcDateTime.UtcNow;
		lock (syncRoot)
		{
			calibrationHeartbeatTime = utcNow;
			if (monitoringThread == null)
			{
				StartCalibrationMonitoringThread();
			}
		}
		currentCalibrationTarget.MulticastStopWatch.Start();
		currentCalibrationTarget.State = ((direction == ShutterDirection.Down) ? CalibrationState.WaitingForUnicastDown : CalibrationState.WaitingForUnicastUp);
		NotifyStateChangedSubscribers(currentCalibrationTarget, null);
		currentCalibrationTarget.Store(utcNow);
		Log.Debug(Module.DeviceManager, "RollerShutterCalibrator", $"ListenForMulticast: Time {utcNow} stored.");
		Log.Debug(Module.DeviceManager, "RollerShutterCalibrator", $"ListenForMulticast: FirstTime is {currentCalibrationTarget.FirstTime}, LastTime is {currentCalibrationTarget.LastTime}.");
	}

	private void OnUnicastOrMulticastReceived(CalibrationTarget currentCalibrationTarget, SIPcosHeader header, byte channelNumber, ShutterDirection direction)
	{
		DateTime utcNow = ShcDateTime.UtcNow;
		currentCalibrationTarget.Store(utcNow);
		Log.Debug(Module.DeviceManager, "RollerShutterCalibrator", $"ListenForUnicastOrMulticast: Time {utcNow} stored.");
		if (header.Destination.Compare(SipCosAddress.AllDevices))
		{
			currentCalibrationTarget.State = ((direction == ShutterDirection.Up) ? CalibrationState.WaitingForUnicastUp : CalibrationState.WaitingForUnicastDown);
			currentCalibrationTarget.MulticastStopWatch.Reset();
			currentCalibrationTarget.MulticastStopWatch.Start();
		}
		else if ((CalibrationState.WaitingForUnicastDown != currentCalibrationTarget.State || ShutterDirection.Up != direction) && (CalibrationState.WaitingForUnicastUp != currentCalibrationTarget.State || ShutterDirection.Down != direction))
		{
			CompleteCalibrationPhase(currentCalibrationTarget);
		}
	}

	private void CompleteCalibrationPhase(CalibrationTarget calibrationTarget)
	{
		calibrationTarget.MulticastStopWatch.Stop();
		calibrationTarget.MulticastStopWatch.Reset();
		calibrationTarget.State = ((calibrationTarget.State == CalibrationState.WaitingForUnicastDown) ? CalibrationState.MeasuredFullDown : CalibrationState.MeasuredFullUp);
		int? num = calibrationTarget.CalculateTotalTime();
		Log.Information(Module.DeviceManager, "RollerShutterCalibrator", $"ISRCalibrationWizard measured a total for time of {num} from {calibrationTarget.FirstTime} to {calibrationTarget.LastTime}.");
		NotifyStateChangedSubscribers(calibrationTarget, num);
		calibrationTarget.ResetSequenceVariables();
	}

	private static ShutterDirection GetShutterDirection(byte channelNumber)
	{
		return channelNumber switch
		{
			2 => ShutterDirection.Down, 
			3 => ShutterDirection.Up, 
			_ => throw new InvalidOperationException("Shutter direction can only be up or down."), 
		};
	}

	private void NotifyStateChangedSubscribers(CalibrationTarget currentCalibrationTarget, int? totalTime)
	{
		DeviceCalibrationStateChangedEventArgs payload = new DeviceCalibrationStateChangedEventArgs(currentCalibrationTarget.Id, currentCalibrationTarget.State, totalTime);
		eventManager.GetEvent<DeviceCalibrationStateChangedEvent>().Publish(payload);
	}

	private void DeviceConfiguredSuccessfully(DeviceConfigurationFinishedEventArgs configurationFinishedEventArgs)
	{
		Log.Debug(Module.DeviceManager, "RollerShutterCalibrator", $"DeviceConfigurationFinished event received.");
		CalibrationTarget calibrationTarget = calibrationTargets.FirstOrDefault((CalibrationTarget x) => x.Id.Equals(configurationFinishedEventArgs.PhysicalDeviceId));
		if (calibrationTarget != null)
		{
			calibrationTarget.State = CalibrationState.Initialized;
			NotifyStateChangedSubscribers(calibrationTarget, null);
			Log.Debug(Module.DeviceManager, "RollerShutterCalibrator", $"LogicalState of {calibrationTarget.Id} is now {calibrationTarget.State}.");
		}
	}

	public void SetCalibrationTargets(IEnumerable<Guid> deviceIds)
	{
		if (deviceIds == null)
		{
			throw new ArgumentNullException("deviceIds");
		}
		List<CalibrationTarget> list = new List<CalibrationTarget>();
		Guid deviceId;
		foreach (Guid deviceId2 in deviceIds)
		{
			deviceId = deviceId2;
			if (!calibrationTargets.Exists((CalibrationTarget x) => x.Id == deviceId))
			{
				list.Add(new CalibrationTarget(deviceId));
				continue;
			}
			list.Add(calibrationTargets.Find((CalibrationTarget x) => x.Id == deviceId));
		}
		calibrationTargets = list;
	}

	public void ResetCalibration(Guid deviceId)
	{
		lock (syncRoot)
		{
			CalibrationTarget calibrationTarget = calibrationTargets.FirstOrDefault((CalibrationTarget x) => x.Id == deviceId);
			if (calibrationTarget == null)
			{
				throw new InvalidOperationException($"Device with Id {deviceId} is not in calibration mode.");
			}
			ResetCalibrationTarget(calibrationTarget);
		}
	}

	internal void ResetCalibrationTarget(CalibrationTarget timeCalibrationTarget)
	{
		if (timeCalibrationTarget == null)
		{
			throw new ArgumentNullException("timeCalibrationTarget");
		}
		timeCalibrationTarget.Reset();
		NotifyStateChangedSubscribers(timeCalibrationTarget, null);
	}
}
