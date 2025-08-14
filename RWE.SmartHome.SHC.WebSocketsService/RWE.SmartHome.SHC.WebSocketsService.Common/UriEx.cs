using System;

namespace RWE.SmartHome.SHC.WebSocketsService.Common;

public class UriEx
{
	protected enum Flags
	{
		LoopbackHost = 0x400000
	}

	public const int HttpDefaultPort = 80;

	public const int HttpsDefaultPort = 443;

	protected const int UnknownPort = -1;

	protected UriHostNameType m_hostNameType;

	protected int m_port = -1;

	protected int m_Flags;

	protected string m_AbsolutePath;

	protected string m_OriginalUriString;

	protected string m_scheme;

	protected string m_host = "";

	protected bool m_isAbsoluteUri;

	protected bool m_isUnc;

	public UriHostNameType HostNameType => m_hostNameType;

	public int Port
	{
		get
		{
			if (!m_isAbsoluteUri)
			{
				throw new InvalidOperationException();
			}
			return m_port;
		}
	}

	public bool IsAbsoluteUri => m_isAbsoluteUri;

	public bool IsUnc
	{
		get
		{
			if (!m_isAbsoluteUri)
			{
				throw new InvalidOperationException();
			}
			return m_isUnc;
		}
	}

	public string AbsolutePath
	{
		get
		{
			if (!m_isAbsoluteUri)
			{
				throw new InvalidOperationException();
			}
			return m_AbsolutePath;
		}
	}

	public string OriginalString => m_OriginalUriString;

	public string AbsoluteUri
	{
		get
		{
			if (!m_isAbsoluteUri)
			{
				throw new InvalidOperationException();
			}
			return m_OriginalUriString;
		}
	}

	public string Scheme
	{
		get
		{
			if (!m_isAbsoluteUri)
			{
				throw new InvalidOperationException();
			}
			return m_scheme;
		}
	}

	public string Host => m_host;

	public bool IsLoopback => (m_Flags & 0x400000) != 0;

	public UriEx(string uriString)
	{
		ConstructAbsoluteUri(ref uriString);
	}

	protected void ConstructAbsoluteUri(ref string uriString)
	{
		ParseUriString(ref uriString);
		m_OriginalUriString = uriString;
	}

	public UriEx(string uriString, UriKind kind)
	{
		switch (kind)
		{
		case UriKind.Absolute:
			ConstructAbsoluteUri(ref uriString);
			break;
		case UriKind.RelativeOrAbsolute:
			throw new ArgumentException();
		case UriKind.Relative:
			ValidateUriPart(uriString, 0);
			m_OriginalUriString = uriString;
			break;
		}
		m_OriginalUriString = uriString;
	}

	protected void ValidateUriPart(string uriString, int startIndex)
	{
		int num = uriString.Length - startIndex;
		for (int i = startIndex; i < num; i++)
		{
			char c = uriString[i];
			if (c < ' ')
			{
				throw new ArgumentException("Invalid char: " + c);
			}
			if (c != '%')
			{
				continue;
			}
			if (num - i < 3)
			{
				throw new ArgumentException("No data after %");
			}
			for (int j = 1; j < 3; j++)
			{
				char c2 = uriString[i + j];
				if ((c2 < '0' || c2 > '9') && (c2 < 'A' || c2 > 'F') && (c2 < 'a' || c2 > 'f'))
				{
					throw new ArgumentException("Invalid char after %: " + c);
				}
			}
			i += 2;
		}
	}

