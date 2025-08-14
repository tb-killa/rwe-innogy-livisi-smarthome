using System;
using System.Collections.Generic;
using System.Net;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;

public struct MDnsResourceRecords
{
	public MDnsDomainName Name;

	public ushort Type;

	public ushort Class;

	public uint TimeToLive;

	public ushort DataLength;

	public int DataOffsetStart;

	public byte[] Data;

	public MDnsPtrRecord AsPtrRecordData;

	public MDnsSrvRecord AsSrvRecordData;

	public MDnsTxtRecord AsTxtRecordData;

	public MDnsARecord AsARecordData;

	public MDnsAAAARecord AsAAAARecordData;

	public byte[] ToByteArray()
	{
		byte[] array = Name.ToByteArray();
		byte[] array2 = new byte[array.Length + 10 + DataLength];
		Array.Copy(array, 0, array2, 0, array.Length);
		array2[array.Length] = (byte)(0xFF & (Type >> 8));
		array2[array.Length + 1] = (byte)(0xFF & Type);
		array2[array.Length + 2] = (byte)(0xFF & (Class >> 8));
		array2[array.Length + 3] = (byte)(0xFF & Class);
		array2[array.Length + 4] = (byte)(0xFF & (TimeToLive >> 24));
		array2[array.Length + 5] = (byte)(0xFF & (TimeToLive >> 16));
		array2[array.Length + 6] = (byte)(0xFF & (TimeToLive >> 8));
		array2[array.Length + 7] = (byte)(0xFF & TimeToLive);
		array2[array.Length + 8] = (byte)(0xFF & (DataLength >> 8));
		array2[array.Length + 9] = (byte)(0xFF & DataLength);
		Array.Copy(Data, 0, array2, array.Length + 10, DataLength);
		return array2;
	}

	public static List<MDnsResourceRecords> FromArray(byte[] data, int numberOfMessagesFromHeader, ref int currentOffset)
	{
		List<MDnsResourceRecords> list = new List<MDnsResourceRecords>();
		int num = numberOfMessagesFromHeader;
		while (num > 0)
		{
			MDnsResourceRecords item = default(MDnsResourceRecords);
			item.Name = MDnsParserHelpers.ParseDomainName(data, ref currentOffset);
			item.Type = MDnsParserHelpers.ParseUInt16(data, ref currentOffset);
			item.Class = MDnsParserHelpers.ParseUInt16(data, ref currentOffset);
			item.TimeToLive = MDnsParserHelpers.ParseUInt32(data, ref currentOffset);
			item.DataLength = MDnsParserHelpers.ParseUInt16(data, ref currentOffset);
			item.Data = new byte[item.DataLength];
			item.DataOffsetStart = currentOffset;
			Array.Copy(data, currentOffset, item.Data, 0, item.DataLength);
			currentOffset += item.DataLength;
			item.UpdateRecordData(data);
			num--;
			list.Add(item);
		}
		return list;
	}

	private void UpdateRecordData(byte[] data)
	{
		switch (Type)
		{
		case 12:
			ReadPtrData(data);
			break;
		case 33:
			ReadSrvData(data);
			break;
		case 16:
			ReadTxtData(data);
			break;
		case 1:
			ReadAData(data);
			break;
		case 28:
			ReadAAAAData(data);
			break;
		}
	}

	private void ReadSrvData(byte[] data)
	{
		AsSrvRecordData = new MDnsSrvRecord();
		int currentOffset = DataOffsetStart;
		AsSrvRecordData.Priority = MDnsParserHelpers.ParseUInt16(data, ref currentOffset);
		AsSrvRecordData.Weight = MDnsParserHelpers.ParseUInt16(data, ref currentOffset);
		AsSrvRecordData.Port = MDnsParserHelpers.ParseUInt16(data, ref currentOffset);
		AsSrvRecordData.Target = MDnsParserHelpers.ParseDomainName(data, ref currentOffset);
	}

	private void ReadPtrData(byte[] data)
	{
		AsPtrRecordData = new MDnsPtrRecord();
		int currentOffset = DataOffsetStart;
		AsPtrRecordData.Domain = MDnsParserHelpers.ParseDomainName(data, ref currentOffset);
	}

	private void ReadTxtData(byte[] data)
	{
		AsTxtRecordData = new MDnsTxtRecord();
		int dataOffsetStart = DataOffsetStart;
		AsTxtRecordData.TxtLines = MDnsParserHelpers.ParseTextLabels(data, dataOffsetStart, DataLength);
	}

	private void ReadAData(byte[] data)
	{
		AsARecordData = new MDnsARecord();
		int dataOffsetStart = DataOffsetStart;
		if (DataLength != 4)
		{
			throw new ArgumentException($"Invalid size for A Record: {DataLength}/4");
		}
		byte[] array = new byte[4];
		Array.Copy(data, dataOffsetStart, array, 0, 4);
		AsARecordData.IPAddress = new IPAddress(array);
	}

	private void ReadAAAAData(byte[] data)
	{
		AsAAAARecordData = new MDnsAAAARecord();
		int dataOffsetStart = DataOffsetStart;
		byte[] array = new byte[DataLength];
		Array.Copy(data, dataOffsetStart, array, 0, DataLength);
		AsAAAARecordData.IPAddress = new IPAddress(array);
	}

	public override string ToString()
	{
		return $"Name [{Name.ToString()}], Type {Type} Class {Class} TTL: {TimeToLive} DataLength {DataLength} Data: {MDnsParserHelpers.ToReadable(Data, DataLength)}" + $" {AsPtrRecordData}{AsSrvRecordData}{AsTxtRecordData}{AsARecordData}{AsAAAARecordData}";
	}
}
