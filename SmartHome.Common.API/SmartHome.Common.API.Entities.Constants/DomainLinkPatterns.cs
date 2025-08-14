namespace SmartHome.Common.API.Entities.Constants;

public static class DomainLinkPatterns
{
	public const string LocationLinkPattern = "/location/{0}";

	public const string CapabilityLinkPattern = "/capability/{0}";

	public const string DeviceLinkPattern = "/device/{0}";

	public const string DeviceMetadataDescriptionPattern = "/desc/device/{0}.{1}/{2}";

	public const string ProductLinkPattern = "/product/{0}/{1}";

	public const string ProductEventLinkPattern = "/product/{0}/event/{1}";

	public const string EventLinkPattern = "/event/{0}";

	public const string EventDescPattern = "/desc/event/{0}";

	public const string EventProductSpecificTypePattern = "product/{0}/{1}/event/{2}";

	public const string InteractionLinkPattern = "/interaction/{0}";

	public const string DeviceSpecificActionPattern = "device/{0}.{1}/{2}/action/{3}";

	public const string DeviceSpecificActionIdentifier = "{0}.{1}";

	public const string CapabilitySpecificActionIdentifier = "{0}.{1}.{2}";

	public const string MemberSpecificActionIdentifier = "member";

	public const string HomeSpecificActionIdentifier = "home";

	public const string TypeLinkPattern = "/types/{0}";

	public const string MessageLinkPattern = "/message/{0}";

	public const string MessageProductSpecificLinkPattern = "/product/{0}/{1}";

	public const string MessageTypeSpecificLinkPattern = "/product/{0}/{1}/message/{2}";

	public const string HomeLinkPattern = "/home/{0}";

	public const string MemberLinkPattern = "/home/member/{0}";

	public const string HomeSetupLinkPattern = "/home/setup/{0}";
}
