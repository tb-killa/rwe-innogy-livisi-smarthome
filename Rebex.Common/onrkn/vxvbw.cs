using System;

namespace onrkn;

internal struct vxvbw<T0>
{
	private dvxgu<T0> ropcf;

	public njvzu<T0> xieya => ropcf.dioyl;

	public static vxvbw<T0> rdzxj()
	{
		return new vxvbw<T0>
		{
			ropcf = new dvxgu<T0>()
		};
	}

	public void viwxd(fgyyk p0)
	{
	}

	public void vzyck(T0 p0)
	{
		ropcf.cbgge(p0);
	}

	public void tudwl(Exception p0)
	{
		if (p0 is lmqll && 0 == 0)
		{
			ropcf.gxdnj();
		}
		else
		{
			ropcf.dassc(p0);
		}
	}

	public void vklen<TStateMachine>(ref TStateMachine p0) where TStateMachine : fgyyk
	{
		p0.tkrrn();
	}

	public void xiwgo<TAwaiter, TStateMachine>(ref TAwaiter p0, ref TStateMachine p1) where TAwaiter : uentq where TStateMachine : fgyyk
	{
		Action p2 = ((fgyyk)p1).tkrrn;
		p0.vcbew(p2);
	}

	public void vtyzj<TAwaiter, TStateMachine>(ref TAwaiter p0, ref TStateMachine p1) where TAwaiter : ihdoj where TStateMachine : fgyyk
	{
		Action p2 = ((fgyyk)p1).tkrrn;
		p0.ymoit(p2);
	}
}
