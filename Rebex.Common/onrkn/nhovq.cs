namespace onrkn;

internal abstract class nhovq : eyqzi, wpvpu
{
	private hqxly mrhfm;

	public bool kulmy => (mrhfm & hqxly.oigov) != 0;

	protected nhovq(hqxly accessMode)
	{
		mrhfm = accessMode;
	}

	public void vexdc()
	{
		xhihh(mrhfm);
	}

	public void xhonm()
	{
		jekgy(mrhfm);
	}

	public static void xhihh(hqxly p0)
	{
		if ((p0 & hqxly.xpmbl) == 0 || 1 == 0)
		{
			throw new nfcev(fvjcl.ovavv, "File not open for reading.");
		}
	}

	public static void jekgy(hqxly p0)
	{
		if ((p0 & hqxly.oigov) == 0 || 1 == 0)
		{
			throw new nfcev(fvjcl.hznby, "File not open for writing.");
		}
	}

	public abstract int mlhqn(byte[] p0, int p1, int p2);

	public abstract void eesbc(byte[] p0, int p1, int p2);

	public abstract long qgsek(vnfav p0, long p1);

	public abstract void rclzx(vgycx p0);

	public abstract void epmxq(bool p0);

	public abstract void mreei();

	public void qvtyb()
	{
		epmxq(p0: true);
	}
}
