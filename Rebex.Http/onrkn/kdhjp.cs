using System;
using System.Net;
using Rebex.Net;

namespace onrkn;

internal class kdhjp : IWebRequestCreate
{
	private static kdhjp ljdhh;

	private HttpRequestCreator wieyz;

	public static kdhjp euiqt => ljdhh;

	private kdhjp()
	{
	}

	static kdhjp()
	{
		ljdhh = new kdhjp();
	}

	private WebRequest ynsaa(Uri p0)
	{
		if (wieyz == null || 1 == 0)
		{
			throw new InvalidOperationException("Call HttpRequestCreator.Register first.");
		}
		return wieyz.Create(p0);
	}

	WebRequest IWebRequestCreate.Create(Uri p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ynsaa
		return this.ynsaa(p0);
	}

	internal void bgkai(HttpRequestCreator p0)
	{
		wieyz = p0;
	}
}
