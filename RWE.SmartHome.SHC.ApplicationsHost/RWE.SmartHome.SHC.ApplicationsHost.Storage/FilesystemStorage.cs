using System.Collections.Generic;
using System.IO;
using System.Linq;
using SmartHome.SHC.API.Storage;

namespace RWE.SmartHome.SHC.ApplicationsHost.Storage;

public class FilesystemStorage : IFilesystemStorage
{
	private const string addinFSStorage = "\\Nandflash\\AddinStorage";

	private readonly string storagePath;

	public FilesystemStorage(string applicationId)
	{
		storagePath = GetAddInPath(applicationId);
	}

	private static string GetAddInPath(string applicationId)
	{
		return string.Format("{0}\\{1}", "\\Nandflash\\AddinStorage", applicationId.Replace("sh://", string.Empty));
	}

	public string GetFileSystemStoragePath()
	{
		if (!Directory.Exists(storagePath))
		{
			Directory.CreateDirectory(storagePath);
		}
		return storagePath;
	}

	internal void Uninstall()
	{
		if (Directory.Exists(storagePath))
		{
			Directory.Delete(storagePath, recursive: true);
		}
	}

	internal static void KeepOnlyFoldersFor(List<string> existingAppIds)
	{
		if (Directory.Exists("\\Nandflash\\AddinStorage"))
		{
			IEnumerable<string> second = existingAppIds.Select((string s) => GetAddInPath(s));
			List<string> list = Directory.GetDirectories("\\Nandflash\\AddinStorage").Except(second).ToList();
			list.ForEach(delegate(string dir)
			{
				Directory.Delete(dir, recursive: true);
			});
		}
	}
}
