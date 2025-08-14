using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace Microsoft.Practices.Mobile.ContainerModel.Properties;

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
				ResourceManager resourceManager = new ResourceManager("Microsoft.Practices.Mobile.ContainerModel.Properties.Resources", typeof(Resources).Assembly);
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

	internal static string Registration_CantRegisterContainer => ResourceManager.GetString("Registration_CantRegisterContainer", resourceCulture);

	internal static string Registration_IncompatibleAsType => ResourceManager.GetString("Registration_IncompatibleAsType", resourceCulture);

	internal static string ResolutionException_MissingNamedType => ResourceManager.GetString("ResolutionException_MissingNamedType", resourceCulture);

	internal static string ResolutionException_MissingType => ResourceManager.GetString("ResolutionException_MissingType", resourceCulture);

	internal static string ResolutionException_UnknownScope => ResourceManager.GetString("ResolutionException_UnknownScope", resourceCulture);

	internal Resources()
	{
	}
}
