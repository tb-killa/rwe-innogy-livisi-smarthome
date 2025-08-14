using System;

namespace RWE.SmartHome.SHC.SHCRelayDriver;

public class DelayRelayDriverManager
{
	private const int INCREASE_DELAY_STEP_SECONDS = 20;

	private const int MIN_DELAY_SEOCNDS = 20;

	private const int MAX_DELAY_SEOCNDS = 180;

	private int delaySeconds;

	public void MarkConnectionSuccess(bool isSuccess)
	{
		if (isSuccess)
		{
			delaySeconds = 0;
		}
		else if (delaySeconds == 0)
		{
			delaySeconds = 20;
		}
		else
		{
			delaySeconds = Math.Min(delaySeconds + 20, 180);
		}
	}

	public int GetDelaySeconds()
	{
		return delaySeconds;
	}
}
