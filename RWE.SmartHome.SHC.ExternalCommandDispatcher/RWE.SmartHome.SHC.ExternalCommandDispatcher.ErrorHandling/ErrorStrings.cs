using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace RWE.SmartHome.SHC.ExternalCommandDispatcher.ErrorHandling;

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
				ResourceManager resourceManager = new ResourceManager("RWE.SmartHome.SHC.ExternalCommandDispatcher.ErrorHandling.ErrorStrings", typeof(ErrorStrings).Assembly);
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

	internal static string NoValidTransaction => ResourceManager.GetString("NoValidTransaction", resourceCulture);

	internal static string RecipientUnknown => ResourceManager.GetString("RecipientUnknown", resourceCulture);

	internal static string SoftwareUpdateInProgress => ResourceManager.GetString("SoftwareUpdateInProgress", resourceCulture);

	internal static string TransactionScopeAlreadyInUse => ResourceManager.GetString("TransactionScopeAlreadyInUse", resourceCulture);

	internal ErrorStrings()
	{
	}
}
