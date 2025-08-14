using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.LocalDeviceKeys;

public class DeviceKeyRepository : IDeviceKeyRepository
{
	private readonly IDeviceMasterKeyRepository deviceMasterKeyRepository;

	private readonly Configuration configuration;

	private readonly int MaximumNumberOfDevicesToDownloadOnce;

	private byte[] MasterKey;

	private readonly object sync = new object();

	public DeviceKeyRepository(IDeviceMasterKeyRepository deviceMasterKeyRepository, IConfigurationManager configurationManager)
	{
		this.deviceMasterKeyRepository = deviceMasterKeyRepository;
		configuration = new Configuration(configurationManager);
		if (configuration.MaximumNumberOfDevicesToDownloadOnce.HasValue)
		{
			MaximumNumberOfDevicesToDownloadOnce = configuration.MaximumNumberOfDevicesToDownloadOnce.Value;
		}
		else
		{
			MaximumNumberOfDevicesToDownloadOnce = 50;
		}
	}

	public List<StoredDevice> GetAllDevicesKeysFromStorage()
	{
		return GetAllDevicesKeysFromFile("\\NandFlash\\DevicesKeysStorage.csv");
	}

	public List<StoredDevice> GetAllDevicesKeysFromFile(string filePath)
	{
		if (!File.Exists(filePath))
		{
			return new List<StoredDevice>();
		}
		lock (sync)
		{
			return GetAllKeysFromFile(filePath);
		}
	}

	public byte[] GetDeviceKeyFromCsvStorage(SGTIN96 deviceSgtin)
	{
		List<StoredDevice> allDevicesKeysFromStorage = GetAllDevicesKeysFromStorage();
		foreach (StoredDevice item in allDevicesKeysFromStorage)
		{
			if (SGTIN96.Create(item.Sgtin).Equals(deviceSgtin))
			{
				Log.Debug(Module.DeviceManager, $"Found the key in the local CSV storage for the device with the serial number: {item.SerialNumber}");
				return item.Key;
			}
		}
		Log.Debug(Module.DeviceManager, $"Could not find the key in the local CSV storage for the device with the serial number: {deviceSgtin.ToString()}");
		return null;
	}

	public void StoreKeys(ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary[] devicesKeys)
	{
		bool isFileAlreadyCreated = File.Exists("\\NandFlash\\DevicesKeysStorage.csv");
		lock (sync)
		{
			using FileStream stream = new FileStream("\\NandFlash\\DevicesKeysStorage.csv", FileMode.Append, FileAccess.Write, FileShare.Write);
			using StreamWriter streamWriter = new StreamWriter(stream);
			AddHeadersIfNeeded(isFileAlreadyCreated, streamWriter);
			foreach (ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary deviceKey in devicesKeys)
			{
				try
				{
					WriteDeviceKeyToCsv(streamWriter, deviceKey);
				}
				catch (Exception ex)
				{
					Log.Error(Module.BusinessLogic, $"There was a problem writing the device keys to CSV. {ex.Message} {ex.StackTrace}");
				}
			}
		}
	}

	public void StoreDeviceKey(ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary deviceKey)
	{
		bool isFileAlreadyCreated = File.Exists("\\NandFlash\\DevicesKeysStorage.csv");
		lock (sync)
		{
			using FileStream stream = new FileStream("\\NandFlash\\DevicesKeysStorage.csv", FileMode.Append, FileAccess.Write, FileShare.Write);
			using StreamWriter streamWriter = new StreamWriter(stream);
			AddHeadersIfNeeded(isFileAlreadyCreated, streamWriter);
			WriteDeviceKeyToCsv(streamWriter, deviceKey);
		}
	}

	public bool DeviceExistsInStorage(byte[] sgtin)
	{
		List<StoredDevice> allDevicesKeysFromStorage = GetAllDevicesKeysFromStorage();
		StoredDevice storedDevice = allDevicesKeysFromStorage.FirstOrDefault((StoredDevice storedDevice2) => storedDevice2.Sgtin.SequenceEqual(sgtin));
		if (storedDevice == null)
		{
			return false;
		}
		return true;
	}

	public List<List<byte[]>> SplitDevices(List<byte[]> devices)
	{
		List<List<byte[]>> list = new List<List<byte[]>>();
		List<byte[]> list2 = new List<byte[]>(devices);
		if (devices.Count <= MaximumNumberOfDevicesToDownloadOnce)
		{
			list.Add(list2);
			return list;
		}
		while (list2.Count > 0)
		{
			int count = Math.Min(MaximumNumberOfDevicesToDownloadOnce, list2.Count);
			List<byte[]> range = list2.GetRange(0, count);
			list.Add(range);
			list2.RemoveRange(0, count);
		}
		return list;
	}

	public int GetDeviceKeysCount()
	{
		return GetAllDevicesKeysFromStorage().Count;
	}

