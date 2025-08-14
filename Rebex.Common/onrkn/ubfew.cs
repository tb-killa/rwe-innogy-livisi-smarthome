using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace onrkn;

internal class ubfew
{
	private const string xqzky = "OID.";

	private rmkkr aicgt;

	private static ubfew[] pedak;

	private static Dictionary<string, ubfew> uqvmw;

	private static Dictionary<string, ubfew> gotwu;

	private string wzsfx;

	private string rnivy;

	private rmkkr dulrk;

	private string[] yweuo;

	public string ahwky
	{
		get
		{
			return wzsfx;
		}
		private set
		{
			wzsfx = value;
		}
	}

	public string wmofh
	{
		get
		{
			return rnivy;
		}
		private set
		{
			rnivy = value;
		}
	}

	public rmkkr rmtmt
	{
		get
		{
			return dulrk;
		}
		set
		{
			dulrk = value;
		}
	}

	public string[] qqymz
	{
		get
		{
			return yweuo;
		}
		private set
		{
			yweuo = value;
		}
	}

	static ubfew()
	{
		pedak = new ubfew[39]
		{
			new ubfew("2.5.4.15", "BusinessCategory"),
			new ubfew("2.5.4.6", "C"),
			new ubfew("2.5.4.3", "CN"),
			new ubfew("0.9.2342.19200300.100.1.25", "DC", rmkkr.dzwiy),
			new ubfew("2.5.4.13", "Description"),
			new ubfew("2.5.4.27", "DestinationIndicator"),
			new ubfew("2.5.4.46", "dnQualifier"),
			new ubfew("2.5.4.23", "FacsimileTelephoneNumber"),
			new ubfew("2.5.4.44", "GenerationQualifier"),
			new ubfew("2.5.4.42", "G", "GivenName"),
			new ubfew("2.5.4.51", "HouseIdentifier"),
			new ubfew("2.5.4.43", "I", "Initials"),
			new ubfew("2.5.4.25", "InternationalISDNNumber"),
			new ubfew("2.5.4.7", "L"),
			new ubfew("2.5.4.41", "Name"),
			new ubfew("2.5.4.10", "O"),
			new ubfew("2.5.4.11", "OU"),
			new ubfew("2.5.4.19", "PhysicalDeliveryOfficeName"),
			new ubfew("2.5.4.16", "PostalAddress"),
			new ubfew("2.5.4.17", "PostalCode"),
			new ubfew("2.5.4.18", "POBox", "PostOfficeBox"),
			new ubfew("2.5.4.28", "PreferredDeliveryMethod"),
			new ubfew("2.5.4.26", "RegisteredAddress"),
			new ubfew("2.5.4.33", "RoleOccupant"),
			new ubfew("2.5.4.14", "SearchGuide"),
			new ubfew("2.5.4.34", "SeeAlso"),
			new ubfew("2.5.4.5", "SERIALNUMBER"),
			new ubfew("2.5.4.4", "SN"),
			new ubfew("2.5.4.8", "S", "ST"),
			new ubfew("2.5.4.9", "STREET"),
			new ubfew("2.5.4.20", "Phone", "TelephoneNumber"),
			new ubfew("2.5.4.22", "TeletexTerminalIdentifier"),
			new ubfew("2.5.4.21", "TelexNumber"),
			new ubfew("2.5.4.12", "T", "Title"),
			new ubfew("0.9.2342.19200300.100.1.1", "UID"),
			new ubfew("2.5.4.50", "UniqueMember"),
			new ubfew("2.5.4.35", "UserPassword"),
			new ubfew("2.5.4.24", "x21Address", "x121Address"),
			new ubfew("1.2.840.113549.1.9.1", "E", rmkkr.dzwiy)
		};
		uqvmw = new Dictionary<string, ubfew>();
		int num = 0;
		if (num != 0)
		{
			goto IL_0446;
		}
		goto IL_0467;
		IL_0446:
		uqvmw.Add(pedak[num].ahwky, pedak[num]);
		num++;
		goto IL_0467;
		IL_0467:
		if (num < pedak.Length)
		{
			goto IL_0446;
		}
		gotwu = new Dictionary<string, ubfew>();
		int num2 = 0;
		if (num2 != 0)
		{
			goto IL_0483;
		}
		goto IL_04fb;
		IL_04fb:
		if (num2 >= pedak.Length)
		{
			return;
		}
		goto IL_0483;
		IL_0483:
		gotwu.Add(pedak[num2].wmofh.ToLower(CultureInfo.InvariantCulture), pedak[num2]);
		string[] array = pedak[num2].qqymz;
		int num3 = 0;
		if (num3 != 0)
		{
			goto IL_04bf;
		}
		goto IL_04e7;
		IL_04bf:
		string text = array[num3];
		gotwu.Add(text.ToLower(CultureInfo.InvariantCulture), pedak[num2]);
		num3++;
		goto IL_04e7;
		IL_04e7:
		if (num3 < array.Length)
		{
			goto IL_04bf;
		}
		num2++;
		goto IL_04fb;
	}

