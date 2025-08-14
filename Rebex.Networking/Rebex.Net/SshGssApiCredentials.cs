using System;

namespace Rebex.Net;

public class SshGssApiCredentials : ICloneable
{
	internal const string bprrx = "1.3.6.1.4.1.311.2.2.10";

	internal const string jaexg = "1.2.840.113554.1.2.2";

	private string jswhf;

	private string kwqrm;

	private string lvkpm;

	private string qnuak;

	private string jyfmc;

	private SshGssApiMechanisms[] woncy;

	private bool utgql;

	public string UserName
	{
		get
		{
			return jswhf;
		}
		set
		{
			jswhf = value;
		}
	}

	public string Domain
	{
		get
		{
			return kwqrm;
		}
		set
		{
			kwqrm = value;
		}
	}

	public string Password
	{
		get
		{
			return lvkpm;
		}
		set
		{
			lvkpm = value;
		}
	}

	public string TargetName
	{
		get
		{
			return qnuak;
		}
		set
		{
			qnuak = value;
		}
	}

	public string AccountName
	{
		get
		{
			return jyfmc;
		}
		set
		{
			jyfmc = value;
		}
	}

	public bool AllowDelegation
	{
		get
		{
			return utgql;
		}
		set
		{
			utgql = value;
		}
	}

	public SshGssApiCredentials Clone()
	{
		SshGssApiCredentials sshGssApiCredentials = new SshGssApiCredentials(jswhf, lvkpm, kwqrm);
		sshGssApiCredentials.qnuak = qnuak;
		sshGssApiCredentials.jyfmc = jyfmc;
		sshGssApiCredentials.woncy = woncy;
		sshGssApiCredentials.utgql = utgql;
		return sshGssApiCredentials;
	}

	private object owpkv()
	{
		return Clone();
	}

	object ICloneable.Clone()
	{
		//ILSpy generated this explicit interface implementation from .override directive in owpkv
		return this.owpkv();
	}

	internal string[] omips()
	{
		string[] array = new string[woncy.Length];
		int num = 0;
		if (num != 0)
		{
			goto IL_0014;
		}
		goto IL_0045;
		IL_0014:
		string text;
		switch (woncy[num])
		{
		case SshGssApiMechanisms.Ntlm:
			text = "1.3.6.1.4.1.311.2.2.10";
			goto IL_003d;
		case SshGssApiMechanisms.KerberosV5:
			{
				text = "1.2.840.113554.1.2.2";
				goto IL_003d;
			}
			IL_003d:
			array[num] = text;
			break;
		}
		num++;
		goto IL_0045;
		IL_0045:
		if (num < array.Length)
		{
			goto IL_0014;
		}
		return array;
	}

	public SshGssApiCredentials()
	{
		SetMechanisms(null);
	}

	public SshGssApiCredentials(string userName, string password, string domain)
		: this()
	{
		jswhf = userName;
		kwqrm = domain;
		lvkpm = password;
	}

	public SshGssApiMechanisms[] GetMechanisms()
	{
		return (SshGssApiMechanisms[])woncy.Clone();
	}

	public void SetMechanisms(params SshGssApiMechanisms[] mechanisms)
	{
		if (mechanisms == null || false || mechanisms.Length == 0 || 1 == 0)
		{
			woncy = new SshGssApiMechanisms[2]
			{
				SshGssApiMechanisms.KerberosV5,
				SshGssApiMechanisms.Ntlm
			};
			return;
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0039;
		}
		goto IL_0065;
		IL_0065:
		if (num < mechanisms.Length)
		{
			goto IL_0039;
		}
		woncy = (SshGssApiMechanisms[])mechanisms.Clone();
		return;
		IL_0039:
		switch (mechanisms[num])
		{
		default:
			throw new ArgumentException("Unsupported GSSAPI mechanism.", "mechanisms");
		case SshGssApiMechanisms.Ntlm:
		case SshGssApiMechanisms.KerberosV5:
			break;
		}
		num++;
		goto IL_0065;
	}
}
