using System;
using System.Text;

namespace onrkn;

internal class vdlbe : whhig
{
	private bool musqi;

	private bdhck wzecw;

	private int wumwv;

	private double yteuj;

	private double mujdr;

	private int ghotf;

	public bool wjruz
	{
		get
		{
			return musqi;
		}
		set
		{
			musqi = value;
		}
	}

	public bdhck pvocn
	{
		get
		{
			return wzecw;
		}
		set
		{
			wzecw = value;
		}
	}

	public int uwzfn
	{
		get
		{
			return wumwv;
		}
		set
		{
			wumwv = value;
		}
	}

	public double vdzuv
	{
		get
		{
			return yteuj;
		}
		private set
		{
			yteuj = value;
		}
	}

	public double rphmo
	{
		get
		{
			return mujdr;
		}
		private set
		{
			mujdr = value;
		}
	}

	public int boeuk
	{
		get
		{
			return ghotf;
		}
		set
		{
			ghotf = value;
		}
	}

	internal vdlbe(fpnng model)
		: base(model)
	{
	}

	internal override string dsdoz()
	{
		return htmrh() + jsday();
	}

	private string jsday()
	{
		vdzuv = zxzbp(uwzfn);
		rphmo = zxzbp(boeuk);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("padding:");
		if (vdzuv > 0.0)
		{
			stringBuilder.Append(brgjd.edcru("{0}em 0 ", vdzuv));
		}
		else
		{
			stringBuilder.Append("0 0 ");
		}
		if (rphmo > 0.0)
		{
			stringBuilder.Append(brgjd.edcru("{0}em 0;", rphmo));
		}
		else
		{
			stringBuilder.Append("0 0;");
		}
		if (stringBuilder.ToString() == "padding:0 0 0 0;" && 0 == 0)
		{
			return "";
		}
		return stringBuilder.ToString();
	}

	private double zxzbp(double p0)
	{
		if (p0 > 0.0)
		{
			double num = p0 / 20.0;
			double value = num / 11.9999998805;
			return Math.Round(value, 2);
		}
		return 0.0;
	}

	private string htmrh()
	{
		return pvocn switch
		{
			bdhck.wnclf => "text-align:center;", 
			bdhck.yeujp => "text-align:justify;", 
			bdhck.berjj => "text-align:left;", 
			bdhck.ifynk => "text-align:right;", 
			bdhck.llnyg => "text-align:justify;", 
			_ => "", 
		};
	}
}
