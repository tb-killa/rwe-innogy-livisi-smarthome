using System;
using System.IO;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal abstract class kumym
{
	private readonly string oiljl;

	public kumym(string hostKeyAlgorithm)
	{
		oiljl = hostKeyAlgorithm;
	}

	protected static void pqmau(Stream p0, byte[] p1)
	{
		byte[] bytes = BitConverter.GetBytes(p1.Length);
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			Array.Reverse(bytes, 0, bytes.Length);
		}
		p0.Write(bytes, 0, bytes.Length);
		p0.Write(p1, 0, p1.Length);
	}

	protected static void ozpwr(Stream p0, int p1)
	{
		byte[] bytes = BitConverter.GetBytes(p1);
		if (BitConverter.IsLittleEndian && 0 == 0)
		{
			Array.Reverse(bytes, 0, bytes.Length);
		}
		p0.Write(bytes, 0, bytes.Length);
	}

	protected SshException xdrcl(byte[] p0, byte[] p1, byte[] p2, out SshPublicKey p3)
	{
		SshHostKeyAlgorithm sshHostKeyAlgorithm = SshPublicKey.snagl(oiljl);
		SshHostKeyAlgorithm sshHostKeyAlgorithm2 = sshHostKeyAlgorithm;
		if (sshHostKeyAlgorithm2 == SshHostKeyAlgorithm.Certificate)
		{
			p3 = SshPublicKey.gnwxo(p0, oiljl);
			if (p3 == null || 1 == 0)
			{
				throw new SshException(tcpjq.ziezw, "Host key algorithm mismatch.");
			}
		}
		else
		{
			p3 = new SshPublicKey(p0);
			if (p3.KeyAlgorithm != sshHostKeyAlgorithm)
			{
				throw new SshException(tcpjq.ziezw, "Host key algorithm mismatch.");
			}
		}
		if (!p3.isjpb(p1, p2, oiljl) || 1 == 0)
		{
			return new SshException(tcpjq.ziezw, "Server signature is not valid.");
		}
		return null;
	}

	internal static byte[] yejxo(byte[] p0)
	{
		if (p0[0] < 128)
		{
			return p0;
		}
		byte[] array = new byte[p0.Length + 1];
		p0.CopyTo(array, 1);
		return array;
	}

	public abstract void kuyvo(SshSession p0, byte[] p1, byte[] p2, byte[] p3, byte[] p4, out qwrgb p5, out byte[] p6, out SshPublicKey p7);

	protected void ovjbp(SshSession p0)
	{
		p0.cnfnb(LogLevel.Debug, "Validating '{0}' signature.", oiljl);
	}
}
