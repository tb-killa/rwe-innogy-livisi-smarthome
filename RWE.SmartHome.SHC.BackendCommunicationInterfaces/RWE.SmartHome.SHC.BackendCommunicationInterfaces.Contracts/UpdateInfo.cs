using System;
using System.Globalization;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

public class UpdateInfo
{
	public UpdateCategory Category { get; set; }

	public UpdateType Type { get; set; }

	public string Version { get; set; }

	public string DownloadLocation { get; set; }

	public string DownloadUser { get; set; }

	public string DownloadPassword { get; set; }

	public DateTime UpdateDeadline { get; set; }

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "Version: {0}, Category: {1}, Type: {2}, UpdateDeadline: {3}, DownloadUser: {4}, DownloadLocation: {5}", Version, Category, Type, UpdateDeadline, DownloadUser, DownloadLocation);
	}
}
