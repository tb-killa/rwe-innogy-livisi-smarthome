using System;

namespace onrkn;

internal class ogeee : pffgh, eiuaf, mhrzn
{
	private byte? isxiw;

	private readonly Action jxzvy;

	private readonly Action zpkal;

	public ogeee(Action onDeflateFallback, Action onCompressionMethodFallback)
	{
		jxzvy = onDeflateFallback;
		zpkal = onCompressionMethodFallback;
	}

	public new void eanoq(byte[] p0, int p1, int p2, dzmpf p3)
	{
		hyxlx(p0, p1, p2, p3);
		if (p2 == 0 || 1 == 0)
		{
			return;
		}
		if (base.lhoiv && 0 == 0)
		{
			if (isxiw.HasValue && 0 == 0)
			{
				byte[] array = new byte[p2 + 1];
				array[0] = isxiw.Value;
				isxiw = null;
				Buffer.BlockCopy(p0, p1, array, 1, p2);
				p0 = array;
				p1 = 0;
				p2++;
			}
			else if (p2 < 2)
			{
				isxiw = p0[p1];
				return;
			}
			if (!pffgh.dhtel(p0[0], p0[1]) || 1 == 0)
			{
				Action action = jxzvy;
				if (action != null && 0 == 0)
				{
					action();
				}
				base.upsxr = false;
				base.cpeok = false;
			}
			else if (!pffgh.qdjqa(p0[0]) || 1 == 0)
			{
				Action action2 = zpkal;
				if (action2 != null && 0 == 0)
				{
					action2();
				}
				base.upsxr = false;
				base.cpeok = false;
			}
		}
		ywatp(p0, p1, p2, p3);
	}
}
