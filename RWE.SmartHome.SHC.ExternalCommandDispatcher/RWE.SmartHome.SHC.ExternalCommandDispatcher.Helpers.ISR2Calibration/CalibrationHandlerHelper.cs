using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Actuators;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Helpers.ISR2Calibration;

public class CalibrationHandlerHelper
{
	private readonly ISR2StateManager stateManager = new ISR2StateManager();

	private readonly IRepository repository;

	public CalibrationHandlerHelper(IRepository repository)
	{
		this.repository = repository;
	}

	public void InitRollerShutterState(RollerShutterActuator capability)
	{
		if (capability != null)
		{
			Guid id = capability.Id;
			ISR2State stateForId = stateManager.GetStateForId(id);
			ISR2CalibrationAction calibrationActionFromPropertyValue = GetCalibrationActionFromPropertyValue(capability.IsCalibrating);
			stateForId.UpdateNewAction(calibrationActionFromPropertyValue);
		}
	}

	public void HandleRollerShutterCapability(RollerShutterActuator capability)
	{
		if (capability != null)
		{
			Guid id = capability.Id;
			ISR2State stateForId = stateManager.GetStateForId(id);
			ISR2CalibrationAction calibrationActionFromPropertyValue = GetCalibrationActionFromPropertyValue(capability.IsCalibrating);
			stateForId.UpdateNewAction(calibrationActionFromPropertyValue);
			ISR2CalibrationState calibrationState = stateForId.GetCalibrationState();
			if (calibrationState == ISR2CalibrationState.Started || calibrationState == ISR2CalibrationState.Stopped)
			{
				UpdateCalibrationProperty(id, capability.IsCalibrating);
			}
			else
			{
				UpdateCapability(capability);
			}
		}
	}

	private void UpdateCalibrationProperty(Guid capabilityId, bool isCalibrating)
	{
		if (repository.GetLogicalDevice(capabilityId) is RollerShutterActuator rollerShutterActuator)
		{
			RollerShutterActuator rollerShutterActuator2 = rollerShutterActuator.Clone();
			rollerShutterActuator2.IsCalibrating = isCalibrating;
			repository.SetLogicalDevice(rollerShutterActuator2);
		}
	}

	private void UpdateCapability(RollerShutterActuator capability)
	{
		if (repository.GetLogicalDevice(capability.Id) is RollerShutterActuator rollerShutterActuator)
		{
			capability.PrimaryPropertyName = rollerShutterActuator.PrimaryPropertyName;
			repository.SetLogicalDevice(capability);
		}
	}

	private ISR2CalibrationAction GetCalibrationActionFromPropertyValue(bool isCalibrating)
	{
		if (!isCalibrating)
		{
			return ISR2CalibrationAction.NoCalibrating;
		}
		return ISR2CalibrationAction.IsCalibrating;
	}
}
