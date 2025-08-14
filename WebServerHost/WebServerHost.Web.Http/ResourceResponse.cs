using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace WebServerHost.Web.Http;

public class ResourceResponse : ShcWebResponse, IStreamResponse, IDisposable
{
	private FileStream resourceFile;

	public string ResourcePath { get; private set; }

	public ResourceResponse(string resourcePath)
	{
		ResourcePath = resourcePath;
		base.Version = "HTTP/1.1";
		SetHeader("Content-Type", Common.GetMimeType(Path.GetExtension(ResourcePath)));
		SetHeader("Accept-Ranges", "bytes");
		if (IsCompressable(Path.GetExtension(resourcePath)))
		{
			SetHeader("Content-Encoding", "gzip");
			SetHeader("Transfer-Encoding", "gzip");
		}
		resourceFile = new FileStream(ResourcePath, FileMode.Open, FileAccess.Read, FileShare.Read);
		if (resourceFile != null)
		{
			base.StatusCode = HttpStatusCode.OK;
		}
		else
		{
			base.StatusCode = HttpStatusCode.NotFound;
		}
		SetHeader("Content-Length", resourceFile.Length.ToString());
	}

	public byte[] GetHeaderBytes()
	{
		return GetBytes();
	}

	public Stream GetStream()
	{
		return resourceFile;
	}

	public void Dispose()
	{
		if (resourceFile != null)
		{
			resourceFile.Close();
			resourceFile = null;
		}
	}

	private bool IsCompressable(string mimetype)
	{
		List<string> list = new List<string>();
		list.Add(".htm");
		list.Add(".html");
		list.Add(".bmp");
		list.Add(".ico");
		list.Add(".cgi");
		list.Add(".js");
		list.Add(".svg");
		list.Add(".css");
		list.Add(".ttf");
		list.Add(".plist");
		list.Add(".xml");
		list.Add(".htc");
		list.Add(".ai");
		list.Add(".an");
		list.Add(".txt");
		List<string> list2 = list;
		return list2.Contains(mimetype.ToLower());
	}
}
