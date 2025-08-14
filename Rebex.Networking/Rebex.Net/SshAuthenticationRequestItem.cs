namespace Rebex.Net;

public class SshAuthenticationRequestItem
{
	private string pkcqe;

	private string lthxt;

	private bool welss;

	public string Prompt => pkcqe;

	public string Response
	{
		get
		{
			return lthxt;
		}
		set
		{
			lthxt = value;
		}
	}

	public bool IsSecret => welss;

	internal SshAuthenticationRequestItem(string prompt, bool isSecret)
	{
		pkcqe = prompt;
		welss = isSecret;
	}
}
