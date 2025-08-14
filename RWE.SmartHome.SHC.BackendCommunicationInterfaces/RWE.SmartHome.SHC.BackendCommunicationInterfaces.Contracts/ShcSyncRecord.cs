using System.Globalization;
using System.Linq;
using RWE.SmartHome.Common.GlobalContracts;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

public class ShcSyncRecord
{
	public ShcRole[] Roles { get; set; }

	public ShcUser[] Users { get; set; }

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "Roles: ({0}), Users: ({1})", new object[2]
		{
			(Roles == null) ? "<null>" : string.Join(", ", Roles.Select((ShcRole r) => r.ToString()).ToArray()),
			(Users == null) ? "<null>" : string.Join(", ", Users.Select((ShcUser u) => u.ToString()).ToArray())
		});
	}
}
