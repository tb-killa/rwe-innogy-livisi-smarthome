using System;

namespace onrkn;

internal class inouf : IComparable
{
	private int ushig;

	private string okjwr;

	public int jodkb => ushig;

	public string yvmaj => okjwr;

	public inouf(int preference, string exchange)
	{
		if (exchange == null || 1 == 0)
		{
			throw new ArgumentNullException("exchange");
		}
		ushig = preference;
		okjwr = exchange;
	}

	public override string ToString()
	{
		return brgjd.edcru("{0} ({1})", okjwr, ushig);
	}

	public int CompareTo(object obj)
	{
		if (!(obj is inouf inouf2) || 1 == 0)
		{
			return -1;
		}
		return ushig.CompareTo(inouf2.jodkb);
	}
}
