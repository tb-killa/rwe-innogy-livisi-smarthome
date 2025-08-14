using System;

namespace RWE.SmartHome.Common.GlobalContracts;

public static class DefaultRoles
{
	public static readonly ShcRole ShcOwner = new ShcRole
	{
		Id = new Guid("{F3EBFAEA-E293-47c3-835C-0E429FED8FDF}"),
		Name = "Owner"
	};

	public static readonly ShcRole ShcUser = new ShcRole
	{
		Id = new Guid("{CC73CEE7-4D60-4d08-8A50-36757F87F1E4}"),
		Name = "User"
	};

	public static readonly ShcRole Guest = new ShcRole
	{
		Id = new Guid("{f465c7bd-1121-4e08-a4e7-695112504c88}"),
		Name = "Guest"
	};

	public static readonly ShcRole Shc = new ShcRole
	{
		Id = new Guid("{A0ABC0B9-DB80-4fd2-8EDB-D4122C30A58E}"),
		Name = "Shc"
	};

	public static readonly ShcRole BackendService = new ShcRole
	{
		Id = new Guid("{5619FC01-B858-4297-AB12-95AA41BA1802}"),
		Name = "BackendService"
	};

	public static readonly ShcRole UserWithoutRights = new ShcRole
	{
		Id = new Guid("{c984b6a1-9f1d-463c-bc8b-43b7b05c5b56}"),
		Name = "UserWithoutRights"
	};
}
