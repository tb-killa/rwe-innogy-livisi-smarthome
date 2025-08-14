namespace SmartHome.SHC.API.Protocols.Lemonbeat.Configuration;

public class CalculationOperand
{
	public uint? ValueId { get; set; }

	public uint? CalculationId { get; set; }

	public bool IsShcValue { get; set; }

	public uint? StateMachineId { get; set; }

	public uint? TimerId { get; set; }

	public uint? CalendarId { get; set; }

	public decimal? ConstantNumber { get; set; }

	public string ConstantString { get; set; }

	public byte[] ConstantBinary { get; set; }

	public bool? IsUpdated { get; set; }
}
