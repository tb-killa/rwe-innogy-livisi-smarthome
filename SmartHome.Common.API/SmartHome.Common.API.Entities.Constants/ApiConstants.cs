namespace SmartHome.Common.API.Entities.Constants;

public class ApiConstants
{
	public const string OAuthIssuerName = "SmartHomeAPI";

	public const string Audience = "all";

	public const string AuthentiactionScheme = "Bearer";

	public const string ClientIdClaim = "client_id";

	public const string TokenSigningCertificateThumbprint = "TokenSignatureCheckCertificates";

	public const string TokenLifetimeMinutes = "TokenLifetimeMinutes";

	public const string ShortTokenLifetimeMinutes = "ShortTokenLifetimeMinutes";

	public const string CacheLifetimeMinutes = "CacheLifetimeMinutes";

	public const string FailedSmartCodeAccessesCacheLifetimeMinutesKey = "FailedSmartCodeAccessesCacheLifetimeMinutes";

	public const string ClientPriorityCacheLifetimeMinutesKey = "ClientPriorityCacheLifetimeMinutes";

	public const string WebSocketUpgradeHeaderValue = "websocket";

	public const string WebSocketTokenQueryParameter = "token";

	public const string RWENamespaceValue = "core.RWE";
}
