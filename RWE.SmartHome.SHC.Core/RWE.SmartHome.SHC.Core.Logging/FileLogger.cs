using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;

namespace RWE.SmartHome.SHC.Core.Logging;

public class FileLogger : Dispatcher, IFileLogger, IService
{
	private const int LogFilesCount = 3;

	private const string LogLineFormat = "{0:u} {1,-32} {2, -12} {3}: {4}\r\n";

	private readonly string[] LogFileNames = new string[3] { "logfile1", "logfile2", "logfile3" };

	private readonly IEventManager eventManager;

	private SubscriptionToken logEventSubscriptionTokenAsync;

	private SubscriptionToken logEventSubscriptionTokenSync;

	private SubscriptionToken logSuspendEventSubscriptionTokenSync;

	private FileStream writer;

	private readonly object fileLock = new object();

	private readonly uint sizeLimit;

	private Timer flushTimer;

	private volatile int currentLog;

	private string logsPath;

	private bool suspendFlush;

	internal FileLogger(Container container)
	{
		base.Name = GetType().Name;
		eventManager = container.Resolve<IEventManager>();
		sizeLimit = 524288u;
	}

	internal FileLogger(Container container, string logsPath, uint sizeLimit)
		: this(container)
	{
		this.logsPath = logsPath;
		this.sizeLimit = sizeLimit;
	}

	public void Initialize()
	{
		currentLog = GetNextLog();
		if (writer == null)
		{
			OpenWriter();
		}
		LogEvent logEvent = eventManager.GetEvent<LogEvent>();
		if (logEventSubscriptionTokenAsync == null)
		{
			logEventSubscriptionTokenAsync = logEvent.Subscribe(InternalLog, (LogEventArgs p) => p.IsPersisted && !p.IsSynchronous, ThreadOption.SubscriberThread, this);
		}
		if (logEventSubscriptionTokenSync == null)
		{
			logEventSubscriptionTokenSync = logEvent.Subscribe(InternalLog, (LogEventArgs p) => p.IsPersisted && p.IsSynchronous, ThreadOption.PublisherThread, this);
		}
		logSuspendEventSubscriptionTokenSync = eventManager.GetEvent<LogSuspendEvent>().Subscribe(SuspendLog, (LogSuspendEventArgs _) => true, ThreadOption.PublisherThread, this);
		flushTimer = new Timer(Flush, null, TimeSpan.FromSeconds(60.0), TimeSpan.FromSeconds(60.0));
	}

	private void SuspendLog(LogSuspendEventArgs args)
	{
		Flush(null);
		suspendFlush = true;
	}

	public void Uninitialize()
	{
		if (flushTimer != null)
		{
			flushTimer.Dispose();
			flushTimer = null;
		}
		if (writer != null)
		{
			CloseWriter();
		}
		LogEvent logEvent = eventManager.GetEvent<LogEvent>();
		if (logEventSubscriptionTokenSync != null)
		{
			logEvent.Unsubscribe(logEventSubscriptionTokenSync);
			logEventSubscriptionTokenSync = null;
		}
		if (logEventSubscriptionTokenAsync != null)
		{
			logEvent.Unsubscribe(logEventSubscriptionTokenAsync);
			logEventSubscriptionTokenAsync = null;
		}
	}

