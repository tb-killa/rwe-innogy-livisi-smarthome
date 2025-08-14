namespace RWE.SmartHome.SHC.ChannelMultiplexerInterfaces;

public class ChannelConnectivityChangedEventArgs
{
	public bool Connected { get; set; }

	public string ChannelId { get; set; }
}
