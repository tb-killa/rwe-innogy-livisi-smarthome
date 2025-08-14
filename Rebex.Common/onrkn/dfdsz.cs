using Rebex.Security.Cryptography;

namespace onrkn;

internal class dfdsz : KeyedHashAlgorithm
{
	private const int gkdwm = 384;

	private const int ntldc = 48;

	private readonly byte[] xmasq;

	private luida.vczsd jlnjh;

	public override int HashSize => 384;

	public dfdsz(byte[] key)
	{
		byte[] array = key;
		if (array == null || 1 == 0)
		{
			array = luida.nysqo;
		}
		xmasq = array;
		lzuhm();
	}

	public dfdsz()
		: this(luida.nysqo)
	{
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		luida.toshr(ref jlnjh, array.myshu(ibStart, cbSize));
	}

	protected override byte[] HashFinal()
	{
		byte[] array = new byte[48];
		luida.mdjru(ref jlnjh, array);
		return array;
	}

	public override void Initialize()
	{
		lzuhm();
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		jlnjh.ksapj();
	}

	private void lzuhm()
	{
		jlnjh.ksapj();
		jlnjh = luida.gylwc(xmasq, 48);
	}
}
