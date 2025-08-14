namespace WebServerHost.Helpers;

public static class Validator
{
	public static bool ValidatePageParameters(int? page, int? pageSize)
	{
		if (!page.HasValue || !(page < 1))
		{
			if (pageSize.HasValue)
			{
				int? num = pageSize;
				if (num.GetValueOrDefault() < 1 && num.HasValue && pageSize > 500)
				{
					goto IL_005e;
				}
			}
			return true;
		}
		goto IL_005e;
		IL_005e:
		return false;
	}
}
