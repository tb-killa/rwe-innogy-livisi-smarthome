using System;

namespace onrkn;

internal struct vdffb<T0>
{
	private const string zffor = "Some is not initialized!";

	private readonly T0 zlybc;

	public T0 yzhuz
	{
		get
		{
			if (zlybc == null || 1 == 0)
			{
				throw new InvalidOperationException("Some is not initialized!");
			}
			return zlybc;
		}
	}

	public vdffb(T0 value)
	{
		this = default(vdffb<T0>);
		if (value == null || 1 == 0)
		{
			throw new ArgumentNullException("value");
		}
		zlybc = value;
	}
}
