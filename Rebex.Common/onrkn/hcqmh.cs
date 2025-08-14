namespace onrkn;

internal class hcqmh<T0, T1>
{
	private T0 ymtoz;

	private T1 waeej;

	public T0 amanf
	{
		get
		{
			return ymtoz;
		}
		private set
		{
			ymtoz = value;
		}
	}

	public T1 cdois
	{
		get
		{
			return waeej;
		}
		private set
		{
			waeej = value;
		}
	}

	public hcqmh(T0 item1, T1 item2)
	{
		amanf = item1;
		cdois = item2;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is hcqmh<T0, T1> hcqmh2) || 1 == 0)
		{
			return false;
		}
		if (amanf == null || 1 == 0)
		{
			if (hcqmh2.amanf != null && 0 == 0)
			{
				return false;
			}
		}
		else if (!amanf.Equals(hcqmh2.amanf) || 1 == 0)
		{
			return false;
		}
		if (cdois == null || 1 == 0)
		{
			if (hcqmh2.cdois != null && 0 == 0)
			{
				return false;
			}
		}
		else if (!cdois.Equals(hcqmh2.cdois) || 1 == 0)
		{
			return false;
		}
		return true;
	}

	public override int GetHashCode()
	{
		return amanf.GetHashCode() ^ cdois.GetHashCode();
	}
}
