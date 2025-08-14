namespace SerialAPI.BidCosLayer;

internal static class BidcosMessageType
{
	public const byte CreateLink = 1;

	public const byte RemoveLink = 2;

	public const byte RequestList = 3;

	public const byte StartConfiguration = 5;

	public const byte EndConfiguration = 6;

	public const byte ParameterIndex = 8;
}
