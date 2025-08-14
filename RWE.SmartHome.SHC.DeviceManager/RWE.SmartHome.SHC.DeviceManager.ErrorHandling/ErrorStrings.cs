using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace RWE.SmartHome.SHC.DeviceManager.ErrorHandling;

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
				ResourceManager resourceManager = new ResourceManager("RWE.SmartHome.SHC.DeviceManager.ErrorHandling.ErrorStrings", typeof(ErrorStrings).Assembly);
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

	internal static string CommandHandlerError => ResourceManager.GetString("CommandHandlerError", resourceCulture);

	internal static string DeviceDoesNotExist => ResourceManager.GetString("DeviceDoesNotExist", resourceCulture);

	internal static string DeviceInclusionError => ResourceManager.GetString("DeviceInclusionError", resourceCulture);

	internal static string DeviceKeyExchangeNotReachedYet => ResourceManager.GetString("DeviceKeyExchangeNotReachedYet", resourceCulture);

	internal static string DeviceKeyExchangeUnexpectedException => ResourceManager.GetString("DeviceKeyExchangeUnexpectedException", resourceCulture);

	internal static string DeviceUnknownInBackend => ResourceManager.GetString("DeviceUnknownInBackend", resourceCulture);

	internal static string InvalidConfigurationAction => ResourceManager.GetString("InvalidConfigurationAction", resourceCulture);

	internal ErrorStrings()
	{
	}
}
