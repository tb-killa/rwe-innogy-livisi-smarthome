using System.IO;
using System.Threading;

namespace RWE.SmartHome.SHC.HCI;

public class HciReceiver
{
	public static HciMessage ReceiveHciMessage(Stream stream)
	{
		ReceiveBuffer receiveBuffer = new ReceiveBuffer(4);
		bool flag = false;
		bool flag2 = true;
		HciMessage result = null;
		do
		{
			int num = stream.ReadByte();
			if (num != -1)
			{
				receiveBuffer.Add((byte)num);
				if (receiveBuffer.Filled)
				{
					if (!flag2)
					{
						flag = true;
						result = new HciMessage(receiveBuffer.ToArray());
					}
					else if (receiveBuffer[0] == 165)
					{
						flag2 = false;
						HciHeader hciHeader = new HciHeader(receiveBuffer.ToArray(), 1);
						int messageLength = hciHeader.MessageLength;
						receiveBuffer.Size = messageLength;
					}
				}
			}
			else
			{
				Thread.Sleep(10);
			}
		}
		while (!flag);
		return result;
	}
}
