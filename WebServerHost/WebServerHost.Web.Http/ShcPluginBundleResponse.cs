using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WebServerHost.Web.Http;

public class ShcPluginBundleResponse : ShcWebResponse, IStreamResponse, IDisposable
{
	private class PluginsBundleStream : Stream
	{
		private static byte[] OpenBracket = Encoding.UTF8.GetBytes("[");

		private static byte[] CloseBracket = Encoding.UTF8.GetBytes("]");

		private static byte[] Comma = Encoding.UTF8.GetBytes(",");

		private readonly long length;

		private long position;

		private Queue<Stream> streams;

		public override bool CanRead => true;

		public override bool CanSeek => false;

		public override bool CanWrite => false;

		public override long Length => length;

		public override long Position
		{
			get
			{
				return position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public PluginsBundleStream(List<string> files)
		{
			streams = new Queue<Stream>(((IEnumerable<string>)files).Select((Func<string, Stream>)((string f) => File.Open(f, FileMode.Open, FileAccess.Read, FileShare.Read))));
			long num = files.Count + 1;
			foreach (string file in files)
			{
				num += new FileInfo(file).Length;
			}
			length = num;
			position = 0L;
		}

		public override void Flush()
		{
			throw new NotSupportedException("This is a read-only stream.");
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = 0;
			if (position == 0)
			{
				Array.Copy(OpenBracket, 0, buffer, offset, OpenBracket.Length);
				num += OpenBracket.Length;
				offset += OpenBracket.Length;
				count -= OpenBracket.Length;
			}
			while (count > 0 && streams.Count > 0)
			{
				int num2 = streams.Peek().Read(buffer, offset, count);
				if (num2 == 0)
				{
					streams.Dequeue().Dispose();
					if (streams.Count > 0)
					{
						Array.Copy(Comma, 0, buffer, offset, Comma.Length);
						num2 += Comma.Length;
					}
					else
					{
						Array.Copy(CloseBracket, 0, buffer, offset, CloseBracket.Length);
						num2 += CloseBracket.Length;
					}
				}
				num += num2;
				offset += num2;
				count -= num2;
			}
			position += num;
			return num;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("Cannot seek among multiple stream files");
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
	}

	private string root;

	public List<KeyValuePair<string, string>> Plugins;

	private List<string> pluginsJsonFiles = new List<string>();

	private PluginsBundleStream stream;

	public ShcPluginBundleResponse(List<KeyValuePair<string, string>> plugins, string root)
	{
		base.Version = "HTTP/1.1";
		SetHeader("Content-Type", "application/json; charset=utf-8");
		base.StatusCode = HttpStatusCode.OK;
		this.root = root;
		GetPluginFileNames(plugins);
		stream = new PluginsBundleStream(pluginsJsonFiles);
		SetHeader("Content-Length", stream.Length.ToString());
	}

	private void GetPluginFileNames(List<KeyValuePair<string, string>> plugins)
	{
		foreach (KeyValuePair<string, string> plugin in plugins)
		{
			string arg = plugin.Value.Replace("\"", "");
			pluginsJsonFiles.Add($"{root}\\plugins\\{arg}\\plugin.json");
		}
	}

	public byte[] GetHeaderBytes()
	{
		return GetBytes();
	}

	public Stream GetStream()
	{
		return stream;
	}

	public void Dispose()
	{
		if (stream != null)
		{
			stream.Close();
			stream = null;
		}
	}
}
