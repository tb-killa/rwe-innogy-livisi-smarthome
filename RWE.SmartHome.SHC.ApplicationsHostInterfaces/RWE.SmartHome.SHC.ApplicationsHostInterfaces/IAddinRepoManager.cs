using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

namespace RWE.SmartHome.SHC.ApplicationsHostInterfaces;

public interface IAddinRepoManager
{
	string AppIdPrefix { get; }

	List<ApplicationTokenEntry> GetAllAddinsFromPersistenceFile();

	ApplicationTokenEntry GetAddinTokenEntry(LocalAddin localAddin);

	LocalAddin GetAddinFormUsbToTemporatyLocation(string type);

	void InstallLocalAddin(LocalAddin addinTemporaryFilePath, bool deleteZipFromTemporarySpace);

	void RemoveAddinFromPersistenceFile(ApplicationTokenEntry addin);

	void AddOrUpdatePersistenceFile(string name, string appId, string version, bool isService);

	List<AddinUpdateData> GetAddinsToRemove(ApplicationsToken appsToken);

	ApplicationTokenEntry GetUpdatedAddin(AddinUpdateData addinToUpdate);

	void InstallCoreDeliveredAddinsIfExists(ApplicationsToken installedAddins);
}
