namespace Org.Mentalis.Security.Ssl.Shared;

internal abstract class CompressionAlgorithm
{
	public byte[] Compress(byte[] data)
	{
		return data;
	}

	public byte[] Decompress(byte[] data)
	{
		return data;
	}

	public static SslAlgorithms GetCompressionAlgorithm(byte[] algos, SslAlgorithms allowed)
	{
		for (int i = 0; i < algos.Length; i++)
		{
			if (algos[i] == 0)
			{
				return SslAlgorithms.NULL_COMPRESSION;
			}
		}
		throw new SslException(AlertDescription.HandshakeFailure, "No compression method matches the available compression methods.");
	}

	public static byte GetAlgorithmByte(SslAlgorithms algorithm)
	{
		if (algorithm == SslAlgorithms.NULL_COMPRESSION)
		{
			return 0;
		}
		return 0;
	}

	public static SslAlgorithms GetCompressionAlgorithmType(byte[] buffer, int offset)
	{
		byte b = buffer[offset];
		_ = 0;
		return SslAlgorithms.NULL_COMPRESSION;
	}

	public static byte[] GetCompressionAlgorithmBytes(SslAlgorithms algorithm)
	{
		return new byte[1];
	}
}
