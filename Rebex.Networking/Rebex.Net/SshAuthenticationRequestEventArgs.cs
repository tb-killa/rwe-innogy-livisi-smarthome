using System;

namespace Rebex.Net;

public class SshAuthenticationRequestEventArgs : EventArgs
{
	private readonly string slsrg;

	private readonly string vuswr;

	private bool? mkqkr;

	private SshAuthenticationRequestItemCollection mzzys;

	public string Name => slsrg;

	public string Instructions => vuswr;

	public bool Cancel
	{
		get
		{
			bool? flag = mkqkr;
			if (flag == true && 0 == 0)
			{
				return flag.HasValue;
			}
			return false;
		}
		set
		{
			mkqkr = value;
		}
	}

	internal bool sradj => !mkqkr.HasValue;

	public SshAuthenticationRequestItemCollection Items => mzzys;

	public void Ignore()
	{
		mkqkr = null;
	}

	internal SshAuthenticationRequestEventArgs(string name, string instructions, string[] prompt, bool[] echo)
	{
		mkqkr = false;
		slsrg = name;
		vuswr = instructions;
		mzzys = new SshAuthenticationRequestItemCollection(prompt, echo);
	}
}
