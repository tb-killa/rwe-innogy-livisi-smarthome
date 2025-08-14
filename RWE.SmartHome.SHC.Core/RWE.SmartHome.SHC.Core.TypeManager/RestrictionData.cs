using System.Collections.Generic;

namespace RWE.SmartHome.SHC.Core.TypeManager;

public class RestrictionData
{
	public bool IsRestrictionActive { get; set; }

	public Dictionary<string, string> ApplicationParameters { get; set; }
}
