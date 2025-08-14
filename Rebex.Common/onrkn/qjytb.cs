using Rebex.Security.Cryptography;

namespace onrkn;

internal class qjytb : KeyedHashAlgorithm
{
	private const int rtirw = 512;

	private const int dtdjw = 64;

	private readonly byte[] znkqg;

	private luida.vczsd vzcre;

	public override int HashSize => 512;

	public qjytb(byte[] key)
	{
		byte[] array = key;
		if (array == null || 1 == 0)
		{
			array = luida.nysqo;
		}
		znkqg = array;
		ntzdk();
	}

	public qjytb()
		: this(luida.nysqo)
	{
	}

	protected override void HashCore(byte[] array, int ibStart, int cbSize)
	{
		luida.toshr(ref vzcre, array.myshu(ibStart, cbSize));
	}

	protected override byte[] HashFinal()
	{
		byte[] array = new byte[64];
		luida.mdjru(ref vzcre, array);
		return array;
	}

	public override void Initialize()
	{
		ntzdk();
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		vzcre.ksapj();
	}

	private void ntzdk()
	{
		vzcre.ksapj();
		vzcre = luida.gylwc(znkqg, 64);
	}
}
