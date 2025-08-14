using System;
using System.Threading;

namespace onrkn;

internal class vtkax<T0> where T0 : class
{
	private T0 wlstg;

	public T0 frars => wlstg;

	public vtkax()
	{
		wlstg = null;
	}

	public vtkax(T0 value)
	{
		wlstg = value;
	}

	public T0 jcdzm(T0 p0)
	{
		T0 val = wlstg;
		T0 val2;
		do
		{
			val2 = val;
			val = Interlocked.CompareExchange(ref wlstg, p0, val2);
		}
		while (!object.Equals(val, val2));
		return val;
	}

	public bool poozb(T0 p0)
	{
		T0 val = wlstg;
		T0 objB = Interlocked.CompareExchange(ref wlstg, p0, val);
		return object.Equals(val, objB);
	}

	public bool jupiv(T0 p0, Func<T0, T0, bool> p1)
	{
		T0 val = wlstg;
		if (!p1(val, p0) || 1 == 0)
		{
			return false;
		}
		T0 objB = Interlocked.CompareExchange(ref wlstg, p0, val);
		return object.Equals(val, objB);
	}

	public static implicit operator T0(vtkax<T0> atomic)
	{
		if (atomic != null && 0 == 0)
		{
			return atomic.frars;
		}
		return null;
	}
}
