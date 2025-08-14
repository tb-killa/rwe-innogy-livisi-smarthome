using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Rebex;
using Rebex.Net;

namespace onrkn;

internal sealed class ewxsp
{
	private const int wwmfb = 53;

	private ewxsp()
	{
	}

	public static string[] dhjwv(IPAddress p0, string p1, int p2)
	{
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		try
		{
			EndPoint remoteEP = new IPEndPoint(p0, 53);
			byte[] buffer = bmdxf(p1);
			socket.SendTo(buffer, remoteEP);
			byte[] array = new byte[1024];
			EndPoint remoteEP2 = new IPEndPoint(IPAddress.Any, 0);
			IAsyncResult asyncResult = socket.BeginReceiveFrom(array, 0, array.Length, SocketFlags.None, ref remoteEP2, null, null);
			bool flag = true;
			int num = Environment.TickCount + p2;
			while (!asyncResult.IsCompleted)
			{
				if (Environment.TickCount > num)
				{
					flag = false;
					socket.Close();
					break;
				}
				Thread.Sleep(1);
			}
			int num2;
			try
			{
				num2 = socket.EndReceiveFrom(asyncResult, ref remoteEP2);
			}
			catch
			{
				if (!flag || 1 == 0)
				{
					throw new SmtpException("DNS MX query timed out.", SmtpExceptionStatus.Timeout);
				}
				throw;
			}
			byte[] array2 = new byte[num2];
			Array.Copy(array, 0, array2, 0, num2);
			ArrayList arrayList = new ArrayList();
			olhjd(array2, arrayList);
			arrayList.Sort();
			string[] array3 = new string[arrayList.Count];
			int num3 = 0;
			if (num3 != 0)
			{
				goto IL_00f6;
			}
			goto IL_0114;
			IL_0114:
			if (num3 < arrayList.Count)
			{
				goto IL_00f6;
			}
			return array3;
			IL_00f6:
			array3[num3] = ((inouf)arrayList[num3]).yvmaj;
			num3++;
			goto IL_0114;
		}
		finally
		{
			if (socket != null && 0 == 0)
			{
				((IDisposable)socket).Dispose();
			}
		}
	}

	private static byte[] bmdxf(string p0)
	{
		MemoryStream memoryStream = new MemoryStream();
		int tickCount = Environment.TickCount;
		memoryStream.WriteByte((byte)((tickCount >> 8) & 0xFF));
		memoryStream.WriteByte((byte)(tickCount & 0xFF));
		memoryStream.WriteByte(1);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(1);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		memoryStream.WriteByte(0);
		string[] array = p0.Split('.');
		string text = "";
		int num = 0;
		if (num != 0)
		{
			goto IL_0098;
		}
		goto IL_00d0;
		IL_0098:
		text = array[num];
		memoryStream.WriteByte((byte)(text.Length & 0xFF));
		byte[] bytes = EncodingTools.ASCII.GetBytes(text);
		memoryStream.Write(bytes, 0, bytes.Length);
		num++;
		goto IL_00d0;
		IL_00d0:
		if (num >= array.Length)
		{
			memoryStream.WriteByte(0);
			memoryStream.WriteByte(0);
			memoryStream.WriteByte(15);
			memoryStream.WriteByte(0);
			memoryStream.WriteByte(1);
			return memoryStream.ToArray();
		}
		goto IL_0098;
	}

	private static int olhjd(byte[] p0, ArrayList p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		if (p0.Length < 8)
		{
			throw new SmtpException("Invalid answer for DNS MX query.", SmtpExceptionStatus.ServerProtocolViolation);
		}
		int p2 = p0.Length;
		int num = 12;
		int num2 = ((p0[4] & 0xFF) << 8) | p0[5];
		int num3 = ((p0[6] & 0xFF) << 8) | p0[7];
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_005a;
		}
		goto IL_0076;
		IL_0119:
		int num5;
		if (num5 < num3)
		{
			goto IL_0085;
		}
		return num;
		IL_0085:
		string p3 = "";
		num = jhqzv(num, p2, p0, ref p3);
		int num6 = (p0[num++] << 8) | p0[num++];
		int num7 = -1;
		switch (num6)
		{
		case 15:
			num += 8;
			num7 = (p0[num++] << 8) | p0[num++];
			break;
		case 5:
			num += 8;
			break;
		default:
			throw new SmtpException("DNS MX record contains an unsupported record type.", SmtpExceptionStatus.OperationFailure);
		}
		p3 = "";
		num = jhqzv(num, p2, p0, ref p3);
		if (num7 >= 0)
		{
			inouf value = new inouf(num7, p3);
			p1.Add(value);
		}
		num5++;
		goto IL_0119;
		IL_0076:
		if (num4 < num2)
		{
			goto IL_005a;
		}
		num5 = 0;
		if (num5 != 0)
		{
			goto IL_0085;
		}
		goto IL_0119;
		IL_005a:
		p3 = "";
		num = jhqzv(num, p2, p0, ref p3);
		num += 4;
		num4++;
		goto IL_0076;
	}

	private static int jhqzv(int p0, int p1, byte[] p2, ref string p3)
	{
		if (p0 < 0)
		{
			throw new SmtpException("DNS MX record contains invalid pointer.", SmtpExceptionStatus.ServerProtocolViolation);
		}
		if (p0 >= p2.Length)
		{
			throw new SmtpException("DNS MX record ends prematurely.", SmtpExceptionStatus.ServerProtocolViolation);
		}
		int num = p2[p0++];
		if (num == 0 || 1 == 0)
		{
			return p0;
		}
		do
		{
			if ((num & 0xC0) == 192)
			{
				if (p0 >= p1)
				{
					return -1;
				}
				int p4 = ((num & 0x3F) << 8) | (p2[p0++] & 0xFF);
				jhqzv(p4, p1, p2, ref p3);
				return p0;
			}
			if (p0 + num > p1)
			{
				return -1;
			}
			p3 += EncodingTools.ASCII.GetString(p2, p0, num);
			p0 += num;
			if (p0 >= p1)
			{
				return -1;
			}
			num = p2[p0++];
			if (num != 0 && 0 == 0)
			{
				p3 += ".";
			}
		}
		while ((num != 0) ? true : false);
		return p0;
	}
}
