using System.Collections.Generic;
using SmartHome.Common.API.Entities.ClientData;

namespace WebServerHost;

public interface IUserStorageService
{
	List<UserData> Get();

	void Update(List<UserData> userData);

	void Save(List<UserData> userData);

	void Delete(string partition, string key);

	void DeletaAll();
}
