#define NETCF
using System;
using System.IO;

namespace Ionic.Zip;

internal class ZipSegmentedStream : Stream, IDisposable
{
	private int rw;

	private string _baseName;

	private string _baseDir;

	private string _currentName;

	private string _currentTempName;

	private uint _currentDiskNumber;

	private uint _maxDiskNumber;

	private int _maxSegmentSize;

	private Stream _innerStream;

	private bool _003CContiguousWrite_003Ek__BackingField;

	public bool ContiguousWrite
	{
		get
		{
			return _003CContiguousWrite_003Ek__BackingField;
		}
		set
		{
			_003CContiguousWrite_003Ek__BackingField = value;
		}
	}

	public uint CurrentSegment
	{
		get
		{
			return _currentDiskNumber;
		}
		private set
		{
			_currentDiskNumber = value;
			_currentName = null;
		}
	}

	public string CurrentName
	{
		get
		{
			if (_currentName == null)
			{
				_currentName = _NameForSegment(CurrentSegment);
			}
			return _currentName;
		}
	}

	public override bool CanRead
	{
		get
		{
			if (rw == 1)
			{
				return _innerStream.CanRead;
			}
			return false;
		}
	}

	public override bool CanSeek => _innerStream.CanSeek;

	public override bool CanWrite
	{
		get
		{
			if (rw == 2)
			{
				return _innerStream.CanWrite;
			}
			return false;
		}
	}

	public override long Length => _innerStream.Length;

	public override long Position
	{
		get
		{
			return _innerStream.Position;
		}
		set
		{
			_innerStream.Position = value;
		}
	}

	private ZipSegmentedStream()
	{
	}

	public static ZipSegmentedStream ForReading(string name, uint initialDiskNumber, uint maxDiskNumber)
	{
		ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream();
		zipSegmentedStream.rw = 1;
		zipSegmentedStream.CurrentSegment = initialDiskNumber;
		zipSegmentedStream._maxDiskNumber = maxDiskNumber;
		zipSegmentedStream._baseName = name;
		ZipSegmentedStream zipSegmentedStream2 = zipSegmentedStream;
		zipSegmentedStream2._SetReadStream();
		return zipSegmentedStream2;
	}

	public static ZipSegmentedStream ForWriting(string name, int maxSegmentSize)
	{
		ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream();
		zipSegmentedStream.rw = 2;
		zipSegmentedStream.CurrentSegment = 0u;
		zipSegmentedStream._baseName = name;
		zipSegmentedStream._maxSegmentSize = maxSegmentSize;
		zipSegmentedStream._baseDir = Path.GetDirectoryName(name);
		ZipSegmentedStream zipSegmentedStream2 = zipSegmentedStream;
		if (zipSegmentedStream2._baseDir == "")
		{
			zipSegmentedStream2._baseDir = ".";
		}
		zipSegmentedStream2._SetWriteStream(0u);
		return zipSegmentedStream2;
	}

	public static ZipSegmentedStream ForUpdate(string name, uint diskNumber)
	{
		ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream();
		zipSegmentedStream.rw = 3;
		zipSegmentedStream.CurrentSegment = diskNumber;
		zipSegmentedStream._baseName = name;
		zipSegmentedStream._maxSegmentSize = int.MaxValue;
		ZipSegmentedStream zipSegmentedStream2 = zipSegmentedStream;
		zipSegmentedStream2._SetUpdateStream();
		return zipSegmentedStream2;
	}

	private void _SetUpdateStream()
	{
		_innerStream = new FileStream(CurrentName, FileMode.Open);
	}

	private string _NameForSegment(uint diskNumber)
	{
		return $"{Path.Combine(Path.GetDirectoryName(_baseName), Path.GetFileNameWithoutExtension(_baseName))}.z{diskNumber + 1:D2}";
	}

	public uint ComputeSegment(int length)
	{
		if (_innerStream.Position + length > _maxSegmentSize)
		{
			return CurrentSegment + 1;
		}
		return CurrentSegment;
	}

	public override string ToString()
	{
		return string.Format("{0}[{1}][{2}], pos=0x{3:X})", "ZipSegmentedStream", CurrentName, (rw == 1) ? "Read" : ((rw == 2) ? "Write" : ((rw == 3) ? "Update" : "???")), Position);
	}

	public void ResetWriter()
	{
		CurrentSegment = 0u;
		_SetWriteStream(0u);
	}

