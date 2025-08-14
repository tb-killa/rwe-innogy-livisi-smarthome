using System;
using System.Collections.Generic;
using Rebex.Mail;
using Rebex.Mime;
using Rebex.Mime.Headers;
using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class wcxxf
{
	public static bool ibmcb(string p0, bool p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_0039;
		IL_0006:
		if (p0[num] <= ' ' || p0[num] >= '\u007f')
		{
			if (p1 && 0 == 0)
			{
				throw new ArgumentException("Header name contains invalid characters.", p0);
			}
			return false;
		}
		num++;
		goto IL_0039;
		IL_0039:
		if (num < p0.Length)
		{
			goto IL_0006;
		}
		return true;
	}

	public static MailDateTime pxify(MimeHeaderCollection p0)
	{
		MailDateTime mailDateTime = null;
		string text = p0.GetRaw("received");
		if (text != null && 0 == 0)
		{
			int num = text.LastIndexOf(';');
			if (num >= 0)
			{
				text = text.Substring(num + 1);
			}
			try
			{
				MimeHeader mimeHeader = new MimeHeader("date", text);
				mailDateTime = mimeHeader.Value as MailDateTime;
			}
			catch
			{
			}
		}
		if (mailDateTime == null || false || mailDateTime.UniversalTime.Year > DateTime.Now.Year + 10)
		{
			return null;
		}
		return mailDateTime;
	}

	public static MailEncryptionAlgorithm ojbcd(EnvelopedData p0)
	{
		if (p0 == null || 1 == 0)
		{
			return MailEncryptionAlgorithm.Unsupported;
		}
		string value;
		if ((value = p0.ContentEncryptionAlgorithm.Oid.Value) != null && 0 == 0)
		{
			if (czzgh.qkmte == null || 1 == 0)
			{
				czzgh.qkmte = new Dictionary<string, int>(6)
				{
					{ "2.16.840.1.101.3.4.1.2", 0 },
					{ "2.16.840.1.101.3.4.1.22", 1 },
					{ "2.16.840.1.101.3.4.1.42", 2 },
					{ "1.2.840.113549.3.7", 3 },
					{ "1.3.14.3.2.7", 4 },
					{ "1.2.840.113549.3.2", 5 }
				};
			}
			if (czzgh.qkmte.TryGetValue(value, out var value2) && 0 == 0)
			{
				switch (value2)
				{
				case 0:
					return MailEncryptionAlgorithm.AES128;
				case 1:
					return MailEncryptionAlgorithm.AES192;
				case 2:
					return MailEncryptionAlgorithm.AES256;
				case 3:
					return MailEncryptionAlgorithm.TripleDES;
				case 4:
					return MailEncryptionAlgorithm.DES;
				case 5:
					return MailEncryptionAlgorithm.RC2;
				}
			}
		}
		return MailEncryptionAlgorithm.Unsupported;
	}
}
