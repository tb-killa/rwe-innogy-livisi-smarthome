using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ionic.Zlib;

namespace Ionic.Zip;

public class ZipOutputStream : Stream
{
	private EncryptionAlgorithm _encryption;

	private ZipEntryTimestamp _timestamp;

	internal string _password;

	private string _comment;

	private Stream _outputStream;

	private ZipEntry _currentEntry;

	internal Zip64Option _zip64;

	private Dictionary<string, ZipEntry> _entriesWritten;

	private int _entryCount;

	private Encoding _provisionalAlternateEncoding;

	private bool _leaveUnderlyingStreamOpen;

	private bool _disposed;

	private bool _exceptionPending;

	private bool _anyEntriesUsedZip64;

	private bool _directoryNeededZip64;

	private CountingStream _outputCounter;

	private Stream _encryptor;

	private Stream _deflater;

	private CrcCalculatorStream _entryOutputStream;

	private bool _needToWriteEntryHeader;

	private string _name;

	private bool _DontIgnoreCase;

	private int _003CCodecBufferSize_003Ek__BackingField;

	private CompressionStrategy _003CStrategy_003Ek__BackingField;

	private CompressionLevel _003CCompressionLevel_003Ek__BackingField;

	public string Password
	{
		set
		{
			if (_disposed)
			{
				_exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			_password = value;
			if (_password == null)
			{
				_encryption = EncryptionAlgorithm.None;
			}
			else if (_encryption == EncryptionAlgorithm.None)
			{
				_encryption = EncryptionAlgorithm.PkzipWeak;
			}
		}
	}

	public EncryptionAlgorithm Encryption
	{
		get
		{
			return _encryption;
		}
		set
		{
			if (_disposed)
			{
				_exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			if (value == EncryptionAlgorithm.Unsupported)
			{
				_exceptionPending = true;
				throw new InvalidOperationException("You may not set Encryption to that value.");
			}
			_encryption = value;
		}
	}

	public int CodecBufferSize
	{
		get
		{
			return _003CCodecBufferSize_003Ek__BackingField;
		}
		set
		{
			_003CCodecBufferSize_003Ek__BackingField = value;
		}
	}

	public CompressionStrategy Strategy
	{
		get
		{
			return _003CStrategy_003Ek__BackingField;
		}
		set
		{
			_003CStrategy_003Ek__BackingField = value;
		}
	}

	public ZipEntryTimestamp Timestamp
	{
		get
		{
			return _timestamp;
		}
		set
		{
			if (_disposed)
			{
				_exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			_timestamp = value;
		}
	}

	public CompressionLevel CompressionLevel
	{
		get
		{
			return _003CCompressionLevel_003Ek__BackingField;
		}
		set
		{
			_003CCompressionLevel_003Ek__BackingField = value;
		}
	}

	public string Comment
	{
		get
		{
			return _comment;
		}
		set
		{
			if (_disposed)
			{
				_exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			_comment = value;
		}
	}

	public Zip64Option EnableZip64
	{
		get
		{
			return _zip64;
		}
		set
		{
			if (_disposed)
			{
				_exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			_zip64 = value;
		}
	}

	public bool OutputUsedZip64
	{
		get
		{
			if (!_anyEntriesUsedZip64)
			{
				return _directoryNeededZip64;
			}
			return true;
		}
	}

	public bool IgnoreCase
	{
		get
		{
			return !_DontIgnoreCase;
		}
		set
		{
			_DontIgnoreCase = !value;
		}
	}

	public bool UseUnicodeAsNecessary
	{
		get
		{
			return _provisionalAlternateEncoding == Encoding.GetEncoding("UTF-8");
		}
		set
		{
			_provisionalAlternateEncoding = (value ? Encoding.GetEncoding("UTF-8") : ZipFile.DefaultEncoding);
		}
	}

	public Encoding ProvisionalAlternateEncoding
	{
		get
		{
			return _provisionalAlternateEncoding;
		}
		set
		{
			_provisionalAlternateEncoding = value;
		}
	}

	internal Stream OutputStream => _outputStream;

	internal string Name => _name;

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
			return _outputStream.Position;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public ZipOutputStream(Stream stream)
		: this(stream, leaveOpen: false)
	{
	}

	public ZipOutputStream(string fileName)
	{
		Stream stream = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
		_Init(stream, leaveOpen: false);
		_name = fileName;
	}

	public ZipOutputStream(Stream stream, bool leaveOpen)
	{
		_Init(stream, leaveOpen);
	}

	private void _Init(Stream stream, bool leaveOpen)
	{
		_outputStream = (stream.CanRead ? stream : new CountingStream(stream));
		CompressionLevel = CompressionLevel.Default;
		_encryption = EncryptionAlgorithm.None;
		_entriesWritten = new Dictionary<string, ZipEntry>(StringComparer.Ordinal);
		_zip64 = Zip64Option.Default;
		_leaveUnderlyingStreamOpen = leaveOpen;
		Strategy = CompressionStrategy.Default;
		_name = "unknown";
	}

	public override string ToString()
	{
		return $"ZipOutputStream::{_name}(leaveOpen({_leaveUnderlyingStreamOpen})))";
	}

	private void InsureUniqueEntry(ZipEntry ze1)
	{
		if (_entriesWritten.ContainsKey(ze1.FileName))
		{
			_exceptionPending = true;
			throw new ArgumentException($"The entry '{ze1.FileName}' already exists in the zip archive.");
		}
	}

	public bool ContainsEntry(string name)
	{
		return _entriesWritten.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (_disposed)
		{
			_exceptionPending = true;
			throw new InvalidOperationException("The stream has been closed.");
		}
		if (_currentEntry == null)
		{
			_exceptionPending = true;
			throw new InvalidOperationException("You must call PutNextEntry() before calling Write().");
		}
		if (_currentEntry.IsDirectory)
		{
			_exceptionPending = true;
			throw new InvalidOperationException("You cannot Write() data for an entry that is a directory.");
		}
		if (_needToWriteEntryHeader)
		{
			_InitiateCurrentEntry(finishing: false);
		}
		_entryOutputStream.Write(buffer, offset, count);
	}

	public ZipEntry PutNextEntry(string entryName)
	{
		if (_disposed)
		{
			_exceptionPending = true;
			throw new InvalidOperationException("The stream has been closed.");
		}
		_FinishCurrentEntry();
		_currentEntry = ZipEntry.CreateForZipOutputStream(entryName);
		_currentEntry._container = new ZipContainer(this);
		_currentEntry._BitField |= 8;
		_currentEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
		_currentEntry.CompressionLevel = CompressionLevel;
		_currentEntry.Encryption = Encryption;
		_currentEntry.Password = _password;
		if (entryName.EndsWith("/"))
		{
			_currentEntry.MarkAsDirectory();
		}
		_currentEntry.EmitTimesInWindowsFormatWhenSaving = (_timestamp & ZipEntryTimestamp.Windows) != 0;
		_currentEntry.EmitTimesInUnixFormatWhenSaving = (_timestamp & ZipEntryTimestamp.Unix) != 0;
		InsureUniqueEntry(_currentEntry);
		_needToWriteEntryHeader = true;
		return _currentEntry;
	}

	private void _InitiateCurrentEntry(bool finishing)
	{
		_entriesWritten.Add(_currentEntry.FileName, _currentEntry);
		_entryCount++;
		if (_entryCount > 65534 && _zip64 == Zip64Option.Default)
		{
			_exceptionPending = true;
			throw new InvalidOperationException("Too many entries. Consider setting ZipOutputStream.EnableZip64.");
		}
		_currentEntry.WriteHeader(_outputStream, finishing ? 99 : 0);
		_currentEntry.StoreRelativeOffset();
		if (!_currentEntry.IsDirectory)
		{
			_currentEntry.WriteSecurityMetadata(_outputStream);
			_currentEntry.PrepOutputStream(_outputStream, (!finishing) ? (-1) : 0, out _outputCounter, out _encryptor, out _deflater, out _entryOutputStream);
		}
		_needToWriteEntryHeader = false;
	}

	private void _FinishCurrentEntry()
	{
		if (_currentEntry != null)
		{
			if (_needToWriteEntryHeader)
			{
				_InitiateCurrentEntry(finishing: true);
			}
			_currentEntry.FinishOutputStream(_outputStream, _outputCounter, _encryptor, _deflater, _entryOutputStream);
			_currentEntry.PostProcessOutput(_outputStream);
			_anyEntriesUsedZip64 |= _currentEntry.OutputUsedZip64.Value;
			_outputCounter = null;
			_encryptor = (_deflater = null);
			_entryOutputStream = null;
		}
	}

	protected override void Dispose(bool notCalledFromFinalizer)
	{
		if (_disposed)
		{
			return;
		}
		if (notCalledFromFinalizer && !_exceptionPending)
		{
			_FinishCurrentEntry();
			_directoryNeededZip64 = ZipOutput.WriteCentralDirectoryStructure(_outputStream, _entriesWritten.Values, 1u, _zip64, Comment, ProvisionalAlternateEncoding);
			Stream stream = null;
			if (_outputStream is CountingStream countingStream)
			{
				stream = countingStream.WrappedStream;
				countingStream.Close();
			}
			else
			{
				stream = _outputStream;
			}
			if (!_leaveUnderlyingStreamOpen)
			{
				stream.Close();
			}
			_outputStream = null;
		}
		_disposed = true;
	}

	public override void Flush()
	{
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException("Read");
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException("Seek");
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}
}
