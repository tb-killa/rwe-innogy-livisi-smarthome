namespace onrkn;

internal abstract class lbexf
{
	private readonly string kgfgn;

	public virtual int vlbbc => kgfgn.Length;

	protected lbexf(string contents)
	{
		kgfgn = contents;
	}

	public override string ToString()
	{
		return kgfgn;
	}
}
