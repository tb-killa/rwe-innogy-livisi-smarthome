namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.Helpers.ISR2Calibration;

public class ISR2State
{
	private static readonly ISR2CalibrationState[][] graph = new ISR2CalibrationState[2][]
	{
		new ISR2CalibrationState[4]
		{
			ISR2CalibrationState.NoCalibration,
			ISR2CalibrationState.Stopped,
			ISR2CalibrationState.Stopped,
			ISR2CalibrationState.NoCalibration
		},
		new ISR2CalibrationState[4]
		{
			ISR2CalibrationState.Started,
			ISR2CalibrationState.Calibrating,
			ISR2CalibrationState.Calibrating,
			ISR2CalibrationState.Started
		}
	};

	private ISR2CalibrationState currentState;

	public void ClearState()
	{
		currentState = ISR2CalibrationState.NoCalibration;
	}

	public void UpdateNewAction(ISR2CalibrationAction action)
	{
		currentState = graph[(uint)action][(uint)currentState];
	}

	public ISR2CalibrationState GetCalibrationState()
	{
		return currentState;
	}
}
