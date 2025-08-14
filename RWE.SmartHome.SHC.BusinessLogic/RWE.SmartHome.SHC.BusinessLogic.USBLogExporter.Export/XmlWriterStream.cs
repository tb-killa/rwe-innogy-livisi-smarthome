using System;
using System.IO;
using System.Xml;

namespace RWE.SmartHome.SHC.BusinessLogic.USBLogExporter.Export;

public class XmlWriterStream : Stream
{
	private readonly XmlWriter xmlWriter;

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

	public XmlWriterStream(XmlWriter xmlWriter)
	{
		this.xmlWriter = xmlWriter;
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
		xmlWriter.WriteBinHex(buffer, offset, count);
	}

	public override void Flush()
	{
	}
}
