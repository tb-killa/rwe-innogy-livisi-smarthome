namespace SmartHome.SHC.API.SystemServices.WebSocketsService;

internal static class WSConst
{
	public enum CloseStatusCode
	{
		Normal = 1000,
		GoingAway = 1001,
		ProtocolError = 1002,
		DataTypeUnacceptable = 1003,
		NoStatusCode = 1005,
		NoCloseFrameReceived = 1006,
		DataTypeError = 1007,
		PolicyError = 1008,
		DataTooLarge = 1009,
		ExtensionNotSupported = 1010,
		UnexpectedError = 1011,
		NoTLSHandshake = 1015
	}

	public const string SchemeWS = "ws";

	public const string SchemeWSS = "wss";

	public const string HeaderEOL = "\r\n";

	public const string HeaderEOF = "\r\n\r\n";

	public const string HeaderSecurityGUID = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

	public const string Origin = "jdi-websocket";

	public const string ProtocolVersion = "13";

	public const int SendTimeout = 5000;

	public const int ReceiveTimeout = 5000;

	public const int MicrosecondsPerSecond = 1000000;

	public const int WaitHandshakeTimeout = 5000000;

	public const int WaitCloseMsgTimeout = 5000000;

	public const int WaitReceiveTimeout = 1000000;

	public const int WaitActivityTimeout = 120;

	public const int WaitPingRespTimeout = 30;

	public const bool ActivityTimerEnabled = true;

	public const bool MaskingEnabled = true;

	public const int MaxReceiveFrameLength = 4096;

	public const int MaxSendQueueSize = 5;
}
