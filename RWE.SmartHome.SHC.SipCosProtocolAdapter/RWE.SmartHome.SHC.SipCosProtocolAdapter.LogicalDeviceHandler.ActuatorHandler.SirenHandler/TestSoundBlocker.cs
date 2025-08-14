using System;
using RWE.SmartHome.SHC.DomainModel.Types;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.LogicalDeviceHandler.ActuatorHandler.SirenHandler;

public class TestSoundBlocker
{
	private bool isBlocked;

	private TimeSpan timeout;

	private DateTime lastStartedTime;

	public TestSoundBlocker(TimeSpan timeout)
	{
		isBlocked = false;
		this.timeout = timeout;
		lastStartedTime = DateTime.MinValue;
	}

	public bool IsFree()
	{
		return !IsBlocked();
	}

	public bool IsBlocked()
	{
		if (isBlocked)
		{
			return lastStartedTime.Add(timeout) > DateTime.UtcNow;
		}
		return false;
	}

	public void BlockTestSound()
	{
		isBlocked = true;
		lastStartedTime = DateTime.UtcNow;
	}

	public void UnblockTestSound()
	{
		isBlocked = false;
	}

	public void UpdateTestSoundBlock(SIRChannel channel)
	{
		if (channel == SIRChannel.None)
		{
			UnblockTestSound();
		}
		else
		{
			BlockTestSound();
		}
	}
}
