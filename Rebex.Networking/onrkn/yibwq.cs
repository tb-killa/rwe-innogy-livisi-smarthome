namespace onrkn;

internal class yibwq
{
	private readonly object jcwpy;

	private readonly fnofn vlpgb;

	private readonly bvkts yqrzp;

	private bool zjdcf;

	private object[] ckuwe;

	public bool lrugr
	{
		get
		{
			lock (jcwpy)
			{
				return zjdcf;
			}
		}
	}

	public object[] nnfen => ckuwe;

	public fnofn ncfwa => vlpgb;

	public bvkts nnxoj => yqrzp;

	public yibwq(fnofn request, bvkts kind)
	{
		jcwpy = new object();
		vlpgb = request;
		yqrzp = kind;
	}

	public void prarm(qegof p0)
	{
		lock (jcwpy)
		{
			ckuwe = p0.ntypo;
			zjdcf = true;
		}
	}

	public void sbtkj()
	{
		lock (jcwpy)
		{
			zjdcf = true;
		}
	}
}
