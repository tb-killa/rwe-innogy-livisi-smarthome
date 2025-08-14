using System;
using System.Collections.Generic;

namespace SerialAPI.BidCosLayer.DevicesSupport;

internal class ReceiveFrameData
{
	public BIDCOSHeader bidcosHeader { get; set; }

	public List<byte> data { get; set; }

	public DateTime receiveTime { get; set; }

	public int m_tmp_frame_count { get; set; }

	public bool bidcosNodeAlreadyRegistered { get; set; }
}
