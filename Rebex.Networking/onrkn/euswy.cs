namespace onrkn;

internal class euswy : mkuxt
{
	private string[] rsflr;

	public override void jfjrs(tndeg p0)
	{
		mkuxt.agnqw(p0, 61);
		mkuxt.ebmel(p0, (uint)rsflr.Length);
		int num = 0;
		if (num != 0)
		{
			goto IL_0018;
		}
		goto IL_0033;
		IL_0018:
		mkuxt.excko(p0, rsflr[num]);
		num++;
		goto IL_0033;
		IL_0033:
		if (num >= rsflr.Length)
		{
			return;
		}
		goto IL_0018;
	}

	public euswy(params string[] responses)
	{
		rsflr = responses;
	}
}
