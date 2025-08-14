using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core;

public static class SettingsFileHelper
{
	public static bool ShouldRegisterBackendRequests()
	{
		try
		{
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			XElement xElement = XElement.Load(directoryName + "\\settings.config");
			IEnumerable<XElement> enumerable = xElement.Element("Sections").Elements();
			foreach (XElement item in enumerable)
			{
				if (!(item.Attribute("Name").Value != "RWE.SmartHome.SHC.StartupLogic"))
				{
					XElement xElement2 = item.Descendants().FirstOrDefault((XElement descendant) => descendant.Attribute("Key").Value == "StopBackendRequestsDate");
					string value = xElement2.Attribute("Value").Value;
					if (ShcDateTime.Now > DateTime.Parse(value, CultureInfo.CurrentCulture))
					{
						return false;
					}
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(Module.Core, $"There was a problem getting the StopBackendRequestsDate from persistence. {ex.Message} {ex.StackTrace}");
			if (ShcDateTime.Now > DateTime.Parse("3/1/2024", CultureInfo.CurrentCulture))
			{
				return false;
			}
			return true;
		}
	}
}
