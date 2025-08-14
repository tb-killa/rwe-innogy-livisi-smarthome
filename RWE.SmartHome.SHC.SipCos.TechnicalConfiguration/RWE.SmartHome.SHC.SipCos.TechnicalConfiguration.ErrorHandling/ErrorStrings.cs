using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling;

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
				ResourceManager resourceManager = new ResourceManager("RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.ErrorHandling.ErrorStrings", typeof(ErrorStrings).Assembly);
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

	internal static string DefaultConfigurationNull => ResourceManager.GetString("DefaultConfigurationNull", resourceCulture);

	internal static string DeletePersistedConfigurationFailed => ResourceManager.GetString("DeletePersistedConfigurationFailed", resourceCulture);

	internal static string DeviceDoesNotExist => ResourceManager.GetString("DeviceDoesNotExist", resourceCulture);

	internal static string DeviceDoesNotExistForConfig => ResourceManager.GetString("DeviceDoesNotExistForConfig", resourceCulture);

	internal static string DeviceFactoryResetPersistenceFailed => ResourceManager.GetString("DeviceFactoryResetPersistenceFailed", resourceCulture);

	internal static string PersistConfigurationFailed => ResourceManager.GetString("PersistConfigurationFailed", resourceCulture);

	internal static string ReferenceConfigurationNull => ResourceManager.GetString("ReferenceConfigurationNull", resourceCulture);

	internal static string SequenceStateNotSuccess => ResourceManager.GetString("SequenceStateNotSuccess", resourceCulture);

	internal ErrorStrings()
	{
	}
}
