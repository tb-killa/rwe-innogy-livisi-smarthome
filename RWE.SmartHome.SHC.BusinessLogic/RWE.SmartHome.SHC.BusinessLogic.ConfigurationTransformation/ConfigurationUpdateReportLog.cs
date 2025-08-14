using System.Collections.Generic;
using System.Linq;
using System.Text;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.ConfigurationTransformation;

public class ConfigurationUpdateReportLog
{
	public void LogConfigurationUpdateReport(ConfigurationUpdateReport configurationUpdateReport)
	{
		string interactionsLogInformation = GetInteractionsLogInformation(configurationUpdateReport);
		if (!string.IsNullOrEmpty(interactionsLogInformation))
		{
			Log.Information(Module.ConfigurationTransformation, interactionsLogInformation);
		}
	}

	private string GetInteractionsLogInformation(ConfigurationUpdateReport configurationUpdateReport)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (configurationUpdateReport != null)
		{
			AddLogMessageForInteractions(stringBuilder, configurationUpdateReport.NewInteractions, ConfigurationUpdateReportAction.added);
			AddLogMessageForInteractions(stringBuilder, configurationUpdateReport.ModifiedInteractions.Where((Interaction x) => configurationUpdateReport.NewInteractions.All((Interaction m) => m.Id != x.Id)).ToList(), ConfigurationUpdateReportAction.modified);
			AddLogMessageForInteractions(stringBuilder, configurationUpdateReport.DeletedInteractions, ConfigurationUpdateReportAction.deleted);
		}
		return stringBuilder.ToString();
	}

	private void AddLogMessageForInteractions(StringBuilder result, List<Interaction> interactions, ConfigurationUpdateReportAction action)
	{
		if (interactions != null && interactions.Count > 0)
		{
			string[] value = interactions.Select((Interaction x) => GetInteractionInformationLog(x)).ToArray();
			result.AppendFormat("The Interactions with the following Ids and Names were {0}: {1}; ", new object[2]
			{
				action,
				string.Join(",", value)
			});
		}
	}

	private string GetInteractionInformationLog(Interaction interaction)
	{
		return $"(Id: {interaction.Id.ToString()}, Name: {interaction.Name})";
	}
}
