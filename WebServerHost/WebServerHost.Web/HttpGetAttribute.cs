namespace WebServerHost.Web;

public class HttpGetAttribute : HttpMethodAttribute
{
	public override string Method => "GET";
}