	protected void ParseUriString(ref string uriString)
	{
		int startIndex = 0;
		int num = 0;
		if (uriString == null || uriString.Length == 0)
		{
			throw new ArgumentNullException();
		}
		if (uriString.IndexOf(':') == -1)
		{
			throw new ArgumentException();
		}
		string text = uriString.ToLower();
		if (text.IndexOf("urn:", startIndex) == 0)
		{
			ValidateUrn(uriString);
			return;
		}
		if (uriString[0] == '/')
		{
			ValidateRelativePath(uriString);
			return;
		}
		num = uriString.IndexOf(':');
		m_scheme = uriString.Substring(0, num);
		if (!IsAlpha(m_scheme[0]))
		{
			throw new ArgumentException();
		}
		for (int i = 1; i < m_scheme.Length; i++)
		{
			if (!IsAlphaNumeric(m_scheme[i]) && m_scheme[i] != '+' && m_scheme[i] != '-' && m_scheme[i] != '.')
			{
				throw new ArgumentException();
			}
		}
		startIndex = num + 1;
		string text2 = m_scheme.ToLower();
		switch (text2)
		{
		case "http":
		case "https":
		case "ws":
		case "wss":
			if (uriString.Substring(startIndex).IndexOf("//") == 0)
			{
				m_host = "";
				startIndex += 2;
				if (uriString[startIndex] == '[')
				{
					if (-1 == (num = uriString.IndexOf(']', startIndex)))
					{
						throw new ArgumentException();
					}
					num++;
					m_host = uriString.Substring(startIndex, num - startIndex);
					startIndex = num;
				}
				else if ((num = uriString.IndexOf(':', startIndex)) != -1)
				{
					m_host = uriString.Substring(startIndex, num - startIndex);
					startIndex = num;
				}
				if (m_host != "")
				{
					num = uriString.IndexOf('/', startIndex);
					if (num == -1)
					{
						uriString += '/';
						num = uriString.IndexOf('/', startIndex);
					}
					startIndex++;
					int num2 = num - startIndex;
					m_port = Convert.ToInt32(uriString.Substring(startIndex, num2));
					startIndex += num2;
				}
				else
				{
					if ((num = uriString.IndexOf('/', startIndex)) == -1)
					{
						uriString += '/';
						num = uriString.IndexOf('/', startIndex);
					}
					m_host = uriString.Substring(startIndex, num - startIndex);
					startIndex += m_host.Length;
					switch (text2)
					{
					case "http":
					case "ws":
						m_port = 80;
						break;
					case "https":
					case "wss":
						m_port = 443;
						break;
					}
				}
				m_isAbsoluteUri = true;
				if (m_host[0] == '[')
				{
					m_hostNameType = UriHostNameType.IPv6;
				}
				else if (IsIPv4(m_host))
				{
					m_hostNameType = UriHostNameType.IPv4;
				}
				else
				{
					m_hostNameType = UriHostNameType.Dns;
				}
				break;
			}
			throw new ArgumentException();
		default:
			if (m_scheme == null)
			{
				throw new ArgumentException();
			}
			ValidateUriPart(uriString, startIndex);
			m_hostNameType = UriHostNameType.Unknown;
			m_isAbsoluteUri = true;
			break;
		}
		m_AbsolutePath = uriString.Substring(startIndex, uriString.Length - startIndex);
		if (m_host != null)
		{
			string text3 = m_host.ToLower();
			if (text3 == "localhost" || text3 == "loopback")
			{
				m_Flags |= m_Flags | 0x400000;
			}
		}
		m_isUnc = false;
	}

	protected bool IsIPv4(string host)
	{
		int num = 0;
		int num2 = 0;
		bool flag = false;
		int length = host.Length;
		for (int i = 0; i < length; i++)
		{
			char c = host[i];
			if (c <= '9' && c >= '0')
			{
				flag = true;
				num2 = num2 * 10 + (host[i] - 48);
				if (num2 > 255)
				{
					return false;
				}
				continue;
			}
			if (c == '.')
			{
				if (!flag)
				{
					return false;
				}
				num++;
				flag = false;
				num2 = 0;
				continue;
			}
			return false;
		}
		if (num == 3)
		{
			return flag;
		}
		return false;
	}

	protected void ValidateUrn(string uri)
	{
		bool flag = false;
		if (uri.ToLower().IndexOf("urn:uuid:", 0) == 0)
		{
			char[] array = uri.Substring(9).ToLower().ToCharArray();
			int num = array.Length;
			int num2 = 0;
			int[] array2 = new int[4] { 8, 13, 18, 23 };
			for (int i = 0; i < num; i++)
			{
				if (!IsHex(array[i]) && array[i] != '-')
				{
					flag = true;
					break;
				}
				if (array[i] == '-')
				{
					if (num2 > 3)
					{
						flag = true;
						break;
					}
					if (i != array2[num2])
					{
						flag = true;
						break;
					}
					num2++;
				}
			}
			m_AbsolutePath = uri.Substring(4);
		}
		else
		{
			string text = uri.Substring(4).ToLower();
			char[] array3 = text.ToCharArray();
			int num3 = text.IndexOf(':');
			if (num3 == -1)
			{
				throw new ArgumentException();
			}
			int num4 = 0;
			for (num4 = 0; num4 < num3; num4++)
			{
				if (!IsAlphaNumeric(array3[num4]) && array3[num4] != '-')
				{
					flag = true;
					break;
				}
			}
			array3 = text.Substring(num3 + 1).ToCharArray();
			int num5 = array3.Length;
			if (!flag && num5 != 0)
			{
				string text2 = "()+,-.:=@;$_!*'";
				for (num4 = 0; num4 < num5; num4++)
				{
					if (!IsAlphaNumeric(array3[num4]) && !IsHex(array3[num4]) && array3[num4] != '%' && text2.IndexOf(array3[num4]) == -1)
					{
						flag = true;
						break;
					}
				}
				m_AbsolutePath = uri.Substring(4);
			}
		}
		if (flag)
		{
			throw new ArgumentNullException();
		}
		m_host = "";
		m_isAbsoluteUri = true;
		m_isUnc = false;
		m_hostNameType = UriHostNameType.Unknown;
		m_port = -1;
		m_scheme = "urn";
	}

