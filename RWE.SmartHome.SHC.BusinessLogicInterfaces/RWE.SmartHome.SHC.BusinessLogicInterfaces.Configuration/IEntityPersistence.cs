using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;

public interface IEntityPersistence
{
	IEnumerable<T> Load<T>() where T : Entity;

	StringCollection LoadSerialized<T>() where T : Entity;

	StringCollection GetEmptyList<T>() where T : Entity;

	void Save<T>(IEnumerable<T> entities) where T : Entity;

	void Delete<T>(IEnumerable<T> entities) where T : Entity;

	void Begin();

	void Commit();

	void Rollback();
}
