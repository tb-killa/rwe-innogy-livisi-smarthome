using System;
using System.Collections.Generic;
using System.Text;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;

public class MDnsQuestion
{
	public MDnsDomainName QName;

	public ushort QType;

	public ushort QClass;

	public byte[] ToByteArray()
	{
		byte[] array = QName.ToByteArray();
		byte[] array2 = new byte[array.Length + 4];
		Array.Copy(array, 0, array2, 0, array.Length);
		array2[array2.Length - 4] = (byte)((QType & 0xFF00) >> 8);
		array2[array2.Length - 3] = (byte)(QType & 0xFF);
		array2[array2.Length - 2] = (byte)((QClass & 0xFF00) >> 8);
		array2[array2.Length - 1] = (byte)(QClass & 0xFF);
		return array2;
	}

	public override string ToString()
	{
		return "Question: [" + QName.ToString() + "] Type: [" + Encoding.ASCII.GetString(BitConverter.GetBytes(QType), 0, 2) + "] Class: [" + Encoding.ASCII.GetString(BitConverter.GetBytes(QClass), 0, 2) + "]";
	}

	public static List<MDnsQuestion> FromArray(byte[] dnsqueryByteArray, int questionCount, ref int currentMessagePointer)
	{
		List<MDnsQuestion> list = new List<MDnsQuestion>();
		for (int num = questionCount; num > 0; num--)
		{
			MDnsQuestion mDnsQuestion = new MDnsQuestion();
			mDnsQuestion.QName = MDnsParserHelpers.ParseDomainName(dnsqueryByteArray, ref currentMessagePointer);
			mDnsQuestion.QType = MDnsParserHelpers.ParseUInt16(dnsqueryByteArray, ref currentMessagePointer);
			mDnsQuestion.QClass = MDnsParserHelpers.ParseUInt16(dnsqueryByteArray, ref currentMessagePointer);
			list.Add(mDnsQuestion);
		}
		return list;
	}
}
