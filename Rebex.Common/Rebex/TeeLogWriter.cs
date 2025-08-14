using System;
using System.Collections.Generic;

namespace Rebex;

public class TeeLogWriter : ILogWriter, IDisposable
{
	private List<ILogWriter> mfaid;

	private LogLevel twisq;

	public LogLevel Level
	{
		get
		{
			return twisq;
		}
		set
		{
			twisq = value;
			using List<ILogWriter>.Enumerator enumerator = mfaid.GetEnumerator();
			while (enumerator.MoveNext() ? true : false)
			{
				ILogWriter current = enumerator.Current;
				current.Level = twisq;
			}
		}
	}

	public TeeLogWriter(IEnumerable<ILogWriter> writers)
	{
		if (writers == null || 1 == 0)
		{
			throw new ArgumentNullException("writers", "At least one writer is required.");
		}
		mfaid = new List<ILogWriter>(writers);
		twisq = LogLevel.Off;
		IEnumerator<ILogWriter> enumerator = writers.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				ILogWriter current = enumerator.Current;
				if (current == null || 1 == 0)
				{
					throw new InvalidOperationException("One of the supplied writers is null.");
				}
				if (current.Level < twisq)
				{
					twisq = current.Level;
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
	}

	public TeeLogWriter(params ILogWriter[] writers)
		: this((IEnumerable<ILogWriter>)writers)
	{
	}

	public void Write(LogLevel level, Type objectType, int objectId, string area, string message)
	{
		using List<ILogWriter>.Enumerator enumerator = mfaid.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			ILogWriter current = enumerator.Current;
			current.Write(level, objectType, objectId, area, message);
		}
	}

	public void Write(LogLevel level, Type objectType, int objectId, string area, string message, byte[] buffer, int offset, int length)
	{
		using List<ILogWriter>.Enumerator enumerator = mfaid.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			ILogWriter current = enumerator.Current;
			current.Write(level, objectType, objectId, area, message, buffer, offset, length);
		}
	}

	public void Dispose()
	{
		using List<ILogWriter>.Enumerator enumerator = mfaid.GetEnumerator();
		while (enumerator.MoveNext() ? true : false)
		{
			ILogWriter current = enumerator.Current;
			if (current is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
	}
}
