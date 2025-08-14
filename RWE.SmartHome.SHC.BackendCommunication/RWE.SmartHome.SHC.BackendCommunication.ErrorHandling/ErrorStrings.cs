using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace RWE.SmartHome.SHC.BackendCommunication.ErrorHandling;

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
				ResourceManager resourceManager = new ResourceManager("RWE.SmartHome.SHC.BackendCommunication.ErrorHandling.ErrorStrings", typeof(ErrorStrings).Assembly);
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

	internal static string InternetAccessDisabled => ResourceManager.GetString("InternetAccessDisabled", resourceCulture);

	internal ErrorStrings()
	{
	}
}
