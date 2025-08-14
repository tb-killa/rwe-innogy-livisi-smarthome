using System;

namespace onrkn;

internal class zonvu : mggni, jghfk, cyhjf, gwbla, IDisposable, maowd
{
	private readonly ogvcc ydyso;

	private readonly rwfyw<mggni> xrtpt;

	public bool iuvyg => xrtpt.cbgyf;

	public zonvu(Func<mggni> createChannelFunc)
		: this(createChannelFunc, ogvcc.ehqoo)
	{
	}

	public zonvu(Func<mggni> createChannelFunc, ogvcc behavior)
	{
		if (createChannelFunc == null || 1 == 0)
		{
			throw new ArgumentNullException("createChannelFunc");
		}
		xrtpt = new rwfyw<mggni>(createChannelFunc);
		ydyso = behavior;
	}

	public void Dispose()
	{
		if ((xrtpt.cbgyf ? true : false) || (goalb() ? true : false))
		{
			xrtpt.avlfd.Dispose();
		}
	}

	public exkzi jhbpr()
	{
		if ((!xrtpt.cbgyf || 1 == 0) && (!goalb() || 1 == 0))
		{
			return rxpjc.fvxvk;
		}
		return xrtpt.avlfd.jhbpr();
	}

	public njvzu<int> rhjom(ArraySegment<byte> p0)
	{
		return xrtpt.avlfd.rhjom(p0);
	}

	public njvzu<int> razzy(ArraySegment<byte> p0)
	{
		return xrtpt.avlfd.razzy(p0);
	}

	public exkzi qxxgh()
	{
		if (!xrtpt.cbgyf || 1 == 0)
		{
			return rxpjc.iccat;
		}
		return xrtpt.avlfd.qxxgh();
	}

	public void gnmhy()
	{
		_ = xrtpt.avlfd;
	}

	private bool goalb()
	{
		return (ydyso & ogvcc.offgi) != 0;
	}
}
