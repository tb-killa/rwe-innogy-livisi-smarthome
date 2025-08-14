using System;

namespace RWE.SmartHome.SHC.Core;

public interface IFileDownloader
{
	Action DownloadStarted { get; set; }

	Action DownloadCompleted { get; set; }

	Action<string> DownloadInvalidResponse { get; set; }

	Action<string> DownloadServerUnavailable { get; set; }

	void DownloadFile(Uri url, string fileName, string username, string password);
}
