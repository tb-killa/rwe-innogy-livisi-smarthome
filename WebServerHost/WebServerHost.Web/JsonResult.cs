using System.Net;
using WebServerHost.Web.Extensions;

namespace WebServerHost.Web;

public class JsonResult : StatusCodeResult
{
	private object objectValue;

	public JsonResult(object objectValue)
		: this(HttpStatusCode.OK, objectValue)
	{
	}

	public JsonResult(HttpStatusCode statusCode, object objectValue)
		: base(statusCode, objectValue.ToJson())
	{
		this.objectValue = objectValue;
	}
}
