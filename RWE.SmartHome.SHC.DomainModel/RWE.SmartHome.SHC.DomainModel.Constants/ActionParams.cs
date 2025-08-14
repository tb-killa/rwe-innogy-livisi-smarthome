using System.Runtime.InteropServices;

namespace RWE.SmartHome.SHC.DomainModel.Constants;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ActionParams
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct StartRamp
	{
		public const string RampDirection = "RampDirection";

		public const string RampTime = "RampTime";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct RampDirection
	{
		public const string RampUp = "RampUp";

		public const string RampDown = "RampDown";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct SoftSwitchWithOffTimer
	{
		public const string RampOnTime = "RampOnTime";

		public const string RampOffTime = "RampOffTime";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct SwitchOffTimer
	{
		public const string SwitchOffDelayTime = "SwitchOffDelayTime";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct SetStateWithBehavior
	{
		public const string ControlBehavior = "ShutterControlBehavior";

		public const string DriveTime = "StepDriveTime";
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ControlBeahvior
	{
		public const string Normal = "Normal";

		public const string Inverted = "Inverted";
	}

	public const string TargetShutterLevel = "ShutterLevel";
}
