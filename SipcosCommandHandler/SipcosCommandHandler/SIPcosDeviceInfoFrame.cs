using System;
using System.Collections.Generic;
using System.Linq;
using SerialAPI;

namespace SipcosCommandHandler;

public class SIPcosDeviceInfoFrame : SIPCOSMessage
{
	private string m_info;

	private SIPcosNetworkManagementFrameType m_frameType;

	private DeviceInfoCommunicationMedia m_communicationMedia;

	private DeviceInfoStackType m_stackType;

	private byte[] m_sgtin;

	private DeviceInfoManufacturerCode m_manufacturerCode;

	private byte[] m_manufacturerDeviceType;

	private DeviceInfoGenericDeviceProperties m_genericDeviceProperties;

	private byte m_manufacturerDeviceAndFirmware;

	private byte m_testStatus;

	private byte m_numberOfChannels;

	private DeviceInfoOperationModes m_operationModes;

	private DeviceInfoInclusionModes m_inclusionModes;

	private byte[] m_securitySequenceNumber;

	private byte[] m_securityMIC32;

	private byte[] m_deviceIP;

	private DeviceInfoProtocolType m_protocolType;

	private SIPcosDeviceInfoAdditionalData m_additionalData;

	public SIPcosNetworkManagementFrameType FrameType
	{
		get
		{
			return m_frameType;
		}
		set
		{
			m_frameType = value;
		}
	}

	public DeviceInfoCommunicationMedia CommunicationMedia
	{
		get
		{
			return m_communicationMedia;
		}
		set
		{
			m_communicationMedia = value;
		}
	}

	public DeviceInfoStackType StackType
	{
		get
		{
			return m_stackType;
		}
		set
		{
			m_stackType = value;
		}
	}

	public byte[] SGTIN
	{
		get
		{
			return m_sgtin;
		}
		set
		{
			m_sgtin = value;
		}
	}

	public DeviceInfoManufacturerCode ManufacturerCode
	{
		get
		{
			return m_manufacturerCode;
		}
		set
		{
			m_manufacturerCode = value;
		}
	}

	public byte[] ManufacturerDeviceType
	{
		get
		{
			return m_manufacturerDeviceType;
		}
		set
		{
			m_manufacturerDeviceType = value;
		}
	}

	public DeviceInfoGenericDeviceProperties GenericDeviceProperties
	{
		get
		{
			return m_genericDeviceProperties;
		}
		set
		{
			m_genericDeviceProperties = value;
		}
	}

	public byte ManufacturerDeviceAndFirmware
	{
		get
		{
			return m_manufacturerDeviceAndFirmware;
		}
		set
		{
			m_manufacturerDeviceAndFirmware = value;
		}
	}

	public byte TestStatus
	{
		get
		{
			return m_testStatus;
		}
		set
		{
			m_testStatus = value;
		}
	}

	public byte NumberOfChannels
	{
		get
		{
			return m_numberOfChannels;
		}
		set
		{
			m_numberOfChannels = value;
		}
	}

	public DeviceInfoOperationModes OperationMode
	{
		get
		{
			return m_operationModes;
		}
		set
		{
			m_operationModes = value;
		}
	}

	public DeviceInfoInclusionModes InclusionModes
	{
		get
		{
			return m_inclusionModes;
		}
		set
		{
			m_inclusionModes = value;
		}
	}

	public byte[] SecuritySequenceNumber
	{
		get
		{
			return m_securitySequenceNumber;
		}
		set
		{
			m_securitySequenceNumber = value;
		}
	}

	public byte[] SecurityMIC32
	{
		get
		{
			return m_securityMIC32;
		}
		set
		{
			m_securityMIC32 = value;
		}
	}

	public byte[] NewDeviceIP
	{
		get
		{
			return m_deviceIP;
		}
		set
		{
			m_deviceIP = value;
		}
	}

	public DeviceInfoProtocolType ProtocolType
	{
		get
		{
			return m_protocolType;
		}
		set
		{
			m_protocolType = value;
		}
	}

	public SIPcosDeviceInfoAdditionalData AdditionalData
	{
		get
		{
			return m_additionalData;
		}
		set
		{
			m_additionalData = value;
		}
	}

	public SIPcosDeviceInfoFrame(SIPcosHeader header)
		: base(header, new List<byte>())
	{
	}

