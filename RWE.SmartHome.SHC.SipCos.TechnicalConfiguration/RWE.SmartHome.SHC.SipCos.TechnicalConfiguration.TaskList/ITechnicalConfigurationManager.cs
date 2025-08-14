using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.SipCos.TechnicalConfiguration.TaskList;

public interface ITechnicalConfigurationManager
{
	void LoadAllFromPersistence();

	void LoadFromPersistence(IEnumerable<Guid> ids);

	void SetConfiguration(IEnumerable<Guid> devicesToRemove, IList<TechnicalConfigurationTask> allDeviceConfigurations);
}