	public void ImportDevicesKeys(List<StoredDevice> devicesKeys)
	{
		bool isFileAlreadyCreated = File.Exists("\\NandFlash\\DevicesKeysStorage.csv");
		lock (sync)
		{
			using FileStream stream = new FileStream("\\NandFlash\\DevicesKeysStorage.csv", FileMode.Append, FileAccess.Write, FileShare.Write);
			using StreamWriter streamWriter = new StreamWriter(stream);
			AddHeadersIfNeeded(isFileAlreadyCreated, streamWriter);
			foreach (StoredDevice devicesKey in devicesKeys)
			{
				try
				{
					string arg = Convert.ToBase64String(devicesKey.Sgtin, 0, devicesKey.Sgtin.Length);
					string serialNumber = devicesKey.SerialNumber;
					byte[] array = EncryptDeviceKey(devicesKey.Key);
					string arg2 = Convert.ToBase64String(array, 0, array.Length);
					string value = $"{arg},{serialNumber},{arg2}";
					streamWriter.WriteLine(value);
				}
				catch (Exception ex)
				{
					Log.Error(Module.BusinessLogic, $"Could not import the key for the device with serial number {devicesKey.SerialNumber}. {ex.Message} {ex.StackTrace}");
				}
			}
		}
	}

	private List<StoredDevice> GetAllKeysFromFile(string filePath)
	{
		List<StoredDevice> list = new List<StoredDevice>();
		using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
		using StreamReader streamReader = new StreamReader(stream);
		streamReader.ReadLine();
		while (!streamReader.EndOfStream)
		{
			try
			{
				string text = streamReader.ReadLine();
				string[] array = text.Split(',');
				if (!IsDeviceKeyValidBase64(array[2]))
				{
					Log.Information(Module.BusinessLogic, $"The device keys with the serial number {array[1]} and SGTIN {array[0]} is not BASE64 encoded");
					continue;
				}
				byte[] key = DecryptDeviceKey(Convert.FromBase64String(array[2]));
				StoredDevice storedDevice = new StoredDevice();
				storedDevice.Sgtin = Convert.FromBase64String(array[0]);
				storedDevice.SerialNumber = array[1];
				storedDevice.Key = key;
				StoredDevice item = storedDevice;
				list.Add(item);
			}
			catch (Exception ex)
			{
				Log.Information(Module.DeviceManager, $"There was an exception getting the device from the CSV file. {ex.StackTrace}");
			}
		}
		return list;
	}

	private void AddHeadersIfNeeded(bool isFileAlreadyCreated, StreamWriter streamWriter)
	{
		if (!isFileAlreadyCreated)
		{
			string value = string.Format("{0},{1},{2}", "SGTIN", "SerialNo", "Key");
			streamWriter.WriteLine(value);
		}
	}

	private bool IsDeviceKeyValidBase64(string encryptedKey)
	{
		try
		{
			Convert.FromBase64String(encryptedKey);
			return true;
		}
		catch (FormatException)
		{
			return false;
		}
	}

	private byte[] DecryptDeviceKey(byte[] encryptedKey)
	{
		RijndaelManaged aes = GetAes();
		byte[] rgbKey = ResizeMasterKey();
		byte[] result;
		using (ICryptoTransform cryptoTransform = aes.CreateDecryptor(rgbKey, null))
		{
			result = cryptoTransform.TransformFinalBlock(encryptedKey, 0, encryptedKey.Length);
		}
		if (encryptedKey == null)
		{
			throw new Exception("Cannot decrypt the device key");
		}
		return result;
	}

	private byte[] EncryptDeviceKey(byte[] deviceKey)
	{
		RijndaelManaged aes = GetAes();
		byte[] rgbKey = ResizeMasterKey();
		byte[] array;
		using (ICryptoTransform cryptoTransform = aes.CreateEncryptor(rgbKey, null))
		{
			array = cryptoTransform.TransformFinalBlock(deviceKey, 0, deviceKey.Length);
		}
		if (array == null)
		{
			throw new Exception("Cannot encrypt the device key");
		}
		return array;
	}

	private void WriteDeviceKeyToCsv(StreamWriter streamWriter, ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary deviceKey)
	{
		string arg = Convert.ToBase64String(deviceKey.SGTIN, 0, deviceKey.SGTIN.Length);
		string arg2 = SerialForDisplay.FromSgtin(deviceKey.SGTIN);
		byte[] array = EncryptDeviceKey(deviceKey.Key);
		string arg3 = Convert.ToBase64String(array, 0, array.Length);
		string value = $"{arg},{arg2},{arg3}";
		streamWriter.WriteLine(value);
	}

	private RijndaelManaged GetAes()
	{
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Mode = CipherMode.ECB;
		rijndaelManaged.Padding = PaddingMode.PKCS7;
		rijndaelManaged.KeySize = 256;
		return rijndaelManaged;
	}

	private byte[] ResizeMasterKey()
	{
		if (MasterKey == null)
		{
			MasterKey = deviceMasterKeyRepository.GetMasterKeyFromFile();
		}
		byte[] array = MasterKey;
		if (array == null)
		{
			throw new Exception("Cannot find the master key, it is null");
		}
		Array.Resize(ref array, 32);
		return array;
	}
}
