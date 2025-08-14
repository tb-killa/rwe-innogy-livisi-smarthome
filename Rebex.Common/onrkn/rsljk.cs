using System.Reflection;

namespace onrkn;

internal class rsljk
{
	private Assembly sugda;

	private ivdov ifszg;

	private string ouhbj;

	private int kgjgi;

	private string uwbqa;

	private string vkkgk;

	private cltxh icsuv;

	public Assembly dmses
	{
		get
		{
			return sugda;
		}
		private set
		{
			sugda = value;
		}
	}

	public ivdov hfdnx
	{
		get
		{
			return ifszg;
		}
		private set
		{
			ifszg = value;
		}
	}

	public string xarpt
	{
		get
		{
			return ouhbj;
		}
		private set
		{
			ouhbj = value;
		}
	}

	public int cilda
	{
		get
		{
			return kgjgi;
		}
		private set
		{
			kgjgi = value;
		}
	}

	public string aoyjd
	{
		get
		{
			return uwbqa;
		}
		private set
		{
			uwbqa = value;
		}
	}

	public string uwvwe
	{
		get
		{
			return vkkgk;
		}
		private set
		{
			vkkgk = value;
		}
	}

	public cltxh vfpmi
	{
		get
		{
			return icsuv;
		}
		private set
		{
			icsuv = value;
		}
	}

	public rsljk(ivdov id, string name, int build, string version, string platform, cltxh edition)
	{
		if (!dahxy.hdfhq || 1 == 0)
		{
			try
			{
				dmses = Assembly.GetCallingAssembly();
			}
			catch
			{
				dmses = null;
			}
		}
		hfdnx = id;
		xarpt = "Rebex." + name;
		aoyjd = version;
		uwvwe = platform;
		vfpmi = edition;
		cilda = build;
	}

	public override string ToString()
	{
		return xarpt;
	}
}
