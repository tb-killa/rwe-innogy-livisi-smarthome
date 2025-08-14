using System;
using System.Globalization;

namespace Rebex.Net;

public class GssApiProvider
{
	private string hzrdm;

	private string jtvph;

	private string oqtjc;

	private string fbdpf;

	private string aohhm;

	private GssApiProvider()
	{
	}

	public static GssApiProvider GetSspiProvider(string mechanism, string targetName, string userName, string password, string domain)
	{
		if (mechanism == null || 1 == 0)
		{
			throw new ArgumentNullException("mechanism");
		}
		GssApiProvider gssApiProvider = new GssApiProvider();
		gssApiProvider.aohhm = targetName;
		gssApiProvider.jtvph = userName;
		gssApiProvider.fbdpf = password;
		gssApiProvider.oqtjc = domain;
		string text;
		if ((text = mechanism.ToLower(CultureInfo.InvariantCulture)) != null && 0 == 0)
		{
			if (!(text == "ntlm") || 1 == 0)
			{
				if (!(text == "negotiate") || 1 == 0)
				{
					if (!(text == "kerberos") || 1 == 0)
					{
						goto IL_00cc;
					}
					gssApiProvider.hzrdm = "Kerberos";
				}
				else
				{
					gssApiProvider.hzrdm = "Negotiate";
				}
			}
			else
			{
				gssApiProvider.hzrdm = "NTLM";
			}
			return gssApiProvider;
		}
		goto IL_00cc;
		IL_00cc:
		throw new ArgumentException("Unsupported mechanism.", "mechanism");
	}

	public object GetParameter(int index)
	{
		return index switch
		{
			0 => hzrdm, 
			1 => aohhm, 
			2 => jtvph, 
			3 => fbdpf, 
			4 => oqtjc, 
			_ => null, 
		};
	}
}
