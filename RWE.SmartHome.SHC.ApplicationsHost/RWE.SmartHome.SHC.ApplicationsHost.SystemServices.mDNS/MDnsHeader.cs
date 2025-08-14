namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;

public class MDnsHeader
{
	private static int requestId = 4096;

	public ushort Id;

	private ushort flags;

	public ushort QueryCount;

	public ushort AnswerCount;

	public ushort AuthorityCount;

	public ushort AdditionalResourcesCount;

	public bool QueryResponse
	{
		get
		{
			return (flags & 0x8000) != 0;
		}
		set
		{
			if (value)
			{
				flags |= 32768;
			}
			else
			{
				flags &= 32767;
			}
		}
	}

	public byte OpCode => (byte)((flags >> 11) & 0xF);

	public bool AuthoritativeAnswer
	{
		get
		{
			return (flags & 0x400) != 0;
		}
		set
		{
			if (value)
			{
				flags |= 1024;
			}
			else
			{
				flags &= 64511;
			}
		}
	}

	public byte RCode
	{
		get
		{
			return (byte)(flags & 0xF);
		}
		set
		{
			flags &= 65520;
			flags |= (ushort)(value & 0xF);
		}
	}

	public MDnsHeader()
	{
		Id = (ushort)requestId++;
		flags = 0;
		QueryCount = 0;
		AnswerCount = 0;
		AuthorityCount = 0;
		AdditionalResourcesCount = 0;
	}

	public MDnsHeader(byte[] dnsqueryByteArray)
	{
		int currentOffset = 0;
		Id = MDnsParserHelpers.ParseUInt16(dnsqueryByteArray, ref currentOffset);
		flags = MDnsParserHelpers.ParseUInt16(dnsqueryByteArray, ref currentOffset);
		QueryCount = MDnsParserHelpers.ParseUInt16(dnsqueryByteArray, ref currentOffset);
		AnswerCount = MDnsParserHelpers.ParseUInt16(dnsqueryByteArray, ref currentOffset);
		AuthorityCount = MDnsParserHelpers.ParseUInt16(dnsqueryByteArray, ref currentOffset);
		AdditionalResourcesCount = MDnsParserHelpers.ParseUInt16(dnsqueryByteArray, ref currentOffset);
	}

	public byte[] ToByteArray()
	{
		return new byte[12]
		{
			(byte)((Id & 0xFF00) >> 8),
			(byte)(Id & 0xFF),
			(byte)((flags & 0xFF00) >> 8),
			(byte)(flags & 0xFF),
			(byte)((QueryCount & 0xFF00) >> 8),
			(byte)(QueryCount & 0xFF),
			(byte)((AnswerCount & 0xFF00) >> 8),
			(byte)(AnswerCount & 0xFF),
			(byte)((AuthorityCount & 0xFF00) >> 8),
			(byte)(AuthorityCount & 0xFF),
			(byte)((AdditionalResourcesCount & 0xFF00) >> 8),
			(byte)(AdditionalResourcesCount & 0xFF)
		};
	}
}
