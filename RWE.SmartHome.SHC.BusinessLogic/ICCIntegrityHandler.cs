using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.IntegrityManagement;

public interface ICCIntegrityHandler
{
	void Handle(ConfigEventType eventType, Guid entityId);
}
