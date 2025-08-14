using System;
using System.Runtime.InteropServices;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.CommonFunctionality.P2PMessageQueue;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.BusinessLogic.DeviceMonitoring;

public sealed class USBDeviceMonitor : IDisposable, IUSBDeviceMonitor
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct DEVDETAIL
	{
		public Guid guidDevClass;

		public uint dwReserved;

		public int fAttached;

		public int cbName;

		public uint szName;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct STORAGEDEVICEINFO
	{
		public uint cbSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string szProfile;

		public uint dwDeviceClass;

		public uint dwDeviceType;

		public uint dwDeviceFlags;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct FILETIME
	{
		private uint dwLowDateTime;

		private uint dwHighDateTime;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct STOREINFO
	{
		public int cbSize;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
		public string szDeviceName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string szStoreName;

		public uint dwDeviceClass;

		public uint dwDeviceType;

		public STORAGEDEVICEINFO sdi;

		public uint dwDeviceFlags;

		public ulong snNumSectors;

		public uint dwBytesPerSector;

		public ulong snFreeSectors;

		public ulong snBiggestPartCreatable;

		public FILETIME ftCreated;

		public FILETIME ftLastModified;

		public uint dwAttributes;

		public uint dwPartitionCount;

		public uint dwMountCount;
	}

	private const int DEVICENAMESIZE = 8;

	private const int STORENAMESIZE = 32;

	private const int PROFILENAMESIZE = 32;

	private IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

	private static Guid STORE_MOUNT_GUID = new Guid("{C1115848-46FD-4976-BDE9-D79448457004}");

	private static Guid GENERIC_USB_SERIAL_DEVICE_GUID = new Guid("{f8a6ba98-087a-43ac-a9d8-b7f13c5bae31}");

	private static Guid BLOCK_DEVICE_GUID = new Guid("{A4E7EDDA-E575-4252-9D6B-4195D48BB865}");

	private IEventManager eventManager;

	private bool activeMonitoring;

	private IntPtr deviceNotificationHandle;

	private IntPtr usbDeviceNotificationHandle;

	private bool usbDeviceMonitoring;

	[DllImport("coredll.dll", SetLastError = true)]
	private static extern IntPtr RequestDeviceNotifications(byte[] devclass, IntPtr hMsgQ, bool fAll);

	[DllImport("coredll.dll", SetLastError = true)]
	private static extern bool StopDeviceNotifications(IntPtr h);

	[DllImport("coredll.dll", SetLastError = true)]
	private static extern IntPtr OpenStore(string szDeviceName);

	[DllImport("Coredll.dll", SetLastError = true)]
	private static extern bool GetStoreInfo(IntPtr hStore, IntPtr pStoreInfo);

	[DllImport("coredll.dll")]
	private static extern uint GetLastError();

	public USBDeviceMonitor(Container container)
	{
		eventManager = container.Resolve<IEventManager>();
		StartDriveStatusMonitoring();
		StartUsbDeviceStatusMonitoring();
	}

	public void Dispose()
	{
		EndDriveStatusMonitoring();
		EndUsbDeviceStatusMonitoring();
		GC.SuppressFinalize(this);
	}

	private void StartUsbDeviceStatusMonitoring()
	{
		if (!usbDeviceMonitoring)
		{
			P2PMessageQueue p2PMessageQueue = new P2PMessageQueue(forReading: true);
			p2PMessageQueue.DataOnQueueChanged += OnUsbDeviceConfigurationChanged;
			usbDeviceNotificationHandle = RequestDeviceNotifications(GENERIC_USB_SERIAL_DEVICE_GUID.ToByteArray(), p2PMessageQueue.Handle, fAll: false);
			if (usbDeviceNotificationHandle != INVALID_HANDLE_VALUE)
			{
				usbDeviceMonitoring = true;
				Log.Information(Module.BusinessLogic, "Generic USB device monitoring successfully started!");
			}
			else
			{
				Log.Error(Module.BusinessLogic, $"Generic USB device monitoring failed to start! Last SYS error: {GetLastError()}");
			}
		}
	}

	private void EndUsbDeviceStatusMonitoring()
	{
		if (usbDeviceMonitoring)
		{
			if (StopDeviceNotifications(usbDeviceNotificationHandle))
			{
				Log.Information(Module.BusinessLogic, "Generic USB device monitoring successfully stopped.");
				usbDeviceMonitoring = false;
			}
			else
			{
				Log.Error(Module.BusinessLogic, "Failed to stop Generic USB device monitoring!");
			}
		}
	}

	private void OnUsbDeviceConfigurationChanged(object sender, EventArgs args)
	{
		if (!(sender is P2PMessageQueue p2PMessageQueue))
		{
			return;
		}
		Message message = new Message();
		try
		{
			ReadWriteResult readWriteResult;
			do
			{
				readWriteResult = p2PMessageQueue.Receive(message);
				IntPtr intPtr = Marshal.AllocHGlobal(message.MessageBytes.Length);
				if (intPtr != INVALID_HANDLE_VALUE)
				{
					Marshal.Copy(message.MessageBytes, 0, intPtr, message.MessageBytes.Length);
					DEVDETAIL dEVDETAIL = (DEVDETAIL)Marshal.PtrToStructure(intPtr, typeof(DEVDETAIL));
					IntPtr intPtr2 = Marshal.AllocHGlobal(dEVDETAIL.cbName);
					Marshal.Copy(message.MessageBytes, Marshal.SizeOf(typeof(DEVDETAIL)) - Marshal.SizeOf((object)dEVDETAIL.szName), intPtr2, dEVDETAIL.cbName);
					string text = Marshal.PtrToStringUni(intPtr2);
					Marshal.FreeHGlobal(intPtr2);
					Marshal.FreeHGlobal(intPtr);
					if (dEVDETAIL.fAttached == 1)
					{
						Log.Information(Module.BusinessLogic, $"Generic USB serial device {text} plugged!");
						eventManager.GetEvent<USBDeviceNotificationEvent>().Publish(new USBDeviceNotificationEventArgs
						{
							Attached = true,
							DeviceName = text
						});
					}
					else
					{
						eventManager.GetEvent<USBDeviceNotificationEvent>().Publish(new USBDeviceNotificationEventArgs
						{
							Attached = false,
							DeviceName = text
						});
						Log.Information(Module.BusinessLogic, $"Generic USB serial device {text} unplugged...");
					}
				}
			}
			while (readWriteResult == ReadWriteResult.OK);
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, $"Error occured while handling UsbDeviceConfigurationChanged: {ex.ToString()}");
		}
	}

	private void StartDriveStatusMonitoring()
	{
		if (!activeMonitoring)
		{
			P2PMessageQueue p2PMessageQueue = new P2PMessageQueue(forReading: true);
			p2PMessageQueue.DataOnQueueChanged += OnDriveConfigurationChanged;
			deviceNotificationHandle = RequestDeviceNotifications(STORE_MOUNT_GUID.ToByteArray(), p2PMessageQueue.Handle, fAll: false);
			if (deviceNotificationHandle != INVALID_HANDLE_VALUE)
			{
				activeMonitoring = true;
				Log.Information(Module.BusinessLogic, "USB Storage monitoring successfully started");
			}
			else
			{
				Log.Error(Module.BusinessLogic, $"Failed to start USB Storage monitoring! Last SYS error; {GetLastError()}");
			}
		}
	}

	private void EndDriveStatusMonitoring()
	{
		if (activeMonitoring)
		{
			if (StopDeviceNotifications(deviceNotificationHandle))
			{
				Log.Information(Module.BusinessLogic, "USB Storage monitoring successfully stopped.");
				activeMonitoring = false;
			}
			else
			{
				Log.Error(Module.BusinessLogic, "Failed to stop USB Storage monitoring!");
			}
		}
	}

	private void OnDriveConfigurationChanged(object sender, EventArgs args)
	{
		if (!(sender is P2PMessageQueue p2PMessageQueue))
		{
			return;
		}
		Message message = new Message();
		try
		{
			ReadWriteResult readWriteResult;
			do
			{
				readWriteResult = p2PMessageQueue.Receive(message);
				IntPtr intPtr = Marshal.AllocHGlobal(message.MessageBytes.Length);
				if (!(intPtr != INVALID_HANDLE_VALUE))
				{
					continue;
				}
				Marshal.Copy(message.MessageBytes, 0, intPtr, message.MessageBytes.Length);
				DEVDETAIL dEVDETAIL = (DEVDETAIL)Marshal.PtrToStructure(intPtr, typeof(DEVDETAIL));
				IntPtr intPtr2 = Marshal.AllocHGlobal(dEVDETAIL.cbName);
				Marshal.Copy(message.MessageBytes, Marshal.SizeOf(typeof(DEVDETAIL)) - Marshal.SizeOf((object)dEVDETAIL.szName), intPtr2, dEVDETAIL.cbName);
				string text = Marshal.PtrToStringUni(intPtr2);
				Marshal.FreeHGlobal(intPtr2);
				Marshal.FreeHGlobal(intPtr);
				bool mounted = false;
				if (dEVDETAIL.fAttached == 1)
				{
					Log.Information(Module.BusinessLogic, $"USB device {text} plugged!");
					IntPtr zero = IntPtr.Zero;
					zero = OpenStore(text);
					STOREINFO sTOREINFO = new STOREINFO
					{
						cbSize = Marshal.SizeOf(typeof(STOREINFO))
					};
					IntPtr intPtr3 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(STOREINFO)));
					Marshal.StructureToPtr((object)sTOREINFO, intPtr3, fDeleteOld: true);
					if (GetStoreInfo(zero, intPtr3) && ((STOREINFO)Marshal.PtrToStructure(intPtr3, typeof(STOREINFO))).dwMountCount != 0)
					{
						mounted = true;
						Log.Information(Module.BusinessLogic, $"Disk {text} mounted successfully!");
					}
					Marshal.FreeHGlobal(intPtr3);
					eventManager.GetEvent<USBDriveNotificationEvent>().Publish(new USBDriveNotificationEventArgs
					{
						Attached = true,
						DeviceName = text,
						Mounted = mounted
					});
				}
				else
				{
					eventManager.GetEvent<USBDriveNotificationEvent>().Publish(new USBDriveNotificationEventArgs
					{
						Attached = false,
						DeviceName = text,
						Mounted = mounted
					});
					Log.Information(Module.BusinessLogic, $"USB device {text} unplugged...");
				}
			}
			while (readWriteResult == ReadWriteResult.OK);
		}
		catch (Exception ex)
		{
			Log.Error(Module.BusinessLogic, $"Error occured while handling DriveConfigurationChanged: {ex.ToString()}");
		}
	}
}
