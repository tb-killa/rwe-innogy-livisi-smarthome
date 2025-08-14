namespace RWE.SmartHome.SHC.WebSocketsService.Client;

public class WSOptions
{
	public string AcceptEncoding { get; set; }

	public string AcceptLanguage { get; set; }

	public string CacheControl { get; set; }

	public string UserAgent { get; set; }

	public string Origin { get; set; }

	public string SubProtocol { get; set; }

	public string Extensions { get; set; }

	public bool MaskingEnabled { get; set; }

	public int MaxReceiveFrameLength { get; set; }

	public int MaxSendQueueSize { get; set; }

	public bool ActivityTimerEnabled { get; set; }

	public int ActivityTimeout { get; set; }

	public int HandshakeTimeout { get; set; }

	public int ReceiveTimeout { get; set; }

	public int CloseMsgTimeout { get; set; }

	public int PingRespTimeout { get; set; }

	public string Cookies { get; set; }

	public WSOptions()
	{
		Origin = "jdi-websocket";
		SubProtocol = "";
		Extensions = "";
		MaskingEnabled = true;
		MaxReceiveFrameLength = 4096;
		MaxSendQueueSize = 5;
		ActivityTimerEnabled = true;
		ActivityTimeout = 120;
		HandshakeTimeout = 5000000;
		ReceiveTimeout = 1000000;
		CloseMsgTimeout = 5000000;
		PingRespTimeout = 30;
		AcceptEncoding = string.Empty;
		AcceptLanguage = string.Empty;
		CacheControl = string.Empty;
		UserAgent = string.Empty;
		Cookies = string.Empty;
	}
}
