using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;

public interface ICoreIntegrityHandler
{
	void Handle(ConfigEventType eventType, Guid entityId);

	List<ConfigEventType> GetHandledEvents();
}
