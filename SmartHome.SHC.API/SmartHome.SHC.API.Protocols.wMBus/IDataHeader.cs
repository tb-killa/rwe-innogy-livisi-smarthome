namespace SmartHome.SHC.API.Protocols.wMBus;

public interface IDataHeader
{
	byte AccessNumber { get; }

	byte Status { get; }

	byte[] Signature { get; }
}
