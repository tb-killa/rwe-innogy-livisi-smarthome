using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace RWE.SmartHome.SHC.DataAccess.Properties;

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
				ResourceManager resourceManager = new ResourceManager("RWE.SmartHome.SHC.DataAccess.Properties.Resources", typeof(Resources).Assembly);
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

	internal static string ApplicationsSettings => ResourceManager.GetString("ApplicationsSettings", resourceCulture);

	internal static string ApplicationsSettings_PKey => ResourceManager.GetString("ApplicationsSettings_PKey", resourceCulture);

	internal static string ApplicationsToken => ResourceManager.GetString("ApplicationsToken", resourceCulture);

	internal static string BaseDevicesXml => ResourceManager.GetString("BaseDevicesXml", resourceCulture);

	internal static string BaseDevicesXml_PKey => ResourceManager.GetString("BaseDevicesXml_PKey", resourceCulture);

	internal static string ConfigurationVariables => ResourceManager.GetString("ConfigurationVariables", resourceCulture);

	internal static string ConfigurationVariables_PKey => ResourceManager.GetString("ConfigurationVariables_PKey", resourceCulture);

	internal static string DeleteAllApplicationSettings => ResourceManager.GetString("DeleteAllApplicationSettings", resourceCulture);

	internal static string DeviceActivityLogs => ResourceManager.GetString("DeviceActivityLogs", resourceCulture);

	internal static string DeviceActivityLogs_PKey => ResourceManager.GetString("DeviceActivityLogs_PKey", resourceCulture);

	internal static string GetMessagesByParamValue => ResourceManager.GetString("GetMessagesByParamValue", resourceCulture);

	internal static string GetSettingsForApplication => ResourceManager.GetString("GetSettingsForApplication", resourceCulture);

	internal static string GetSettingsNamesForApplication => ResourceManager.GetString("GetSettingsNamesForApplication", resourceCulture);

	internal static string HomeSetupsXml => ResourceManager.GetString("HomeSetupsXml", resourceCulture);

	internal static string HomeSetupsXml_PKey => ResourceManager.GetString("HomeSetupsXml_PKey", resourceCulture);

	internal static string InteractionsXml => ResourceManager.GetString("InteractionsXml", resourceCulture);

	internal static string InteractionsXml_PKey => ResourceManager.GetString("InteractionsXml_PKey", resourceCulture);

	internal static string LocationsXml => ResourceManager.GetString("LocationsXml", resourceCulture);

	internal static string LocationsXml_PKey => ResourceManager.GetString("LocationsXml_PKey", resourceCulture);

	internal static string LogicalDevicesXml => ResourceManager.GetString("LogicalDevicesXml", resourceCulture);

	internal static string LogicalDevicesXml_PKey => ResourceManager.GetString("LogicalDevicesXml_PKey", resourceCulture);

	internal static string MessageParameters => ResourceManager.GetString("MessageParameters", resourceCulture);

	internal static string MessageParameters_Messages_FKey => ResourceManager.GetString("MessageParameters_Messages_FKey", resourceCulture);

	internal static string MessageParameters_PKey => ResourceManager.GetString("MessageParameters_PKey", resourceCulture);

	internal static string Messages => ResourceManager.GetString("Messages", resourceCulture);

	internal static string Messages_PKey => ResourceManager.GetString("Messages_PKey", resourceCulture);

	internal static string ProtocolSpecificEntities => ResourceManager.GetString("ProtocolSpecificEntities", resourceCulture);

	internal static string ProtocolSpecificEntities_PKey => ResourceManager.GetString("ProtocolSpecificEntities_PKey", resourceCulture);

	internal static string SelectMessageParametersByMessage => ResourceManager.GetString("SelectMessageParametersByMessage", resourceCulture);

	internal static string SelectMessages => ResourceManager.GetString("SelectMessages", resourceCulture);

	internal static string SelectMessagesByClassAndType => ResourceManager.GetString("SelectMessagesByClassAndType", resourceCulture);

	internal static string TechnicalConfigurations => ResourceManager.GetString("TechnicalConfigurations", resourceCulture);

	internal static string TechnicalConfigurations_PKey => ResourceManager.GetString("TechnicalConfigurations_PKey", resourceCulture);

	internal static string TrackData => ResourceManager.GetString("TrackData", resourceCulture);

	internal static string TrackData_AlterEntityId => ResourceManager.GetString("TrackData_AlterEntityId", resourceCulture);

	internal static string TrackData_DropIndex => ResourceManager.GetString("TrackData_DropIndex", resourceCulture);

	internal static string TrackData_Index => ResourceManager.GetString("TrackData_Index", resourceCulture);

	internal static string TrackData_PKey => ResourceManager.GetString("TrackData_PKey", resourceCulture);

	internal static string UtilityData => ResourceManager.GetString("UtilityData", resourceCulture);

	internal static string UtilityData_Index => ResourceManager.GetString("UtilityData_Index", resourceCulture);

	internal static string UtilityData_PKey => ResourceManager.GetString("UtilityData_PKey", resourceCulture);

	internal Resources()
	{
	}
}
