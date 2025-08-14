namespace WebServerHost.Web.Extensions;

internal static class IResultConversion
{
	public static IResult AsIResult(this object obj)
	{
		if (obj is IResult)
		{
			return (IResult)obj;
		}
		return new JsonResult(obj);
	}
}
