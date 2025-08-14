using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceFirmware;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.ErrorHandling;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceFirmwareUpdate;

public class DeviceFirmwareRepository : IDeviceFirmwareRepository
{
	private const string LoggingSource = "DeviceFirmwareRepository";

	private const string DeviceFirmwareLocation = "\\NandFlash\\Shc\\DeviceFirmware";

	private const string TempDownloadFileName = "temp.bin";

	private const string DownloadFileName = "update.bin";

	private readonly object syncLock = new object();

	private DownloadRequest? currentDownload;

	private readonly Queue<DownloadRequest> downloadsQueue = new Queue<DownloadRequest>();

	private readonly IFileDownloader fileDownloader;

	public event EventHandler<FirmwareDownloadFinishedEventArgs> FirmwareDownloadFinished;

	public DeviceFirmwareRepository(IFileDownloader fileDownloaderInstance)
	{
		fileDownloader = fileDownloaderInstance;
		SetupFileDownloader();
		if (!Directory.Exists("\\NandFlash\\Shc\\DeviceFirmware"))
		{
			Directory.CreateDirectory("\\NandFlash\\Shc\\DeviceFirmware");
		}
		File.Delete(GetTemporaryFirmwareFile());
	}

	public void DownloadFirmware(DeviceDescriptor deviceDescriptor, string url, string md5Hash, string targetFirmwareVersion)
	{
		if (deviceDescriptor == null)
		{
			throw new ArgumentNullException("deviceDescriptor");
		}
		if (string.IsNullOrEmpty(url))
		{
			throw new ArgumentNullException("url");
		}
		if (string.IsNullOrEmpty(md5Hash))
		{
			throw new ArgumentNullException("md5Hash");
		}
		Log.Debug(Module.BusinessLogic, "DeviceFirmwareRepository", "Scheduling FW download from:" + url);
		lock (syncLock)
		{
			downloadsQueue.Enqueue(new DownloadRequest
			{
				DeviceDescriptor = deviceDescriptor,
				Url = CreateDownloadUrl(url),
				MD5Hash = md5Hash,
				TargetFile = Path.Combine(GetFirmwarePath(deviceDescriptor), Path.GetFileNameWithoutExtension(GetFirmwareLocation(deviceDescriptor)) + "_" + targetFirmwareVersion + ".bin")
			});
		}
		ProcessNextItem();
	}

