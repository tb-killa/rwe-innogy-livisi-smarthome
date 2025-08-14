using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using SmartHome.SHC.API.SystemServices.Dns;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;

internal class MDnsResolver : IDnsResolver
{
	private MDnsSeeker seeker;

	public event EventHandler<DnsPacketReceivedEventArgs> DnsPacketReceived;

	public void ForceDiscoveryFinished()
	{
		MDnsSeeker mDnsSeeker = seeker;
		if (mDnsSeeker != null)
		{
			mDnsSeeker.ForceDiscoveryFinished = true;
		}
	}

	public void Resolve(IEnumerable<DnsQuery> queries, TimeSpan duration)
	{
		seeker = new MDnsSeeker();
		seeker.MDNSPacketReceived += OnMDNSPacketReceived;
		seeker.Resolve(queries.Select((DnsQuery q) => ToInternalQuestion(q)), duration);
	}

	private static MDnsQuestion ToInternalQuestion(DnsQuery q)
	{
		MDnsQuestion mDnsQuestion = new MDnsQuestion();
		mDnsQuestion.QClass = q.DnsClass;
		mDnsQuestion.QName = new MDnsDomainName(q.Query);
		mDnsQuestion.QType = q.DnsType;
		return mDnsQuestion;
	}

	private void OnMDNSPacketReceived(MDnsPacket packet)
	{
		EventHandler<DnsPacketReceivedEventArgs> dnsPacketReceived = this.DnsPacketReceived;
		if (dnsPacketReceived != null)
		{
			DnsPacketReceivedEventArgs e = new DnsPacketReceivedEventArgs(new DnsPacket
			{
				Answers = packet.Answers.Select((MDnsResourceRecords a) => ToDnsResourceRecord(a)).ToArray(),
				Authority = packet.DomainAuthorities.Select((MDnsResourceRecords a) => ToDnsResourceRecord(a)).ToArray(),
				AdditionalInformation = packet.AdditionalResources.Select((MDnsResourceRecords a) => ToDnsResourceRecord(a)).ToArray(),
				Queries = packet.Questions.Select((MDnsQuestion q) => ToDnsQuery(q)).ToArray()
			});
			try
			{
				dnsPacketReceived(this, e);
			}
			catch (Exception arg)
			{
				Log.Warning(Module.ApplicationsHost, $"MDNS parser: error on packet receiver: {arg}");
			}
		}
	}

	private DnsQuery ToDnsQuery(MDnsQuestion q)
	{
		return new DnsQuery
		{
			DnsClass = q.QClass,
			DnsType = q.QType,
			Query = q.QName.Name
		};
	}

	private static DnsResourceRecord ToDnsResourceRecord(MDnsResourceRecords rec)
	{
		DnsResourceRecord result = new DnsResourceRecord
		{
			Class = rec.Class,
			Type = rec.Type,
			DomainName = rec.Name.Name,
			RawData = rec.Data,
			TimeToLive = rec.TimeToLive,
			TranslatedData = null
		};
		try
		{
			switch (rec.Type)
			{
			case 12:
				result.TranslatedData = rec.AsPtrRecordData.Domain.Name;
				break;
			case 33:
			{
				MDnsSrvRecord asSrvRecordData = rec.AsSrvRecordData;
				result.TranslatedData = new DnsServiceRecordSpecificData
				{
					Port = asSrvRecordData.Port,
					Priority = asSrvRecordData.Priority,
					TargetDomainName = asSrvRecordData.Target.Name,
					Weight = asSrvRecordData.Weight
				};
				break;
			}
			case 16:
				result.TranslatedData = rec.AsTxtRecordData.TxtLines.ToArray();
				break;
			case 1:
				result.TranslatedData = rec.AsARecordData.IPAddress;
				break;
			case 28:
				result.TranslatedData = rec.AsAAAARecordData.IPAddress;
				break;
			}
		}
		catch (Exception arg)
		{
			Log.Debug(Module.ApplicationsHost, $"MDNS parser: error reading translated data: {arg}");
		}
		return result;
	}
}
