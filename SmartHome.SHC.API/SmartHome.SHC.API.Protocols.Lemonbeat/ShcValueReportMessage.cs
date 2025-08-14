namespace SmartHome.SHC.API.Protocols.Lemonbeat;

public class ShcValueReportMessage : ControlMessage
{
	public decimal ValueToBeReported { get; private set; }

	public string RegisteredValueName { get; private set; }

	public ShcValueReportMessage(string registeredValueName, decimal valueToBeReported, Transport transportMode)
	{
		ValueToBeReported = valueToBeReported;
		RegisteredValueName = registeredValueName;
		base.Transport = transportMode;
	}
}
