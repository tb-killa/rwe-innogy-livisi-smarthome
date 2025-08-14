using System;
using System.Runtime.InteropServices;
using System.Text;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Services;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication;

public class LemonbeatExiCompressor : IExiCompressor
{
	private const int DefaultInitialBufferSize = 20480;

	private const int MaximumCompressionSize = 81920;

	private object EXICompressionSyncObject = new object();

	private byte[] EXICompressionBuffer = new byte[20480];

	[DllImport("EXIDLL.dll")]
	public static extern int compressXML(ushort port, byte[] xmlBuf, uint xmlBufLen, byte[] exiBuf, uint exiBufLen, uint bitOffset);

	[DllImport("EXIDLL.dll")]
	public static extern int decompressEXI(ushort port, byte[] exiBuf, uint exiBufLen, byte[] xmlBuf, uint xmlBufLen, uint bitOffset);

	public byte[] GetCompressedMessage(string uncompressedMessage, LemonbeatServicePort port)
	{
		Log.Debug(Module.LemonbeatProtocolAdapter, $"COMPRESS EXI Port [{port}]: Uncompressed Message: {uncompressedMessage}");
		byte[] bytes = Encoding.ASCII.GetBytes(uncompressedMessage);
		int num = 0;
		byte[] array;
		lock (EXICompressionSyncObject)
		{
			num = compressXML((ushort)port, bytes, (uint)bytes.Length, EXICompressionBuffer, (uint)EXICompressionBuffer.Length, 0u);
			if (num < 0)
			{
				num = -num;
			}
			array = new byte[num];
			if (num <= EXICompressionBuffer.Length)
			{
				EXICompressionBuffer.CopySubArray(array, 0, num);
			}
			else
			{
				num = compressXML((ushort)port, bytes, (uint)bytes.Length, array, (uint)array.Length, 0u);
			}
		}
		Log.Debug(Module.LemonbeatProtocolAdapter, $"COMPRESS EXI Port [{port}]: Compressed Message: {array.ToReadable()}");
		return array;
	}

	public string DecompressMessage(byte[] exiMessage, LemonbeatServicePort port)
	{
		string text = null;
		int num = 0;
		Log.Debug(Module.LemonbeatProtocolAdapter, $"DECOMPRESS EXI Port [{port}]: Compressed Message: {exiMessage.ToReadable()}");
		lock (EXICompressionSyncObject)
		{
			num = decompressEXI((ushort)port, exiMessage, (uint)exiMessage.Length, EXICompressionBuffer, (uint)EXICompressionBuffer.Length, 0u);
			if (num < 0)
			{
				num = -num;
			}
			if (num >= 81920)
			{
				throw new Exception("The uncompressed EXI message exceeds " + 81920 + " characters");
			}
			if (EXICompressionBuffer.Length < num)
			{
				EXICompressionBuffer = new byte[1024 * (num / 1024 + 1)];
				Log.Information(Module.LemonbeatProtocolAdapter, "Exi buffer length exceeded, increased to " + EXICompressionBuffer.Length);
				decompressEXI((ushort)port, exiMessage, (uint)exiMessage.Length, EXICompressionBuffer, (uint)EXICompressionBuffer.Length, 0u);
			}
			text = Encoding.ASCII.GetString(EXICompressionBuffer, 0, num);
		}
		Log.Debug(Module.LemonbeatProtocolAdapter, $"DECOMPRESS EXI Port [{port}]: Uncompressed Message: {text}");
		return text;
	}
}