	internal bool parse(ref List<byte> message)
	{
		if (message.Count < 34)
		{
			Console.WriteLine("CosIP Network management frame invalid; rejected frame for length: {0}; expected 34", message.Count);
			return false;
		}
		m_message.Clear();
		m_message.AddRange(message);
		if (message.Count > 0)
		{
			m_frameType = (SIPcosNetworkManagementFrameType)message[0];
			message.RemoveAt(0);
		}
		if (message.Count > 0)
		{
			m_communicationMedia = (DeviceInfoCommunicationMedia)(message[0] & 0xF);
			m_stackType = (DeviceInfoStackType)((message[0] & 0xC0) >> 6);
			message.RemoveAt(0);
		}
		m_sgtin = new byte[12];
		if (message.Count > 12)
		{
			for (int i = 0; i < 12; i++)
			{
				m_sgtin[i] = message[i];
			}
			message.RemoveRange(0, 12);
		}
		if (message.Count > 1)
		{
			m_manufacturerCode = (DeviceInfoManufacturerCode)((message[0] << 8) | message[1]);
			message.RemoveRange(0, 2);
		}
		if (message.Count > 3)
		{
			m_manufacturerDeviceType = new byte[4];
			m_manufacturerDeviceType[0] = message[0];
			m_manufacturerDeviceType[1] = message[1];
			m_manufacturerDeviceType[2] = message[2];
			m_manufacturerDeviceType[3] = message[3];
			message.RemoveRange(0, 4);
		}
		if (message.Count > 1)
		{
			m_genericDeviceProperties = (DeviceInfoGenericDeviceProperties)(message[0] | (message[1] << 8));
			message.RemoveRange(0, 2);
		}
		if (message.Count > 0)
		{
			m_manufacturerDeviceAndFirmware = message[0];
			message.RemoveAt(0);
		}
		if (message.Count > 0)
		{
			m_testStatus = message[0];
			message.RemoveAt(0);
		}
		if (message.Count > 0)
		{
			m_numberOfChannels = message[0];
			message.RemoveAt(0);
		}
		if (message.Count > 1)
		{
			m_operationModes = (DeviceInfoOperationModes)message[0];
			m_inclusionModes = (DeviceInfoInclusionModes)message[1];
			message.RemoveRange(0, 2);
		}
		if (message.Count > 3)
		{
			m_securitySequenceNumber = new byte[4];
			m_securitySequenceNumber[0] = message[0];
			m_securitySequenceNumber[1] = message[1];
			m_securitySequenceNumber[2] = message[2];
			m_securitySequenceNumber[3] = message[3];
			message.RemoveRange(0, 4);
		}
		if (message.Count > 3)
		{
			m_securityMIC32 = new byte[4];
			m_securityMIC32[0] = message[0];
			m_securityMIC32[1] = message[1];
			m_securityMIC32[2] = message[2];
			m_securityMIC32[3] = message[3];
			message.RemoveRange(0, 4);
		}
		m_deviceIP = new byte[3];
		if (m_frameType == SIPcosNetworkManagementFrameType.ForwardedDeviceInfoFrame)
		{
			if (message.Count > 3)
			{
				m_deviceIP[0] = message[1];
				m_deviceIP[1] = message[2];
				m_deviceIP[2] = message[3];
				message.RemoveRange(0, 4);
			}
		}
		else
		{
			m_deviceIP = base.Header.MacSource;
		}
		if (m_header.SyncWord.SequenceEqual(new byte[2] { 233, 202 }))
		{
			m_protocolType = DeviceInfoProtocolType.BIDcos;
			m_additionalData = new SIPcosDeviceInfoAdditionalData();
			m_additionalData.DeviceTypeNumber[0] = m_sgtin[0];
			m_additionalData.DeviceTypeNumber[1] = m_sgtin[1];
			m_additionalData.SerialNumber[0] = m_sgtin[2];
			m_additionalData.SerialNumber[1] = m_sgtin[3];
			m_additionalData.SerialNumber[2] = m_sgtin[4];
			m_additionalData.SerialNumber[3] = m_sgtin[5];
			m_additionalData.SerialNumber[4] = m_sgtin[6];
			m_additionalData.SerialNumber[5] = m_sgtin[7];
			m_additionalData.SerialNumber[6] = m_sgtin[8];
			m_additionalData.SerialNumber[7] = m_sgtin[9];
			m_additionalData.SerialNumber[8] = m_sgtin[10];
			m_additionalData.SerialNumber[9] = m_sgtin[11];
		}
		return true;
	}