	public bool ProcessAllLines(Func<string, bool> action)
	{
		Flush();
		string[] array = new string[3];
		for (int i = 0; i < 3; i++)
		{
			array[i] = GetFullLogFileName(i);
		}
		foreach (string item in array.OrderBy((string s) => (!File.Exists(s)) ? DateTime.MaxValue : File.GetLastWriteTime(s)))
		{
			if (!File.Exists(item))
			{
				continue;
			}
			using StreamReader streamReader = new StreamReader(new FileStream(item, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
			while (true)
			{
				string text = streamReader.ReadLine();
				if (text != null)
				{
					if (!action(text + "\r\n"))
					{
						return false;
					}
					continue;
				}
				break;
			}
		}
		return true;
	}

	public void StopLoggingAndMoveTo(string path)
	{
		try
		{
			lock (fileLock)
			{
				CloseWriter();
				for (int i = 0; i < 3; i++)
				{
					try
					{
						string fullLogFileName = GetFullLogFileName(i);
						if (File.Exists(fullLogFileName))
						{
							File.Move(fullLogFileName, Path.Combine(path, LogFileNames[i]));
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("Failed to move file: " + ex);
					}
				}
			}
		}
		catch
		{
		}
	}

	public void RestoreLogFrom(string path)
	{
		lock (fileLock)
		{
			CloseWriter();
			for (int i = 0; i < 3; i++)
			{
				try
				{
					if (File.Exists(GetFullLogFileName(i)))
					{
						File.Delete(GetFullLogFileName(i));
					}
					if (File.Exists(Path.Combine(path, LogFileNames[i])))
					{
						File.Move(Path.Combine(path, LogFileNames[i]), GetFullLogFileName(i));
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Failed to restore logs: " + ex);
				}
			}
			currentLog = GetNextLog();
			OpenWriter();
		}
	}

	public void Flush()
	{
		Flush(null);
	}

	public void RestoreLegacyLog(string fileName)
	{
		LegacyLogPersistence legacyLogPersistence = new LegacyLogPersistence();
		try
		{
			legacyLogPersistence.LoadFile(fileName);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Unable to restore old logs: {0}", ex.ToString());
		}
		foreach (LegacyLogEntry logEntry in legacyLogPersistence.LogEntries)
		{
			try
			{
				InternalLog(new LogEventArgs
				{
					IsPersisted = true,
					IsSynchronous = true,
					LogLevel = GetSeverityFromLegacyEntry(logEntry.Severity),
					Message = logEntry.Message,
					Module = GetModuleFromLegacyEntry(logEntry.Source),
					Source = GetSourceFromLegacyEntry(logEntry.Source),
					Timestamp = logEntry.Timestamp
				});
			}
			catch (Exception ex2)
			{
				Console.WriteLine("Failed to restore legacy log entry: {0}", ex2.ToString());
			}
		}
		try
		{
			File.Delete(fileName);
		}
		catch (Exception ex3)
		{
			Console.WriteLine("Could not remove legacy file: {0}", ex3.ToString());
		}
	}

	private Severity GetSeverityFromLegacyEntry(byte severity)
	{
		try
		{
			return (Severity)severity;
		}
		catch
		{
			return Severity.Information;
		}
	}

	private Module GetModuleFromLegacyEntry(string source)
	{
		int num = source.IndexOf('.');
		string value = ((num > 0) ? source.Substring(0, num) : source);
		try
		{
			return (Module)Enum.Parse(typeof(Module), value, ignoreCase: true);
		}
		catch
		{
			return Module.Core;
		}
	}

	private string GetSourceFromLegacyEntry(string source)
	{
		int num = source.IndexOf('.');
		if (num <= 0)
		{
			return source;
		}
		return source.Substring(num);
	}

	public override void Start()
	{
		Initialize();
		base.Start();
	}

	public override void Stop()
	{
		Uninitialize();
		base.Stop();
	}

	private void InternalLog(LogEventArgs args)
	{
		if (suspendFlush)
		{
			return;
		}
		try
		{
			string s = $"{args.Timestamp:u} {args.Module,-32} {args.LogLevel,-12} {args.Source}: {args.Message}\r\n";
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			lock (fileLock)
			{
				if (writer != null && writer.CanWrite)
				{
					if (writer.Position + bytes.Length > sizeLimit)
					{
						CloseWriter();
						currentLog = (currentLog + 1) % 3;
						OpenWriter();
					}
					writer.Write(bytes, 0, bytes.Length);
				}
			}
		}
		catch
		{
		}
	}

	private void OpenWriter()
	{
		string fullLogFileName = GetFullLogFileName(currentLog);
		writer = new FileStream(fullLogFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 4096);
	}

	private string GetFullLogFileName(int index)
	{
		if (string.IsNullOrEmpty(logsPath))
		{
			logsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
		}
		return Path.Combine(logsPath, LogFileNames[index]);
	}

	private void CloseWriter()
	{
		try
		{
			Flush();
			writer.Close();
			writer = null;
		}
		catch
		{
		}
	}

	private void Flush(object o)
	{
		if (suspendFlush)
		{
			return;
		}
		lock (fileLock)
		{
			try
			{
				if (writer != null)
				{
					writer.Flush();
				}
			}
			catch
			{
			}
		}
	}

	private int GetNextLog()
	{
		int num = -1;
		DateTime dateTime = DateTime.MinValue;
		for (int i = 0; i < 3; i++)
		{
			string fullLogFileName = GetFullLogFileName(i);
			if (File.Exists(fullLogFileName) && File.GetLastWriteTime(fullLogFileName) > dateTime)
			{
				dateTime = File.GetLastWriteTime(fullLogFileName);
				num = i;
			}
		}
		return (num + 1) % 3;
	}
}
