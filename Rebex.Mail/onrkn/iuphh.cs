using System.Text;

namespace onrkn;

internal class iuphh : bgxyp
{
	private dmcjx hvmqa;

	private gnadp nmddf;

	private qbhyy mnsov;

	private oxbyz kgbzo;

	public dmcjx cydme
	{
		get
		{
			return hvmqa;
		}
		set
		{
			hvmqa = value;
		}
	}

	public gnadp ozxic
	{
		get
		{
			return nmddf;
		}
		set
		{
			nmddf = value;
		}
	}

	public oxbyz shgkl
	{
		get
		{
			return kgbzo;
		}
		set
		{
			kgbzo = value;
		}
	}

	public qbhyy diubo
	{
		get
		{
			return mnsov;
		}
		set
		{
			mnsov = value;
		}
	}

	internal iuphh(fpnng model)
		: base(model)
	{
		hvmqa = new dmcjx(model);
		nmddf = new gnadp(model);
		kgbzo = new oxbyz(model);
		mnsov = new qbhyy(model);
	}

	public override bgxyp xakax()
	{
		iuphh iuphh2 = new iuphh(hokmy);
		iuphh2.cydme.taxdl = cydme.taxdl;
		iuphh2.cydme.hrait = cydme.hrait;
		iuphh2.ozxic.lljxt = ozxic.lljxt;
		iuphh2.ozxic.jdcus = ozxic.jdcus;
		iuphh2.ozxic.hchkn = ozxic.hchkn;
		iuphh2.ozxic.xsiqy = ozxic.xsiqy;
		iuphh2.ozxic.ywrnu = ozxic.ywrnu;
		iuphh2.ozxic.iekwa = ozxic.iekwa;
		iuphh2.ozxic.uqwbx = ozxic.uqwbx;
		iuphh2.ozxic.qsrru = ozxic.qsrru;
		iuphh2.ozxic.geabl = ozxic.geabl;
		iuphh2.ozxic.lfwjp = ozxic.lfwjp;
		iuphh2.shgkl.xtrgh = shgkl.xtrgh;
		iuphh2.shgkl.mzdmu = shgkl.mzdmu;
		iuphh2.diubo.dpodp = diubo.dpodp;
		return iuphh2;
	}

	internal override string dsdoz()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (hvmqa.taxdl != null && 0 == 0)
		{
			string value = hokmy.ninlm(hvmqa.hrait);
			stringBuilder.Append(value);
		}
		stringBuilder.Append(mnsov.twxvb((nmddf.uqwbx ? true : false) || nmddf.qsrru));
		if (nmddf.jdcus && 0 == 0)
		{
			stringBuilder.Append("font-style:italic;");
		}
		if (nmddf.lljxt && 0 == 0)
		{
			stringBuilder.Append("font-weight:bold;");
		}
		if (nmddf.hchkn && 0 == 0)
		{
			stringBuilder.Append("text-decoration:underline;");
		}
		if ((nmddf.xsiqy ? true : false) || nmddf.ywrnu)
		{
			stringBuilder.Append("text-decoration:line-through;");
		}
		if (nmddf.iekwa && 0 == 0)
		{
			stringBuilder.Append("background-color: #FFFF00;");
		}
		if (nmddf.uqwbx && 0 == 0)
		{
			stringBuilder.Append("vertical-align:sub;");
		}
		else if (nmddf.qsrru && 0 == 0)
		{
			stringBuilder.Append("vertical-align:super;");
		}
		if (nmddf.geabl && 0 == 0)
		{
			stringBuilder.Append("text-shadow:-0.03em -0.03em 0.01em black,0.03em -0.03em 0 grey,-0.03em 0.03em 0 grey,0.03em 0.03em 0 grey;");
		}
		else if (nmddf.lfwjp && 0 == 0)
		{
			stringBuilder.Append("text-shadow:0.03em 0.03em 0.01em black,0.03em -0.03em 0 grey,-0.03em 0.03em 0 grey,-0.03em -0.03em 0 grey;");
		}
		if (kgbzo.xtrgh != null && 0 == 0)
		{
			string value2 = hokmy.llbka(kgbzo.mzdmu);
			stringBuilder.Append(value2);
		}
		return stringBuilder.ToString();
	}

	public override bool Equals(object obj)
	{
		if (obj == null || 1 == 0)
		{
			return false;
		}
		if (!(obj is iuphh iuphh2) || 1 == 0)
		{
			return false;
		}
		if (mnsov.Equals(iuphh2.diubo) && 0 == 0 && kgbzo.Equals(iuphh2.shgkl) && 0 == 0 && nmddf.Equals(iuphh2.ozxic) && 0 == 0)
		{
			return hvmqa.Equals(iuphh2.cydme);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
