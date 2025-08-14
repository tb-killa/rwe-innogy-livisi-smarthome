namespace RWE.SmartHome.SHC.wMBusProtocol;

public interface IBlock
{
	byte BlockSize { get; }

	byte[] ToArray();
}
