namespace Org.Mentalis.Security.Ssl.Shared;

internal class TransferItem
{
	public AsyncResult AsyncResult;

	public byte[] Buffer;

	public int Offset;

	public int OriginalSize;

	public int Size;

	public int Transferred;

	public DataType Type;

	public TransferItem(byte[] buffer, int offset, int size, AsyncResult asyncResult, DataType type)
	{
		Buffer = buffer;
		Offset = offset;
		Size = size;
		AsyncResult = asyncResult;
		Transferred = 0;
		Type = type;
		OriginalSize = size;
	}
}
