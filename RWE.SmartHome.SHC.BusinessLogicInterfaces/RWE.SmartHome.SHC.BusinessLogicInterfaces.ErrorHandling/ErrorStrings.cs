using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ErrorHandling;

[DebuggerNonUserCode]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
public class ErrorStrings
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("RWE.SmartHome.SHC.BusinessLogicInterfaces.ErrorHandling.ErrorStrings", typeof(ErrorStrings).Assembly);
				resourceMan = resourceManager;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
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

	public static string ActuatorTypeNotSupported => ResourceManager.GetString("ActuatorTypeNotSupported", resourceCulture);

	public static string DeviceUnknown => ResourceManager.GetString("DeviceUnknown", resourceCulture);

	public static string FailedToBackupCustomApplicationsData => ResourceManager.GetString("FailedToBackupCustomApplicationsData", resourceCulture);

	public static string FailedToBackupDeviceList => ResourceManager.GetString("FailedToBackupDeviceList", resourceCulture);

	public static string FailedToBackupMessagesAndAlerts => ResourceManager.GetString("FailedToBackupMessagesAndAlerts", resourceCulture);

	public static string FailedToBackupTechnicalConfiguration => ResourceManager.GetString("FailedToBackupTechnicalConfiguration", resourceCulture);

	public static string FailedToBackupUiConfiguration => ResourceManager.GetString("FailedToBackupUiConfiguration", resourceCulture);

	public static string FailedToWriteToFlash => ResourceManager.GetString("FailedToWriteToFlash", resourceCulture);

	public static string InconsistentConfigurationData => ResourceManager.GetString("InconsistentConfigurationData", resourceCulture);

	internal ErrorStrings()
	{
	}
}
