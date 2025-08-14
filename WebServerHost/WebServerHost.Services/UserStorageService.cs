using System.Collections.Generic;
using System.IO;
using System.Linq;
using SmartHome.Common.API.Entities.ClientData;
using SmartHome.Common.API.Entities.Serializers;

namespace WebServerHost.Services;

internal class UserStorageService : IUserStorageService
{
	private const string UserStorageFilePath = "NandFlash/SHC/userStorage.json";

	private object locker = new object();

	public List<UserData> Get()
	{
		return ReadFromFile();
	}

	private List<UserData> ReadFromFile()
	{
		List<UserData> result = new List<UserData>();
		if (File.Exists("NandFlash/SHC/userStorage.json"))
		{
			string json = string.Empty;
			lock (locker)
			{
				using StreamReader streamReader = File.OpenText("NandFlash/SHC/userStorage.json");
				json = streamReader.ReadToEnd();
			}
			try
			{
				result = ApiJsonSerializer.Deserialize<List<UserData>>(json);
			}
			catch
			{
				File.Delete("NandFlash/SHC/userStorage.json");
			}
		}
		return result;
	}

	public void Save(List<UserData> userData)
	{
		List<UserData> list = ReadFromFile();
		UserData entry;
		foreach (UserData userDatum in userData)
		{
			entry = userDatum;
			UserData userData2 = list.FirstOrDefault((UserData e) => e.Partition == entry.Partition && e.Key == entry.Key);
			if (userData2 != null)
			{
				list.Remove(userData2);
			}
			list.Add(entry);
		}
		WriteToFile(list);
	}

	private void WriteToFile(List<UserData> data)
	{
		lock (locker)
		{
			string value = ApiJsonSerializer.Serialize(data);
			using StreamWriter streamWriter = new StreamWriter("NandFlash/SHC/userStorage.json");
			streamWriter.Write(value);
			streamWriter.Flush();
			streamWriter.Close();
		}
	}

	public void Update(List<UserData> userData)
	{
		Save(userData);
	}

	public void Delete(string partition, string key)
	{
		List<UserData> list = ReadFromFile();
		list.RemoveAll((UserData d) => d.Partition == partition && d.Key == key);
		WriteToFile(list);
	}

	public void DeletaAll()
	{
		lock (locker)
		{
			if (File.Exists("NandFlash/SHC/userStorage.json"))
			{
				File.Delete("NandFlash/SHC/userStorage.json");
			}
		}
	}
}
