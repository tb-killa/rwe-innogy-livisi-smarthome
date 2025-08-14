using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using SerialApiInterfaces;

namespace SerialAPI;

public class SerialDLL : DebugInterface, ISerialPort, IDisposable
{
	private IntPtr pHandlerObject = IntPtr.Zero;

	public string[] SerialPorts => SerialPort.GetPortNames();

	public event ReceiveSerialData ReceiveData;

	[DllImport("SerialDLL.dll")]
	public static extern IntPtr CreateHandler();

	[DllImport("SerialDLL.dll")]
	public static extern void DisposeHandler(IntPtr pHandlerObject);

	[DllImport("SerialDLL.dll", CharSet = CharSet.Unicode)]
	public static extern int CallOpen(IntPtr pHandlerObject, string port, int baud);

	[DllImport("SerialDLL.dll")]
	public static extern int CallClose(IntPtr pHandlerObject);

	[DllImport("SerialDLL.dll")]
	public static extern int CallRead(IntPtr pHandlerObject, byte[] buffer, int size);

	[DllImport("SerialDLL.dll")]
	public static extern int CallWrite(IntPtr pHandlerObject, byte[] buffer, int size);

	public SerialDLL()
	{
		pHandlerObject = CreateHandler();
	}

	public void Dispose()
	{
		DisposeHandler(pHandlerObject);
		pHandlerObject = IntPtr.Zero;
	}

	public bool Open(string port)
	{
		return Open(port, Baudrate.Baudrate115200);
	}

	public bool Open(string port, Baudrate baudrate)
	{
		if (CallOpen(pHandlerObject, port, (int)baudrate) == 1)
		{
			Thread thread = new Thread(SerialReadThread);
			thread.Name = "Serial Read Thread";
			thread.IsBackground = true;
			thread.Start();
			return true;
		}
		return false;
	}

	public void Close()
	{
		CallClose(pHandlerObject);
	}

	private void SerialReadThread()
	{
		byte[] array = new byte[100];
		while (true)
		{
			ReceiveSerialData receiveData = this.ReceiveData;
			if (receiveData != null)
			{
				int num = CallRead(pHandlerObject, array, array.Length);
				if (num > 0)
				{
					receiveData(array.Take(num).ToList());
				}
			}
			else
			{
				Thread.Sleep(1000);
			}
		}
	}

	public int Send(List<byte> data)
	{
		byte[] array = data.ToArray();
		return CallWrite(pHandlerObject, array, array.Length);
	}
}
