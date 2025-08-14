using System;
using System.Text;
using RWE.SmartHome.SHC.WebSocketsService.Common;
using RWE.SmartHome.SHC.WebSocketsService.Extensions;
using RWE.SmartHome.SHC.WebSocketsService.Util;

namespace RWE.SmartHome.SHC.WebSocketsService.Client;

internal class WSFrame : IDisposable
{
	protected int maskIndex;

	protected int payloadIndex;

	protected int payloadLength;

	protected byte[] frameData;

	public bool FIN => (frameData[0] & 0x80) == 128;

	public byte RSV => (byte)((frameData[0] >> 4) & 7);

	public WSFrameType OpCode => (WSFrameType)(frameData[0] & 0xF);

	public bool IsMasked => (frameData[1] & 0x80) == 128;

	public int MaskIndex => maskIndex;

	public byte[] Mask
	{
		get
		{
			if (maskIndex <= 0)
			{
				return null;
			}
			return frameData.SubArray(maskIndex, 4);
		}
	}

	public int PayloadIndex => payloadIndex;

	public int PayloadLength => payloadLength;

	public byte[] PayloadBytes
	{
		get
		{
			if (payloadLength <= 0)
			{
				return null;
			}
			return frameData.SubArray(payloadIndex, payloadLength);
		}
	}

	public string PayloadText
	{
		get
		{
			if (payloadLength <= 0)
			{
				return null;
			}
			return frameData.ToString(payloadIndex, payloadLength);
		}
	}

	public byte[] FrameData => frameData;

	protected WSFrame()
	{
		frameData = null;
		maskIndex = 0;
		payloadLength = 0;
		payloadIndex = 0;
	}

	public void Dispose()
	{
		frameData = null;
	}

	public static WSFrame CreateFrame(WSFrameType opCode)
	{
		return CreateFrame(opCode, isMasked: false, (byte[])null);
	}

	public static WSFrame CreateFrame(WSFrameType opCode, bool isMasked, string payLoad)
	{
		return CreateFrame(opCode, isMasked, Encoding.UTF8.GetBytes(payLoad));
	}

	public static WSFrame CreateFrame(WSFrameType opCode, bool isMasked, byte[] payLoad)
	{
		byte b = (byte)((WSFrameType)128 | opCode);
		byte b2 = (byte)((isMasked && payLoad != null && payLoad.Length > 0) ? 128 : 0);
		byte[] array = (isMasked ? CryptoUtils.GetRandomBytes(4) : new byte[0]);
		int num = 0;
		int num2 = 0;
		ushort num3 = 0;
		uint num4 = 0u;
		ulong num5 = 0uL;
		byte[] array2 = null;
		if (payLoad == null || payLoad.Length == 0)
		{
			num4 = 0u;
			num2 = 0;
			array2 = ArrayUtil.Concat(b, b2);
		}
		else if (payLoad.Length <= 125)
		{
			b2 |= (byte)payLoad.Length;
			num4 = (uint)payLoad.Length;
			num = (isMasked ? 2 : 0);
			num2 = (isMasked ? 6 : 2);
			array2 = ArrayUtil.Concat(b, b2, array, payLoad);
		}
		else if (payLoad.Length <= 65535)
		{
			b2 |= 0x7E;
			num3 = (ushort)payLoad.Length;
			num4 = num3;
			num = (isMasked ? 4 : 0);
			num2 = (isMasked ? 8 : 4);
			array2 = ArrayUtil.Concat(b, b2, num3, array, payLoad);
		}
		else
		{
			b2 |= 0x7F;
			num4 = (uint)payLoad.Length;
			num5 = (ulong)payLoad.Length;
			num = (isMasked ? 10 : 0);
			num2 = (isMasked ? 14 : 10);
			array2 = ArrayUtil.Concat(b, b2, num5, array, payLoad);
		}
		WSFrame wSFrame = new WSFrame();
		wSFrame.maskIndex = num;
		wSFrame.payloadIndex = num2;
		wSFrame.payloadLength = (int)num4;
		wSFrame.frameData = array2;
		WSFrame wSFrame2 = wSFrame;
		if (isMasked)
		{
			ApplyMask(wSFrame2);
		}
		return wSFrame2;
	}

	public static bool TryParse(byte[] dataBuffer, int startPos, int length, int maxFrameLength, out WSFrame dataFrame)
	{
		dataFrame = null;
		if (dataBuffer == null || dataBuffer.Length == 0 || dataBuffer.Length < startPos + length)
		{
			return false;
		}
		_ = dataBuffer[startPos];
		byte b = dataBuffer[startPos + 1];
		byte b2 = (byte)(b & 0x7F);
		bool flag = (b & 0x80) == 128;
		int num = 0;
		int num2 = 0;
		ulong num3 = 0uL;
		switch (b2)
		{
		case 127:
			num = (flag ? 10 : 0);
			num2 = (flag ? 14 : 10);
			num3 = dataBuffer.ToUInt64(startPos + 2, JDIConst.ByteOrder.Network);
			break;
		case 126:
			num = (flag ? 4 : 0);
			num2 = (flag ? 8 : 4);
			num3 = dataBuffer.ToUInt16(startPos + 2, JDIConst.ByteOrder.Network);
			break;
		default:
			num = (flag ? 2 : 0);
			num2 = (flag ? 6 : 2);
			num3 = b2;
			break;
		}
		int num4 = num2 + (int)num3;
		if (num4 > maxFrameLength)
		{
			throw new NotSupportedException("Maximum frame size of " + maxFrameLength + " bytes has been exceeded.");
		}
		if (num4 <= length)
		{
			dataFrame = new WSFrame
			{
				maskIndex = num,
				payloadIndex = num2,
				payloadLength = (int)num3,
				frameData = new byte[num4]
			};
			Array.Copy(dataBuffer, startPos, dataFrame.frameData, 0, num4);
			if (flag)
			{
				ApplyMask(dataFrame);
			}
			return true;
		}
		return false;
	}

	public static void ApplyMask(WSFrame wsFrame)
	{
		if (wsFrame != null && wsFrame.payloadLength > 0)
		{
			int num = 0;
			for (int i = 0; i < wsFrame.payloadLength; i++)
			{
				num = wsFrame.maskIndex + i % 4;
				wsFrame.frameData[wsFrame.payloadIndex + i] = (byte)(wsFrame.frameData[wsFrame.payloadIndex + i] ^ wsFrame.frameData[num]);
			}
		}
	}
}
