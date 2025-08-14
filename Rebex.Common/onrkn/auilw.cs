using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace onrkn;

internal static class auilw
{
	public const int mcesh = 995;

	public const int culgy = 10004;

	public const int itzfb = 10035;

	public const int lfdvs = 10038;

	public const int pjlvq = 10053;

	public const int jwsyj = 10054;

	public const int txmvu = 10058;

	public const int ctcsl = 10060;

	public static Func<IAsyncResult, IPHostEntry> syedj = dpbpb;

	public static int skehp(this SocketException p0)
	{
		return p0.ErrorCode;
	}

	public static bool kmqhm(this SocketException p0)
	{
		switch (p0.skehp())
		{
		case 10050:
		case 10051:
		case 10052:
		case 10053:
		case 10054:
			return true;
		default:
			return false;
		}
	}

	public static string astkw(this SocketException p0)
	{
		int p1 = p0.skehp();
		return jczqg(p1);
	}

	public static string jczqg(int p0)
	{
		return mmchc(p0, null);
	}

	public static string mmchc(int p0, string p1)
	{
		switch (p0)
		{
		case 10013:
			return "An attempt was made to access a socket in a way forbidden by its access permissions.";
		case 10038:
			return "Connection has been lost.";
		case 10040:
			return "Message too long.";
		case 10050:
			return "A socket operation encountered a dead network.";
		case 10051:
			return "A socket operation was attempted to an unreachable network.";
		case 10052:
			return "The connection has been broken due to keep-alive activity detecting a failure while the operation was in progress.";
		case 10053:
			return "An established connection was aborted.";
		case 10054:
			return "An existing connection was forcibly closed by the remote host.";
		case 10057:
			return "A request to send or receive data was disallowed because the socket is not connected.";
		case 10060:
			return "A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond.";
		case 10061:
			return "No connection could be made because the target machine actively refused it.";
		case 10064:
			return "A socket operation failed because the destination host was down.";
		case 10065:
			return "A socket operation was attempted to an unreachable host.";
		case 10109:
			return "The specified class was not found.";
		case 11001:
			return "No such host is known.";
		case 11004:
			return "The requested name is valid, but no data of the requested type was found.";
		default:
		{
			string text = p1;
			if (text == null || 1 == 0)
			{
				text = brgjd.edcru("Socket error {0} occured.", p0);
			}
			return text;
		}
		}
	}

	public static string mxmjt()
	{
		return Dns.GetHostName();
	}

	public static string ymclc()
	{
		string hostName = Dns.GetHostName();
		if (string.IsNullOrEmpty(hostName) && 0 == 0)
		{
			return "localhost";
		}
		hostName = hostName.Replace('_', '-');
		return hostName.ToLower(CultureInfo.InvariantCulture);
	}

	public static string vydkr(string p0, out string p1)
	{
		string text;
		try
		{
			byte[] array = Convert.FromBase64String(p0);
			text = Encoding.UTF8.GetString(array, 0, array.Length);
		}
		catch
		{
			p1 = "Invalid OAuth response.";
			return null;
		}
		int p2 = 0;
		string text2 = text.Replace('{', ' ').Replace('}', ' ').Replace('"', ' ')
			.Replace('\t', ' ')
			.Replace(" ", "");
		string text3 = "status:";
		int num = text2.IndexOf(text3);
		if (num >= 0)
		{
			num += text3.Length;
			int num2 = text2.IndexOf(',', num);
			if (num2 < 0)
			{
				num2 = text2.Length;
			}
			text3 = text2.Substring(num, num2 - num);
			if (!dahxy.crqjb(text3, out p2) || 1 == 0)
			{
				p2 = 0;
			}
		}
		switch (p2)
		{
		case 0:
			p1 = "OAUTH: Unable to authenticate.";
			break;
		case 400:
			p1 = "OAUTH: Bad authentication request (400).";
			break;
		case 401:
			p1 = "OAUTH: Unauthorized (401).";
			break;
		default:
			p1 = "OAUTH: Unable to authenticate (" + p2 + ").";
			break;
		}
		return text;
	}

	public static IPHostEntry qennv(string p0)
	{
		IPHostEntry hostEntry = Dns.GetHostEntry(p0);
		return axofj(hostEntry, p0);
	}

	public static IAsyncResult znhgl(string p0, AsyncCallback p1)
	{
		return Dns.BeginGetHostEntry(p0, p1, p0);
	}

	public static IPHostEntry dpbpb(IAsyncResult p0)
	{
		IPHostEntry p1 = Dns.EndGetHostEntry(p0);
		string p2 = (string)p0.AsyncState;
		return axofj(p1, p2);
	}

	private static IPHostEntry axofj(IPHostEntry p0, string p1)
	{
		return p0;
	}

