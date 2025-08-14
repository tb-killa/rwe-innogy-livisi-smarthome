using Rebex.Security.Cryptography;

namespace onrkn;

internal class pyule : KeyedHashAlgorithm
{
	private const int fpntb = 256;

	private const int mxlxz = 32;

	private readonly byte[] vndjs;

	private luida.vczsd gieav;

	public override int HashSize => 256;

	public pyule(byte[] key)
	{
		byte[] array = key;
		if (array == null || 1 == 0)
		{
			array = luida.nysqo;
		}
		vndjs = array;
		tcemu();
	}

	public pyule()
		: this(luida.nysqo)
	{
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		luida.toshr(ref gieav, array.myshu(ibStart, cbSize));
	}

	protected override byte[] HashFinal()
	{
		byte[] array = new byte[32];
		luida.mdjru(ref gieav, array);
		return array;
	}

	public override void Initialize()
	{
		tcemu();
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		gieav.ksapj();
	}

	private void tcemu()
	{
		gieav.ksapj();
		gieav = luida.gylwc(vndjs, 32);
	}
}
