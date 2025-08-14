namespace RWE.SmartHome.SHC.DomainModel.Constants;

public static class Capabilities
{
	public static class Name
	{
		public const string SirenActuator = "Siren actuator";

		public const string SabotageSensor = "Sabotage sensor";
	}

	public static class Type
	{
		public const string SirenActuator = "SirenActuator";

		public const string SabotageSensor = "SabotageSensor";
	}

	public static class SirenActuatorParams
	{
		public const string AlarmSoundId = "AlarmSoundId";

		public const string NotificationSoundId = "NotificationSoundId";

		public const string FeedbackSoundId = "FeedbackSoundId";
	}
}
