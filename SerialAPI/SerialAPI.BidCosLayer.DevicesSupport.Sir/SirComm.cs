using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommonFunctionality.Encryption;

namespace SerialAPI.BidCosLayer.DevicesSupport.Sir;

internal class SirComm : DeviceComm
{
	public SirComm(BidCosHandlerRef bidcosHandler)
		: base(bidcosHandler)
	{
	}

	public bool ConfigureNodeKey(byte[] destination, byte[] currentKey, byte[] targetKey)
	{
		bool flag = false;
		byte[] array = null;
		array = BuildSirKeyChangePayload(1, 0, targetKey, currentKey);
		flag = SendBidcos(destination, 4, array, SendMode.Normal);
		if (flag)
		{
			array = BuildSirKeyChangePayload(1, 1, targetKey, currentKey);
			flag = SendBidcos(destination, 4, array, SendMode.Normal);
		}
		return flag;
	}

	public static byte[] BuildSirKeyChangePayload(byte keyIndex, byte keyPart, byte[] newKey, byte[] oldKey)
	{
		byte[] array = new byte[16]
		{
			1,
			(byte)((keyIndex << 1) | (keyPart & 1)),
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};
		newKey.Skip((keyPart != 0) ? 8 : 0).Take(8).ToArray()
			.CopyTo(array, 2);
		byte[] array2 = new byte[2];
		new Random().NextBytes(array2);
		array2.CopyTo(array, 10);
		new byte[4] { 126, 41, 111, 165 }.CopyTo(array, 12);
		Aes aes = new Aes(KeySize.Bits128, oldKey);
		byte[] output = new byte[16];
		aes.Cipher(array, out output);
		return output;
	}

	public List<byte> BuildSirConditionalSwitchMessage(byte[] source, byte[] destination, byte[] message, byte[] counter, byte[] key)
	{
		byte header = 20;
		byte command = 65;
		AesConditionalSwitch aesConditionalSwitch = new AesConditionalSwitch(key);
		byte[] collection = aesConditionalSwitch.CalculateAesSignature(source, destination, counter, header, message);
		List<byte> list = new List<byte>();
		list.AddRange(message);
		list.Add(counter[0]);
		list.Add(counter[1]);
		list.AddRange(collection);
		return BuildBidCosMessageSirStatic(header, source, destination, command, list.ToArray(), SendMode.Normal, counter[2]);
	}

	public void LogIfVerbose(string message)
	{
	}

	public void SendRampStart(byte[] address, byte channel, double switchingLevel, int? delayMs)
	{
		List<byte> list = new List<byte>();
		list.Add(2);
		list.Add(channel);
		list.Add((byte)(switchingLevel * 2.0));
		byte[] collection = new byte[2];
		list.AddRange(collection);
		byte[] collection2;
		if (!delayMs.HasValue)
		{
			byte[] array = new byte[2];
			collection2 = array;
		}
		else
		{
			collection2 = ConvertMsToBytes(delayMs.Value);
		}
		list.AddRange(collection2);
		SendDecSiren(address, list.ToArray(), SendMode.Normal);
	}

	public bool EnableAllChannels(byte[] address)
	{
		List<byte> list = new List<byte>();
		list.Add(2);
		list.Add(4);
		list.Add(200);
		list.Add(0);
		list.Add(0);
		return SendDecSiren(address, list.ToArray(), SendMode.Normal);
	}

	public bool DisableConfirmationSound(byte[] destinationAddress)
	{
		return SendConfigDataTransaction(4, destinationAddress, new byte[2] { 169, 0 }, SendMode.Burst);
	}

	public bool SendDecSiren(byte[] destinationAddress, byte[] message, SendMode mode)
	{
		return SendBidcos(destinationAddress, 17, message, mode);
	}

