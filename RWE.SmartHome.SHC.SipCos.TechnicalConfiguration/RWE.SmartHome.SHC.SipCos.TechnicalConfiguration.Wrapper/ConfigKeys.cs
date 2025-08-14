using System.Collections.Generic;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.Wrapper;

internal static class ConfigKeys
{
	public static KeyValuePair<byte, byte> PowerSave = new KeyValuePair<byte, byte>(0, 1);

	public static KeyValuePair<byte, byte> ChildLock = new KeyValuePair<byte, byte>(0, 2);

	public static KeyValuePair<byte, byte> Language = new KeyValuePair<byte, byte>(0, 3);

	public static KeyValuePair<byte, byte> DestInfoHigh = new KeyValuePair<byte, byte>(0, 4);

	public static KeyValuePair<byte, byte> DestInfoMid = new KeyValuePair<byte, byte>(0, 5);

	public static KeyValuePair<byte, byte> DestInfoLow = new KeyValuePair<byte, byte>(0, 6);

	public static KeyValuePair<byte, byte> AckLed = new KeyValuePair<byte, byte>(0, 7);

	public static KeyValuePair<byte, byte> DutyLimit = new KeyValuePair<byte, byte>(0, 8);

	public static KeyValuePair<byte, byte> EventFilterNumberAndPeriod = new KeyValuePair<byte, byte>(1, 1);

	public static KeyValuePair<byte, byte> BrightnessFilterAndInterval = new KeyValuePair<byte, byte>(1, 2);

	public static KeyValuePair<byte, byte> LongPressTime = new KeyValuePair<byte, byte>(1, 3);

	public static KeyValuePair<byte, byte> ReferenceRunningTimeTopBottomHigh = new KeyValuePair<byte, byte>(1, 4);

	public static KeyValuePair<byte, byte> ReferenceRunningTimeTopBottomLow = new KeyValuePair<byte, byte>(1, 5);

	public static KeyValuePair<byte, byte> ReferenceRunningTimeBottomTopHigh = new KeyValuePair<byte, byte>(1, 6);

	public static KeyValuePair<byte, byte> ReferenceRunningTimeBottomTopLow = new KeyValuePair<byte, byte>(1, 7);

	public static KeyValuePair<byte, byte> EventFiltertime = new KeyValuePair<byte, byte>(1, 8);

	public static KeyValuePair<byte, byte> InputMode = new KeyValuePair<byte, byte>(1, 9);

	public static KeyValuePair<byte, byte> CcExerciseDay = new KeyValuePair<byte, byte>(1, 10);

	public static KeyValuePair<byte, byte> CcExerciseTime = new KeyValuePair<byte, byte>(1, 11);

	public static KeyValuePair<byte, byte> CcActuatorRangeLimit = new KeyValuePair<byte, byte>(1, 12);

	public static KeyValuePair<byte, byte> CcActuatorFailLevel = new KeyValuePair<byte, byte>(1, 13);

	public static KeyValuePair<byte, byte> CcTempOffset = new KeyValuePair<byte, byte>(1, 14);

	public static KeyValuePair<byte, byte> CcAntiFreeze = new KeyValuePair<byte, byte>(1, 15);

	public static KeyValuePair<byte, byte> CcAntiMold = new KeyValuePair<byte, byte>(1, 16);

	public static KeyValuePair<byte, byte> CcRotaryMin = new KeyValuePair<byte, byte>(1, 17);

	public static KeyValuePair<byte, byte> CcRotaryMax = new KeyValuePair<byte, byte>(1, 18);

	public static KeyValuePair<byte, byte> CcWindowDtOpen = new KeyValuePair<byte, byte>(1, 19);

	public static KeyValuePair<byte, byte> CcWindowDtClose = new KeyValuePair<byte, byte>(1, 20);

	public static KeyValuePair<byte, byte> CcWindowTemp = new KeyValuePair<byte, byte>(1, 21);

	public static KeyValuePair<byte, byte> SendInfoRetries = new KeyValuePair<byte, byte>(1, 22);

	public static KeyValuePair<byte, byte> CcCycleDurationPwm = new KeyValuePair<byte, byte>(1, 23);

