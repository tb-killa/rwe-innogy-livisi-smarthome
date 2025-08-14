namespace WebServerHost.Web.Http;

public static class HttpMethod
{
	public const string Get = "GET";

	public const string Post = "POST";

	public const string Put = "PUT";

	public const string Patch = "PATCH";

	public const string Delete = "DELETE";

	public const string Options = "OPTIONS";

	public static string AllowedMethods = string.Format("{0}, {1}, {2}, {3}, {4}", "GET", "POST", "PUT", "PATCH", "DELETE");

	public static bool IsValid(string method)
	{
		switch (method)
		{
		default:
			return method == "DELETE";
		case "GET":
		case "POST":
		case "PUT":
		case "PATCH":
			return true;
		}
	}
}
