using System;
using System.Globalization;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration.DevicesDefinitions;

public class SipcosFirmwareVersion : FirmwareVersion
{
	private readonly byte versionNumber;

	public SipcosFirmwareVersion(byte versionNumber)
	{
		this.versionNumber = versionNumber;
	}

	public SipcosFirmwareVersion(string versionString)
	{
		versionNumber = (byte)int.Parse(versionString.Replace(".", string.Empty), NumberStyles.HexNumber);
	}

	public override int CompareTo(object obj)
	{
		if (!(obj is SipcosFirmwareVersion sipcosFirmwareVersion))
		{
			throw new Exception("Incompatible comparision parameter.");
		}
		return versionNumber.CompareTo(sipcosFirmwareVersion.versionNumber);
	}
}
