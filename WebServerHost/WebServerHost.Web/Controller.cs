using System.Net;
using WebServerHost.Web.Extensions;
using WebServerHost.Web.Http;

namespace WebServerHost.Web;

public abstract class Controller
{
	public ShcWebRequest Request { get; set; }

	protected virtual IResult Ok()
	{
		return new StatusCodeResult(HttpStatusCode.OK);
	}

	protected virtual IResult Ok(string content)
	{
		if (content != null)
		{
			return new StatusCodeResult(HttpStatusCode.OK, content);
		}
		return new JsonResult(content);
	}

	protected virtual IResult Ok(object data)
	{
		return new JsonResult(data);
	}

	protected virtual IResult Accepted(string content)
	{
		if (content != null)
		{
			return new StatusCodeResult(HttpStatusCode.Accepted, content);
		}
		return new JsonResult(content);
	}

	protected virtual IResult BadRequest(object data)
	{
		return new JsonResult(HttpStatusCode.BadRequest, data.ToJson());
	}

	protected virtual IResult BadRequest(string message)
	{
		return new ErrorResult(HttpStatusCode.BadRequest, message);
	}

	protected virtual IResult Unauthorized()
	{
		return new StatusCodeResult(HttpStatusCode.Unauthorized);
	}

	protected virtual IResult NotFound()
	{
		return new StatusCodeResult(HttpStatusCode.NotFound);
	}

	protected virtual IResult NotFound(object data)
	{
		return new StatusCodeResult(HttpStatusCode.NotFound, data.ToJson());
	}

	protected virtual IResult PreconditionFailed()
	{
		return new StatusCodeResult(HttpStatusCode.PreconditionFailed);
	}

	protected virtual IResult PreconditionFailed(object data)
	{
		return new StatusCodeResult(HttpStatusCode.PreconditionFailed, data.ToJson());
	}
}
