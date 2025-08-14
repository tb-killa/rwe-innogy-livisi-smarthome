namespace onrkn;

internal class zppmb : qoqui
{
	private byte jxeyi;

	private byte hjvfi;

	public byte wzwvm => jxeyi;

	public byte jmwmm => hjvfi;

	public bool bgpwy
	{
		get
		{
			if (jxeyi != 1)
			{
				return true;
			}
			switch (hjvfi)
			{
			default:
				return true;
			case 0:
			case 42:
			case 43:
			case 44:
			case 45:
			case 46:
			case 51:
			case 90:
			case 100:
				return false;
			}
		}
	}

	public bool uqcgg => hjvfi == 0;

	public override int nimwj => 2;

	public override void gjile(byte[] p0, int p1)
	{
		p0[p1] = jxeyi;
		p0[p1 + 1] = hjvfi;
	}

	public zppmb(rtzwv level, mjddr description)
		: base(vcedo.nzoxg)
	{
		jxeyi = (byte)level;
		hjvfi = (byte)description;
	}

	public zppmb(byte level, byte description)
		: base(vcedo.nzoxg)
	{
		jxeyi = level;
		hjvfi = description;
	}

	public override string ToString()
	{
		rtzwv rtzwv2 = (rtzwv)jxeyi;
		string text = grlvu((mjddr)hjvfi);
		if (rtzwv2 == rtzwv.iddlf)
		{
			return "Alert:" + text;
		}
		return mznjh(rtzwv2) + " Alert:" + text;
	}

	public static string mznjh(rtzwv p0)
	{
		return p0 switch
		{
			rtzwv.iddlf => "Warning", 
			rtzwv.iogyt => "Fatal", 
			_ => "Unknown", 
		};
	}

	public static string grlvu(mjddr p0)
	{
		return p0 switch
		{
			mjddr.pbymk => "CloseNotify", 
			mjddr.ypibb => "UnexpectedMessage", 
			mjddr.mmnuq => "BadRecordMac", 
			mjddr.wdkjl => "DecryptionFailed", 
			mjddr.zgqpi => "RecordOverflow", 
			mjddr.kylem => "DecompressionFailure", 
			mjddr.jhrgr => "HandshakeFailure", 
			mjddr.frppg => "NoCertificate", 
			mjddr.fvtwt => "BadCertificate", 
			mjddr.zfebd => "UnsupportedCertificate", 
			mjddr.wskoy => "CertificateRevoked", 
			mjddr.cyvqp => "CertificateExpired", 
			mjddr.vyvjd => "CertificateUnknown", 
			mjddr.ziopd => "IllegalParameter", 
			mjddr.kxgat => "UnknownCa", 
			mjddr.yjvwc => "AccessDenied", 
			mjddr.gkkle => "DecodeError", 
			mjddr.wmgut => "DecryptError", 
			mjddr.nkvah => "ExportRestriction", 
			mjddr.puqjh => "ProtocolVersion", 
			mjddr.zwmax => "InsufficientSecurity", 
			mjddr.qssln => "InternalError", 
			mjddr.bapex => "UserCanceled", 
			mjddr.ophzj => "NoRenegotiation", 
			_ => "UnknownError", 
		};
	}
}
