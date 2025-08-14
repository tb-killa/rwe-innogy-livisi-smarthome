namespace SipcosCommandHandler;

public enum DeviceInfoGenericDeviceType : short
{
	Actuator = 0,
	Sensor = 1,
	Controller = 2,
	RemoteControl = 3,
	Display = 4,
	DynamicFeatureSet = 5,
	SNC = 256,
	RadiatorMountedThermostat = 257,
	PlugInSwitch = 258,
	WallMountedController = 259,
	WallMountedMotionDetector = 261,
	DoorWindowSensor = 262,
	Dimmer = 263,
	SmokeDetector = 264,
	Router = 265,
	InWallSwitch = 266,
	InWallDimmer = 267,
	InWallRollerShutter = 268,
	InWallController = 269,
	RoomThermostat = 270,
	FloorHeating = 271,
	RadiatorMountedValveActuator = 272
}
