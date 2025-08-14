using System;

namespace onrkn;

internal static class ihlbr
{
	private sealed class jtbah
	{
		public IAsyncResult fvctm;

		public AsyncCallback ozmer;

		public void rgwmz(exkzi p0)
		{
			ozmer(fvctm);
		}
	}

	public static TR aevhw<TR>(IAsyncResult p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("asyncResult");
		}
		njvzu<TR> njvzu2 = ((!(p0 is faagu faagu2)) ? ((njvzu<TR>)p0) : ((njvzu<TR>)faagu2.ajklf));
		njvzu2.xgngc();
		return njvzu2.islme;
	}

	public static void reuwd(IAsyncResult p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("asyncResult");
		}
		exkzi p1 = ((!(p0 is faagu faagu2)) ? ((exkzi)p0) : faagu2.ajklf);
		p1.xgngc();
	}

	public static IAsyncResult fgnul(AsyncCallback p0, object p1, exkzi p2)
	{
		Action<exkzi> action = null;
		jtbah jtbah = new jtbah();
		jtbah.ozmer = p0;
		object fvctm;
		if (p1 != null && 0 == 0)
		{
			IAsyncResult asyncResult = new faagu(p2, p1);
			fvctm = asyncResult;
		}
		else
		{
			fvctm = p2;
		}
		jtbah.fvctm = (IAsyncResult)fvctm;
		if (jtbah.ozmer != null && 0 == 0)
		{
			if (action == null || 1 == 0)
			{
				action = jtbah.rgwmz;
			}
			p2.kvzxl(action);
		}
		return jtbah.fvctm;
	}
}
