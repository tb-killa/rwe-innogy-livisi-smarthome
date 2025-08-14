using System;
using System.IO;

namespace onrkn;

internal class ctpqa : npohs, IDisposable
{
	public const string objjw = "The stream is read-only.";

	public override bool CanWrite
	{
		get
		{
			tibxc();
			return false;
		}
	}

	public ctpqa(Stream inner, bool ownsStream)
		: base(inner, !ownsStream)
	{
	}

	public override void Flush()
	{
		tibxc();
	}

	public override void SetLength(long value)
	{
		tibxc();
		dxrxk();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		tibxc();
		dxrxk();
	}

	public override void WriteByte(byte value)
	{
		tibxc();
		dxrxk();
	}

	private static void dxrxk()
	{
		throw new IOException("The stream is read-only.");
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		dxrxk();
		return null;
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		dxrxk();
	}
}
