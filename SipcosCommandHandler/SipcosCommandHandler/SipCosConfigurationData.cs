namespace SipcosCommandHandler;

public struct SipCosConfigurationData
{
	public byte index;

	public byte value;

	public bool lastValue;

	public override string ToString()
	{
		if (lastValue)
		{
			return $"{index:X} = {value:X} (Last)";
		}
		return $"{index:X} = {value:X}";
	}
}
