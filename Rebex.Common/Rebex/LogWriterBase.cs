using System;
using System.Text;
using System.Threading;
using Rebex.Security.Cryptography;
using onrkn;

namespace Rebex;

public abstract class LogWriterBase : ILogWriter, IDisposable
{
	private LogLevel zammk;

	private readonly int orwxh;

	private static int wvgbf;

	public LogLevel Level
	{
		get
		{
			return zammk;
		}
		set
		{
			zammk = value;
		}
	}

	protected virtual bool IsClosed => false;

	protected LogWriterBase()
	{
		zammk = LogLevel.Info;
		orwxh = Interlocked.Increment(ref wvgbf);
	}

	public virtual void Close()
	{
		Dispose();
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public virtual void Write(LogLevel level, Type objectType, int objectId, string area, string message)
	{
		if (IsClosed && 0 == 0)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		if (level >= zammk && level != LogLevel.Off)
		{
			string text = level switch
			{
				LogLevel.Verbose => "VERBOSE", 
				LogLevel.Debug => "DEBUG", 
				LogLevel.Info => "INFO", 
				LogLevel.Error => "ERROR", 
				_ => "LEVEL" + (int)level, 
			};
			string text2 = (((object)objectType == null) ? string.Empty : objectType.Name);
			int qmuio = dahxy.qmuio;
			WriteMessage(brgjd.edcru("{0:yyyy-MM-dd HH:mm:ss} {1} {2}({3})[{4}] {5}: {6}\r\n", DateTime.Now, text, text2, objectId, qmuio, area, message));
		}
	}

	public virtual void Write(LogLevel level, Type objectType, int objectId, string area, string message, byte[] buffer, int offset, int length)
	{
		if (IsClosed && 0 == 0)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		if (buffer == null || 1 == 0)
		{
			throw new ArgumentNullException("buffer", "Buffer cannot be null.");
		}
		if (offset < 0)
		{
			throw hifyx.nztrs("offset", offset, "Argument is out of range of valid values.");
		}
		if (length < 0)
		{
			throw hifyx.nztrs("length", length, "Argument is out of range of valid values.");
		}
		if (buffer.Length - offset < length)
		{
			throw new ArgumentException("Invalid offset.", "offset");
		}
		if (level < zammk || level == LogLevel.Off)
		{
			return;
		}
		string text = "\r\n";
		StringBuilder stringBuilder = new StringBuilder(message);
		int num = 0;
		if (num != 0)
		{
			goto IL_00bb;
		}
		goto IL_0191;
		IL_0191:
		if (num < length)
		{
			goto IL_00bb;
		}
		Write(level, objectType, objectId, area, stringBuilder.ToString());
		return;
		IL_0187:
		int num2;
		int num3;
		if (num2 < num3)
		{
			goto IL_0155;
		}
		num += 16;
		goto IL_0191;
		IL_0155:
		byte b = buffer[offset + num + num2];
		if (b < 32 || b >= 127)
		{
			stringBuilder.Append('.');
		}
		else
		{
			stringBuilder.Append((char)b);
		}
		num2++;
		goto IL_0187;
		IL_00bb:
		num3 = Math.Min(16, length - num);
		string text2 = ((num3 > 8) ? (BitConverter.ToString(buffer, offset + num, 8) + ' ' + BitConverter.ToString(buffer, offset + num + 8, num3 - 8)) : BitConverter.ToString(buffer, offset + num, num3));
		stringBuilder.fazck("{3}{0}{1:X4} |{2}| ", (num > 65535) ? "" : " ", num, text2.PadRight(47), text);
		num2 = 0;
		if (num2 != 0)
		{
			goto IL_0155;
		}
		goto IL_0187;
	}

	protected virtual void WriteMessage(string message)
	{
		throw new NotSupportedException("WriteMessage method has to be implemented by a derived class.");
	}

	internal void gaahq()
	{
		knfwp(this, kcrsy.fqzqd(), qacxy.dhiwg, GetType(), orwxh, p5: false);
	}

	internal static void unxux(ILogWriter p0, rsljk p1, qacxy? p2, Type p3, int p4)
	{
		knfwp(p0, p1, p2, p3, p4, p5: false);
	}

	internal static void knfwp(ILogWriter p0, rsljk p1, qacxy? p2, Type p3, int p4, bool p5)
	{
		if (p0 != null)
		{
			string text = brgjd.yfllk(p1.xarpt, " ", p1.aoyjd, " for ", p1.uwvwe);
			qacxy? qacxy = p2;
			string text2 = ((qacxy == onrkn.qacxy.nabbw && (qacxy.HasValue ? true : false)) ? " (Trial)" : "");
			LogLevel level = ((p5 ? true : false) ? LogLevel.Debug : LogLevel.Info);
			p0.Write(level, p3, p4, "Info", brgjd.edcru("Assembly: {0}{1}", text, text2));
			p0.Write(level, p3, p4, "Info", dahxy.phtxx());
			p0.Write(LogLevel.Debug, p3, p4, "Info", brgjd.edcru("Culture: {0}", dahxy.hvexk()));
			bool useFipsAlgorithmsOnly = CryptoHelper.UseFipsAlgorithmsOnly;
			if (useFipsAlgorithmsOnly != CryptoHelper.bwwly)
			{
				p0.Write(level, p3, p4, "Info", "FIPS-only mode has been " + ((useFipsAlgorithmsOnly ? true : false) ? "enabled" : "disabled"));
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
