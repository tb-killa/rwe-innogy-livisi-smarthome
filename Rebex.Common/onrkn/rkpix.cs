namespace onrkn;

internal class rkpix
{
	private int rfall;

	private int jyyxk;

	private int utnii;

	private ffooh fovqh;

	public int tqhsq
	{
		get
		{
			return rfall;
		}
		private set
		{
			rfall = value;
		}
	}

	public int ngtpx
	{
		get
		{
			return jyyxk;
		}
		private set
		{
			jyyxk = value;
		}
	}

	public int xoepc
	{
		get
		{
			return utnii;
		}
		private set
		{
			utnii = value;
		}
	}

	public ffooh tqhyl
	{
		get
		{
			return fovqh;
		}
		private set
		{
			fovqh = value;
		}
	}

	public rkpix(ffooh type, int memorySizeInKB, int iterations, int degreeOfParallelism)
	{
		tqhsq = memorySizeInKB;
		ngtpx = iterations;
		xoepc = degreeOfParallelism;
		tqhyl = type;
	}

	public override string ToString()
	{
		return brgjd.edcru("Type: {0}, Iterations: {1}, DegreeOfParallelism: {2}, MemorySizeInKB: {3}", tqhyl, ngtpx, xoepc, tqhsq);
	}
}
