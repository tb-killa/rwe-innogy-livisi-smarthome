using System;
using System.IO;
using System.Security.Cryptography;
using RWE.SmartHome.SHC.DisplayManagerInterfaces;
using RWE.SmartHome.SHC.DisplayManagerInterfaces.Exceptions;

namespace RWE.SmartHome.SHC.BusinessLogic.USBLogExporter.Export;

public class HashingStream : Stream
{
	private readonly Stream wrappedStream;

	private SHA1Managed sha1;

	private bool hashMode;

	private bool skipPreviousBracket;

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

	public override long Length
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override long Position
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public byte[] Hash { get; private set; }

	public HashingStream(Stream wrappedStream)
	{
		this.wrappedStream = wrappedStream;
		hashMode = false;
		skipPreviousBracket = false;
	}

	public override void Flush()
	{
		ExceptionWithWorkflowError.WrapException(wrappedStream.Flush, WorkflowError.UsbStickLogExport_WriteFailed);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotImplementedException();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotImplementedException();
	}

	public override void SetLength(long value)
	{
		throw new NotImplementedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (hashMode)
		{
			bool flag = false;
			if (skipPreviousBracket && count > 0)
			{
				if (buffer[offset] == 62)
				{
					sha1.TransformBlock(buffer, offset + 1, count - 1, buffer, offset + 1);
					flag = true;
				}
				skipPreviousBracket = false;
			}
			if (!flag)
			{
				sha1.TransformBlock(buffer, offset, count, buffer, offset);
			}
		}
		ExceptionWithWorkflowError.WrapException(delegate
		{
			wrappedStream.Write(buffer, offset, count);
		}, WorkflowError.UsbStickLogExport_WriteFailed);
	}

	public override void Close()
	{
		ExceptionWithWorkflowError.WrapException(wrappedStream.Close, WorkflowError.UsbStickLogExport_WriteFailed);
	}

	public void BeginHashing(bool skipPreviousBracket)
	{
		hashMode = true;
		this.skipPreviousBracket = skipPreviousBracket;
		sha1 = new SHA1Managed();
	}

	public void FinishHashing()
	{
		hashMode = false;
		byte[] inputBuffer = new byte[0];
		sha1.TransformFinalBlock(inputBuffer, 0, 0);
		Hash = (byte[])sha1.Hash.Clone();
		sha1 = null;
	}
}
