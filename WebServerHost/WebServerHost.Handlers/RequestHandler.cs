using WebServerHost.Web.Http;

namespace WebServerHost.Handlers;

public abstract class RequestHandler
{
	public IAuthorization Authorization { get; internal set; }

	protected virtual void Authorize(ShcWebRequest request)
	{
		if (Authorization != null)
		{
			Authorization.Authorize(request);
		}
	}

	public abstract ShcWebResponse HandleRequest(ShcWebRequest request);
}
