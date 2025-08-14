using System;

namespace WebServerHost.Services;

public class ImageMetaData
{
	public string FileName { get; set; }

	public Guid CameraId { get; set; }

	public string CameraName { get; set; }

	public string Description { get; set; }

	public DateTime CaptureTime { get; set; }

	public bool IsTriggeredFromUi { get; set; }
}
