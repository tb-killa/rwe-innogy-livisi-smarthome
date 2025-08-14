using System;

namespace Rebex.Mime;

[Flags]
public enum MimeOptions : long
{
	DoNotWriteBcc = 1L,
	DoNotParseMimeTree = 2L,
	ProcessAllHeaders = 4L,
	IgnoreUnparsableHeaders = 8L,
	AlwaysWriteContentTransferEncoding = 0x10L,
	IgnoreUnparsableSignatures = 0x20L,
	AllowAnyTextCharacters = 0x40L,
	DoNotQuoteProblematicSequences = 0x80L,
	OnlyParseHeaders = 0x100L,
	DoNotAddDateIfNoSubjectAndFrom = 0x200L,
	DisableEncryptionKeyPreference = 0x400L,
	DisableSMimeCapabilitiesAttribute = 0x800L,
	SkipCertificateUsageCheck = 0x1000L,
	IgnoreInvalidTnefMessages = 0x2000L,
	SkipSenderCheck = 0x4000L,
	KeepRawEntityBody = 0x8000L,
	DoNotCloseStreamAfterLoad = 0x10000L,
	DoNotPreloadAttachments = 0x20000L,
	DisableSinglePartHtmlWorkaround = 0x40000L,
	LoadMsgProperties = 0x100000L,
	DoNotTrimHeaderValues = 0x1000000L,
	DisableRtfToHtmlConversion = 0x2000000L,
	TreatBinaryRtfAsAlternateView = 0x4000000L,
	SkipTnefMessageProcessing = 0x8000000L,
	AllowOversizedLines = 0x10000000L
}
