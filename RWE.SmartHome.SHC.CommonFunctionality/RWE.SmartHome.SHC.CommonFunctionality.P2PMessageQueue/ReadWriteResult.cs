namespace RWE.SmartHome.SHC.CommonFunctionality.P2PMessageQueue;

public enum ReadWriteResult
{
	OK,
	Timeout,
	Disconnected,
	BufferFail,
	OutOfMemory,
	InvalidHandle
}
