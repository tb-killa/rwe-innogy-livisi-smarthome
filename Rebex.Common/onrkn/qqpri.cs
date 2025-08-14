using System;

namespace onrkn;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
internal class qqpri : Attribute
{
	private Type dsufu;

	public Type msihj
	{
		get
		{
			return dsufu;
		}
		private set
		{
			dsufu = value;
		}
	}

	public qqpri(Type stateMachineType)
	{
		msihj = stateMachineType;
	}
}