	public override string ToString()
	{
		string text = "N/A";
		if (m_deviceIP != null)
		{
			text = $"{m_deviceIP[0]:X2} {m_deviceIP[1]:X2} {m_deviceIP[2]:X2}";
			if (m_frameType == SIPcosNetworkManagementFrameType.ForwardedDeviceInfoFrame)
			{
				text += " (F)";
			}
		}
		return text;
	}

	public string GetInfo()
	{
		if (m_info == null)
		{
			GenerateStringInfo();
		}
		return m_info;
	}

	private void GenerateStringInfo()
	{
		m_info = "";
		if (m_frameType == SIPcosNetworkManagementFrameType.ForwardedDeviceInfoFrame)
		{
			m_info += "Forwarded ";
		}
		m_info += "Device Info Frame:\n";
		m_info = m_info + "Media: " + m_communicationMedia.ToString() + "\n";
		m_info = m_info + "Type: " + m_stackType.ToString() + "\n";
		m_info = m_info + "Manufacturer: " + m_manufacturerCode.ToString() + "\n";
		if (m_manufacturerDeviceType != null)
		{
			m_info += "Device Type: ";
			for (int i = 0; i < m_manufacturerDeviceType.Length; i++)
			{
				m_info += $"{m_manufacturerDeviceType[i]:X2} ";
			}
			m_info += "\n";
		}
		m_info += $"Firmware: {m_manufacturerDeviceAndFirmware:X2}\n";
		m_info += $"Test Status: {m_testStatus:X2}\n";
		m_info = m_info + "Number Of Channels: " + m_numberOfChannels + "\n";
		m_info += "Properties: \n";
		m_info = m_info + m_genericDeviceProperties.ToString() + "\n";
		m_info += "Operation Modes:\n";
		m_info = m_info + m_operationModes.ToString() + "\n";
		m_info += "InclusionModes: \n";
		m_info = m_info + m_inclusionModes.ToString() + "\n";
		m_info += "SGTIN: ";
		if (m_sgtin != null)
		{
			for (int j = 0; j < m_sgtin.Length; j++)
			{
				m_info += $"{m_sgtin[j]:X2} ";
			}
		}
		m_info += "\nNonce: ";
		if (m_securitySequenceNumber != null)
		{
			for (int k = 0; k < m_securitySequenceNumber.Length; k++)
			{
				m_info += $"{m_securitySequenceNumber[k]:X2} ";
			}
		}
		m_info += "\nMIC32: ";
		if (m_securityMIC32 != null)
		{
			for (int l = 0; l < m_securityMIC32.Length; l++)
			{
				m_info += $"{m_securityMIC32[l]:X2} ";
			}
		}
	}

	public override bool Equals(object obj)
	{
		if (obj != null && obj is SIPcosDeviceInfoFrame)
		{
			SIPcosDeviceInfoFrame sIPcosDeviceInfoFrame = (SIPcosDeviceInfoFrame)obj;
			if (sIPcosDeviceInfoFrame.m_securitySequenceNumber != null && m_securitySequenceNumber != null && sIPcosDeviceInfoFrame.m_deviceIP != null && m_deviceIP != null && m_securitySequenceNumber[0] == sIPcosDeviceInfoFrame.m_securitySequenceNumber[0] && m_securitySequenceNumber[1] == sIPcosDeviceInfoFrame.m_securitySequenceNumber[1] && m_securitySequenceNumber[2] == sIPcosDeviceInfoFrame.m_securitySequenceNumber[2] && m_securitySequenceNumber[3] == sIPcosDeviceInfoFrame.m_securitySequenceNumber[3] && m_deviceIP[0] == sIPcosDeviceInfoFrame.m_deviceIP[0] && m_deviceIP[1] == sIPcosDeviceInfoFrame.m_deviceIP[1] && m_deviceIP[2] == sIPcosDeviceInfoFrame.m_deviceIP[2] && m_frameType == sIPcosDeviceInfoFrame.m_frameType)
			{
				return true;
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
