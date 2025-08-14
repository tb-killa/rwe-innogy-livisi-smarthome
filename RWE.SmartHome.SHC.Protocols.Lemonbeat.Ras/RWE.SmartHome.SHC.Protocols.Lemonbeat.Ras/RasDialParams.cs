namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.Ras;

internal class RasDialParams
{
	public byte[] Data { get; set; }

	public string EntryName
	{
		get
		{
			return Data.GetString(4, 20);
		}
		set
		{
			Data.SetString(4, value);
		}
	}

	public string UserName
	{
		get
		{
			return Data.GetString(402, 128);
		}
		set
		{
			Data.SetString(402, value);
		}
	}

	public string Password
	{
		get
		{
			return Data.GetString(916, 256);
		}
		set
		{
			Data.SetString(916, value);
		}
	}

	public RasDialParams()
	{
		int dialParamsSize = NativeRasWrapper.DialParamsSize;
		Data = new byte[dialParamsSize];
		Data.SetUInt(0, (uint)dialParamsSize);
	}

	public RasDialParams(byte[] buffer)
	{
		Data = buffer;
	}
}
