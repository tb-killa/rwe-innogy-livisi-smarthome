namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public class ConditionalSwitchCommand : SwitchCommand
{
	public byte DecisionValue { get; set; }

	public ConditionalSwitchCommand(byte[] buffer)
		: base(buffer)
	{
		DecisionValue = buffer[2];
	}

	public ConditionalSwitchCommand()
	{
	}

	public override byte[] ToArray()
	{
		byte[] array = new byte[4];
		byte[] array2 = base.ToArray();
		array2.CopyTo(array, 0);
		array[2] = DecisionValue;
		return array;
	}
}
