namespace RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

public class OwnershipReassignmentCompletedEventArgs
{
	public bool SyncUsersAndRoles { get; set; }

	public OwnershipReassignmentCompletedEventArgs(bool syncUsersAndRoles)
	{
		SyncUsersAndRoles = syncUsersAndRoles;
	}
}
