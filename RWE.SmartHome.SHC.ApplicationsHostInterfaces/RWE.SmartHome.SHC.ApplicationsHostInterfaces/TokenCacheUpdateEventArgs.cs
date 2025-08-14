using System;

namespace RWE.SmartHome.SHC.ApplicationsHostInterfaces;

public class TokenCacheUpdateEventArgs : EventArgs
{
	public bool Changed { get; set; }

	public int ShcType { get; set; }

	public string Hash { get; set; }
}
