using System.Net;
using WebServerHost.Web.Http;

namespace WebServerHost.Web;

internal class ErrorResult : IResult
{
	private HttpStatusCode statusCode;

	private string message;

	public ErrorResult(HttpStatusCode statusCode, string message)
	{
		this.statusCode = statusCode;
		this.message = message;
	}

	public ShcWebResponse ExecuteResult()
	{
		return new ShcErrorResponse(statusCode, message);
	}
}
