using System.Collections.Generic;

namespace SmartHome.SHC.API.Storage;

public interface IIsolatedStorage
{
	string GetValue(string name);

	void SetValue(string name, string value);

	IDictionary<string, string> GetAllSettings();

	IList<string> GetAllNames();

	bool Delete(string name);

	void DeleteAllSettings();
}
