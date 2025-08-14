using System;
using System.Collections;
using Rebex.Net;

namespace onrkn;

internal class bxqhd
{
	private readonly ArrayList fmmur = new ArrayList();

	public SmtpRejectedRecipient[] zvlxm => (SmtpRejectedRecipient[])fmmur.ToArray(typeof(SmtpRejectedRecipient));

	public void uyeot(string p0, Exception p1)
	{
		fmmur.Add(new SmtpRejectedRecipient(p0, p1));
	}

	public void pyyod(object p0, SmtpRejectedRecipientEventArgs p1)
	{
		p1.Ignore = true;
		fmmur.Add(new SmtpRejectedRecipient(p1.Recipient, p1.Response));
	}
}
