namespace onrkn;

internal class dmcjx : ssdlg
{
	private string bsani;

	private int kvowd;

	public string taxdl
	{
		get
		{
			return bsani;
		}
		set
		{
			bsani = value;
		}
	}

	public int hrait
	{
		get
		{
			return kvowd;
		}
		set
		{
			kvowd = value;
		}
	}

	internal dmcjx(fpnng model)
		: base(model)
	{
		bsani = null;
	}

	internal dmcjx(string fontTypeName, int fontId, fpnng model)
		: base(model)
	{
		bsani = fontTypeName;
		kvowd = fontId;
	}

	public override bool Equals(object o)
	{
		bool result = false;
		if (o is dmcjx && 0 == 0 && o != null && 0 == 0)
		{
			dmcjx dmcjx2 = (dmcjx)o;
			if (taxdl == dmcjx2.taxdl && 0 == 0)
			{
				result = true;
			}
		}
		return result;
	}

	public override int GetHashCode()
	{
		return taxdl.GetHashCode();
	}
}
