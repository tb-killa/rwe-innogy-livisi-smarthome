namespace RWE.SmartHome.SHC.ChannelInterfaces;

public class ChannelContext
{
	public string ClientId { get; set; }

	public string ChannelId { get; set; }

	public ChannelContext(string clientId, string channelType)
	{
		ClientId = clientId;
		ChannelId = channelType;
	}

	public bool Equals(ChannelContext other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		if (object.Equals(other.ClientId, ClientId))
		{
			return object.Equals(other.ChannelId, ChannelId);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj))
		{
			return false;
		}
		if (object.ReferenceEquals(this, obj))
		{
			return true;
		}
		if ((object)obj.GetType() != typeof(ChannelContext))
		{
			return false;
		}
		return Equals((ChannelContext)obj);
	}

	public override int GetHashCode()
	{
		return (((ClientId != null) ? ClientId.GetHashCode() : 0) * 397) ^ ((ChannelId != null) ? ChannelId.GetHashCode() : 0);
	}
}
