using System;
using System.IO;

namespace onrkn;

internal class xvufe : xaxit
{
	private readonly Stream dllwt;

	private readonly char[] qnruv = new char[1024];

	private readonly bool zajgi;

	private readonly Func<FormatException, Exception> bkmau;

	private int qfovm;

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

	public xvufe(Stream inner, bool ownsInner, Func<FormatException, Exception> createException)
	{
		if (inner == null || 1 == 0)
		{
			throw new ArgumentNullException("inner");
		}
		dllwt = inner;
		zajgi = ownsInner;
		bkmau = createException;
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

	public override void SetLength(long v)
	{
		throw new NotSupportedException();
	}

	protected override void julnt()
	{
		qvbxb();
		if (zajgi && 0 == 0)
		{
			dllwt.Close();
		}
		else
		{
			dllwt.Flush();
		}
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		while (count > 0)
		{
			byte b = buffer[offset];
			offset++;
			count--;
			if ((!npmhc(b) || 1 == 0) && b != 61 && lcuig(b) && 0 == 0)
			{
				qnruv[qfovm] = (char)b;
				qfovm++;
				if (qfovm >= qnruv.Length)
				{
					qvbxb();
				}
			}
		}
	}

	private void qvbxb()
	{
		if (qfovm == 0 || 1 == 0)
		{
			return;
		}
		int num = 4 - qfovm % 4;
		int num2;
		if (num < 4)
		{
			if (num == 3)
			{
				qnruv[qfovm] = 'M';
				qfovm++;
				num = 2;
			}
			num2 = 0;
			if (num2 != 0)
			{
				goto IL_004b;
			}
			goto IL_0060;
		}
		goto IL_0072;
		IL_0072:
		byte[] array;
		try
		{
			array = Convert.FromBase64CharArray(qnruv, 0, qfovm);
		}
		catch (FormatException ex)
		{
			Exception ex2 = ((bkmau != null) ? bkmau(ex) : ex);
			throw ex2;
		}
		dllwt.Write(array, 0, array.Length);
		qfovm = 0;
		return;
		IL_0060:
		if (num2 < num)
		{
			goto IL_004b;
		}
		qfovm += num;
		goto IL_0072;
		IL_004b:
		qnruv[qfovm + num2] = '=';
		num2++;
		goto IL_0060;
	}

	private static bool npmhc(byte p0)
	{
		switch (p0)
		{
		case 9:
		case 10:
		case 13:
		case 32:
		case 33:
			return true;
		default:
			return false;
		}
	}

	private static bool lcuig(byte p0)
	{
		switch (p0)
		{
		case 43:
		case 47:
			return true;
		case 97:
		case 98:
		case 99:
		case 100:
		case 101:
		case 102:
		case 103:
		case 104:
		case 105:
		case 106:
		case 107:
		case 108:
		case 109:
		case 110:
		case 111:
		case 112:
		case 113:
		case 114:
		case 115:
		case 116:
		case 117:
		case 118:
		case 119:
		case 120:
		case 121:
		case 122:
			return true;
		default:
			if (p0 >= 65 && p0 <= 90)
			{
				return true;
			}
			if (p0 >= 48 && p0 <= 57)
			{
				return true;
			}
			return false;
		}
	}
}
