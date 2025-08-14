using System;

namespace Rebex.Net;

public class SmtpTransferProgressEventArgs : EventArgs
{
	private SmtpTransferState fssgc;

	private long pphpa;

	private int jxcip;

	private long rczqy;

	private byte[] tqbku;

	private int ojqyw;

	public SmtpTransferState State => fssgc;

	public long BytesTransferred => pphpa;

	public long BytesSinceLastEvent => jxcip;

	public long TotalBytes => rczqy;

	public byte[] GetData()
	{
		if (tqbku == null || 1 == 0)
		{
			return null;
		}
		byte[] array = new byte[jxcip];
		Array.Copy(tqbku, ojqyw, array, 0, array.Length);
		return array;
	}

	public SmtpTransferProgressEventArgs(SmtpTransferState state, long bytesTransferred, int bytesSinceLastEvent)
		: this(state, bytesTransferred, bytesSinceLastEvent, 0L, null, 0)
	{
	}

	public SmtpTransferProgressEventArgs(SmtpTransferState state, long bytesTransferred, int bytesSinceLastEvent, long totalBytes)
		: this(state, bytesTransferred, bytesSinceLastEvent, totalBytes, null, 0)
	{
	}

	internal SmtpTransferProgressEventArgs(SmtpTransferState state, long bytesTransferred, int bytesSinceLastEvent, long totalBytes, byte[] buffer, int offset)
	{
		tqbku = buffer;
		ojqyw = offset;
		fssgc = state;
		if (state != SmtpTransferState.None && 0 == 0)
		{
			pphpa = bytesTransferred;
			jxcip = bytesSinceLastEvent;
			rczqy = totalBytes;
		}
	}
}
