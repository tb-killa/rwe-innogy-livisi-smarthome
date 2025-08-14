using System;
using System.Globalization;
using System.Linq;

namespace RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;

public class ShcUser
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string PasswordHash { get; set; }

	public DateTime CreateDate { get; set; }

	public ShcRef[] Roles { get; set; }

	public ulong Permission { get; set; }

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "Id: {0}, Name: {1}, PasswordHash: {2}, CreateDate: {3}, Roles: ({4})", Id, Name, PasswordHash, CreateDate, (Roles == null) ? "<null>" : string.Join(", ", Roles.Select((ShcRef r) => r.ToString()).ToArray()));
	}
}
