using System;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces;

public class AccountlessShcException : Exception
{
	public AccountlessShcException(string message)
		: base(message)
	{
	}
}
