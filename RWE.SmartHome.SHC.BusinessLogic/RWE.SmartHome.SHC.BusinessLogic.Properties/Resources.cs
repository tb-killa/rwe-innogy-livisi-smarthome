using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace RWE.SmartHome.SHC.BusinessLogic.Properties;

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
				ResourceManager resourceManager = new ResourceManager("RWE.SmartHome.SHC.BusinessLogic.Properties.Resources", typeof(Resources).Assembly);
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

	internal static byte[] SHCLogFileEncryptionCertificate
	{
		get
		{
			object obj = ResourceManager.GetObject("SHCLogFileEncryptionCertificate", resourceCulture);
			return (byte[])obj;
		}
	}

	internal Resources()
	{
	}
}
