using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Ionic.Zlib;

namespace Ionic.Zip;

[ComVisible(true)]
[Guid("ebc25cf6-9120-4283-b972-0e5520d00005")]
public class ZipFile : IEnumerable<ZipEntry>, IEnumerable, IDisposable
{
	private sealed class _003C_003Ec__DisplayClass1
	{
		public StringComparison sc;

		public int _003Cget_EntriesSorted_003Eb__0(ZipEntry x, ZipEntry y)
		{
			return string.Compare(x.FileName, y.FileName, sc);
		}
	}

	private sealed class _003CGetEnumerator_003Ed__3 : IEnumerator<ZipEntry>, IEnumerator, IDisposable
	{
		private ZipEntry _003C_003E2__current;

		private int _003C_003E1__state;

		public ZipFile _003C_003E4__this;

		public ZipEntry _003Ce_003E5__4;

		public Dictionary<string, ZipEntry>.ValueCollection.Enumerator _003C_003E7__wrap5;

		ZipEntry IEnumerator<ZipEntry>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap5 = _003C_003E4__this._entries.Values.GetEnumerator();
					_003C_003E1__state = 1;
					goto IL_007e;
				case 2:
					{
						_003C_003E1__state = 1;
						goto IL_007e;
					}
					IL_007e:
					if (_003C_003E7__wrap5.MoveNext())
					{
						_003Ce_003E5__4 = _003C_003E7__wrap5.Current;
						_003C_003E2__current = _003Ce_003E5__4;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally6();
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case 1:
			case 2:
				try
				{
					break;
				}
				finally
				{
					_003C_003Em__Finally6();
				}
			}
		}

