using System.Net;
using System.Text;

namespace WebServerHost.Web.Http;

public class ShcErrorResponse : ShcHttpWebResponse
{
	public ShcErrorResponse(HttpStatusCode statusCode, string message)
		: base(statusCode, null, null, GetErrorPage(statusCode, message))
	{
	}

	private static string GetErrorPage(HttpStatusCode statusCode, string message)
	{
		string arg = statusCode.ToString();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<html>\n");
		stringBuilder.Append("<head>\n");
		stringBuilder.Append($"<title>{arg}</title>\n");
		stringBuilder.Append("</head>\n");
		stringBuilder.Append("<body>\n");
		stringBuilder.Append($"<h1>{arg}</h1>\n");
		stringBuilder.Append($"<p>{message}</p>\n");
		stringBuilder.Append("<hr>\n");
		stringBuilder.Append("</body>\n");
		stringBuilder.Append("</html>\n");
		return stringBuilder.ToString();
	}
}
