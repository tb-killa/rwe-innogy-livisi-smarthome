using System;
using System.IO;

namespace WebServerHost.Web.Http;

public interface IStreamResponse : IDisposable
{
	byte[] GetHeaderBytes();

	Stream GetStream();
}