	public static KeyValuePair<byte, byte> CcTimebarMin1 = new KeyValuePair<byte, byte>(1, 24);

	public static KeyValuePair<byte, byte> CcTimebarMin2 = new KeyValuePair<byte, byte>(1, 25);

	public static KeyValuePair<byte, byte> CcProportionalRegion = new KeyValuePair<byte, byte>(1, 26);

	public static KeyValuePair<byte, byte> CcResetTime = new KeyValuePair<byte, byte>(1, 27);

	public static KeyValuePair<byte, byte> CcValveDelayOpen = new KeyValuePair<byte, byte>(1, 28);

	public static KeyValuePair<byte, byte> CcValveDelayClose = new KeyValuePair<byte, byte>(1, 29);

	public static KeyValuePair<byte, byte> CcValveType = new KeyValuePair<byte, byte>(1, 30);

	public static KeyValuePair<byte, byte> CurrentDetectionBehavior = new KeyValuePair<byte, byte>(1, 31);

	public static KeyValuePair<byte, byte> CcControlMode = new KeyValuePair<byte, byte>(1, 37);

	public static KeyValuePair<byte, byte> CcTempDisplay = new KeyValuePair<byte, byte>(1, 33);

	public static KeyValuePair<byte, byte> ShortCtRampOnOff = new KeyValuePair<byte, byte>(3, 1);

	public static KeyValuePair<byte, byte> ShortCtOnOffDelay = new KeyValuePair<byte, byte>(3, 2);

	public static KeyValuePair<byte, byte> ShortCtOnOff = new KeyValuePair<byte, byte>(3, 3);

	public static KeyValuePair<byte, byte> ShortCondValueLow = new KeyValuePair<byte, byte>(3, 4);

	public static KeyValuePair<byte, byte> ShortCondValueHigh = new KeyValuePair<byte, byte>(3, 5);

	public static KeyValuePair<byte, byte> ShortOnDelayTime = new KeyValuePair<byte, byte>(3, 6);

	public static KeyValuePair<byte, byte> ShortOnTime = new KeyValuePair<byte, byte>(3, 7);

	public static KeyValuePair<byte, byte> ShortOffDelayTime = new KeyValuePair<byte, byte>(3, 8);

	public static KeyValuePair<byte, byte> ShortOffTime = new KeyValuePair<byte, byte>(3, 9);

	public static KeyValuePair<byte, byte> ShortActionOperators = new KeyValuePair<byte, byte>(3, 10);

	public static KeyValuePair<byte, byte> ShortJtOnOff = new KeyValuePair<byte, byte>(3, 11);

	public static KeyValuePair<byte, byte> ShortJtOnOffDelay = new KeyValuePair<byte, byte>(3, 12);

	public static KeyValuePair<byte, byte> ShortJtRampOnOff = new KeyValuePair<byte, byte>(3, 13);

	public static KeyValuePair<byte, byte> ShortOffLevel = new KeyValuePair<byte, byte>(3, 14);

	public static KeyValuePair<byte, byte> ShortOnLevel = new KeyValuePair<byte, byte>(3, 15);

	public static KeyValuePair<byte, byte> ShortOnMinLevel = new KeyValuePair<byte, byte>(3, 16);

	public static KeyValuePair<byte, byte> ShortRampOnTime = new KeyValuePair<byte, byte>(3, 17);

	public static KeyValuePair<byte, byte> ShortRampOffTime = new KeyValuePair<byte, byte>(3, 18);

	public static KeyValuePair<byte, byte> ShortDimMinLevel = new KeyValuePair<byte, byte>(3, 19);

	public static KeyValuePair<byte, byte> ShortDimMaxLevel = new KeyValuePair<byte, byte>(3, 20);

	public static KeyValuePair<byte, byte> ShortCtRefOnOff = new KeyValuePair<byte, byte>(3, 21);

	public static KeyValuePair<byte, byte> ShortMaxTimeFirstDir = new KeyValuePair<byte, byte>(3, 22);

	public static KeyValuePair<byte, byte> ShortJtRefOnOff = new KeyValuePair<byte, byte>(3, 23);

	public static KeyValuePair<byte, byte> ShortDrivingMode = new KeyValuePair<byte, byte>(3, 24);

