namespace SipcosCommandHandler;

public enum SIPcosStatusType : byte
{
	STATUS_REQUEST = 0,
	STATUS_FRAME = 1,
	STATUS_FRAME_CC_SENSOR = 2,
	STATUS_FRAME_TIME_SLOT = byte.MaxValue
}