		[DebuggerHidden]
		public _003CGetEnumerator_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		private void _003C_003Em__Finally6()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap5/*cast due to .constrained prefix*/).Dispose();
		}
	}

	public static readonly Encoding DefaultEncoding = Encoding.GetEncoding("IBM437");

	private TextWriter _StatusMessageTextWriter;

	private bool _CaseSensitiveRetrieval;

	private Stream _readstream;

	private Stream _writestream;

	private ushort _versionMadeBy;

	private ushort _versionNeededToExtract;

	private uint _diskNumberWithCd;

	private int _maxOutputSegmentSize;

	private uint _numberOfSegmentsForMostRecentSave;

	private ZipErrorAction _zipErrorAction;

	private bool _disposed;

	private Dictionary<string, ZipEntry> _entries;

	private List<ZipEntry> _zipEntriesAsList;

	private string _name;

	private string _Comment;

	internal string _Password;

	private bool _emitNtfsTimes = true;

	private bool _emitUnixTimes;

	private CompressionStrategy _Strategy;

	private bool _fileAlreadyExists;

	private string _temporaryFileName;

	private bool _contentsChanged;

	private bool _hasBeenSaved;

	private string _TempFileFolder;

	private bool _ReadStreamIsOurs = true;

	private object LOCK = new object();

	private bool _saveOperationCanceled;

	private bool _extractOperationCanceled;

	private EncryptionAlgorithm _Encryption;

	private bool _JustSaved;

	private long _locEndOfCDS = -1L;

	private bool? _OutputUsesZip64;

	internal bool _inExtractAll;

	private Encoding _provisionalAlternateEncoding = Encoding.GetEncoding("IBM437");

	private int _BufferSize = IoBufferSizeDefault;

	internal Zip64Option _zip64;

	private bool _SavingSfx;

	public static readonly int IoBufferSizeDefault = 32768;

	private long _lengthOfReadStream = -99L;

	private bool _003CFullScan_003Ek__BackingField;

	private bool _003CSortEntriesBeforeSaving_003Ek__BackingField;

	private bool _003CAddDirectoryWillTraverseReparsePoints_003Ek__BackingField;

	private int _003CCodecBufferSize_003Ek__BackingField;

	private bool _003CFlattenFoldersOnExtract_003Ek__BackingField;

	private CompressionLevel _003CCompressionLevel_003Ek__BackingField;

	private ExtractExistingFileAction _003CExtractExistingFile_003Ek__BackingField;

	private SetCompressionCallback _003CSetCompression_003Ek__BackingField;

	public bool FullScan
	{
		get
		{
			return _003CFullScan_003Ek__BackingField;
		}
		set
		{
			_003CFullScan_003Ek__BackingField = value;
		}
	}

	public bool SortEntriesBeforeSaving
	{
		get
		{
			return _003CSortEntriesBeforeSaving_003Ek__BackingField;
		}
		set
		{
			_003CSortEntriesBeforeSaving_003Ek__BackingField = value;
		}
	}

	public bool AddDirectoryWillTraverseReparsePoints
	{
		get
		{
			return _003CAddDirectoryWillTraverseReparsePoints_003Ek__BackingField;
		}
		set
		{
			_003CAddDirectoryWillTraverseReparsePoints_003Ek__BackingField = value;
		}
	}

	public int BufferSize
	{
		get
		{
			return _BufferSize;
		}
		set
		{
			_BufferSize = value;
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

	public bool FlattenFoldersOnExtract
	{
		get
		{
			return _003CFlattenFoldersOnExtract_003Ek__BackingField;
		}
		set
		{
			_003CFlattenFoldersOnExtract_003Ek__BackingField = value;
		}
	}

	public CompressionStrategy Strategy
	{
		get
		{
			return _Strategy;
		}
		set
		{
			_Strategy = value;
		}
	}

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
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
			return _Comment;
		}
		set
		{
			_Comment = value;
			_contentsChanged = true;
		}
	}

	public bool EmitTimesInWindowsFormatWhenSaving
	{
		get
		{
			return _emitNtfsTimes;
		}
		set
		{
			_emitNtfsTimes = value;
		}
	}

	public bool EmitTimesInUnixFormatWhenSaving
	{
		get
		{
			return _emitUnixTimes;
		}
		set
		{
			_emitUnixTimes = value;
		}
	}

	internal bool Verbose => _StatusMessageTextWriter != null;

	public bool CaseSensitiveRetrieval
	{
		get
		{
			return _CaseSensitiveRetrieval;
		}
		set
		{
			if (value != _CaseSensitiveRetrieval)
			{
				_CaseSensitiveRetrieval = value;
				_initEntriesDictionary();
			}
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
			_provisionalAlternateEncoding = (value ? Encoding.GetEncoding("UTF-8") : DefaultEncoding);
		}
	}

	public Zip64Option UseZip64WhenSaving
	{
		get
		{
			return _zip64;
		}
		set
		{
			_zip64 = value;
		}
	}

	public bool? RequiresZip64
	{
		get
		{
			if (_entries.Count > 65534)
			{
				return true;
			}
			if (!_hasBeenSaved || _contentsChanged)
			{
				return null;
			}
			foreach (ZipEntry value in _entries.Values)
			{
				if (value.RequiresZip64.Value)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool? OutputUsedZip64 => _OutputUsesZip64;

	public bool? InputUsesZip64
	{
		get
		{
			if (_entries.Count > 65534)
			{
				return true;
			}
			using (IEnumerator<ZipEntry> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ZipEntry current = enumerator.Current;
					if (current.Source != ZipEntrySource.ZipFile)
					{
						return null;
					}
					if (current._InputUsesZip64)
					{
						return true;
					}
				}
			}
			return false;
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

	public TextWriter StatusMessageTextWriter
	{
		get
		{
			return _StatusMessageTextWriter;
		}
		set
		{
			_StatusMessageTextWriter = value;
		}
	}

	public string TempFileFolder
	{
		get
		{
			return _TempFileFolder;
		}
		set
		{
			_TempFileFolder = value;
			if (value == null || Directory.Exists(value))
			{
				return;
			}
			throw new FileNotFoundException($"That directory ({value}) does not exist.");
		}
	}

	public string Password
	{
		set
		{
			_Password = value;
			if (_Password == null)
			{
				Encryption = EncryptionAlgorithm.None;
			}
			else if (Encryption == EncryptionAlgorithm.None)
			{
				Encryption = EncryptionAlgorithm.PkzipWeak;
			}
		}
	}

	public ExtractExistingFileAction ExtractExistingFile
	{
		get
		{
			return _003CExtractExistingFile_003Ek__BackingField;
		}
		set
		{
			_003CExtractExistingFile_003Ek__BackingField = value;
		}
	}

	public ZipErrorAction ZipErrorAction
	{
		get
		{
			if (this.ZipError != null)
			{
				_zipErrorAction = ZipErrorAction.InvokeErrorEvent;
			}
			return _zipErrorAction;
		}
		set
		{
			_zipErrorAction = value;
			if (_zipErrorAction != ZipErrorAction.InvokeErrorEvent && this.ZipError != null)
			{
				this.ZipError = null;
			}
		}
	}

	public EncryptionAlgorithm Encryption
	{
		get
		{
			return _Encryption;
		}
		set
		{
			if (value == EncryptionAlgorithm.Unsupported)
			{
				throw new InvalidOperationException("You may not set Encryption to that value.");
			}
			_Encryption = value;
		}
	}

	public SetCompressionCallback SetCompression
	{
		get
		{
			return _003CSetCompression_003Ek__BackingField;
		}
		set
		{
			_003CSetCompression_003Ek__BackingField = value;
		}
	}

	public int MaxOutputSegmentSize
	{
		get
		{
			return _maxOutputSegmentSize;
		}
		set
		{
			if (value < 65536 && value != 0)
			{
				throw new ZipException("The minimum acceptable segment size is 65536.");
			}
			_maxOutputSegmentSize = value;
		}
	}

	public int NumberOfSegmentsForMostRecentSave => (int)(_numberOfSegmentsForMostRecentSave + 1);

	public static Version LibraryVersion => Assembly.GetExecutingAssembly().GetName().Version;

	private List<ZipEntry> ZipEntriesAsList
	{
		get
		{
			if (_zipEntriesAsList == null)
			{
				_zipEntriesAsList = new List<ZipEntry>(_entries.Values);
			}
			return _zipEntriesAsList;
		}
	}

	public ZipEntry this[int ix] => ZipEntriesAsList[ix];

	public ZipEntry this[string fileName]
	{
		get
		{
			string key = SharedUtilities.NormalizePathForUseInZipFile(fileName);
			if (_entries.ContainsKey(key))
			{
				return _entries[key];
			}
			return null;
		}
	}

	public ICollection<string> EntryFileNames => _entries.Keys;

	public ICollection<ZipEntry> Entries => _entries.Values;

	public ICollection<ZipEntry> EntriesSorted
	{
		get
		{
			_003C_003Ec__DisplayClass1 CS_0024_003C_003E8__locals2 = new _003C_003Ec__DisplayClass1();
			List<ZipEntry> list = new List<ZipEntry>();
			foreach (ZipEntry entry in Entries)
			{
				list.Add(entry);
			}
			CS_0024_003C_003E8__locals2.sc = (CaseSensitiveRetrieval ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
			list.Sort((ZipEntry x, ZipEntry y) => string.Compare(x.FileName, y.FileName, CS_0024_003C_003E8__locals2.sc));
			return list.AsReadOnly();
		}
	}

	public int Count => _entries.Count;

	internal Stream ReadStream
	{
		get
		{
			if (_readstream == null && _name != null)
			{
				_readstream = File.OpenRead(_name);
				_ReadStreamIsOurs = true;
			}
			return _readstream;
		}
	}

	private Stream WriteStream
	{
		get
		{
			if (_writestream != null)
			{
				return _writestream;
			}
			if (_name == null)
			{
				return _writestream;
			}
			if (_maxOutputSegmentSize != 0)
			{
				_writestream = ZipSegmentedStream.ForWriting(_name, _maxOutputSegmentSize);
				return _writestream;
			}
			SharedUtilities.CreateAndOpenUniqueTempFile(TempFileFolder ?? Path.GetDirectoryName(_name), out _writestream, out _temporaryFileName);
			return _writestream;
		}
		set
		{
			if (value != null)
			{
				throw new ZipException("Cannot set the stream to a non-null value.");
			}
			_writestream = null;
		}
	}

	private string ArchiveNameForEvent
	{
		get
		{
			if (_name == null)
			{
				return "(stream)";
			}
			return _name;
		}
	}

	private long LengthOfReadStream
	{
		get
		{
			if (_lengthOfReadStream == -99)
			{
				_lengthOfReadStream = (_ReadStreamIsOurs ? SharedUtilities.GetFileLength(_name) : (-1));
			}
			return _lengthOfReadStream;
		}
	}

	public event EventHandler<SaveProgressEventArgs> SaveProgress;

	public event EventHandler<ReadProgressEventArgs> ReadProgress;

	public event EventHandler<ExtractProgressEventArgs> ExtractProgress;

	public event EventHandler<AddProgressEventArgs> AddProgress;

	public event EventHandler<ZipErrorEventArgs> ZipError;

	public ZipEntry AddItem(string fileOrDirectoryName)
	{
		return AddItem(fileOrDirectoryName, null);
	}

	public ZipEntry AddItem(string fileOrDirectoryName, string directoryPathInArchive)
	{
		if (File.Exists(fileOrDirectoryName))
		{
			return AddFile(fileOrDirectoryName, directoryPathInArchive);
		}
		if (Directory.Exists(fileOrDirectoryName))
		{
			return AddDirectory(fileOrDirectoryName, directoryPathInArchive);
		}
		throw new FileNotFoundException($"That file or directory ({fileOrDirectoryName}) does not exist!");
	}

	public ZipEntry AddFile(string fileName)
	{
		return AddFile(fileName, null);
	}

	public ZipEntry AddFile(string fileName, string directoryPathInArchive)
	{
		string nameInArchive = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
		ZipEntry ze = ZipEntry.CreateFromFile(fileName, nameInArchive);
		if (Verbose)
		{
			StatusMessageTextWriter.WriteLine("adding {0}...", fileName);
		}
		return _InternalAddEntry(ze);
	}

	public void RemoveEntries(ICollection<ZipEntry> entriesToRemove)
	{
		foreach (ZipEntry item in entriesToRemove)
		{
			RemoveEntry(item);
		}
	}

	public void RemoveEntries(ICollection<string> entriesToRemove)
	{
		foreach (string item in entriesToRemove)
		{
			RemoveEntry(item);
		}
	}

	public void AddFiles(IEnumerable<string> fileNames)
	{
		AddFiles(fileNames, null);
	}

	public void UpdateFiles(IEnumerable<string> fileNames)
	{
		UpdateFiles(fileNames, null);
	}

	public void AddFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
	{
		AddFiles(fileNames, preserveDirHierarchy: false, directoryPathInArchive);
	}

	public void AddFiles(IEnumerable<string> fileNames, bool preserveDirHierarchy, string directoryPathInArchive)
	{
		OnAddStarted();
		if (preserveDirHierarchy)
		{
			foreach (string fileName in fileNames)
			{
				if (directoryPathInArchive != null)
				{
					string fullPath = Path.GetFullPath(Path.Combine(directoryPathInArchive, Path.GetDirectoryName(fileName)));
					AddFile(fileName, fullPath);
				}
				else
				{
					AddFile(fileName, null);
				}
			}
		}
		else
		{
			foreach (string fileName2 in fileNames)
			{
				AddFile(fileName2, directoryPathInArchive);
			}
		}
		OnAddCompleted();
	}

	public void UpdateFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
	{
		OnAddStarted();
		foreach (string fileName in fileNames)
		{
			UpdateFile(fileName, directoryPathInArchive);
		}
		OnAddCompleted();
	}

	public ZipEntry UpdateFile(string fileName)
	{
		return UpdateFile(fileName, null);
	}

	public ZipEntry UpdateFile(string fileName, string directoryPathInArchive)
	{
		string fileName2 = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
		if (this[fileName2] != null)
		{
			RemoveEntry(fileName2);
		}
		return AddFile(fileName, directoryPathInArchive);
	}

	public ZipEntry UpdateDirectory(string directoryName)
	{
		return UpdateDirectory(directoryName, null);
	}

	public ZipEntry UpdateDirectory(string directoryName, string directoryPathInArchive)
	{
		return AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOrUpdate);
	}

	public void UpdateItem(string itemName)
	{
		UpdateItem(itemName, null);
	}

	public void UpdateItem(string itemName, string directoryPathInArchive)
	{
		if (File.Exists(itemName))
		{
			UpdateFile(itemName, directoryPathInArchive);
			return;
		}
		if (Directory.Exists(itemName))
		{
			UpdateDirectory(itemName, directoryPathInArchive);
			return;
		}
		throw new FileNotFoundException($"That file or directory ({itemName}) does not exist!");
	}

	public ZipEntry AddEntry(string entryName, string content)
	{
		return AddEntry(entryName, content, Encoding.Default);
	}

	public ZipEntry AddEntry(string entryName, string content, Encoding encoding)
	{
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter streamWriter = new StreamWriter(memoryStream, encoding);
		streamWriter.Write(content);
		streamWriter.Flush();
		memoryStream.Seek(0L, SeekOrigin.Begin);
		return AddEntry(entryName, memoryStream);
	}

	public ZipEntry AddEntry(string entryName, Stream stream)
	{
		ZipEntry zipEntry = ZipEntry.CreateForStream(entryName, stream);
		zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
		if (Verbose)
		{
			StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
		}
		return _InternalAddEntry(zipEntry);
	}

	public ZipEntry AddEntry(string entryName, WriteDelegate writer)
	{
		ZipEntry ze = ZipEntry.CreateForWriter(entryName, writer);
		if (Verbose)
		{
			StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
		}
		return _InternalAddEntry(ze);
	}

	public ZipEntry AddEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
	{
		ZipEntry zipEntry = ZipEntry.CreateForJitStreamProvider(entryName, opener, closer);
		zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
		if (Verbose)
		{
			StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
		}
		return _InternalAddEntry(zipEntry);
	}

	private ZipEntry _InternalAddEntry(ZipEntry ze)
	{
		ze._container = new ZipContainer(this);
		ze.CompressionLevel = CompressionLevel;
		ze.ExtractExistingFile = ExtractExistingFile;
		ze.ZipErrorAction = ZipErrorAction;
		ze.SetCompression = SetCompression;
		ze.ProvisionalAlternateEncoding = ProvisionalAlternateEncoding;
		ze.Password = _Password;
		ze.Encryption = Encryption;
		ze.EmitTimesInWindowsFormatWhenSaving = _emitNtfsTimes;
		ze.EmitTimesInUnixFormatWhenSaving = _emitUnixTimes;
		InternalAddEntry(ze.FileName, ze);
		AfterAddEntry(ze);
		return ze;
	}

	public ZipEntry UpdateEntry(string entryName, string content)
	{
		return UpdateEntry(entryName, content, Encoding.Default);
	}

	public ZipEntry UpdateEntry(string entryName, string content, Encoding encoding)
	{
		string directoryPathInArchive = null;
		if (entryName.IndexOf('\\') != -1)
		{
			directoryPathInArchive = Path.GetDirectoryName(entryName);
			entryName = Path.GetFileName(entryName);
		}
		string fileName = ZipEntry.NameInArchive(entryName, directoryPathInArchive);
		if (this[fileName] != null)
		{
			RemoveEntry(fileName);
		}
		return AddEntry(entryName, content, encoding);
	}

	public ZipEntry UpdateEntry(string entryName, Stream stream)
	{
		string directoryPathInArchive = null;
		if (entryName.IndexOf('\\') != -1)
		{
			directoryPathInArchive = Path.GetDirectoryName(entryName);
			entryName = Path.GetFileName(entryName);
		}
		string fileName = ZipEntry.NameInArchive(entryName, directoryPathInArchive);
		if (this[fileName] != null)
		{
			RemoveEntry(fileName);
		}
		return AddEntry(entryName, stream);
	}

	public ZipEntry AddEntry(string entryName, byte[] byteContent)
	{
		if (byteContent == null)
		{
			throw new ArgumentException("bad argument", "byteContent");
		}
		MemoryStream stream = new MemoryStream(byteContent);
		return AddEntry(entryName, stream);
	}

	public ZipEntry UpdateEntry(string entryName, byte[] byteContent)
	{
		string directoryPathInArchive = null;
		if (entryName.IndexOf('\\') != -1)
		{
			directoryPathInArchive = Path.GetDirectoryName(entryName);
			entryName = Path.GetFileName(entryName);
		}
		string fileName = ZipEntry.NameInArchive(entryName, directoryPathInArchive);
		if (this[fileName] != null)
		{
			RemoveEntry(fileName);
		}
		return AddEntry(entryName, byteContent);
	}

	public ZipEntry AddDirectory(string directoryName)
	{
		return AddDirectory(directoryName, null);
	}

	public ZipEntry AddDirectory(string directoryName, string directoryPathInArchive)
	{
		return AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOnly);
	}

	public ZipEntry AddDirectoryByName(string directoryNameInArchive)
	{
		ZipEntry zipEntry = ZipEntry.CreateFromNothing(directoryNameInArchive);
		zipEntry._container = new ZipContainer(this);
		zipEntry.MarkAsDirectory();
		zipEntry.ProvisionalAlternateEncoding = ProvisionalAlternateEncoding;
		zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
		zipEntry.EmitTimesInWindowsFormatWhenSaving = _emitNtfsTimes;
		zipEntry.EmitTimesInUnixFormatWhenSaving = _emitUnixTimes;
		zipEntry._Source = ZipEntrySource.Stream;
		InternalAddEntry(zipEntry.FileName, zipEntry);
		AfterAddEntry(zipEntry);
		return zipEntry;
	}

	private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action)
	{
		if (rootDirectoryPathInArchive == null)
		{
			rootDirectoryPathInArchive = "";
		}
		return AddOrUpdateDirectoryImpl(directoryName, rootDirectoryPathInArchive, action, recurse: true, 0);
	}

	internal void InternalAddEntry(string name, ZipEntry entry)
	{
		_entries.Add(name, entry);
		_zipEntriesAsList = null;
		_contentsChanged = true;
	}

	private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action, bool recurse, int level)
	{
		if (Verbose)
		{
			StatusMessageTextWriter.WriteLine("{0} {1}...", (action == AddOrUpdateAction.AddOnly) ? "adding" : "Adding or updating", directoryName);
		}
		if (level == 0)
		{
			OnAddStarted();
		}
		string text = rootDirectoryPathInArchive;
		ZipEntry zipEntry = null;
		if (level > 0)
		{
			int num = directoryName.Length;
			for (int num2 = level; num2 > 0; num2--)
			{
				num = directoryName.LastIndexOfAny("/\\".ToCharArray(), num - 1, num - 1);
			}
			text = directoryName.Substring(num + 1);
			text = Path.Combine(rootDirectoryPathInArchive, text);
		}
		if (level > 0 || rootDirectoryPathInArchive != "")
		{
			zipEntry = ZipEntry.CreateFromFile(directoryName, text);
			zipEntry._container = new ZipContainer(this);
			zipEntry.ProvisionalAlternateEncoding = ProvisionalAlternateEncoding;
			zipEntry.MarkAsDirectory();
			zipEntry.EmitTimesInWindowsFormatWhenSaving = _emitNtfsTimes;
			zipEntry.EmitTimesInUnixFormatWhenSaving = _emitUnixTimes;
			if (!_entries.ContainsKey(zipEntry.FileName))
			{
				InternalAddEntry(zipEntry.FileName, zipEntry);
				AfterAddEntry(zipEntry);
			}
			text = zipEntry.FileName;
		}
		string[] files = Directory.GetFiles(directoryName);
		if (recurse)
		{
			string[] array = files;
			foreach (string fileName in array)
			{
				if (action == AddOrUpdateAction.AddOnly)
				{
					AddFile(fileName, text);
				}
				else
				{
					UpdateFile(fileName, text);
				}
			}
			string[] directories = Directory.GetDirectories(directoryName);
			string[] array2 = directories;
			foreach (string text2 in array2)
			{
				FileAttributes attributes = (FileAttributes)NetCfFile.GetAttributes(text2);
				if (AddDirectoryWillTraverseReparsePoints || (attributes & FileAttributes.ReparsePoint) == 0)
				{
					AddOrUpdateDirectoryImpl(text2, rootDirectoryPathInArchive, action, recurse, level + 1);
				}
			}
		}
		if (level == 0)
		{
			OnAddCompleted();
		}
		return zipEntry;
	}

	public bool ContainsEntry(string name)
	{
		return _entries.ContainsKey(name);
	}

	public override string ToString()
	{
		return $"ZipFile::{Name}";
	}

	internal void NotifyEntryChanged()
	{
		_contentsChanged = true;
	}

	internal Stream StreamForDiskNumber(uint diskNumber)
	{
		if (diskNumber + 1 == _diskNumberWithCd || (diskNumber == 0 && _diskNumberWithCd == 0))
		{
			return ReadStream;
		}
		return ZipSegmentedStream.ForReading(_name, diskNumber, _diskNumberWithCd);
	}

	internal void Reset()
	{
		if (!_JustSaved)
		{
			return;
		}
		ZipFile zipFile = new ZipFile();
		zipFile._name = _name;
		zipFile.ProvisionalAlternateEncoding = ProvisionalAlternateEncoding;
		ReadIntoInstance(zipFile);
		foreach (ZipEntry item in zipFile)
		{
			using IEnumerator<ZipEntry> enumerator2 = GetEnumerator();
			while (enumerator2.MoveNext())
			{
				ZipEntry current2 = enumerator2.Current;
				if (item.FileName == current2.FileName)
				{
					current2.CopyMetaData(item);
					break;
				}
			}
		}
		zipFile.Dispose();
		_JustSaved = false;
	}

	public ZipFile(string fileName)
	{
		try
		{
			_InitInstance(fileName, null);
		}
		catch (Exception innerException)
		{
			throw new ZipException($"{fileName} is not a valid zip file", innerException);
		}
	}

	public ZipFile(string fileName, Encoding encoding)
	{
		try
		{
			_InitInstance(fileName, null);
			ProvisionalAlternateEncoding = encoding;
		}
		catch (Exception innerException)
		{
			throw new ZipException($"{fileName} is not a valid zip file", innerException);
		}
	}

	public ZipFile()
	{
		_InitInstance(null, null);
	}

	public ZipFile(Encoding encoding)
	{
		_InitInstance(null, null);
		ProvisionalAlternateEncoding = encoding;
	}

	public ZipFile(string fileName, TextWriter statusMessageWriter)
	{
		try
		{
			_InitInstance(fileName, statusMessageWriter);
		}
		catch (Exception innerException)
		{
			throw new ZipException($"{fileName} is not a valid zip file", innerException);
		}
	}

	public ZipFile(string fileName, TextWriter statusMessageWriter, Encoding encoding)
	{
		try
		{
			_InitInstance(fileName, statusMessageWriter);
			ProvisionalAlternateEncoding = encoding;
		}
		catch (Exception innerException)
		{
			throw new ZipException($"{fileName} is not a valid zip file", innerException);
		}
	}

	public void Initialize(string fileName)
	{
		try
		{
			_InitInstance(fileName, null);
		}
		catch (Exception innerException)
		{
			throw new ZipException($"{fileName} is not a valid zip file", innerException);
		}
	}

	private void _initEntriesDictionary()
	{
		StringComparer comparer = (CaseSensitiveRetrieval ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase);
		_entries = ((_entries == null) ? new Dictionary<string, ZipEntry>(comparer) : new Dictionary<string, ZipEntry>(_entries, comparer));
	}

	private void _InitInstance(string zipFileName, TextWriter statusMessageWriter)
	{
		_name = zipFileName;
		_StatusMessageTextWriter = statusMessageWriter;
		_contentsChanged = true;
		AddDirectoryWillTraverseReparsePoints = true;
		CompressionLevel = CompressionLevel.Default;
		_initEntriesDictionary();
		if (File.Exists(_name))
		{
			if (FullScan)
			{
				ReadIntoInstance_Orig(this);
			}
			else
			{
				ReadIntoInstance(this);
			}
			_fileAlreadyExists = true;
		}
	}

	public void RemoveEntry(ZipEntry entry)
	{
		_entries.Remove(SharedUtilities.NormalizePathForUseInZipFile(entry.FileName));
		_zipEntriesAsList = null;
		_contentsChanged = true;
	}

	public void RemoveEntry(string fileName)
	{
		string fileName2 = ZipEntry.NameInArchive(fileName, null);
		ZipEntry zipEntry = this[fileName2];
		if (zipEntry == null)
		{
			throw new ArgumentException("The entry you specified was not found in the zip archive.");
		}
		RemoveEntry(zipEntry);
	}

	public void Dispose()
	{
		Dispose(disposeManagedResources: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposeManagedResources)
	{
		if (_disposed)
		{
			return;
		}
		if (disposeManagedResources)
		{
			if (_ReadStreamIsOurs && _readstream != null)
			{
				_readstream.Close();
				_readstream = null;
			}
			if (_temporaryFileName != null && _name != null && _writestream != null)
			{
				_writestream.Close();
				_writestream = null;
			}
		}
		_disposed = true;
	}

	internal bool OnSaveBlock(ZipEntry entry, long bytesXferred, long totalBytesToXfer)
	{
		if (this.SaveProgress != null)
		{
			lock (LOCK)
			{
				SaveProgressEventArgs e = SaveProgressEventArgs.ByteUpdate(ArchiveNameForEvent, entry, bytesXferred, totalBytesToXfer);
				this.SaveProgress(this, e);
				if (e.Cancel)
				{
					_saveOperationCanceled = true;
				}
			}
		}
		return _saveOperationCanceled;
	}

	private void OnSaveEntry(int current, ZipEntry entry, bool before)
	{
		if (this.SaveProgress == null)
		{
			return;
		}
		lock (LOCK)
		{
			SaveProgressEventArgs e = new SaveProgressEventArgs(ArchiveNameForEvent, before, _entries.Count, current, entry);
			this.SaveProgress(this, e);
			if (e.Cancel)
			{
				_saveOperationCanceled = true;
			}
		}
	}

	private void OnSaveEvent(ZipProgressEventType eventFlavor)
	{
		if (this.SaveProgress == null)
		{
			return;
		}
		lock (LOCK)
		{
			SaveProgressEventArgs e = new SaveProgressEventArgs(ArchiveNameForEvent, eventFlavor);
			this.SaveProgress(this, e);
			if (e.Cancel)
			{
				_saveOperationCanceled = true;
			}
		}
	}

	private void OnSaveStarted()
	{
		if (this.SaveProgress != null)
		{
			lock (LOCK)
			{
				SaveProgressEventArgs e = SaveProgressEventArgs.Started(ArchiveNameForEvent);
				this.SaveProgress(this, e);
			}
		}
	}

	private void OnSaveCompleted()
	{
		if (this.SaveProgress != null)
		{
			lock (LOCK)
			{
				SaveProgressEventArgs e = SaveProgressEventArgs.Completed(ArchiveNameForEvent);
				this.SaveProgress(this, e);
			}
		}
	}

	private void OnReadStarted()
	{
		if (this.ReadProgress != null)
		{
			lock (LOCK)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.Started(ArchiveNameForEvent);
				this.ReadProgress(this, e);
			}
		}
	}

	private void OnReadCompleted()
	{
		if (this.ReadProgress != null)
		{
			lock (LOCK)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.Completed(ArchiveNameForEvent);
				this.ReadProgress(this, e);
			}
		}
	}

	internal void OnReadBytes(ZipEntry entry)
	{
		if (this.ReadProgress != null)
		{
			lock (LOCK)
			{
				ReadProgressEventArgs e = ReadProgressEventArgs.ByteUpdate(ArchiveNameForEvent, entry, ReadStream.Position, LengthOfReadStream);
				this.ReadProgress(this, e);
			}
		}
	}

	internal void OnReadEntry(bool before, ZipEntry entry)
	{
		if (this.ReadProgress != null)
		{
			lock (LOCK)
			{
				ReadProgressEventArgs e = (before ? ReadProgressEventArgs.Before(ArchiveNameForEvent, _entries.Count) : ReadProgressEventArgs.After(ArchiveNameForEvent, entry, _entries.Count));
				this.ReadProgress(this, e);
			}
		}
	}

	private void OnExtractEntry(int current, bool before, ZipEntry currentEntry, string path)
	{
		if (this.ExtractProgress == null)
		{
			return;
		}
		lock (LOCK)
		{
			ExtractProgressEventArgs e = new ExtractProgressEventArgs(ArchiveNameForEvent, before, _entries.Count, current, currentEntry, path);
			this.ExtractProgress(this, e);
			if (e.Cancel)
			{
				_extractOperationCanceled = true;
			}
		}
	}

	internal bool OnExtractBlock(ZipEntry entry, long bytesWritten, long totalBytesToWrite)
	{
		if (this.ExtractProgress != null)
		{
			lock (LOCK)
			{
				ExtractProgressEventArgs e = ExtractProgressEventArgs.ByteUpdate(ArchiveNameForEvent, entry, bytesWritten, totalBytesToWrite);
				this.ExtractProgress(this, e);
				if (e.Cancel)
				{
					_extractOperationCanceled = true;
				}
			}
		}
		return _extractOperationCanceled;
	}

	internal bool OnSingleEntryExtract(ZipEntry entry, string path, bool before)
	{
		if (this.ExtractProgress != null)
		{
			lock (LOCK)
			{
				ExtractProgressEventArgs e = (before ? ExtractProgressEventArgs.BeforeExtractEntry(ArchiveNameForEvent, entry, path) : ExtractProgressEventArgs.AfterExtractEntry(ArchiveNameForEvent, entry, path));
				this.ExtractProgress(this, e);
				if (e.Cancel)
				{
					_extractOperationCanceled = true;
				}
			}
		}
		return _extractOperationCanceled;
	}

	internal bool OnExtractExisting(ZipEntry entry, string path)
	{
		if (this.ExtractProgress != null)
		{
			lock (LOCK)
			{
				ExtractProgressEventArgs e = ExtractProgressEventArgs.ExtractExisting(ArchiveNameForEvent, entry, path);
				this.ExtractProgress(this, e);
				if (e.Cancel)
				{
					_extractOperationCanceled = true;
				}
			}
		}
		return _extractOperationCanceled;
	}

	private void OnExtractAllCompleted(string path)
	{
		if (this.ExtractProgress != null)
		{
			lock (LOCK)
			{
				ExtractProgressEventArgs e = ExtractProgressEventArgs.ExtractAllCompleted(ArchiveNameForEvent, path);
				this.ExtractProgress(this, e);
			}
		}
	}

	private void OnExtractAllStarted(string path)
	{
		if (this.ExtractProgress != null)
		{
			lock (LOCK)
			{
				ExtractProgressEventArgs e = ExtractProgressEventArgs.ExtractAllStarted(ArchiveNameForEvent, path);
				this.ExtractProgress(this, e);
			}
		}
	}

	private void OnAddStarted()
	{
		if (this.AddProgress != null)
		{
			lock (LOCK)
			{
				AddProgressEventArgs e = AddProgressEventArgs.Started(ArchiveNameForEvent);
				this.AddProgress(this, e);
			}
		}
	}

	private void OnAddCompleted()
	{
		if (this.AddProgress != null)
		{
			lock (LOCK)
			{
				AddProgressEventArgs e = AddProgressEventArgs.Completed(ArchiveNameForEvent);
				this.AddProgress(this, e);
			}
		}
	}

	internal void AfterAddEntry(ZipEntry entry)
	{
		if (this.AddProgress != null)
		{
			lock (LOCK)
			{
				AddProgressEventArgs e = AddProgressEventArgs.AfterEntry(ArchiveNameForEvent, entry, _entries.Count);
				this.AddProgress(this, e);
			}
		}
	}

	internal bool OnZipErrorSaving(ZipEntry entry, Exception exc)
	{
		if (this.ZipError != null)
		{
			lock (LOCK)
			{
				ZipErrorEventArgs e = ZipErrorEventArgs.Saving(Name, entry, exc);
				this.ZipError(this, e);
				if (e.Cancel)
				{
					_saveOperationCanceled = true;
				}
			}
		}
		return _saveOperationCanceled;
	}

	public void ExtractAll(string path)
	{
		_InternalExtractAll(path, overrideExtractExistingProperty: true);
	}

	public void ExtractAll(string path, ExtractExistingFileAction extractExistingFile)
	{
		ExtractExistingFile = extractExistingFile;
		_InternalExtractAll(path, overrideExtractExistingProperty: true);
	}

	private void _InternalExtractAll(string path, bool overrideExtractExistingProperty)
	{
		bool flag = Verbose;
		_inExtractAll = true;
		try
		{
			OnExtractAllStarted(path);
			int num = 0;
			foreach (ZipEntry value in _entries.Values)
			{
				if (flag)
				{
					StatusMessageTextWriter.WriteLine("\n{1,-22} {2,-8} {3,4}   {4,-8}  {0}", "Name", "Modified", "Size", "Ratio", "Packed");
					StatusMessageTextWriter.WriteLine(new string('-', 72));
					flag = false;
				}
				if (Verbose)
				{
					StatusMessageTextWriter.WriteLine("{1,-22} {2,-8} {3,4:F0}%   {4,-8} {0}", value.FileName, value.LastModified.ToString("yyyy-MM-dd HH:mm:ss"), value.UncompressedSize, value.CompressionRatio, value.CompressedSize);
					if (!string.IsNullOrEmpty(value.Comment))
					{
						StatusMessageTextWriter.WriteLine("  Comment: {0}", value.Comment);
					}
				}
				value.Password = _Password;
				OnExtractEntry(num, before: true, value, path);
				if (overrideExtractExistingProperty)
				{
					value.ExtractExistingFile = ExtractExistingFile;
				}
				value.Extract(path);
				num++;
				OnExtractEntry(num, before: false, value, path);
				if (_extractOperationCanceled)
				{
					break;
				}
			}
			foreach (ZipEntry value2 in _entries.Values)
			{
				if (value2.IsDirectory || value2.FileName.EndsWith("/"))
				{
					string fileOrDirectory = (value2.FileName.StartsWith("/") ? Path.Combine(path, value2.FileName.Substring(1)) : Path.Combine(path, value2.FileName));
					value2._SetTimes(fileOrDirectory, isFile: false);
				}
			}
			OnExtractAllCompleted(path);
		}
		finally
		{
			_inExtractAll = false;
		}
	}

	public static ZipFile Read(string fileName)
	{
		return Read(fileName, null, DefaultEncoding);
	}

	public static ZipFile Read(string fileName, EventHandler<ReadProgressEventArgs> readProgress)
	{
		return Read(fileName, null, DefaultEncoding, readProgress);
	}

	public static ZipFile Read(string fileName, TextWriter statusMessageWriter)
	{
		return Read(fileName, statusMessageWriter, DefaultEncoding);
	}

	public static ZipFile Read(string fileName, TextWriter statusMessageWriter, EventHandler<ReadProgressEventArgs> readProgress)
	{
		return Read(fileName, statusMessageWriter, DefaultEncoding, readProgress);
	}

	public static ZipFile Read(string fileName, Encoding encoding)
	{
		return Read(fileName, null, encoding);
	}

	public static ZipFile Read(string fileName, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
	{
		return Read(fileName, null, encoding, readProgress);
	}

	public static ZipFile Read(string fileName, TextWriter statusMessageWriter, Encoding encoding)
	{
		return Read(fileName, statusMessageWriter, encoding, null);
	}

	public static ZipFile Read(string fileName, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
	{
		ZipFile zipFile = new ZipFile();
		zipFile.ProvisionalAlternateEncoding = encoding;
		zipFile._StatusMessageTextWriter = statusMessageWriter;
		zipFile._name = fileName;
		if (readProgress != null)
		{
			zipFile.ReadProgress = readProgress;
		}
		if (zipFile.Verbose)
		{
			zipFile._StatusMessageTextWriter.WriteLine("reading from {0}...", fileName);
		}
		ReadIntoInstance(zipFile);
		zipFile._fileAlreadyExists = true;
		return zipFile;
	}

	public static ZipFile Read(Stream zipStream)
	{
		return Read(zipStream, null, DefaultEncoding);
	}

	public static ZipFile Read(Stream zipStream, EventHandler<ReadProgressEventArgs> readProgress)
	{
		return Read(zipStream, null, DefaultEncoding, readProgress);
	}

	public static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter)
	{
		return Read(zipStream, statusMessageWriter, DefaultEncoding);
	}

	public static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter, EventHandler<ReadProgressEventArgs> readProgress)
	{
		return Read(zipStream, statusMessageWriter, DefaultEncoding, readProgress);
	}

	public static ZipFile Read(Stream zipStream, Encoding encoding)
	{
		return Read(zipStream, null, encoding);
	}

	public static ZipFile Read(Stream zipStream, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
	{
		return Read(zipStream, null, encoding, readProgress);
	}

	public static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter, Encoding encoding)
	{
		return Read(zipStream, statusMessageWriter, encoding, null);
	}

	public static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
	{
		if (zipStream == null)
		{
			throw new ArgumentException("The stream must be non-null", "zipStream");
		}
		ZipFile zipFile = new ZipFile();
		zipFile._provisionalAlternateEncoding = encoding;
		if (readProgress != null)
		{
			zipFile.ReadProgress = (EventHandler<ReadProgressEventArgs>)Delegate.Combine(zipFile.ReadProgress, readProgress);
		}
		zipFile._StatusMessageTextWriter = statusMessageWriter;
		zipFile._readstream = ((zipStream.Position == 0) ? zipStream : new OffsetStream(zipStream));
		zipFile._ReadStreamIsOurs = false;
		if (zipFile.Verbose)
		{
			zipFile._StatusMessageTextWriter.WriteLine("reading from stream...");
		}
		ReadIntoInstance(zipFile);
		return zipFile;
	}

	public static ZipFile Read(byte[] buffer)
	{
		return Read(buffer, null, DefaultEncoding);
	}

	public static ZipFile Read(byte[] buffer, TextWriter statusMessageWriter)
	{
		return Read(buffer, statusMessageWriter, DefaultEncoding);
	}

	public static ZipFile Read(byte[] buffer, TextWriter statusMessageWriter, Encoding encoding)
	{
		ZipFile zipFile = new ZipFile();
		zipFile._StatusMessageTextWriter = statusMessageWriter;
		zipFile._provisionalAlternateEncoding = encoding;
		zipFile._readstream = new MemoryStream(buffer);
		zipFile._ReadStreamIsOurs = true;
		if (zipFile.Verbose)
		{
			zipFile._StatusMessageTextWriter.WriteLine("reading from byte[]...");
		}
		ReadIntoInstance(zipFile);
		return zipFile;
	}

	private static void ReadIntoInstance(ZipFile zf)
	{
		Stream readStream = zf.ReadStream;
		try
		{
			if (!readStream.CanSeek)
			{
				ReadIntoInstance_Orig(zf);
				return;
			}
			zf.OnReadStarted();
			uint num = VerifyBeginningOfZipFile(readStream);
			if (num == 101010256)
			{
				return;
			}
			int num2 = 0;
			bool flag = false;
			long num3 = readStream.Length - 64;
			long num4 = Math.Max(readStream.Length - 16384, 10L);
			do
			{
				readStream.Seek(num3, SeekOrigin.Begin);
				long num5 = SharedUtilities.FindSignature(readStream, 101010256);
				if (num5 != -1)
				{
					flag = true;
					continue;
				}
				num2++;
				num3 -= 32 * (num2 + 1) * num2;
				if (num3 < 0)
				{
					num3 = 0L;
				}
			}
			while (!flag && num3 > num4);
			if (flag)
			{
				zf._locEndOfCDS = readStream.Position - 4;
				byte[] array = new byte[16];
				readStream.Read(array, 0, array.Length);
				zf._diskNumberWithCd = BitConverter.ToUInt16(array, 2);
				if (zf._diskNumberWithCd == 65535)
				{
					throw new ZipException("Spanned archives with more than 65534 segments are not supported at this time.");
				}
				zf._diskNumberWithCd++;
				int startIndex = 12;
				uint num6 = BitConverter.ToUInt32(array, startIndex);
				if (num6 == uint.MaxValue)
				{
					Zip64SeekToCentralDirectory(zf);
				}
				else
				{
					readStream.Seek(num6, SeekOrigin.Begin);
				}
				ReadCentralDirectory(zf);
			}
			else
			{
				readStream.Seek(0L, SeekOrigin.Begin);
				ReadIntoInstance_Orig(zf);
			}
		}
		catch
		{
			if (zf._ReadStreamIsOurs && zf._readstream != null)
			{
				zf._readstream.Close();
				zf._readstream = null;
			}
			throw;
		}
		zf._contentsChanged = false;
	}

	private static void Zip64SeekToCentralDirectory(ZipFile zf)
	{
		Stream readStream = zf.ReadStream;
		byte[] array = new byte[16];
		readStream.Seek(-40L, SeekOrigin.Current);
		readStream.Read(array, 0, 16);
		long offset = BitConverter.ToInt64(array, 8);
		readStream.Seek(offset, SeekOrigin.Begin);
		uint num = (uint)SharedUtilities.ReadInt(readStream);
		if (num != 101075792)
		{
			throw new BadReadException($"  ZipFile::Read(): Bad signature (0x{num:X8}) looking for ZIP64 EoCD Record at position 0x{readStream.Position:X8}");
		}
		readStream.Read(array, 0, 8);
		long num2 = BitConverter.ToInt64(array, 0);
		array = new byte[num2];
		readStream.Read(array, 0, array.Length);
		offset = BitConverter.ToInt64(array, 36);
		readStream.Seek(offset, SeekOrigin.Begin);
	}

	private static uint VerifyBeginningOfZipFile(Stream s)
	{
		return (uint)SharedUtilities.ReadInt(s);
	}

	private static void ReadCentralDirectory(ZipFile zf)
	{
		bool flag = false;
		ZipEntry zipEntry;
		while ((zipEntry = ZipEntry.ReadDirEntry(zf)) != null)
		{
			zipEntry.ResetDirEntry();
			zf.OnReadEntry(before: true, null);
			if (zf.Verbose)
			{
				zf.StatusMessageTextWriter.WriteLine("entry {0}", zipEntry.FileName);
			}
			zf._entries.Add(zipEntry.FileName, zipEntry);
			if (zipEntry._InputUsesZip64)
			{
				flag = true;
			}
		}
		if (flag)
		{
			zf.UseZip64WhenSaving = Zip64Option.Always;
		}
		if (zf._locEndOfCDS > 0)
		{
			zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
		}
		ReadCentralDirectoryFooter(zf);
		if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
		{
			zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
		}
		if (zf.Verbose)
		{
			zf.StatusMessageTextWriter.WriteLine("read in {0} entries.", zf._entries.Count);
		}
		zf.OnReadCompleted();
	}

	private static void ReadIntoInstance_Orig(ZipFile zf)
	{
		zf.OnReadStarted();
		zf._entries = new Dictionary<string, ZipEntry>();
		if (zf.Verbose)
		{
			if (zf.Name == null)
			{
				zf.StatusMessageTextWriter.WriteLine("Reading zip from stream...");
			}
			else
			{
				zf.StatusMessageTextWriter.WriteLine("Reading zip {0}...", zf.Name);
			}
		}
		bool first = true;
		ZipContainer zc = new ZipContainer(zf);
		ZipEntry zipEntry;
		while ((zipEntry = ZipEntry.ReadEntry(zc, first)) != null)
		{
			if (zf.Verbose)
			{
				zf.StatusMessageTextWriter.WriteLine("  {0}", zipEntry.FileName);
			}
			zf._entries.Add(zipEntry.FileName, zipEntry);
			first = false;
		}
		try
		{
			ZipEntry zipEntry2;
			while ((zipEntry2 = ZipEntry.ReadDirEntry(zf)) != null)
			{
				ZipEntry zipEntry3 = zf._entries[zipEntry2.FileName];
				if (zipEntry3 != null)
				{
					zipEntry3._Comment = zipEntry2.Comment;
					if (zipEntry2.IsDirectory)
					{
						zipEntry3.MarkAsDirectory();
					}
				}
			}
			if (zf._locEndOfCDS > 0)
			{
				zf.ReadStream.Seek(zf._locEndOfCDS, SeekOrigin.Begin);
			}
			ReadCentralDirectoryFooter(zf);
			if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
			{
				zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
			}
		}
		catch
		{
		}
		zf.OnReadCompleted();
	}

	private static void ReadCentralDirectoryFooter(ZipFile zf)
	{
		Stream readStream = zf.ReadStream;
		int num = SharedUtilities.ReadSignature(readStream);
		byte[] array = null;
		int num2 = 0;
		if ((long)num == 101075792)
		{
			array = new byte[52];
			readStream.Read(array, 0, array.Length);
			long num3 = BitConverter.ToInt64(array, 0);
			if (num3 < 44)
			{
				throw new ZipException("Bad DataSize in the ZIP64 Central Directory.");
			}
			zf._versionMadeBy = BitConverter.ToUInt16(array, num2);
			num2 += 2;
			zf._versionNeededToExtract = BitConverter.ToUInt16(array, num2);
			num2 += 2;
			zf._diskNumberWithCd = BitConverter.ToUInt32(array, num2);
			num2 += 2;
			array = new byte[num3 - 44];
			readStream.Read(array, 0, array.Length);
			num = SharedUtilities.ReadSignature(readStream);
			if ((long)num != 117853008)
			{
				throw new ZipException("Inconsistent metadata in the ZIP64 Central Directory.");
			}
			array = new byte[16];
			readStream.Read(array, 0, array.Length);
			num = SharedUtilities.ReadSignature(readStream);
		}
		if ((long)num != 101010256)
		{
			readStream.Seek(-4L, SeekOrigin.Current);
			throw new BadReadException($"ZipFile::ReadCentralDirectoryFooter: Bad signature ({num:X8}) at position 0x{readStream.Position:X8}");
		}
		array = new byte[16];
		zf.ReadStream.Read(array, 0, array.Length);
		if (zf._diskNumberWithCd == 0)
		{
			zf._diskNumberWithCd = BitConverter.ToUInt16(array, 2);
		}
		ReadZipFileComment(zf);
	}

	private static void ReadZipFileComment(ZipFile zf)
	{
		byte[] array = new byte[2];
		zf.ReadStream.Read(array, 0, array.Length);
		short num = (short)(array[0] + array[1] * 256);
		if (num > 0)
		{
			array = new byte[num];
			zf.ReadStream.Read(array, 0, array.Length);
			string text = DefaultEncoding.GetString(array, 0, array.Length);
			byte[] bytes = DefaultEncoding.GetBytes(text);
			if (BlocksAreEqual(array, bytes))
			{
				zf.Comment = text;
				return;
			}
			Encoding encoding = ((zf._provisionalAlternateEncoding.CodePage == 437) ? Encoding.UTF8 : zf._provisionalAlternateEncoding);
			zf.Comment = encoding.GetString(array, 0, array.Length);
		}
	}

	private static bool BlocksAreEqual(byte[] a, byte[] b)
	{
		if (a.Length != b.Length)
		{
			return false;
		}
		for (int i = 0; i < a.Length; i++)
		{
			if (a[i] != b[i])
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsZipFile(string fileName)
	{
		return IsZipFile(fileName, testExtract: false);
	}

	public static bool IsZipFile(string fileName, bool testExtract)
	{
		bool result = false;
		try
		{
			if (!File.Exists(fileName))
			{
				return false;
			}
			using FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			result = IsZipFile(stream, testExtract);
		}
		catch
		{
		}
		return result;
	}

	public static bool IsZipFile(Stream stream, bool testExtract)
	{
		bool result = false;
		try
		{
			if (!stream.CanRead)
			{
				return false;
			}
			Stream stream2 = Stream.Null;
			using (ZipFile zipFile = Read(stream, null, Encoding.GetEncoding("IBM437")))
			{
				if (testExtract)
				{
					foreach (ZipEntry item in zipFile)
					{
						if (!item.IsDirectory)
						{
							item.Extract(stream2);
						}
					}
				}
			}
			result = true;
		}
		catch
		{
		}
		return result;
	}

	public void Save()
	{
		try
		{
			bool flag = false;
			_saveOperationCanceled = false;
			_numberOfSegmentsForMostRecentSave = 0u;
			OnSaveStarted();
			if (WriteStream == null)
			{
				throw new BadStateException("You haven't specified where to save the zip.");
			}
			if (_name != null && _name.EndsWith(".exe") && !_SavingSfx)
			{
				throw new BadStateException("You specified an EXE for a plain zip file.");
			}
			if (!_contentsChanged)
			{
				OnSaveCompleted();
				if (Verbose)
				{
					StatusMessageTextWriter.WriteLine("No save is necessary....");
				}
				return;
			}
			Reset();
			if (Verbose)
			{
				StatusMessageTextWriter.WriteLine("saving....");
			}
			if (_entries.Count >= 65535 && _zip64 == Zip64Option.Default)
			{
				throw new ZipException("The number of entries is 65535 or greater. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
			}
			int num = 0;
			ICollection<ZipEntry> collection = (SortEntriesBeforeSaving ? EntriesSorted : Entries);
			foreach (ZipEntry item in collection)
			{
				OnSaveEntry(num, item, before: true);
				item.Write(WriteStream);
				if (!_saveOperationCanceled)
				{
					num++;
					OnSaveEntry(num, item, before: false);
					if (!_saveOperationCanceled)
					{
						if (item.IncludedInMostRecentSave)
						{
							flag |= item.OutputUsedZip64.Value;
						}
						continue;
					}
					break;
				}
				break;
			}
			if (_saveOperationCanceled)
			{
				return;
			}
			ZipSegmentedStream zipSegmentedStream = WriteStream as ZipSegmentedStream;
			_numberOfSegmentsForMostRecentSave = zipSegmentedStream?.CurrentSegment ?? 1;
			bool flag2 = ZipOutput.WriteCentralDirectoryStructure(WriteStream, collection, _numberOfSegmentsForMostRecentSave, _zip64, Comment, ProvisionalAlternateEncoding);
			OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
			_hasBeenSaved = true;
			_contentsChanged = false;
			flag = flag || flag2;
			_OutputUsesZip64 = flag;
			if (_name != null && (_temporaryFileName != null || zipSegmentedStream != null))
			{
				WriteStream.Close();
				if (_saveOperationCanceled)
				{
					return;
				}
				if (_fileAlreadyExists && _readstream != null)
				{
					_readstream.Close();
					_readstream = null;
					foreach (ZipEntry item2 in collection)
					{
						item2._archiveStream = null;
					}
				}
				if (_fileAlreadyExists)
				{
					File.Delete(_name);
				}
				OnSaveEvent(ZipProgressEventType.Saving_BeforeRenameTempArchive);
				File.Move((zipSegmentedStream != null) ? zipSegmentedStream.CurrentName : _temporaryFileName, _name);
				OnSaveEvent(ZipProgressEventType.Saving_AfterRenameTempArchive);
				_fileAlreadyExists = true;
			}
			NotifyEntriesSaveComplete(collection);
			OnSaveCompleted();
			_JustSaved = true;
		}
		finally
		{
			CleanupAfterSaveOperation();
		}
	}

	private void NotifyEntriesSaveComplete(ICollection<ZipEntry> c)
	{
		foreach (ZipEntry item in c)
		{
			item.NotifySaveComplete();
		}
	}

	private void RemoveTempFile()
	{
		try
		{
			if (File.Exists(_temporaryFileName))
			{
				File.Delete(_temporaryFileName);
			}
		}
		catch (Exception ex)
		{
			if (Verbose)
			{
				StatusMessageTextWriter.WriteLine("ZipFile::Save: could not delete temp file: {0}.", ex.Message);
			}
		}
	}

	private void CleanupAfterSaveOperation()
	{
		if (_name == null)
		{
			return;
		}
		if (_writestream != null)
		{
			try
			{
				_writestream.Close();
			}
			catch
			{
			}
		}
		_writestream = null;
		if (_temporaryFileName != null)
		{
			RemoveTempFile();
			_temporaryFileName = null;
		}
	}

	public void Save(string fileName)
	{
		if (_name == null)
		{
			_writestream = null;
		}
		_name = fileName;
		if (Directory.Exists(_name))
		{
			throw new ZipException("Bad Directory", new ArgumentException("That name specifies an existing directory. Please specify a filename.", "fileName"));
		}
		_contentsChanged = true;
		_fileAlreadyExists = File.Exists(_name);
		Save();
	}

	public void Save(Stream outputStream)
	{
		if (!outputStream.CanWrite)
		{
			throw new ArgumentException("The outputStream must be a writable stream.");
		}
		_name = null;
		_writestream = new CountingStream(outputStream);
		_contentsChanged = true;
		_fileAlreadyExists = false;
		Save();
	}

	public IEnumerator<ZipEntry> GetEnumerator()
	{
		_003CGetEnumerator_003Ed__3 _003CGetEnumerator_003Ed__ = new _003CGetEnumerator_003Ed__3(0);
		_003CGetEnumerator_003Ed__._003C_003E4__this = this;
		return _003CGetEnumerator_003Ed__;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	[DispId(-4)]
	public IEnumerator GetNewEnum()
	{
		return GetEnumerator();
	}
}
