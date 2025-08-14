namespace SipcosCommandHandler;

public struct SipCosPartner
{
	public byte[] ip;

	public byte channel;

	public bool last;

	public override string ToString()
	{
		if (ip != null && ip.Length == 3)
		{
			string text = $"IP: {ip[0]:X2} {ip[1]:X2} {ip[2]:X2} Channel: {channel}";
			if (last)
			{
				text += " (last)";
			}
			return text;
		}
		return "IP: N/A Channel: 0";
	}
}