	public DeviceFirmwareDescriptor GetFirmware(DeviceDescriptor deviceDescriptor)
	{
		string directoryName = Path.GetDirectoryName(GetFirmwareLocation(deviceDescriptor));
		if (Directory.Exists(directoryName))
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
			FileInfo[] files = directoryInfo.GetFiles();
			if (files.Length > 0)
			{
				string name = files[0].Name;
				DeviceFirmwareDescriptor deviceFirmwareDescriptor = new DeviceFirmwareDescriptor();
				deviceFirmwareDescriptor.ImageFile = Path.Combine(GetFirmwarePath(deviceDescriptor), name);
				deviceFirmwareDescriptor.CurrentVersion = deviceDescriptor.FirmwareVersion;
				deviceFirmwareDescriptor.TargetVersion = GetTargetFirmwareVersion(Path.GetFileNameWithoutExtension(Path.GetFullPath(name)));
				return deviceFirmwareDescriptor;
			}
		}
		return null;
	}

	public List<DeviceDescriptor> GetDownloadedFirmwareDescriptors()
	{
		List<DeviceDescriptor> list = new List<DeviceDescriptor>();
		DirectoryInfo directoryInfo = new DirectoryInfo("\\NandFlash\\Shc\\DeviceFirmware");
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		foreach (DirectoryInfo dir in directories)
		{
			list.AddRange(GetFirmwareList(dir));
		}
		return list;
	}

	public void DeleteFirmware(DeviceDescriptor deviceDescriptor)
	{
		string firmwarePath = GetFirmwarePath(deviceDescriptor);
		if (Directory.Exists(firmwarePath))
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(firmwarePath);
			DirectoryInfo parent = directoryInfo.Parent;
			if (parent.GetDirectories().Length < 2 && parent.GetFiles().Length < 2)
			{
				Directory.Delete(parent.FullName, recursive: true);
			}
			else
			{
				Directory.Delete(firmwarePath, recursive: true);
			}
		}
	}

	private void ProcessNextItem()
	{
		DownloadRequest? downloadRequest = null;
		lock (syncLock)
		{
			if (!currentDownload.HasValue && downloadsQueue.Count > 0)
			{
				currentDownload = downloadsQueue.Dequeue();
				downloadRequest = currentDownload;
			}
		}
		if (downloadRequest.HasValue)
		{
			StartDownload();
		}
	}

	private void StartDownload()
	{
		DownloadRequest currentDownloadRequest = GetCurrentDownloadRequest();
		Log.Information(Module.BusinessLogic, "DeviceFirmwareRepository", "Downloading FW from: " + currentDownloadRequest.Url);
		try
		{
			if (File.Exists(currentDownloadRequest.TargetFile))
			{
				Log.Debug(Module.BusinessLogic, $"A device firmware download at the specified URL {currentDownloadRequest.Url} has already started");
				CompleteDownload();
			}
			else
			{
				fileDownloader.DownloadFile(currentDownloadRequest.Url, GetTemporaryFirmwareFile(), string.Empty, string.Empty);
			}
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, $"Could not download device firmware from URL {currentDownloadRequest.Url}. Error: {ex.Message}");
			CompleteDownload();
		}
	}

	private void SetupFileDownloader()
	{
		fileDownloader.DownloadCompleted = delegate
		{
			DownloadRequest currentDownloadRequest = GetCurrentDownloadRequest();
			try
			{
				HandleDownloadedFile();
			}
			catch (Exception ex)
			{
				Log.Debug(Module.BusinessLogic, $"Could not download device firmware from URL {currentDownloadRequest.Url}. Error: {ex.Message}");
			}
			finally
			{
				CompleteDownload();
			}
		};
		fileDownloader.DownloadInvalidResponse = delegate(string x)
		{
			Log.Debug(Module.BusinessLogic, $"Could not download device firmware from URL {GetCurrentDownloadRequest().Url} due to invalid response. Error message: {x}.");
			CompleteDownload();
		};
		fileDownloader.DownloadServerUnavailable = delegate(string x)
		{
			Log.Debug(Module.BusinessLogic, $"Could not download device firmware from URL {GetCurrentDownloadRequest().Url}, server unavailable. Error message: {x}.");
			CompleteDownload();
		};
	}

	private void HandleDownloadedFile()
	{
		DownloadRequest currentDownloadRequest = GetCurrentDownloadRequest();
		if (CheckFileMd5Hash(GetTemporaryFirmwareFile(), currentDownloadRequest.MD5Hash))
		{
			Directory.CreateDirectory(GetFirmwarePath(currentDownloadRequest.DeviceDescriptor));
			File.Move(GetTemporaryFirmwareFile(), currentDownloadRequest.TargetFile);
			this.FirmwareDownloadFinished?.Invoke(this, new FirmwareDownloadFinishedEventArgs
			{
				DeviceInfo = currentDownloadRequest.DeviceDescriptor,
				Firmware = new DeviceFirmwareDescriptor
				{
					ImageFile = null,
					TargetVersion = currentDownloadRequest.DeviceDescriptor.FirmwareVersion
				}
			});
		}
		else
		{
			Log.Debug(Module.BusinessLogic, $"Checksum error for firmware file downloaded from {currentDownloadRequest.Url}.");
		}
	}

	private void CompleteDownload()
	{
		currentDownload = null;
		string text = string.Empty;
		try
		{
			text = GetTemporaryFirmwareFile();
			File.Delete(text);
		}
		catch (Exception ex)
		{
			Log.Debug(Module.BusinessLogic, $"Unable to remove temporary file {text} : {ex.Message}");
		}
		ProcessNextItem();
	}

	private DownloadRequest GetCurrentDownloadRequest()
	{
		if (currentDownload.HasValue)
		{
			return currentDownload.Value;
		}
		throw new ShcException("Expected current download not found", Module.BusinessLogic.ToString(), 0);
	}

	private IEnumerable<DeviceDescriptor> GetFirmwareList(DirectoryInfo dir)
	{
		string fileName = GetUpdateFileInDir(dir.FullName);
		if (!string.IsNullOrEmpty(fileName))
		{
			yield return new DeviceDescriptor
			{
				Manufacturer = Convert.ToInt16(GetManufacturer(dir.Name)),
				ProductId = Convert.ToUInt32(GetProductId(dir.Name)),
				FirmwareVersion = GetFirmwareVersion(dir.Name),
				HardwareVersion = null
			};
		}
		try
		{
			DirectoryInfo[] directories = dir.GetDirectories();
			foreach (DirectoryInfo subDir in directories)
			{
				fileName = GetUpdateFileInDir(subDir.FullName);
				if (!string.IsNullOrEmpty(fileName))
				{
					yield return new DeviceDescriptor
					{
						Manufacturer = Convert.ToInt16(GetManufacturer(dir.Name)),
						ProductId = Convert.ToUInt32(GetProductId(dir.Name)),
						FirmwareVersion = GetFirmwareVersion(dir.Name),
						HardwareVersion = subDir.Name
					};
				}
			}
		}
		finally
		{
		}
	}

	private string GetUpdateFileInDir(string dir)
	{
		string[] files = Directory.GetFiles(dir, "update*.bin");
		if (files.Length > 0)
		{
			return files[0];
		}
		return null;
	}

	private Uri CreateDownloadUrl(string url)
	{
		if (url == null)
		{
			throw new ArgumentNullException("DownloadLocation");
		}
		return new Uri(url);
	}

	private string GetFirmwareLocation(DeviceDescriptor deviceDescriptor)
	{
		return GetFirmwarePath(deviceDescriptor) + "\\update.bin";
	}

	private string GetTemporaryFirmwareFile()
	{
		return Path.Combine("\\NandFlash\\Shc\\DeviceFirmware", "temp.bin");
	}

	private string GetFirmwarePath(DeviceDescriptor deviceDescriptor)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("{0}\\{1}.{2}.{3}", "\\NandFlash\\Shc\\DeviceFirmware", deviceDescriptor.Manufacturer, deviceDescriptor.ProductId, deviceDescriptor.FirmwareVersion);
		if (!string.IsNullOrEmpty(deviceDescriptor.HardwareVersion))
		{
			stringBuilder.Append("\\" + deviceDescriptor.HardwareVersion);
		}
		return stringBuilder.ToString();
	}

	private string GetManufacturer(string directoryName)
	{
		int num = directoryName.IndexOf('.');
		if (num != -1)
		{
			return directoryName.Substring(0, num);
		}
		return string.Empty;
	}

	private string GetTargetFirmwareVersion(string fileName)
	{
		int num = fileName.IndexOf('_');
		int num2 = fileName.IndexOf('.');
		if (num != -1 && num2 != -1)
		{
			return fileName.Substring(num + 1, fileName.Length - num - 1);
		}
		return string.Empty;
	}

	private string GetProductId(string directoryName)
	{
		string[] array = directoryName.Split('.');
		int num = directoryName.IndexOf('.');
		if (num != -1 && array.Length > 2)
		{
			return directoryName.Substring(num + 1, array[1].Length);
		}
		return string.Empty;
	}

	private string GetFirmwareVersion(string directoryName)
	{
		int num = directoryName.IndexOf('.');
		int num2 = directoryName.IndexOf('.', num + 1);
		if (num2 == -1)
		{
			return string.Empty;
		}
		return directoryName.Substring(num2 + 1, directoryName.Length - num2 - 1);
	}

	private bool CheckFileMd5Hash(string file, string expectedHash)
	{
		return GetFileMd5(file).Equals(expectedHash, StringComparison.InvariantCultureIgnoreCase);
	}

	private string GetFileMd5(string file)
	{
		using MD5 mD = MD5.Create();
		using FileStream inputStream = File.OpenRead(file);
		byte[] me = mD.ComputeHash(inputStream);
		return me.ToReadable();
	}
}
