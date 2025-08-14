using System;
using System.Diagnostics;
using Rebex.Security.Cryptography;

namespace onrkn;

internal abstract class bvxhw : KeyedHashAlgorithm, IHashTransform, IDisposable
{
	public const int dbvbg = 32;

	public const int gquqr = 16;

	public const int kudcw = 16;

	public const int hdtci = 15;

	public const int cbxrn = 16;

	public const int crner = 2;

	public const ulong jrsax = 18446744073709551611uL;

	public const ulong jacst = ulong.MaxValue;

	public const ulong naegi = 3uL;

	public const ulong zzawm = 1152921487695413247uL;

	public const ulong elmoq = 1152921487695413244uL;

	public const string iktzm = "Key must have 32 bytes";

	private bool avjqo;

	public abstract bool txhsi { get; }

	public override int InputBlockSize => 16;

	protected bvxhw()
	{
		avjqo = false;
	}

	public void Process(byte[] buffer, int offset, int count)
	{
		fimcw(buffer, offset, count, p3: false);
	}

	public void saubf(ulong p0, ulong p1)
	{
		olrhm(p0, p1);
	}

	public abstract byte[] GetHash();

	public void fimcw(byte[] p0, int p1, int p2, bool p3)
	{
		dahxy.dionp(p0, p1, p2);
		qjveg(p0, p1, p2, p3);
	}

	public virtual void zzsom(nxtme<byte> p0)
	{
		emwui(p0);
	}

	public abstract void Reset();

	public void gaxag(nxtme<byte> p0)
	{
		vkjuk(p0);
	}

	public static bvxhw aztdt(nxtme<byte> p0)
	{
		return new oezhp(p0);
	}

	protected abstract void vkjuk(nxtme<byte> p0);

	protected abstract void olrhm(ulong p0, ulong p1);

	protected abstract void qjveg(byte[] p0, int p1, int p2, bool p3);

	protected abstract void emwui(nxtme<byte> p0);

	protected override void Dispose(bool disposing)
	{
		avjqo = true;
		base.Dispose(disposing);
	}

	[Conditional("DEBUG")]
	protected void zalpv()
	{
		if (avjqo && 0 == 0)
		{
			throw new ObjectDisposedException("Poly1305Base");
		}
	}
}
