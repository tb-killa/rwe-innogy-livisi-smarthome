using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace RWE.SmartHome.SHC.BackendCommunication.ApplicationTokenScope;

[Serializable]
[GeneratedCode("NetCFSvcUtil", "3.5.0.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://rwe.com/SmartHome/2011/11/15/ApplicationManagement")]
public class ApplicationTokenEntry
{
	private string appIdField;

	private string appManifestField;

	private ApplicationKind applicationKindField;

	private bool applicationKindFieldSpecified;

	private string applicationUrlField;

	private DateTime? expirationDateField;

	private bool expirationDateFieldSpecified;

	private string fullyQualifiedAssemblyNameField;

	private bool isEnabledByUserField;

	private bool isEnabledByUserFieldSpecified;

	private bool isEnabledByWebshopField;

	private bool isEnabledByWebshopFieldSpecified;

	private bool isServiceField;

	private bool isServiceFieldSpecified;

	private string nameField;

	private ApplicationParameter[] parametersField;

	private string sHCPackageChecksumField;

	private string shcPackageFilenameField;

	private string slPackageChecksumField;

	private bool updateAvailableField;

	private bool updateAvailableFieldSpecified;

	private string versionField;

	[XmlElement(IsNullable = true, Order = 0)]
	public string AppId
	{
		get
		{
			return appIdField;
		}
		set
		{
			appIdField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 1)]
	public string AppManifest
	{
		get
		{
			return appManifestField;
		}
		set
		{
			appManifestField = value;
		}
	}

	[XmlElement(Order = 2)]
	public ApplicationKind ApplicationKind
	{
		get
		{
			return applicationKindField;
		}
		set
		{
			applicationKindField = value;
		}
	}

	[XmlIgnore]
	public bool ApplicationKindSpecified
	{
		get
		{
			return applicationKindFieldSpecified;
		}
		set
		{
			applicationKindFieldSpecified = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 3)]
	public string ApplicationUrl
	{
		get
		{
			return applicationUrlField;
		}
		set
		{
			applicationUrlField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 4)]
	public DateTime? ExpirationDate
	{
		get
		{
			return expirationDateField;
		}
		set
		{
			expirationDateField = value;
		}
	}

	[XmlIgnore]
	public bool ExpirationDateSpecified
	{
		get
		{
			return expirationDateFieldSpecified;
		}
		set
		{
			expirationDateFieldSpecified = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 5)]
	public string FullyQualifiedAssemblyName
	{
		get
		{
			return fullyQualifiedAssemblyNameField;
		}
		set
		{
			fullyQualifiedAssemblyNameField = value;
		}
	}

	[XmlElement(Order = 6)]
	public bool IsEnabledByUser
	{
		get
		{
			return isEnabledByUserField;
		}
		set
		{
			isEnabledByUserField = value;
		}
	}

	[XmlIgnore]
	public bool IsEnabledByUserSpecified
	{
		get
		{
			return isEnabledByUserFieldSpecified;
		}
		set
		{
			isEnabledByUserFieldSpecified = value;
		}
	}

	[XmlElement(Order = 7)]
	public bool IsEnabledByWebshop
	{
		get
		{
			return isEnabledByWebshopField;
		}
		set
		{
			isEnabledByWebshopField = value;
		}
	}

	[XmlIgnore]
	public bool IsEnabledByWebshopSpecified
	{
		get
		{
			return isEnabledByWebshopFieldSpecified;
		}
		set
		{
			isEnabledByWebshopFieldSpecified = value;
		}
	}

	[XmlElement(Order = 8)]
	public bool IsService
	{
		get
		{
			return isServiceField;
		}
		set
		{
			isServiceField = value;
		}
	}

	[XmlIgnore]
	public bool IsServiceSpecified
	{
		get
		{
			return isServiceFieldSpecified;
		}
		set
		{
			isServiceFieldSpecified = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 9)]
	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	[XmlArray(IsNullable = true, Order = 10)]
	public ApplicationParameter[] Parameters
	{
		get
		{
			return parametersField;
		}
		set
		{
			parametersField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 11)]
	public string SHCPackageChecksum
	{
		get
		{
			return sHCPackageChecksumField;
		}
		set
		{
			sHCPackageChecksumField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 12)]
	public string ShcPackageFilename
	{
		get
		{
			return shcPackageFilenameField;
		}
		set
		{
			shcPackageFilenameField = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 13)]
	public string SlPackageChecksum
	{
		get
		{
			return slPackageChecksumField;
		}
		set
		{
			slPackageChecksumField = value;
		}
	}

	[XmlElement(Order = 14)]
	public bool UpdateAvailable
	{
		get
		{
			return updateAvailableField;
		}
		set
		{
			updateAvailableField = value;
		}
	}

	[XmlIgnore]
	public bool UpdateAvailableSpecified
	{
		get
		{
			return updateAvailableFieldSpecified;
		}
		set
		{
			updateAvailableFieldSpecified = value;
		}
	}

	[XmlElement(IsNullable = true, Order = 15)]
	public string Version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}
}
