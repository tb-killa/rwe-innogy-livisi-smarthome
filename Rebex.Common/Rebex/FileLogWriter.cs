using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using onrkn;

namespace Rebex;

public class FileLogWriter : LogWriterBase
{
	private class xymid
	{
		private readonly object jmmcu;

		private readonly string wufqf;

		private readonly Stream jgveg;

		private readonly TextWriter mfyjo;

		private int eatbs;

		public string feedh => wufqf;

		public int oragp()
		{
			eatbs++;
			return eatbs;
		}

		public int sbyts()
		{
			eatbs--;
			return eatbs;
		}

		public xymid(string key, string path)
		{
			jmmcu = new object();
			wufqf = key;
			eatbs = 1;
			jgveg = vtdxm.vswch(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
			mfyjo = new StreamWriter(jgveg, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
		}

		public void xumbu(string p0, bool p1)
		{
			lock (jmmcu)
			{
				mfyjo.Write(p0);
				if (p1 && 0 == 0)
				{
					mfyjo.Flush();
				}
			}
		}

		public void jpahd()
		{
			if (jgveg.CanWrite && 0 == 0)
			{
				mfyjo.Close();
			}
		}
	}

	private readonly string dtcfa;

	private xymid xjdfh;

	private static readonly Dictionary<string, xymid> exvwe = new Dictionary<string, xymid>();

	public string Path => dtcfa;

	[Obsolete("The Filename property has been deprecated. Please use the Path property instead.", false)]
	[wptwl(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string Filename => dtcfa;

	protected override bool IsClosed => xjdfh == null;

	public FileLogWriter(string path)
		: this(path, LogLevel.Info)
	{
	}

	public FileLogWriter(string path, LogLevel level)
	{
		if (path == null || 1 == 0)
		{
			throw new ArgumentNullException("path");
		}
		if (path.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("Path cannot be empty.", "path");
		}
		dtcfa = path;
		base.Level = level;
		string text = vtdxm.smjda(dtcfa);
		if (!dahxy.xzevd || 1 == 0)
		{
			text = text.ToLower(CultureInfo.InvariantCulture);
		}
		lock (exvwe)
		{
			if (!exvwe.TryGetValue(text, out xjdfh) || 1 == 0)
			{
				xjdfh = new xymid(text, path);
				exvwe.Add(text, xjdfh);
				if (base.Level != LogLevel.Off)
				{
					xjdfh.xumbu(brgjd.edcru("{0:yyyy-MM-dd HH:mm:ss} Opening log file.\r\n", DateTime.Now), p1: false);
					gaahq();
				}
			}
			else
			{
				xjdfh.oragp();
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		lock (exvwe)
		{
			xymid xymid = xjdfh;
			if (xymid != null)
			{
				xjdfh = null;
				if (xymid.sbyts() == 0 || 1 == 0)
				{
					exvwe.Remove(xymid.feedh);
					xymid.jpahd();
				}
			}
		}
	}

	protected override void WriteMessage(string message)
	{
		xymid xymid = xjdfh;
		if (xymid == null || 1 == 0)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		xymid.xumbu(message, p1: true);
	}

	~FileLogWriter()
	{
		Dispose(disposing: false);
	}
}
