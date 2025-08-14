using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Ionic.Zip;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.ApplicationsHostInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.FileDownload;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.NetworkMonitoringInterfaces;

namespace RWE.SmartHome.SHC.ApplicationsHost;

public static class AppModuleHandler
{
	internal const string AppsPath = "\\NandFlash\\Apps";

	private const string AppIdPrefix = "sh://";

	internal static AppDownloadResult DownloadApplicationPackage(string packageUrl, string destinationFolder, string mainAssemblyName, byte[] checkSum, ApplicationTokenEntry entry, bool forceDownload, INetworkingMonitor networkMonitor, IAddinRepoManager addinRepo)
	{
		if (!forceDownload && File.Exists(Path.Combine(destinationFolder, mainAssemblyName)))
		{
			return AppDownloadResult.Success;
		}
		try
		{
			string text = Path.Combine(destinationFolder, Guid.NewGuid().ToString());
			if (networkMonitor.InternetAccessAllowed)
			{
				DownloadAddin(packageUrl, text);
				if (!File.Exists(text))
				{
					return AppDownloadResult.NetworkFailure;
				}
			}
			try
			{
				if (!IsAddinChecksumValid(checkSum, text))
				{
					return AppDownloadResult.InvalidData;
				}
				ExtractArchive(destinationFolder, text);
				return (!File.Exists(Path.Combine(destinationFolder, mainAssemblyName))) ? AppDownloadResult.NetworkFailure : AppDownloadResult.Success;
			}
			finally
			{
				addinRepo.AddOrUpdatePersistenceFile(entry.Name, entry.AppId, entry.Version, isService: false);
				File.Delete(text);
			}
		}
		catch (Exception arg)
		{
			Log.Warning(Module.ApplicationsHost, $"Failed to download package from URL {packageUrl}: {arg}");
			DeleteFileOrDirectory(new DirectoryInfo(destinationFolder));
		}
		return AppDownloadResult.NetworkFailure;
	}

	private static void DownloadAddin(string packageUrl, string localTempFileName)
	{
		FileDownloader fileDownloader = new FileDownloader();
		Uri url = new Uri(packageUrl);
		fileDownloader.DownloadInvalidResponse = delegate(string message)
		{
			Log.Error(Module.ApplicationsHost, $"Failed to download custom app from {packageUrl}: {message}");
		};
		fileDownloader.DownloadServerUnavailable = fileDownloader.DownloadInvalidResponse;
		fileDownloader.DownloadFile(url, localTempFileName, null, null);
	}

	private static bool IsAddinChecksumValid(byte[] checkSum, string localTempFileName)
	{
		using FileStream inputStream = new FileStream(localTempFileName, FileMode.Open, FileAccess.Read);
		using MD5 mD = MD5.Create();
		byte[] array = mD.ComputeHash(inputStream);
		if (!array.SequenceEqual(checkSum))
		{
			Log.Warning(Module.ApplicationsHost, $"Checksum mismatch. Manifest says {checkSum.ToReadable()} but the file has {array.ToReadable()}");
			return false;
		}
		return true;
	}

	private static void ExtractArchive(string destinationFolder, string localTempFileName)
	{
		using ZipFile zipFile = ZipFile.Read(localTempFileName);
		foreach (ZipEntry item in zipFile)
		{
			string path = Path.Combine(destinationFolder, item.FileName);
			if (File.Exists(path))
			{
				try
				{
					File.Delete(path);
				}
				catch (IOException)
				{
					continue;
				}
			}
			item.Extract(destinationFolder);
		}
	}

	public static void CleanupUnusedAppFolders(ApplicationsToken token)
	{
		if (!Directory.Exists("\\NandFlash\\Apps"))
		{
			return;
		}
		string[] directories = Directory.GetDirectories("\\NandFlash\\Apps");
		string[] array = directories;
		foreach (string text in array)
		{
			string dirName = text.Remove(0, "\\NandFlash\\Apps".Length);
			while (dirName.Length > 0 && dirName[0] == '\\')
			{
				dirName = dirName.Remove(0, 1);
			}
			if (!token.Entries.Any((ApplicationTokenEntry entry) => 0 == string.Compare(GetLocalFolderName(entry), dirName, ignoreCase: true)))
			{
				try
				{
					Log.Information(Module.ApplicationsHost, $"Directory {text} does not match any token app. Removing it.");
					DeleteFileOrDirectory(new DirectoryInfo(text));
				}
				catch (Exception ex)
				{
					Log.Error(Module.ApplicationsHost, $"Error while removing directory of {text}: {ex.Message}");
				}
			}
		}
	}

	private static void DeleteFileOrDirectory(FileSystemInfo fsi)
	{
		fsi.Attributes = FileAttributes.Normal;
		if (fsi is DirectoryInfo directoryInfo)
		{
			FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
			foreach (FileSystemInfo fsi2 in fileSystemInfos)
			{
				DeleteFileOrDirectory(fsi2);
			}
		}
		fsi.Delete();
	}

	internal static string GetLocalFolderName(ApplicationTokenEntry entry)
	{
		return entry.AppId.Remove(0, "sh://".Length) + entry.Version;
	}

	internal static bool DownloadManifestFile(ApplicationTokenEntry entry, string manifestFileName, string localAppFolder, INetworkingMonitor networkMonitor, IAddinRepoManager addinRepo)
	{
		_ = entry.AppId;
		if (!Directory.Exists(localAppFolder))
		{
			Directory.CreateDirectory(localAppFolder);
		}
		if (!File.Exists(manifestFileName) && networkMonitor.InternetAccessAllowed)
		{
			Uri uri = new Uri(GetManifestUrl(entry));
			try
			{
				FileDownloader fileDownloader = new FileDownloader();
				fileDownloader.DownloadFile(uri, manifestFileName, null, null);
			}
			catch (Exception arg)
			{
				Log.Error(Module.ApplicationsHost, $"Failed to download manifest from {uri.ToString()}: {arg}");
			}
		}
		return File.Exists(manifestFileName);
	}

	private static string GetManifestUrl(ApplicationTokenEntry entry)
	{
		return $"{entry.ApplicationUrl}/{entry.Version}/{entry.AppManifest}";
	}
}