	public static byte[] ConvertMsToBytes(int miliseconds)
	{
		long num = miliseconds / 100;
		int num2 = (byte)Math.Floor(Math.Log(Math.Abs(num)) / Math.Log(2.0)) + 1;
		int num3 = num2 - 11;
		if (num3 > 0)
		{
			num >>= num3;
		}
		num3 = Math.Max(num3, 0);
		byte b = (byte)(num >> 3);
		byte b2 = (byte)(((num & 7) << 5) | (byte)(num3 & 0x1F));
		return new byte[2] { b, b2 };
	}

	private List<byte> BuildBidCosMessageSirStatic(byte header, byte[] sender, byte[] address, byte command, byte[] data, SendMode mode, byte specialCounter)
	{
		List<byte> list = new List<byte>();
		byte item = specialCounter;
		list.Add(item);
		if (mode != SendMode.Normal)
		{
			header |= 0x10;
		}
		list.Add(header);
		list.Add(command);
		list.AddRange(sender);
		list.AddRange(address);
		list.AddRange(data);
		return list;
	}

	public bool SendConfigDataTransaction(byte channel, byte[] address, byte[] parameters, SendMode mode)
	{
		List<BidCosMessage> list = new List<BidCosMessage>();
		list.Add(new BidCosMessage
		{
			Command = 1,
			Data = new byte[7] { channel, 5, 0, 0, 0, 0, 1 },
			DestinationAddress = address,
			SenderAddress = bidcosHandler.DefaultIP,
			Header = bidcosHandler.GetDefaultHeader()
		});
		list.Add(new BidCosMessage
		{
			Command = 1,
			Data = new List<byte> { channel, 8 }.Concat(parameters).ToArray(),
			DestinationAddress = address,
			SenderAddress = bidcosHandler.DefaultIP,
			Header = bidcosHandler.GetDefaultHeader()
		});
		list.Add(new BidCosMessage
		{
			Command = 1,
			Data = new byte[2] { channel, 6 },
			DestinationAddress = address,
			SenderAddress = bidcosHandler.DefaultIP,
			Header = bidcosHandler.GetDefaultHeader()
		});
		List<BidCosMessage> messages = list;
		return SendBidcosTransaction(messages, 5, SendMode.Burst);
	}

	protected bool SendBidcosTransaction(List<BidCosMessage> messages, int retriesCount, SendMode mode)
	{
		bool flag = false;
		for (int i = 0; i < retriesCount; i++)
		{
			if (flag)
			{
				break;
			}
			flag = true;
			foreach (BidCosMessage message in messages)
			{
				if (i > 0)
				{
					message.Header = (byte)(message.Header | 0x10 | 1);
				}
				bidcosHandler.answerFrameReceivedEvent.Reset();
				SendMessage(message, mode);
				flag = flag && IsACK();
				if (!flag)
				{
					mode = SendMode.Burst;
					break;
				}
			}
		}
		return flag;
	}

	private SendStatus SendMessage(BidCosMessage message, SendMode mode)
	{
		SendStatus sendStatus = SendStatus.NO_REPLY;
		List<byte> data = (base.PendingMessage = GetBidcosMessage(message, mode));
		for (int i = 0; i < 3; i++)
		{
			if (sendStatus == SendStatus.ACK)
			{
				break;
			}
			if (i > 1)
			{
				Thread.Sleep(11);
			}
			sendStatus = bidcosHandler.BroadcastFrameToAir(data, mode);
		}
		return sendStatus;
	}

	private bool IsACK()
	{
		int millisecondsTimeout = 1200;
		if (!bidcosHandler.answerFrameReceivedEvent.WaitOne(millisecondsTimeout, exitContext: false))
		{
			return false;
		}
		return bidcosHandler.m_answer_ack;
	}

	private List<byte> GetBidcosMessage(BidCosMessage msg, SendMode mode)
	{
		return BuildBidCosMessage(msg.Header, msg.SenderAddress, msg.DestinationAddress, msg.Command, msg.Data, mode, IncrementFrameType.Increment, useSpecialCounter: false, 0);
	}
}
