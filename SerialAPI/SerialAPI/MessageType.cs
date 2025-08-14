namespace SerialAPI;

internal enum MessageType : byte
{
	SERIAL_TYPE_RAW = 0,
	SERIAL_TYPE_SIPCOS = 1,
	SERIAL_TYPE_COMMAND = 2,
	SERIAL_TYPE_SERIAL = 3,
	SERIAL_TYPE_RES = byte.MaxValue
}
