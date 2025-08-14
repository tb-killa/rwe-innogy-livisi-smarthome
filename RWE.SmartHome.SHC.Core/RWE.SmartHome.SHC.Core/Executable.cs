using System;

namespace RWE.SmartHome.SHC.Core;

public class Executable<T> : IExecutable
{
	public Action<T> Action { get; set; }

	public T Argument { get; set; }

	public void Execute()
	{
		Action(Argument);
	}
}
