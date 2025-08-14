using System;

namespace Rebex.Net;

public class SshFingerprintEventArgs : EventArgs
{
	private readonly SshFingerprint kltco;

	private readonly SshPublicKey ugbvk;

	private readonly string mkgls;

	private bool jwygy;

	public SshFingerprint Fingerprint => kltco;

	public SshPublicKey ServerKey => ugbvk;

	internal string kmeov => mkgls;

	public bool Accept
	{
		get
		{
			return jwygy;
		}
		set
		{
			jwygy = value;
		}
	}

	public SshFingerprintEventArgs(SshFingerprint fingerprint)
	{
		kltco = fingerprint;
	}

	public SshFingerprintEventArgs(SshPublicKey serverKey)
		: this(serverKey, null)
	{
	}

	internal SshFingerprintEventArgs(SshPublicKey serverKey, string serverName)
	{
		ugbvk = serverKey;
		kltco = serverKey.Fingerprint;
		mkgls = serverName;
	}
}
