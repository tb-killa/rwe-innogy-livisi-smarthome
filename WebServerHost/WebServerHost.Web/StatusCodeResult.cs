using System.Net;
using WebServerHost.Web.Http;

namespace WebServerHost.Web;

public class StatusCodeResult : IResult
{
	public HttpStatusCode statusCode;

	public string body;

	public StatusCodeResult(HttpStatusCode statusCode)
		: this(statusCode, string.Empty)
	{
	}

	public StatusCodeResult(HttpStatusCode statusCode, string body)
	{
		this.statusCode = statusCode;
		this.body = body;
	}

	public ShcWebResponse ExecuteResult()
	{
		return new ShcRestResponse(statusCode, body);
	}
}
