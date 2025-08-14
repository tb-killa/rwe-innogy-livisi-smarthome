using System;
using System.IO;

namespace onrkn;

internal abstract class xaxit : Stream, IDisposable
{
	private sealed class tcsml
	{
		public xaxit ngazn;

		public byte[] ybfse;

		public int nwiif;

		public int dxcul;

		public int maoos()
		{
			return ngazn.Read(ybfse, nwiif, dxcul);
		}
	}

	private sealed class fiutf
	{
		public xaxit krzim;

		public byte[] iohjq;

		public int ynzpb;

		public int pnrra;

		public object zeulf()
		{
			krzim.Write(iohjq, ynzpb, pnrra);
			return null;
		}
	}

	protected virtual void julnt()
	{
	}

	public new void Close()
	{
		base.Close();
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		tcsml tcsml = new tcsml();
		tcsml.ybfse = buffer;
		tcsml.nwiif = offset;
		tcsml.dxcul = count;
		tcsml.ngazn = this;
		return rxpjc.tzeev(tcsml.maoos, callback, state);
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		fiutf fiutf = new fiutf();
		fiutf.iohjq = buffer;
		fiutf.ynzpb = offset;
		fiutf.pnrra = count;
		fiutf.krzim = this;
		return rxpjc.tzeev(fiutf.zeulf, callback, state);
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		return rxpjc.wzgzd<int>(asyncResult);
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		rxpjc.wzgzd<object>(asyncResult);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && 0 == 0)
		{
			julnt();
		}
		base.Dispose(disposing);
	}
}