	public static KeyValuePair<byte, byte> LongCtRampOnOff = new KeyValuePair<byte, byte>(3, 129);

	public static KeyValuePair<byte, byte> LongCtOnOffDelay = new KeyValuePair<byte, byte>(3, 130);

	public static KeyValuePair<byte, byte> LongCtOnOff = new KeyValuePair<byte, byte>(3, 131);

	public static KeyValuePair<byte, byte> LongCondValueLow = new KeyValuePair<byte, byte>(3, 132);

	public static KeyValuePair<byte, byte> LongCondValueHigh = new KeyValuePair<byte, byte>(3, 133);

	public static KeyValuePair<byte, byte> LongOnDelayTime = new KeyValuePair<byte, byte>(3, 134);

	public static KeyValuePair<byte, byte> LongOnTime = new KeyValuePair<byte, byte>(3, 135);

	public static KeyValuePair<byte, byte> LongOffDelayTime = new KeyValuePair<byte, byte>(3, 136);

	public static KeyValuePair<byte, byte> LongOffTime = new KeyValuePair<byte, byte>(3, 137);

	public static KeyValuePair<byte, byte> LongActionOperators = new KeyValuePair<byte, byte>(3, 138);

	public static KeyValuePair<byte, byte> LongJtOnOff = new KeyValuePair<byte, byte>(3, 139);

	public static KeyValuePair<byte, byte> LongJtOnOffDelay = new KeyValuePair<byte, byte>(3, 140);

	public static KeyValuePair<byte, byte> LongJtRampOnOff = new KeyValuePair<byte, byte>(3, 141);

	public static KeyValuePair<byte, byte> LongOffLevel = new KeyValuePair<byte, byte>(3, 142);

	public static KeyValuePair<byte, byte> LongOnLevel = new KeyValuePair<byte, byte>(3, 143);

	public static KeyValuePair<byte, byte> LongOnMinLevel = new KeyValuePair<byte, byte>(3, 144);

	public static KeyValuePair<byte, byte> LongRampOnTime = new KeyValuePair<byte, byte>(3, 145);

	public static KeyValuePair<byte, byte> LongRampOffTime = new KeyValuePair<byte, byte>(3, 146);

	public static KeyValuePair<byte, byte> LongDimMinLevel = new KeyValuePair<byte, byte>(3, 147);

	public static KeyValuePair<byte, byte> LongDimMaxLevel = new KeyValuePair<byte, byte>(3, 148);

	public static KeyValuePair<byte, byte> LongCtRefOnOff = new KeyValuePair<byte, byte>(3, 149);

	public static KeyValuePair<byte, byte> LongMaxTimeFirstDir = new KeyValuePair<byte, byte>(3, 150);

	public static KeyValuePair<byte, byte> LongJtRefOnOff = new KeyValuePair<byte, byte>(3, 151);

	public static KeyValuePair<byte, byte> LongDrivingMode = new KeyValuePair<byte, byte>(3, 152);

	public static KeyValuePair<byte, byte> LinkConfigUpdatePending = new KeyValuePair<byte, byte>(4, 3);

	public static KeyValuePair<byte, byte> List11FrameType = new KeyValuePair<byte, byte>(11, 0);

	public static KeyValuePair<byte, byte> List11FirstSourceIpFirstByte = new KeyValuePair<byte, byte>(11, 1);

	public static KeyValuePair<byte, byte> List11FirstSourceIpSecondByte = new KeyValuePair<byte, byte>(11, 2);

	public static KeyValuePair<byte, byte> List11FirstSourceIpThirdByte = new KeyValuePair<byte, byte>(11, 3);

	public static KeyValuePair<byte, byte> List12FrameType = new KeyValuePair<byte, byte>(12, 0);

	public static KeyValuePair<byte, byte> List12FirstSourceIpFirstByte = new KeyValuePair<byte, byte>(12, 1);

	public static KeyValuePair<byte, byte> List12FirstSourceIpSecondByte = new KeyValuePair<byte, byte>(12, 2);

	public static KeyValuePair<byte, byte> List12FirstSourceIpThirdByte = new KeyValuePair<byte, byte>(12, 3);
}
