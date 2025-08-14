using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace RWE.SmartHome.SHC.StartupLogic.ErrorHandling;

[DebuggerNonUserCode]
internal class ErrorStrings
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
				ResourceManager resourceManager = new ResourceManager("RWE.SmartHome.SHC.StartupLogic.ErrorHandling.ErrorStrings", typeof(ErrorStrings).Assembly);
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

	internal static string FailedToRestoreCustomApplicationsData => ResourceManager.GetString("FailedToRestoreCustomApplicationsData", resourceCulture);

	internal static string FailedToRestoreDeviceList => ResourceManager.GetString("FailedToRestoreDeviceList", resourceCulture);

	internal static string FailedToRestoreMessageAndAlerts => ResourceManager.GetString("FailedToRestoreMessageAndAlerts", resourceCulture);

	internal static string FailedToRestoreTechnicalConfiguration => ResourceManager.GetString("FailedToRestoreTechnicalConfiguration", resourceCulture);

	internal static string FailedToRestoreUiConfiguration => ResourceManager.GetString("FailedToRestoreUiConfiguration", resourceCulture);

	internal static string FailedToSyncUsersAndRoles => ResourceManager.GetString("FailedToSyncUsersAndRoles", resourceCulture);

	internal ErrorStrings()
	{
	}
}
