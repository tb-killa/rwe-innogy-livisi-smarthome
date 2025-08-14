using System;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Enums;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;

public interface IDeviceController
{
	Guid ChangeDeviceState(RampMode rampMode, int rampTime, byte state, byte targetChannel, int offTimer);

	Guid SendSwitchCommand(ActivationTime activationTime, byte[] sourceAddress, byte channel, byte keyStrokeCounter, byte? decisionValue);

	Guid RequestStatusInfo(byte targetChannel);

	Guid SendTestSound(byte[] sourceAddress, byte channel, byte soundId, byte currentSoundId, int delayMs);

	Guid SendVirtualConfigCommand(byte[] sourceAddress, byte channel, byte[] values);
}
