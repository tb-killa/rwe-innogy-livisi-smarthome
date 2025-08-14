using System;
using System.IO;

namespace onrkn;

internal abstract class xlbfv : xaxit
{
	private readonly int vnnfp;

	private readonly MemoryStream itadp = new MemoryStream();

	private byte qcemw;

	private bool gcply;

	public bool ydwoh
	{
		get
		{
			return gcply;
		}
		set
		{
			gcply = value;
		}
	}

	public int hmokd => vnnfp;

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

	private void wjiur(byte[] p0, int p1, int p2)
	{
		while (p2 > 0)
		{
			int num = vnnfp - (int)itadp.Length;
			if (p2 < num)
			{
				itadp.Write(p0, p1, p2);
				p1 += p2;
				qcemw = p0[p1 - 1];
				break;
			}
			itadp.Write(p0, p1, num);
			tfllm(p0: false);
			p1 += num;
			p2 -= num;
		}
	}

	private void tfllm(bool p0)
	{
		iabst(itadp.GetBuffer(), 0, (int)itadp.Length, p0);
		itadp.SetLength(0L);
		qcemw = 0;
	}

	public override void Write(byte[] buffer, int index, int count)
	{
		if (count <= 0)
		{
			return;
		}
		if (qcemw == 13 && buffer[0] == 10)
		{
			itadp.SetLength(itadp.Length - 1);
			tfllm(p0: true);
			index++;
			count--;
		}
		int num = index + count;
		int num2 = index;
		while (num2 < num)
		{
			int num3 = num2;
			while (buffer[num3] != 13)
			{
				if (buffer[num3] == 10)
				{
					if (!gcply || 1 == 0)
					{
						wjiur(buffer, num2, num3 - num2);
						tfllm(p0: true);
					}
					else
					{
						wjiur(buffer, num2, num3 - num2 + 1);
						tfllm(p0: false);
					}
					num2 = num3 + 1;
				}
				num3++;
				if (num3 >= num)
				{
					wjiur(buffer, num2, num - num2);
					return;
				}
			}
			num3++;
			if (num3 == num)
			{
				wjiur(buffer, num2, num - num2);
				break;
			}
			if (buffer[num3] == 10)
			{
				wjiur(buffer, num2, num3 - num2 - 1);
				tfllm(p0: true);
			}
			else
			{
				if (buffer[num3] == 13 && (!gcply || 1 == 0))
				{
					wjiur(buffer, num2, num3 - num2 - 1);
					num2 = num3;
					continue;
				}
				wjiur(buffer, num2, num3 - num2 + 1);
			}
			num2 = num3 + 1;
		}
	}

	protected abstract void iabst(byte[] p0, int p1, int p2, bool p3);

	public override void Flush()
	{
	}

	protected override void julnt()
	{
		if (itadp.Length != 1 || itadp.GetBuffer()[0] != 13)
		{
			if (itadp.Length > 0)
			{
				tfllm(p0: false);
			}
			itadp.Close();
		}
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long v)
	{
		throw new NotSupportedException();
	}

	public xlbfv(int maxLineLength)
	{
		if (maxLineLength < 74 || maxLineLength > 16777216)
		{
			throw new ArgumentOutOfRangeException("maxLineLength");
		}
		vnnfp = maxLineLength;
	}

	public xlbfv()
		: this(65536)
	{
	}
}