	private void _SetReadStream()
	{
		if (_innerStream != null)
		{
			_innerStream.Close();
		}
		if (CurrentSegment + 1 == _maxDiskNumber)
		{
			_currentName = _baseName;
		}
		_innerStream = File.OpenRead(CurrentName);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (rw != 1)
		{
			throw new ZipException("Stream Error: Cannot Read.");
		}
		int num = _innerStream.Read(buffer, offset, count);
		int num2 = num;
		while (num2 != count)
		{
			if (_innerStream.Position != _innerStream.Length)
			{
				throw new ZipException($"Read error in file {CurrentName}");
			}
			if (CurrentSegment + 1 == _maxDiskNumber)
			{
				return num;
			}
			CurrentSegment++;
			_SetReadStream();
			offset += num2;
			count -= num2;
			num2 = _innerStream.Read(buffer, offset, count);
			num += num2;
		}
		return num;
	}

	private void _SetWriteStream(uint increment)
	{
		if (_innerStream != null)
		{
			_innerStream.Close();
			if (File.Exists(CurrentName))
			{
				File.Delete(CurrentName);
			}
			File.Move(_currentTempName, CurrentName);
		}
		if (increment != 0)
		{
			CurrentSegment += increment;
		}
		SharedUtilities.CreateAndOpenUniqueTempFile(_baseDir, out _innerStream, out _currentTempName);
		if (CurrentSegment == 0)
		{
			_innerStream.Write(BitConverter.GetBytes(134695760), 0, 4);
		}
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (rw == 2)
		{
			if (ContiguousWrite)
			{
				if (_innerStream.Position + count > _maxSegmentSize)
				{
					_SetWriteStream(1u);
				}
			}
			else
			{
				while (_innerStream.Position + count > _maxSegmentSize)
				{
					int num = _maxSegmentSize - (int)_innerStream.Position;
					_innerStream.Write(buffer, offset, num);
					_SetWriteStream(1u);
					count -= num;
					offset += num;
				}
			}
			_innerStream.Write(buffer, offset, count);
		}
		else
		{
			if (rw != 3)
			{
				throw new ZipException("Stream Error: Cannot Write.");
			}
			_innerStream.Write(buffer, offset, count);
		}
	}

	public long TruncateBackward(uint diskNumber, long offset)
	{
		if (rw != 2)
		{
			throw new ZipException("bad state.");
		}
		if (diskNumber == CurrentSegment)
		{
			long result = _innerStream.Seek(offset, SeekOrigin.Begin);
			SharedUtilities.Workaround_Ladybug318918(_innerStream);
			return result;
		}
		if (_innerStream != null)
		{
			_innerStream.Close();
			if (File.Exists(_currentTempName))
			{
				File.Delete(_currentTempName);
			}
		}
		for (uint num = CurrentSegment - 1; num > diskNumber; num--)
		{
			string path = _NameForSegment(num);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
		CurrentSegment = diskNumber;
		for (int i = 0; i < 3; i++)
		{
			try
			{
				_currentTempName = SharedUtilities.InternalGetTempFileName();
				File.Move(CurrentName, _currentTempName);
			}
			catch (IOException)
			{
				if (i == 2)
				{
					throw;
				}
			}
		}
		_innerStream = new FileStream(_currentTempName, FileMode.Open);
		long result2 = _innerStream.Seek(offset, SeekOrigin.Begin);
		SharedUtilities.Workaround_Ladybug318918(_innerStream);
		return result2;
	}

	public override void Flush()
	{
		_innerStream.Flush();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		long result = _innerStream.Seek(offset, origin);
		SharedUtilities.Workaround_Ladybug318918(_innerStream);
		return result;
	}

	public override void SetLength(long value)
	{
		if (rw != 2)
		{
			throw new NotImplementedException();
		}
		_innerStream.SetLength(value);
	}

	void IDisposable.Dispose()
	{
		Close();
	}

	public override void Close()
	{
		if (_innerStream != null)
		{
			_innerStream.Close();
			_innerStream = null;
			if (rw == 2)
			{
				if (File.Exists(CurrentName))
				{
					File.Delete(CurrentName);
				}
				if (File.Exists(_currentTempName))
				{
					File.Move(_currentTempName, CurrentName);
				}
			}
		}
		base.Close();
	}
}
