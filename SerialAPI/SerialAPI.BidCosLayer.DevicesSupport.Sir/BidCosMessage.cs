namespace SerialAPI.BidCosLayer.DevicesSupport.Sir;

public class BidCosMessage
{
	public byte Header { get; set; }

	public byte[] DestinationAddress { get; set; }

	public byte[] SenderAddress { get; set; }

	public byte Command { get; set; }

	public byte[] Data { get; set; }
}
