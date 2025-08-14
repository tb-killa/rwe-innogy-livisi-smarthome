using System;
using System.IO;

namespace onrkn;

internal class nzqad : xaxit
{
	private class wospf : npohs
	{
		private Action oyuts;

		private Exception ebxdt;

		public wospf(Stream inner, Action firstReadAction)
			: base(inner)
		{
			oyuts = firstReadAction;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			Action action = oyuts;
			if (action != null && 0 == 0)
			{
				oyuts = null;
				action();
			}
			int result = base.Read(buffer, offset, count);
			if (ebxdt != null && 0 == 0)
			{
				throw ebxdt;
			}
			return result;
		}

		internal void ppqfg(Exception p0)
		{
			Exception ex = ebxdt;
			if (ex == null || 1 == 0)
			{
				ex = p0;
			}
			ebxdt = ex;
		}
	}

	private readonly rowkj gsujd;

	private readonly Stream hktgm;

	private readonly long yitqr;

	private readonly wospf hfyax;

	private long biohd;

	private volatile Exception vvpdq;

	public Stream msodx => hfyax;

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override long Position
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public nzqad(int capacity, long expectedLength, Action firstConsumeAction)
	{
		gsujd = new rowkj(capacity);
		hktgm = gsujd.czdzb;
		yitqr = expectedLength;
		hfyax = new wospf(gsujd.jhfaw, firstConsumeAction);
	}

	protected override void julnt()
	{
		if (biohd < yitqr)
		{
			hfyax.ppqfg(new ujepc("More data expected according to the specified ContentLength.", ezmya.ydksh));
		}
		hktgm.Close();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (vvpdq != null && 0 == 0)
		{
			throw new IOException("HTTP request failed.", vvpdq);
		}
		try
		{
			if (yitqr >= 0 && biohd + count > yitqr)
			{
				string message = "Unable to upload more data than the specified ContentLength.";
				hfyax.ppqfg(new ujepc(message, ezmya.ydksh));
				throw new IOException(message);
			}
			hktgm.Write(buffer, offset, count);
			biohd += count;
		}
		catch (ObjectDisposedException)
		{
			if (vvpdq == null || 1 == 0)
			{
				throw;
			}
		}
		if (vvpdq != null && 0 == 0)
		{
			throw new IOException("HTTP request failed.", vvpdq);
		}
	}

	public override void Flush()
	{
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		throw new NotSupportedException();
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		throw new NotSupportedException();
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		if (vvpdq != null && 0 == 0)
		{
			throw new IOException("HTTP request failed.", vvpdq);
		}
		return base.BeginWrite(buffer, offset, count, callback, state);
	}

	internal void geugb(Exception p0)
	{
		Exception ex = vvpdq;
		if (ex == null || 1 == 0)
		{
			ex = p0;
		}
		vvpdq = ex;
		gsujd.mlowf();
	}
}
