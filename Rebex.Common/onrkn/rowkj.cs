using System;
using System.IO;
using System.Threading;

namespace onrkn;

internal class rowkj
{
	private class ivkyc : xaxit
	{
		private readonly rowkj ahdzj;

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

		public ivkyc(rowkj pipe)
		{
			ahdzj = pipe;
		}

		public override void Flush()
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			ahdzj.igvjy(buffer, offset, count);
		}

		protected override void julnt()
		{
			ahdzj.okuow();
		}
	}

	private class yidon : xaxit
	{
		private readonly rowkj fsrrd;

		public override bool CanRead => true;

		public override bool CanSeek => false;

		public override bool CanWrite => false;

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

		public yidon(rowkj pipe)
		{
			fsrrd = pipe;
		}

		public override void Flush()
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return fsrrd.sljpe(buffer, offset, count);
		}

		public override int ReadByte()
		{
			return fsrrd.qzpox();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		protected override void julnt()
		{
			fsrrd.wjdae();
		}
	}

	private readonly object fuyaz;

	private readonly object siqdk;

	private readonly object uobuj;

	private readonly ManualResetEvent gwpor;

	private readonly ManualResetEvent mbylh;

	private readonly vqbky mlftn;

	private readonly Stream mpozp;

	private readonly Stream wqrew;

	private volatile bool dsukv;

	private volatile bool jgzon;

	private EventHandler nqvfe;

	private EventHandler banqk;

	public bool hfovs
	{
		get
		{
			lock (fuyaz)
			{
				return (dsukv ? true : false) || mlftn.kagot > 0;
			}
		}
	}

	public Stream czdzb => mpozp;

	public Stream jhfaw => wqrew;

	public int ubyfe
	{
		get
		{
			lock (fuyaz)
			{
				return mlftn.hybhd;
			}
		}
	}

	public event EventHandler fcups
	{
		add
		{
			EventHandler eventHandler = nqvfe;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref nqvfe, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = nqvfe;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref nqvfe, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler wevlx
	{
		add
		{
			EventHandler eventHandler = banqk;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref banqk, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = banqk;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref banqk, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public rowkj(int capacity)
	{
		fuyaz = new object();
		siqdk = new object();
		uobuj = new object();
		gwpor = new ManualResetEvent(initialState: false);
		mbylh = new ManualResetEvent(initialState: false);
		mlftn = new vqbky(capacity);
		mpozp = new ivkyc(this);
		wqrew = new yidon(this);
	}

	private void zokvm(byte p0)
	{
		igvjy(new byte[1] { p0 }, 0, 1);
	}

	private void igvjy(byte[] p0, int p1, int p2)
	{
		dahxy.dionp(p0, p1, p2);
		bool flag = false;
		lock (siqdk)
		{
			while (p2 > 0)
			{
				lock (fuyaz)
				{
					if (dsukv && 0 == 0)
					{
						throw new ObjectDisposedException("Stream");
					}
					int num = Math.Min(p2, mlftn.hybhd);
					if (num > 0)
					{
						if (mlftn.kagot == 0 || 1 == 0)
						{
							gwpor.Set();
						}
						mlftn.ogzaw(p0, p1, num);
						p1 += num;
						p2 -= num;
						flag = true;
						continue;
					}
					mbylh.Reset();
				}
				mbylh.WaitOne(-1, exitContext: false);
			}
		}
		if (flag && 0 == 0)
		{
			EventHandler eventHandler = banqk;
			if (eventHandler != null && 0 == 0)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}
	}

	private int sljpe(byte[] p0, int p1, int p2)
	{
		dahxy.dionp(p0, p1, p2);
		if (p2 == 0 || 1 == 0)
		{
			return 0;
		}
		bool flag = false;
		lock (uobuj)
		{
			while (true)
			{
				lock (fuyaz)
				{
					if (jgzon && 0 == 0)
					{
						return 0;
					}
					if (mlftn.kagot > 0)
					{
						if (mlftn.hybhd == 0 || 1 == 0)
						{
							mbylh.Set();
							flag = true;
						}
						p2 = mlftn.pazpl(p0, p1, p2);
						break;
					}
					if (dsukv && 0 == 0)
					{
						return 0;
					}
					gwpor.Reset();
				}
				gwpor.WaitOne(-1, exitContext: false);
			}
		}
		if (flag && 0 == 0)
		{
			EventHandler eventHandler = nqvfe;
			if (eventHandler != null && 0 == 0)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}
		return p2;
	}

	private int qzpox()
	{
		if (jgzon && 0 == 0)
		{
			throw new ObjectDisposedException("Stream");
		}
		bool flag = false;
		int result;
		lock (uobuj)
		{
			while (true)
			{
				lock (fuyaz)
				{
					if (mlftn.kagot > 0)
					{
						if (mlftn.hybhd == 0 || 1 == 0)
						{
							mbylh.Set();
							flag = true;
						}
						result = mlftn.jztfb();
						break;
					}
					if (dsukv && 0 == 0)
					{
						return -1;
					}
					gwpor.Reset();
				}
				gwpor.WaitOne(-1, exitContext: false);
			}
		}
		if (flag && 0 == 0)
		{
			EventHandler eventHandler = nqvfe;
			if (eventHandler != null && 0 == 0)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}
		return result;
	}

	private void okuow()
	{
		lock (fuyaz)
		{
			dsukv = true;
			gwpor.Set();
		}
		EventHandler eventHandler = banqk;
		if (eventHandler != null && 0 == 0)
		{
			eventHandler(this, EventArgs.Empty);
		}
	}

	private void wjdae()
	{
		lock (fuyaz)
		{
			jgzon = true;
		}
	}

	public void mlowf()
	{
		lock (fuyaz)
		{
			mlftn.jumkt();
			okuow();
			wjdae();
			mbylh.Set();
		}
	}
}
