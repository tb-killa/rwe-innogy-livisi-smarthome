using System;

namespace onrkn;

internal static class ttius
{
	private sealed class oweyt
	{
		public Action atzyz;

		public void fpuuw(exkzi p0)
		{
			atzyz();
		}
	}

	static ttius()
	{
		rxpjc.tjzre();
	}

	internal static void thhob(exkzi p0, Action p1, bool p2)
	{
		Action<exkzi> action = null;
		oweyt oweyt = new oweyt();
		oweyt.atzyz = p1;
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("task");
		}
		if (oweyt.atzyz == null || 1 == 0)
		{
			throw new ArgumentNullException("continuation");
		}
		if (action == null || 1 == 0)
		{
			action = oweyt.fpuuw;
		}
		Action<exkzi> p3 = action;
		p0.kvzxl(p3);
	}
}
