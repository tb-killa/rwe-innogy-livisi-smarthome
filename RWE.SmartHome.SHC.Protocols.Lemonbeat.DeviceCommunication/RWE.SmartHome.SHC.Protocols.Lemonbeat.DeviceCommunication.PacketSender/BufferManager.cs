using System;
using System.Collections.Generic;
using System.Linq;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.PacketSender;

public static class BufferManager
{
	private const int PoolSize = 3;

	private static object sync = new object();

	private static List<byte[]> freeBuffers = new List<byte[]>();

	private static List<byte[]> takenBuffers = new List<byte[]>();

	public static byte[] GetBuffer()
	{
		lock (sync)
		{
			byte[] array;
			if (freeBuffers.Count > 0)
			{
				array = freeBuffers.Last();
				freeBuffers.Remove(array);
			}
			else
			{
				array = new byte[32768];
			}
			takenBuffers.Add(array);
			return array;
		}
	}

	public static void ReleaseBuffer(byte[] buffer)
	{
		lock (sync)
		{
			if (!takenBuffers.Remove(buffer))
			{
				throw new ArgumentException("Invalid buffer being freed.");
			}
			if (freeBuffers.Count < 3)
			{
				freeBuffers.Add(buffer);
			}
		}
	}
}
