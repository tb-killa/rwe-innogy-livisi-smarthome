namespace onrkn;

internal class prjlw
{
	private static readonly prjlw trhzi = new prjlw();

	private int iiwya;

	private int cqwke;

	public int sopyo
	{
		get
		{
			return iiwya;
		}
		set
		{
			iiwya = value;
		}
	}

	public int lvrxy
	{
		get
		{
			return cqwke;
		}
		set
		{
			cqwke = value;
		}
	}

	public static prjlw mrnmb => trhzi;

	public prjlw()
	{
		lvrxy = 16;
		sopyo = 16;
	}
}
