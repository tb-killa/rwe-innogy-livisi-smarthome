using System;

namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public class DirectExecutionCommand
{
	public ActionCode ActionCode { get; set; }

	public byte Channel { get; set; }

	public byte[] ActionData { get; set; }

	public DirectExecutionCommand(byte[] buffer)
	{
		ActionCode = (ActionCode)buffer[0];
		Channel = buffer[1];
		ActionData = new byte[buffer.Length - 2];
		Array.Copy(buffer, 2, ActionData, 0, ActionData.Length);
	}

	public DirectExecutionCommand()
	{
	}

	public byte[] ToArray()
	{
		byte[] array = new byte[ActionData.Length + 2];
		array[0] = (byte)ActionCode;
		array[1] = Channel;
		Array.Copy(ActionData, 0, array, 2, ActionData.Length);
		return array;
	}
}
