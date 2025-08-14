using WebServerHost.Web.Http;

namespace WebServerHost.Web;

public interface IResult
{
	ShcWebResponse ExecuteResult();
}
