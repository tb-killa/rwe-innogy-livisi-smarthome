namespace RWE.SmartHome.SHC.HCI;

internal class AnswerIdentifier
{
	public EndpointIdentifier EndpointIdentifier { get; private set; }

	public byte MessageIdentifier { get; private set; }

	public AnswerIdentifier(EndpointIdentifier endpointIdentifier, byte messageIdentifier)
	{
		EndpointIdentifier = endpointIdentifier;
		MessageIdentifier = messageIdentifier;
	}

	public bool Equals(AnswerIdentifier other)
	{
		if (object.ReferenceEquals(null, other))
		{
			return false;
		}
		if (object.ReferenceEquals(this, other))
		{
			return true;
		}
		if (object.Equals(other.EndpointIdentifier, EndpointIdentifier))
		{
			return other.MessageIdentifier == MessageIdentifier;
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
		if ((object)obj.GetType() != typeof(AnswerIdentifier))
		{
			return false;
		}
		return Equals((AnswerIdentifier)obj);
	}

	public override int GetHashCode()
	{
		return (EndpointIdentifier.GetHashCode() * 397) ^ MessageIdentifier.GetHashCode();
	}
}
