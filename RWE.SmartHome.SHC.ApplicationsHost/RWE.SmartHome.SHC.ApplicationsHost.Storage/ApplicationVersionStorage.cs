using System;
using System.Collections.Generic;
using System.IO;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.ApplicationsHost.Storage;

public class ApplicationVersionStorage
{
	private const string KeyValuePairSeparator = "=";

	private const string StorageFileName = "apps_version.txt";

	private const string StoragePath = "\\NandFlash\\Apps";

	private static readonly string StorageFilePath = Path.Combine("\\NandFlash\\Apps", "apps_version.txt");

	private readonly IDictionary<string, string> appVersions = new Dictionary<string, string>();

	private readonly object sync = new object();

	public void LoadAllVersions()
	{
		lock (sync)
		{
			List<string> fileLines = GetFileLines(StorageFilePath);
			PopulateAppVersionsFromFileLines(fileLines);
		}
	}

	public void SaveAllVersions()
	{
		lock (sync)
		{
			List<string> linesFromAppVersions = GetLinesFromAppVersions();
			WriteFile(StorageFilePath, linesFromAppVersions);
		}
	}

	public void ClearVersions()
	{
		lock (sync)
		{
			appVersions.Clear();
		}
	}

	public string GetAppVersion(string appId)
	{
		lock (sync)
		{
			string value = null;
			return appVersions.TryGetValue(appId, out value) ? value : null;
		}
	}

	public void SetAppVersion(string appId, string version)
	{
		lock (sync)
		{
			appVersions[appId] = version;
		}
	}

	private List<string> GetFileLines(string filePath)
	{
		List<string> list = new List<string>();
		try
		{
			if (File.Exists(filePath))
			{
				using StreamReader streamReader = new StreamReader(filePath);
				while (!streamReader.EndOfStream)
				{
					string item = streamReader.ReadLine();
					list.Add(item);
				}
				streamReader.Close();
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.ApplicationsHost, ex, "Failed to read Apps Versions from local file");
			CleanFile(filePath);
		}
		return list;
	}

	private void WriteFile(string filePath, List<string> lines)
	{
		try
		{
			CleanFile(filePath);
			if (lines == null)
			{
				return;
			}
			using StreamWriter streamWriter = new StreamWriter(filePath);
			foreach (string line in lines)
			{
				streamWriter.WriteLine(line);
			}
			streamWriter.Close();
		}
		catch (Exception ex)
		{
			Log.Exception(Module.ApplicationsHost, ex, "Failed to write Apps Versions to local file");
			CleanFile(filePath);
		}
	}

	private void CleanFile(string filePath)
	{
		if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
		{
			File.Delete(filePath);
		}
	}

	private void PopulateAppVersionsFromFileLines(List<string> lines)
	{
		try
		{
			appVersions.Clear();
			if (lines == null)
			{
				return;
			}
			foreach (string line in lines)
			{
				if (!string.IsNullOrEmpty(line))
				{
					int num = line.LastIndexOf("=");
					if (num > 0 && num < line.Length - 2)
					{
						string key = line.Substring(0, num);
						string value = line.Substring(num + 1);
						appVersions.Add(key, value);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.ApplicationsHost, ex, "Cannot populate Apps versions from the file");
		}
	}

	private List<string> GetLinesFromAppVersions()
	{
		List<string> list = new List<string>();
		try
		{
			foreach (KeyValuePair<string, string> appVersion in appVersions)
			{
				string item = string.Format("{0}{1}{2}", appVersion.Key, "=", appVersion.Value);
				list.Add(item);
			}
		}
		catch (Exception ex)
		{
			Log.Exception(Module.ApplicationsHost, ex, "Cannot obtain the lines from the Apps versions");
		}
		return list;
	}
}
