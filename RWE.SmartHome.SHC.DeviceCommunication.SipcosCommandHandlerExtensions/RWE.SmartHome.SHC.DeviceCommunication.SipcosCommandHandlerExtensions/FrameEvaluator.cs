using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;

namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public class FrameEvaluator
{
	private struct FrameIdentifier
	{
		public byte[] Source { private get; set; }

		public byte Channel { private get; set; }

		public bool BiDi { private get; set; }

		public override int GetHashCode()
		{
			int num = Channel;
			if (Source != null)
			{
				byte[] source = Source;
				foreach (byte b in source)
				{
					num ^= b << 8;
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is FrameIdentifier frameIdentifier))
			{
				return false;
			}
			if (frameIdentifier.Source == null)
			{
				return false;
			}
			if (frameIdentifier.Source.SequenceEqual(Source) && frameIdentifier.Channel.Equals(Channel))
			{
				return frameIdentifier.BiDi.Equals(BiDi);
			}
			return false;
		}
	}

	private class FrameData
	{
		public byte SequenceNumber { get; set; }

		public DateTime TimeOfArrival { get; set; }
	}

	private readonly Dictionary<FrameIdentifier, FrameData> frameHolder = new Dictionary<FrameIdentifier, FrameData>();

	private readonly IDeviceList deviceList;

	private DateTime lastTimeForOldFramesCleanup = DateTime.UtcNow;

	private double retentionTime = 15000.0;

	private double cleanupInterval = 1000.0;

	public double RetentionTime
	{
		get
		{
			return retentionTime;
		}
		set
		{
			if (value < CleanupInterval)
			{
				CleanupInterval = value;
			}
			retentionTime = value;
		}
	}

	public double CleanupInterval
	{
		get
		{
			return cleanupInterval;
		}
		set
		{
			cleanupInterval = ((value > RetentionTime) ? RetentionTime : value);
		}
	}

	public FrameEvaluator(IDeviceList deviceList)
	{
		this.deviceList = deviceList;
	}

	public bool IsDuplicatedOrOutOfOrderFrame(byte[] source, byte channel, byte sequenceNumber, bool biDi)
	{
		if (deviceList == null || !deviceList.ContainsRouter)
		{
			return false;
		}
		if (DateTime.UtcNow.Subtract(lastTimeForOldFramesCleanup).TotalMilliseconds >= CleanupInterval)
		{
			RemoveOldFrames();
		}
		bool result = false;
		DateTime utcNow = DateTime.UtcNow;
		FrameIdentifier key = new FrameIdentifier
		{
			Source = source,
			Channel = channel,
			BiDi = biDi
		};
		if (frameHolder.TryGetValue(key, out var value))
		{
			if (value.SequenceNumber >= sequenceNumber)
			{
				result = true;
			}
			else
			{
				value.SequenceNumber = sequenceNumber;
				value.TimeOfArrival = utcNow;
			}
		}
		else
		{
			frameHolder.Add(key, new FrameData
			{
				SequenceNumber = sequenceNumber,
				TimeOfArrival = utcNow
			});
		}
		return result;
	}

	private void RemoveOldFrames()
	{
		List<FrameIdentifier> list = (from kvp in frameHolder
			where DateTime.UtcNow.Subtract(kvp.Value.TimeOfArrival).TotalMilliseconds >= RetentionTime
			select kvp.Key).ToList();
		foreach (FrameIdentifier item in list)
		{
			frameHolder.Remove(item);
		}
		lastTimeForOldFramesCleanup = DateTime.UtcNow;
	}
}
