using System.Collections.Generic;

namespace SipcosCommandHandler;

public struct NetworkAcceptFrame
{
	public byte[] MIC32;

	public byte[] CCM;

	public byte[] OneTimeKey;

	public NetworkAcceptKey KeyUsed;

	public byte[] NetworkSyncWord;

	internal List<byte> getSerialData()
	{
		List<byte> list = new List<byte>();
		list.Add((byte)KeyUsed);
		if (CCM == null)
		{
			byte[] collection = new byte[16];
			list.AddRange(collection);
		}
		else
		{
			list.AddRange(CCM);
		}
		if (MIC32 == null)
		{
			byte[] collection2 = new byte[4];
			list.AddRange(collection2);
		}
		else
		{
			list.AddRange(MIC32);
		}
		if (OneTimeKey == null)
		{
			byte[] collection3 = new byte[16];
			list.AddRange(collection3);
		}
		else
		{
			list.AddRange(OneTimeKey);
		}
		if (NetworkSyncWord == null)
		{
			list.AddRange(new byte[2] { 154, 125 });
		}
		else
		{
			list.AddRange(NetworkSyncWord);
		}
		return list;
	}
}
