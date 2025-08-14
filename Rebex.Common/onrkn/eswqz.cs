using System;

namespace onrkn;

internal class eswqz<T0> : rhzda<T0>
{
	private readonly Func<T0> ljddp;

	private readonly Action<T0> bfnnp;

	private readonly Action<T0> bnfah;

	public eswqz(int maxSize, Func<T0> create)
		: base(maxSize)
	{
		if (create == null || 1 == 0)
		{
			throw new ArgumentNullException("create");
		}
		ljddp = create;
	}

	public eswqz(int maxSize, Func<T0> create, Action<T0> cleanUp, Action<T0> destroy)
		: this(maxSize, create)
	{
		bfnnp = cleanUp;
		bnfah = destroy;
	}

	protected override void gdvxj(T0 p0)
	{
		if (bfnnp != null && 0 == 0)
		{
			bfnnp(p0);
		}
	}

	protected override T0 qbuja()
	{
		return ljddp();
	}

	protected override void cjyoc(T0 p0)
	{
		if (bnfah != null && 0 == 0)
		{
			bnfah(p0);
		}
	}
}
