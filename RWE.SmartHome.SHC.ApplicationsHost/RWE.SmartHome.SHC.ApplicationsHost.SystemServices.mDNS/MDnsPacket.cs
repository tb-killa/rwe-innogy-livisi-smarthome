using System;
using System.Collections.Generic;
using System.Linq;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;

public class MDnsPacket
{
	public byte[] PacketData;

	public MDnsHeader Header;

	public List<MDnsQuestion> Questions;

	public List<MDnsResourceRecords> Answers;

	public List<MDnsResourceRecords> DomainAuthorities;

	public List<MDnsResourceRecords> AdditionalResources;

	public MDnsPacket()
	{
		Header = new MDnsHeader();
		Questions = new List<MDnsQuestion>();
		Answers = new List<MDnsResourceRecords>();
		DomainAuthorities = new List<MDnsResourceRecords>();
		AdditionalResources = new List<MDnsResourceRecords>();
		PacketData = null;
	}

	public MDnsPacket(byte[] data)
	{
		try
		{
			Header = new MDnsHeader(data);
			int currentMessagePointer = 12;
			Questions = MDnsQuestion.FromArray(data, Header.QueryCount, ref currentMessagePointer);
			Answers = MDnsResourceRecords.FromArray(data, Header.AnswerCount, ref currentMessagePointer);
			DomainAuthorities = MDnsResourceRecords.FromArray(data, Header.AuthorityCount, ref currentMessagePointer);
			AdditionalResources = MDnsResourceRecords.FromArray(data, Header.AdditionalResourcesCount, ref currentMessagePointer);
			PacketData = data.Take(currentMessagePointer).ToArray();
		}
		catch (Exception ex)
		{
			throw new Exception("The dns message format is invalid: " + ex.Message);
		}
	}

	public MDnsPacket AsQuery()
	{
		Header.QueryResponse = false;
		Header.AuthoritativeAnswer = false;
		return this;
	}

	public MDnsPacket AsResponse()
	{
		Header.QueryResponse = true;
		Header.AuthoritativeAnswer = true;
		return this;
	}

	public MDnsPacket AddQuestions(IEnumerable<MDnsQuestion> questions)
	{
		Questions.AddRange(questions);
		Header.QueryCount = (ushort)Questions.Count;
		return this;
	}

	public byte[] ToByteArray()
	{
		List<byte[]> responseSections = new List<byte[]>();
		responseSections.Add(Header.ToByteArray());
		Questions.ForEach(delegate(MDnsQuestion q)
		{
			responseSections.Add(q.ToByteArray());
		});
		Answers.ForEach(delegate(MDnsResourceRecords q)
		{
			responseSections.Add(q.ToByteArray());
		});
		DomainAuthorities.ForEach(delegate(MDnsResourceRecords q)
		{
			responseSections.Add(q.ToByteArray());
		});
		AdditionalResources.ForEach(delegate(MDnsResourceRecords q)
		{
			responseSections.Add(q.ToByteArray());
		});
		return responseSections.SelectMany((byte[] section) => section).ToArray();
	}
}
