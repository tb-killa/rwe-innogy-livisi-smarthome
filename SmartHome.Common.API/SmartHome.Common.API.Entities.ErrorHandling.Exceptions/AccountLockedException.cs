using System;

namespace SmartHome.Common.API.Entities.ErrorHandling.Exceptions;

public class AccountLockedException : Exception
{
	private readonly int secondsToWait;

	public int SecondsToWait => secondsToWait;

	public AccountLockedException(int secondsToWait)
	{
		this.secondsToWait = secondsToWait;
	}
}