	public static IPEndPoint bolwk(string p0, int p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("host");
		}
		if (p0.IndexOf(":") >= 0)
		{
			try
			{
				IPAddress iPAddress = IPAddress.Parse(p0);
				if (iPAddress.AddressFamily != AddressFamily.InterNetworkV6)
				{
					return null;
				}
				return new IPEndPoint(iPAddress, p1);
			}
			catch (FormatException)
			{
				return null;
			}
		}
		Regex regex = new Regex("^\\s*([0-9]{1,3})\\.([0-9]{1,3})\\.([0-9]{1,3})\\.([0-9]{1,3})\\s*$");
		Match match = regex.Match(p0);
		if (!match.Success || false || match.Groups.Count != 5)
		{
			return null;
		}
		uint num = 0u;
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0093;
		}
		goto IL_00cc;
		IL_00cc:
		if (num2 < 4)
		{
			goto IL_0093;
		}
		return new IPEndPoint(num, p1);
		IL_0093:
		uint num3 = uint.Parse(match.Groups[num2 + 1].Value);
		if (num3 > 255)
		{
			return null;
		}
		num |= num3 << 8 * num2;
		num2++;
		goto IL_00cc;
	}

	public static IPEndPoint tulbp(IPHostEntry p0, int p1)
	{
		if (p0.AddressList == null || 1 == 0)
		{
			throw new InvalidOperationException("No IP address records found in the host entry.");
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_0023;
		}
		goto IL_0041;
		IL_0070:
		int num2;
		if (num2 < p0.AddressList.Length)
		{
			goto IL_0051;
		}
		throw new InvalidOperationException("No IP address records found in the host entry.");
		IL_0051:
		IPAddress iPAddress = p0.AddressList[num2];
		if (iPAddress.AddressFamily == AddressFamily.InterNetworkV6)
		{
			return new IPEndPoint(iPAddress, p1);
		}
		num2++;
		goto IL_0070;
		IL_0023:
		IPAddress iPAddress2 = p0.AddressList[num];
		if (iPAddress2.AddressFamily == AddressFamily.InterNetwork)
		{
			return new IPEndPoint(iPAddress2, p1);
		}
		num++;
		goto IL_0041;
		IL_0041:
		if (num < p0.AddressList.Length)
		{
			goto IL_0023;
		}
		num2 = 0;
		if (num2 != 0)
		{
			goto IL_0051;
		}
		goto IL_0070;
	}

	public static bool pogqj(string p0)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("host");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			return false;
		}
		bool flag = true;
		bool flag2 = true;
		int num = 0;
		if (num != 0)
		{
			goto IL_0037;
		}
		goto IL_00d1;
		IL_0037:
		int num2 = p0[num];
		if (num2 < 48 || num2 > 57)
		{
			if (num2 == 46)
			{
				flag2 = false;
				if (!flag2)
				{
					goto IL_00cd;
				}
			}
			if ((num2 >= 97 && num2 <= 102) || (num2 >= 65 && num2 <= 70))
			{
				flag = false;
				if (!flag)
				{
					goto IL_00cd;
				}
			}
			if ((num2 >= 103 && num2 <= 122) || (num2 >= 71 && num2 <= 90) || num2 > 128 || num2 == 45 || num2 == 95)
			{
				flag = false;
				flag2 = false;
				if (!flag2)
				{
					goto IL_00cd;
				}
			}
			if (num2 == 58 || num2 == 37)
			{
				if (!flag2 || 1 == 0)
				{
					return false;
				}
				IPEndPoint iPEndPoint = bolwk(p0, 0);
				return iPEndPoint != null;
			}
			return false;
		}
		goto IL_00cd;
		IL_00cd:
		num++;
		goto IL_00d1;
		IL_00d1:
		if (num >= p0.Length)
		{
			if (flag && 0 == 0)
			{
				IPEndPoint iPEndPoint2 = bolwk(p0, 0);
				return iPEndPoint2 != null;
			}
			return true;
		}
		goto IL_0037;
	}

	public static int kgtmu(Uri p0)
	{
		int num = p0.Port;
		string scheme;
		if (num <= 0 && (scheme = p0.Scheme) != null && 0 == 0)
		{
			if ((!(scheme == "http") || 1 == 0) && !(scheme == "ws"))
			{
				if (scheme == "https" || scheme == "wss")
				{
					goto IL_0083;
				}
			}
			else
			{
				num = 80;
				if (num == 0)
				{
					goto IL_0083;
				}
			}
		}
		goto IL_0091;
		IL_0091:
		return num;
		IL_0083:
		num = 443;
		goto IL_0091;
	}

	public static bool cbyyw(string p0, UriKind p1, out Uri p2)
	{
		bool flag = Uri.TryCreate(p0, p1, out p2);
		if (flag && 0 == 0 && p2.Port < 0)
		{
			if ((!(p2.Scheme == "ws") || 1 == 0) && !(p2.Scheme == "wss"))
			{
				p2 = null;
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder(p2.Scheme + "://");
			stringBuilder.Append(p2.Authority + ":");
			stringBuilder.arumx((p2.Scheme == "ws") ? 80 : 443);
			stringBuilder.Append(p2.PathAndQuery);
			flag = Uri.TryCreate(stringBuilder.ToString(), p1, out p2);
			flag &= p2.Port > 0;
		}
		return flag;
	}

	public static bool giojp(Uri p0, string p1, out Uri p2)
	{
		bool flag = Uri.TryCreate(p0, p1, out p2);
		if (flag && 0 == 0 && p2.Port < 0)
		{
			if ((!(p2.Scheme == "ws") || 1 == 0) && !(p2.Scheme == "wss"))
			{
				p2 = null;
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder(p2.Scheme + "://");
			stringBuilder.Append(p2.Authority + ":");
			stringBuilder.arumx((p2.Scheme == "ws") ? 80 : 443);
			stringBuilder.Append(p2.PathAndQuery);
			flag = Uri.TryCreate(stringBuilder.ToString(), UriKind.Absolute, out p2);
			flag &= p2.Port > 0;
		}
		return flag;
	}
}
