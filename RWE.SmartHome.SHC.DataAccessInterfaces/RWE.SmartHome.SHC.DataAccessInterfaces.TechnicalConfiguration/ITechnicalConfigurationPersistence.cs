using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Database;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.TechnicalConfiguration;

public interface ITechnicalConfigurationPersistence : IService
{
	IEnumerable<TechnicalConfigurationEntity> LoadAll();

	void SaveAll(IEnumerable<TechnicalConfigurationEntity> technicalConfigurations);

	void Save(TechnicalConfigurationEntity technicalConfiguration);

	TechnicalConfigurationEntity Load(Guid id);

	void Delete(Guid id);

	DatabaseConnection OpenDatabaseTransaction();
}
