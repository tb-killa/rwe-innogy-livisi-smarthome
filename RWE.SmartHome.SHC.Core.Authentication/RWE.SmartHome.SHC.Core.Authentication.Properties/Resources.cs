using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace RWE.SmartHome.SHC.Core.Authentication.Properties;

[DebuggerNonUserCode]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("RWE.SmartHome.SHC.Core.Authentication.Properties.Resources", typeof(Resources).Assembly);
				resourceMan = resourceManager;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static string AddUserToRole => ResourceManager.GetString("AddUserToRole", resourceCulture);

	internal static string RemoveUserFromRole => ResourceManager.GetString("RemoveUserFromRole", resourceCulture);

	internal static string Roles => ResourceManager.GetString("Roles", resourceCulture);

	internal static string Roles_PKey => ResourceManager.GetString("Roles_PKey", resourceCulture);

	internal static string SelectRoleById => ResourceManager.GetString("SelectRoleById", resourceCulture);

	internal static string SelectRolesByUser => ResourceManager.GetString("SelectRolesByUser", resourceCulture);

	internal static string SelectUserById => ResourceManager.GetString("SelectUserById", resourceCulture);

	internal static string SelectUserByName => ResourceManager.GetString("SelectUserByName", resourceCulture);

	internal static string SelectUsersByRole => ResourceManager.GetString("SelectUsersByRole", resourceCulture);

	internal static string Users => ResourceManager.GetString("Users", resourceCulture);

	internal static string Users_PKey => ResourceManager.GetString("Users_PKey", resourceCulture);

	internal static string UsersRoles => ResourceManager.GetString("UsersRoles", resourceCulture);

	internal static string UsersRoles_PKey => ResourceManager.GetString("UsersRoles_PKey", resourceCulture);

	internal static string UsersRoles_Roles_FKey => ResourceManager.GetString("UsersRoles_Roles_FKey", resourceCulture);

	internal static string UsersRoles_Users_FKey => ResourceManager.GetString("UsersRoles_Users_FKey", resourceCulture);

	internal Resources()
	{
	}
}
