namespace onrkn;

internal class ocwfj<T0, T1, T2>
{
	private T0 vdazl;

	private T1 uponc;

	private T2 bmoxo;

	public T0 shrdw
	{
		get
		{
			return vdazl;
		}
		private set
		{
			vdazl = value;
		}
	}

	public T1 vqhck
	{
		get
		{
			return uponc;
		}
		private set
		{
			uponc = value;
		}
	}

	public T2 anmqs
	{
		get
		{
			return bmoxo;
		}
		private set
		{
			bmoxo = value;
		}
	}

	public ocwfj(T0 item1, T1 item2, T2 item3)
	{
		shrdw = item1;
		vqhck = item2;
		anmqs = item3;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ocwfj<T0, T1, T2> ocwfj2) || 1 == 0)
		{
			return false;
		}
		if (shrdw == null || 1 == 0)
		{
			if (ocwfj2.shrdw != null && 0 == 0)
			{
				return false;
			}
		}
		else if (!shrdw.Equals(ocwfj2.shrdw) || 1 == 0)
		{
			return false;
		}
		if (vqhck == null || 1 == 0)
		{
			if (ocwfj2.vqhck != null && 0 == 0)
			{
				return false;
			}
		}
		else if (!vqhck.Equals(ocwfj2.vqhck) || 1 == 0)
		{
			return false;
		}
		if (anmqs == null || 1 == 0)
		{
			if (ocwfj2.anmqs != null && 0 == 0)
			{
				return false;
			}
		}
		else if (!anmqs.Equals(ocwfj2.anmqs) || 1 == 0)
		{
			return false;
		}
		return true;
	}

	public override int GetHashCode()
	{
		return shrdw.GetHashCode() ^ vqhck.GetHashCode() ^ anmqs.GetHashCode();
	}
}
