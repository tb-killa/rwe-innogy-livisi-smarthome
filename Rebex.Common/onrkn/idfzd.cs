using System;
using System.Reflection;

namespace onrkn;

internal static class idfzd
{
	private static readonly MethodInfo pxpns = rzthf("IsImmutableAgileException", BindingFlags.Static | BindingFlags.NonPublic);

	private static readonly MethodInfo vhmlu;

	public static void irjxm(this Exception p0)
	{
		cgfaf(p0);
	}

	private static void cgfaf(Exception p0)
	{
		if (!bmlix(p0) || 1 == 0)
		{
			qsixt();
		}
		throw p0;
	}

	private static bool bmlix(Exception p0)
	{
		if ((object)pxpns != null && 0 == 0)
		{
			try
			{
				return (bool)pxpns.Invoke(null, new object[1] { p0 });
			}
			catch
			{
			}
		}
		return false;
	}

	private static void qsixt()
	{
		if ((object)vhmlu != null && 0 == 0)
		{
			try
			{
				vhmlu.Invoke(null, null);
			}
			catch
			{
			}
		}
	}

	private static MethodInfo rzthf(string p0, BindingFlags p1)
	{
		try
		{
			return typeof(Exception).GetMethod(p0, p1);
		}
		catch
		{
			return null;
		}
	}

	static idfzd()
	{
		MethodInfo methodInfo = rzthf("PrepareForForeignExceptionRaise", BindingFlags.Static | BindingFlags.NonPublic);
		if ((object)methodInfo == null || 1 == 0)
		{
			methodInfo = rzthf("PrepForRemoting", BindingFlags.Static | BindingFlags.NonPublic);
		}
		vhmlu = methodInfo;
	}
}
