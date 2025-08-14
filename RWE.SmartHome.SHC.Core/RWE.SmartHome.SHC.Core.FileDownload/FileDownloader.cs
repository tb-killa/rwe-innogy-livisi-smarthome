using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.TLSDetector;
using Rebex;
using Rebex.Net;
using SmartHome.SHC.SCommAdapter;

namespace RWE.SmartHome.SHC.Core.FileDownload;

public class FileDownloader : IFileDownloader
{
	private const int BufferDefaultSize = 16384;

	private static readonly TLSCipherDetector cipherDetector = new TLSCipherDetector("FileDownloader");

	public Action DownloadStarted { get; set; }

	public Action DownloadCompleted { get; set; }

	public Action<string> DownloadInvalidResponse { get; set; }

	public Action<string> DownloadServerUnavailable { get; set; }

	public void DownloadFile(Uri url, string fileName, string username, string password)
	{
		DownloadFile(url, fileName, username, password, TimeSpan.FromSeconds(100.0));
	}

	public void DownloadFile(Uri url, string fileName, string username, string password, TimeSpan timeout)
	{
		try
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}
			ManualResetEvent completionEvent = new ManualResetEvent(initialState: false);
			bool isUsingRebex = url.Scheme == "https";
			WebRequest request = CreateRequest(url, timeout, username, password, isUsingRebex);
			request.BeginGetResponse(delegate(IAsyncResult result)
			{
				DownloadFileAsync(result, request, fileName, timeout, isUsingRebex, ref completionEvent);
			}, null);
			if (!completionEvent.WaitOne(3600000, exitContext: false))
			{
				request.Abort();
				throw new TimeoutException("The download did not complete within one hour.");
			}
		}
		catch (Exception ex)
		{
			HandleServerUnavailable(ex.ToString());
		}
	}

	private void DownloadFileAsync(IAsyncResult result, WebRequest request, string fileName, TimeSpan timeout, bool isUsingRebex, ref ManualResetEvent completionEvent)
	{
		_ = timeout.TotalMilliseconds;
		byte[] bytes = Encoding.ASCII.GetBytes("<base64");
		byte[] buffer = new byte[16384];
		try
		{
			using (WebResponse webResponse = request.EndGetResponse(result))
			{
				RunHandler(DownloadStarted);
				using Stream stream = webResponse.GetResponseStream();
				long contentLength = webResponse.ContentLength;
				int i;
				for (i = 0; i < bytes.Length && i < contentLength; i += ReadStream(stream, buffer, timeout, isUsingRebex))
				{
				}
				HandleContent(bytes, fileName, stream, buffer, i, contentLength, timeout, isUsingRebex);
			}
			RunHandler(DownloadCompleted);
		}
		catch (Exception ex)
		{
			if (ex is WebException { Status: WebExceptionStatus.Timeout })
			{
				request.Abort();
			}
			HandleServerUnavailable(ex.ToString());
		}
		finally
		{
			completionEvent.Set();
		}
	}

	private void HandleContent(byte[] header, string fileName, Stream responseStream, byte[] buffer, int receivedBytes, long contentLength, TimeSpan timeout, bool isUsingRebex)
	{
		if (buffer.Take(header.Length).SequenceEqual(header))
		{
			HandleBase64Content(fileName, responseStream, buffer, receivedBytes, contentLength, timeout, isUsingRebex);
		}
		else
		{
			HandleBinaryContent(fileName, responseStream, buffer, receivedBytes, contentLength, timeout, isUsingRebex);
		}
	}

	private void HandleBase64Content(string fileName, Stream responseStream, byte[] buffer, int receivedBytes, long contentLength, TimeSpan timeout, bool isUsingRebex)
	{
		using DecoderBase64 decoderBase = new DecoderBase64(fileName);
		decoderBase.DecodeAndWrite(buffer, 0, receivedBytes);
		HandleStream(responseStream, decoderBase.DecodeAndWrite, DownloadInvalidResponse, contentLength - receivedBytes, timeout, buffer, isUsingRebex);
	}

	private void HandleBinaryContent(string fileName, Stream responseStream, byte[] buffer, int receivedBytes, long contentLength, TimeSpan timeout, bool isUsingRebex)
	{
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		fileStream.Write(buffer, 0, receivedBytes);
		HandleStream(responseStream, fileStream.Write, DownloadInvalidResponse, contentLength - receivedBytes, timeout, buffer, isUsingRebex);
	}

	private void HandleStream(Stream inputStream, BufferProcess bufferHandler, Action<string> failureHandler, long remainedBytes, TimeSpan timeout, byte[] buffer, bool isUsingRebex)
	{
		int num = 0;
		for (int i = 0; i < remainedBytes; i += num)
		{
			try
			{
				num = ReadStream(inputStream, buffer, timeout, isUsingRebex);
				if (num > 0)
				{
					bufferHandler(buffer, 0, num);
				}
			}
			catch (WebException)
			{
				failureHandler("Read timeout while downloading data");
				throw;
			}
			catch
			{
				failureHandler("Exception encountered while downloading data");
				throw;
			}
		}
	}

	private WebRequest CreateRequest(Uri url, TimeSpan timeout, string username, string password, bool isUsingRebex)
	{
		if (!isUsingRebex)
		{
			return GetHttpRequest(url, timeout, username, password);
		}
		return GetHttpsRequest(url, timeout, username, password);
	}

	private WebRequest GetHttpRequest(Uri url, TimeSpan timeout, string username, string password)
	{
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
		int timeout2 = (httpWebRequest.ReadWriteTimeout = (int)timeout.TotalMilliseconds);
		httpWebRequest.Timeout = timeout2;
		httpWebRequest.Credentials = new NetworkCredential(username, password);
		return httpWebRequest;
	}

	private WebRequest GetHttpsRequest(Uri url, TimeSpan timeout, string username, string password)
	{
		HttpRequestCreator httpRequestCreator = new HttpRequestCreator();
		httpRequestCreator.LogWriter = new HandleLogWriter(cipherDetector.CheckCipherLog, LogLevel.Debug);
		HttpRequestCreator httpRequestCreator2 = httpRequestCreator;
		httpRequestCreator2.Settings.SslAllowedSuites = TlsCipherSuite.Fast;
		HttpRequest httpRequest = httpRequestCreator2.Create(url);
		httpRequest.Timeout = (int)timeout.TotalMilliseconds;
		httpRequest.Credentials = new NetworkCredential(username, password);
		return httpRequest;
	}

	private int ReadStream(Stream inputStream, byte[] buffer, TimeSpan timeout, bool isUsingRebex)
	{
		if (!isUsingRebex)
		{
			return ReadStreamAsync(inputStream, buffer, timeout);
		}
		return ReadStreamOnThread(inputStream, buffer, timeout);
	}

	private int ReadStreamAsync(Stream inputStream, byte[] buffer, TimeSpan timeout)
	{
		int readBytes = -1;
		Exception exception = null;
		EventWaitHandle waitReadFinished = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);
		try
		{
			inputStream.BeginRead(buffer, 0, buffer.Length, delegate(IAsyncResult result)
			{
				try
				{
					readBytes = inputStream.EndRead(result);
				}
				catch (Exception ex)
				{
					exception = ex;
				}
				finally
				{
					try
					{
						waitReadFinished.Set();
					}
					catch (ObjectDisposedException)
					{
					}
				}
			}, null);
			if (!waitReadFinished.WaitOne((int)timeout.TotalMilliseconds, exitContext: false))
			{
				throw new WebException("read timeout", WebExceptionStatus.Timeout);
			}
			if (exception != null)
			{
				throw exception;
			}
		}
		finally
		{
			if (waitReadFinished != null)
			{
				((IDisposable)waitReadFinished).Dispose();
			}
		}
		return readBytes;
	}

	private int ReadStreamOnThread(Stream inputStream, byte[] buffer, TimeSpan timeout)
	{
		int readBytes = -1;
		Exception exception = null;
		EventWaitHandle waitReadFinished = new EventWaitHandle(initialState: false, EventResetMode.AutoReset);
		try
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				try
				{
					readBytes = inputStream.Read(buffer, 0, buffer.Length);
				}
				catch (Exception ex)
				{
					exception = ex;
				}
				finally
				{
					try
					{
						waitReadFinished.Set();
					}
					catch (ObjectDisposedException)
					{
					}
				}
			}, null);
			if (!waitReadFinished.WaitOne((int)timeout.TotalMilliseconds, exitContext: false))
			{
				throw new WebException("read timeout", WebExceptionStatus.Timeout);
			}
			if (exception != null)
			{
				throw exception;
			}
		}
		finally
		{
			if (waitReadFinished != null)
			{
				((IDisposable)waitReadFinished).Dispose();
			}
		}
		return readBytes;
	}

	private void HandleServerUnavailable(string message)
	{
		Action<string> downloadServerUnavailable = DownloadServerUnavailable;
		if (downloadServerUnavailable != null)
		{
			try
			{
				downloadServerUnavailable(message);
			}
			catch (Exception ex)
			{
				Log.Exception(Module.Core, ex, "File download exception handling failed.");
			}
		}
	}

	private void RunHandler(Action handler)
	{
		handler?.Invoke();
	}
}
