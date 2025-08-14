using Rebex.Mime;
using onrkn;

namespace Rebex.Mail;

public class MailSettings
{
	private MimeOptions jdimx;

	private RtfProcessingMode ueulb;

	internal int tfuau;

	private bool ezgwz;

	private bool diltw;

	private bool dsgdf;

	public bool DoNotParseMimeTree
	{
		get
		{
			return avdyi(MimeOptions.DoNotParseMimeTree);
		}
		set
		{
			snndx(MimeOptions.DoNotParseMimeTree, value);
		}
	}

	public bool ProcessAllHeaders
	{
		get
		{
			return avdyi(MimeOptions.ProcessAllHeaders);
		}
		set
		{
			snndx(MimeOptions.ProcessAllHeaders, value);
		}
	}

	public bool IgnoreUnparsableHeaders
	{
		get
		{
			return avdyi(MimeOptions.IgnoreUnparsableHeaders);
		}
		set
		{
			snndx(MimeOptions.IgnoreUnparsableHeaders, value);
		}
	}

	public bool AlwaysWriteContentTransferEncoding
	{
		get
		{
			return avdyi(MimeOptions.AlwaysWriteContentTransferEncoding);
		}
		set
		{
			snndx(MimeOptions.AlwaysWriteContentTransferEncoding, value);
		}
	}

	public bool IgnoreUnparsableSignatures
	{
		get
		{
			return avdyi(MimeOptions.IgnoreUnparsableSignatures);
		}
		set
		{
			snndx(MimeOptions.IgnoreUnparsableSignatures, value);
		}
	}

	public bool DoNotQuoteProblematicSequences
	{
		get
		{
			return avdyi(MimeOptions.DoNotQuoteProblematicSequences);
		}
		set
		{
			snndx(MimeOptions.DoNotQuoteProblematicSequences, value);
		}
	}

	public bool OnlyParseHeaders
	{
		get
		{
			return avdyi(MimeOptions.OnlyParseHeaders);
		}
		set
		{
			snndx(MimeOptions.OnlyParseHeaders, value);
		}
	}

	public bool DoNotAddDateIfNoSubjectAndFrom
	{
		get
		{
			return avdyi(MimeOptions.DoNotAddDateIfNoSubjectAndFrom);
		}
		set
		{
			snndx(MimeOptions.DoNotAddDateIfNoSubjectAndFrom, value);
		}
	}

	public bool DisableEncryptionKeyPreference
	{
		get
		{
			return avdyi(MimeOptions.DisableEncryptionKeyPreference);
		}
		set
		{
			snndx(MimeOptions.DisableEncryptionKeyPreference, value);
		}
	}

	public bool DisableSMimeCapabilitiesAttribute
	{
		get
		{
			return avdyi(MimeOptions.DisableSMimeCapabilitiesAttribute);
		}
		set
		{
			snndx(MimeOptions.DisableSMimeCapabilitiesAttribute, value);
		}
	}

	public bool SkipCertificateUsageCheck
	{
		get
		{
			return avdyi(MimeOptions.SkipCertificateUsageCheck);
		}
		set
		{
			snndx(MimeOptions.SkipCertificateUsageCheck, value);
		}
	}

	public bool IgnoreInvalidTnefMessages
	{
		get
		{
			return avdyi(MimeOptions.IgnoreInvalidTnefMessages);
		}
		set
		{
			snndx(MimeOptions.IgnoreInvalidTnefMessages, value);
		}
	}

	public bool SkipSenderCheck
	{
		get
		{
			return avdyi(MimeOptions.SkipSenderCheck);
		}
		set
		{
			snndx(MimeOptions.SkipSenderCheck, value);
		}
	}

	public bool DisableSinglePartHtmlWorkaround
	{
		get
		{
			return avdyi(MimeOptions.DisableSinglePartHtmlWorkaround);
		}
		set
		{
			snndx(MimeOptions.DisableSinglePartHtmlWorkaround, value);
		}
	}

	public bool LoadMsgProperties
	{
		get
		{
			return avdyi(MimeOptions.LoadMsgProperties);
		}
		set
		{
			snndx(MimeOptions.LoadMsgProperties, value);
		}
	}

	public bool DoNotTrimHeaderValues
	{
		get
		{
			return avdyi(MimeOptions.DoNotTrimHeaderValues);
		}
		set
		{
			snndx(MimeOptions.DoNotTrimHeaderValues, value);
		}
	}

	internal bool racyr
	{
		get
		{
			if (avdyi(MimeOptions.DisableRtfToHtmlConversion) && 0 == 0)
			{
				return false;
			}
			if (RtfMode == RtfProcessingMode.TreatAsAttachment)
			{
				return false;
			}
			return true;
		}
	}

	public bool SkipTnefMessageProcessing
	{
		get
		{
			return avdyi(MimeOptions.SkipTnefMessageProcessing);
		}
		set
		{
			snndx(MimeOptions.SkipTnefMessageProcessing, value);
		}
	}

	internal bool pdktx
	{
		get
		{
			if (!avdyi((MimeOptions)2097152L) || 1 == 0)
			{
				return !PreferExplicitBody;
			}
			return true;
		}
	}

	public bool PreferExplicitBody
	{
		get
		{
			return ezgwz;
		}
		set
		{
			ezgwz = value;
		}
	}

	internal bool pjfau => avdyi((MimeOptions)8388608L);

	public RtfProcessingMode RtfMode
	{
		get
		{
			return ueulb;
		}
		set
		{
			switch (value)
			{
			case RtfProcessingMode.Default:
			case RtfProcessingMode.TreatAsAttachment:
			case RtfProcessingMode.Legacy:
				ueulb = value;
				break;
			default:
				throw hifyx.nztrs("value", value, "Invalid RtfProcessingMode. Possible values are 0-2.");
			}
		}
	}

	public bool IgnoreMsgTransportHeaders
	{
		get
		{
			return diltw;
		}
		set
		{
			diltw = value;
		}
	}

	public bool TreatMixedInlineAsAttachment
	{
		get
		{
			return dsgdf;
		}
		set
		{
			dsgdf = value;
		}
	}

	public bool AllowOversizedLines
	{
		get
		{
			return avdyi(MimeOptions.AllowOversizedLines);
		}
		set
		{
			snndx(MimeOptions.AllowOversizedLines, value);
		}
	}

	public MailSettings()
	{
		TreatMixedInlineAsAttachment = true;
	}

	internal MimeOptions hsrhh()
	{
		return jdimx;
	}

	internal void mbcjp(MimeOptions p0)
	{
		jdimx = p0;
	}

	private bool avdyi(MimeOptions p0)
	{
		return (jdimx & p0) == p0;
	}

	private void snndx(MimeOptions p0, bool p1)
	{
		if (p1 && 0 == 0)
		{
			jdimx |= p0;
		}
		else
		{
			jdimx &= ~p0;
		}
	}

	public MailSettings Clone()
	{
		MailSettings mailSettings = (MailSettings)MemberwiseClone();
		mailSettings.tfuau = 0;
		return mailSettings;
	}

	internal bool bxjim(bool p0)
	{
		if (RtfMode != RtfProcessingMode.Legacy)
		{
			return true;
		}
		if (!p0 || 1 == 0)
		{
			return false;
		}
		return !avdyi(MimeOptions.TreatBinaryRtfAsAlternateView);
	}
}
