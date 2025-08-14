namespace SerialAPI.BidCosLayer;

internal class BidCosMessageForSend
{
	public SIPcosHeader header { get; private set; }

	public byte[] message { get; private set; }

	public SendMode mode { get; private set; }

	public BidCosMessageForSend(SIPcosHeader header, byte[] message, SendMode mode)
	{
		this.header = header;
		this.message = message;
		this.mode = mode;
	}
}
