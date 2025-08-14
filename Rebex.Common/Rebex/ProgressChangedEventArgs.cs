using System;

namespace Rebex;

public class ProgressChangedEventArgs : EventArgs
{
	private int vbcfn;

	private object jbfxp;

	public int ProgressPercentage => vbcfn;

	public object UserState => jbfxp;

	public ProgressChangedEventArgs(int progressPercentage, object userState)
	{
		vbcfn = progressPercentage;
		jbfxp = userState;
	}
}
