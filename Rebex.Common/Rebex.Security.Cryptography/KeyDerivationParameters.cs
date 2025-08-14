namespace Rebex.Security.Cryptography;

public class KeyDerivationParameters
{
	internal byte[] nscqa;

	internal byte[] ijxbw;

	internal int zdnet;

	private string yddya;

	private HashingAlgorithmId yggvd;

	private byte[] kfxlu;

	private byte[] wmxvy;

	private byte[] dgcnk;

	public string KeyDerivationFunction
	{
		get
		{
			return yddya;
		}
		set
		{
			yddya = value;
		}
	}

	public HashingAlgorithmId HashAlgorithm
	{
		get
		{
			return yggvd;
		}
		set
		{
			yggvd = value;
		}
	}

	public byte[] HmacKey
	{
		get
		{
			return kfxlu;
		}
		set
		{
			kfxlu = value;
		}
	}

	public byte[] SecretAppend
	{
		get
		{
			return wmxvy;
		}
		set
		{
			wmxvy = value;
		}
	}

	public byte[] SecretPrepend
	{
		get
		{
			return dgcnk;
		}
		set
		{
			dgcnk = value;
		}
	}
}
