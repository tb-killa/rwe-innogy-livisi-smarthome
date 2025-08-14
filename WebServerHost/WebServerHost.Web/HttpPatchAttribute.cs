namespace WebServerHost.Web;

public class HttpPatchAttribute : HttpMethodAttribute
{
	public override string Method => "PATCH";
}
