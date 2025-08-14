namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal class RasEntry
{
	public const int Ppp = 1;

	public byte[] Data { get; set; }

	public OptionFlags Options
	{
		get
		{
			return (OptionFlags)Data.GetUInt(4);
		}
		set
		{
			Data.SetUInt(4, (uint)value);
		}
	}

	public uint CountryId
	{
		get
		{
			return Data.GetUInt(8);
		}
		set
		{
			Data.SetUInt(8, value);
		}
	}

	public uint CountryCode
	{
		get
		{
			return Data.GetUInt(12);
		}
		set
		{
			Data.SetUInt(12, value);
		}
	}

	public uint AlternatesOffset
	{
		get
		{
			return Data.GetUInt(296);
		}
		set
		{
			Data.SetUInt(296, value);
		}
	}

	public uint IPAddr
	{
		get
		{
			return Data.GetUInt(300);
		}
		set
		{
			Data.SetUInt(300, value);
		}
	}

	public uint IPAddrDns
	{
		get
		{
			return Data.GetUInt(304);
		}
		set
		{
			Data.SetUInt(304, value);
		}
	}

	public uint IPAddrDnsAlt
	{
		get
		{
			return Data.GetUInt(308);
		}
		set
		{
			Data.SetUInt(308, value);
		}
	}

	public uint IPAddrWins
	{
		get
		{
			return Data.GetUInt(312);
		}
		set
		{
			Data.SetUInt(312, value);
		}
	}

	public uint IPAddrWinsAlt
	{
		get
		{
			return Data.GetUInt(316);
		}
		set
		{
			Data.SetUInt(316, value);
		}
	}

	public uint FrameSize
	{
		get
		{
			return Data.GetUInt(320);
		}
		set
		{
			Data.SetUInt(320, value);
		}
	}

	public uint NetProtocols
	{
		get
		{
			return Data.GetUInt(324);
		}
		set
		{
			Data.SetUInt(324, value);
		}
	}

	public uint FramingProtocol
	{
		get
		{
			return Data.GetUInt(328);
		}
		set
		{
			Data.SetUInt(328, value);
		}
	}

	public string AreaCode
	{
		get
		{
			return Data.GetString(16, 10);
		}
		set
		{
			Data.SetString(16, value);
		}
	}

	public string LocalPhoneNumber
	{
		get
		{
			return Data.GetString(38, 128);
		}
		set
		{
			Data.SetString(38, value);
		}
	}

	public string Script
	{
		get
		{
			return Data.GetString(332, 259);
		}
		set
		{
			Data.SetString(332, value);
		}
	}

	public string AutoDialDll
	{
		get
		{
			return Data.GetString(852, 259);
		}
		set
		{
			Data.SetString(852, value);
		}
	}

	public string AutoDialFunc
	{
		get
		{
			return Data.GetString(1372, 259);
		}
		set
		{
			Data.SetString(1372, value);
		}
	}

	public string DeviceType
	{
		get
		{
			return Data.GetString(1892, 16);
		}
		set
		{
			Data.SetString(1892, value);
		}
	}

	public string DeviceName
	{
		get
		{
			return Data.GetString(1926, 128);
		}
		set
		{
			Data.SetString(1926, value);
		}
	}

	public RasEntry()
	{
		int rasEntrySize = NativeRasWrapper.RasEntrySize;
		Data = new byte[rasEntrySize];
		Data.SetUInt(0, (uint)rasEntrySize);
	}

	public RasEntry(byte[] buffer)
	{
		Data = buffer;
	}
}
