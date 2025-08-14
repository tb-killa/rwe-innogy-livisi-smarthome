using System;
using System.Collections.Generic;
using SerialAPI;

namespace RWE.SmartHome.SHC.DeviceManager;

public class MulticastSwitchParameters
{
	public IEnumerable<Guid> TargetDeviceIds { get; set; }

	public SIPcosFrameType FrameType { get; set; }

	public byte[] CommandBuffer { get; set; }

	public byte[] SourceAddress { get; set; }

	public MulticastSwitchParameters(IEnumerable<Guid> targetDeviceIds, SIPcosFrameType frameType, byte[] commandBuffer, byte[] source)
	{
		TargetDeviceIds = targetDeviceIds;
		FrameType = frameType;
		CommandBuffer = commandBuffer;
		SourceAddress = source;
	}
}
