using System.Collections.Generic;
using SmartHome.Common.API.Entities.Extensions;

namespace SmartHome.Common.API.ModelTransformationService;

internal class InvariantEqualityComparer : IEqualityComparer<string>
{
	public bool Equals(string x, string y)
	{
		return x.EqualsIgnoreCase(y);
	}

	public int GetHashCode(string obj)
	{
		return obj.GetHashCode();
	}
}