	protected void ValidateRelativePath(string uri)
	{
		if (uri == null || uri.Length == 0)
		{
			throw new ArgumentNullException();
		}
		if (uri[1] == '/')
		{
			throw new ArgumentException();
		}
		for (int i = 1; i < uri.Length; i++)
		{
			if (!IsAlphaNumeric(uri[i]) && "()+,-.:=@;$_!*'".IndexOf(uri[i]) == -1)
			{
				throw new ArgumentException();
			}
		}
		m_AbsolutePath = uri.Substring(1);
		m_host = "";
		m_isAbsoluteUri = false;
		m_isUnc = false;
		m_hostNameType = UriHostNameType.Unknown;
		m_port = -1;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object o)
	{
		return this == (UriEx)o;
	}

	public static bool operator ==(UriEx lhs, UriEx rhs)
	{
		if ((object)lhs == null)
		{
			return (object)rhs == null;
		}
		if ((object)rhs == null)
		{
			return false;
		}
		if (lhs.m_isAbsoluteUri && rhs.m_isAbsoluteUri)
		{
			return lhs.m_AbsolutePath.ToLower() == rhs.m_AbsolutePath.ToLower();
		}
		return lhs.m_OriginalUriString.ToLower() == rhs.m_OriginalUriString.ToLower();
	}

	public static bool operator !=(UriEx lhs, UriEx rhs)
	{
		if ((object)lhs == null)
		{
			return (object)rhs != null;
		}
		if ((object)rhs == null)
		{
			return true;
		}
		if (lhs.m_isAbsoluteUri && rhs.m_isAbsoluteUri)
		{
			return lhs.m_AbsolutePath.ToLower() != rhs.m_AbsolutePath.ToLower();
		}
		return lhs.m_OriginalUriString.ToLower() != rhs.m_OriginalUriString.ToLower();
	}

	protected bool IsAlpha(char testChar)
	{
		if (testChar < 'A' || testChar > 'Z')
		{
			if (testChar >= 'a')
			{
				return testChar <= 'z';
			}
			return false;
		}
		return true;
	}

	protected bool IsAlphaNumeric(char testChar)
	{
		if ((testChar < 'A' || testChar > 'Z') && (testChar < 'a' || testChar > 'z'))
		{
			if (testChar >= '0')
			{
				return testChar <= '9';
			}
			return false;
		}
		return true;
	}

	protected bool IsHex(char testChar)
	{
		if ((testChar < 'A' || testChar > 'F') && (testChar < 'a' || testChar > 'f'))
		{
			if (testChar >= '0')
			{
				return testChar <= '9';
			}
			return false;
		}
		return true;
	}

	public static bool IsWellFormedUriString(string uriString, UriKind uriKind)
	{
		try
		{
			switch (uriKind)
			{
			case UriKind.Absolute:
			{
				UriEx uriEx2 = new UriEx(uriString);
				if (uriEx2.IsAbsoluteUri)
				{
					return true;
				}
				return false;
			}
			case UriKind.Relative:
			{
				UriEx uriEx = new UriEx(uriString, UriKind.Relative);
				if (!uriEx.IsAbsoluteUri)
				{
					return true;
				}
				return false;
			}
			default:
				return false;
			}
		}
		catch
		{
			return false;
		}
	}

	public string GetPath()
	{
		int num = AbsoluteUri.IndexOf("//");
		int num2 = ((num >= 0) ? AbsoluteUri.IndexOf('/', num + 2) : AbsoluteUri.IndexOf('/'));
		int num3 = AbsoluteUri.IndexOf('?');
		int length = AbsoluteUri.Length;
		int num4 = ((num2 >= 0) ? ((num3 >= 0) ? (num3 - num2) : (length - num2)) : 0);
		if (num4 <= 0)
		{
			return "/";
		}
		return AbsoluteUri.Substring(num2, num4);
	}

	public string GetQuery()
	{
		int num = AbsoluteUri.IndexOf("//");
		int num2 = ((num >= 0) ? AbsoluteUri.IndexOf('/', num + 2) : AbsoluteUri.IndexOf('/'));
		int num3 = AbsoluteUri.IndexOf('?');
		int length = AbsoluteUri.Length;
		if (num3 < 0)
		{
			return "";
		}
		return AbsoluteUri.Substring(num3);
	}

	public string GetPathAndQuery()
	{
		int num = AbsoluteUri.IndexOf("//");
		int num2 = ((num >= 0) ? AbsoluteUri.IndexOf('/', num + 2) : AbsoluteUri.IndexOf('/'));
		int num3 = AbsoluteUri.IndexOf('?');
		int length = AbsoluteUri.Length;
		int num4 = ((num2 >= 0) ? ((num3 >= 0) ? (num3 - num2) : (length - num2)) : 0);
		string text = ((num4 > 0) ? AbsoluteUri.Substring(num2, num4) : "/");
		return text + ((num3 >= 0) ? AbsoluteUri.Substring(num3) : "");
	}
}