	public static ubfew umnjs(string p0)
	{
		string friendlyName;
		if (p0.IndexOf("OID.", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			friendlyName = p0;
			p0 = p0.Substring("OID.".Length);
		}
		else
		{
			friendlyName = "OID." + p0;
		}
		if (uqvmw.ContainsKey(p0) && 0 == 0)
		{
			return uqvmw[p0];
		}
		return new ubfew(p0, friendlyName);
	}

	public static ubfew yzylr(string p0)
	{
		if (gotwu.TryGetValue(p0.ToLower(CultureInfo.InvariantCulture), out var value) && 0 == 0)
		{
			return value;
		}
		throw new ufgee(brgjd.edcru("Unable to transform friendly name to oid: unknown friendly name: {0}.", p0));
	}

	public ubfew(string oid, string friendlyName, params string[] alternateNames)
		: this(oid, friendlyName, rmkkr.jgutu, alternateNames)
	{
	}

	public ubfew(string oid, string friendlyName, rmkkr preferredStringType, params string[] alternateNames)
	{
		ahwky = oid;
		wmofh = friendlyName;
		rmtmt = preferredStringType;
		string[] array = alternateNames;
		if (array == null || 1 == 0)
		{
			array = new string[0];
		}
		qqymz = array;
	}

	public lnabj qbevv(string p0)
	{
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ufgee("Provided data string is not in correct format, missing value in 'Attribute Type and Value' part.");
		}
		if (p0[0] == '#')
		{
			if (p0.Length == 1)
			{
				throw new ufgee("Provided data string is not in correct format, missing hex value in hex string.");
			}
			byte[] data = brgjd.qaycu(p0.Substring(1));
			return new rwolq(data);
		}
		StringBuilder stringBuilder = new StringBuilder();
		rmkkr rmkkr2 = ssrjw(p0, stringBuilder);
		byte[] data2 = ((rmkkr2 != rmkkr.pcxmz) ? Encoding.UTF8.GetBytes(stringBuilder.ToString()) : Encoding.BigEndianUnicode.GetBytes(stringBuilder.ToString()));
		return new vesyi(rmkkr2, data2);
	}

	private rmkkr ssrjw(string p0, StringBuilder p1)
	{
		if (p0[0] == '"' && p0[p0.Length - 1] == '"')
		{
			p0 = p0.Substring(1, p0.Length - 2);
		}
		aicgt = rmtmt;
		int num = 0;
		if (num != 0)
		{
			goto IL_0047;
		}
		goto IL_01de;
		IL_0047:
		if (p0[num] >= '\u007f')
		{
			ekmre(rmkkr.pcxmz);
			p1.Append(p0[num]);
		}
		else if (p0[num] == '\\')
		{
			switch (p0[num + 1])
			{
			case ' ':
			case '"':
			case '#':
			case '+':
			case ',':
			case ';':
			case '<':
			case '=':
			case '>':
			case '\\':
				if (num + 1 >= p0.Length)
				{
					throw new ufgee("Provided data string is not in correct format, unexpected '\\' at the end of string value.");
				}
				p1.Append(p0[++num]);
				break;
			default:
				if (num + 1 >= p0.Length)
				{
					throw new ufgee("Provided data string is not in correct format, invalid use of '\\' as hex pair at the end of string value.");
				}
				ekmre(rmkkr.xiwym);
				p1.Append((char)brgjd.qaycu(p0.Substring(num + 1, 2))[0]);
				num += 2;
				break;
			}
		}
		else if (p0[num] == '"')
		{
			p1.Append(p0[++num]);
		}
		else
		{
			if (kpzln(p0[num]) && 0 == 0)
			{
				ekmre(rmkkr.pcxmz);
			}
			else if ((!zlwsj(p0[num]) || 1 == 0) && (!pxrhk(p0[num]) || 1 == 0) && (!olneo(p0[num]) || 1 == 0))
			{
				ekmre(rmkkr.dzwiy);
			}
			p1.Append(p0[num]);
		}
		num++;
		goto IL_01de;
		IL_01de:
		if (num < p0.Length)
		{
			goto IL_0047;
		}
		return aicgt;
	}

	private static bool zlwsj(char p0)
	{
		if ('a' > p0 || p0 > 'z')
		{
			if ('A' <= p0)
			{
				return p0 <= 'Z';
			}
			return false;
		}
		return true;
	}

	private static bool pxrhk(char p0)
	{
		if ('0' <= p0)
		{
			return p0 <= '9';
		}
		return false;
	}

	public static bool olneo(char p0)
	{
		switch (p0)
		{
		case ' ':
		case '\'':
		case '(':
		case ')':
		case '+':
		case ',':
		case '-':
		case '.':
		case '/':
		case ':':
		case '=':
		case '?':
			return true;
		default:
			return false;
		}
	}

	public static bool kpzln(char p0)
	{
		char c = p0;
		if (c == '&' || c == '_')
		{
			return true;
		}
		return false;
	}

	private void ekmre(rmkkr p0)
	{
		if (aicgt != rmkkr.pcxmz)
		{
			aicgt = p0;
		}
	}
}
