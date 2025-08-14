using System.IO;
using System.Text;
using Ionic.Zlib;

namespace Ionic.Zip;

internal class ZipContainer
{
	private ZipFile _zf;

	private ZipOutputStream _zos;

	private ZipInputStream _zis;

	public ZipFile ZipFile => _zf;

	public ZipOutputStream ZipOutputStream => _zos;

	public string Name
	{
		get
		{
			if (_zf != null)
			{
				return _zf.Name;
			}
			return _zos.Name;
		}
	}

	public string Password
	{
		get
		{
			if (_zf != null)
			{
				return _zf._Password;
			}
			return _zos._password;
		}
	}

	public Zip64Option Zip64
	{
		get
		{
			if (_zf != null)
			{
				return _zf._zip64;
			}
			return _zos._zip64;
		}
	}

	public int BufferSize
	{
		get
		{
			if (_zf != null)
			{
				return _zf.BufferSize;
			}
			return 0;
		}
	}

	public int CodecBufferSize
	{
		get
		{
			if (_zf != null)
			{
				return _zf.CodecBufferSize;
			}
			return _zos.CodecBufferSize;
		}
	}

	public CompressionStrategy Strategy
	{
		get
		{
			if (_zf != null)
			{
				return _zf.Strategy;
			}
			return _zos.Strategy;
		}
	}

	public Zip64Option UseZip64WhenSaving
	{
		get
		{
			if (_zf != null)
			{
				return _zf.UseZip64WhenSaving;
			}
			return _zos.EnableZip64;
		}
	}

	public Encoding ProvisionalAlternateEncoding
	{
		get
		{
			if (_zf != null)
			{
				return _zf.ProvisionalAlternateEncoding;
			}
			return _zis.ProvisionalAlternateEncoding;
		}
	}

	public Stream ReadStream
	{
		get
		{
			if (_zf != null)
			{
				return _zf.ReadStream;
			}
			return _zis.ReadStream;
		}
	}

	public ZipContainer(object o)
	{
		_zf = o as ZipFile;
		_zos = o as ZipOutputStream;
		_zis = o as ZipInputStream;
	}
}
