using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace RWE.SmartHome.SHC.Core.Properties;

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
				ResourceManager resourceManager = new ResourceManager("RWE.SmartHome.SHC.Core.Properties.Resources", typeof(Resources).Assembly);
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

	internal static byte[] BaltimoreCyberTrustRoot
	{
		get
		{
			object obj = ResourceManager.GetObject("BaltimoreCyberTrustRoot", resourceCulture);
			return (byte[])obj;
		}
	}

	internal static byte[] DigiCertGlobalRootCA
	{
		get
		{
			object obj = ResourceManager.GetObject("DigiCertGlobalRootCA", resourceCulture);
			return (byte[])obj;
		}
	}

	internal static byte[] DigiCertGlobalRootG2
	{
		get
		{
			object obj = ResourceManager.GetObject("DigiCertGlobalRootG2", resourceCulture);
			return (byte[])obj;
		}
	}

	internal static byte[] DTRUSTRootClass3CA22009
	{
		get
		{
			object obj = ResourceManager.GetObject("DTRUSTRootClass3CA22009", resourceCulture);
			return (byte[])obj;
		}
	}

	internal static byte[] MicrosoftEVECCRootCA2017
	{
		get
		{
			object obj = ResourceManager.GetObject("MicrosoftEVECCRootCA2017", resourceCulture);
			return (byte[])obj;
		}
	}

	internal static byte[] MicrosoftRSARootCA2017
	{
		get
		{
			object obj = ResourceManager.GetObject("MicrosoftRSARootCA2017", resourceCulture);
			return (byte[])obj;
		}
	}

	internal static byte[] shc_ca_certs
	{
		get
		{
			object obj = ResourceManager.GetObject("shc_ca_certs", resourceCulture);
			return (byte[])obj;
		}
	}

	internal static byte[] shc_root_certs
	{
		get
		{
			object obj = ResourceManager.GetObject("shc_root_certs", resourceCulture);
			return (byte[])obj;
		}
	}

	internal static byte[] SHCRootCA_VerisignNew
	{
		get
		{
			object obj = ResourceManager.GetObject("SHCRootCA_VerisignNew", resourceCulture);
			return (byte[])obj;
		}
	}

	internal static byte[] SHCSubCA_VerisignNew
	{
		get
		{
			object obj = ResourceManager.GetObject("SHCSubCA_VerisignNew", resourceCulture);
			return (byte[])obj;
		}
	}

	internal Resources()
	{
	}
}
