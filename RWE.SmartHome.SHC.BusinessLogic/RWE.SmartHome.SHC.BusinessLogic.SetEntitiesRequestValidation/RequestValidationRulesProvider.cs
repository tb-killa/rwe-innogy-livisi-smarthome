using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration;
using RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.EntityMatches;
using RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.EntityRules;
using RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation.Configuration.ValueConstrains;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.SetEntitiesRequestValidation;

public class RequestValidationRulesProvider
{
	private readonly IRepository repository;

	private AppRulesSet commonRules;

	private readonly Dictionary<string, AppRulesSet> appRules = new Dictionary<string, AppRulesSet>();

	public RequestValidationRulesProvider(IRepository repository)
	{
		this.repository = repository;
		InitCommonRules();
	}

	public void AddRulesSet(AppRulesSet rulesSet)
	{
		if (rulesSet != null && !string.IsNullOrEmpty(rulesSet.AppId))
		{
			RemoveRulesSet(rulesSet.AppId);
			appRules.Add(rulesSet.AppId, rulesSet);
		}
	}

	public void RemoveRulesSet(string appId)
	{
		appRules.Remove(appId);
	}

	public static AppRulesSet LoadCoreConfig()
	{
		string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\entitiesRequestValidation.config";
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(AppRulesSet));
		Log.Information(RWE.SmartHome.SHC.Core.Module.BusinessLogic, "Created serializer: RequestValidation");
		using StreamReader textReader = new StreamReader(path);
		return xmlSerializer.Deserialize(textReader) as AppRulesSet;
	}

	public AppRulesSet GetCommonRules()
	{
		return commonRules;
	}

	public AppRulesSet GetRulesSetForEntity(BaseDevice entity)
	{
		AppRulesSet appRulesSet = new AppRulesSet();
		appRulesSet.AddRulesSet(GetCommonRules());
		appRulesSet.AddRulesSet(GetRulesSetForApplication(entity.AppId));
		return appRulesSet;
	}

	public AppRulesSet GetRulesSetForEntity(LogicalDevice entity)
	{
		return GetRulesSetForEntity(repository.GetBaseDevice(entity.BaseDeviceId));
	}

	private AppRulesSet GetRulesSetForApplication(string appId)
	{
		if (string.IsNullOrEmpty(appId) || !appRules.ContainsKey(appId))
		{
			return null;
		}
		return appRules[appId];
	}

	private void InitCommonRules()
	{
		commonRules = new AppRulesSet
		{
			BaseDeviceRules = new List<GenericDeviceRule<BaseDevice>>(),
			LogicalDeviceRules = new List<GenericDeviceRule<LogicalDevice>>()
		};
		string[] source = new string[7] { "Id", "Manufacturer", "Type", "Version", "Product", "SerialNumber", "ProtocolId" };
		BaseDeviceRule baseDeviceRule = new BaseDeviceRule();
		baseDeviceRule.Condition = new BaseDeviceMatch
		{
			Type = "*"
		};
		baseDeviceRule.AttributesRules = source.Select((string x) => new PropertyRule
		{
			Name = x,
			Type = PropertyTypes.StringProperty,
			ValueConstraints = new List<ValueConstraint>
			{
				new ReadOnly()
			}
		}).ToList();
		BaseDeviceRule baseDeviceRule2 = baseDeviceRule;
		baseDeviceRule2.AttributesRules.Add(new PropertyRule
		{
			Name = "TimeOfAcceptance",
			Type = PropertyTypes.DateTimeProperty,
			ValueConstraints = new List<ValueConstraint>
			{
				new ReadOnly()
			}
		});
		baseDeviceRule2.AttributesRules.Add(new PropertyRule
		{
			Name = "TimeOfDiscovery",
			Type = PropertyTypes.DateTimeProperty,
			ValueConstraints = new List<ValueConstraint>
			{
				new ReadOnly()
			}
		});
		baseDeviceRule2.AttributesRules.Add(new PropertyRule
		{
			Name = "LogicalDeviceIds",
			Type = PropertyTypes.GuidList,
			ValueConstraints = new List<ValueConstraint>
			{
				new Required(),
				new ReadOnly()
			}
		});
		commonRules.BaseDeviceRules.Add(baseDeviceRule2);
		string[] source2 = new string[3] { "Id", "BaseDeviceId", "Type" };
		LogicalDeviceRule logicalDeviceRule = new LogicalDeviceRule();
		logicalDeviceRule.Condition = new LogicalDeviceMatch
		{
			Type = "*"
		};
		logicalDeviceRule.AttributesRules = source2.Select((string x) => new PropertyRule
		{
			Name = x,
			Type = PropertyTypes.StringProperty,
			ValueConstraints = new List<ValueConstraint>
			{
				new ReadOnly()
			}
		}).ToList();
		LogicalDeviceRule logicalDeviceRule2 = logicalDeviceRule;
		logicalDeviceRule2.AttributesRules.Add(new PropertyRule
		{
			Name = "ActivityLogActive",
			Type = PropertyTypes.BooleanProperty,
			ValueConstraints = new List<ValueConstraint>
			{
				new Required()
			}
		});
		commonRules.LogicalDeviceRules.Add(logicalDeviceRule2);
		commonRules.InteractionRules.Add(new InteractionRule());
		commonRules.InteractionRules.Add(new InteractionHasValidDevicesRule(repository));
	}
}
